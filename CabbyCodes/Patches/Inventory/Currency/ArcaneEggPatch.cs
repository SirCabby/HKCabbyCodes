using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.Utilities;

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
            RangeInputFieldPanel<int> panel = new RangeInputFieldPanel<int>(new ArcaneEggPatch(), KeyCodeMap.ValidChars.Numeric, 0, Constants.MAX_ARCANE_EGGS, "Arcane Eggs (0-4)");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
