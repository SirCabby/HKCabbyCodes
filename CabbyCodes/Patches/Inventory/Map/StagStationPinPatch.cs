using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Map
{
    public class StagStationPinPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return FlagManager.GetBoolFlag(FlagInstances.hasPinStag);
        }

        public void Set(bool value)
        {
            FlagManager.SetBoolFlag(FlagInstances.hasPinStag, value);
            if (value)
                FlagManager.SetBoolFlag(FlagInstances.hasPin, true);
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new StagStationPinPatch(), "Stag Station Pin"));
        }
    }
}
