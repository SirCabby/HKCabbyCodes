using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Map
{
    public class CocoonPinPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return FlagManager.GetBoolFlag(FlagInstances.hasPinCocoon);
        }

        public void Set(bool value)
        {
            FlagManager.SetBoolFlag(FlagInstances.hasPinCocoon, value);
            if (value)
                FlagManager.SetBoolFlag(FlagInstances.hasPin, true);
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new CocoonPinPatch(), "Cocoon Pin"));
        }
    }
}
