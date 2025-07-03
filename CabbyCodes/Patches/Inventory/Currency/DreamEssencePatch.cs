using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.Utilities;

namespace CabbyCodes.Patches.Inventory.Currency
{
    public class DreamEssencePatch : ISyncedReference<int>
    {
        public int Get()
        {
            return PlayerData.instance.dreamOrbs;
        }

        public void Set(int value)
        {
            value = ValidationUtils.ValidateRange(value, Constants.MIN_DREAM_ESSENCE, Constants.MAX_DREAM_ESSENCE, nameof(value));
            PlayerData.instance.dreamOrbs = value;
        }

        public static void AddPanel()
        {
            RangeInputFieldPanel<int> panel = new RangeInputFieldPanel<int>(new DreamEssencePatch(), KeyCodeMap.ValidChars.Numeric, Constants.MIN_DREAM_ESSENCE, Constants.MAX_DREAM_ESSENCE, "Dream Essence (0-9999)");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
