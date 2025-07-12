using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Map
{
    public class TramPinPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return FlagManager.GetBoolFlag(FlagInstances.hasPinTram);
        }

        public void Set(bool value)
        {
            FlagManager.SetBoolFlag(FlagInstances.hasPinTram, value);
            if (value)
                FlagManager.SetBoolFlag(FlagInstances.hasPin, true);
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new TramPinPatch(), "Tram Pin"));
        }
    }
}
