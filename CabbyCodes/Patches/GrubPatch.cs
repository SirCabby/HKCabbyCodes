using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using System.Collections.Generic;
using System.Linq;

namespace CabbyCodes.Patches
{
    /// <summary>
    /// Handles grub rescue functionality and tracking for specific scenes.
    /// </summary>
    public class GrubPatch : ISyncedReference<bool>
    {
        /// <summary>
        /// The identifier for grub bottle persistent data.
        /// </summary>
        private static readonly string grubId = "Grub Bottle";

        /// <summary>
        /// List of scenes that contain grubs.
        /// </summary>
        private static readonly List<string> grubScenes = new List<string>()
        {
            "Abyss_17",
            "Abyss_19",
            "Crossroads_03",
            "Crossroads_05",
            "Crossroads_31",
            "Crossroads_35",
            "Crossroads_48",
            "Deepnest_03",
            "Deepnest_31",
            "Deepnest_36",
            "Deepnest_39",
            "Deepnest_East_11",
            "Deepnest_East_14",
            "Deepnest_Spider_Town",
            "Fungus1_06",
            "Fungus1_07",
            "Fungus1_13",
            "Fungus1_21",
            "Fungus1_28",
            "Fungus2_18",
            "Fungus2_20",
            "Fungus3_10",
            "Fungus3_22",
            "Fungus3_47",
            "Fungus3_48",
            "Hive_03",
            "Hive_04",
            "Mines_03",
            "Mines_04",
            "Mines_16",
            "Mines_19",
            "Mines_24",
            "Mines_31",
            "Mines_35",
            "RestingGrounds_10",
            "Ruins_House_01",
            "Ruins1_05",
            "Ruins1_32",
            "Ruins2_03",
            "Ruins2_07",
            "Ruins2_11",
            "Waterways_04",
            "Waterways_13",
            "Waterways_14",
        };

        /// <summary>
        /// Mapping from scene names to area names based on the maps data.
        /// </summary>
        private static readonly Dictionary<string, string> sceneToAreaMap = BuildSceneToAreaMap();

        /// <summary>
        /// The scene name this grub patch is associated with.
        /// </summary>
        private readonly string sceneName;

        /// <summary>
        /// Initializes a new instance of the GrubPatch class.
        /// </summary>
        /// <param name="sceneName">The scene name where the grub is located.</param>
        public GrubPatch(string sceneName)
        {
            this.sceneName = sceneName;
        }

        /// <summary>
        /// Gets whether the grub in this scene has been rescued.
        /// </summary>
        /// <returns>True if the grub has been rescued, false otherwise.</returns>
        public bool Get()
        {
            // true = got it
            return PbdMaker.GetPbd(grubId, sceneName).activated;
        }

        /// <summary>
        /// Sets whether the grub in this scene has been rescued.
        /// </summary>
        /// <param name="value">True to mark the grub as rescued, false to mark it as not rescued.</param>
        public void Set(bool value)
        {
            bool hasGrub = Get();

            if (value && !hasGrub)
            {
                PersistentBoolData pbd = PbdMaker.GetPbd(grubId, sceneName);
                pbd.activated = true;
                SceneData.instance.SaveMyState(pbd);

                if (!PlayerData.instance.scenesGrubRescued.Contains(sceneName))
                {
                    PlayerData.instance.scenesGrubRescued.Add(sceneName);
                    PlayerData.instance.grubsCollected++;
                }
            }
            else if (!value && hasGrub)
            {
                PersistentBoolData pbd = PbdMaker.GetPbd(grubId, sceneName);
                pbd.activated = false;
                SceneData.instance.SaveMyState(pbd);

                if (PlayerData.instance.scenesGrubRescued.Contains(sceneName))
                {
                    PlayerData.instance.scenesGrubRescued.Remove(sceneName);
                    PlayerData.instance.grubsCollected--;
                }
            }
        }

        /// <summary>
        /// Builds a mapping from scene names to area names based on the maps data.
        /// </summary>
        /// <returns>Dictionary mapping scene names to area names.</returns>
        private static Dictionary<string, string> BuildSceneToAreaMap()
        {
            var map = new Dictionary<string, string>();
            
            // Add mappings based on the area-scene correlations from MapRoomPatch
            var areaScenes = new Dictionary<string, List<string>>
            {
                {
                    "Dirtmouth",
                    new List<string>
                    {
                        "Town", "Tutorial_01"
                    }
                },
                {
                    "Abyss",
                    new List<string>
                    {
                        "Abyss_03", "Abyss_04", "Abyss_05", "Abyss_06_Core", "Abyss_06_Core_b",
                        "Abyss_08", "Abyss_09", "Abyss_10", "Abyss_12", "Abyss_16",
                        "Abyss_17", "Abyss_18", "Abyss_18_b", "Abyss_19", "Abyss_20",
                        "Abyss_21", "Abyss_22"
                    }
                },
                {
                    "City",
                    new List<string>
                    {
                        "Crossroads_49b", "Ruins1_01", "Ruins1_02", "Ruins1_03", "Ruins1_04",
                        "Ruins1_05", "Ruins1_05b", "Ruins1_05c", "Ruins1_06", "Ruins1_09",
                        "Ruins1_17", "Ruins1_18", "Ruins1_18_b", "Ruins1_23", "Ruins1_24",
                        "Ruins1_25", "Ruins1_27", "Ruins1_28", "Ruins1_29", "Ruins1_30",
                        "Ruins1_31", "Ruins1_31b", "Ruins1_31_top", "Ruins1_31_top_2",
                        "Ruins1_32", "Ruins2_01", "Ruins2_01_b", "Ruins2_03", "Ruins2_03b",
                        "Ruins2_04", "Ruins2_05", "Ruins2_06", "Ruins2_07", "Ruins2_07_left",
                        "Ruins2_07_right", "Ruins2_08", "Ruins2_09", "Ruins2_10_b",
                        "Ruins2_11", "Ruins2_11_b", "Ruins2_Watcher_Room", "Ruins_Bathhouse",
                        "Ruins_Elevator", "Ruins_House_01"
                    }
                },
                {
                    "Crossroads",
                    new List<string>
                    {
                        "Crossroads_01", "Crossroads_02", "Crossroads_03", "Crossroads_04",
                        "Crossroads_05", "Crossroads_06", "Crossroads_07", "Crossroads_08",
                        "Crossroads_09", "Crossroads_10", "Crossroads_11_alt", "Crossroads_12",
                        "Crossroads_13", "Crossroads_14", "Crossroads_15", "Crossroads_16",
                        "Crossroads_18", "Crossroads_19", "Crossroads_21", "Crossroads_22",
                        "Crossroads_25", "Crossroads_27", "Crossroads_30", "Crossroads_31",
                        "Crossroads_33", "Crossroads_35", "Crossroads_36", "Crossroads_37",
                        "Crossroads_38", "Crossroads_39", "Crossroads_40", "Crossroads_42",
                        "Crossroads_43", "Crossroads_45", "Crossroads_46", "Crossroads_47",
                        "Crossroads_48", "Crossroads_49", "Crossroads_52"
                    }
                },
                {
                    "Mines",
                    new List<string>
                    {
                        "Mines_01", "Mines_02", "Mines_03", "Mines_04", "Mines_05",
                        "Mines_06", "Mines_07", "Mines_10", "Mines_11", "Mines_13",
                        "Mines_16", "Mines_17", "Mines_18", "Mines_19", "Mines_20",
                        "Mines_20_b", "Mines_23", "Mines_24", "Mines_25", "Mines_28",
                        "Mines_28_b", "Mines_29", "Mines_30", "Mines_31", "Mines_32",
                        "Mines_34", "Mines_35", "Mines_36", "Mines_37"
                    }
                },
                {
                    "Cliffs",
                    new List<string>
                    {
                        "Cliffs_01", "Cliffs_01_b", "Cliffs_02", "Cliffs_02_b",
                        "Cliffs_04", "Cliffs_05", "Cliffs_06", "Cliffs_06_b",
                        "Fungus1_28", "Fungus1_28_b"
                    }
                },
                {
                    "Deepnest",
                    new List<string>
                    {
                        "Abyss_03_b", "Deepnest_01b", "Deepnest_02", "Deepnest_03",
                        "Deepnest_09", "Deepnest_10", "Deepnest_14", "Deepnest_16",
                        "Deepnest_17", "Deepnest_26", "Deepnest_26b", "Deepnest_30",
                        "Deepnest_30_b", "Deepnest_31", "Deepnest_32", "Deepnest_33",
                        "Deepnest_34", "Deepnest_35", "Deepnest_36", "Deepnest_37",
                        "Deepnest_38", "Deepnest_39", "Deepnest_40", "Deepnest_41",
                        "Deepnest_41_b", "Deepnest_42", "Deepnest_44", "Deepnest_44_b",
                        "Fungus2_25", "Room_Mask_maker", "Deepnest_Spider_Town"
                    }
                },
                {
                    "FogCanyon",
                    new List<string>
                    {
                        "Fungus3_01", "Fungus3_02", "Fungus3_03", "Fungus3_24",
                        "Fungus3_25", "Fungus3_25b", "Fungus3_26", "Fungus3_27",
                        "Fungus3_28", "Fungus3_30", "Fungus3_35", "Fungus3_44",
                        "Fungus3_47"
                    }
                },
                {
                    "FungalWastes",
                    new List<string>
                    {
                        "Deepnest_01", "Fungus2_01", "Fungus2_02", "Fungus2_03",
                        "Fungus2_04", "Fungus2_05", "Fungus2_06", "Fungus2_07",
                        "Fungus2_08", "Fungus2_09", "Fungus2_10", "Fungus2_11",
                        "Fungus2_12", "Fungus2_13", "Fungus2_14", "Fungus2_14_b",
                        "Fungus2_14_c", "Fungus2_15", "Fungus2_17", "Fungus2_18",
                        "Fungus2_19", "Fungus2_20", "Fungus2_21", "Fungus2_23",
                        "Fungus2_26", "Fungus2_28", "Fungus2_29", "Fungus2_30",
                        "Fungus2_31", "Fungus2_32", "Fungus2_29_b", "Fungus2_33",
                        "Fungus2_34"
                    }
                },
                {
                    "Greenpath",
                    new List<string>
                    {
                        "Fungus1_01", "Fungus1_01b", "Fungus1_02", "Fungus1_03",
                        "Fungus1_04", "Fungus1_05", "Fungus1_06", "Fungus1_07",
                        "Fungus1_08", "Fungus1_09", "Fungus1_09_b", "Fungus1_10",
                        "Fungus1_11", "Fungus1_12", "Fungus1_13", "Fungus1_14",
                        "Fungus1_14_b", "Fungus1_15", "Fungus1_16_alt", "Fungus1_17",
                        "Fungus1_19", "Fungus1_20_v02", "Fungus1_21", "Fungus1_22",
                        "Fungus1_25", "Fungus1_26", "Fungus1_29", "Fungus1_30",
                        "Fungus1_31", "Fungus1_32", "Fungus1_34", "Fungus1_37",
                        "Fungus1_Slug"
                    }
                },
                {
                    "Outskirts",
                    new List<string>
                    {
                        "Abyss_03_c", "Deepnest_East_01", "Deepnest_East_02",
                        "Deepnest_East_02b", "Deepnest_East_03", "Deepnest_East_04",
                        "Deepnest_East_06", "Deepnest_East_07", "Deepnest_East_08",
                        "Deepnest_East_09", "Deepnest_East_09_b", "Deepnest_East_10",
                        "Deepnest_East_11", "Deepnest_East_12", "Deepnest_East_13",
                        "Deepnest_East_14", "Deepnest_East_15", "Deepnest_East_16",
                        "Deepnest_East_18", "Deepnest_East_Hornet",
                        "Deepnest_East_Hornet_b", "Hive_01", "Hive_02", "Hive_03",
                        "Hive_03_b", "Hive_03_c", "Hive_04", "Hive_04_b", "Hive_05"
                    }
                },
                {
                    "RoyalGardens",
                    new List<string>
                    {
                        "Deepnest_43", "Deepnest_43_b", "Fungus1_23", "Fungus1_24",
                        "Fungus3_04", "Fungus3_05", "Fungus3_08", "Fungus3_10",
                        "Fungus3_11", "Fungus3_13", "Fungus3_21", "Fungus3_22",
                        "Fungus3_22_b", "Fungus3_23", "Fungus3_23_b", "Fungus3_34",
                        "Fungus3_39", "Fungus3_40", "Fungus3_48", "Fungus3_48_bot",
                        "Fungus3_48_left", "Fungus3_48_top", "Fungus3_49", "Fungus3_50"
                    }
                },
                {
                    "RestingGrounds",
                    new List<string>
                    {
                        "Crossroads_46b", "Crossroads_50", "RestingGrounds_02",
                        "RestingGrounds_04", "RestingGrounds_05", "RestingGrounds_06",
                        "RestingGrounds_08", "RestingGrounds_09", "RestingGrounds_10_b",
                        "RestingGrounds_10_c", "RestingGrounds_10_d", "RestingGrounds_12",
                        "RestingGrounds_17", "Ruins2_10", "RestingGrounds_10"
                    }
                },
                {
                    "Waterways",
                    new List<string>
                    {
                        "Abyss_01", "Abyss_02", "Waterways_01", "Waterways_02",
                        "Waterways_02b", "Waterways_03", "Waterways_04",
                        "Waterways_04_part_b", "Waterways_04b", "Waterways_05",
                        "Waterways_06", "Waterways_07", "Waterways_08", "Waterways_09",
                        "Waterways_12", "Waterways_13", "Waterways_14", "Waterways_15"
                    }
                }
            };

            foreach (var kvp in areaScenes)
            {
                foreach (string scene in kvp.Value)
                {
                    map[scene] = kvp.Key;
                }
            }

            return map;
        }

        /// <summary>
        /// Gets the area name for a given scene name.
        /// </summary>
        /// <param name="sceneName">The scene name to look up.</param>
        /// <returns>The area name, or "Unknown" if not found.</returns>
        private static string GetAreaForScene(string sceneName)
        {
            return sceneToAreaMap.TryGetValue(sceneName, out string area) ? area : "Unknown";
        }

        /// <summary>
        /// Groups grub scenes by area.
        /// </summary>
        /// <returns>Dictionary mapping area names to lists of grub scenes in that area.</returns>
        private static Dictionary<string, List<string>> GroupGrubsByArea()
        {
            var groupedGrubs = new Dictionary<string, List<string>>();

            foreach (string scene in grubScenes)
            {
                string area = GetAreaForScene(scene);
                if (!groupedGrubs.ContainsKey(area))
                {
                    groupedGrubs[area] = new List<string>();
                }
                groupedGrubs[area].Add(scene);
            }

            return groupedGrubs;
        }

        /// <summary>
        /// Adds grub-related panels to the mod menu for all grub locations, grouped by area.
        /// </summary>
        public static void AddPanels()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Grubs Found").SetColor(CheatPanel.headerColor));

            var groupedGrubs = GroupGrubsByArea();
            
            // Sort areas alphabetically for consistent ordering
            var sortedAreas = groupedGrubs.Keys.OrderBy(area => area).ToList();

            foreach (string area in sortedAreas)
            {
                // Add subheader for the area
                CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Area: " + area).SetColor(CheatPanel.subHeaderColor));
                
                // Add grub panels for this area
                foreach (string grubScene in groupedGrubs[area])
                {
                    CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new GrubPatch(grubScene), grubScene));
                }
            }
        }
    }
}
