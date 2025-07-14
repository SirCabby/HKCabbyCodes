using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.Utilities;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Currency
{
    public class WanderersJournalPatch : ISyncedReference<int>
    {
        private static readonly FlagDef flag1 = FlagInstances.trinket1;
        private static readonly FlagDef flag2 = FlagInstances.foundTrinket1;

        public int Get()
        {
            return FlagManager.GetIntFlag(flag1);
        }

        public void Set(int value)
        {
            value = ValidationUtils.ValidateRange(value, 0, Constants.MAX_WANDERERS_JOURNALS, nameof(value));
            if (value > 0)
            {
                FlagManager.SetBoolFlag(flag2, true);
            }
            FlagManager.SetIntFlag(flag1, value);
        }

        public static void AddPanel()
        {
            RangeInputFieldPanel<int> panel = new RangeInputFieldPanel<int>(new WanderersJournalPatch(), KeyCodeMap.ValidChars.Numeric, 0, Constants.MAX_WANDERERS_JOURNALS, flag1.ReadableName);
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
