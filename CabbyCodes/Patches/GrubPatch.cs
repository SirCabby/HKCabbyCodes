using CabbyCodes.SyncedReferences;
using CabbyCodes.UI.CheatPanels;
using System.Collections.Generic;

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
        private static readonly List<string> grubScenes = new()
        {
            "Crossroads_48",
            "Crossroads_05",
            "Crossroads_03",
            "Crossroads_35",
            "Crossroads_31",
            "Fungus1_06",
            "Fungus1_07",
            "Fungus1_28",
            "Fungus1_21",
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
        /// Adds grub-related panels to the mod menu for all grub locations.
        /// </summary>
        public static void AddPanels()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Grubs Found").SetColor(CheatPanel.headerColor));

            foreach (string grubScene in grubScenes)
            {
                CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new GrubPatch(grubScene), grubScene));
            }
        }
    }
}
