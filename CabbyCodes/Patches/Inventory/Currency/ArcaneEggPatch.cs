using CabbyCodes.SyncedReferences;
using CabbyCodes.UI.CheatPanels;
using System;

namespace CabbyCodes.Patches.Inventory.Currency
{
    public class ArcaneEggPatch : ISyncedReference<int>
    {
        public int Get()
        {
            return PlayerData.instance.trinket4;
        }

        public void Set(int value)
        {
            value = ValidationUtils.ValidateRange(value, 0, Constants.MAX_ARCANE_EGGS, nameof(value));
            if (value > 0)
            {
                PlayerData.instance.foundTrinket4 = true;
            }
            PlayerData.instance.trinket4 = value;
        }

        public static void AddPanel()
        {
            InputFieldPanel<int> panel = new(new ArcaneEggPatch(), KeyCodeMap.ValidChars.Numeric, 1, 120, "Arcane Eggs (0-4)");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
