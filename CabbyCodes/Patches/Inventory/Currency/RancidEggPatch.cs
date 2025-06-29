using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;

namespace CabbyCodes.Patches.Inventory.Currency
{
    public class RancidEggPatch : ISyncedReference<int>
    {
        public int Get()
        {
            return PlayerData.instance.rancidEggs;
        }

        public void Set(int value)
        {
            value = CabbyMenu.ValidationUtils.ValidateRange(value, 0, Constants.MAX_RANCID_EGGS, nameof(value));
            PlayerData.instance.rancidEggs = value;
        }

        public static void AddPanel()
        {
            InputFieldPanel<int> panel = new(new RancidEggPatch(), CabbyMenu.KeyCodeMap.ValidChars.Numeric, 2, Constants.PANEL_WIDTH_120, "Rancid Eggs (0-80)");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
