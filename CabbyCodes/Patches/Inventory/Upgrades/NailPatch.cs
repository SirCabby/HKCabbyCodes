using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using System.Collections.Generic;

namespace CabbyCodes.Patches.Inventory.PowerUps
{
    public class NailPatch : ISyncedValueList
    {
        public int Get()
        {
            return PlayerData.instance.nailSmithUpgrades;
        }

        public void Set(int value)
        {
            value = CabbyMenu.ValidationUtils.ValidateRange(value, 0, Constants.MAX_NAIL_UPGRADES, nameof(value));
            PlayerData.instance.nailSmithUpgrades = value;
        }

        public List<string> GetValueList()
        {
            return new()
            {
                "Old Nail", "Sharpened Nail", "Channelled Nail", "Coiled Nail", "Pure Nail"
            };
        }

        public static void AddPanel()
        {
            InputFieldPanel<int> panel = new(new NailPatch(), CabbyMenu.KeyCodeMap.ValidChars.Numeric, 1, Constants.PANEL_WIDTH_120, "Nail Upgrades (0-4)");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
