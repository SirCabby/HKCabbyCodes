using CabbyMenu.UI.CheatPanels;
using System.Collections.Generic;
using System.Linq;
using CabbyCodes.Flags;
using static CabbyCodes.Scenes.SceneManagement;
using CabbyMenu.Utilities;
using CabbyMenu.SyncedReferences;
using CabbyCodes.Flags.FlagData;

namespace CabbyCodes.Patches.Flags
{
    public class SceneFlagsPanelManager
    {
        private readonly SceneFlagsAreaSelector areaSelector;
        private readonly List<CheatPanel> dynamicPanels = new List<CheatPanel>();
        private string currentArea = "";

        public SceneFlagsPanelManager(SceneFlagsAreaSelector areaSelector)
        {
            this.areaSelector = areaSelector;
        }

        public void AddAllPanelsToMenu()
        {
            // Initial setup
            UpdateVisibleArea();
        }

        public void UpdateVisibleArea()
        {
            // Remove existing dynamic panels
            foreach (var panel in dynamicPanels)
            {
                CabbyCodesPlugin.cabbyMenu.RemoveCheatPanel(panel);
            }
            dynamicPanels.Clear();

            // Get current area name from the area selector's filtered list
            var areaNames = areaSelector.GetValueList();
            var selectedAreaIndex = areaSelector.Get();
            if (selectedAreaIndex >= 0 && selectedAreaIndex < areaNames.Count)
            {
                currentArea = areaNames[selectedAreaIndex];
            }
            else
            {
                currentArea = "";
                return;
            }

            // Get scenes for this area
            var areaToScenes = GetAreaToScenesMapping();
            if (!areaToScenes.ContainsKey(currentArea))
            {
                return;
            }

            var scenesInArea = areaToScenes[currentArea];

            // Create panels for each scene that has flags
            foreach (var sceneName in scenesInArea.OrderBy(s => s))
            {
                // Get flags for this scene using SceneFlagData
                var sceneFlags = SceneFlagData.GetFlagsForScene(sceneName);
                
                if (sceneFlags.Count > 0)
                {
                    // Add scene subheader
                    var sceneData = GetSceneData(sceneName);
                    var sceneDisplayName = sceneData?.ReadableName ?? sceneName;
                    var subheaderPanel = new InfoPanel(sceneDisplayName).SetColor(CheatPanel.subHeaderColor);
                    CabbyCodesPlugin.cabbyMenu.AddCheatPanel(subheaderPanel);
                    dynamicPanels.Add(subheaderPanel);

                    // Add flag panels for this scene
                    foreach (var flag in sceneFlags.OrderBy(f => f.Id))
                    {
                        var flagPanel = CreateFlagPanel(flag);
                        if (flagPanel != null)
                        {
                            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(flagPanel);
                            dynamicPanels.Add(flagPanel);
                        }
                    }
                }
            }
        }

        private CheatPanel CreateFlagPanel(FlagDef flagDef)
        {
            var displayName = flagDef.ReadableName;

            switch (flagDef.Type)
            {
                case "PlayerData_Bool":
                    return new TogglePanel(
                        new DelegateReference<bool>(
                            () => FlagManager.GetBoolFlag(flagDef, flagDef.SemiPersistent),
                            v => FlagManager.SetBoolFlag(flagDef, v, flagDef.SemiPersistent)),
                        displayName);

                case "PlayerData_Int":
                    return new RangeInputFieldPanel<int>(
                        new DelegateReference<int>(
                            () => FlagManager.GetIntFlag(flagDef),
                            v => FlagManager.SetIntFlag(flagDef, v)),
                        KeyCodeMap.ValidChars.Numeric, 0, 999, displayName);

                case "PlayerData_Float":
                    return new RangeInputFieldPanel<float>(
                        new DelegateReference<float>(
                            () => FlagManager.GetFloatFlag(flagDef),
                            v => FlagManager.SetFloatFlag(flagDef, v)),
                        KeyCodeMap.ValidChars.Decimal, 0f, 999f, displayName);

                case "PersistentBoolData":
                    return new TogglePanel(
                        new DelegateReference<bool>(
                            () => FlagManager.GetBoolFlag(flagDef, flagDef.SemiPersistent),
                            v => FlagManager.SetBoolFlag(flagDef, v, flagDef.SemiPersistent)),
                        displayName);

                case "PersistentIntData":
                    return new RangeInputFieldPanel<int>(
                        new DelegateReference<int>(
                            () => FlagManager.GetIntFlag(flagDef),
                            v => FlagManager.SetIntFlag(flagDef, v)),
                        KeyCodeMap.ValidChars.Numeric, 0, 999, displayName);

                case "GeoRockData":
                    return new RangeInputFieldPanel<int>(
                        new DelegateReference<int>(
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
                        KeyCodeMap.ValidChars.Numeric, 0, 10, displayName + " (Hits Left)");

                default:
                    return null;
            }
        }
    }
} 