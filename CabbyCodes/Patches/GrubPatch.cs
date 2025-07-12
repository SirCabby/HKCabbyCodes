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
        /// List of scenes that contain grubs.
        /// </summary>
        private static readonly List<string> grubScenes = new List<string>()
        {
            "Abyss_17",
            "Abyss_19",
            "Crossroads_03",
            "Crossroads_05",
            "Crossroads_31",
            "Crossroads_35",
            "Crossroads_48",
            "Deepnest_03",
            "Deepnest_31",
            "Deepnest_36",
            "Deepnest_39",
            "Deepnest_East_11",
            "Deepnest_East_14",
            "Deepnest_Spider_Town",
            "Fungus1_06",
            "Fungus1_07",
            "Fungus1_13",
            "Fungus1_21",
            "Fungus1_28",
            "Fungus2_18",
            "Fungus2_20",
            "Fungus3_10",
            "Fungus3_22",
            "Fungus3_47",
            "Fungus3_48",
            "Hive_03",
            "Hive_04",
            "Mines_03",
            "Mines_04",
            "Mines_16",
            "Mines_19",
            "Mines_24",
            "Mines_31",
            "Mines_35",
            "RestingGrounds_10",
            "Ruins_House_01",
            "Ruins1_05",
            "Ruins1_32",
            "Ruins2_03",
            "Ruins2_07",
            "Ruins2_11",
            "Waterways_04",
            "Waterways_13",
            "Waterways_14",
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
