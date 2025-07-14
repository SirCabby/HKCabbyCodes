using System.Collections.Generic;

namespace CabbyCodes.Flags.FlagData
{
    /// <summary>
    /// Centralized scene flag data structure providing access to all flag information organized by scene.
    /// This class stores FlagDef objects per scene for efficient lookup.
    /// </summary>
    public static class SceneFlagData
    {
        /// <summary>
        /// Hardcoded scene flag data organized by scene name.
        /// Each scene contains a list of FlagDef objects with their associated flags
        /// </summary>
        private static readonly Dictionary<string, List<FlagDef>> FlagsByScene = new Dictionary<string, List<FlagDef>>
        {
            // Crossroads
            ["Crossroads_01"] = new List<FlagDef>
            {
                FlagInstances.Crossroads_01__Geo_Rock_2,
                FlagInstances.Crossroads_01__Secret_Mask,
                FlagInstances.Crossroads_01__Shiny_Item,
                FlagInstances.Crossroads_01__Zombie_Runner,
                FlagInstances.Crossroads_01__Zombie_Runner_1
            },
            ["Crossroads_03"] = new List<FlagDef>
            {
                FlagInstances.Crossroads_03__Break_Wall_2,
                FlagInstances.Crossroads_03__Grub_Bottle,
                FlagInstances.Crossroads_03__Toll_Gate_Switch
            },
            ["Crossroads_05"] = new List<FlagDef>
            {
                FlagInstances.Crossroads_05__Geo_Rock_1,
                FlagInstances.Crossroads_05__Grub_Bottle,
                FlagInstances.Crossroads_05__Zombie_Runner
            },
            ["Crossroads_07"] = new List<FlagDef>
            {
                FlagInstances.Crossroads_07__Breakable_Wall_Silhouette,
                FlagInstances.Crossroads_07__Dream_Plant,
                FlagInstances.Crossroads_07__Dream_Plant_Orb,
                FlagInstances.Crossroads_07__Dream_Plant_Orb_1,
                FlagInstances.Crossroads_07__Dream_Plant_Orb_10,
                FlagInstances.Crossroads_07__Dream_Plant_Orb_11,
                FlagInstances.Crossroads_07__Dream_Plant_Orb_12,
                FlagInstances.Crossroads_07__Dream_Plant_Orb_13,
                FlagInstances.Crossroads_07__Dream_Plant_Orb_14,
                FlagInstances.Crossroads_07__Dream_Plant_Orb_15,
                FlagInstances.Crossroads_07__Dream_Plant_Orb_16,
                FlagInstances.Crossroads_07__Dream_Plant_Orb_17,
                FlagInstances.Crossroads_07__Dream_Plant_Orb_18,
                FlagInstances.Crossroads_07__Dream_Plant_Orb_19,
                FlagInstances.Crossroads_07__Dream_Plant_Orb_2,
                FlagInstances.Crossroads_07__Dream_Plant_Orb_20,
                FlagInstances.Crossroads_07__Dream_Plant_Orb_21,
                FlagInstances.Crossroads_07__Dream_Plant_Orb_22,
                FlagInstances.Crossroads_07__Dream_Plant_Orb_23,
                FlagInstances.Crossroads_07__Dream_Plant_Orb_24,
                FlagInstances.Crossroads_07__Dream_Plant_Orb_25,
                FlagInstances.Crossroads_07__Dream_Plant_Orb_26,
                FlagInstances.Crossroads_07__Dream_Plant_Orb_27,
                FlagInstances.Crossroads_07__Dream_Plant_Orb_28,
                FlagInstances.Crossroads_07__Dream_Plant_Orb_3,
                FlagInstances.Crossroads_07__Dream_Plant_Orb_4,
                FlagInstances.Crossroads_07__Dream_Plant_Orb_5,
                FlagInstances.Crossroads_07__Dream_Plant_Orb_6,
                FlagInstances.Crossroads_07__Dream_Plant_Orb_7,
                FlagInstances.Crossroads_07__Dream_Plant_Orb_8,
                FlagInstances.Crossroads_07__Dream_Plant_Orb_9,
                FlagInstances.Crossroads_07__Geo_Rock_1,
                FlagInstances.Crossroads_07__Geo_Rock_1_1,
                FlagInstances.Crossroads_07__Geo_Rock_1_2,
                FlagInstances.Crossroads_07__Remasker,
                FlagInstances.Crossroads_07__Tute_Door_1
            },
            ["Crossroads_08"] = new List<FlagDef>
            {
                FlagInstances.Crossroads_08__Battle_Scene,
                FlagInstances.Crossroads_08__Break_Wall_2,
                FlagInstances.Crossroads_08__break_wall_masks,
                FlagInstances.Crossroads_08__Geo_Rock_1,
                FlagInstances.Crossroads_08__Geo_Rock_1_1,
                FlagInstances.Crossroads_08__Geo_Rock_1_2,
                FlagInstances.Crossroads_08__Geo_Rock_1_3
            },
            ["Crossroads_09"] = new List<FlagDef>
            {
                FlagInstances.Crossroads_09__Battle_Scene,
                FlagInstances.Crossroads_09__Break_Floor_1,
                FlagInstances.Crossroads_09__Mawlek_Body
            },
            ["Crossroads_11_alt"] = new List<FlagDef>
            {
                FlagInstances.Crossroads_11_alt__Battle_Scene,
                FlagInstances.Crossroads_11_alt__Blocker
            },
            ["Crossroads_12"] = new List<FlagDef>
            {
                FlagInstances.Crossroads_12__Geo_Rock_2
            },
            ["Crossroads_13"] = new List<FlagDef>
            {
                FlagInstances.Crossroads_13__Break_Floor_1,
                FlagInstances.Crossroads_13__Geo_Rock_1,
                FlagInstances.Crossroads_13__Geo_Rock_2,
                FlagInstances.Crossroads_13__Heart_Piece,
                FlagInstances.Crossroads_13__Zombie_Barger
            },
            ["Crossroads_16"] = new List<FlagDef>
            {
                FlagInstances.Crossroads_16__Geo_Rock_2,
                FlagInstances.Crossroads_16__Zombie_Hornhead
            },
            ["Crossroads_18"] = new List<FlagDef>
            {
                FlagInstances.Crossroads_18__Breakable_Wall_Waterways,
                FlagInstances.Crossroads_18__Geo_Rock_1,
                FlagInstances.Crossroads_18__Geo_Rock_2,
                FlagInstances.Crossroads_18__Geo_Rock_3,
                FlagInstances.Crossroads_18__Inverse_Remasker,
                FlagInstances.Crossroads_18__Soul_Totem_mini_horned
            },
            ["Crossroads_19"] = new List<FlagDef>
            {
                FlagInstances.Crossroads_19__Geo_Rock_1,
                FlagInstances.Crossroads_19__Hatcher,
                FlagInstances.Crossroads_19__Soul_Totem_mini_two_horned,
                FlagInstances.Crossroads_19__Zombie_Leaper
            },
            ["Crossroads_25"] = new List<FlagDef>
            {
                FlagInstances.Crossroads_25__Soul_Totem_mini_two_horned
            },
            ["Crossroads_31"] = new List<FlagDef>
            {
                FlagInstances.Crossroads_31__Grub_Bottle
            },
            ["Crossroads_35"] = new List<FlagDef>
            {
                FlagInstances.Crossroads_35__Grub_Bottle,
                FlagInstances.Crossroads_35__Hatcher,
                FlagInstances.Crossroads_35__Remasker,
                FlagInstances.Crossroads_35__Soul_Totem_mini_horned
            },
            ["Crossroads_36"] = new List<FlagDef>
            {
                FlagInstances.Crossroads_36__Collapser_Small,
                FlagInstances.Crossroads_36__Collapser_Small_1,
                FlagInstances.Crossroads_36__Force_Hard_Landing,
                FlagInstances.Crossroads_36__Geo_Rock_1,
                FlagInstances.Crossroads_36__Mask_Bottom,
                FlagInstances.Crossroads_36__Reminder_Look_Down,
                FlagInstances.Crossroads_36__Secret_Mask,
                FlagInstances.Crossroads_36__Soul_Totem_4
            },
            ["Crossroads_38"] = new List<FlagDef>
            {
                FlagInstances.Crossroads_38__Reward_10,
                FlagInstances.Crossroads_38__Reward_16,
                FlagInstances.Crossroads_38__Reward_23,
                FlagInstances.Crossroads_38__Reward_31,
                FlagInstances.Crossroads_38__Reward_38,
                FlagInstances.Crossroads_38__Reward_46,
                FlagInstances.Crossroads_38__Reward_5
            },
            ["Crossroads_39"] = new List<FlagDef>
            {
                FlagInstances.Crossroads_39__Zombie_Hornhead_1,
                FlagInstances.Crossroads_39__Zombie_Runner,
                FlagInstances.Crossroads_39__Zombie_Runner_1
            },
            ["Crossroads_40"] = new List<FlagDef>
            {
                FlagInstances.Crossroads_40__Zombie_Hornhead,
                FlagInstances.Crossroads_40__Zombie_Leaper,
                FlagInstances.Crossroads_40__Zombie_Leaper_1,
                FlagInstances.Crossroads_40__Zombie_Runner_2
            },
            ["Crossroads_42"] = new List<FlagDef>
            {
                FlagInstances.Crossroads_42__Geo_Rock_1,
                FlagInstances.Crossroads_42__Geo_Rock_2
            },
            ["Crossroads_45"] = new List<FlagDef>
            {
                FlagInstances.Crossroads_45__Soul_Totem_5,
                FlagInstances.Crossroads_45__Zombie_Myla
            },
            ["Crossroads_48"] = new List<FlagDef>
            {
                FlagInstances.Crossroads_48__Grub_Bottle,
                FlagInstances.Crossroads_48__Zombie_Guard
            },

            // Abyss
            ["Abyss_17"] = new List<FlagDef>
            {
                FlagInstances.Abyss_17__Grub_Bottle
            },
            ["Abyss_19"] = new List<FlagDef>
            {
                FlagInstances.Abyss_19__Grub_Bottle
            },

            // Deepnest
            ["Deepnest_03"] = new List<FlagDef>
            {
                FlagInstances.Deepnest_03__Grub_Bottle
            },
            ["Deepnest_31"] = new List<FlagDef>
            {
                FlagInstances.Deepnest_31__Grub_Bottle
            },
            ["Deepnest_36"] = new List<FlagDef>
            {
                FlagInstances.Deepnest_36__Grub_Bottle
            },
            ["Deepnest_39"] = new List<FlagDef>
            {
                FlagInstances.Deepnest_39__Grub_Bottle
            },
            ["Deepnest_East_11"] = new List<FlagDef>
            {
                FlagInstances.Deepnest_East_11__Grub_Bottle
            },
            ["Deepnest_East_14"] = new List<FlagDef>
            {
                FlagInstances.Deepnest_East_14__Grub_Bottle
            },
            ["Deepnest_Spider_Town"] = new List<FlagDef>
            {
                FlagInstances.Deepnest_Spider_Town__Grub_Bottle
            },

            // Fungus
            ["Fungus1_06"] = new List<FlagDef>
            {
                FlagInstances.Fungus1_06__Grub_Bottle
            },
            ["Fungus1_07"] = new List<FlagDef>
            {
                FlagInstances.Fungus1_07__Grub_Bottle
            },
            ["Fungus1_13"] = new List<FlagDef>
            {
                FlagInstances.Fungus1_13__Grub_Bottle
            },
            ["Fungus1_21"] = new List<FlagDef>
            {
                FlagInstances.Fungus1_21__Grub_Bottle
            },
            ["Fungus1_28"] = new List<FlagDef>
            {
                FlagInstances.Fungus1_28__Grub_Bottle
            },
            ["Fungus2_18"] = new List<FlagDef>
            {
                FlagInstances.Fungus2_18__Grub_Bottle
            },
            ["Fungus2_20"] = new List<FlagDef>
            {
                FlagInstances.Fungus2_20__Grub_Bottle
            },
            ["Fungus3_10"] = new List<FlagDef>
            {
                FlagInstances.Fungus3_10__Grub_Bottle
            },
            ["Fungus3_22"] = new List<FlagDef>
            {
                FlagInstances.Fungus3_22__Grub_Bottle
            },
            ["Fungus3_47"] = new List<FlagDef>
            {
                FlagInstances.Fungus3_47__Grub_Bottle
            },
            ["Fungus3_48"] = new List<FlagDef>
            {
                FlagInstances.Fungus3_48__Grub_Bottle
            },

            // Hive
            ["Hive_03"] = new List<FlagDef>
            {
                FlagInstances.Hive_03__Grub_Bottle
            },
            ["Hive_04"] = new List<FlagDef>
            {
                FlagInstances.Hive_04__Grub_Bottle
            },

            // Mines
            ["Mines_01"] = new List<FlagDef>
            {
                FlagInstances.Mines_01__Egg_Sac,
                FlagInstances.Mines_01__mine_1_quake_floor
            },
            ["Mines_03"] = new List<FlagDef>
            {
                FlagInstances.Mines_03__Grub_Bottle
            },
            ["Mines_04"] = new List<FlagDef>
            {
                FlagInstances.Mines_04__Grub_Bottle
            },
            ["Mines_16"] = new List<FlagDef>
            {
                FlagInstances.Mines_16__Grub_Bottle
            },
            ["Mines_19"] = new List<FlagDef>
            {
                FlagInstances.Mines_19__Grub_Bottle
            },
            ["Mines_24"] = new List<FlagDef>
            {
                FlagInstances.Mines_24__Grub_Bottle
            },
            ["Mines_31"] = new List<FlagDef>
            {
                FlagInstances.Mines_31__Grub_Bottle
            },
            ["Mines_35"] = new List<FlagDef>
            {
                FlagInstances.Mines_35__Grub_Bottle
            },

            // Resting Grounds
            ["RestingGrounds_10"] = new List<FlagDef>
            {
                FlagInstances.RestingGrounds_10__Grub_Bottle
            },

            // Ruins
            ["Ruins1_05"] = new List<FlagDef>
            {
                FlagInstances.Ruins1_05__Grub_Bottle
            },
            ["Ruins1_32"] = new List<FlagDef>
            {
                FlagInstances.Ruins1_32__Grub_Bottle
            },
            ["Ruins2_03"] = new List<FlagDef>
            {
                FlagInstances.Ruins2_03__Grub_Bottle
            },
            ["Ruins2_07"] = new List<FlagDef>
            {
                FlagInstances.Ruins2_07__Grub_Bottle
            },
            ["Ruins2_11"] = new List<FlagDef>
            {
                FlagInstances.Ruins2_11__Grub_Bottle
            },
            ["Ruins_House_01"] = new List<FlagDef>
            {
                FlagInstances.Ruins_House_01__Grub_Bottle
            },

            // Town
            ["Town"] = new List<FlagDef>
            {
                FlagInstances.Town__Death_Respawn_Trigger,
                FlagInstances.Town__Death_Respawn_Trigger_1,
                FlagInstances.Town__Door_Destroyer,
                FlagInstances.Town__Gravedigger_NPC,
                FlagInstances.Town__Interact_Reminder,
                FlagInstances.Town__Mines_Lever
            },

            // Tutorial
            ["Tutorial_01"] = new List<FlagDef>
            {
                FlagInstances.Tutorial_01__Break_Floor_1,
                FlagInstances.Tutorial_01__Chest,
                FlagInstances.Tutorial_01__Collapser_Tute_01,
                FlagInstances.Tutorial_01__Door,
                FlagInstances.Tutorial_01__fury_charm_remask,
                FlagInstances.Tutorial_01__Geo_Rock_1,
                FlagInstances.Tutorial_01__Geo_Rock_2,
                FlagInstances.Tutorial_01__Geo_Rock_3,
                FlagInstances.Tutorial_01__Geo_Rock_4,
                FlagInstances.Tutorial_01__Geo_Rock_5,
                FlagInstances.Tutorial_01__Health_Cocoon,
                FlagInstances.Tutorial_01__Initial_Fall_Impact,
                FlagInstances.Tutorial_01__Interact_Reminder,
                FlagInstances.Tutorial_01__Inverse_Remasker,
                FlagInstances.Tutorial_01__inverse_remask_right,
                FlagInstances.Tutorial_01__Secret_Mask,
                FlagInstances.Tutorial_01__Secret_Sound_Region,
                FlagInstances.Tutorial_01__Secret_Sound_Region_1,
                FlagInstances.Tutorial_01__Shiny_Item_1,
                FlagInstances.Tutorial_01__Tute_Door_1,
                FlagInstances.Tutorial_01__Tute_Door_2,
                FlagInstances.Tutorial_01__Tute_Door_3,
                FlagInstances.Tutorial_01__Tute_Door_4,
                FlagInstances.Tutorial_01__Tute_Door_5,
                FlagInstances.Tutorial_01__Tute_Door_6,
                FlagInstances.Tutorial_01__Tute_Door_7
            },

            // Waterways
            ["Waterways_04"] = new List<FlagDef>
            {
                FlagInstances.Waterways_04__Grub_Bottle
            },
            ["Waterways_13"] = new List<FlagDef>
            {
                FlagInstances.Waterways_13__Grub_Bottle
            },
            ["Waterways_14"] = new List<FlagDef>
            {
                FlagInstances.Waterways_14__Grub_Bottle
            }
        };

        /// <summary>
        /// Gets all flags for a specific scene.
        /// </summary>
        /// <param name="sceneName">The scene name to get flags for</param>
        /// <returns>List of FlagDef objects for the scene, or empty list if scene not found</returns>
        public static List<FlagDef> GetFlagsForScene(string sceneName)
        {
            if (FlagsByScene.TryGetValue(sceneName, out var flags))
            {
                return flags;
            }
            return new List<FlagDef>();
        }

        /// <summary>
        /// Gets the count of flags for a specific scene.
        /// </summary>
        /// <param name="sceneName">The scene name to get flag count for</param>
        /// <returns>The number of flags for the scene, or 0 if scene not found</returns>
        public static int GetFlagCountForScene(string sceneName)
        {
            if (FlagsByScene.TryGetValue(sceneName, out var flags))
            {
                return flags.Count;
            }
            return 0;
        }
    }
} 