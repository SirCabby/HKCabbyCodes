using CabbyMenu.UI.CheatPanels;
using System.Collections.Generic;
using System.Linq;
using static CabbyCodes.Scenes.Areas;
using CabbyCodes.Flags;
using CabbyCodes.Patches.Teleport;
using CabbyCodes.Patches.BasePatches;
using UnityEngine;

namespace CabbyCodes.Patches
{
    /// <summary>
    /// Handles grub rescue functionality and tracking for specific scenes.
    /// </summary>
    public class GrubPatch : BoolPatch
    {
        /// <summary>
        /// Dictionary of all grub locations with their teleport coordinates
        /// </summary>
        private static readonly Dictionary<FlagDef, TeleportLocation> grubLocations = CreateGrubLocations();

        /// <summary>
        /// Creates the grub locations dictionary with teleport coordinates
        /// </summary>
        private static Dictionary<FlagDef, TeleportLocation> CreateGrubLocations()
        {
            var locations = new Dictionary<FlagDef, TeleportLocation>();

            void Add(FlagDef flag, Vector2? teleportCoords = null) => locations[flag] = teleportCoords.HasValue ? new TeleportLocation(flag.Scene, teleportCoords.Value) : null;
            
            // Abyss
            Add(FlagInstances.Abyss_17__Grub_Bottle);
            Add(FlagInstances.Abyss_19__Grub_Bottle);
            
            // Crossroads
            Add(FlagInstances.Crossroads_03__Grub_Bottle, new Vector2(7, 42));
            Add(FlagInstances.Crossroads_05__Grub_Bottle, new Vector2(53, 17));
            Add(FlagInstances.Crossroads_31__Grub_Bottle, new Vector2(23, 14));
            Add(FlagInstances.Crossroads_35__Grub_Bottle, new Vector2(7, 4));
            Add(FlagInstances.Crossroads_48__Grub_Bottle, new Vector2(52, 4));
            
            // Deepnest
            Add(FlagInstances.Deepnest_03__Grub_Bottle);
            Add(FlagInstances.Deepnest_31__Grub_Bottle);
            Add(FlagInstances.Deepnest_36__Grub_Bottle);
            Add(FlagInstances.Deepnest_39__Grub_Bottle);
            Add(FlagInstances.Deepnest_East_11__Grub_Bottle);
            Add(FlagInstances.Deepnest_East_14__Grub_Bottle);
            Add(FlagInstances.Deepnest_Spider_Town__Grub_Bottle);
            
            // Fungal Wastes
            Add(FlagInstances.Fungus1_06__Grub_Bottle, new Vector2(154, 22));
            Add(FlagInstances.Fungus1_07__Grub_Bottle, new Vector2(51, 13));
            Add(FlagInstances.Fungus1_13__Grub_Bottle);
            Add(FlagInstances.Fungus1_21__Grub_Bottle, new Vector2(83, 25));
            Add(FlagInstances.Fungus1_28__Grub_Bottle);
            Add(FlagInstances.Fungus2_18__Grub_Bottle);
            Add(FlagInstances.Fungus2_20__Grub_Bottle);
            Add(FlagInstances.Fungus3_10__Grub_Bottle);
            Add(FlagInstances.Fungus3_22__Grub_Bottle);
            Add(FlagInstances.Fungus3_47__Grub_Bottle);
            Add(FlagInstances.Fungus3_48__Grub_Bottle);

            // Hive
            Add(FlagInstances.Hive_03__Grub_Bottle);
            Add(FlagInstances.Hive_04__Grub_Bottle);
            
            // Mines
            Add(FlagInstances.Mines_03__Grub_Bottle);
            Add(FlagInstances.Mines_04__Grub_Bottle);
            Add(FlagInstances.Mines_16__Grub_Bottle);
            Add(FlagInstances.Mines_19__Grub_Bottle);
            Add(FlagInstances.Mines_24__Grub_Bottle);
            Add(FlagInstances.Mines_31__Grub_Bottle);
            Add(FlagInstances.Mines_35__Grub_Bottle);
            
            // Resting Grounds
            Add(FlagInstances.RestingGrounds_10__Grub_Bottle);
            
            // City of Tears
            Add(FlagInstances.Ruins_House_01__Grub_Bottle);
            Add(FlagInstances.Ruins1_05__Grub_Bottle);
            Add(FlagInstances.Ruins1_32__Grub_Bottle);
            Add(FlagInstances.Ruins2_03__Grub_Bottle);
            Add(FlagInstances.Ruins2_07__Grub_Bottle);
            Add(FlagInstances.Ruins2_11__Grub_Bottle);
            
            // Waterways
            Add(FlagInstances.Waterways_04__Grub_Bottle);
            Add(FlagInstances.Waterways_13__Grub_Bottle);
            Add(FlagInstances.Waterways_14__Grub_Bottle);
            
            return locations;
        }

        /// <summary>
        /// The flag that tracks this grub bottle.
        /// </summary>
        private readonly FlagDef grubFlag;

        /// <summary>
        /// Initializes a new instance of the GrubPatch class with a predefined FlagDef.
        /// </summary>
        /// <param name="grubFlag">The flag definition representing this grub bottle.</param>
        public GrubPatch(FlagDef grubFlag) : base(grubFlag, grubFlag.Scene?.ReadableName ?? grubFlag.SceneName)
        {
            this.grubFlag = grubFlag;
        }

        /// <summary>
        /// Gets whether the grub in this scene has been rescued.
        /// </summary>
        /// <returns>True if the grub has been rescued, false otherwise.</returns>
        public override bool Get()
        {
            return FlagManager.GetBoolFlag(grubFlag);
        }

        /// <summary>
        /// Sets whether the grub in this scene has been rescued.
        /// </summary>
        /// <param name="value">True to mark the grub as rescued, false to mark it as not rescued.</param>
        public override void Set(bool value)
        {
            bool hasGrub = Get();

            var sceneName = grubFlag.SceneName;

            if (value && !hasGrub)
            {
                FlagManager.SetBoolFlag(grubFlag, true);

                if (!FlagManager.ListFlagContains(FlagInstances.scenesGrubRescued, sceneName))
                {
                    FlagManager.AddToListFlag(FlagInstances.scenesGrubRescued, sceneName);
                    FlagManager.SetIntFlag(FlagInstances.grubsCollected, PlayerData.instance.grubsCollected + 1);
                }
            }
            else if (!value && hasGrub)
            {
                FlagManager.SetBoolFlag(grubFlag, false);

                if (FlagManager.ListFlagContains(FlagInstances.scenesGrubRescued, sceneName))
                {
                    FlagManager.RemoveFromListFlag(FlagInstances.scenesGrubRescued, sceneName);
                    FlagManager.SetIntFlag(FlagInstances.grubsCollected, PlayerData.instance.grubsCollected - 1);
                }
            }
        }

        /// <summary>
        /// Groups grub scenes by area.
        /// </summary>
        /// <returns>Dictionary mapping area names to lists of grub scenes in that area.</returns>
        private static Dictionary<string, List<FlagDef>> GroupGrubsByArea()
        {
            var groupedGrubs = new Dictionary<string, List<FlagDef>>();

            foreach (var flag in grubLocations.Keys)
            {
                string area = flag.Scene.AreaName;
                if (!groupedGrubs.ContainsKey(area))
                {
                    groupedGrubs[area] = new List<FlagDef>();
                }
                
                groupedGrubs[area].Add(flag);
            }

            return groupedGrubs;
        }

        /// <summary>
        /// Adds grub-related panels to the mod menu for all grub locations, grouped by area.
        /// </summary>
        public static void AddPanels()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Grubs Found").SetColor(CheatPanel.headerColor));

            var groupedGrubs = GroupGrubsByArea();
            
            // Sort areas alphabetically for consistent ordering
            var sortedAreas = groupedGrubs.Keys.OrderBy(area => area).ToList();

            foreach (string area in sortedAreas)
            {
                // Get readable area name
                var areaData = GetAreaData(area);
                string readableAreaName = areaData?.ReadableName ?? area;
                
                // Add subheader for the area
                CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Area: " + readableAreaName).SetColor(CheatPanel.subHeaderColor));
                
                // Add grub panels for this area
                foreach (var grubFlag in groupedGrubs[area])
                {
                    string displayName = grubFlag.Scene.ReadableName;
                    
                    // Check if this grub has teleport coordinates defined
                    if (grubLocations.TryGetValue(grubFlag, out var grubTeleportLocation) && grubTeleportLocation != null)
                    {
                        // Create panel with toggle and teleport button
                        var grubPatch = new GrubPatch(grubFlag);
                        CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new ToggleWithTeleportPanel(
                            grubPatch, 
                            () => TeleportService.DoTeleport(grubTeleportLocation), 
                            grubFlag.Scene.ReadableName));
                    }
                    else
                    {
                        // Create panel with just toggle (no teleport button)
                        var grubPatch = new GrubPatch(grubFlag);
                        CabbyCodesPlugin.cabbyMenu.AddCheatPanel(grubPatch.CreatePanel());
                    }
                }
            }
        }
    }
}
