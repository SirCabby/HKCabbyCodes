using CabbyMenu.UI.CheatPanels;
using CabbyMenu.UI.DynamicPanels;
using System.Collections.Generic;
using System.Linq;

namespace CabbyCodes.Patches.Flags.RoomFlags
{
    public class RoomFlagsPatch
    {
        public static List<CheatPanel> CreatePanels()
        {
            var panels = new List<CheatPanel>
            {
                new InfoPanel("Toggling room flags will not update until entering the room").SetColor(CheatPanel.warningColor)
            };

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
                insertionIndex: 1  // insert directly after the dropdown
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

            switch (flagDef.Type)
            {
                case "PlayerData_Bool":
                    return new TogglePanel(
                        new CabbyMenu.SyncedReferences.DelegateReference<bool>(
                            () => CabbyCodes.Flags.FlagManager.GetBoolFlag(flagDef, flagDef.SemiPersistent),
                            v => CabbyCodes.Flags.FlagManager.SetBoolFlag(flagDef, v, flagDef.SemiPersistent)),
                        displayName);

                case "PlayerData_Int":
                    return new RangeInputFieldPanel<int>(
                        new CabbyMenu.SyncedReferences.DelegateReference<int>(
                            () => CabbyCodes.Flags.FlagManager.GetIntFlag(flagDef),
                            v => CabbyCodes.Flags.FlagManager.SetIntFlag(flagDef, v)),
                        CabbyMenu.Utilities.KeyCodeMap.ValidChars.Numeric, 0, 999, displayName);

                case "PlayerData_Float":
                    return new RangeInputFieldPanel<float>(
                        new CabbyMenu.SyncedReferences.DelegateReference<float>(
                            () => CabbyCodes.Flags.FlagManager.GetFloatFlag(flagDef),
                            v => CabbyCodes.Flags.FlagManager.SetFloatFlag(flagDef, v)),
                        CabbyMenu.Utilities.KeyCodeMap.ValidChars.Decimal, 0f, 999f, displayName);

                case "PersistentBoolData":
                    return new TogglePanel(
                        new CabbyMenu.SyncedReferences.DelegateReference<bool>(
                            () => CabbyCodes.Flags.FlagManager.GetBoolFlag(flagDef, flagDef.SemiPersistent),
                            v => CabbyCodes.Flags.FlagManager.SetBoolFlag(flagDef, v, flagDef.SemiPersistent)),
                        displayName);

                case "PersistentIntData":
                    return new RangeInputFieldPanel<int>(
                        new CabbyMenu.SyncedReferences.DelegateReference<int>(
                            () => CabbyCodes.Flags.FlagManager.GetIntFlag(flagDef),
                            v => CabbyCodes.Flags.FlagManager.SetIntFlag(flagDef, v)),
                        CabbyMenu.Utilities.KeyCodeMap.ValidChars.Numeric, 0, 999, displayName);

                case "GeoRockData":
                    return new RangeInputFieldPanel<int>(
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

                default:
                    return null;
            }
        }
    }
} 