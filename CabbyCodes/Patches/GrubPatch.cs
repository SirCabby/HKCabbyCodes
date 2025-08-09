using CabbyMenu.UI.CheatPanels;
using System.Collections.Generic;
using System.Linq;
using CabbyCodes.Scenes;
using static CabbyCodes.Scenes.SceneManagement;
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
        /// The identifier for grub bottle persistent data.
        /// </summary>
        private static readonly string grubId = "Grub Bottle";

        /// <summary>
        /// Dictionary of all grub locations with their teleport coordinates
        /// </summary>
        private static readonly Dictionary<SceneMapData, TeleportLocation> grubLocations = CreateGrubLocations();

        /// <summary>
        /// Creates the grub locations dictionary with teleport coordinates
        /// </summary>
        private static Dictionary<SceneMapData, TeleportLocation> CreateGrubLocations()
        {
            var locations = new Dictionary<SceneMapData, TeleportLocation>();
            
            // Helper method to add a scene with optional teleport coordinates
            void Add(SceneMapData scene, Vector2? teleportCoords = null) => locations[scene] = teleportCoords.HasValue ? new TeleportLocation(scene, teleportCoords.Value) : null;
            
            // Abyss
            Add(SceneInstances.Abyss_17);
            Add(SceneInstances.Abyss_19);
            
            // Crossroads
            Add(SceneInstances.Crossroads_03, new Vector2(7, 42));
            Add(SceneInstances.Crossroads_05);
            Add(SceneInstances.Crossroads_31, new Vector2(23, 14));
            Add(SceneInstances.Crossroads_35, new Vector2(7, 4));
            Add(SceneInstances.Crossroads_48, new Vector2(52, 4));
            
            // Deepnest
            Add(SceneInstances.Deepnest_03);
            Add(SceneInstances.Deepnest_31);
            Add(SceneInstances.Deepnest_36);
            Add(SceneInstances.Deepnest_39);
            Add(SceneInstances.Deepnest_East_11);
            Add(SceneInstances.Deepnest_East_14);
            Add(SceneInstances.Deepnest_Spider_Town);
            
            // Fungal Wastes
            Add(SceneInstances.Fungus1_06, new Vector2(154, 22));
            Add(SceneInstances.Fungus1_07, new Vector2(51, 13));
            Add(SceneInstances.Fungus1_13);
            Add(SceneInstances.Fungus1_21, new Vector2(83, 25));
            Add(SceneInstances.Fungus1_28);
            Add(SceneInstances.Fungus2_18);
            Add(SceneInstances.Fungus2_20);
            Add(SceneInstances.Fungus3_10);
            Add(SceneInstances.Fungus3_22);
            Add(SceneInstances.Fungus3_47);
            Add(SceneInstances.Fungus3_48);

            // Hive
            Add(SceneInstances.Hive_03);
            Add(SceneInstances.Hive_04);
            
            // Mines
            Add(SceneInstances.Mines_03);
            Add(SceneInstances.Mines_04);
            Add(SceneInstances.Mines_16);
            Add(SceneInstances.Mines_19);
            Add(SceneInstances.Mines_24);
            Add(SceneInstances.Mines_31);
            Add(SceneInstances.Mines_35);
            
            // Resting Grounds
            Add(SceneInstances.RestingGrounds_10);
            
            // City of Tears
            Add(SceneInstances.Ruins_House_01);
            Add(SceneInstances.Ruins1_05);
            Add(SceneInstances.Ruins1_32);
            Add(SceneInstances.Ruins2_03);
            Add(SceneInstances.Ruins2_07);
            Add(SceneInstances.Ruins2_11);
            
            // Waterways
            Add(SceneInstances.Waterways_04);
            Add(SceneInstances.Waterways_13);
            Add(SceneInstances.Waterways_14);
            
            return locations;
        }

        /// <summary>
        /// The scene name this grub patch is associated with.
        /// </summary>
        private readonly string sceneName;

        /// <summary>
        /// Initializes a new instance of the GrubPatch class.
        /// </summary>
        /// <param name="sceneName">The scene name where the grub is located.</param>
        public GrubPatch(string sceneName) : base(CreateGrubFlagDef(sceneName), GetGrubDisplayName(sceneName))
        {
            this.sceneName = sceneName;
        }

        /// <summary>
        /// Creates a FlagDef for a grub in the specified scene.
        /// </summary>
        /// <param name="sceneName">The scene name where the grub is located.</param>
        /// <returns>A FlagDef for the grub.</returns>
        private static FlagDef CreateGrubFlagDef(string sceneName)
        {
            return new FlagDef(grubId, sceneName, false, "PersistentBoolData", GetGrubDisplayName(sceneName));
        }

        /// <summary>
        /// Gets the display name for a grub in the specified scene.
        /// </summary>
        /// <param name="sceneName">The scene name where the grub is located.</param>
        /// <returns>The display name for the grub.</returns>
        private static string GetGrubDisplayName(string sceneName)
        {
            var sceneData = GetSceneData(sceneName);
            return sceneData?.ReadableName ?? sceneName;
        }

        /// <summary>
        /// Gets whether the grub in this scene has been rescued.
        /// </summary>
        /// <returns>True if the grub has been rescued, false otherwise.</returns>
        public override bool Get()
        {
            // true = got it
            return FlagManager.GetBoolFlag(grubId, sceneName);
        }

        /// <summary>
        /// Sets whether the grub in this scene has been rescued.
        /// </summary>
        /// <param name="value">True to mark the grub as rescued, false to mark it as not rescued.</param>
        public override void Set(bool value)
        {
            bool hasGrub = Get();

            if (value && !hasGrub)
            {
                FlagManager.SetBoolFlag(grubId, sceneName, true);

                if (!FlagManager.ListFlagContains(FlagInstances.scenesGrubRescued, sceneName))
                {
                    FlagManager.AddToListFlag(FlagInstances.scenesGrubRescued, sceneName);
                    FlagManager.SetIntFlag(FlagInstances.grubsCollected, PlayerData.instance.grubsCollected + 1);
                }
            }
            else if (!value && hasGrub)
            {
                FlagManager.SetBoolFlag(grubId, sceneName, false);

                if (FlagManager.ListFlagContains(FlagInstances.scenesGrubRescued, sceneName))
                {
                    FlagManager.RemoveFromListFlag(FlagInstances.scenesGrubRescued, sceneName);
                    FlagManager.SetIntFlag(FlagInstances.grubsCollected, PlayerData.instance.grubsCollected - 1);
                }
            }
        }

        /// <summary>
        /// Gets the area name for a given scene name.
        /// </summary>
        /// <param name="sceneName">The scene name to look up.</param>
        /// <returns>The area name, or "Unknown" if not found.</returns>
        private static string GetAreaForScene(string sceneName)
        {
            var sceneData = GetSceneData(sceneName);
            return sceneData?.AreaName ?? "Unknown";
        }

        /// <summary>
        /// Groups grub scenes by area.
        /// </summary>
        /// <returns>Dictionary mapping area names to lists of grub scenes in that area.</returns>
        private static Dictionary<string, List<SceneMapData>> GroupGrubsByArea()
        {
            var groupedGrubs = new Dictionary<string, List<SceneMapData>>();

            foreach (var sceneData in grubLocations.Keys)
            {
                string area = sceneData.AreaName;
                if (!groupedGrubs.ContainsKey(area))
                {
                    groupedGrubs[area] = new List<SceneMapData>();
                }
                groupedGrubs[area].Add(sceneData);
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
                foreach (var sceneData in groupedGrubs[area])
                {
                    string displayName = sceneData.ReadableName;
                    
                    // Check if this grub has teleport coordinates defined
                    if (grubLocations.TryGetValue(sceneData, out var grubTeleportLocation) && grubTeleportLocation != null)
                    {
                        // Create panel with toggle and teleport button
                        var grubPatch = new GrubPatch(sceneData.SceneName);
                        CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new ToggleWithTeleportPanel(
                            grubPatch, 
                            () => TeleportService.DoTeleport(grubTeleportLocation), 
                            GetGrubDisplayName(sceneData.SceneName)));
                    }
                    else
                    {
                        // Create panel with just toggle (no teleport button)
                        var grubPatch = new GrubPatch(sceneData.SceneName);
                        CabbyCodesPlugin.cabbyMenu.AddCheatPanel(grubPatch.CreatePanel());
                    }
                }
            }
        }
    }
}
