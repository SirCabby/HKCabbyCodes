using CabbyCodes.Flags;
using CabbyCodes.Patches.BasePatches;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Patches.Teleport;
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
        private static readonly Dictionary<string, TeleportLocation> dreamerLocations = CreateDreamerLocations();

        /// <summary>
        /// Creates the dreamer locations dictionary with teleport coordinates
        /// </summary>
        private static Dictionary<string, TeleportLocation> CreateDreamerLocations()
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

            return panels;
        }

        private List<CheatPanel> CreateDreamerPanels(FlagDef[][] flagGroups)
        {
            var panels = new List<CheatPanel>();
            foreach (var flagGroup in flagGroups)
            {
                if (flagGroup.Length > 0)
                {
                    // Create a synced reference that handles all flags in the group
                    var syncedReference = new CabbyMenu.SyncedReferences.DelegateReference<bool>(
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
                    if (dreamerLocations.TryGetValue(dreamerName, out var teleportLocation) && teleportLocation != null)
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