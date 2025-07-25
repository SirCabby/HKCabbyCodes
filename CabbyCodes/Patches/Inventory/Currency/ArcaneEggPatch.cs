using CabbyCodes.Flags;
using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.Utilities;

namespace CabbyCodes.Patches.Inventory.Currency
{
    public class ArcaneEggPatch
    {
        private static readonly FlagDef flag1 = FlagInstances.trinket4;
        private static readonly FlagDef flag2 = FlagInstances.foundTrinket4;
        
        public int Get()
        {
            return FlagManager.GetIntFlag(flag1);
        }

        public static void AddPanel()
        {
            var reference = new DelegateReference<int>(
                () => FlagManager.GetIntFlag(flag1),
                value => {
                    value = ValidationUtils.ValidateRange(value, 0, Constants.MAX_ARCANE_EGGS, nameof(value));
                    if (value > 0)
                        FlagManager.SetBoolFlag(flag2, true);
                    FlagManager.SetIntFlag(flag1, value);
                }
            );
            RangeInputFieldPanel<int> panel = new RangeInputFieldPanel<int>(reference, KeyCodeMap.ValidChars.Numeric, 0, Constants.MAX_ARCANE_EGGS, flag1.ReadableName);
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
