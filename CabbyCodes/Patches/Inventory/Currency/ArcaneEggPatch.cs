using CabbyCodes.Flags;
using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.Utilities;

namespace CabbyCodes.Patches.Inventory.Currency
{
    public class ArcaneEggPatch : ISyncedReference<int>
    {
        private static readonly FlagDef flag1 = FlagInstances.trinket4;
        private static readonly FlagDef flag2 = FlagInstances.foundTrinket4;
        
        public int Get()
        {
            return FlagManager.GetIntFlag(flag1);
        }

        public void Set(int value)
        {
            value = ValidationUtils.ValidateRange(value, 0, Constants.MAX_ARCANE_EGGS, nameof(value));
            if (value > 0)
            {
                FlagManager.SetBoolFlag(flag2, true);
            }
            FlagManager.SetIntFlag(flag1, value);
        }

        public static void AddPanel()
        {
            RangeInputFieldPanel<int> panel = new RangeInputFieldPanel<int>(new ArcaneEggPatch(), KeyCodeMap.ValidChars.Numeric, 0, Constants.MAX_ARCANE_EGGS, flag1.ReadableName);
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
