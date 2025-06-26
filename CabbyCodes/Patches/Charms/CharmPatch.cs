using CabbyCodes.Patches.Charms;
using CabbyMenu.SyncedReferences;
using CabbyMenu.Types;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.UI.Modders;
using System.Collections.Generic;
using UnityEngine;
using CabbyCodes.Types;

namespace CabbyCodes.Patches
{
    public class CharmPatch : ISyncedReference<bool>
    {
        private static readonly string gotCharmName = "gotCharm_";
        private static readonly Color unearnedColor = new(0.57f, 0.57f, 0.57f, 0.57f);
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
                CharmPatch patch = new(charm.id);

                int index = i + 1;
                TogglePanel togglePanel = new(patch, index + ": " + charm.name);
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
                new(40, "Grimmchild")
                //new(36, "Kingsoul / Void Heart")
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
