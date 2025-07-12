using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.Utilities;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Currency
{
    public class PaleOrePatch : ISyncedReference<int>
    {
        public int Get()
        {
            return FlagManager.GetIntFlag(FlagInstances.ore);
        }

        public void Set(int value)
        {
            value = ValidationUtils.ValidateRange(value, 0, Constants.MAX_PALE_ORE, nameof(value));
            FlagManager.SetIntFlag(FlagInstances.ore, value);
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new RangeInputFieldPanel<int>(new PaleOrePatch(), KeyCodeMap.ValidChars.Numeric, 0, Constants.MAX_PALE_ORE, "Pale Ore"));
        }
    }
}
