using CabbyCodes.SyncedReferences;
using CabbyCodes.Types;
using CabbyCodes.UI.CheatPanels;
using System.Collections.Generic;

namespace CabbyCodes.Patches
{
    public class CharmPatch : ISyncedReference<bool>
    {
        private static readonly string gotCharmName = "gotCharm_";
        public static readonly List<Charm> charms = BuildCharmList();
        
        private readonly int charmIndex;
        private TogglePanel parent;

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
            parent?.Update();
        }

        public void SetCheatPanel(TogglePanel parent)
        {
            this.parent = parent;
        }

        private static void AddCharmPanels()
        {
            for (int i = 0; i < charms.Count; i++)
            {
                Charm charm = charms[i];
                CharmPatch patch = new(charm.id);

                int index = i + 1;
                TogglePanel togglePanel = new(patch, index + ": " + charm.name);
                PanelAdder.AddSprite(togglePanel, CharmIconList.Instance.GetSprite(charm.id), () => { return PlayerData.instance.GetBool(gotCharmName + charm.id); }, 1);
                patch.SetCheatPanel(togglePanel);
                
                CabbyCodesPlugin.cabbyMenu.AddCheatPanel(togglePanel);
            }
        }

        private static List<Charm> BuildCharmList()
        {
            List<Charm> result = new()
            {
                new( 2, "Wayward Compass"),
                new( 1, "Gathering Swarm"),
                new( 4, "Stalward Shell"),
                new(20, "Soul Catcher"),
                new(19, "Shaman Stone"),
                new(21, "Soul Eater"),
                new(31, "Dashmaster"),
                new(37, "Sprintmaster"),
                new( 3, "Grubsong"),
                new(35, "Grubberfly's Elegy"),

                new(23, "Fragile Heart"),
                new(24, "Fragile Greed"),
                new(25, "Fragile Strength"),
                new(33, "Spell Twister"),
                new(14, "Steady Body"),
                new(15, "Heavy Blow"),
                new(32, "Quickslash"),
                new(18, "Longnail"),
                new(13, "Mark of Pride"),
                new( 6, "Fury of the Fallen"),

                new(12, "Thorns of Agony"),
                new( 5, "Baldur Shell"),
                new(11, "Flukenest"),
                new(10, "Defender's Crest"),
                new(22, "Glowing Womb"),
                new( 7, "Quick Focus"),
                new(34, "Deep Focus"),
                new( 8, "Lifeblood Heart"),
                new( 9, "Lifeblood Core"),
                new(27, "Joni's Blessing"),

                new(29, "Hiveblood"),
                new(17, "Spore Shroom"),
                new(16, "Sharp Shadow"),
                new(28, "Shape of Unn"),
                new(26, "Nailmaster's Glory"),
                new(39, "Weaversong"),
                new(30, "Dream Wielder"),
                new(38, "Dreamshield"),
                new(40, "Grimmchild"),
                new(36, "<Unknown>")
            };

            return result;
        }

        public static void AddPanels()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Charms:").SetColor(CheatPanel.headerColor));
            CharmCostPatch.AddPanel();
            GrimmChildLevelPatch.AddPanel();
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Toggle ON to Have Charm").SetColor(CheatPanel.headerColor));
            AddCharmPanels();
        }
    }
}
