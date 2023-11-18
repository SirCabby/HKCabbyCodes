using CabbyCodes.SyncedReferences;
using CabbyCodes.UI.CheatPanels;
using System.Collections.Generic;

namespace CabbyCodes.Patches
{
    public class GrubPatch : ISyncedReference<bool>
    {
        private static readonly string grubId = "Grub Bottle";
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

        private readonly string sceneName;

        public GrubPatch(string sceneName)
        {
            this.sceneName = sceneName;
        }

        public bool Get()
        {
            // true = got it
            return PbdMaker.GetPbd(grubId, sceneName).activated;
        }

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
