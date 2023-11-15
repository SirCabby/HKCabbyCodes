using CabbyCodes.SyncedReferences;
using CabbyCodes.UI.CheatPanels;
using System.Collections.Generic;

namespace CabbyCodes.Patches
{
    public class CharmPatch : ISyncedReference<bool>
    {
        private static readonly string gotCharmName = "gotCharm_";
        private static readonly Dictionary<int, string> charmDict = BuildCharmDict();
        private static readonly List<int> charmOrder = new()
        {
            2, 1, 4, 20, 19, 21, 31, 37, 3, 35,
            23, 24, 25, 33, 14, 15, 32, 18, 13, 6,
            12, 5, 11, 10, 22, 7, 34, 8, 9, 27,
            29, 17, 16, 28, 26, 39, 30, 38, 40, 36
        };

        private readonly int charmIndex;

        public CharmPatch(int charmIndex)
        {
            this.charmIndex = charmIndex;
        }

        public bool Get()
        {
            return PlayerData.instance.GetBool(gotCharmName + charmIndex);
        }

        public void Set(bool value)
        {
            PlayerData.instance.SetBool(gotCharmName + charmIndex, value);
        }

        public static void AddPanels()
        {
            for (int i = 0; i < charmOrder.Count;)
            {
                int charmIndex = charmOrder[i];
                TogglePanel togglePanel = new(new CharmPatch(charmIndex), ++i + ": " + charmDict[charmIndex]);
                PanelAdder.AddSprite(togglePanel, CharmIconList.Instance.GetSprite(charmIndex), () => { return PlayerData.instance.GetBool(gotCharmName + charmIndex); }, 1);
                CabbyCodesPlugin.cabbyMenu.AddCheatPanel(togglePanel);
            }
        }

        private static Dictionary<int, string> BuildCharmDict()
        {
            Dictionary<int, string> result = new()
            {
                { 1, "Gathering Swarm" },
                { 2, "Wayward Compass" },
                { 3, "Grubsong" },
                { 4, "Stalward Shell" },
                { 5, "Baldur Shell" },
                { 6, "Fury of the Fallen" },
                { 7, "Quick Focus" },
                { 8, "Lifeblood Heart" },
                { 9, "Lifeblood Core" },
                { 10, "Defender's Crest" },
                { 11, "Flukenest" },
                { 12, "Thorns of Agony" },
                { 13, "Mark of Pride" },
                { 14, "Steady Body" },
                { 15, "Heavy Blow" },
                { 16, "Sharp Shadow" },
                { 17, "Spore Shroom" },
                { 18, "Longnail" },
                { 19, "Shaman Stone" },
                { 20, "Soul Catcher" },
                { 21, "Soul Eater" },
                { 22, "Glowing Womb" },
                { 23, "Fragile Heart" },
                { 24, "Fragile Greed" },
                { 25, "Fragile Strength" },
                { 26, "Nailmaster's Glory" },
                { 27, "Joni's Blessing" },
                { 28, "Shape of Unn" },
                { 29, "Hiveblood" },
                { 30, "Dream Wielder" },
                { 31, "Dashmaster" },
                { 32, "Quickslash" },
                { 33, "Spell Twister" },
                { 34, "Deep Focus" },
                { 35, "Grubberfly's Elegy" },
                { 36, "<Unknown>" },
                { 37, "Sprintmaster" },
                { 38, "Dreamshield" },
                { 39, "Weaversong" },
                { 40, "Grimmchild" }
            };

            return result;
        }
    }
}
