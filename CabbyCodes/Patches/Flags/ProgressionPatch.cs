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