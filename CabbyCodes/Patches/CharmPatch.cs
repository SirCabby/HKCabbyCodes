using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.UI.Modders;
using System.Collections.Generic;
using UnityEngine;
using CabbyCodes.Flags.FlagInfo;
using CabbyCodes.Flags.FlagData;
using CabbyCodes.Flags;
using CabbyCodes.Patches.BasePatches;
using CabbyMenu;

namespace CabbyCodes.Patches
{
    public class CharmPatch
    {
        private static readonly Color unearnedColor = CabbyMenu.Constants.UNEARNED_COLOR;
        public static readonly List<CharmInfo> charms = CharmData.GetAllCharms();

        // Charm cost removal functionality
        public const string key = "CharmCost_Patch";
        private static readonly BoxedReference<bool> charmCostValue = CodeState.Get(key, false);

        public List<CheatPanel> CreatePanels()
        {
            var panels = new List<CheatPanel>
            {
                // Notch count panel
                new IntPatch(FlagInstances.charmSlots).CreatePanel(),

                // Charm cost removal panel
                new TogglePanel(new DelegateReference<bool>(
                    () => charmCostValue.Get(),
                    (value) =>
                    {
                        if (value)
                        {
                            foreach (var charm in charms)
                            {
                                FlagManager.SetIntFlag(charm.CostFlag, 0);
                            }
                        }
                        else
                        {
                            foreach (var charm in charms)
                            {
                                // Get the default cost from the game data
                                int defaultCost = FlagManager.GetIntFlag(charm.CostFlag);
                                FlagManager.SetIntFlag(charm.CostFlag, defaultCost);
                            }
                        }
                        charmCostValue.Set(value);
                    }
                ), "Remove Charm Notch Cost"),

                // Grimm Child level dropdown
                new DropdownPatch(FlagInstances.grimmChildLevel, new List<string> { "1", "2", "3", "4", "CM" }, "Grimm Child Level (1-4) or Carefree Melody").CreatePanel(),

                // Royal Charm state dropdown
                new DropdownPatch(FlagInstances.royalCharmState, new List<string> { "NONE", "Kingsoul", "Void Heart" }, "Kingsoul / Void Heart").CreatePanel()
            };
            
            // Broken charm panels
            var breakableCharms = CharmData.GetBreakableCharms();
            foreach (var charm in breakableCharms)
            {
                panels.Add(CreateBrokenCharmPanel(charm));
            }
            
            // Upgrade charm panels
            var upgradeableCharms = CharmData.GetUpgradeableCharms();
            foreach (var charm in upgradeableCharms)
            {
                panels.Add(CreateUpgradeCharmPanel(charm));
            }
            
            return panels;
        }

        public List<CheatPanel> CreateCharmTogglePanels()
        {
            var panels = new List<CheatPanel>();
            
            // Individual charm panels
            foreach (var charm in charms)
            {
                panels.Add(CreateCharmTogglePanel(charm));
            }
            
            return panels;
        }

        private CheatPanel CreateBrokenCharmPanel(CharmInfo charm)
        {
            int index = charms.IndexOf(charm) + 1;
            TogglePanel togglePanel = new TogglePanel(new BoolPatch(charm.BrokenFlag), index + ": " + charm.Name + " is Broken");
            (_, ImageMod spriteImageMod) = PanelAdder.AddSprite(togglePanel, CharmIconList.Instance.GetSprite(charm.Id), 1);

            togglePanel.updateActions.Add(() =>
            {
                spriteImageMod.SetSprite(GetCharmIcon(charm.Id));
                if (FlagManager.GetBoolFlag(charm.BrokenFlag))
                {
                    spriteImageMod.SetColor(Color.white);
                }
                else
                {
                    spriteImageMod.SetColor(unearnedColor);
                }
            });
            
            return togglePanel;
        }

        private CheatPanel CreateUpgradeCharmPanel(CharmInfo charm)
        {
            int index = charms.IndexOf(charm) + 1;
            TogglePanel togglePanel = new TogglePanel(new BoolPatch(charm.UpgradeFlag), index + ": " + charm.Name + " is Upgraded");
            (_, ImageMod spriteImageMod) = PanelAdder.AddSprite(togglePanel, CharmIconList.Instance.GetSprite(charm.Id), 1);

            togglePanel.updateActions.Add(() =>
            {
                spriteImageMod.SetSprite(GetCharmIcon(charm.Id));
                if (FlagManager.GetBoolFlag(charm.UpgradeFlag))
                {
                    spriteImageMod.SetColor(Color.white);
                }
                else
                {
                    spriteImageMod.SetColor(unearnedColor);
                }
            });
            
            return togglePanel;
        }

        private CheatPanel CreateCharmTogglePanel(CharmInfo charm)
        {
            int index = charms.IndexOf(charm) + 1;
            TogglePanel togglePanel = new TogglePanel(
                new DelegateReference<bool>(
                    () => FlagManager.GetBoolFlag(charm.GotFlag),
                    value =>
                    {
                        // Get current charm count before changing the flag
                        int currentCharmsOwned = FlagManager.GetIntFlag(FlagInstances.charmsOwned);
                        bool wasCharmOwned = FlagManager.GetBoolFlag(charm.GotFlag);
                        
                        // Set the charm flag
                        FlagManager.SetBoolFlag(charm.GotFlag, value);
                        
                        // Update charmsOwned count
                        if (value && !wasCharmOwned)
                        {
                            // Charm is being turned on - increment count
                            FlagManager.SetIntFlag(FlagInstances.charmsOwned, currentCharmsOwned + 1);
                            
                            // If this is the first charm, set hasCharm to true
                            if (currentCharmsOwned == 0)
                            {
                                FlagManager.SetBoolFlag(FlagInstances.hasCharm, true);
                            }
                        }
                        else if (!value && wasCharmOwned)
                        {
                            // Charm is being turned off - decrement count
                            FlagManager.SetIntFlag(FlagInstances.charmsOwned, currentCharmsOwned - 1);
                            
                            // If this was the last charm, set hasCharm to false
                            if (currentCharmsOwned == 1)
                            {
                                FlagManager.SetBoolFlag(FlagInstances.hasCharm, false);
                            }
                        }
                    }
                ), index + ": " + charm.Name);
            
            (_, ImageMod spriteImageMod) = PanelAdder.AddSprite(togglePanel, CharmIconList.Instance.GetSprite(charm.Id), 1);

            togglePanel.updateActions.Add(() =>
            {
                spriteImageMod.SetSprite(GetCharmIcon(charm.Id));
                if (FlagManager.GetBoolFlag(charm.GotFlag))
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
            
            return togglePanel;
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

        public static void AddPanels()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Charms:").SetColor(CheatPanel.headerColor));
            
            // Create the charm patch instance
            var charmPatch = new CharmPatch();
            var panels = charmPatch.CreatePanels();
            
            // Add all panels to the menu
            foreach (var panel in panels)
            {
                CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
            }
            
            // Add individual charm toggle panels
            var togglePanels = charmPatch.CreateCharmTogglePanels();
            foreach (var panel in togglePanels)
            {
                CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
            }
        }
    }
} 