using CabbyCodes.Patches.BasePatches;
using CabbyCodes.Flags;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.SyncedReferences;
using CabbyCodes.Scenes;
using System;
using System.Collections.Generic;
using CabbyCodes.SavedGames;

namespace CabbyCodes.Patches.Inventory
{
    public class InventoryPatch : BasePatch
    {
        // Static state tracking for heart piece changes that require save/reload
        private static int startingMaxHealth = -1;
        private static int startingMaxHealthBase = -1;
        private static bool hasInitializedStartingState = false;

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

            // Wanderer's Journals section
            panels.Add(new InfoPanel("Wanderer's Journals").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateWanderersJournalPanels());

            // Map Accessories section
            panels.Add(new InfoPanel("Map Accessories").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateMapAccessoryPanels());

            // Mask Shards section
            panels.Add(new InfoPanel("Mask Shards").SetColor(CheatPanel.subHeaderColor));
            panels.Add(new InfoPanel("May require restart to take effect").SetColor(CheatPanel.warningColor));
            panels.AddRange(CreateMaskShardPanels());

            // Vessel Fragments section
            panels.Add(new InfoPanel("Vessel Fragments").SetColor(CheatPanel.subHeaderColor));
            panels.Add(new InfoPanel("May require restart to take effect").SetColor(CheatPanel.warningColor));
            panels.AddRange(CreateVesselFragmentPanels());

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
                new BoolPatch(FlagInstances.hasKingsBrand).CreatePanel(),
                CreateSimpleKeyPanel(FlagInstances.slySimpleKey)
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
                CreateHeartPiecePanel(FlagInstances.Crossroads_13__Heart_Piece),
                CreateHeartPiecePanel(FlagInstances.Fungus1_36__Heart_Piece),
                CreateHeartPiecePanel(FlagInstances.Fungus2_01__Heart_Piece),
                CreateHeartPiecePanel(FlagInstances.slyShellFrag1),
                CreateHeartPiecePanel(FlagInstances.slyShellFrag2),
                CreateHeartPiecePanel(FlagInstances.slyShellFrag3),
                CreateHeartPiecePanel(FlagInstances.slyShellFrag4),
            };
            
            return panels;
        }

        private List<CheatPanel> CreateVesselFragmentPanels()
        {
            var panels = new List<CheatPanel>
            {
                CreateVesselFragmentPanel(FlagInstances.slyVesselFrag1),
                CreateVesselFragmentPanel(FlagInstances.slyVesselFrag2),
                CreateVesselFragmentPanel(FlagInstances.slyVesselFrag3),
                CreateVesselFragmentPanel(FlagInstances.slyVesselFrag4),
            };
            
            return panels;
        }

        private TogglePanel CreateHeartPiecePanel(FlagDef heartPieceFlag)
        {
            return new TogglePanel(new DelegateReference<bool>(
                () => FlagManager.GetBoolFlag(heartPieceFlag),
                value =>
                {
                    // Initialize starting state if not done yet
                    if (!hasInitializedStartingState)
                    {
                        startingMaxHealth = FlagManager.GetIntFlag(FlagInstances.maxHealth);
                        startingMaxHealthBase = FlagManager.GetIntFlag(FlagInstances.maxHealthBase);
                        hasInitializedStartingState = true;
                    }

                    FlagManager.SetBoolFlag(heartPieceFlag, value);
                    
                    var currentHeartPieces = FlagManager.GetIntFlag(FlagInstances.heartPieces);
                    var newHeartPieces = currentHeartPieces;
                    
                    if (value)
                    {
                        // Increment heart pieces when enabling
                        newHeartPieces = currentHeartPieces + 1;
                        
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
                        newHeartPieces = currentHeartPieces - 1;
                        
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
            ), heartPieceFlag.ReadableName);
        }

        private TogglePanel CreateVesselFragmentPanel(FlagDef vesselFragmentFlag)
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
                        }
                    }
                    
                    FlagManager.SetIntFlag(FlagInstances.vesselFragments, newVesselFragments);
                }
            ), vesselFragmentFlag.ReadableName);
        }

        private TogglePanel CreateSimpleKeyPanel(FlagDef simpleKeyFlag)
        {
            return new TogglePanel(new DelegateReference<bool>(
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
                }
            ), simpleKeyFlag.ReadableName);
        }

        // Wanderer's Journals panels
        private List<CheatPanel> CreateWanderersJournalPanels()
        {
            var panels = new List<CheatPanel>
            {
                CreateWanderersJournalPanel(FlagInstances.Fungus1_22__Shiny_Item),
            };

            return panels;
        }

        private TogglePanel CreateWanderersJournalPanel(FlagDef journalFlag)
        {
            // Determine human-readable label automatically from scene metadata
            string label = journalFlag.SceneName == "Global"
                ? journalFlag.ReadableName
                : SceneManagement.GetSceneData(journalFlag.SceneName)?.ReadableName ?? journalFlag.SceneName;

            return new TogglePanel(new DelegateReference<bool>(
                () => FlagManager.GetBoolFlag(journalFlag),
                value =>
                {
                    FlagManager.SetBoolFlag(journalFlag, value);

                    int currentJournals = FlagManager.GetIntFlag(FlagInstances.trinket1);
                    int newJournals = value ? currentJournals + 1 : Math.Max(0, currentJournals - 1);

                    FlagManager.SetIntFlag(FlagInstances.trinket1, newJournals);

                    // foundTrinket1 should be true if any journals are collected
                    FlagManager.SetBoolFlag(FlagInstances.foundTrinket1, newJournals > 0);
                }
            ), label);
        }
    }
}
