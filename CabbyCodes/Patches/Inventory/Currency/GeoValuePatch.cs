using CabbyCodes.Flags;
using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.Utilities;

namespace CabbyCodes.Patches.Inventory.Currency
{
    public class GeoValuePatch
    {
        private static readonly FlagDef flag = FlagInstances.geo;
        
        public int Get()
        {
            return FlagManager.GetIntFlag(flag);
        }

        public void Set(int value)
        {
            value = ValidationUtils.ValidateRange(value, Constants.MIN_GEO, Constants.MAX_GEO, nameof(value));

            FlagManager.SetIntFlag(flag, value);
        }

        public static void AddPanel()
        {
            var reference = new DelegateReference<int>(
                () => FlagManager.GetIntFlag(flag),
                value => FlagManager.SetIntFlag(flag, ValidationUtils.ValidateRange(value, Constants.MIN_GEO, Constants.MAX_GEO, nameof(value)))
            );
            RangeInputFieldPanel<int> panel = new RangeInputFieldPanel<int>(reference, KeyCodeMap.ValidChars.Numeric, Constants.MIN_GEO, Constants.MAX_GEO, flag.ReadableName);
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
