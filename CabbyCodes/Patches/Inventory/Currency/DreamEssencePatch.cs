using CabbyCodes.SyncedReferences;
using CabbyCodes.UI.CheatPanels;
using System;

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
            InputFieldPanel<int> panel = new(new DreamEssencePatch(), KeyCodeMap.ValidChars.Numeric, 4, 120, "Dream Essence (0-9999)");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
