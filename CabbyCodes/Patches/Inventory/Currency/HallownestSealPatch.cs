using CabbyCodes.SyncedReferences;
using CabbyCodes.UI.CheatPanels;
using System;

namespace CabbyCodes.Patches.Inventory.Currency
{
    public class HallownestSealPatch : ISyncedReference<int>
    {
        public int Get()
        {
            return PlayerData.instance.trinket2;
        }

        public void Set(int value)
        {
            value = ValidationUtils.ValidateRange(value, 0, Constants.MAX_HALLOWNEST_SEALS, nameof(value));
            if (value > 0)
            {
                PlayerData.instance.foundTrinket2 = true;
            }
            PlayerData.instance.trinket2 = value;
        }

        public static void AddPanel()
        {
            InputFieldPanel<int> panel = new(new HallownestSealPatch(), KeyCodeMap.ValidChars.Numeric, 2, 120, "Hallownest Seals (0-17)");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
