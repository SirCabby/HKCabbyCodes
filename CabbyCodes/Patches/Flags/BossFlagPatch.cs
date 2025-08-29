using CabbyCodes.Flags;
using CabbyCodes.Patches.BasePatches;
using CabbyCodes.Patches.Teleport;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Scenes;
using System;
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
            Add(FlagInstances.aladarSlugDefeated, SceneInstances.Cliffs_02, new Vector2(50, 34));
            Add(FlagInstances.collectorDefeated, SceneInstances.Ruins2_11, new Vector2(36, 96));
            Add(FlagInstances.Crossroads_09__Mawlek_Body, SceneInstances.Crossroads_09, new Vector2(39, 5));
            Add(FlagInstances.Deepnest_32__Battle_Scene, SceneInstances.Deepnest_32, new Vector2(59, 5));
            Add(FlagInstances.defeatedMantisLords, SceneInstances.Fungus2_15, new Vector2(36, 8));
            Add(FlagInstances.defeatedDungDefender, SceneInstances.Waterways_05, new Vector2(57, 12));
            Add(FlagInstances.elderHuDefeated, SceneInstances.Fungus2_32, new Vector2(46, 4));
            Add(FlagInstances.falseKnightDefeated, SceneInstances.Crossroads_10, new Vector2(57, 28));
            Add(FlagInstances.falseKnightDreamDefeated, SceneInstances.Crossroads_10, new Vector2(58, 48));
            Add(FlagInstances.Fungus3_23__Battle_Scene, SceneInstances.Fungus3_23, new Vector2(46, 22));
            Add(FlagInstances.galienDefeated, SceneInstances.Cliffs_01, new Vector2(50, 34));
            Add(FlagInstances.hornet1Defeated, SceneInstances.Fungus1_04 , new Vector2(40, 36));
            Add(FlagInstances.hornetOutskirtsDefeated, SceneInstances.Deepnest_East_Hornet, new Vector2(10, 29));
            Add(FlagInstances.infectedKnightDreamDefeated, SceneInstances.Abyss_20, new Vector2(28, 29));
            Add(FlagInstances.infectedKnightEncountered, SceneInstances.Abyss_19, new Vector2(44, 29));
            Add(FlagInstances.mageLordDefeated, SceneInstances.Ruins1_24 , new Vector2(41, 30));
            Add(FlagInstances.mageLordDreamDefeated, SceneInstances.Ruins1_24, new Vector2(32, 11));
            Add(FlagInstances.defeatedMegaJelly, SceneInstances.Fungus3_archive_02, new Vector2(54, 111));
            Add(FlagInstances.Mines_32__Battle_Scene, SceneInstances.Mines_32, new Vector2(44, 12));
            Add(FlagInstances.mumCaterpillarDefeated, SceneInstances.Fungus3_40, new Vector2(63, 11));
            Add(FlagInstances.noEyesDefeated, SceneInstances.Fungus1_35, new Vector2(46, 4));
            Add(FlagInstances.xeroDefeated, SceneInstances.RestingGrounds_02, new Vector2(93, 12));
            
            // Flamebearer Flags
            Add(FlagInstances.Abyss_02__Flamebearer_Spawn, SceneInstances.Abyss_02, new Vector2(100, 18));
            Add(FlagInstances.Deepnest_East_03__Flamebearer_Spawn, SceneInstances.Deepnest_32, new Vector2(37, 77));
            Add(FlagInstances.Fungus1_10__Flamebearer_Spawn, SceneInstances.Fungus1_10, new Vector2(81, 9));
            Add(FlagInstances.Fungus2_30__Flamebearer_Spawn, SceneInstances.Fungus2_30, new Vector2(22, 34));
            Add(FlagInstances.Hive_03__Flamebearer_Spawn, SceneInstances.Hive_03, new Vector2(85, 123));
            Add(FlagInstances.Mines_10__Flamebearer_Spawn, SceneInstances.Mines_10, new Vector2(111, 7));
            Add(FlagInstances.RestingGrounds_06__Flamebearer_Spawn, SceneInstances.RestingGrounds_06, new Vector2(27, 20));
            Add(FlagInstances.Ruins1_28__Flamebearer_Spawn, SceneInstances.Ruins1_28, new Vector2(55, 19));
            Add(FlagInstances.Tutorial_01__Flamebearer_Spawn, SceneInstances.Tutorial_01, new Vector2(142, 13));
            
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
                FlagInstances.infectedKnightEncountered,
                FlagInstances.Mines_32__Battle_Scene,
                FlagInstances.hornetOutskirtsDefeated,
                FlagInstances.Fungus3_23__Battle_Scene,
                FlagInstances.collectorDefeated,
                FlagInstances.flukeMotherDefeated,
                FlagInstances.defeatedDungDefender,
                FlagInstances.defeatedMegaJelly,
                FlagInstances.Deepnest_32__Battle_Scene,


                FlagInstances.falseKnightDreamDefeated,
                FlagInstances.mageLordDreamDefeated,
                FlagInstances.infectedKnightDreamDefeated,


                FlagInstances.elderHuDefeated,
                FlagInstances.galienDefeated,
                FlagInstances.aladarSlugDefeated,
                FlagInstances.mumCaterpillarDefeated,
                FlagInstances.noEyesDefeated,
                FlagInstances.xeroDefeated,


                FlagInstances.Mines_10__Flamebearer_Spawn,
                FlagInstances.Ruins1_28__Flamebearer_Spawn,
                FlagInstances.Fungus1_10__Flamebearer_Spawn,
                FlagInstances.Tutorial_01__Flamebearer_Spawn,
                FlagInstances.RestingGrounds_06__Flamebearer_Spawn,
                FlagInstances.Deepnest_East_03__Flamebearer_Spawn,
                FlagInstances.foughtGrimm,
                FlagInstances.Hive_03__Flamebearer_Spawn,
                FlagInstances.Abyss_02__Flamebearer_Spawn,
                FlagInstances.Fungus2_30__Flamebearer_Spawn,
                FlagInstances.defeatedNightmareGrimm,
                
                FlagInstances.greyPrinceDefeated,
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
                    panels.Add(CreateBossPanelWithDependentFlags(flag, new[] { 
                        FlagInstances.mageLordEncountered, 
                        FlagInstances.mageLordEncountered_2 
                    }));
                    continue;
                }
                else if (flag == FlagInstances.infectedKnightDreamDefeated)
                {
                    panels.Add(CreateBossPanelWithDependentFlags(flag, new[] { 
                        FlagInstances.infectedKnightOrbsCollected 
                    }));
                    continue;
                }
                else if (flag == FlagInstances.flukeMotherDefeated)
                {
                    panels.Add(CreateBossPanelWithDependentFlags(flag, new[] { 
                        FlagInstances.flukeMotherEncountered 
                    }));
                    continue;
                }
                else if (flag == FlagInstances.Deepnest_32__Battle_Scene)
                {
                    panels.Add(CreateBossPanelWithDependentFlags(flag, new[] { 
                        FlagInstances.encounteredMimicSpider 
                    }));
                    continue;
                }

                // Check if this is a flamebearer flag
                if (IsFlamebearerFlag(flag))
                {
                    panels.Add(CreateFlamebearerPanel(flag));
                }
                // Check if this boss has teleport coordinates defined
                else if (bossLocations.TryGetValue(flag, out var bossTeleportLocation) && bossTeleportLocation != null)
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

        /// <summary>
        /// Checks if a flag is a flamebearer flag
        /// </summary>
        /// <param name="flag">The flag to check</param>
        /// <returns>True if the flag is a flamebearer flag</returns>
        private bool IsFlamebearerFlag(FlagDef flag)
        {
            return flag == FlagInstances.Abyss_02__Flamebearer_Spawn ||
                   flag == FlagInstances.Deepnest_East_03__Flamebearer_Spawn ||
                   flag == FlagInstances.Fungus1_10__Flamebearer_Spawn ||
                   flag == FlagInstances.Fungus2_30__Flamebearer_Spawn ||
                   flag == FlagInstances.Hive_03__Flamebearer_Spawn ||
                   flag == FlagInstances.Mines_10__Flamebearer_Spawn ||
                   flag == FlagInstances.RestingGrounds_06__Flamebearer_Spawn ||
                   flag == FlagInstances.Ruins1_28__Flamebearer_Spawn ||
                   flag == FlagInstances.Tutorial_01__Flamebearer_Spawn;
        }

        /// <summary>
        /// Creates a special panel for a flamebearer that manages both the spawn flag and flamesCollected
        /// </summary>
        /// <param name="flag">The flamebearer spawn flag</param>
        /// <returns>ToggleWithTeleportPanel for the flamebearer</returns>
        private ToggleWithTeleportPanel CreateFlamebearerPanel(FlagDef flag)
        {
            bossLocations.TryGetValue(flag, out var teleportLocation);

            return new ToggleWithTeleportPanel(new DelegateReference<bool>(
                () => FlagManager.GetBoolFlag(flag),
                value =>
                {
                    FlagManager.SetBoolFlag(flag, value);
                    
                    // Get current flames collected count
                    int currentFlames = FlagManager.GetIntFlag(FlagInstances.flamesCollected);
                    
                    if (value)
                    {
                        // If enabling, increment flames collected by 1
                        FlagManager.SetIntFlag(FlagInstances.flamesCollected, currentFlames + 1);
                    }
                    else
                    {
                        // If disabling, decrement flames collected by 1
                        FlagManager.SetIntFlag(FlagInstances.flamesCollected, Math.Max(0, currentFlames - 1));
                    }
                }
            ), () => TeleportService.DoTeleport(teleportLocation), $"{flag.ReadableName} {flag.Scene.ReadableName}");
        }

        /// <summary>
        /// Creates a boss panel with teleport functionality and dependent flags that get set to the same value
        /// </summary>
        /// <param name="flag">The main boss flag</param>
        /// <param name="dependentFlags">Additional flags that should be set to the same value</param>
        /// <returns>ToggleWithTeleportPanel for the boss</returns>
        private ToggleWithTeleportPanel CreateBossPanelWithDependentFlags(FlagDef flag, FlagDef[] dependentFlags)
        {
            bossLocations.TryGetValue(flag, out var bossTeleportLocation);

            return new ToggleWithTeleportPanel(new DelegateReference<bool>(
                () => FlagManager.GetBoolFlag(flag),
                value =>
                {
                    FlagManager.SetBoolFlag(flag, value);
                    // Set all dependent flags to the same value
                    foreach (var dependentFlag in dependentFlags)
                    {
                        FlagManager.SetBoolFlag(dependentFlag, value);
                    }
                }
            ), () => TeleportService.DoTeleport(bossTeleportLocation), flag.ReadableName);
        }
    }
} 