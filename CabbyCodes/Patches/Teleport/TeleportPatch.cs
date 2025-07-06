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
        /// Synced reference for the custom teleport name input field.
        /// </summary>
        private static readonly CustomTeleportNameReference customTeleportNameRef = new CustomTeleportNameReference();

        /// <summary>
        /// Reference to the custom name input field panel for forcing updates.
        /// </summary>
        private static InputFieldPanel<string> customNameInputPanel;

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
                // Try to get a list of saved teleport location keys from a special config entry
                var savedLocationsEntry = CabbyCodesPlugin.configFile.Bind(key, "SavedLocations", "",
                    new ConfigDescription("List of saved teleport location keys"));

                if (!string.IsNullOrEmpty(savedLocationsEntry.Value))
                {
                    string[] locationKeys = savedLocationsEntry.Value.Split(',');

                    foreach (string locationKey in locationKeys)
                    {
                        if (!string.IsNullOrEmpty(locationKey))
                        {
                            // Try to get the location data for this key
                            var locationEntry = CabbyCodesPlugin.configFile.Bind(key, locationKey, "",
                                new ConfigDescription($"Teleport location data for {locationKey}"));

                            if (!string.IsNullOrEmpty(locationEntry.Value))
                            {
                                // Validate the format before creating the teleport location
                                var parts = locationEntry.Value.Split('|');
                                if (parts.Length == 3)
                                {
                                    ConfigDefinition configDef = new ConfigDefinition(key, locationKey);
                                    result.Add(new CustomTeleportLocation(configDef, locationEntry));
                                }
                                else
                                {
                                    // Log error and skip this teleport
                                    CabbyCodesPlugin.BLogger.LogWarning(string.Format("Invalid teleport location format for '{0}'. Expected 'sceneName|x|y', got '{1}'. Skipping this teleport.", locationKey, locationEntry.Value));
                                }
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

            // Get the custom name if provided, otherwise use the scene name
            string customName = customTeleportNameRef.Get().Trim();
            string displayName = !string.IsNullOrEmpty(customName) ? customName : sceneName;

            List<TeleportLocation> teleportLocations = savedTeleports;
            bool locationFound = false;
            for (int i = 0; i < teleportLocations.Count; i++)
            {
                if (teleportLocations[i] is CustomTeleportLocation ctl)
                {
                    // Match on display name
                    if (ctl.DisplayName == displayName)
                    {
                        ctl.Location = teleportLocation;
                        locationFound = true;
                        break;
                    }
                }
            }

            if (!locationFound)
            {
                // Store as sceneName|x|y
                string locationValue = $"{sceneName}|{teleportLocation.x}|{teleportLocation.y}";
                ConfigEntry<string> configEntry = CabbyCodesPlugin.configFile.Bind(key, displayName, "",
                    new ConfigDescription($"Custom teleport location for {displayName}"));

                if (configEntry != null)
                {
                    // Set the value before creating the CustomTeleportLocation
                    configEntry.Value = locationValue;
                    
                    ConfigDefinition configDef = new ConfigDefinition(key, displayName);
                    CustomTeleportLocation loc = new CustomTeleportLocation(configDef, configEntry);
                    teleportLocations.Add(loc);
                    AddCustomPanel(loc);

                    // Update the list of saved locations
                    UpdateSavedLocationsList();

                    // Clear the custom name input field after saving
                    customTeleportNameRef.Set("");
                    
                    // Force an update to ensure the UI reflects the cleared value
                    customNameInputPanel?.ForceUpdate();

                    CabbyCodesPlugin.BLogger.LogDebug(string.Format("Saved new teleport location: {0} ({1}) at ({2}, {3})", displayName, sceneName, teleportLocation.x, teleportLocation.y));
                }
                else
                {
                    CabbyCodesPlugin.BLogger.LogWarning(string.Format("Failed to create config entry for teleport location: {0} ({1})", displayName, sceneName));
                }
            }
        }

        /// <summary>
        /// Updates the list of saved teleport location keys in the config file.
        /// </summary>
        private static void UpdateSavedLocationsList()
        {
            try
            {
                var savedLocationsEntry = CabbyCodesPlugin.configFile.Bind(key, "SavedLocations", "",
                    new ConfigDescription("List of saved teleport location keys"));

                var locationKeys = savedTeleports
                    .Where(loc => loc is CustomTeleportLocation ctl)
                    .Select(loc => ((CustomTeleportLocation)loc).configDef.Key)
                    .ToArray();

                savedLocationsEntry.Value = string.Join(",", locationKeys);
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
        /// Adds the custom teleport name input field panel to the menu.
        /// </summary>
        private static void AddCustomNamePanel()
        {
            // Calculate width for 35 characters but allow 50 characters
            int widthFor35Chars = CalculatePanelWidth(35);
            customNameInputPanel = new InputFieldPanel<string>(customTeleportNameRef, CabbyMenu.Utilities.KeyCodeMap.ValidChars.AlphaNumeric, 60, widthFor35Chars, "(Optional) Custom teleport name");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(customNameInputPanel);
        }

        /// <summary>
        /// Calculates the panel width based on character limit, matching the logic from InputFieldPanel.
        /// </summary>
        /// <param name="characterLimit">The maximum number of characters allowed.</param>
        /// <returns>The calculated width in pixels.</returns>
        private static int CalculatePanelWidth(int characterLimit)
        {
            // Use the cursor character width for panel sizing to match visible character logic
            float estimatedCharWidth = CalculateCursorCharacterWidth(CabbyMenu.Constants.DEFAULT_FONT_SIZE);
            float uiBuffer = 10f;
            float calculatedWidth = (characterLimit * estimatedCharWidth) + uiBuffer;
            return Mathf.Max(CabbyMenu.Constants.MIN_PANEL_WIDTH, Mathf.RoundToInt(calculatedWidth));
        }

        /// <summary>
        /// Calculates the character width for cursor positioning calculations.
        /// </summary>
        /// <param name="fontSize">The font size in pixels.</param>
        /// <returns>The estimated character width for cursor positioning.</returns>
        private static float CalculateCursorCharacterWidth(int fontSize)
        {
            return fontSize * 0.65f;
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
            AddCustomNamePanel();
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Custom Teleport Locations").SetColor(CheatPanel.subHeaderColor));
            foreach (TeleportLocation location in savedTeleports)
            {
                AddCustomPanel(location);
            }
        }
    }

    /// <summary>
    /// Simple string reference for custom teleport names.
    /// </summary>
    public class CustomTeleportNameReference : ISyncedReference<string>
    {
        private string value = "";

        public string Get()
        {
            return value;
        }

        public void Set(string value)
        {
            this.value = value ?? "";
        }
    }
} 