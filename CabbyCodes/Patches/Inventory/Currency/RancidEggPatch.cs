using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.Utilities;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Currency
{
    public class RancidEggPatch : ISyncedReference<int>
    {
        public int Get()
        {
            return FlagManager.GetIntFlag(FlagInstances.rancidEggs);
        }

        public void Set(int value)
        {
            value = ValidationUtils.ValidateRange(value, 0, Constants.MAX_RANCID_EGGS, nameof(value));
            FlagManager.SetIntFlag(FlagInstances.rancidEggs, value);
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new RangeInputFieldPanel<int>(new RancidEggPatch(), KeyCodeMap.ValidChars.Numeric, 0, Constants.MAX_RANCID_EGGS, "Rancid Eggs"));
        }
    }
}
