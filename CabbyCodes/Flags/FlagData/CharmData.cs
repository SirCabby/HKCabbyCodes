using System.Collections.Generic;
using CabbyCodes.Flags.FlagInfo;

namespace CabbyCodes.Flags.FlagData
{
    /// <summary>
    /// Centralized charm data structure providing access to all charm properties.
    /// </summary>
    public static class CharmData
    {

        /// <summary>
        /// All charms in the game with their properties.
        /// </summary>
        public static readonly Dictionary<int, CharmInfo> AllCharms = CreateAllCharms();

        /// <summary>
        /// Creates the all charms dictionary
        /// </summary>
        private static Dictionary<int, CharmInfo> CreateAllCharms()
        {
            var charms = new Dictionary<int, CharmInfo>();
            
            // Helper method to add a charm with optional broken, upgrade, gave, and pooed flags
            void Add(int id, FlagDef gotFlag, FlagDef costFlag, FlagDef brokenFlag = null, FlagDef upgradeFlag = null, FlagDef gaveFlag = null, FlagDef pooedFlag = null, List<FlagDef> associatedFlags = null) 
                => charms[id] = new CharmInfo(id, gotFlag, costFlag, brokenFlag, upgradeFlag, gaveFlag, pooedFlag, associatedFlags);
            
            // Charm 1-22: Standard charms
            Add(1, FlagInstances.gotCharm_1, FlagInstances.charmCost_1);
            Add(2, FlagInstances.gotCharm_2, FlagInstances.charmCost_2);
            Add(3, FlagInstances.gotCharm_3, FlagInstances.charmCost_3, null, null, null, null, new List<FlagDef>() { FlagInstances.Crossroads_38__Reward_10, FlagInstances.Crossroads_38__Shiny_Item_Grubsong });
            Add(4, FlagInstances.gotCharm_4, FlagInstances.charmCost_4);
            Add(5, FlagInstances.gotCharm_5, FlagInstances.charmCost_5);
            Add(6, FlagInstances.gotCharm_6, FlagInstances.charmCost_6);
            Add(7, FlagInstances.gotCharm_7, FlagInstances.charmCost_7);
            Add(8, FlagInstances.gotCharm_8, FlagInstances.charmCost_8);
            Add(9, FlagInstances.gotCharm_9, FlagInstances.charmCost_9);
            Add(10, FlagInstances.gotCharm_10, FlagInstances.charmCost_10);
            Add(11, FlagInstances.gotCharm_11, FlagInstances.charmCost_11);
            Add(12, FlagInstances.gotCharm_12, FlagInstances.charmCost_12);
            Add(13, FlagInstances.gotCharm_13, FlagInstances.charmCost_13);
            Add(14, FlagInstances.gotCharm_14, FlagInstances.charmCost_14);
            Add(15, FlagInstances.gotCharm_15, FlagInstances.charmCost_15);
            Add(16, FlagInstances.gotCharm_16, FlagInstances.charmCost_16);
            Add(17, FlagInstances.gotCharm_17, FlagInstances.charmCost_17);
            Add(18, FlagInstances.gotCharm_18, FlagInstances.charmCost_18);
            Add(19, FlagInstances.gotCharm_19, FlagInstances.charmCost_19);
            Add(20, FlagInstances.gotCharm_20, FlagInstances.charmCost_20);
            Add(21, FlagInstances.gotCharm_21, FlagInstances.charmCost_21);
            Add(22, FlagInstances.gotCharm_22, FlagInstances.charmCost_22);
            
            // Charm 23-25: Fragile charms (can be broken, upgraded, given to Divine, or pooed)
            Add(23, FlagInstances.gotCharm_23, FlagInstances.charmCost_23, 
                FlagInstances.brokenCharm_23, FlagInstances.fragileHealth_unbreakable,
                FlagInstances.gaveFragileHeart, FlagInstances.pooedFragileHeart);
            Add(24, FlagInstances.gotCharm_24, FlagInstances.charmCost_24, 
                FlagInstances.brokenCharm_24, FlagInstances.fragileGreed_unbreakable,
                FlagInstances.gaveFragileGreed, FlagInstances.pooedFragileGreed);
            Add(25, FlagInstances.gotCharm_25, FlagInstances.charmCost_25, 
                FlagInstances.brokenCharm_25, FlagInstances.fragileStrength_unbreakable,
                FlagInstances.gaveFragileStrength, FlagInstances.pooedFragileStrength);
            
            // Charm 26-40: Standard charms
            Add(26, FlagInstances.gotCharm_26, FlagInstances.charmCost_26);
            Add(27, FlagInstances.gotCharm_27, FlagInstances.charmCost_27);
            Add(28, FlagInstances.gotCharm_28, FlagInstances.charmCost_28);
            Add(29, FlagInstances.gotCharm_29, FlagInstances.charmCost_29);
            Add(30, FlagInstances.gotCharm_30, FlagInstances.charmCost_30);
            Add(31, FlagInstances.gotCharm_31, FlagInstances.charmCost_31);
            Add(32, FlagInstances.gotCharm_32, FlagInstances.charmCost_32);
            Add(33, FlagInstances.gotCharm_33, FlagInstances.charmCost_33);
            Add(34, FlagInstances.gotCharm_34, FlagInstances.charmCost_34);
            Add(35, FlagInstances.gotCharm_35, FlagInstances.charmCost_35);
            Add(36, FlagInstances.gotCharm_36, FlagInstances.charmCost_36);
            Add(37, FlagInstances.gotCharm_37, FlagInstances.charmCost_37);
            Add(38, FlagInstances.gotCharm_38, FlagInstances.charmCost_38);
            Add(39, FlagInstances.gotCharm_39, FlagInstances.charmCost_39);
            Add(40, FlagInstances.gotCharm_40, FlagInstances.charmCost_40);
            
            return charms;
        }

        /// <summary>
        /// Gets charm information by ID.
        /// </summary>
        /// <param name="charmId">The charm ID (1-40)</param>
        /// <returns>The charm information</returns>
        public static CharmInfo GetCharm(int charmId)
        {
            if (AllCharms.TryGetValue(charmId, out CharmInfo charm))
            {
                return charm;
            }
            throw new System.ArgumentOutOfRangeException(nameof(charmId), $"Charm ID {charmId} does not exist");
        }

        /// <summary>
        /// Gets all charms as a list, ordered by game menu order
        /// </summary>
        /// <returns>List of all charms</returns>
        public static List<CharmInfo> GetAllCharms()
        {
            return new List<CharmInfo>()
            {
                // 1
                AllCharms[2],
                AllCharms[1],
                AllCharms[4],
                AllCharms[20],
                AllCharms[19],
                AllCharms[21],
                AllCharms[31],
                AllCharms[37],
                AllCharms[3],
                AllCharms[35],
                // 11
                AllCharms[23],
                AllCharms[24],
                AllCharms[25],
                AllCharms[33],
                AllCharms[14],
                AllCharms[15],
                AllCharms[32],
                AllCharms[18],
                AllCharms[13],
                AllCharms[6],
                // 21
                AllCharms[12],
                AllCharms[5],
                AllCharms[11],
                AllCharms[10],
                AllCharms[22],
                AllCharms[7],
                AllCharms[34],
                AllCharms[8],
                AllCharms[9],
                AllCharms[27],
                // 31
                AllCharms[29],
                AllCharms[17],
                AllCharms[16],
                AllCharms[28],
                AllCharms[26],
                AllCharms[39],
                AllCharms[30],
                AllCharms[38],
                AllCharms[40],
                AllCharms[36],
            };
        }

        /// <summary>
        /// Gets all charms that can be broken (Fragile charms).
        /// </summary>
        /// <returns>List of breakable charms</returns>
        public static List<CharmInfo> GetBreakableCharms()
        {
            var result = new List<CharmInfo>();
            foreach (var charm in AllCharms.Values)
            {
                if (charm.CanBeBroken)
                {
                    result.Add(charm);
                }
            }
            return result;
        }

        /// <summary>
        /// Gets all charms that can be upgraded (Fragile charms).
        /// </summary>
        /// <returns>List of upgradeable charms</returns>
        public static List<CharmInfo> GetUpgradeableCharms()
        {
            var result = new List<CharmInfo>();
            foreach (var charm in AllCharms.Values)
            {
                if (charm.CanBeUpgraded)
                {
                    result.Add(charm);
                }
            }
            return result;
        }
    }
} 