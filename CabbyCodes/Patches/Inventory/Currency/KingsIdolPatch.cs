using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.Utilities;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Currency
{
    public class KingsIdolPatch : ISyncedReference<int>
    {
        private static readonly FlagDef flag1 = FlagInstances.trinket3;
        private static readonly FlagDef flag2 = FlagInstances.foundTrinket3;

        public int Get()
        {
            return FlagManager.GetIntFlag(flag1);
        }

        public void Set(int value)
        {
            value = ValidationUtils.ValidateRange(value, 0, Constants.MAX_KINGS_IDOLS, nameof(value));
            if (value > 0)
            {
                FlagManager.SetBoolFlag(flag2, true);
            }
            FlagManager.SetIntFlag(flag1, value);
        }

        public static void AddPanel()
        {
            RangeInputFieldPanel<int> panel = new RangeInputFieldPanel<int>(new KingsIdolPatch(), KeyCodeMap.ValidChars.Numeric, 0, Constants.MAX_KINGS_IDOLS, flag1.ReadableName);
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
