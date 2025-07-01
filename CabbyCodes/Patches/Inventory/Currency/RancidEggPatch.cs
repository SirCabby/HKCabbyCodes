using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.Utilities;

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
            value = ValidationUtils.ValidateRange(value, 0, Constants.MAX_RANCID_EGGS, nameof(value));
            PlayerData.instance.rancidEggs = value;
        }

        public static void AddPanel()
        {
            InputFieldPanel<int> panel = new InputFieldPanel<int>(new RancidEggPatch(), KeyCodeMap.ValidChars.Numeric, 2, "Rancid Eggs (0-80)");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
