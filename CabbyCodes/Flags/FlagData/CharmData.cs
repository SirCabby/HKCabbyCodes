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
        public static readonly Dictionary<int, CharmInfo> AllCharms = new Dictionary<int, CharmInfo>
        {
            { 1, new CharmInfo(1, "Gathering Swarm", FlagInstances.gotCharm_1, FlagInstances.charmCost_1) },
            { 2, new CharmInfo(2, "Wayward Compass", FlagInstances.gotCharm_2, FlagInstances.charmCost_2) },
            { 3, new CharmInfo(3, "Grubsong", FlagInstances.gotCharm_3, FlagInstances.charmCost_3) },
            { 4, new CharmInfo(4, "Stalward Shell", FlagInstances.gotCharm_4, FlagInstances.charmCost_4) },
            { 5, new CharmInfo(5, "Baldur Shell", FlagInstances.gotCharm_5, FlagInstances.charmCost_5) },
            { 6, new CharmInfo(6, "Fury of the Fallen", FlagInstances.gotCharm_6, FlagInstances.charmCost_6) },
            { 7, new CharmInfo(7, "Quick Focus", FlagInstances.gotCharm_7, FlagInstances.charmCost_7) },
            { 8, new CharmInfo(8, "Lifeblood Heart", FlagInstances.gotCharm_8, FlagInstances.charmCost_8) },
            { 9, new CharmInfo(9, "Lifeblood Core", FlagInstances.gotCharm_9, FlagInstances.charmCost_9) },
            { 10, new CharmInfo(10, "Defender's Crest", FlagInstances.gotCharm_10, FlagInstances.charmCost_10) },
            { 11, new CharmInfo(11, "Flukenest", FlagInstances.gotCharm_11, FlagInstances.charmCost_11) },
            { 12, new CharmInfo(12, "Thorns of Agony", FlagInstances.gotCharm_12, FlagInstances.charmCost_12) },
            { 13, new CharmInfo(13, "Mark of Pride", FlagInstances.gotCharm_13, FlagInstances.charmCost_13) },
            { 14, new CharmInfo(14, "Steady Body", FlagInstances.gotCharm_14, FlagInstances.charmCost_14) },
            { 15, new CharmInfo(15, "Heavy Blow", FlagInstances.gotCharm_15, FlagInstances.charmCost_15) },
            { 16, new CharmInfo(16, "Sharp Shadow", FlagInstances.gotCharm_16, FlagInstances.charmCost_16) },
            { 17, new CharmInfo(17, "Spore Shroom", FlagInstances.gotCharm_17, FlagInstances.charmCost_17) },
            { 18, new CharmInfo(18, "Longnail", FlagInstances.gotCharm_18, FlagInstances.charmCost_18) },
            { 19, new CharmInfo(19, "Shaman Stone", FlagInstances.gotCharm_19, FlagInstances.charmCost_19) },
            { 20, new CharmInfo(20, "Soul Catcher", FlagInstances.gotCharm_20, FlagInstances.charmCost_20) },
            { 21, new CharmInfo(21, "Soul Eater", FlagInstances.gotCharm_21, FlagInstances.charmCost_21) },
            { 22, new CharmInfo(22, "Glowing Womb", FlagInstances.gotCharm_22, FlagInstances.charmCost_22) },
            { 23, new CharmInfo(23, "Fragile Heart", FlagInstances.gotCharm_23, FlagInstances.charmCost_23, 
                               FlagInstances.brokenCharm_23, FlagInstances.fragileHealth_unbreakable) },
            { 24, new CharmInfo(24, "Fragile Greed", FlagInstances.gotCharm_24, FlagInstances.charmCost_24, 
                               FlagInstances.brokenCharm_24, FlagInstances.fragileGreed_unbreakable) },
            { 25, new CharmInfo(25, "Fragile Strength", FlagInstances.gotCharm_25, FlagInstances.charmCost_25, 
                               FlagInstances.brokenCharm_25, FlagInstances.fragileStrength_unbreakable) },
            { 26, new CharmInfo(26, "Nailmaster's Glory", FlagInstances.gotCharm_26, FlagInstances.charmCost_26) },
            { 27, new CharmInfo(27, "Joni's Blessing", FlagInstances.gotCharm_27, FlagInstances.charmCost_27) },
            { 28, new CharmInfo(28, "Shape of Unn", FlagInstances.gotCharm_28, FlagInstances.charmCost_28) },
            { 29, new CharmInfo(29, "Hiveblood", FlagInstances.gotCharm_29, FlagInstances.charmCost_29) },
            { 30, new CharmInfo(30, "Dream Wielder", FlagInstances.gotCharm_30, FlagInstances.charmCost_30) },
            { 31, new CharmInfo(31, "Dashmaster", FlagInstances.gotCharm_31, FlagInstances.charmCost_31) },
            { 32, new CharmInfo(32, "Quickslash", FlagInstances.gotCharm_32, FlagInstances.charmCost_32) },
            { 33, new CharmInfo(33, "Spell Twister", FlagInstances.gotCharm_33, FlagInstances.charmCost_33) },
            { 34, new CharmInfo(34, "Deep Focus", FlagInstances.gotCharm_34, FlagInstances.charmCost_34) },
            { 35, new CharmInfo(35, "Grubberfly's Elegy", FlagInstances.gotCharm_35, FlagInstances.charmCost_35) },
            { 36, new CharmInfo(36, "Kingsoul / Void Heart", FlagInstances.gotCharm_36, FlagInstances.charmCost_36) },
            { 37, new CharmInfo(37, "Sprintmaster", FlagInstances.gotCharm_37, FlagInstances.charmCost_37) },
            { 38, new CharmInfo(38, "Dreamshield", FlagInstances.gotCharm_38, FlagInstances.charmCost_38) },
            { 39, new CharmInfo(39, "Weaversong", FlagInstances.gotCharm_39, FlagInstances.charmCost_39) },
            { 40, new CharmInfo(40, "Grimmchild", FlagInstances.gotCharm_40, FlagInstances.charmCost_40) }
        };

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
        /// Gets all charms as a list, ordered by ID.
        /// </summary>
        /// <returns>List of all charms</returns>
        public static List<CharmInfo> GetAllCharms()
        {
            var result = new List<CharmInfo>();
            for (int i = 1; i <= 40; i++)
            {
                if (AllCharms.ContainsKey(i))
                {
                    result.Add(AllCharms[i]);
                }
            }
            return result;
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