using CabbyCodes.Patches.BasePatches;
using CabbyCodes.Flags;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.SyncedReferences;
using System;
using System.Collections.Generic;

namespace CabbyCodes.Patches.Inventory
{
    public class InventoryPatch : BasePatch
    {
        public static void AddPanels()
        {
            var inventoryPatch = new InventoryPatch();
            var panels = inventoryPatch.CreatePanels();
            foreach (var panel in panels)
            {
                CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
            }
        }

        public override List<CheatPanel> CreatePanels()
        {
            var panels = new List<CheatPanel>
            {
                new InfoPanel("Inventory").SetColor(CheatPanel.headerColor),
                // Currency section
                new InfoPanel("Currency").SetColor(CheatPanel.subHeaderColor)
            };
            panels.AddRange(CreateCurrencyPanels());

            // Ability Items section
            panels.Add(new InfoPanel("Ability Items").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateAbilityPanels());

            // Nail Arts section
            panels.Add(new InfoPanel("Nail Arts").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNailArtPanels());

            // Spells section
            panels.Add(new InfoPanel("Spells").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateSpellPanels());

            // Items section
            panels.Add(new InfoPanel("Items").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateItemPanels());

            // Keys section
            panels.Add(new InfoPanel("Keys").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateKeyPanels());

            // Map Accessories section
            panels.Add(new InfoPanel("Map Accessories").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateMapAccessoryPanels());

            // Mask Shards section
            panels.Add(new InfoPanel("Mask Shards").SetColor(CheatPanel.subHeaderColor));
            panels.Add(new InfoPanel("May require restart to take effect").SetColor(CheatPanel.warningColor));
            panels.AddRange(CreateMaskShardPanels());

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
                new DropdownPanel(new MothwingCloakReference(), FlagInstances.hasDash.ReadableName + " / " + FlagInstances.hasShadowDash.ReadableName, Constants.DEFAULT_PANEL_HEIGHT),
                new BoolPatch(FlagInstances.hasDoubleJump).CreatePanel(),
                new BoolPatch(FlagInstances.hasWalljump).CreatePanel(),
                new BoolPatch(FlagInstances.hasSuperDash).CreatePanel(),
                new BoolPatch(FlagInstances.hasAcidArmour).CreatePanel(),
                new DropdownPanel(new DreamNailReference(), FlagInstances.hasDreamNail.ReadableName + " / " + FlagInstances.dreamNailUpgraded.ReadableName, Constants.DEFAULT_PANEL_HEIGHT),
                new BoolPatch(FlagInstances.hasDreamGate).CreatePanel(),
                                        new BoolPatch(FlagInstances.unlockedCompletionRate).CreatePanel(),
                new BoolPatch(FlagInstances.salubraBlessing).CreatePanel()
            };

            return panels;
        }

        private List<CheatPanel> CreateNailArtPanels()
        {
            var panels = new List<CheatPanel>
            {
                new BoolPatch(FlagInstances.hasCyclone).CreatePanel(),
                new BoolPatch(FlagInstances.hasUpwardSlash).CreatePanel(),
                new BoolPatch(FlagInstances.hasDashSlash).CreatePanel()
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
                new DropdownPanel(new DelicateFlowerReference(), FlagInstances.hasXunFlower.ReadableName, Constants.DEFAULT_PANEL_HEIGHT),
                new BoolPatch(FlagInstances.hasGodfinder).CreatePanel()
            };

            return panels;
        }

        private List<CheatPanel> CreateKeyPanels()
        {
            var panels = new List<CheatPanel>
            {
                new BoolPatch(FlagInstances.hasWhiteKey).CreatePanel(),
                new BoolPatch(FlagInstances.hasLoveKey).CreatePanel(),
                new BoolPatch(FlagInstances.hasSlykey).CreatePanel(),
                new BoolPatch(FlagInstances.hasCityKey).CreatePanel(),
                new BoolPatch(FlagInstances.hasKingsBrand).CreatePanel()
            };

            return panels;
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

        private List<CheatPanel> CreateMaskShardPanels()
        {
            var panels = new List<CheatPanel>
            {
                CreateHeartPiecePanel(FlagInstances.Crossroads_09__Heart_Piece),
                CreateHeartPiecePanel(FlagInstances.Fungus1_36__Heart_Piece)
            };
            
            return panels;
        }

        private TogglePanel CreateHeartPiecePanel(FlagDef heartPieceFlag)
        {
            return new TogglePanel(new DelegateReference<bool>(
                () => FlagManager.GetBoolFlag(heartPieceFlag),
                value =>
                {
                    FlagManager.SetBoolFlag(heartPieceFlag, value);
                    
                    var currentHeartPieces = FlagManager.GetIntFlag(FlagInstances.heartPieces);
                    if (value)
                    {
                        // Increment heart pieces when enabling
                        FlagManager.SetIntFlag(FlagInstances.heartPieces, currentHeartPieces + 1);
                    }
                    else
                    {
                        // Decrement heart pieces when disabling
                        FlagManager.SetIntFlag(FlagInstances.heartPieces, Math.Max(0, currentHeartPieces - 1));
                    }
                    
                    // Update heartPieceCollected flag based on count
                    var newHeartPieces = FlagManager.GetIntFlag(FlagInstances.heartPieces);
                    FlagManager.SetBoolFlag(FlagInstances.heartPieceCollected, newHeartPieces > 0);
                }
            ), heartPieceFlag.ReadableName);
        }
    }
}
