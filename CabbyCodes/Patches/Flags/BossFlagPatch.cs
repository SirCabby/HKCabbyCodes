using CabbyCodes.Flags;
using CabbyCodes.Patches.BasePatches;
using CabbyCodes.Patches.Teleport;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Scenes;
using System.Collections.Generic;
using UnityEngine;

namespace CabbyCodes.Patches.Flags
{
    public class BossFlagPatch : BasePatch
    {
        /// <summary>
        /// Dictionary of boss locations with their teleport coordinates
        /// </summary>
        private static readonly Dictionary<FlagDef, TeleportLocation> bossLocations = CreateBossLocations();

        /// <summary>
        /// Creates the boss locations dictionary with teleport coordinates
        /// </summary>
        private static Dictionary<FlagDef, TeleportLocation> CreateBossLocations()
        {
            var locations = new Dictionary<FlagDef, TeleportLocation>();
            
            // Helper method to add a flag with optional teleport coordinates
            void Add(FlagDef flag, SceneMapData scene, Vector2? teleportCoords = null) => locations[flag] = teleportCoords.HasValue ? new TeleportLocation(scene, teleportCoords.Value) : null;
            
            // Boss Flags
            Add(FlagInstances.falseKnightDefeated, SceneInstances.Crossroads_10, new Vector2(57, 28));
            Add(FlagInstances.Crossroads_09__Mawlek_Body, SceneInstances.Crossroads_09, new Vector2(39, 5));
            
            return locations;
        }

        protected override FlagDef[] GetFlags()
        {
            return new FlagDef[]
            {
                FlagInstances.falseKnightDefeated,
                FlagInstances.Crossroads_09__Mawlek_Body,
            };
        }

        /// <summary>
        /// Override CreatePanels to support teleport functionality for boss flags
        /// </summary>
        /// <returns>List of panels to display</returns>
        public override List<CheatPanel> CreatePanels()
        {
            var flags = GetFlags();
            var panels = new List<CheatPanel>();
            
            foreach (var flag in flags)
            {
                // Check if this boss has teleport coordinates defined
                if (bossLocations.TryGetValue(flag, out var bossTeleportLocation) && bossTeleportLocation != null)
                {
                    // Create panel with toggle and teleport button
                    var flagPatch = CreatePatch(flag);
                    if (flagPatch is BoolPatch boolPatch)
                    {
                        panels.Add(new ToggleWithTeleportPanel(
                            boolPatch, 
                            () => TeleportService.DoTeleport(bossTeleportLocation), 
                            GetDescription(flag)));
                    }
                    else
                    {
                        // Fallback to regular panel if not a bool patch
                        panels.Add(flagPatch.CreatePanel());
                    }
                }
                else
                {
                    // Create panel with just toggle (no teleport button)
                    var flagPatch = CreatePatch(flag);
                    panels.Add(flagPatch.CreatePanel());
                }
            }
            
            return panels;
        }
    }
} 