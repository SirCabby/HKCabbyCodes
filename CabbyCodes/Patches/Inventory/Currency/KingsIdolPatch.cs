using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using System;

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
            value = CabbyMenu.ValidationUtils.ValidateRange(value, 0, Constants.MAX_KINGS_IDOLS, nameof(value));
            if (value > 0)
            {
                PlayerData.instance.foundTrinket3 = true;
            }
            PlayerData.instance.trinket3 = value;
        }

        public static void AddPanel()
        {
            InputFieldPanel<int> panel = new(new KingsIdolPatch(), CabbyMenu.KeyCodeMap.ValidChars.Numeric, 1, 120, "King's Idols (0-8)");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
