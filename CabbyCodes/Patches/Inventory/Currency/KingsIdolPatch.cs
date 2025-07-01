using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.Utilities;

namespace CabbyCodes.Patches.Inventory.Currency
{
    public class KingsIdolPatch : ISyncedReference<int>
    {
        public int Get()
        {
            return PlayerData.instance.trinket3;
        }

        public void Set(int value)
        {
            value = ValidationUtils.ValidateRange(value, 0, Constants.MAX_KINGS_IDOLS, nameof(value));
            if (value > 0)
            {
                PlayerData.instance.foundTrinket3 = true;
            }
            PlayerData.instance.trinket3 = value;
        }

        public static void AddPanel()
        {
            InputFieldPanel<int> panel = new InputFieldPanel<int>(new KingsIdolPatch(), KeyCodeMap.ValidChars.Numeric, 1, "King's Idols (0-8)");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
