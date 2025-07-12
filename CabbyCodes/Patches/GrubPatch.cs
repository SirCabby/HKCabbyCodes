using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using System.Collections.Generic;
using System.Linq;
using CabbyCodes.Scenes;
using static CabbyCodes.Scenes.Scenes;
using static CabbyCodes.Scenes.Areas;

namespace CabbyCodes.Patches
{
    /// <summary>
    /// Handles grub rescue functionality and tracking for specific scenes.
    /// </summary>
    public class GrubPatch : ISyncedReference<bool>
    {
        /// <summary>
        /// The identifier for grub bottle persistent data.
        /// </summary>
        private static readonly string grubId = "Grub Bottle";

        /// <summary>
        /// List of scenes that contain grubs, using SceneInstances references.
        /// </summary>
        private static readonly List<string> grubScenes = new List<string>()
        {
            SceneInstances.Abyss_17.SceneName,
            SceneInstances.Abyss_19.SceneName,
            SceneInstances.Crossroads_03.SceneName,
            SceneInstances.Crossroads_05.SceneName,
            SceneInstances.Crossroads_31.SceneName,
            SceneInstances.Crossroads_35.SceneName,
            SceneInstances.Crossroads_48.SceneName,
            SceneInstances.Deepnest_03.SceneName,
            SceneInstances.Deepnest_31.SceneName,
            SceneInstances.Deepnest_36.SceneName,
            SceneInstances.Deepnest_39.SceneName,
            SceneInstances.Deepnest_East_11.SceneName,
            SceneInstances.Deepnest_East_14.SceneName,
            SceneInstances.Deepnest_Spider_Town.SceneName,
            SceneInstances.Fungus1_06.SceneName,
            SceneInstances.Fungus1_07.SceneName,
            SceneInstances.Fungus1_13.SceneName,
            SceneInstances.Fungus1_21.SceneName,
            SceneInstances.Fungus1_28.SceneName,
            SceneInstances.Fungus2_18.SceneName,
            SceneInstances.Fungus2_20.SceneName,
            SceneInstances.Fungus3_10.SceneName,
            SceneInstances.Fungus3_22.SceneName,
            SceneInstances.Fungus3_47.SceneName,
            SceneInstances.Fungus3_48.SceneName,
            SceneInstances.Hive_03.SceneName,
            SceneInstances.Hive_04.SceneName,
            SceneInstances.Mines_03.SceneName,
            SceneInstances.Mines_04.SceneName,
            SceneInstances.Mines_16.SceneName,
            SceneInstances.Mines_19.SceneName,
            SceneInstances.Mines_24.SceneName,
            SceneInstances.Mines_31.SceneName,
            SceneInstances.Mines_35.SceneName,
            SceneInstances.RestingGrounds_10.SceneName,
            SceneInstances.Ruins_House_01.SceneName,
            SceneInstances.Ruins1_05.SceneName,
            SceneInstances.Ruins1_32.SceneName,
            SceneInstances.Ruins2_03.SceneName,
            SceneInstances.Ruins2_07.SceneName,
            SceneInstances.Ruins2_11.SceneName,
            SceneInstances.Waterways_04.SceneName,
            SceneInstances.Waterways_13.SceneName,
            SceneInstances.Waterways_14.SceneName,
        };

        /// <summary>
        /// The scene name this grub patch is associated with.
        /// </summary>
        private readonly string sceneName;

        /// <summary>
        /// Initializes a new instance of the GrubPatch class.
        /// </summary>
        /// <param name="sceneName">The scene name where the grub is located.</param>
        public GrubPatch(string sceneName)
        {
            this.sceneName = sceneName;
        }

        /// <summary>
        /// Gets whether the grub in this scene has been rescued.
        /// </summary>
        /// <returns>True if the grub has been rescued, false otherwise.</returns>
        public bool Get()
        {
            // true = got it
            return PbdMaker.GetPbd(grubId, sceneName).activated;
        }

        /// <summary>
        /// Sets whether the grub in this scene has been rescued.
        /// </summary>
        /// <param name="value">True to mark the grub as rescued, false to mark it as not rescued.</param>
        public void Set(bool value)
        {
            bool hasGrub = Get();

            if (value && !hasGrub)
            {
                PersistentBoolData pbd = PbdMaker.GetPbd(grubId, sceneName);
                pbd.activated = true;
                SceneData.instance.SaveMyState(pbd);

                if (!PlayerData.instance.scenesGrubRescued.Contains(sceneName))
                {
                    PlayerData.instance.scenesGrubRescued.Add(sceneName);
                    PlayerData.instance.grubsCollected++;
                }
            }
            else if (!value && hasGrub)
            {
                PersistentBoolData pbd = PbdMaker.GetPbd(grubId, sceneName);
                pbd.activated = false;
                SceneData.instance.SaveMyState(pbd);

                if (PlayerData.instance.scenesGrubRescued.Contains(sceneName))
                {
                    PlayerData.instance.scenesGrubRescued.Remove(sceneName);
                    PlayerData.instance.grubsCollected--;
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
        private static Dictionary<string, List<string>> GroupGrubsByArea()
        {
            var groupedGrubs = new Dictionary<string, List<string>>();

            foreach (string scene in grubScenes)
            {
                string area = GetAreaForScene(scene);
                if (!groupedGrubs.ContainsKey(area))
                {
                    groupedGrubs[area] = new List<string>();
                }
                groupedGrubs[area].Add(scene);
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
                foreach (string grubScene in groupedGrubs[area])
                {
                    var sceneData = GetSceneData(grubScene);
                    string displayName = sceneData?.ReadableName ?? grubScene;
                    CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new GrubPatch(grubScene), displayName));
                }
            }
        }
    }
}
