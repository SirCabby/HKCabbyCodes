using System.Collections.Generic;
using CabbyCodes.Flags.FlagTypes;

namespace CabbyCodes.Flags
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
            { 1, new CharmInfo(1, "Gathering Swarm", CharmGot.Gotcharm1, CharmCost.Charmcost1) },
            { 2, new CharmInfo(2, "Wayward Compass", CharmGot.Gotcharm2, CharmCost.Charmcost2) },
            { 3, new CharmInfo(3, "Grubsong", CharmGot.Gotcharm3, CharmCost.Charmcost3) },
            { 4, new CharmInfo(4, "Stalward Shell", CharmGot.Gotcharm4, CharmCost.Charmcost4) },
            { 5, new CharmInfo(5, "Baldur Shell", CharmGot.Gotcharm5, CharmCost.Charmcost5) },
            { 6, new CharmInfo(6, "Fury of the Fallen", CharmGot.Gotcharm6, CharmCost.Charmcost6) },
            { 7, new CharmInfo(7, "Quick Focus", CharmGot.Gotcharm7, CharmCost.Charmcost7) },
            { 8, new CharmInfo(8, "Lifeblood Heart", CharmGot.Gotcharm8, CharmCost.Charmcost8) },
            { 9, new CharmInfo(9, "Lifeblood Core", CharmGot.Gotcharm9, CharmCost.Charmcost9) },
            { 10, new CharmInfo(10, "Defender's Crest", CharmGot.Gotcharm10, CharmCost.Charmcost10) },
            { 11, new CharmInfo(11, "Flukenest", CharmGot.Gotcharm11, CharmCost.Charmcost11) },
            { 12, new CharmInfo(12, "Thorns of Agony", CharmGot.Gotcharm12, CharmCost.Charmcost12) },
            { 13, new CharmInfo(13, "Mark of Pride", CharmGot.Gotcharm13, CharmCost.Charmcost13) },
            { 14, new CharmInfo(14, "Steady Body", CharmGot.Gotcharm14, CharmCost.Charmcost14) },
            { 15, new CharmInfo(15, "Heavy Blow", CharmGot.Gotcharm15, CharmCost.Charmcost15) },
            { 16, new CharmInfo(16, "Sharp Shadow", CharmGot.Gotcharm16, CharmCost.Charmcost16) },
            { 17, new CharmInfo(17, "Spore Shroom", CharmGot.Gotcharm17, CharmCost.Charmcost17) },
            { 18, new CharmInfo(18, "Longnail", CharmGot.Gotcharm18, CharmCost.Charmcost18) },
            { 19, new CharmInfo(19, "Shaman Stone", CharmGot.Gotcharm19, CharmCost.Charmcost19) },
            { 20, new CharmInfo(20, "Soul Catcher", CharmGot.Gotcharm20, CharmCost.Charmcost20) },
            { 21, new CharmInfo(21, "Soul Eater", CharmGot.Gotcharm21, CharmCost.Charmcost21) },
            { 22, new CharmInfo(22, "Glowing Womb", CharmGot.Gotcharm22, CharmCost.Charmcost22) },
            { 23, new CharmInfo(23, "Fragile Heart", CharmGot.Gotcharm23, CharmCost.Charmcost23, 
                               CharmStatus.Brokencharm23, CharmUpgrade.FragileHealthUnbreakable) },
            { 24, new CharmInfo(24, "Fragile Greed", CharmGot.Gotcharm24, CharmCost.Charmcost24, 
                               CharmStatus.Brokencharm24, CharmUpgrade.FragileGreedUnbreakable) },
            { 25, new CharmInfo(25, "Fragile Strength", CharmGot.Gotcharm25, CharmCost.Charmcost25, 
                               CharmStatus.Brokencharm25, CharmUpgrade.FragileStrengthUnbreakable) },
            { 26, new CharmInfo(26, "Nailmaster's Glory", CharmGot.Gotcharm26, CharmCost.Charmcost26) },
            { 27, new CharmInfo(27, "Joni's Blessing", CharmGot.Gotcharm27, CharmCost.Charmcost27) },
            { 28, new CharmInfo(28, "Shape of Unn", CharmGot.Gotcharm28, CharmCost.Charmcost28) },
            { 29, new CharmInfo(29, "Hiveblood", CharmGot.Gotcharm29, CharmCost.Charmcost29) },
            { 30, new CharmInfo(30, "Dream Wielder", CharmGot.Gotcharm30, CharmCost.Charmcost30) },
            { 31, new CharmInfo(31, "Dashmaster", CharmGot.Gotcharm31, CharmCost.Charmcost31) },
            { 32, new CharmInfo(32, "Quickslash", CharmGot.Gotcharm32, CharmCost.Charmcost32) },
            { 33, new CharmInfo(33, "Spell Twister", CharmGot.Gotcharm33, CharmCost.Charmcost33) },
            { 34, new CharmInfo(34, "Deep Focus", CharmGot.Gotcharm34, CharmCost.Charmcost34) },
            { 35, new CharmInfo(35, "Grubberfly's Elegy", CharmGot.Gotcharm35, CharmCost.Charmcost35) },
            { 36, new CharmInfo(36, "Kingsoul / Void Heart", CharmGot.Gotcharm36, CharmCost.Charmcost36) },
            { 37, new CharmInfo(37, "Sprintmaster", CharmGot.Gotcharm37, CharmCost.Charmcost37) },
            { 38, new CharmInfo(38, "Dreamshield", CharmGot.Gotcharm38, CharmCost.Charmcost38) },
            { 39, new CharmInfo(39, "Weaversong", CharmGot.Gotcharm39, CharmCost.Charmcost39) },
            { 40, new CharmInfo(40, "Grimmchild", CharmGot.Gotcharm40, CharmCost.Charmcost40) }
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