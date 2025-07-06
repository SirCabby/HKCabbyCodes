using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.UI.Modders;
using System.Collections.Generic;
using UnityEngine;

namespace CabbyCodes.Patches.Charms
{
    public class CharmPatch : ISyncedReference<bool>
    {
        private static readonly string gotCharmName = "gotCharm_";
        private static readonly Color unearnedColor = CabbyMenu.Constants.UNEARNED_COLOR;
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

        public static Sprite GetCharmIcon(int id)
        {
            PlayerData playerData = PlayerData.instance;
            if (id == 23)
            {
                if (playerData.fragileHealth_unbreakable)
                {
                    return CharmIconList.Instance.unbreakableHeart;
                }
            }
            else if (id == 24)
            {
                if (playerData.fragileGreed_unbreakable)
                {
                    return CharmIconList.Instance.unbreakableGreed;
                }
            }
            else if (id == 25)
            {
                if (playerData.fragileStrength_unbreakable)
                {
                    return CharmIconList.Instance.unbreakableStrength;
                }
            }
            else if (id == 40)
            {
                if (playerData.grimmChildLevel == 1)
                {
                    return CharmIconList.Instance.grimmchildLevel1;
                }
                if (playerData.grimmChildLevel == 2)
                {
                    return CharmIconList.Instance.grimmchildLevel2;
                }
                if (playerData.grimmChildLevel == 3)
                {
                    return CharmIconList.Instance.grimmchildLevel3;
                }
                if (playerData.grimmChildLevel == 4)
                {
                    return CharmIconList.Instance.grimmchildLevel4;
                }
                if (playerData.grimmChildLevel == 5)
                {
                    return CharmIconList.Instance.nymmCharm;
                }
            }
            return CharmIconList.Instance.spriteList[id];
        }

        private static void AddCharmPanels()
        {
            for (int i = 0; i < charms.Count; i++)
            {
                Charm charm = charms[i];
                CharmPatch patch = new CharmPatch(charm.id);

                int index = i + 1;
                TogglePanel togglePanel = new TogglePanel(patch, index + ": " + charm.name);
                (_, ImageMod spriteImageMod) = PanelAdder.AddSprite(togglePanel, CharmIconList.Instance.GetSprite(charm.id), 1);
                patch.SetCheatPanel(togglePanel);

                togglePanel.updateActions.Add(() =>
                {
                    spriteImageMod.SetSprite(GetCharmIcon(charm.id));
                    if (PlayerData.instance.GetBool(gotCharmName + charm.id))
                    {
                        if (charm.id == 40 && PlayerData.instance.royalCharmState < 3)
                        {
                            PlayerData.instance.royalCharmState = 3;
                        }
                        spriteImageMod.SetColor(Color.white);
                    }
                    else
                    {
                        if (charm.id == 40 && PlayerData.instance.royalCharmState > 0)
                        {
                            PlayerData.instance.royalCharmState = 0;
                        }
                        spriteImageMod.SetColor(unearnedColor);
                    }
                });
                togglePanel.Update();

                CabbyCodesPlugin.cabbyMenu.AddCheatPanel(togglePanel);
            }
        }

        private static List<Charm> BuildCharmList()
        {
            List<Charm> result = new List<Charm>
            {
                new Charm( 2, "Wayward Compass"),
                new Charm( 1, "Gathering Swarm"),
                new Charm( 4, "Stalward Shell"),
                new Charm(20, "Soul Catcher"),
                new Charm(19, "Shaman Stone"),
                new Charm(21, "Soul Eater"),
                new Charm(31, "Dashmaster"),
                new Charm(37, "Sprintmaster"),
                new Charm( 3, "Grubsong"),
                new Charm(35, "Grubberfly's Elegy"),
                new Charm(23, "Fragile Heart"),
                new Charm(24, "Fragile Greed"),
                new Charm(25, "Fragile Strength"),
                new Charm(33, "Spell Twister"),
                new Charm(14, "Steady Body"),
                new Charm(15, "Heavy Blow"),
                new Charm(32, "Quickslash"),
                new Charm(18, "Longnail"),
                new Charm(13, "Mark of Pride"),
                new Charm( 6, "Fury of the Fallen"),
                new Charm(12, "Thorns of Agony"),
                new Charm( 5, "Baldur Shell"),
                new Charm(11, "Flukenest"),
                new Charm(10, "Defender's Crest"),
                new Charm(22, "Glowing Womb"),
                new Charm( 7, "Quick Focus"),
                new Charm(34, "Deep Focus"),
                new Charm( 8, "Lifeblood Heart"),
                new Charm( 9, "Lifeblood Core"),
                new Charm(27, "Joni's Blessing"),
                new Charm(29, "Hiveblood"),
                new Charm(17, "Spore Shroom"),
                new Charm(16, "Sharp Shadow"),
                new Charm(28, "Shape of Unn"),
                new Charm(26, "Nailmaster's Glory"),
                new Charm(39, "Weaversong"),
                new Charm(30, "Dream Wielder"),
                new Charm(38, "Dreamshield"),
                new Charm(40, "Grimmchild")
                //new Charm(36, "Kingsoul / Void Heart")
            };

            return result;
        }

        public static void AddPanels()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Charms:").SetColor(CheatPanel.headerColor));
            NotchPatch.AddPanel();
            CharmCostPatch.AddPanel();
            GrimmChildLevelPatch.AddPanel();
            RoyalCharmPatch.AddPanel();
            BrokenCharmPatch.AddPanels();
            UpgradeCharmPatch.AddPanels();
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Toggle ON to Have Charm").SetColor(CheatPanel.headerColor));
            AddCharmPanels();
        }
    }
}
