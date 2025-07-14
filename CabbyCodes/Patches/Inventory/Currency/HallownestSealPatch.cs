using CabbyCodes.Flags;
using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.Utilities;

namespace CabbyCodes.Patches.Inventory.Currency
{
    public class HallownestSealPatch : ISyncedReference<int>
    {
        private static readonly FlagDef flag1 = FlagInstances.trinket2;
        private static readonly FlagDef flag2 = FlagInstances.foundTrinket2;

        public int Get()
        {
            return FlagManager.GetIntFlag(flag1);
        }

        public void Set(int value)
        {
            value = ValidationUtils.ValidateRange(value, 0, Constants.MAX_HALLOWNEST_SEALS, nameof(value));
            if (value > 0)
            {
                FlagManager.SetBoolFlag(flag2, true);
            }
            FlagManager.SetIntFlag(flag2, value);
        }

        public static void AddPanel()
        {
            RangeInputFieldPanel<int> panel = new RangeInputFieldPanel<int>(new HallownestSealPatch(), KeyCodeMap.ValidChars.Numeric, 0, Constants.MAX_HALLOWNEST_SEALS, flag1.ReadableName);
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
