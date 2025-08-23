using CabbyCodes.Patches.BasePatches;
using CabbyCodes.Flags;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.SyncedReferences;
using System;
using System.Collections.Generic;
using CabbyCodes.SavedGames;

namespace CabbyCodes.Patches
{
    public class InventoryPatch : BasePatch
    {
        // Static state tracking for heart piece changes that require save/reload
        private static int startingMaxHealth = -1;
        private static int startingMaxHealthBase = -1;
        private static bool hasInitializedStartingState = false;

        // Reference to the nail upgrade dropdown for updating disabled options
        private static CabbyMenu.UI.Controls.CustomDropdown.CustomDropdown nailUpgradeDropdown;

        public static void AddPanels()
        {
            var inventoryPatch = new InventoryPatch();
            var panels = inventoryPatch.CreatePanels();
            foreach (var panel in panels)
            {
                CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
            }
        }

        /// <summary>
        /// Refreshes the nail upgrade dropdown disabled options based on current ore count and upgrade level.
        /// Call this when ore count or upgrade level changes.
        /// </summary>
        public static void RefreshNailUpgradeDropdown()
        {
            if (nailUpgradeDropdown != null)
            {
                var currentOre = FlagManager.GetIntFlag(FlagInstances.ore);
                var currentUpgradeLevel = FlagManager.GetIntFlag(FlagInstances.nailSmithUpgrades);
                
                // Calculate which options should be disabled
                var disabledOptions = new List<bool>();
                var hoverMessages = new List<string>();
                
                // Option 0: Old Nail - always available (0 ore)
                disabledOptions.Add(false);
                hoverMessages.Add("");
                
                // Option 1: Sharpened Nail - always available (0 ore)
                disabledOptions.Add(false);
                hoverMessages.Add("");
                
                // Option 2: Channelled Nail - requires cost difference from current level
                int costForLevel2 = GetCumulativeOreCost(currentUpgradeLevel, 2);
                bool canAffordChannelled = currentOre >= costForLevel2;
                disabledOptions.Add(!canAffordChannelled);
                if (canAffordChannelled)
                {
                    hoverMessages.Add("");
                }
                else
                {
                    hoverMessages.Add($"You require {costForLevel2} ore(s) to upgrade to this level");
                }
                
                // Option 3: Coiled Nail - requires cost difference from current level
                int costForLevel3 = GetCumulativeOreCost(currentUpgradeLevel, 3);
                bool canAffordCoiled = currentOre >= costForLevel3;
                disabledOptions.Add(!canAffordCoiled);
                if (canAffordCoiled)
                {
                    hoverMessages.Add("");
                }
                else
                {
                    hoverMessages.Add($"You require {costForLevel3} ore(s) to upgrade to this level");
                }
                
                // Option 4: Pure Nail - requires cost difference from current level
                int costForLevel4 = GetCumulativeOreCost(currentUpgradeLevel, 4);
                bool canAffordPure = currentOre >= costForLevel4;
                disabledOptions.Add(!canAffordPure);
                if (canAffordPure)
                {
                    hoverMessages.Add("");
                }
                else
                {
                    hoverMessages.Add($"You require {costForLevel4} ore(s) to upgrade to this level");
                }
                
                // Update the disabled options and hover messages
                nailUpgradeDropdown.SetOptions(new List<string> { "Old Nail", "Sharpened Nail", "Channelled Nail", "Coiled Nail", "Pure Nail" }, disabledOptions, hoverMessages);

                // Ensure the dropdown shows the correct current level
                nailUpgradeDropdown.SetValue(currentUpgradeLevel);
            }
        }

        /// <summary>
        /// Resets the heart piece state tracking. Call this when loading a new save or when
        /// the game state is reset to prevent unnecessary reloads.
        /// </summary>
        public static void ResetHeartPieceStateTracking()
        {
            startingMaxHealth = -1;
            startingMaxHealthBase = -1;
            hasInitializedStartingState = false;
        }

        public override List<CheatPanel> CreatePanels()
        {
            var panels = new List<CheatPanel>
            {
                new InfoPanel("Inventory").SetColor(CheatPanel.headerColor),
                new InfoPanel("Currency").SetColor(CheatPanel.subHeaderColor)
            };
            panels.AddRange(CreateCurrencyPanels());

            panels.Add(new InfoPanel("Ability Items").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateAbilityPanels());

            panels.Add(new InfoPanel("Nail Arts").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNailArtPanels());

            panels.Add(new InfoPanel("Spells").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateSpellPanels());

            panels.Add(new InfoPanel("Items").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateItemPanels());

            panels.Add(new InfoPanel("Keys").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateKeyPanels());

            panels.Add(new InfoPanel("Map Accessories").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateMapAccessoryPanels());

            panels.Add(new InfoPanel("Mask Shards").SetColor(CheatPanel.subHeaderColor));
            panels.Add(new InfoPanel("May require restart to take effect").SetColor(CheatPanel.warningColor));
            panels.AddRange(CreateMaskShardPanels());

            panels.Add(new InfoPanel("Vessel Fragments").SetColor(CheatPanel.subHeaderColor));
            panels.Add(new InfoPanel("May require restart to take effect").SetColor(CheatPanel.warningColor));
            panels.AddRange(CreateVesselFragmentPanels());

            panels.Add(new InfoPanel("Notches").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNotchPanels());

            panels.Add(new InfoPanel("Pale Ore").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreatePaleOrePanels());

            panels.Add(new InfoPanel("Wanderer's Journals").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateWanderersJournalPanels());

            panels.Add(new InfoPanel("Hallownest Seals").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateHallownestSealPanels());

            panels.Add(new InfoPanel("King's Idols").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateKingsIdolPanels());

            panels.Add(new InfoPanel("Arcane Eggs").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateArcaneEggPanels());

            return panels;
        }

        private List<CheatPanel> CreateCurrencyPanels()
        {
            var panels = new List<CheatPanel>();

            // Geo
            var geoPatch = new IntPatch(FlagInstances.geo);
            panels.Add(geoPatch.CreatePanel());

            // Dream Essence
            var dreamEssencePatch = new IntPatch(FlagInstances.dreamOrbs);
            panels.Add(dreamEssencePatch.CreatePanel());

            // Rancid Eggs
            var rancidEggPatch = new IntPatch(FlagInstances.rancidEggs);
            panels.Add(rancidEggPatch.CreatePanel());

            return panels;
        }

        private List<CheatPanel> CreateAbilityPanels()
        {
            var panels = new List<CheatPanel>
            {
                new DropdownPanel(new DelegateValueList(
                    // Getter: 0 = NONE, 1 = Dash only, 2 = Shadow Dash
                    () => {
                        if (!FlagManager.GetBoolFlag(FlagInstances.hasDash)) return 0;
                        else if (!FlagManager.GetBoolFlag(FlagInstances.hasShadowDash)) return 1;
                        return 2;
                    },
                    // Setter
                    value => {
                        if (value > 1)
                        {
                            FlagManager.SetBoolFlag(FlagInstances.hasDash, true);
                            FlagManager.SetBoolFlag(FlagInstances.hasShadowDash, true);
                        }
                        else if (value == 1)
                        {
                            FlagManager.SetBoolFlag(FlagInstances.hasDash, true);
                            FlagManager.SetBoolFlag(FlagInstances.hasShadowDash, false);
                        }
                        else
                        {
                            FlagManager.SetBoolFlag(FlagInstances.hasDash, false);
                            FlagManager.SetBoolFlag(FlagInstances.hasShadowDash, false);
                        }
                    },
                    // Value list
                    () => new List<string> { "NONE", FlagInstances.hasDash.ReadableName, FlagInstances.hasShadowDash.ReadableName }
                ), FlagInstances.hasDash.ReadableName + " / " + FlagInstances.hasShadowDash.ReadableName, Constants.DEFAULT_PANEL_HEIGHT),
                new BoolPatch(FlagInstances.hasDoubleJump).CreatePanel(),
                new BoolPatch(FlagInstances.hasWalljump).CreatePanel(),
                new BoolPatch(FlagInstances.hasSuperDash).CreatePanel(),
                new BoolPatch(FlagInstances.hasAcidArmour).CreatePanel(),
                CreateNailUpgradePanel(),
                CreateDreamNailPanel(),
                CreateDreamGatePanel(),
                new BoolPatch(FlagInstances.unlockedCompletionRate).CreatePanel(),
                new BoolPatch(FlagInstances.salubraBlessing).CreatePanel()
            };

            return panels;
        }

        public static DropdownPanel CreateDreamNailPanel()
        {
            return new DropdownPanel(new DelegateValueList(
                () => {
                    if (FlagManager.GetBoolFlag(FlagInstances.dreamNailUpgraded)) return 2;
                    else if (FlagManager.GetBoolFlag(FlagInstances.hasDreamNail)) return 1;
                    return 0;
                },
                value => {
                    if (value == 2)
                    {
                        FlagManager.SetBoolFlag(FlagInstances.hasDreamNail, true);
                        FlagManager.SetBoolFlag(FlagInstances.dreamNailUpgraded, true);
                        FlagManager.SetBoolFlag(FlagInstances.dreamReward8, true);
                    }
                    else if (value == 1)
                    {
                        FlagManager.SetBoolFlag(FlagInstances.hasDreamNail, true);
                        FlagManager.SetBoolFlag(FlagInstances.dreamNailUpgraded, false);
                        FlagManager.SetBoolFlag(FlagInstances.dreamReward8, false);
                    }
                    else
                    {
                        FlagManager.SetBoolFlag(FlagInstances.hasDreamNail, false);
                        FlagManager.SetBoolFlag(FlagInstances.dreamNailUpgraded, false);
                        FlagManager.SetBoolFlag(FlagInstances.dreamReward8, false);
                    }
                },
                () => new List<string> { "NONE", FlagInstances.hasDreamNail.ReadableName, FlagInstances.dreamNailUpgraded.ReadableName }
            ), FlagInstances.hasDreamNail.ReadableName + " / " + FlagInstances.dreamNailUpgraded.ReadableName, Constants.DEFAULT_PANEL_HEIGHT);
        }

        public static TogglePanel CreateDreamGatePanel()
        {
            var flag = FlagInstances.dreamReward5b;

            return new TogglePanel(new DelegateReference<bool>(
                () => FlagManager.GetBoolFlag(flag),
                value =>
                {
                    FlagManager.SetBoolFlag(flag, value);
                    FlagManager.SetBoolFlag(FlagInstances.hasDreamGate, value);
                }
            ), flag.ReadableName);
        }

        private List<CheatPanel> CreateNailArtPanels()
        {
            var panels = new List<CheatPanel>
            {
                // Cyclone Slash
                new TogglePanel(new DelegateReference<bool>(
                    () => FlagManager.GetBoolFlag(FlagInstances.hasCyclone),
                    value =>
                    {
                        FlagManager.SetBoolFlag(FlagInstances.hasCyclone, value);
                        UpdateHasNailArtFlag();
                    }
                ), FlagInstances.hasCyclone.ReadableName),

                // Upward Slash
                new TogglePanel(new DelegateReference<bool>(
                    () => FlagManager.GetBoolFlag(FlagInstances.hasUpwardSlash),
                    value =>
                    {
                        FlagManager.SetBoolFlag(FlagInstances.hasUpwardSlash, value);
                        UpdateHasNailArtFlag();
                    }
                ), FlagInstances.hasUpwardSlash.ReadableName),

                // Dash Slash
                new TogglePanel(new DelegateReference<bool>(
                    () => FlagManager.GetBoolFlag(FlagInstances.hasDashSlash),
                    value =>
                    {
                        FlagManager.SetBoolFlag(FlagInstances.hasDashSlash, value);
                        UpdateHasNailArtFlag();
                    }
                ), FlagInstances.hasDashSlash.ReadableName)
            };

            return panels;
        }

        private List<CheatPanel> CreateSpellPanels()
        {
            var panels = new List<CheatPanel>
            {
                new BoolPatch(FlagInstances.hasSpell).CreatePanel()
            };

            // Vengeful Spirit
            var vengefulSpiritPatch = new DropdownPatch(FlagInstances.fireballLevel, new List<string> { "NONE", "Vengeful Spirit", "Shade Soul" }, FlagInstances.fireballLevel.ReadableName);
            panels.Add(vengefulSpiritPatch.CreatePanel());

            // Desolate Dive
            var desolateDivePatch = new DropdownPatch(FlagInstances.quakeLevel, new List<string> { "NONE", "Desolate Dive", "Descending Dark" }, FlagInstances.quakeLevel.ReadableName);
            panels.Add(desolateDivePatch.CreatePanel());

            // Howling Wraiths
            var howlingWraithsPatch = new DropdownPatch(FlagInstances.screamLevel, new List<string> { "NONE", "Howling Wraiths", "Abyss Shriek" }, FlagInstances.screamLevel.ReadableName);
            panels.Add(howlingWraithsPatch.CreatePanel());

            return panels;
        }

        private List<CheatPanel> CreateItemPanels()
        {
            var panels = new List<CheatPanel>
            {
                new BoolPatch(FlagInstances.hasQuill).CreatePanel(),
                new BoolPatch(FlagInstances.hasLantern).CreatePanel(),
                new BoolPatch(FlagInstances.hasTramPass).CreatePanel(),
                new BoolPatch(FlagInstances.hasPinGrub).CreatePanel(),
                new BoolPatch(FlagInstances.hasJournal).CreatePanel(),
                new BoolPatch(FlagInstances.hasHuntersMark).CreatePanel(),
                new DropdownPanel(new DelegateValueList(
                    () => {
                        if (FlagManager.GetBoolFlag(FlagInstances.hasXunFlower) && !FlagManager.GetBoolFlag(FlagInstances.xunFlowerBroken)) return 2;
                        else if (FlagManager.GetBoolFlag(FlagInstances.hasXunFlower) && FlagManager.GetBoolFlag(FlagInstances.xunFlowerBroken)) return 1;
                        return 0;
                    },
                    value => {
                        if (value == 1)
                        {
                            FlagManager.SetBoolFlag(FlagInstances.hasXunFlower, true);
                            FlagManager.SetBoolFlag(FlagInstances.xunFlowerBroken, true);
                        }
                        else if (value == 2)
                        {
                            FlagManager.SetBoolFlag(FlagInstances.hasXunFlower, true);
                            FlagManager.SetBoolFlag(FlagInstances.xunFlowerBroken, false);
                        }
                        else
                        {
                            FlagManager.SetBoolFlag(FlagInstances.hasXunFlower, false);
                            FlagManager.SetBoolFlag(FlagInstances.xunFlowerBroken, false);
                        }
                    },
                    () => new List<string> { "NONE", FlagInstances.xunFlowerBroken.ReadableName, FlagInstances.hasXunFlower.ReadableName }
                ), FlagInstances.hasXunFlower.ReadableName, Constants.DEFAULT_PANEL_HEIGHT),
                new BoolPatch(FlagInstances.hasGodfinder).CreatePanel()
            };

            return panels;
        }

        private List<CheatPanel> CreateKeyPanels()
        {
            var panels = new List<CheatPanel>
            {
                CreateWhiteKeyPanel(),
                CreateLoveKeyPanel(),
                CreateSlyKeyPanel(),
                CreateCityKeyPanel(),
                new BoolPatch(FlagInstances.hasKingsBrand).CreatePanel(),
                CreateSimpleKeyPanel(FlagInstances.slySimpleKey),
                CreateSimpleKeyPanel(FlagInstances.Ruins1_17__Shiny_Item),
                CreateSimpleKeyPanel(FlagInstances.Abyss_20__Shiny_Item_Stand),
                CreateSimpleKeyPanel(FlagInstances.gotLurkerKey)
            };

            return panels;
        }

        private TogglePanel CreateSlyKeyPanel()
        {
            TogglePanel panel = null;
            
            panel = new TogglePanel(new DelegateReference<bool>(
                () => FlagManager.GetBoolFlag(FlagInstances.hasSlykey),
                value =>
                {
                    FlagManager.SetBoolFlag(FlagInstances.hasSlykey, value);
                    UpdateSlyKeyPanelInteractable(panel);
                }
            ), FlagInstances.hasSlykey.ReadableName);

            UpdateSlyKeyPanelInteractable(panel);

            return panel;
        }

        private void UpdateSlyKeyPanelInteractable(TogglePanel panel)
        {
            var gaveSlyKey = FlagManager.GetBoolFlag(FlagInstances.gaveSlykey);
            
            // Disable the panel if the key has been given to Sly
            bool shouldBeInteractable = !gaveSlyKey;
            
            var toggleButton = panel.GetToggleButton();
            toggleButton.SetInteractable(shouldBeInteractable);
            
            // Set disabled message for hover popup
            if (!shouldBeInteractable)
            {
                toggleButton.SetDisabledMessage("Key already given to Sly, untoggle that flag to get the key back");
            }
            else
            {
                toggleButton.SetDisabledMessage(""); // Clear message when enabled
            }
        }

        private List<CheatPanel> CreateMapAccessoryPanels()
        {
            var panels = new List<CheatPanel>
            {
                // Scarab Marker
                new TogglePanel(new DelegateReference<bool>(
                    () => FlagManager.GetBoolFlag(FlagInstances.hasMarker_b),
                    value =>
                    {
                        FlagManager.SetBoolFlag(FlagInstances.hasMarker_b, value);
                        UpdateHasMarkerFlag();
                    }
                ), FlagInstances.hasMarker_b.ReadableName),

                // Shell Marker
                new TogglePanel(new DelegateReference<bool>(
                    () => FlagManager.GetBoolFlag(FlagInstances.hasMarker_r),
                    value =>
                    {
                        FlagManager.SetBoolFlag(FlagInstances.hasMarker_r, value);
                        UpdateHasMarkerFlag();
                    }
                ), FlagInstances.hasMarker_r.ReadableName),

                // Gleaming Marker
                new TogglePanel(new DelegateReference<bool>(
                    () => FlagManager.GetBoolFlag(FlagInstances.hasMarker_w),
                    value =>
                    {
                        FlagManager.SetBoolFlag(FlagInstances.hasMarker_w, value);
                        UpdateHasMarkerFlag();
                    }
                ), FlagInstances.hasMarker_w.ReadableName),

                // Token Marker
                new TogglePanel(new DelegateReference<bool>(
                    () => FlagManager.GetBoolFlag(FlagInstances.hasMarker_y),
                    value =>
                    {
                        FlagManager.SetBoolFlag(FlagInstances.hasMarker_y, value);
                        UpdateHasMarkerFlag();
                    }
                ), FlagInstances.hasMarker_y.ReadableName),

                // Whispering Root Pin
                new TogglePanel(new DelegateReference<bool>(
                    () => FlagManager.GetBoolFlag(FlagInstances.hasPinDreamPlant),
                    value =>
                    {
                        FlagManager.SetBoolFlag(FlagInstances.hasPinDreamPlant, value);
                        UpdateHasPinFlag();
                    }
                ), FlagInstances.hasPinDreamPlant.ReadableName),

                // Warrior's Grave Pin
                new TogglePanel(new DelegateReference<bool>(
                    () => FlagManager.GetBoolFlag(FlagInstances.hasPinGhost),
                    value =>
                    {
                        FlagManager.SetBoolFlag(FlagInstances.hasPinGhost, value);
                        UpdateHasPinFlag();
                    }
                ), FlagInstances.hasPinGhost.ReadableName),

                // Stag Station Pin
                new TogglePanel(new DelegateReference<bool>(
                    () => FlagManager.GetBoolFlag(FlagInstances.hasPinStag),
                    value =>
                    {
                        FlagManager.SetBoolFlag(FlagInstances.hasPinStag, value);
                        UpdateHasPinFlag();
                    }
                ), FlagInstances.hasPinStag.ReadableName),

                // Tram Pin
                new TogglePanel(new DelegateReference<bool>(
                    () => FlagManager.GetBoolFlag(FlagInstances.hasPinTram),
                    value =>
                    {
                        FlagManager.SetBoolFlag(FlagInstances.hasPinTram, value);
                        UpdateHasPinFlag();
                    }
                ), FlagInstances.hasPinTram.ReadableName),

                // Vendor Pin
                new TogglePanel(new DelegateReference<bool>(
                    () => FlagManager.GetBoolFlag(FlagInstances.hasPinShop),
                    value =>
                    {
                        FlagManager.SetBoolFlag(FlagInstances.hasPinShop, value);
                        UpdateHasPinFlag();
                    }
                ), FlagInstances.hasPinShop.ReadableName),

                // Hot Spring Pin
                new TogglePanel(new DelegateReference<bool>(
                    () => FlagManager.GetBoolFlag(FlagInstances.hasPinSpa),
                    value =>
                    {
                        FlagManager.SetBoolFlag(FlagInstances.hasPinSpa, value);
                        UpdateHasPinFlag();
                    }
                ), FlagInstances.hasPinSpa.ReadableName),

                // Cocoon Pin
                new TogglePanel(new DelegateReference<bool>(
                    () => FlagManager.GetBoolFlag(FlagInstances.hasPinCocoon),
                    value =>
                    {
                        FlagManager.SetBoolFlag(FlagInstances.hasPinCocoon, value);
                        UpdateHasPinFlag();
                    }
                ), FlagInstances.hasPinCocoon.ReadableName),

                // Bench Pin
                new TogglePanel(new DelegateReference<bool>(
                    () => FlagManager.GetBoolFlag(FlagInstances.hasPinBench),
                    value =>
                    {
                        FlagManager.SetBoolFlag(FlagInstances.hasPinBench, value);
                        UpdateHasPinFlag();
                    }
                ), FlagInstances.hasPinBench.ReadableName)
            };

            return panels;
        }

        private void UpdateHasPinFlag()
        {
            // Check if any pin is enabled
            bool anyPinEnabled = FlagManager.GetBoolFlag(FlagInstances.hasPinDreamPlant) ||
                                FlagManager.GetBoolFlag(FlagInstances.hasPinGhost) ||
                                FlagManager.GetBoolFlag(FlagInstances.hasPinStag) ||
                                FlagManager.GetBoolFlag(FlagInstances.hasPinTram) ||
                                FlagManager.GetBoolFlag(FlagInstances.hasPinShop) ||
                                FlagManager.GetBoolFlag(FlagInstances.hasPinSpa) ||
                                FlagManager.GetBoolFlag(FlagInstances.hasPinCocoon) ||
                                FlagManager.GetBoolFlag(FlagInstances.hasPinBench);

            // Set hasPin flag based on whether any pin is enabled
            FlagManager.SetBoolFlag(FlagInstances.hasPin, anyPinEnabled);
        }

        private void UpdateHasMarkerFlag()
        {
            // Check if any marker is enabled
            bool anyMarkerEnabled = FlagManager.GetBoolFlag(FlagInstances.hasMarker_b) ||
                                    FlagManager.GetBoolFlag(FlagInstances.hasMarker_r) ||
                                    FlagManager.GetBoolFlag(FlagInstances.hasMarker_w) ||
                                    FlagManager.GetBoolFlag(FlagInstances.hasMarker_y);

            // Set hasMarker flag based on whether any marker is enabled
            FlagManager.SetBoolFlag(FlagInstances.hasMarker, anyMarkerEnabled);
        }

        private void UpdateHasNailArtFlag()
        {
            // Check if any Nail Art is enabled
            bool anyNailArtEnabled = FlagManager.GetBoolFlag(FlagInstances.hasCyclone) ||
                                     FlagManager.GetBoolFlag(FlagInstances.hasUpwardSlash) ||
                                     FlagManager.GetBoolFlag(FlagInstances.hasDashSlash);

            // Set aggregate hasNailArt flag based on individual Nail Arts
            FlagManager.SetBoolFlag(FlagInstances.hasNailArt, anyNailArtEnabled);

            bool allNailArtEnabled = FlagManager.GetBoolFlag(FlagInstances.hasCyclone) &&
                                     FlagManager.GetBoolFlag(FlagInstances.hasUpwardSlash) &&
                                     FlagManager.GetBoolFlag(FlagInstances.hasDashSlash);

            FlagManager.SetBoolFlag(FlagInstances.hasAllNailArts, allNailArtEnabled);
        }

        private List<CheatPanel> CreateMaskShardPanels()
        {
            var panels = new List<CheatPanel>
            {
                CreateHeartPiecePanel(FlagInstances.Crossroads_38__Heart_Piece),
                CreateHeartPiecePanel(FlagInstances.Crossroads_09__Heart_Piece),
                CreateHeartPiecePanel(FlagInstances.Crossroads_13__Heart_Piece),
                CreateHeartPiecePanel(FlagInstances.Fungus1_36__Heart_Piece),
                CreateHeartPiecePanel(FlagInstances.Fungus2_01__Heart_Piece),
                CreateHeartPiecePanel(FlagInstances.slyShellFrag1),
                CreateHeartPiecePanel(FlagInstances.slyShellFrag2),
                CreateHeartPiecePanel(FlagInstances.slyShellFrag3),
                CreateHeartPiecePanel(FlagInstances.slyShellFrag4),
                CreateHeartPiecePanel(FlagInstances.Crossroads_38__Reward_5, new List<FlagDef> { FlagInstances.Crossroads_38__Heart_Piece }),
                CreateHeartPiecePanel(FlagInstances.Fungus2_25__Heart_Piece),
                CreateHeartPiecePanel(FlagInstances.Mines_32__Heart_Piece),
                CreateHeartPiecePanel(FlagInstances.dreamReward7),
                CreateHeartPiecePanel(FlagInstances.Room_Bretta__Heart_Piece),
                CreateHeartPiecePanel(FlagInstances.Hive_04__Heart_Piece),
                CreateHeartPiecePanel(FlagInstances.Waterways_04b__Heart_Piece),
            };
            
            return panels;
        }

        public static TogglePanel CreateHeartPiecePanel(FlagDef flag, List<FlagDef> additionalFlags = null)
        {
            return new TogglePanel(new DelegateReference<bool>(
                () => FlagManager.GetBoolFlag(flag),
                value => {
                    // Initialize starting state if not done yet
                    if (!hasInitializedStartingState)
                    {
                        startingMaxHealth = FlagManager.GetIntFlag(FlagInstances.maxHealth);
                        startingMaxHealthBase = FlagManager.GetIntFlag(FlagInstances.maxHealthBase);
                        hasInitializedStartingState = true;
                    }

                    FlagManager.SetBoolFlag(flag, value);
                    if (additionalFlags != null){
                        foreach (FlagDef flagDef in additionalFlags)
                        {
                            FlagManager.SetBoolFlag(flagDef, value);
                        }
                    }

                    var currentHeartPieces = FlagManager.GetIntFlag(FlagInstances.heartPieces);
                    var newHeartPieces = currentHeartPieces;
                    
                    if (value)
                    {
                        // Increment heart pieces when enabling
                        newHeartPieces++;
                        
                        // If we hit 4 heart pieces, reset to 0 and increase max health
                        if (newHeartPieces >= 4)
                        {
                            newHeartPieces = 0;
                            var currentMaxHealth = FlagManager.GetIntFlag(FlagInstances.maxHealth);
                            var currentMaxHealthBase = FlagManager.GetIntFlag(FlagInstances.maxHealthBase);
                            FlagManager.SetIntFlag(FlagInstances.maxHealth, currentMaxHealth + 1);
                            FlagManager.SetIntFlag(FlagInstances.maxHealthBase, currentMaxHealthBase + 1);
                        }
                    }
                    else
                    {
                        // Decrement heart pieces when disabling
                        newHeartPieces--;
                        
                        // If we go below 0, reset to 3 and decrease max health
                        if (newHeartPieces < 0)
                        {
                            newHeartPieces = 3;
                            var currentMaxHealth = FlagManager.GetIntFlag(FlagInstances.maxHealth);
                            var currentMaxHealthBase = FlagManager.GetIntFlag(FlagInstances.maxHealthBase);
                            FlagManager.SetIntFlag(FlagInstances.maxHealth, Math.Max(0, currentMaxHealth - 1));
                            FlagManager.SetIntFlag(FlagInstances.maxHealthBase, Math.Max(0, currentMaxHealthBase - 1));
                        }
                    }
                    
                    FlagManager.SetIntFlag(FlagInstances.heartPieces, newHeartPieces);
                    
                    // Update heartPieceCollected flag based on count
                    FlagManager.SetBoolFlag(FlagInstances.heartPieceCollected, newHeartPieces > 0);
                    
                    // Check if current health differs from starting health
                    var finalMaxHealth = FlagManager.GetIntFlag(FlagInstances.maxHealth);
                    var finalMaxHealthBase = FlagManager.GetIntFlag(FlagInstances.maxHealthBase);
                    
                    bool healthDiffersFromStart = finalMaxHealth != startingMaxHealth || finalMaxHealthBase != startingMaxHealthBase;
                    
                    if (healthDiffersFromStart)
                    {
                        // Health has changed from starting state, request reload
                        GameReloadManager.RequestReload($"HeartPiece");
                    }
                    else
                    {
                        // Health matches starting state, cancel reload request
                        GameReloadManager.CancelReload($"HeartPiece");
                    }
                }
            ), flag.ReadableName);
        }

        private List<CheatPanel> CreateVesselFragmentPanels()
        {
            var panels = new List<CheatPanel>
            {
                CreateVesselFragmentPanel(FlagInstances.slyVesselFrag1),
                CreateVesselFragmentPanel(FlagInstances.slyVesselFrag2),
                CreateVesselFragmentPanel(FlagInstances.slyVesselFrag3),
                CreateVesselFragmentPanel(FlagInstances.slyVesselFrag4),
                CreateVesselFragmentPanel(FlagInstances.Fungus1_13__Vessel_Fragment),
                CreateVesselFragmentPanel(FlagInstances.Deepnest_38__Vessel_Fragment),
                CreateVesselFragmentPanel(FlagInstances.Ruins2_09__Vessel_Fragment),
                CreateVesselFragmentPanel(FlagInstances.Crossroads_37__Vessel_Fragment),
                CreateVesselFragmentPanel(FlagInstances.dreamReward5),
            };
            
            return panels;
        }

        public static TogglePanel CreateVesselFragmentPanel(FlagDef vesselFragmentFlag)
        {
            return new TogglePanel(new DelegateReference<bool>(
                () => FlagManager.GetBoolFlag(vesselFragmentFlag),
                value =>
                {
                    FlagManager.SetBoolFlag(vesselFragmentFlag, value);
                    
                    var currentVesselFragments = FlagManager.GetIntFlag(FlagInstances.vesselFragments);
                    var newVesselFragments = currentVesselFragments;
                    
                    if (value)
                    {
                        // Increment vessel fragments when enabling
                        newVesselFragments = currentVesselFragments + 1;
                        
                        // If we hit 3 vessel fragments, reset to 0
                        if (newVesselFragments >= 3)
                        {
                            newVesselFragments = 0;
                            var currentMpReserveMax = FlagManager.GetIntFlag(FlagInstances.MPReserveMax);
                            FlagManager.SetIntFlag(FlagInstances.MPReserveMax, currentMpReserveMax + 33);
                        }
                    }
                    else
                    {
                        // Decrement vessel fragments when disabling
                        newVesselFragments = currentVesselFragments - 1;
                        
                        // If we go below 0, reset to 2
                        if (newVesselFragments < 0)
                        {
                            newVesselFragments = 2;
                            var currentMpReserveMax = FlagManager.GetIntFlag(FlagInstances.MPReserveMax);
                            FlagManager.SetIntFlag(FlagInstances.MPReserveMax, currentMpReserveMax - 33);
                        }
                    }
                    
                    FlagManager.SetIntFlag(FlagInstances.vesselFragments, newVesselFragments);
                }
            ), vesselFragmentFlag.ReadableName);
        }

        private TogglePanel CreateSimpleKeyPanel(FlagDef simpleKeyFlag)
        {
            TogglePanel panel = null;
            
            panel = new TogglePanel(new DelegateReference<bool>(
                () => FlagManager.GetBoolFlag(simpleKeyFlag),
                value =>
                {
                    var currentSimpleKeys = FlagManager.GetIntFlag(FlagInstances.simpleKeys);
                    
                    if (value)
                    {
                        // Increment simple keys when enabling
                        FlagManager.SetIntFlag(FlagInstances.simpleKeys, currentSimpleKeys + 1);
                    }
                    else
                    {
                        // Decrement simple keys when disabling
                        FlagManager.SetIntFlag(FlagInstances.simpleKeys, Math.Max(0, currentSimpleKeys - 1));
                    }
                    
                    FlagManager.SetBoolFlag(simpleKeyFlag, value);
                    
                    // Update the panel's interactable state after changing the key value
                    UpdateSimpleKeyPanelInteractable(panel);
                }
            ), simpleKeyFlag.ReadableName);

            // Set initial interactable state
            UpdateSimpleKeyPanelInteractable(panel);

            return panel;
        }

        private void UpdateSimpleKeyPanelInteractable(TogglePanel panel)
        {
            var currentSimpleKeys = FlagManager.GetIntFlag(FlagInstances.simpleKeys);
            
            // Panel is disabled if there are no simple keys available
            // This prevents disabling keys when they are in use
            bool shouldBeInteractable = currentSimpleKeys > 0;
            
            var toggleButton = panel.GetToggleButton();
            toggleButton.SetInteractable(shouldBeInteractable);
            
            // Set disabled message for hover popup
            if (!shouldBeInteractable)
            {
                toggleButton.SetDisabledMessage("Cannot disable keys while they are in use\n\nSimple key count cannot go below 0");
            }
            else
            {
                toggleButton.SetDisabledMessage(""); // Clear message when enabled
            }
        }

        private TogglePanel CreateCityKeyPanel()
        {
            TogglePanel panel = null;
            
            panel = new TogglePanel(new DelegateReference<bool>(
                () => FlagManager.GetBoolFlag(FlagInstances.hasCityKey),
                value =>
                {
                    FlagManager.SetBoolFlag(FlagInstances.hasCityKey, value);
                    // Update the panel's interactable state after changing the key value
                    UpdateCityKeyPanelInteractable(panel);
                }
            ), FlagInstances.hasCityKey.ReadableName);

            // Set initial interactable state
            UpdateCityKeyPanelInteractable(panel);

            return panel;
        }

        private void UpdateCityKeyPanelInteractable(TogglePanel panel)
        {
            var openedCityGate = FlagManager.GetBoolFlag(FlagInstances.openedCityGate);
            
            // Disable the panel if the key has been used to open the gate
            bool shouldBeInteractable = !openedCityGate;
            
            var toggleButton = panel.GetToggleButton();
            toggleButton.SetInteractable(shouldBeInteractable);
            
            // Set disabled message for hover popup
            if (!shouldBeInteractable)
            {
                toggleButton.SetDisabledMessage("Key has been used to open the City of Tears gate");
            }
            else
            {
                toggleButton.SetDisabledMessage(""); // Clear message when enabled
            }
        }

        private TogglePanel CreateLoveKeyPanel()
        {
            TogglePanel panel = null;
            
            panel = new TogglePanel(new DelegateReference<bool>(
                () => FlagManager.GetBoolFlag(FlagInstances.hasLoveKey),
                value =>
                {
                    FlagManager.SetBoolFlag(FlagInstances.hasLoveKey, value);
                    // Update the panel's interactable state after changing the key value
                    UpdateLoveKeyPanelInteractable(panel);
                }
            ), FlagInstances.hasLoveKey.ReadableName);

            // Set initial interactable state
            UpdateLoveKeyPanelInteractable(panel);

            return panel;
        }

        private void UpdateLoveKeyPanelInteractable(TogglePanel panel)
        {
            var openedLoveDoor = FlagManager.GetBoolFlag(FlagInstances.openedLoveDoor);
            
            // Disable the panel if the key has been used to open the door
            bool shouldBeInteractable = !openedLoveDoor;
            
            var toggleButton = panel.GetToggleButton();
            toggleButton.SetInteractable(shouldBeInteractable);
            
            // Set disabled message for hover popup
            if (!shouldBeInteractable)
            {
                toggleButton.SetDisabledMessage("Key has been used to open the Love Door");
            }
            else
            {
                toggleButton.SetDisabledMessage(""); // Clear message when enabled
            }
        }

        private TogglePanel CreateWhiteKeyPanel()
        {
            TogglePanel panel = null;
            
            panel = new TogglePanel(new DelegateReference<bool>(
                () => FlagManager.GetBoolFlag(FlagInstances.hasWhiteKey),
                value =>
                {
                    FlagManager.SetBoolFlag(FlagInstances.hasWhiteKey, value);
                    // Update the panel's interactable state after changing the key value
                    UpdateWhiteKeyPanelInteractable(panel);
                }
            ), FlagInstances.hasWhiteKey.ReadableName);

            // Set initial interactable state
            UpdateWhiteKeyPanelInteractable(panel);

            return panel;
        }

        private void UpdateWhiteKeyPanelInteractable(TogglePanel panel)
        {
            var usedWhiteKey = FlagManager.GetBoolFlag(FlagInstances.usedWhiteKey);
            
            // Disable the panel if the key has been used to open the door
            bool shouldBeInteractable = !usedWhiteKey;
            
            var toggleButton = panel.GetToggleButton();
            toggleButton.SetInteractable(shouldBeInteractable);
            
            // Set disabled message for hover popup
            if (!shouldBeInteractable)
            {
                toggleButton.SetDisabledMessage("Key has been used to open the door to Shade Soul");
            }
            else
            {
                toggleButton.SetDisabledMessage(""); // Clear message when enabled
            }
        }

        private List<CheatPanel> CreateNotchPanels()
        {
            var panels = new List<CheatPanel>
            {
                CreateNotchPanel(FlagInstances.salubraNotch1),
                CreateNotchPanel(FlagInstances.salubraNotch2),
                CreateNotchPanel(FlagInstances.salubraNotch3),
                CreateNotchPanel(FlagInstances.salubraNotch4),
                CreateNotchPanel(FlagInstances.notchShroomOgres),
                CreateNotchPanel(FlagInstances.Room_Colosseum_Bronze__Shiny_Item),
                CreateNotchPanel(FlagInstances.Fungus3_28__Shiny_Item),
            };

            return panels;
        }

        private TogglePanel CreateNotchPanel(FlagDef notchFlag)
        {
            return new TogglePanel(new DelegateReference<bool>(
                () => FlagManager.GetBoolFlag(notchFlag),
                value =>
                {
                    var currentNotches = FlagManager.GetIntFlag(FlagInstances.charmSlots);
                    
                    if (value)
                    {
                        // Increment simple keys when enabling
                        FlagManager.SetIntFlag(FlagInstances.charmSlots, currentNotches + 1);
                    }
                    else
                    {
                        // Decrement simple keys when disabling
                        FlagManager.SetIntFlag(FlagInstances.charmSlots, Math.Max(0, currentNotches - 1));
                    }
                    
                    FlagManager.SetBoolFlag(notchFlag, value);
                }
            ), notchFlag.ReadableName);
        }

        private List<CheatPanel> CreatePaleOrePanels()
        {
            var panels = new List<CheatPanel>
            {
                CreatePaleOrePanel(FlagInstances.Abyss_17__Shiny_Item_Stand),
                CreatePaleOrePanel(FlagInstances.dreamReward3),
                CreatePaleOrePanel(FlagInstances.Mines_34__Shiny_Item_Stand),
                CreatePaleOrePanel(FlagInstances.Room_Colosseum_Silver__Shiny_Item),
                CreatePaleOrePanel(FlagInstances.Crossroads_38__Shiny_Item_Ore),
            };

            return panels;
        }

        public static TogglePanel CreatePaleOrePanel(FlagDef flag)
        {
            // Create a custom synced reference that handles ore count changes and interactable state
            var syncedReference = new DelegateReference<bool>(
                () => FlagManager.GetBoolFlag(flag),
                value => {
                    var current = FlagManager.GetIntFlag(FlagInstances.ore);
            
                    if (value)
                    {
                        FlagManager.SetIntFlag(FlagInstances.ore, current + 1);
                    }
                    else
                    {
                        FlagManager.SetIntFlag(FlagInstances.ore, Math.Max(0, current - 1));
                    }
                    
                    FlagManager.SetBoolFlag(flag, value);
                    
                    // Update all pale ore panel interactable states after ore count changes
                    UpdateAllPaleOrePanels();
                }
            );

            var togglePanel = new TogglePanel(syncedReference, flag.ReadableName);
            
            // Store reference to this panel for ore count change notifications
            RegisterPaleOrePanel(togglePanel, flag);
            
            return togglePanel;
        }

        // Static list to track all pale ore panels for ore count change notifications
        private static readonly List<(TogglePanel panel, FlagDef flag)> paleOrePanels = new List<(TogglePanel, FlagDef)>();

        private static void RegisterPaleOrePanel(TogglePanel togglePanel, FlagDef flag)
        {
            paleOrePanels.Add((togglePanel, flag));
            // Initial interactable state check
            UpdatePaleOreInteractable(togglePanel, flag);
        }

        /// <summary>
        /// Updates all pale ore panel interactable states. Call this whenever ore count changes.
        /// </summary>
        public static void UpdateAllPaleOrePanels()
        {
            foreach (var (panel, flag) in paleOrePanels)
            {
                if (panel != null && panel.GetGameObject() != null)
                {
                    UpdatePaleOreInteractable(panel, flag);
                }
            }
        }

        private static void UpdatePaleOreInteractable(TogglePanel togglePanel, FlagDef flag)
        {
            var currentOre = FlagManager.GetIntFlag(FlagInstances.ore);
            var isCurrentlyOn = FlagManager.GetBoolFlag(flag);
            bool shouldBeInteractable = !(currentOre == 0 && isCurrentlyOn);
            
            var toggleButton = togglePanel.GetToggleButton();
            toggleButton.SetInteractable(shouldBeInteractable);
            
            // Set disabled message for hover popup
            if (!shouldBeInteractable)
            {
                toggleButton.SetDisabledMessage("Cannot disable pale ore when ore count is 0\n\nReduce the Nail Upgrade to free up ores");
            }
            else
            {
                toggleButton.SetDisabledMessage(""); // Clear message when enabled
            }
        }

        // Wanderer's Journals panels
        private List<CheatPanel> CreateWanderersJournalPanels()
        {
            var panels = new List<CheatPanel>
            {
                CreateWanderersJournalPanel(FlagInstances.Fungus1_22__Shiny_Item),
                CreateWanderersJournalPanel(FlagInstances.Fungus2_17__Shiny_Item),
                CreateWanderersJournalPanel(FlagInstances.Cliffs_01__Shiny_Item_1),
                CreateWanderersJournalPanel(FlagInstances.Fungus2_04__Shiny_Item),
                CreateWanderersJournalPanel(FlagInstances.Abyss_02__Shiny_Item),
                CreateWanderersJournalPanel(FlagInstances.Ruins_Elevator__Shiny_Item_1),
                CreateWanderersJournalPanel(FlagInstances.Ruins2_05__Shiny_Item),
                CreateWanderersJournalPanel(FlagInstances.Ruins1_28__Shiny_Item),
                CreateWanderersJournalPanel(FlagInstances.RestingGrounds_10__Shiny_Item),
                CreateWanderersJournalPanel(FlagInstances.Mines_20__Shiny_Item_1),
                CreateWanderersJournalPanel(FlagInstances.Deepnest_East_18__Shiny_Item),
                CreateWanderersJournalPanel(FlagInstances.Deepnest_East_13__Shiny_Item),
            };

            return panels;
        }

        private TogglePanel CreateWanderersJournalPanel(FlagDef flag)
        {
            return new TogglePanel(new DelegateReference<bool>(
                () => FlagManager.GetBoolFlag(flag),
                value =>
                {
                    FlagManager.SetBoolFlag(flag, value);

                    int currentJournals = FlagManager.GetIntFlag(FlagInstances.trinket1);
                    int newJournals = value ? currentJournals + 1 : Math.Max(0, currentJournals - 1);

                    FlagManager.SetIntFlag(FlagInstances.trinket1, newJournals);
                    FlagManager.SetBoolFlag(FlagInstances.foundTrinket1, newJournals > 0);
                }
            ), flag.Scene.ReadableName);
        }

        private List<CheatPanel> CreateHallownestSealPanels()
        {
            var panels = new List<CheatPanel>
            {
                CreateHallownestSealPanel(FlagInstances.Fungus2_03__Shiny_Item),
                CreateHallownestSealPanel(FlagInstances.Fungus3_30__Shiny_Item),
                CreateHallownestSealPanel(FlagInstances.Deepnest_Spider_Town__Shiny_Item),
                CreateHallownestSealPanel(FlagInstances.Fungus2_31__Shiny_Item),
                CreateHallownestSealPanel(FlagInstances.Ruins2_08__Shiny_Item),
                CreateHallownestSealPanel(FlagInstances.Ruins1_03__Shiny_Item),
                CreateHallownestSealPanel(FlagInstances.Ruins1_32__Shiny_Item),
                CreateHallownestSealPanel(FlagInstances.RestingGrounds_10__Shiny_Item_1),
                CreateHallownestSealPanel(FlagInstances.dreamReward1),
                CreateHallownestSealPanel(FlagInstances.Ruins2_03__Shiny_Item),
                CreateHallownestSealPanel(FlagInstances.Fungus3_48__Shiny_Item),
                CreateHallownestSealPanel(FlagInstances.Crossroads_38__Shiny_Item_Relic2),
            };

            return panels;
        }

        public static TogglePanel CreateHallownestSealPanel(FlagDef flag)
        {
            string label;

            if (flag.Scene != null)
            {
                label = flag.Scene.ReadableName;
            }
            else
            {
                label = flag.ReadableName;
            }

            return new TogglePanel(new DelegateReference<bool>(
                () => FlagManager.GetBoolFlag(flag),
                value => {
                    FlagManager.SetBoolFlag(flag, value);

                    int current = FlagManager.GetIntFlag(FlagInstances.trinket2);
                    int newCount = value ? current + 1 : Math.Max(0, current - 1);

                    FlagManager.SetIntFlag(FlagInstances.trinket2, newCount);
                    FlagManager.SetBoolFlag(FlagInstances.foundTrinket2, newCount > 0);
                }
            ), label);
        }

        private List<CheatPanel> CreateKingsIdolPanels()
        {
            var panels = new List<CheatPanel>
            {
                CreateKingsIdolPanel(FlagInstances.Cliffs_01__Shiny_Item),
                CreateKingsIdolPanel(FlagInstances.Deepnest_33__Shiny_Item),
                CreateKingsIdolPanel(FlagInstances.Ruins1_32__Shiny_Item),
                CreateKingsIdolPanel(FlagInstances.RestingGrounds_08__Shiny_Item),
                CreateKingsIdolPanel(FlagInstances.Mines_30__Shiny_Item_Stand),
                CreateKingsIdolPanel(FlagInstances.Deepnest_East_08__Shiny_Item),
                CreateKingsIdolPanel(FlagInstances.GG_Lurker__Shiny_Item),
                CreateKingsIdolPanel(FlagInstances.Waterways_15__Shiny_Item_Stand),
                CreateKingsIdolPanel(FlagInstances.Crossroads_38__Shiny_Item_Relic3),
            };

            return panels;
        }

        private TogglePanel CreateKingsIdolPanel(FlagDef flag)
        {
            return new TogglePanel(new DelegateReference<bool>(
                () => FlagManager.GetBoolFlag(flag),
                value =>
                {
                    FlagManager.SetBoolFlag(flag, value);

                    int current = FlagManager.GetIntFlag(FlagInstances.trinket3);
                    int newCount = value ? current + 1 : Math.Max(0, current - 1);

                    FlagManager.SetIntFlag(FlagInstances.trinket3, newCount);
                    FlagManager.SetBoolFlag(FlagInstances.foundTrinket3, newCount > 0);
                }
            ), flag.Scene.ReadableName);
        }

        private List<CheatPanel> CreateArcaneEggPanels()
        {
            var panels = new List<CheatPanel>
            {
                CreateArcaneEggPanel(FlagInstances.dreamReward6),
                CreateArcaneEggPanel(FlagInstances.Abyss_10__Shiny_Item),
            };

            return panels;
        }

        public static TogglePanel CreateArcaneEggPanel(FlagDef flag)
        {
            var label = flag.Scene?.ReadableName ?? flag.ReadableName;

            return new TogglePanel(new DelegateReference<bool>(
                () => FlagManager.GetBoolFlag(flag),
                value =>
                {
                    FlagManager.SetBoolFlag(flag, value);

                    int current = FlagManager.GetIntFlag(FlagInstances.trinket4);
                    int newCount = value ? current + 1 : Math.Max(0, current - 1);

                    FlagManager.SetIntFlag(FlagInstances.trinket4, newCount);
                    FlagManager.SetBoolFlag(FlagInstances.foundTrinket4, newCount > 0);
                }
            ), label);
        }

        private CheatPanel CreateNailUpgradePanel()
        {
            // Create a custom dropdown panel that can handle disabled options
            var dropdownPanel = new DropdownPanel(new DelegateValueList(
                // Getter: returns current nail upgrade level
                () => FlagManager.GetIntFlag(FlagInstances.nailSmithUpgrades),
                
                // Setter: handles nail upgrade logic
                value => {
                    // Don't allow setting disabled options
                    var currentOre = FlagManager.GetIntFlag(FlagInstances.ore);
                    var currentUpgradeLevel = FlagManager.GetIntFlag(FlagInstances.nailSmithUpgrades);
                    
                    // Check if the selected upgrade can be afforded
                    bool canAfford = false;
                    switch (value)
                    {
                        case 0: // Old Nail - always available (0 ore)
                            canAfford = true;
                            break;
                        case 1: // Sharpened Nail - always available (0 ore)
                            canAfford = true;
                            break;
                        case 2: // Channelled Nail - requires cumulative ore from current level
                            int oreForLevel2 = GetCumulativeOreCost(currentUpgradeLevel, 2);
                            canAfford = currentOre >= oreForLevel2;
                            break;
                        case 3: // Coiled Nail - requires cumulative ore from current level
                            int oreForLevel3 = GetCumulativeOreCost(currentUpgradeLevel, 3);
                            canAfford = currentOre >= oreForLevel3;
                            break;
                        case 4: // Pure Nail - requires cumulative ore from current level
                            int oreForLevel4 = GetCumulativeOreCost(currentUpgradeLevel, 4);
                            canAfford = currentOre >= oreForLevel4;
                            break;
                    }
                    
                    if (!canAfford)
                    {
                        return; // Don't allow the upgrade if it can't be afforded
                    }
                    
                    // Calculate ore cost difference and update ore count
                    int oreCostDifference = CalculateOreCostDifference(currentUpgradeLevel, value);
                    FlagManager.SetIntFlag(FlagInstances.ore, currentOre - oreCostDifference);
                    
                    // Apply the upgrade
                    switch (value)
                    {
                        case 0: // Old Nail
                            FlagManager.SetIntFlag(FlagInstances.nailDamage, 5);
                            FlagManager.SetIntFlag(FlagInstances.beamDamage, 2);
                            FlagManager.SetIntFlag(FlagInstances.nailSmithUpgrades, 0);
                            FlagManager.SetBoolFlag(FlagInstances.honedNail, false);
                            FlagManager.SetBoolFlag(FlagInstances.nailsmithCliff, false);
                            break;
                        case 1: // Sharpened Nail
                            FlagManager.SetIntFlag(FlagInstances.nailDamage, 9);
                            FlagManager.SetIntFlag(FlagInstances.beamDamage, 4);
                            FlagManager.SetIntFlag(FlagInstances.nailSmithUpgrades, 1);
                            FlagManager.SetBoolFlag(FlagInstances.honedNail, true);
                            FlagManager.SetBoolFlag(FlagInstances.nailsmithCliff, false);
                            break;
                        case 2: // Channelled Nail
                            FlagManager.SetIntFlag(FlagInstances.nailDamage, 13);
                            FlagManager.SetIntFlag(FlagInstances.beamDamage, 6);
                            FlagManager.SetIntFlag(FlagInstances.nailSmithUpgrades, 2);
                            FlagManager.SetBoolFlag(FlagInstances.honedNail, true);
                            FlagManager.SetBoolFlag(FlagInstances.nailsmithCliff, false);
                            break;
                        case 3: // Coiled Nail
                            FlagManager.SetIntFlag(FlagInstances.nailDamage, 17);
                            FlagManager.SetIntFlag(FlagInstances.beamDamage, 8);
                            FlagManager.SetIntFlag(FlagInstances.nailSmithUpgrades, 3);
                            FlagManager.SetBoolFlag(FlagInstances.honedNail, true);
                            FlagManager.SetBoolFlag(FlagInstances.nailsmithCliff, false);
                            break;
                        case 4: // Pure Nail
                            FlagManager.SetIntFlag(FlagInstances.nailDamage, 21);
                            FlagManager.SetIntFlag(FlagInstances.beamDamage, 10);
                            FlagManager.SetIntFlag(FlagInstances.nailSmithUpgrades, 4);
                            FlagManager.SetBoolFlag(FlagInstances.honedNail, true);
                            FlagManager.SetBoolFlag(FlagInstances.nailsmithCliff, true);
                            break;
                    }

                    FlagManager.SetBoolFlag(FlagInstances.nailsmithKillSpeech, false);
                    FlagManager.SetBoolFlag(FlagInstances.nailsmithKilled, false);
                    
                    // Refresh the dropdown to update disabled options after the upgrade
                    RefreshNailUpgradeDropdown();
                    UpdateAllPaleOrePanels();
                },
                
                // Value list: returns the nail upgrade options
                () => new List<string> { "Old Nail", "Sharpened Nail", "Channelled Nail", "Coiled Nail", "Pure Nail" }
            ), "Nail Upgrade", Constants.DEFAULT_PANEL_HEIGHT);
            
            // Get the CustomDropdown component to set up disabled options
            var customDropdown = dropdownPanel.GetDropDownSync().GetCustomDropdown();
            
            // Set up callback to refresh disabled options when dropdown is opened
            customDropdown.onDropdownOpened = () => UpdateNailUpgradeDisabledOptions(customDropdown);
            
            // Store the reference for updating
            nailUpgradeDropdown = customDropdown;
            
            return dropdownPanel;
        }

        private void UpdateNailUpgradeDisabledOptions(CabbyMenu.UI.Controls.CustomDropdown.CustomDropdown customDropdown)
        {
            var currentOre = FlagManager.GetIntFlag(FlagInstances.ore);
            var currentUpgradeLevel = FlagManager.GetIntFlag(FlagInstances.nailSmithUpgrades);
            
            // Calculate which options should be disabled
            var disabledOptions = new List<bool>();
            var hoverMessages = new List<string>();
            
            // Option 0: Old Nail - always available (0 ore)
            disabledOptions.Add(false);
            hoverMessages.Add("");
            
            // Option 1: Sharpened Nail - always available (0 ore)
            disabledOptions.Add(false);
            hoverMessages.Add("");
            
            // Option 2: Channelled Nail - requires cost difference from current level
            int costForLevel2 = GetCumulativeOreCost(currentUpgradeLevel, 2);
            bool canAffordChannelled = currentOre >= costForLevel2;
            disabledOptions.Add(!canAffordChannelled);
            if (canAffordChannelled)
            {
                hoverMessages.Add("");
            }
            else
            {
                if (costForLevel2 == 0)
                {
                    hoverMessages.Add("You already have this upgrade or higher");
                }
                else
                {
                    hoverMessages.Add($"You require {costForLevel2} ore(s) to upgrade to this level");
                }
            }
            
            // Option 3: Coiled Nail - requires cost difference from current level
            int costForLevel3 = GetCumulativeOreCost(currentUpgradeLevel, 3);
            bool canAffordCoiled = currentOre >= costForLevel3;
            disabledOptions.Add(!canAffordCoiled);
            if (canAffordCoiled)
            {
                hoverMessages.Add("");
            }
            else
            {
                if (costForLevel3 == 0)
                {
                    hoverMessages.Add("You already have this upgrade or higher");
                }
                else
                {
                    hoverMessages.Add($"You require {costForLevel3} ore(s) to upgrade to this level");
                }
            }
            
            // Option 4: Pure Nail - requires cost difference from current level
            int costForLevel4 = GetCumulativeOreCost(currentUpgradeLevel, 4);
            bool canAffordPure = currentOre >= costForLevel4;
            disabledOptions.Add(!canAffordPure);
            if (canAffordPure)
            {
                hoverMessages.Add("");
            }
            else
            {
                if (costForLevel4 == 0)
                {
                    hoverMessages.Add("You already have this upgrade or higher");
                }
                else
                {
                    hoverMessages.Add($"You require {costForLevel4} ore(s) to upgrade to this level");
                }
            }
            
            // Set the disabled options and hover messages
            customDropdown.SetOptions(new List<string> { "Old Nail", "Sharpened Nail", "Channelled Nail", "Coiled Nail", "Pure Nail" }, disabledOptions, hoverMessages);

            // Ensure the dropdown shows the correct current level
            customDropdown.SetValue(currentUpgradeLevel);
        }

        /// <summary>
        /// Calculates the cumulative ore cost to upgrade from current level to target level
        /// </summary>
        /// <param name="currentLevel">Current nail upgrade level</param>
        /// <param name="targetLevel">Target nail upgrade level</param>
        /// <returns>Total ore cost needed</returns>
        private static int GetCumulativeOreCost(int currentLevel, int targetLevel)
        {
            if (targetLevel <= currentLevel) return 0; // Already at or above target level
            
            int totalCost = 0;
            for (int level = currentLevel + 1; level <= targetLevel; level++)
            {
                totalCost += GetOreCostForLevel(level);
            }
            return totalCost;
        }

        /// <summary>
        /// Gets the ore cost for a specific nail level
        /// </summary>
        /// <param name="level">Nail upgrade level</param>
        /// <returns>Ore cost for that level</returns>
        private static int GetOreCostForLevel(int level)
        {
            switch (level)
            {
                case 0: return 0; // Old Nail
                case 1: return 0; // Sharpened Nail
                case 2: return 1; // Channelled Nail
                case 3: return 2; // Coiled Nail
                case 4: return 3; // Pure Nail
                default: return 0;
            }
        }

        /// <summary>
        /// Calculates the ore cost difference when changing from one nail level to another.
        /// Positive values mean ore is consumed, negative values mean ore is refunded.
        /// </summary>
        /// <param name="fromLevel">Current nail upgrade level</param>
        /// <param name="toLevel">Target nail upgrade level</param>
        /// <returns>Ore cost difference (positive = cost, negative = refund)</returns>
        private int CalculateOreCostDifference(int fromLevel, int toLevel)
        {
            // If upgrading to the same level, no cost difference
            if (fromLevel == toLevel) return 0;
            
            // Calculate cumulative cost for each level
            int fromCost = GetCumulativeOreCost(0, fromLevel);
            int toCost = GetCumulativeOreCost(0, toLevel);
            
            // The cost difference is what needs to be paid or refunded
            return toCost - fromCost;
        }
    }
}
