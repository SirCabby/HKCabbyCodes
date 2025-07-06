using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using BepInEx.Configuration;
using System.Linq;
using GlobalEnums;
using System.Collections;

namespace CabbyCodes.Patches.Teleport
{
    /// <summary>
    /// Handles teleportation functionality and custom teleport location management.
    /// </summary>
    public class TeleportPatch : ISyncedValueList
    {
        /// <summary>
        /// Configuration key for storing teleport locations.
        /// </summary>
        public const string key = "TeleportLocations";

        /// <summary>
        /// List of saved custom teleport locations.
        /// </summary>
        public static readonly List<TeleportLocation> savedTeleports = InitTeleportLocations();

        /// <summary>
        /// Field info for accessing the hero field in GameMap.
        /// </summary>
        private static readonly FieldInfo heroFieldInfo = typeof(GameMap).GetField("hero", BindingFlags.NonPublic | BindingFlags.Instance);

        /// <summary>
        /// List of predefined teleport locations.
        /// </summary>
        private static readonly List<TeleportLocation> teleportLocations = new List<TeleportLocation>
        {
            new TeleportLocation("", "<Select Location>", Vector2.zero),
            new TeleportLocation("Town", "Starting Town", new Vector2(Constants.TOWN_X_POSITION, Constants.TOWN_Y_POSITION)),
        };

        /// <summary>
        /// Flag to prevent multiple teleport attempts while one is in progress.
        /// </summary>
        private static bool teleportInProgress = false;

        /// <summary>
        /// Gets the current teleport selection index.
        /// </summary>
        /// <returns>Always returns 0 as this is used for the dropdown.</returns>
        public int Get()
        {
            return 0;
        }

        /// <summary>
        /// Sets the teleport destination based on the selected index.
        /// </summary>
        /// <param name="value">The index of the teleport location to use.</param>
        public void Set(int value)
        {
            if (value < 1 || value > teleportLocations.Count) return;

            DoTeleport(teleportLocations[value]);
        }

        /// <summary>
        /// Gets the list of teleport location display names for the dropdown.
        /// </summary>
        /// <returns>A list of display names for available teleport locations.</returns>
        public List<string> GetValueList()
        {
            List<string> result = new List<string>();
            foreach (TeleportLocation loc in teleportLocations)
            {
                result.Add(loc.DisplayName);
            }
            return result;
        }

        /// <summary>
        /// Initializes teleport locations from configuration.
        /// </summary>
        /// <returns>A list of custom teleport locations loaded from configuration.</returns>
        private static List<TeleportLocation> InitTeleportLocations()
        {
            List<TeleportLocation> result = new List<TeleportLocation>();

            try
            {
                // Try to get a list of saved teleport location names from a special config entry
                var savedLocationsEntry = CabbyCodesPlugin.configFile.Bind(key, "SavedLocations", "",
                    new ConfigDescription("List of saved teleport location names"));

                if (!string.IsNullOrEmpty(savedLocationsEntry.Value))
                {
                    string[] locationNames = savedLocationsEntry.Value.Split(',');

                    foreach (string locationName in locationNames)
                    {
                        if (!string.IsNullOrEmpty(locationName))
                        {
                            // Try to get the location data for this name
                            var locationEntry = CabbyCodesPlugin.configFile.Bind(key, locationName, "",
                                new ConfigDescription($"Teleport location data for {locationName}"));

                            if (!string.IsNullOrEmpty(locationEntry.Value))
                            {
                                ConfigDefinition configDef = new ConfigDefinition(key, locationName);
                                result.Add(new CustomTeleportLocation(configDef, locationEntry));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CabbyCodesPlugin.BLogger.LogWarning(string.Format("Failed to load teleport locations from config: {0}", ex.Message));
            }

            CabbyCodesPlugin.BLogger.LogDebug(string.Format("Loaded {0} custom teleport locations", result.Count));
            return result;
        }

        /// <summary>
        /// Performs the actual teleportation to the specified location.
        /// </summary>
        /// <param name="teleportLocation">The location to teleport to.</param>
        public static void DoTeleport(TeleportLocation teleportLocation)
        {
            if (teleportInProgress)
            {
                CabbyCodesPlugin.BLogger.LogWarning("Teleport already in progress, ignoring request");
                return;
            }

            CabbyCodesPlugin.BLogger.LogInfo(string.Format("Teleporting to [{0}] ({1}, {2})", teleportLocation.SceneName, teleportLocation.Location.x, teleportLocation.Location.y));

            GameManager gm = GameManager._instance;
            if (gm == null)
            {
                CabbyCodesPlugin.BLogger.LogError("GameManager is null, cannot teleport");
                return;
            }

            // Unpause the game by setting all pause state fields before unpausing the hero
            var hero = gm.hero_ctrl;
            if (hero != null)
            {
                // Set all pause state fields to unpaused state
                gm.isPaused = false;
                Time.timeScale = 1f;
                TimeController.GenericTimeScale = 1f;
                
                // Restore input handling
                gm.inputHandler.StartAcceptingInput();
                gm.inputHandler.AllowPause();
                
                // Set game state to playing
                gm.SetState(GameState.PLAYING);
                
                // Update UI state to match game state
                gm.ui?.SetState(UIState.PLAYING);
                
                // Now unpause the hero
                hero.UnPause();
            }

            // Store the target location for use after the scene loads
            _pendingTeleportLocation = teleportLocation;
            teleportInProgress = true;

            // Start the coroutine that waits for the hero to be ready before teleporting
            gm.StartCoroutine(WaitForHeroReadyAndTeleport(teleportLocation));
        }

        /// <summary>
        /// Coroutine that waits for the hero to be fully initialized before starting the teleport.
        /// </summary>
        /// <param name="teleportLocation">The location to teleport to.</param>
        private static IEnumerator WaitForHeroReadyAndTeleport(TeleportLocation teleportLocation)
        {
            GameManager gm = GameManager._instance;
            if (gm == null)
            {
                teleportInProgress = false;
                yield break;
            }

            // Wait for the hero to be available
            HeroController hero = null;
            while (hero == null)
            {
                hero = gm.hero_ctrl;
                if (hero == null)
                {
                    yield return null;
                }
            }

            // Wait for the hero to be fully initialized and ready
            while (true)
            {
                // Check if the hero is ready for teleportation
                bool heroReady = IsHeroReadyForTeleport(hero, gm);
                if (heroReady)
                {
                    CabbyCodesPlugin.BLogger.LogInfo("Hero is ready for teleportation, proceeding");
                    break;
                }
                yield return null;
            }

            // Subscribe to the event that fires after the hero is spawned in the new scene
            gm.OnFinishedEnteringScene += OnFinishedEnteringSceneTeleport;

            // Start the scene transition
            gm.BeginSceneTransition(new GameManager.SceneLoadInfo
            {
                AlwaysUnloadUnusedAssets = true,
                EntryGateName = "dreamGate", // Use a valid entry gate or empty if not needed
                PreventCameraFadeOut = true,
                SceneName = teleportLocation.SceneName,
                Visualization = GameManager.SceneLoadVisualizations.Dream
            });
        }

        /// <summary>
        /// Checks if the hero is ready for teleportation by examining various state conditions.
        /// </summary>
        /// <param name="hero">The hero controller to check.</param>
        /// <param name="gm">The game manager instance.</param>
        /// <returns>True if the hero is ready for teleportation, false otherwise.</returns>
        private static bool IsHeroReadyForTeleport(HeroController hero, GameManager gm)
        {
            if (hero == null || gm == null)
            {
                CabbyCodesPlugin.BLogger.LogDebug("[Teleport] Not ready: hero or gm is null");
                return false;
            }

            // Check if the game is in a playable state
            bool isGamePlaying = gm.gameState == GameState.PLAYING;
            // Check if the hero is accepting input (indicates it's fully initialized)
            bool acceptingInput = gm.inputHandler.acceptingInput;
            // Check if the hero is in a valid state
            var heroState = hero.hero_state;
            // Check if the hero is on the ground (if available)
            bool onGround = false;
            try { onGround = hero.cState.onGround; } catch { }
            CabbyCodesPlugin.BLogger.LogDebug(string.Format("[Teleport] Checking readiness: isGamePlaying={0}, acceptingInput={1}, heroState={2}, onGround={3}", isGamePlaying, acceptingInput, heroState, onGround));

            if (!isGamePlaying) return false;
            if (!acceptingInput) return false;
            if (heroState == ActorStates.no_input || heroState == ActorStates.airborne) return false;
            if (!onGround) return false;
            if (hero.transform == null) return false;

            return true;
        }

        // Store the pending teleport location
        private static TeleportLocation _pendingTeleportLocation;

        // Event handler to set the hero's position after entering the scene
        private static void OnFinishedEnteringSceneTeleport()
        {
            var gm = GameManager._instance;
            var hero = gm.hero_ctrl;
            
            if (_pendingTeleportLocation != null && hero != null)
            {
                Vector3 newPos = new Vector3(_pendingTeleportLocation.Location.x, _pendingTeleportLocation.Location.y, hero.transform.position.z);
                hero.transform.position = newPos;
                gm.cameraCtrl.SnapTo(newPos.x, newPos.y);
                
                // Restore the game state to fully playable
                gm.SetState(GameState.PLAYING);
                gm.inputHandler.StartAcceptingInput();
                gm.inputHandler.AllowPause();
                
                // Actually unpause the game
                gm.isPaused = false;
                Time.timeScale = 1f;
                TimeController.GenericTimeScale = 1f;
                
                // Transition audio snapshot
                gm.actorSnapshotUnpaused?.TransitionTo(0f);
                
                // Update UI state to match game state
                gm.ui?.SetState(UIState.PLAYING);
            }
            // Unsubscribe to avoid memory leaks
            gm.OnFinishedEnteringScene -= OnFinishedEnteringSceneTeleport;
            _pendingTeleportLocation = null;
            teleportInProgress = false;
        }

        /// <summary>
        /// Saves the current player position as a custom teleport location.
        /// </summary>
        public static void SaveTeleportLocation()
        {
            GameMap gm = GameManager._instance.gameMap.GetComponent<GameMap>();
            Vector3 heroPos = ((GameObject)heroFieldInfo.GetValue(gm)).transform.position;
            string sceneName = GameManager.GetBaseSceneName(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            Vector2 teleportLocation = new Vector2((int)Math.Round(heroPos.x), (int)Math.Ceiling(heroPos.y));

            List<TeleportLocation> teleportLocations = savedTeleports;
            bool locationFound = false;
            for (int i = 0; i < teleportLocations.Count; i++)
            {
                if (teleportLocations[i].SceneName == sceneName)
                {
                    teleportLocations[i].Location = teleportLocation;
                    locationFound = true;
                    break;
                }
            }

            if (!locationFound)
            {
                // Create the config entry directly using BepInEx
                string locationValue = teleportLocation.x + "," + teleportLocation.y;
                ConfigEntry<string> configEntry = CabbyCodesPlugin.configFile.Bind(key, sceneName, locationValue,
                    new ConfigDescription($"Custom teleport location for {sceneName}"));

                if (configEntry != null)
                {
                    ConfigDefinition configDef = new ConfigDefinition(key, sceneName);
                    CustomTeleportLocation loc = new CustomTeleportLocation(configDef, configEntry);
                    teleportLocations.Add(loc);
                    AddCustomPanel(loc);

                    // Update the list of saved locations
                    UpdateSavedLocationsList();

                    CabbyCodesPlugin.BLogger.LogDebug(string.Format("Saved new teleport location: {0} at ({1}, {2})", sceneName, teleportLocation.x, teleportLocation.y));
                }
                else
                {
                    CabbyCodesPlugin.BLogger.LogWarning(string.Format("Failed to create config entry for teleport location: {0}", sceneName));
                }
            }
        }

        /// <summary>
        /// Updates the list of saved teleport location names in the config file.
        /// </summary>
        private static void UpdateSavedLocationsList()
        {
            try
            {
                var savedLocationsEntry = CabbyCodesPlugin.configFile.Bind(key, "SavedLocations", "",
                    new ConfigDescription("List of saved teleport location names"));

                var locationNames = savedTeleports
                    .Where(loc => loc is CustomTeleportLocation)
                    .Select(loc => loc.SceneName)
                    .ToArray();

                savedLocationsEntry.Value = string.Join(",", locationNames);
                CabbyCodesPlugin.configFile.Save();
            }
            catch (Exception ex)
            {
                CabbyCodesPlugin.BLogger.LogWarning(string.Format("Failed to update saved locations list: {0}", ex.Message));
            }
        }

        /// <summary>
        /// Adds the main teleport dropdown panel to the menu.
        /// </summary>
        private static void AddPanel()
        {
                            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new DropdownPanel(new TeleportPatch(), "Select Area to Teleport", Constants.DEFAULT_PANEL_HEIGHT));
        }

        /// <summary>
        /// Adds the save teleport location button panel to the menu.
        /// </summary>
        private static void AddSavePanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new ButtonPanel(SaveTeleportLocation, "Save", "Save a custom teleport at current position"));
        }

        /// <summary>
        /// Adds a custom teleport location panel with delete functionality.
        /// </summary>
        /// <param name="location">The teleport location to create a panel for.</param>
        private static void AddCustomPanel(TeleportLocation location)
        {
            ButtonPanel buttonPanel = new ButtonPanel(() => { DoTeleport(location); }, "Teleport", location.DisplayName);

            GameObject destroyButton = PanelAdder.AddDestroyPanelButton(buttonPanel, buttonPanel.cheatPanel.transform.childCount, () =>
            {
                savedTeleports.Remove(location);
                if (location is CustomTeleportLocation customLocation)
                {
                    // Remove the config entry directly
                    try
                    {
                        CabbyCodesPlugin.configFile.Remove(customLocation.configDef);
                        CabbyCodesPlugin.configFile.Save();

                        // Update the saved locations list
                        UpdateSavedLocationsList();

                        CabbyCodesPlugin.BLogger.LogDebug(string.Format("Removed teleport location: {0}", customLocation.SceneName));
                    }
                    catch (Exception ex)
                    {
                        CabbyCodesPlugin.BLogger.LogWarning(string.Format("Failed to remove teleport location from config: {0} - {1}", customLocation.SceneName, ex.Message));
                    }
                }
            }, "X", new Vector2(Constants.TELEPORT_DESTROY_BUTTON_SIZE, Constants.TELEPORT_DESTROY_BUTTON_SIZE));

            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(buttonPanel);
        }

        /// <summary>
        /// Adds all teleport-related panels to the mod menu.
        /// </summary>
        public static void AddPanels()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Teleportation: Select common point of interest to travel to").SetColor(CheatPanel.headerColor));
            AddPanel();
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Lloyd's Beacon: Save and recall custom teleportation locations").SetColor(CheatPanel.headerColor));
            AddSavePanel();
            foreach (TeleportLocation location in savedTeleports)
            {
                AddCustomPanel(location);
            }
        }
    }
} 