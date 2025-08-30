using CabbyMenu.UI.CheatPanels;
using CabbyMenu.UI.DynamicPanels;
using CabbyMenu.UI.Controls;
using CabbyMenu.UI.Popups;
using CabbyMenu.SyncedReferences;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using CabbyCodes.Patches.Settings;
using BepInEx.Configuration;

namespace CabbyCodes.Patches.Flags.RoomFlags
{
    public class RoomFlagsPatch
    {
        // Configuration entry for show all flags state
        private static ConfigEntry<bool> showAllFlagsConfig;
        
        /// <summary>
        /// Initializes the configuration entry.
        /// </summary>
        private static void InitializeConfig()
        {
            if (showAllFlagsConfig == null)
            {
                showAllFlagsConfig = CabbyCodesPlugin.configFile.Bind("RoomFlags", "ShowAllFlags", false, 
                    "Show all room flags including unused ones");
            }
        }
        
        public static List<CheatPanel> CreatePanels()
        {
            // Initialize configuration
            InitializeConfig();
            
            var panels = new List<CheatPanel>();

            // Determine the default area selection based on the player's current position
            int currentAreaIndex = GetCurrentAreaIndex();

            // Dropdown bound to an AreaSelector reference
            var areaSelector = new AreaSelector(currentAreaIndex, false); // Start with filtered flags
            var dropdownPanel = new DropdownPanel(areaSelector, "Select Area", Constants.DEFAULT_PANEL_HEIGHT);

            // Container proxying to the main menu
            var container = new MainMenuPanelContainer(CabbyCodesPlugin.cabbyMenu);

            // Create a BoxedReference to store the actual toggle state, initialized from config
            InitializeConfig(); // Ensure config is initialized
            var toggleState = new BoxedReference<bool>(showAllFlagsConfig.Value);

            // Create a DelegateReference that will be updated after panelManager is created
            DelegateReference<bool> showAllFlagsReference = null;

            // Manager responsible for creating and replacing room flag panels when the dropdown changes
            var panelManager = new DynamicPanelManager(
                dropdownPanel,
                (areaIndex) => {
                    // Always get the current value of toggleState when creating panels
                    bool currentShowAllFlags = toggleState.Get();
                    return CreateRoomFlagsPanels(areaIndex, currentShowAllFlags);
                },
                container,
                insertionIndex: 2,  // insert after both the toggle and dropdown
                parentManager: null,
                onPanelsChanged: null,
                menu: CabbyCodesPlugin.cabbyMenu
            );

            // Now create the DelegateReference with the change handler
            showAllFlagsReference = new DelegateReference<bool>(
                () => toggleState.Get(), // Return the current stored state
                (showAll) => {
                    try
                    {
                        // Save to config immediately
                        showAllFlagsConfig.Value = showAll;
                        
                        // Show loading popup immediately
                        var loadingPopup = new PopupBase(CabbyCodesPlugin.cabbyMenu, "Loading", "Loading . . .", 400f, 200f);
                        loadingPopup.SetPanelBackgroundColor(new Color(0.2f, 0.4f, 0.8f, 1f)); // Blue background
                        loadingPopup.SetMessageBold(); // Make message text bold
                        loadingPopup.Show();
                        Canvas.ForceUpdateCanvases();
                        
                        // Defer panel operations to the next frame to ensure loading popup renders first
                        CabbyMenu.Utilities.CoroutineRunner.RunNextFrame(() => {
                            try
                            {
                                // Store the new state
                                toggleState.Set(showAll);
                                
                                // Update the area selector with the new flag list
                                areaSelector.UpdateFlagList(showAll);
                                
                                // Since both filtered and unfiltered modes have the same area list, 
                                // we don't need to maintain area selection - it will stay the same
                                
                                // Force the dropdown to update its value list and current selection
                                var dropdownSync = dropdownPanel.GetDropDownSync();
                                if (dropdownSync != null)
                                {
                                    // Update the dropdown's value list to reflect the new area names
                                    var newAreaNames = areaSelector.GetValueList();
                                    dropdownSync.GetCustomDropdown().SetOptions(newAreaNames);
                                    // Set the dropdown to the current area selector value
                                    dropdownSync.GetCustomDropdown().SetValue(areaSelector.Get());
                                }
                                
                                // Rebuild the panels to show the updated flag list
                                
                                // Get the current area index AFTER all updates are complete
                                int finalAreaIndex = areaSelector.Get();
                                
                                // Force panel recreation by directly calling the panel factory and updating the container
                                var newPanels = CreateRoomFlagsPanels(finalAreaIndex, showAll);
                                
                                // Get all current panels and find the toggle and dropdown panels to preserve them
                                var allPanels = container.GetAllPanels();
                                var foundTogglePanel = allPanels.FirstOrDefault(p => p.GetDescription().Contains("Show ALL flags"));
                                var foundDropdownPanel = allPanels.FirstOrDefault(p => p.GetDescription().Contains("Select Area"));
                                
                                if (foundDropdownPanel != null)
                                {
                                    // Find the index of the dropdown panel (always exists)
                                    int dropdownIndex = container.GetPanelIndex(foundDropdownPanel);
                                    
                                    // Determine what to preserve based on whether dev options are enabled
                                    int preserveCount;
                                    if (foundTogglePanel != null && CabbyCodesPlugin.configFile.Bind("Settings", "EnableDevOptions", Constants.DEFAULT_DEV_OPTIONS_ENABLED, 
                                        "Enable developer options and advanced features").Value)
                                    {
                                        // Dev options are on - preserve both toggle and dropdown
                                        int toggleIndex = container.GetPanelIndex(foundTogglePanel);
                                        preserveCount = Math.Max(toggleIndex, dropdownIndex) + 1;
                                    }
                                    else
                                    {
                                        // Dev options are off - preserve only dropdown
                                        preserveCount = dropdownIndex + 1;
                                    }
                                    
                                    // Remove all panels after the preserved panels
                                    int dynamicPanelStartIndex = preserveCount;
                                    int dynamicPanelCount = allPanels.Count - preserveCount;
                                    
                                    if (dynamicPanelCount > 0)
                                    {
                                        // Remove only the dynamic room flag panels (starting after preserved panels)
                                        container.DetachPanelsAtRange(startIndex: dynamicPanelStartIndex, count: dynamicPanelCount);
                                    }
                                    
                                    // Insert the new dynamic panels after the preserved panels
                                    container.ReattachPanelsAtRange(newPanels, index: dynamicPanelStartIndex);
                                }
                                else
                                {
                                    // Could not find dropdown panel - this shouldn't happen
                                }
                                
                                // Panel rebuild complete - hide and destroy loading popup in the next frame
                                CabbyMenu.Utilities.CoroutineRunner.RunNextFrame(() => {
                                    if (loadingPopup != null)
                                    {
                                        loadingPopup.Hide();
                                        loadingPopup.Destroy();
                                    }
                                });
                            }
                            catch
                            {
                                // Handle any errors silently
                            }
                        });
                    }
                    catch
                    {
                        // Handle any errors silently
                    }
                }
            );

            var showAllFlagsPanel = new TogglePanel(
                showAllFlagsReference,
                "Show ALL flags? (includes some unknown and many very useless ones)"
            );
            
            // Only show the "show all flags" toggle when dev options are enabled
            if (CabbyCodesPlugin.configFile.Bind("Settings", "EnableDevOptions", Constants.DEFAULT_DEV_OPTIONS_ENABLED, 
                "Enable developer options and advanced features").Value)
            {
                // Add panels in the correct order: toggle first, then dropdown
                panels.Add(showAllFlagsPanel);
            }
            panels.Add(dropdownPanel);

            // Selection change listener will rebuild panels; no per-frame update needed
            dropdownPanel.GetDropDownSync().GetCustomDropdown().onValueChanged.AddListener((areaIndex) => {
                
                // Show loading popup immediately
                var loadingPopup = new PopupBase(CabbyCodesPlugin.cabbyMenu, "Loading", "Loading . . .", 400f, 200f);
                loadingPopup.SetPanelBackgroundColor(new Color(0.2f, 0.4f, 0.8f, 1f)); // Blue background
                loadingPopup.SetMessageBold(); // Make message text bold
                loadingPopup.Show();
                Canvas.ForceUpdateCanvases();
                
                // Defer panel operations to the next frame to ensure loading popup renders first
                CabbyMenu.Utilities.CoroutineRunner.RunNextFrame(() => {
                    try
                    {
                        // Get the current toggle state
                        bool currentShowAllFlags = toggleState.Get();
                        
                        // Create new panels for the selected area
                        var newAreaPanels = CreateRoomFlagsPanels(areaIndex, currentShowAllFlags);
                        
                        // Get all current panels and find the toggle and dropdown panels to preserve them
                                var allPanels = container.GetAllPanels();
                                var foundTogglePanel = allPanels.FirstOrDefault(p => p.GetDescription().Contains("Show ALL flags"));
                                var foundDropdownPanel = allPanels.FirstOrDefault(p => p.GetDescription().Contains("Select Area"));
                                
                                if (foundDropdownPanel != null)
                                {
                                    // Find the index of the dropdown panel (always exists)
                                    int dropdownIndex = container.GetPanelIndex(foundDropdownPanel);
                                    
                                    // Determine what to preserve based on whether dev options are enabled
                                    int preserveCount;
                                    if (foundTogglePanel != null && CabbyCodesPlugin.configFile.Bind("Settings", "EnableDevOptions", Constants.DEFAULT_DEV_OPTIONS_ENABLED, 
                                        "Enable developer options and advanced features").Value)
                                    {
                                        // Dev options are on - preserve both toggle and dropdown
                                        int toggleIndex = container.GetPanelIndex(foundTogglePanel);
                                        preserveCount = Math.Max(toggleIndex, dropdownIndex) + 1;
                                    }
                                    else
                                    {
                                        // Dev options are off - preserve only dropdown
                                        preserveCount = dropdownIndex + 1;
                                    }
                                    
                                    // Remove all panels after the preserved panels
                                    int dynamicPanelStartIndex = preserveCount;
                                    int dynamicPanelCount = allPanels.Count - preserveCount;
                                    
                                    if (dynamicPanelCount > 0)
                                    {
                                        // Remove only the dynamic room flag panels (starting after preserved panels)
                                        container.DetachPanelsAtRange(startIndex: dynamicPanelStartIndex, count: dynamicPanelCount);
                                    }
                                    
                                    // Insert the new dynamic panels after the preserved panels
                                    container.ReattachPanelsAtRange(newAreaPanels, index: dynamicPanelStartIndex);
                                }
                                else
                                {
                                    // Could not find dropdown panel - this shouldn't happen
                                }
                        
                        // Panel recreation complete - hide and destroy loading popup in the next frame
                        CabbyMenu.Utilities.CoroutineRunner.RunNextFrame(() => {
                            if (loadingPopup != null)
                            {
                                loadingPopup.Hide();
                                loadingPopup.Destroy();
                            }
                        });
                    }
                    catch
                    {
                        // Make sure to destroy loading popup even on error
                        if (loadingPopup != null)
                        {
                            loadingPopup.Hide();
                            loadingPopup.Destroy();
                        }
                    }
                });
            });

            // Defer initial panel creation to next frame, after the panels have been inserted
            CabbyMenu.Utilities.CoroutineRunner.RunNextFrame(() => panelManager.CreateInitialPanels(currentAreaIndex));

            return panels;
        }

        private static int GetCurrentAreaIndex()
        {
            try
            {
                // Get current scene name from shared provider
                string sceneName = GameStateProvider.GetCurrentSceneName();
                
                // Get area name from scene
                var sceneData = Scenes.SceneManagement.GetSceneData(sceneName);
                if (sceneData != null)
                {
                    string areaName = sceneData.AreaName;
                    
                    // Find the index of this area in the filtered list (only areas with flags)
                    var areaNames = Scenes.SceneManagement.GetAreaFlags().Keys.ToList();
                    int index = areaNames.IndexOf(areaName);
                    
                    // If current area has flags, return its index; otherwise default to 0
                    return index >= 0 ? index : 0;
                }
            }
            catch
            {
                // If anything goes wrong, default to 0
            }
            
            return 0; // Default to first area if we can't determine current area
        }

        private static List<CheatPanel> CreateRoomFlagsPanels(int areaIndex)
        {
            return CreateRoomFlagsPanels(areaIndex, false);
        }

        private static List<CheatPanel> CreateRoomFlagsPanels(int areaIndex, bool showAllFlags)
        {
            var panels = new List<CheatPanel>();
            
            // Get the area flags dictionary based on the showAllFlags setting
            var areaFlags = showAllFlags 
                ? Scenes.SceneManagement.GetAllAreaFlags() 
                : Scenes.SceneManagement.GetAreaFlags();
            var areaNames = areaFlags.Keys.ToList();
            
            if (areaIndex >= 0 && areaIndex < areaNames.Count)
            {
                string selectedArea = areaNames[areaIndex];
                var flagsForArea = areaFlags[selectedArea];
                
                // Group flags by scene for better organization
                var flagsByScene = flagsForArea
                    .GroupBy(f => f.SceneName)
                    .OrderBy(g => g.Key)
                    .ToList();
                
                foreach (var sceneGroup in flagsByScene)
                {
                    string sceneName = sceneGroup.Key;
                    var sceneFlags = sceneGroup.ToList();
                    
                    if (sceneFlags.Count > 0)
                    {
                        // Add scene subheader
                        var sceneData = Scenes.SceneManagement.GetSceneData(sceneName);
                        var sceneDisplayName = sceneData?.ReadableName ?? sceneName;
                        panels.Add(new InfoPanel(sceneDisplayName).SetColor(CheatPanel.subHeaderColor));

                        // Add flag panels for this scene
                        foreach (var flag in sceneFlags)
                        {
                            var flagPanel = CreateFlagPanel(flag);
                            if (flagPanel != null)
                            {
                                panels.Add(flagPanel);
                            }
                        }
                    }
                }
            }
            
            return panels;
        }

        private static CheatPanel CreateFlagPanel(CabbyCodes.Flags.FlagDef flagDef)
        {
            var displayName = flagDef.ReadableName;
            CheatPanel panel = null;

            switch (flagDef.Type)
            {
                case "PlayerData_Bool":
                    var togglePanel = new TogglePanel(
                        new CabbyMenu.SyncedReferences.DelegateReference<bool>(
                            () => CabbyCodes.Flags.FlagManager.GetBoolFlag(flagDef, flagDef.SemiPersistent),
                            v => CabbyCodes.Flags.FlagManager.SetBoolFlag(flagDef, v, flagDef.SemiPersistent)),
                        displayName);
                    
                    // Check if we're in the same scene and disable if so
                    if (IsInSameScene(flagDef.SceneName))
                    {
                        togglePanel.GetToggleButton().SetInteractable(false);
                        togglePanel.GetToggleButton().SetDisabledMessage("Cannot edit room flags while in the room");
                    }
                    
                    panel = togglePanel;
                    break;

                case "PlayerData_Int":
                    var intPanel = new RangeInputFieldPanel<int>(
                        new CabbyMenu.SyncedReferences.DelegateReference<int>(
                            () => CabbyCodes.Flags.FlagManager.GetIntFlag(flagDef),
                            v => CabbyCodes.Flags.FlagManager.SetIntFlag(flagDef, v)),
                        CabbyMenu.Utilities.KeyCodeMap.ValidChars.Numeric, 0, 999, displayName);
                    
                    // Check if we're in the same scene and disable if so
                    if (IsInSameScene(flagDef.SceneName))
                    {
                        // For input field panels, we need to disable the input field component
                        var inputField = intPanel.GetInputField();
                        if (inputField != null)
                        {
                            inputField.interactable = false;
                            // Add hover popup to the panel itself since input fields don't have built-in hover popups
                            AddHoverPopupToPanel(intPanel, "Cannot edit room flags while in the room");
                        }
                    }
                    
                    panel = intPanel;
                    break;

                case "PlayerData_Float":
                    var floatPanel = new RangeInputFieldPanel<float>(
                        new CabbyMenu.SyncedReferences.DelegateReference<float>(
                            () => CabbyCodes.Flags.FlagManager.GetFloatFlag(flagDef),
                            v => CabbyCodes.Flags.FlagManager.SetFloatFlag(flagDef, v)),
                        CabbyMenu.Utilities.KeyCodeMap.ValidChars.Decimal, 0f, 999f, displayName);
                    
                    // Check if we're in the same scene and disable if so
                    if (IsInSameScene(flagDef.SceneName))
                    {
                        // For input field panels, we need to disable the input field component
                        var inputField = floatPanel.GetInputField();
                        if (inputField != null)
                        {
                            inputField.interactable = false;
                            // Add hover popup to the panel itself since input fields don't have built-in hover popups
                            AddHoverPopupToPanel(floatPanel, "Cannot edit room flags while in the room");
                        }
                    }
                    
                    panel = floatPanel;
                    break;

                case "PersistentBoolData":
                    var persistentTogglePanel = new TogglePanel(
                        new CabbyMenu.SyncedReferences.DelegateReference<bool>(
                            () => CabbyCodes.Flags.FlagManager.GetBoolFlag(flagDef, flagDef.SemiPersistent),
                            v => CabbyCodes.Flags.FlagManager.SetBoolFlag(flagDef, v, flagDef.SemiPersistent)),
                        displayName);
                    
                    // Check if we're in the same scene and disable if so
                    if (IsInSameScene(flagDef.SceneName))
                    {
                        persistentTogglePanel.GetToggleButton().SetInteractable(false);
                        persistentTogglePanel.GetToggleButton().SetDisabledMessage("Cannot edit room flags while in the room");
                    }
                    
                    panel = persistentTogglePanel;
                    break;

                case "PersistentIntData":
                    var persistentIntPanel = new RangeInputFieldPanel<int>(
                        new CabbyMenu.SyncedReferences.DelegateReference<int>(
                            () => CabbyCodes.Flags.FlagManager.GetIntFlag(flagDef),
                            v => CabbyCodes.Flags.FlagManager.SetIntFlag(flagDef, v)),
                        CabbyMenu.Utilities.KeyCodeMap.ValidChars.Numeric, 0, 999, displayName);
                    
                    // Check if we're in the same scene and disable if so
                    if (IsInSameScene(flagDef.SceneName))
                    {
                        // For input field panels, we need to disable the input field component
                        var inputField = persistentIntPanel.GetInputField();
                        if (inputField != null)
                        {
                            inputField.interactable = false;
                            // Add hover popup to the panel itself since input fields don't have built-in hover popups
                            AddHoverPopupToPanel(persistentIntPanel, "Cannot edit room flags while in the room");
                        }
                    }
                    
                    panel = persistentIntPanel;
                    break;

                case "GeoRockData":
                    var geoRockPanel = new RangeInputFieldPanel<int>(
                        new CabbyMenu.SyncedReferences.DelegateReference<int>(
                            () => {
                                if (SceneData.instance?.geoRocks == null) return 0;
                                foreach (var grd in SceneData.instance.geoRocks)
                                    if (grd.id == flagDef.Id && grd.sceneName == flagDef.SceneName)
                                        return grd.hitsLeft;
                                return 0;
                            },
                            v => {
                                if (SceneData.instance?.geoRocks == null) return;
                                foreach (var grd in SceneData.instance.geoRocks)
                                    if (grd.id == flagDef.Id && grd.sceneName == flagDef.SceneName)
                                    {
                                        grd.hitsLeft = v;
                                        break;
                                    }
                            }),
                        CabbyMenu.Utilities.KeyCodeMap.ValidChars.Numeric, 0, 10, displayName + " (Hits Left)");
                    
                    // Check if we're in the same scene and disable if so
                    if (IsInSameScene(flagDef.SceneName))
                    {
                        // For input field panels, we need to disable the input field component
                        var inputField = geoRockPanel.GetInputField();
                        if (inputField != null)
                        {
                            inputField.interactable = false;
                            // Add hover popup to the panel itself since input fields don't have built-in hover popups
                            AddHoverPopupToPanel(geoRockPanel, "Cannot edit room flags while in the room");
                        }
                    }
                    
                    panel = geoRockPanel;
                    break;

                default:
                    return null;
            }
            
            return panel;
        }

        /// <summary>
        /// Checks if the current scene matches the specified scene name.
        /// </summary>
        /// <param name="sceneName">The scene name to check against</param>
        /// <returns>True if the current scene matches, false otherwise</returns>
        private static bool IsInSameScene(string sceneName)
        {
            try
            {
                string currentSceneName = GameStateProvider.GetCurrentSceneName();
                return currentSceneName == sceneName;
            }
            catch
            {
                // If anything goes wrong, assume we're not in the same scene
                return false;
            }
        }

        /// <summary>
        /// Adds a hover popup to a panel for disabled input field panels.
        /// </summary>
        /// <param name="panel">The panel to add the hover popup to</param>
        /// <param name="message">The message to display on hover</param>
        private static void AddHoverPopupToPanel(CheatPanel panel, string message)
        {
            if (panel?.GetGameObject() != null)
            {
                var hoverPopup = panel.GetGameObject().AddComponent<HoverPopup>();
                
                // Add EventTrigger to handle mouse enter/exit events for the hover popup
                var eventTrigger = panel.GetGameObject().GetComponent<EventTrigger>();
                if (eventTrigger == null)
                {
                    eventTrigger = panel.GetGameObject().AddComponent<EventTrigger>();
                }

                // Mouse Enter event
                EventTrigger.Entry enterEntry = new EventTrigger.Entry
                {
                    eventID = EventTriggerType.PointerEnter
                };
                enterEntry.callback.AddListener((data) => hoverPopup.ShowPopup(message));
                eventTrigger.triggers.Add(enterEntry);

                // Mouse Exit event
                EventTrigger.Entry exitEntry = new EventTrigger.Entry
                {
                    eventID = EventTriggerType.PointerExit
                };
                exitEntry.callback.AddListener((data) => hoverPopup.HidePopup());
                eventTrigger.triggers.Add(exitEntry);
            }
        }
    }
} 