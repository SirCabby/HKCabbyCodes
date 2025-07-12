using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.UI.Modders;
using System.Collections.Generic;
using UnityEngine;
using CabbyCodes.Flags.FlagInfo;
using CabbyCodes.Flags.FlagData;

namespace CabbyCodes.Patches.Charms
{
    public class CharmPatch : ISyncedReference<bool>
    {
        private static readonly Color unearnedColor = CabbyMenu.Constants.UNEARNED_COLOR;
        public static readonly List<CharmInfo> charms = CharmData.GetAllCharms();

        private readonly int charmIndex;
        private TogglePanel parent;

        public CharmPatch(int charmIndex)
        {
            this.charmIndex = charmIndex;
        }

        public bool Get()
        {
            var charm = CharmData.GetCharm(charmIndex);
            return PlayerData.instance.GetBool(charm.GotFlag.Id);
        }

        public void Set(bool value)
        {
            var charm = CharmData.GetCharm(charmIndex);
            PlayerData.instance.SetBool(charm.GotFlag.Id, value);
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
                var charm = charms[i];
                CharmPatch patch = new CharmPatch(charm.Id);

                int index = i + 1;
                TogglePanel togglePanel = new TogglePanel(patch, index + ": " + charm.Name);
                (_, ImageMod spriteImageMod) = PanelAdder.AddSprite(togglePanel, CharmIconList.Instance.GetSprite(charm.Id), 1);
                patch.SetCheatPanel(togglePanel);

                togglePanel.updateActions.Add(() =>
                {
                    spriteImageMod.SetSprite(GetCharmIcon(charm.Id));
                    if (PlayerData.instance.GetBool(charm.GotFlag.Id))
                    {
                        if (charm.Id == 40 && PlayerData.instance.royalCharmState < 3)
                        {
                            PlayerData.instance.royalCharmState = 3;
                        }
                        spriteImageMod.SetColor(Color.white);
                    }
                    else
                    {
                        if (charm.Id == 40 && PlayerData.instance.royalCharmState > 0)
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
