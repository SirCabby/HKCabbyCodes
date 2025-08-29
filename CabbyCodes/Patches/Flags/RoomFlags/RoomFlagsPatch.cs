using CabbyMenu.UI.CheatPanels;
using CabbyMenu.UI.DynamicPanels;
using CabbyMenu.UI.Controls;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;

namespace CabbyCodes.Patches.Flags.RoomFlags
{
    public class RoomFlagsPatch
    {
        public static List<CheatPanel> CreatePanels()
        {
            var panels = new List<CheatPanel>();

            // Determine the default area selection based on the player's current position
            int currentAreaIndex = GetCurrentAreaIndex();

            // Dropdown bound to an AreaSelector reference
            var areaSelector  = new AreaSelector(currentAreaIndex);
            var dropdownPanel = new DropdownPanel(areaSelector, "Select Area", Constants.DEFAULT_PANEL_HEIGHT);
            panels.Add(dropdownPanel);

            // Container proxying to the main menu
            var container = new MainMenuPanelContainer(CabbyCodesPlugin.cabbyMenu);

            // Manager responsible for creating and replacing room flag panels when the dropdown changes
            var panelManager = new DynamicPanelManager(
                dropdownPanel,
                CreateRoomFlagsPanels,
                container,
                insertionIndex: 1,  // insert directly after the dropdown
                parentManager: null,
                onPanelsChanged: null,
                menu: CabbyCodesPlugin.cabbyMenu
            );

            // Selection change listener will rebuild panels; no per-frame update needed
            dropdownPanel.GetDropDownSync().GetCustomDropdown().onValueChanged.AddListener(_ => panelManager.RecreateDynamicPanels());

            // Defer initial panel creation to next frame, after the dropdown has been inserted
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
            var panels = new List<CheatPanel>();
            
            // Get the area flags dictionary
            var areaFlags = Scenes.SceneManagement.GetAreaFlags();
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