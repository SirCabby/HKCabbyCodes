using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Flags.General
{
    public class CrossroadsInfectedPatch : ISyncedReference<bool>
    {
        private static readonly FlagDef flag = FlagInstances.crossroadsInfected;

        public bool Get()
        {
            return FlagManager.GetBoolFlag(flag);
        }

        public void Set(bool value)
        {
            FlagManager.SetBoolFlag(flag, value);
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new CrossroadsInfectedPatch(), flag.ReadableName));
        }
    }
}
