using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Map
{
    public class BenchPinPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return FlagManager.GetBoolFlag(FlagInstances.hasPinBench);
        }

        public void Set(bool value)
        {
            FlagManager.SetBoolFlag(FlagInstances.hasPinBench, value);
            if (value)
                FlagManager.SetBoolFlag(FlagInstances.hasPin, true);
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new BenchPinPatch(), "Bench Pin"));
        }
    }
}
