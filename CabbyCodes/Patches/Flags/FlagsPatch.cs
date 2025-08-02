using CabbyCodes.Patches.Flags.General;
using CabbyCodes.Patches.Flags.NPC_Status;
using CabbyCodes.Patches.Flags.Triage;
using CabbyCodes.Patches.Teleport;
using CabbyMenu.UI.CheatPanels;
using System.Collections.Generic;
using System.Linq;

namespace CabbyCodes.Patches.Flags
{
    public class FlagsPatch
    {
        public static void AddPanels()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Game Flags").SetColor(CheatPanel.headerColor));

            // Add Options section using the new generic system
            var optionsSection = new CategorizedPanelSection(
                "Options", 
                "Select Option", 
                new List<string> { "Option 1", "Option 2" },
                CreateOptionsPanels
            );
            optionsSection.AddToMenu();

            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("General").SetColor(CheatPanel.subHeaderColor));
            CrossroadsInfectedPatch.AddPanel();

            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("NPC Flags").SetColor(CheatPanel.subHeaderColor));
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Must re-enter npc room to see update").SetColor(CheatPanel.warningColor));
            MylaWaifuPatch.AddPanel();

            FlagExtractionPatch.AddPanel();
            FlagMonitorPatch.AddPanel();

            // Get current area for default selection
            int currentAreaIndex = GetCurrentAreaIndex();
            
            // Add Room Flags section using the new generic system
            var roomFlagsSection = new CategorizedPanelSection(
                "Room Flags",
                "Select Area",
                Scenes.SceneManagement.GetAreaNamesWithFlags().ToList(),
                CreateRoomFlagsPanels,
                1, // insertionIndex
                currentAreaIndex // defaultSelection
            );
            roomFlagsSection.AddToMenu();
        }

        private static int GetCurrentAreaIndex()
        {
            try
            {
                // Get current scene name
                var (sceneName, _) = TeleportService.GetCurrentPlayerPosition();
                
                // Get area name from scene
                var sceneData = Scenes.SceneManagement.GetSceneData(sceneName);
                if (sceneData != null)
                {
                    string areaName = sceneData.AreaName;
                    
                    // Find the index of this area in the filtered list (only areas with flags)
                    var areaNames = Scenes.SceneManagement.GetAreaNamesWithFlags().ToList();
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

        private static List<CheatPanel> CreateOptionsPanels(int optionIndex)
        {
            var panels = new List<CheatPanel>();
            
            string panelLabel = optionIndex == 0 ? "option 1 sub" : "option 2 sub";
            panels.Add(new InfoPanel(panelLabel).SetColor(CheatPanel.subHeaderColor));
            
            return panels;
        }

        private static List<CheatPanel> CreateRoomFlagsPanels(int areaIndex)
        {
            var panels = new List<CheatPanel>
            {
                new InfoPanel("Toggling room flags will not update until entering the room").SetColor(CheatPanel.warningColor)
            };
            
            // Get the selected area
            var areaNames = Scenes.SceneManagement.GetAreaNamesWithFlags().ToList();
            if (areaIndex >= 0 && areaIndex < areaNames.Count)
            {
                string selectedArea = areaNames[areaIndex];
                
                // Create panels for the selected area using the existing SceneFlagsPanelManager logic
                var areaToScenes = Scenes.SceneManagement.GetAreaToScenesMapping();
                if (areaToScenes.ContainsKey(selectedArea))
                {
                    var scenesInArea = areaToScenes[selectedArea];
                    
                    foreach (var sceneName in scenesInArea)
                    {
                        var sceneFlags = CabbyCodes.Flags.FlagData.SceneFlagData.GetFlagsForScene(sceneName);
                        
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
