using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.Utilities;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Currency
{
    public class WanderersJournalPatch : ISyncedReference<int>
    {
        public int Get()
        {
            return FlagManager.GetIntFlag(FlagInstances.trinket1);
        }

        public void Set(int value)
        {
            value = ValidationUtils.ValidateRange(value, 0, Constants.MAX_WANDERERS_JOURNALS, nameof(value));
            if (value > 0)
            {
                FlagManager.SetBoolFlag(FlagInstances.foundTrinket1, true);
            }
            FlagManager.SetIntFlag(FlagInstances.trinket1, value);
        }

        public static void AddPanel()
        {
            RangeInputFieldPanel<int> panel = new RangeInputFieldPanel<int>(new WanderersJournalPatch(), KeyCodeMap.ValidChars.Numeric, 0, Constants.MAX_WANDERERS_JOURNALS, "Wanderer's Journals (0-14)");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
