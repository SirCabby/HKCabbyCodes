using CabbyMenu.UI.CheatPanels;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
                    var boolSync = new BoolFlagSync(flagDef);
                    return new TogglePanel(boolSync, displayName);

                case "PlayerData_Int":
                    var intSync = new IntFlagSync(flagDef);
                    return new RangeInputFieldPanel<int>(intSync, KeyCodeMap.ValidChars.Numeric, 0, 999, displayName);

                case "PlayerData_Float":
                    var floatSync = new FloatFlagSync(flagDef);
                    return new RangeInputFieldPanel<float>(floatSync, KeyCodeMap.ValidChars.Decimal, 0f, 999f, displayName);

                case "PersistentBoolData":
                    var persistentBoolSync = new PersistentBoolFlagSync(flagDef);
                    return new TogglePanel(persistentBoolSync, displayName);

                case "PersistentIntData":
                    var persistentIntSync = new PersistentIntFlagSync(flagDef);
                    return new RangeInputFieldPanel<int>(persistentIntSync, KeyCodeMap.ValidChars.Numeric, 0, 999, displayName);

                case "GeoRockData":
                    var geoRockSync = new GeoRockFlagSync(flagDef);
                    return new TogglePanel(geoRockSync, displayName);

                default:
                    return null;
            }
        }

        private class BoolFlagSync : ISyncedReference<bool>
        {
            private readonly FlagDef flagDef;

            public BoolFlagSync(FlagDef flagDef)
            {
                this.flagDef = flagDef;
            }

            public bool Get()
            {
                return FlagManager.GetBoolFlag(flagDef, flagDef.SemiPersistent);
            }

            public void Set(bool value)
            {
                FlagManager.SetBoolFlag(flagDef, value, flagDef.SemiPersistent);
            }
        }

        private class IntFlagSync : ISyncedReference<int>
        {
            private readonly FlagDef flagDef;

            public IntFlagSync(FlagDef flagDef)
            {
                this.flagDef = flagDef;
            }

            public int Get()
            {
                return FlagManager.GetIntFlag(flagDef);
            }

            public void Set(int value)
            {
                FlagManager.SetIntFlag(flagDef, value);
            }
        }

        private class FloatFlagSync : ISyncedReference<float>
        {
            private readonly FlagDef flagDef;

            public FloatFlagSync(FlagDef flagDef)
            {
                this.flagDef = flagDef;
            }

            public float Get()
            {
                return FlagManager.GetFloatFlag(flagDef);
            }

            public void Set(float value)
            {
                FlagManager.SetFloatFlag(flagDef, value);
            }
        }

        private class PersistentBoolFlagSync : ISyncedReference<bool>
        {
            private readonly FlagDef flagDef;

            public PersistentBoolFlagSync(FlagDef flagDef)
            {
                this.flagDef = flagDef;
            }

            public bool Get()
            {
                return FlagManager.GetBoolFlag(flagDef, flagDef.SemiPersistent);
            }

            public void Set(bool value)
            {
                FlagManager.SetBoolFlag(flagDef, value, flagDef.SemiPersistent);
            }
        }

        private class PersistentIntFlagSync : ISyncedReference<int>
        {
            private readonly FlagDef flagDef;

            public PersistentIntFlagSync(FlagDef flagDef)
            {
                this.flagDef = flagDef;
            }

            public int Get()
            {
                return FlagManager.GetIntFlag(flagDef);
            }

            public void Set(int value)
            {
                FlagManager.SetIntFlag(flagDef, value);
            }
        }

        private class GeoRockFlagSync : ISyncedReference<bool>
        {
            private readonly FlagDef flagDef;

            public GeoRockFlagSync(FlagDef flagDef)
            {
                this.flagDef = flagDef;
            }

            public bool Get()
            {
                // GeoRockData is considered "true" when broken (hitsLeft <= 0)
                return FlagManager.GetBoolFlag(flagDef, flagDef.SemiPersistent);
            }

            public void Set(bool value)
            {
                FlagManager.SetBoolFlag(flagDef, value, flagDef.SemiPersistent);
            }
        }
    }
} 