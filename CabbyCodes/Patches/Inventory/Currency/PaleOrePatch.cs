using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.Utilities;

namespace CabbyCodes.Patches.Inventory.Currency
{
    public class PaleOrePatch : ISyncedReference<int>
    {
        public int Get()
        {
            return PlayerData.instance.ore;
        }

        public void Set(int value)
        {
            value = ValidationUtils.ValidateRange(value, 0, Constants.MAX_PALE_ORE, nameof(value));
            PlayerData.instance.ore = value;
        }

        public static void AddPanel()
        {
            InputFieldPanel<int> panel = new InputFieldPanel<int>(new PaleOrePatch(), KeyCodeMap.ValidChars.Numeric, 1, "Ore (0-6)");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
