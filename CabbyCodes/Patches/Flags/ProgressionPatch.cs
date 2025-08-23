using CabbyCodes.Flags;
using CabbyCodes.Patches.BasePatches;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Patches.Teleport;
using CabbyMenu.SyncedReferences;
using System;
using System.Collections.Generic;
using CabbyCodes.Scenes;
using UnityEngine;

namespace CabbyCodes.Patches.Flags
{
    public class ProgressionPatch : BasePatch
    {
        /// <summary>
        /// Dictionary of dreamer locations with their teleport coordinates
        /// </summary>
        private static readonly Dictionary<string, TeleportLocation> teleportLocations = TeleportLocations();

        /// <summary>
        /// Creates the dreamer locations dictionary with teleport coordinates
        /// </summary>
        private static Dictionary<string, TeleportLocation> TeleportLocations()
        {
            var locations = new Dictionary<string, TeleportLocation>
            {
                [FlagInstances.hegemolDefeated.ReadableName] = new TeleportLocation(SceneInstances.Deepnest_Spider_Town, new Vector2(58, 153)),
                [FlagInstances.lurienDefeated.ReadableName] = new TeleportLocation(SceneInstances.Ruins2_Watcher_Room, new Vector2(52, 136))
            };
            
            return locations;
        }

        public override List<CheatPanel> CreatePanels()
        {
            var panels = new List<CheatPanel>
            {
                new InfoPanel("Dreamers").SetColor(CheatPanel.subHeaderColor)
            };
            panels.AddRange(CreateDreamerPanels(new[] {
                new[] { FlagInstances.hegemolDefeated, FlagInstances.maskBrokenHegemol },
                new[] { FlagInstances.lurienDefeated, FlagInstances.maskBrokenLurien },
                new[] { FlagInstances.monomonDefeated, FlagInstances.maskBrokenMonomon }
            }));

            panels.Add(new InfoPanel("Colosseum").SetColor(CheatPanel.subHeaderColor));
            panels.Add(new DropdownPanel(CreateColosseumValueList(), "Colosseum Status", Constants.DEFAULT_PANEL_HEIGHT));

            panels.Add(new InfoPanel("Godhome").SetColor(CheatPanel.headerColor));
            panels.AddRange(CreateBossStatuePanels());

            return panels;
        }

        /// <summary>
        /// Creates a DelegateValueList for managing colosseum flags through a dropdown
        /// </summary>
        private DelegateValueList CreateColosseumValueList()
        {
            return new DelegateValueList(
                // Getter: returns the current colosseum state as an integer
                () =>
                {
                    // Check each colosseum tier and return appropriate value
                    if (FlagManager.GetBoolFlag(FlagInstances.colosseumGoldCompleted))
                        return 6; // Gold completed
                    if (FlagManager.GetBoolFlag(FlagInstances.colosseumGoldOpened))
                        return 5; // Gold opened
                    if (FlagManager.GetBoolFlag(FlagInstances.colosseumSilverCompleted))
                        return 4; // Silver completed
                    if (FlagManager.GetBoolFlag(FlagInstances.colosseumSilverOpened))
                        return 3; // Silver opened
                    if (FlagManager.GetBoolFlag(FlagInstances.colosseumBronzeCompleted))
                        return 2; // Bronze completed
                    if (FlagManager.GetBoolFlag(FlagInstances.colosseumBronzeOpened))
                        return 1; // Bronze opened
                    return 0; // None opened
                },
                // Setter: sets the colosseum flags based on the selected value
                (value) =>
                {
                    // Set flags based on selected value
                    switch (value)
                    {
                        case 0: // None opened
                            FlagManager.SetBoolFlag(FlagInstances.colosseumBronzeOpened, false);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumBronzeCompleted, false);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumSilverOpened, false);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumSilverCompleted, false);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumGoldOpened, false);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumGoldCompleted, false);
                            break;
                        case 1: // Bronze opened
                            FlagManager.SetBoolFlag(FlagInstances.colosseumBronzeOpened, true);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumBronzeCompleted, false);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumSilverOpened, false);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumSilverCompleted, false);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumGoldOpened, false);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumGoldCompleted, false);
                            break;
                        case 2: // Bronze completed
                            FlagManager.SetBoolFlag(FlagInstances.colosseumBronzeOpened, true);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumBronzeCompleted, true);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumSilverOpened, false);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumSilverCompleted, false);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumGoldOpened, false);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumGoldCompleted, false);
                            break;
                        case 3: // Silver opened
                            FlagManager.SetBoolFlag(FlagInstances.colosseumBronzeOpened, true);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumBronzeCompleted, true);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumSilverOpened, true);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumSilverCompleted, false);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumGoldOpened, false);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumGoldCompleted, false);
                            break;
                        case 4: // Silver completed
                            FlagManager.SetBoolFlag(FlagInstances.colosseumBronzeOpened, true);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumBronzeCompleted, true);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumSilverOpened, true);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumSilverCompleted, true);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumGoldOpened, false);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumGoldCompleted, false);
                            break;
                        case 5: // Gold opened
                            FlagManager.SetBoolFlag(FlagInstances.colosseumBronzeOpened, true);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumBronzeCompleted, true);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumSilverOpened, true);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumSilverCompleted, true);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumGoldOpened, true);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumGoldCompleted, false);
                            break;
                        case 6: // Gold completed
                            FlagManager.SetBoolFlag(FlagInstances.colosseumBronzeOpened, true);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumBronzeCompleted, true);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumSilverOpened, true);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumSilverCompleted, true);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumGoldOpened, true);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumGoldCompleted, true);
                            break;
                    }
                },
                // List provider: returns the list of available colosseum states
                () => new List<string>
                {
                    "None Opened",
                    "Bronze Opened",
                    "Bronze Completed",
                    "Silver Opened",
                    "Silver Completed",
                    "Gold Opened",
                    "Gold Completed"
                }
            );
        }

        /// <summary>
        /// Creates panels for managing boss statue states
        /// </summary>
        private List<CheatPanel> CreateBossStatuePanels()
        {
            var panels = new List<CheatPanel>();
            
            // Group boss statues by category for better organization
            var bossGroups = new[]
            {
                // Main Story Bosses
                new[] { 
                    FlagInstances.statueStateFalseKnight, 
                    FlagInstances.statueStateHornet1, 
                    FlagInstances.statueStateHornet2,
                    FlagInstances.statueStateSoulMaster,
                    FlagInstances.statueStateSoulTyrant,
                    FlagInstances.statueStateMantisLords,
                    FlagInstances.statueStateMantisLordsExtra,
                    FlagInstances.statueStateWatcherKnights,
                    FlagInstances.statueStateUumuu,
                    FlagInstances.statueStateDungDefender,
                    FlagInstances.statueStateWhiteDefender,
                    FlagInstances.statueStateHollowKnight,
                    FlagInstances.statueStateRadiance
                },
                // Optional Bosses
                new[] { 
                    FlagInstances.statueStateBroodingMawlek,
                    FlagInstances.statueStateGruzMother,
                    FlagInstances.statueStateMegaMossCharger,
                    FlagInstances.statueStateVengefly,
                    FlagInstances.statueStateBrokenVessel,
                    FlagInstances.statueStateLostKin,
                    FlagInstances.statueStateNosk,
                    FlagInstances.statueStateNoskHornet,
                    FlagInstances.statueStateFlukemarm,
                    FlagInstances.statueStateCollector,
                    FlagInstances.statueStateHiveKnight,
                    FlagInstances.statueStateTraitorLord
                },
                // Dream Bosses
                new[] { 
                    FlagInstances.statueStateGorb,
                    FlagInstances.statueStateElderHu,
                    FlagInstances.statueStateGalien,
                    FlagInstances.statueStateMarkoth,
                    FlagInstances.statueStateMarmu,
                    FlagInstances.statueStateNoEyes,
                    FlagInstances.statueStateXero
                },
                // DLC Bosses
                new[] { 
                    FlagInstances.statueStateGrimm,
                    FlagInstances.statueStateNightmareGrimm,
                    FlagInstances.statueStateGreyPrince,
                    FlagInstances.statueStateGodTamer
                },
                // Other
                new[] { 
                    FlagInstances.statueStateSly,
                    FlagInstances.statueStateNailmasters,
                    FlagInstances.statueStateMageKnight,
                    FlagInstances.statueStatePaintmaster,
                    FlagInstances.statueStateZote
                }
            };

            var groupNames = new[] { "Main Story", "Optional", "Dream Warriors", "DLC", "Other" };

            for (int i = 0; i < bossGroups.Length; i++)
            {
                panels.Add(new InfoPanel(groupNames[i]).SetColor(CheatPanel.subHeaderColor));
                panels.AddRange(CreateBossStatueGroupPanels(bossGroups[i]));
            }

            return panels;
        }

        /// <summary>
        /// Creates panels for a group of boss statues
        /// </summary>
        private List<CheatPanel> CreateBossStatueGroupPanels(FlagDef[] bossFlags)
        {
            var panels = new List<CheatPanel>();
            
            foreach (var bossFlag in bossFlags)
            {
                var bossName = bossFlag.ReadableName.Replace("statueState", "");
                panels.Add(new DropdownPanel(CreateBossStatueValueList(bossFlag), bossName, Constants.DEFAULT_PANEL_HEIGHT));
            }
            
            return panels;
        }

        /// <summary>
        /// Creates a DelegateValueList for managing a boss statue's completion state
        /// </summary>
        private DelegateValueList CreateBossStatueValueList(FlagDef bossFlag)
        {
            return new DelegateValueList(
                // Getter: returns the current boss statue state as an integer
                () =>
                {
                    var bossData = FlagManager.GetCompletionFlag(bossFlag);
                    if (bossData == null) return 0;
                    
                    // Use reflection to get the actual field values from the game's BossStatue+Completion class
                    var bossType = bossData.GetType();
                    
                    // Get the completion tier fields using the actual field names we discovered
                    var completedTier3 = GetBoolField(bossData, "completedTier3");
                    var completedTier2 = GetBoolField(bossData, "completedTier2");
                    var completedTier1 = GetBoolField(bossData, "completedTier1");
                    var isUnlocked = GetBoolField(bossData, "isUnlocked");
                    var hasBeenSeen = GetBoolField(bossData, "hasBeenSeen");
                    
                    // Return the highest completion level achieved
                    if (completedTier3) return 5; // Tier 3 completed
                    if (completedTier2) return 4; // Tier 2 completed
                    if (completedTier1) return 3; // Tier 1 completed
                    if (isUnlocked) return 2; // Unlocked
                    if (hasBeenSeen) return 1; // Seen
                    return 0; // Not seen
                },
                // Setter: sets the boss statue flags based on the selected completion level
                (value) =>
                {
                    var bossData = FlagManager.GetCompletionFlag(bossFlag);
                    if (bossData == null) return;
                    
                    // Set all fields based on the selected level
                    switch (value)
                    {
                        case 0: // Not seen
                            SetBoolField(bossData, "hasBeenSeen", false);
                            SetBoolField(bossData, "isUnlocked", false);
                            SetBoolField(bossData, "completedTier1", false);
                            SetBoolField(bossData, "completedTier2", false);
                            SetBoolField(bossData, "completedTier3", false);
                            break;
                        case 1: // Seen
                            SetBoolField(bossData, "hasBeenSeen", true);
                            SetBoolField(bossData, "isUnlocked", false);
                            SetBoolField(bossData, "completedTier1", false);
                            SetBoolField(bossData, "completedTier2", false);
                            SetBoolField(bossData, "completedTier3", false);
                            break;
                        case 2: // Unlocked
                            SetBoolField(bossData, "hasBeenSeen", true);
                            SetBoolField(bossData, "isUnlocked", true);
                            SetBoolField(bossData, "completedTier1", false);
                            SetBoolField(bossData, "completedTier2", false);
                            SetBoolField(bossData, "completedTier3", false);
                            break;
                        case 3: // Tier 1 completed
                            SetBoolField(bossData, "hasBeenSeen", true);
                            SetBoolField(bossData, "isUnlocked", true);
                            SetBoolField(bossData, "completedTier1", true);
                            SetBoolField(bossData, "completedTier2", false);
                            SetBoolField(bossData, "completedTier3", false);
                            break;
                        case 4: // Tier 2 completed
                            SetBoolField(bossData, "hasBeenSeen", true);
                            SetBoolField(bossData, "isUnlocked", true);
                            SetBoolField(bossData, "completedTier1", true);
                            SetBoolField(bossData, "completedTier2", true);
                            SetBoolField(bossData, "completedTier3", false);
                            break;
                        case 5: // Tier 3 completed
                            SetBoolField(bossData, "hasBeenSeen", true);
                            SetBoolField(bossData, "isUnlocked", true);
                            SetBoolField(bossData, "completedTier1", true);
                            SetBoolField(bossData, "completedTier2", true);
                            SetBoolField(bossData, "completedTier3", true);
                            break;
                    }
                    
                    // Update the PlayerData field with the modified boss data
                    FlagManager.SetCompletionFlag(bossFlag, bossData);
                },
                // Options for the dropdown
                () => new List<string> { "Not Seen", "Seen", "Unlocked", "Tier 1 Completed", "Tier 2 Completed", "Tier 3 Completed" }
            );
        }
        
        /// <summary>
        /// Helper method to safely get a boolean field value using reflection
        /// </summary>
        private bool GetBoolField(object obj, string fieldName)
        {
            try
            {
                var field = obj.GetType().GetField(fieldName);
                if (field != null && field.FieldType == typeof(bool))
                {
                    return (bool)field.GetValue(obj);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[Boss Statue] Failed to get field {fieldName}: {ex.Message}");
            }
            return false;
        }
        
        /// <summary>
        /// Helper method to safely set a boolean field value using reflection
        /// </summary>
        private void SetBoolField(object obj, string fieldName, bool value)
        {
            try
            {
                var field = obj.GetType().GetField(fieldName);
                if (field != null && field.FieldType == typeof(bool))
                {
                    field.SetValue(obj, value);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[Boss Statue] Failed to set field {fieldName}: {ex.Message}");
            }
        }

        private List<CheatPanel> CreateDreamerPanels(FlagDef[][] flagGroups)
        {
            var panels = new List<CheatPanel>();
            foreach (var flagGroup in flagGroups)
            {
                if (flagGroup.Length > 0)
                {
                    // Create a synced reference that handles all flags in the group
                    var syncedReference = new DelegateReference<bool>(
                        () => FlagManager.GetBoolFlag(flagGroup[0]),
                        (newValue) =>
                        {
                            foreach (var flag in flagGroup)
                            {
                                FlagManager.SetBoolFlag(flag, newValue);
                            }
                            
                            // Handle mask and guardian flag increments/decrements
                            if (newValue)
                            {
                                // Toggle ON: increment both flags by 1
                                var currentGuardianValue = FlagManager.GetIntFlag(FlagInstances.guardiansDefeated);
                                FlagManager.SetIntFlag(FlagInstances.guardiansDefeated, currentGuardianValue + 1);
                            }
                            else
                            {
                                // Toggle OFF: decrement both flags by 1
                                var currentGuardianValue = FlagManager.GetIntFlag(FlagInstances.guardiansDefeated);
                                FlagManager.SetIntFlag(FlagInstances.guardiansDefeated, Math.Max(0, currentGuardianValue - 1));
                            }
                        }
                    );
                    
                    // Check if this dreamer has teleport coordinates defined
                    var dreamerName = flagGroup[0].ReadableName;
                    if (teleportLocations.TryGetValue(dreamerName, out var teleportLocation) && teleportLocation != null)
                    {
                        // Create panel with toggle and teleport button
                        panels.Add(new ToggleWithTeleportPanel(
                            syncedReference, 
                            () => TeleportService.DoTeleport(teleportLocation), 
                            dreamerName));
                    }
                    else
                    {
                        // Create panel with just toggle (no teleport button)
                        var panel = new TogglePanel(syncedReference, dreamerName);
                        panels.Add(panel);
                    }
                }
            }
            return panels;
        }
    }
} 