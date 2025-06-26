using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;

namespace CabbyCodes.Patches.Flags.General
{
    public class CrossroadsInfectedPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return PlayerData.instance.crossroadsInfected;
        }

        public void Set(bool value)
        {
            PlayerData.instance.crossroadsInfected = value;
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new CrossroadsInfectedPatch(), "Crossroads Infected"));
        }
    }
}
