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
                new DropdownPatch(FlagInstances.grimmChildLevel, new List<string> { "1", "2", "3", "4", "CM" }, "Grimm Child Level (1-4) or Carefree Melody").CreatePanel()
            };
            
            // Fragile charm panels (replacing separate broken and upgraded panels)
            var fragileCharms = CharmData.GetBreakableCharms();
            foreach (var charm in fragileCharms)
            {
                panels.Add(CreateFragileCharmPanel(charm));
            }
            
            // Add subheader for individual charm toggles
            panels.Add(new InfoPanel("Charms Obtained").SetColor(CheatPanel.subHeaderColor));
            
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

        private CheatPanel CreateFragileCharmPanel(CharmInfo charm)
        {
            int index = charms.IndexOf(charm) + 1;
            
            // Create a custom dropdown for fragile charm states using DelegateValueList pattern
            var dropdownPanel = new DropdownPanel(new DelegateValueList(
                // Getter: 0 = Normal, 1 = Broken, 2 = Gave to Divine, 3 = Unbreakable
                () => GetFragileCharmState(charm),
                // Setter
                value => SetFragileCharmState(charm, value),
                // Value list
                () => new List<string> { "Normal", "Broken", "Gave to Divine", "Unbreakable" }
            ), index + ": " + charm.Name + " State", CabbyMenu.Constants.DEFAULT_PANEL_HEIGHT);
            
            (_, ImageMod spriteImageMod) = PanelAdder.AddSprite(dropdownPanel, CharmIconList.Instance.GetSprite(charm.Id), 1);

            dropdownPanel.updateActions.Add(() =>
            {
                spriteImageMod.SetSprite(GetCharmIcon(charm.Id));
                int currentState = GetFragileCharmState(charm);
                if (currentState == 0) // Normal
                {
                    spriteImageMod.SetColor(unearnedColor);
                }
                else // Broken, Unbreakable, or Gave to Divine
                {
                    spriteImageMod.SetColor(Color.white);
                }
            });
            
            return dropdownPanel;
        }

        private static int GetFragileCharmState(CharmInfo charm)
        {
            bool isBroken = FlagManager.GetBoolFlag(charm.BrokenFlag);
            bool isUpgraded = FlagManager.GetBoolFlag(charm.UpgradeFlag);
            bool isGave = FlagManager.GetBoolFlag(charm.GaveFlag);
            
            if (isUpgraded)
                return 3; // Unbreakable
            else if (isGave)
                return 2; // Gave to Divine
            else if (isBroken)
                return 1; // Broken
            else
                return 0; // Normal
        }

        private static void SetFragileCharmState(CharmInfo charm, int state)
        {
            bool isBroken = state == 1;
            bool isGave = state == 2;
            bool isUpgraded = state == 3;
            
            // Clear all flags first
            FlagManager.SetBoolFlag(charm.BrokenFlag, false);
            FlagManager.SetBoolFlag(charm.UpgradeFlag, false);
            FlagManager.SetBoolFlag(charm.GaveFlag, false);
            
            // Set the appropriate flag based on state
            if (isBroken)
                FlagManager.SetBoolFlag(charm.BrokenFlag, true);
            else if (isGave)
                FlagManager.SetBoolFlag(charm.GaveFlag, true);
            else if (isUpgraded)
                FlagManager.SetBoolFlag(charm.UpgradeFlag, true);
            
            // Update pooed status: true if unbreakable, false otherwise
            bool shouldBePooed = isUpgraded;
            FlagManager.SetBoolFlag(charm.PooedFlag, shouldBePooed);
        }

        private static CheatPanel CreateKingsoulCombinedPanel(CharmInfo charm, int index)
        {
            // Create dropdown panel with 5 states for Kingsoul/Void Heart
            var dropdownPanel = new DropdownPanel(new DelegateValueList(
                // Getter: Get current royal charm state
                () => PlayerData.instance.royalCharmState,
                // Setter: Handle the complex logic for setting both charm state and royal charm state
                value =>
                {
                    // Get current charm count before changing anything
                    int currentCharmsOwned = FlagManager.GetIntFlag(FlagInstances.charmsOwned);
                    bool wasCharmOwned = FlagManager.GetBoolFlag(charm.GotFlag);
                    
                    // Determine if charm should be owned based on state
                    bool shouldOwnCharm = value > 0;
                    
                    // Set the charm flag
                    FlagManager.SetBoolFlag(charm.GotFlag, shouldOwnCharm);
                    
                    // Set the royal charm state
                    PlayerData.instance.royalCharmState = value;
                    FlagManager.SetBoolFlag(FlagInstances.gotQueenFragment, value == 1 || value > 2);
                    FlagManager.SetBoolFlag(FlagInstances.gotKingFragment, value > 1);

                    // Update charmsOwned count
                    if (shouldOwnCharm && !wasCharmOwned)
                    {
                        // Charm is being turned on - increment count
                        FlagManager.SetIntFlag(FlagInstances.charmsOwned, currentCharmsOwned + 1);
                        
                        // If this is the first charm, set hasCharm to true
                        if (currentCharmsOwned == 0)
                        {
                            FlagManager.SetBoolFlag(FlagInstances.hasCharm, true);
                        }
                    }
                    else if (!shouldOwnCharm && wasCharmOwned)
                    {
                        // Charm is being turned off - decrement count
                        FlagManager.SetIntFlag(FlagInstances.charmsOwned, currentCharmsOwned - 1);
                        
                        // If this was the last charm, set hasCharm to false
                        if (currentCharmsOwned == 1)
                        {
                            FlagManager.SetBoolFlag(FlagInstances.hasCharm, false);
                        }
                    }
                },
                // Value list: 5 states for Kingsoul/Void Heart
                () => new List<string> { "NONE", "Queen's fragment", "King's Fragment", "Kingsoul", "Void Heart" }
            ), index + ": " + charm.Name + " State", CabbyMenu.Constants.DEFAULT_PANEL_HEIGHT);
            
            // Add sprite to the panel
            (_, ImageMod spriteImageMod) = PanelAdder.AddSprite(dropdownPanel, CharmIconList.Instance.GetSprite(charm.Id), 1);

            // Add update actions for sprite and color
            dropdownPanel.updateActions.Add(() =>
            {
                spriteImageMod.SetSprite(GetCharmIcon(charm.Id));
                int currentState = PlayerData.instance.royalCharmState;
                if (currentState > 0) // Queen's fragment, King's Fragment, Kingsoul or Void Heart
                {
                    spriteImageMod.SetColor(Color.white);
                }
                else // NONE
                {
                    spriteImageMod.SetColor(unearnedColor);
                }
            });
            
            return dropdownPanel;
        }

        public static CheatPanel CreateCharmTogglePanel(CharmInfo charm)
        {
            int index = charms.IndexOf(charm) + 1;
            
            // Special handling for charm 36 (Kingsoul) - create combined dropdown panel
            if (charm.Id == 36)
            {
                return CreateKingsoulCombinedPanel(charm, index);
            }
            
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
                        
                        if (charm.AssociatedFlags != null)
                        {
                            foreach (FlagDef flag in charm.AssociatedFlags)
                            {
                                CabbyCodesPlugin.BLogger.LogInfo("flag: " + flag.ReadableName);
                                FlagManager.SetBoolFlag(flag, value);
                            }
                        }

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
                    spriteImageMod.SetColor(Color.white);
                }
                else
                {
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