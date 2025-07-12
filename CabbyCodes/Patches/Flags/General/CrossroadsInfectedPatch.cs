using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Flags.General
{
    public class CrossroadsInfectedPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return FlagManager.GetBoolFlag(FlagInstances.crossroadsInfected);
        }

        public void Set(bool value)
        {
            FlagManager.SetBoolFlag(FlagInstances.crossroadsInfected, value);
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new CrossroadsInfectedPatch(), "Crossroads Infected"));
        }
    }
}
