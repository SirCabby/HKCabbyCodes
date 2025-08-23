using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using System;
using System.Collections.Generic;
using UnityEngine;
using BepInEx.Configuration;
using System.Linq;
using CabbyCodes.Scenes;

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
        /// Synced reference for the custom teleport name input field.
        /// </summary>
        private static readonly BoxedReference<string> customTeleportNameRef = new BoxedReference<string>("");

        /// <summary>
        /// Reference to the custom name input field panel for forcing updates.
        /// </summary>
        private static InputFieldPanel<string> customNameInputPanel;

        /// <summary>
        /// List of predefined teleport locations.
        /// </summary>
        private static readonly List<TeleportLocation> teleportLocations = new List<TeleportLocation>
        {
            new TeleportLocation(new SceneMapData(""), "<Select Location>", Vector2.zero),
            new TeleportLocation(SceneInstances.Town, SceneInstances.Town.ReadableName, new Vector2(136, 12)),
            new TeleportLocation(SceneInstances.Crossroads_02, SceneInstances.Room_temple.ReadableName, new Vector2(41, 5)),
            new TeleportLocation(SceneInstances.Crossroads_38, "The Grubfather", new Vector2(64, 4)),
            new TeleportLocation(SceneInstances.RestingGrounds_07, "The Seer", new Vector2(30, 10)),
            new TeleportLocation(SceneInstances.Fungus1_08, "The Hunter", new Vector2(43, 39)),
            new TeleportLocation(SceneInstances.Crossroads_04, "Salubra's Charm Shop", new Vector2(141, 12)),
            new TeleportLocation(SceneInstances.Fungus2_26, "Leg Eater's Shop", new Vector2(35, 6)),
            new TeleportLocation(SceneInstances.Room_nailmaster, "Nailmaster Mato", new Vector2(57, 5)),
            new TeleportLocation(SceneInstances.Abyss_05, "Nailmaster Oro", new Vector2(124, 18)),
            new TeleportLocation(SceneInstances.Room_nailmaster_02, "Nailmaster Sheo", new Vector2(26, 5)),
            new TeleportLocation(SceneInstances.Room_Mask_Maker, "Mask Maker", new Vector2(26, 7)),
            new TeleportLocation(SceneInstances.Ruins1_04, "Nailsmith", new Vector2(34, 37)),
            new TeleportLocation(SceneInstances.Room_Mansion, "Grey Mourner", new Vector2(16, 7)),
            new TeleportLocation(SceneInstances.Abyss_05, "White Castle", new Vector2(124, 18)),
            new TeleportLocation(SceneInstances.Waterways_03, "Tuk", new Vector2(89, 5)),
            new TeleportLocation(SceneInstances.GG_Waterways, "Godfinder", new Vector2(50, 16)),
        };

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

            return result;
        }

        /// <summary>
        /// Performs the actual teleportation to the specified location.
        /// </summary>
        /// <param name="teleportLocation">The location to teleport to.</param>
        public static void DoTeleport(TeleportLocation teleportLocation)
        {
            TeleportService.DoTeleport(teleportLocation);
        }



        /// <summary>
        /// Saves the current player position as a custom teleport location.
        /// </summary>
        public static void SaveTeleportLocation()
        {
            var (sceneName, teleportLocation) = TeleportService.GetCurrentPlayerPosition();

            // Get the custom name if provided, otherwise use the scene name
            string customName = (customTeleportNameRef.Get() ?? "").Trim();
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
                        // Ask for confirmation before overwrite
                        var menu = CabbyCodesPlugin.cabbyMenu;
                        Action doOverwrite = () => { ctl.Location = teleportLocation; };
                        if (menu != null)
                        {
                            string msg = string.Format("Overwrite teleport '{0}' with current position?", displayName);
                            var popup = new CabbyMenu.UI.Popups.ConfirmationPopup(menu, "Overwrite Teleport?", msg, "Overwrite", "Cancel", doOverwrite, null);
                            popup.Show();
                        }
                        else
                        {
                            doOverwrite();
                        }
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
            ButtonPanel buttonPanel = new ButtonPanel(() =>
            {
                var menu = CabbyCodesPlugin.cabbyMenu;
                if (menu == null) { DoTeleport(location); return; }
                string msg = string.Format("Teleport to '{0}'?", location.DisplayName);
                var popup = new CabbyMenu.UI.Popups.ConfirmationPopup(
                    menu,
                    "Are you sure?",
                    msg,
                    "Teleport",
                    "Cancel",
                    () => { DoTeleport(location); },
                    null);
                popup.Show();
            }, "Teleport", location.DisplayName);

            // Add the panel to the menu first so it has a parent
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(buttonPanel);

            // Create the X button after the panel has been added to the menu
            PanelAdder.AddDestroyButtonToPanel(buttonPanel, () =>
            {
                savedTeleports.Remove(location);
                if (location is CustomTeleportLocation customLocation)
                {
                    try
                    {
                        CabbyCodesPlugin.configFile.Remove(customLocation.configDef);
                        CabbyCodesPlugin.configFile.Save();
                        UpdateSavedLocationsList();
                        CabbyCodesPlugin.BLogger.LogDebug(string.Format("Removed teleport location: {0}", customLocation.Scene.SceneName));
                    }
                    catch (Exception ex)
                    {
                        CabbyCodesPlugin.BLogger.LogWarning(string.Format("Failed to remove teleport location from config: {0} - {1}", customLocation.Scene.SceneName, ex.Message));
                    }
                }
            }, "X", 60f, (confirm, cancel) =>
            {
                var menu = CabbyCodesPlugin.cabbyMenu;
                if (menu == null) { confirm(); return; }
                string msg = string.Format("Delete teleport '{0}'?\n\nThis action cannot be undone.", location.DisplayName);
                var popup = new CabbyMenu.UI.Popups.ConfirmationPopup(menu, "Are you sure?", msg, "Delete", "Keep", confirm, cancel);
                popup.Show();
            });

            // Add inline Save button to overwrite this teleport location
            PanelAdder.AddButton(buttonPanel, 1, () =>
            {
                // Confirm overwrite
                var menu = CabbyCodesPlugin.cabbyMenu;
                Action performSave = () =>
                {
                    Vector2 newLoc = TeleportService.GetCurrentPlayerPosition().position;
                    if (location is CustomTeleportLocation ctl)
                    {
                        ctl.Location = newLoc;
                        CabbyCodesPlugin.BLogger.LogDebug(string.Format("Overwrote teleport location {0} to new coords {1}", location.DisplayName, newLoc));
                    }
                };

                if (menu != null)
                {
                    string msg = string.Format("Overwrite teleport '{0}' with current position?", location.DisplayName);
                    var popup = new CabbyMenu.UI.Popups.ConfirmationPopup(menu, "Overwrite Teleport?", msg, "Overwrite", "Cancel", performSave, null);
                    popup.Show();
                }
                else
                {
                    performSave();
                }

            }, "Save", new Vector2(CabbyMenu.Constants.MIN_PANEL_WIDTH, CabbyMenu.Constants.DEFAULT_PANEL_HEIGHT));
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
} 