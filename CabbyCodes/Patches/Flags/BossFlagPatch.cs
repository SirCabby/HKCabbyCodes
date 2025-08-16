using CabbyCodes.Flags;
using CabbyCodes.Patches.BasePatches;
using CabbyCodes.Patches.Teleport;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Scenes;
using System.Collections.Generic;
using UnityEngine;
using CabbyMenu.SyncedReferences;

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
            Add(FlagInstances.hornet1Defeated, SceneInstances.Fungus1_04 , new Vector2(40, 36));
            Add(FlagInstances.defeatedMantisLords, SceneInstances.Fungus2_15, new Vector2(36, 8));
            Add(FlagInstances.mageLordDefeated, SceneInstances.Ruins1_24 , new Vector2(41, 30));
            Add(FlagInstances.elderHuDefeated, SceneInstances.Fungus2_32, new Vector2(46, 4));
            Add(FlagInstances.falseKnightDreamDefeated, SceneInstances.Crossroads_10, new Vector2(58, 48));
            Add(FlagInstances.galienDefeated, SceneInstances.Cliffs_01, new Vector2(50, 34));
            Add(FlagInstances.aladarSlugDefeated, SceneInstances.Cliffs_02, new Vector2(50, 34));
            Add(FlagInstances.noEyesDefeated, SceneInstances.Fungus1_35, new Vector2(46, 4));
            Add(FlagInstances.xeroDefeated, SceneInstances.RestingGrounds_02, new Vector2(93, 12));
            
            return locations;
        }

        protected override FlagDef[] GetFlags()
        {
            return new FlagDef[]
            {
                FlagInstances.falseKnightDefeated,
                FlagInstances.Crossroads_09__Mawlek_Body,
                FlagInstances.hornet1Defeated,
                FlagInstances.defeatedMantisLords,
                FlagInstances.mageLordDefeated,




                
                FlagInstances.falseKnightDreamDefeated,
                FlagInstances.mageLordDreamDefeated,
                FlagInstances.elderHuDefeated,
                FlagInstances.galienDefeated,
                FlagInstances.aladarSlugDefeated,
                FlagInstances.noEyesDefeated,
                FlagInstances.xeroDefeated
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
                if (flag == FlagInstances.mageLordDefeated)
                {
                    panels.Add(CreateMageLordPanel());
                    continue;
                }

                // Check if this boss has teleport coordinates defined
                if (bossLocations.TryGetValue(flag, out var bossTeleportLocation) && bossTeleportLocation != null)
                {
                    // Create panel with toggle/input and teleport button
                    var flagPatch = CreatePatch(flag);
                    if (flagPatch is BoolPatch boolPatch)
                    {
                        panels.Add(new ToggleWithTeleportPanel(
                            boolPatch, 
                            () => TeleportService.DoTeleport(bossTeleportLocation), 
                            GetDescription(flag)));
                    }
                    else if (flagPatch is IntPatch intPatch)
                    {
                        // For int-type boss flags, create panel with int input and teleport button
                        // Get validation data for proper min/max values
                        var validationData = FlagValidationData.GetIntValidationData(flag);
                        int minValue = validationData?.MinValue ?? 0;
                        int maxValue = validationData?.MaxValue ?? 999;
                        
                        panels.Add(new IntWithTeleportPanel(
                            intPatch, 
                            () => TeleportService.DoTeleport(bossTeleportLocation), 
                            GetDescription(flag),
                            minValue, maxValue));
                    }
                    else
                    {
                        // Fallback to regular panel if not a bool or int patch
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

        private ToggleWithTeleportPanel CreateMageLordPanel()
        {
            var flag = FlagInstances.mageLordDefeated;
            bossLocations.TryGetValue(flag, out var bossTeleportLocation);

            return new ToggleWithTeleportPanel(new DelegateReference<bool>(
                () => FlagManager.GetBoolFlag(flag),
                value =>
                {
                    FlagManager.SetBoolFlag(flag, value);
                    FlagManager.SetBoolFlag(FlagInstances.mageLordEncountered, value);
                    FlagManager.SetBoolFlag(FlagInstances.mageLordEncountered_2, value);
                }
            ), () => TeleportService.DoTeleport(bossTeleportLocation), flag.ReadableName);
        }
    }
} 