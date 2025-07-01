using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.Utilities;

namespace CabbyCodes.Patches.Inventory.Currency
{
    public class WanderersJournalPatch : ISyncedReference<int>
    {
        public int Get()
        {
            return PlayerData.instance.trinket1;
        }

        public void Set(int value)
        {
            value = ValidationUtils.ValidateRange(value, 0, Constants.MAX_WANDERERS_JOURNALS, nameof(value));
            if (value > 0)
            {
                PlayerData.instance.foundTrinket1 = true;
            }
            PlayerData.instance.trinket1 = value;
        }

        public static void AddPanel()
        {
            InputFieldPanel<int> panel = new InputFieldPanel<int>(new WanderersJournalPatch(), KeyCodeMap.ValidChars.Numeric, 2, "Wanderer's Journals (0-14)");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
