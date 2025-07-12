using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Map
{
    public class HotSpringPinPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return FlagManager.GetBoolFlag(FlagInstances.hasPinSpa);
        }

        public void Set(bool value)
        {
            FlagManager.SetBoolFlag(FlagInstances.hasPinSpa, value);
            if (value)
                FlagManager.SetBoolFlag(FlagInstances.hasPin, true);
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new HotSpringPinPatch(), "Hot Spring Pin"));
        }
    }
}
