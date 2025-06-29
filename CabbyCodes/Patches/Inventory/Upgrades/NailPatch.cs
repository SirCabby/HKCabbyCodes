using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using System.Collections.Generic;
using CabbyMenu;

namespace CabbyCodes.Patches.Inventory.Upgrades
{
    public class NailPatch : ISyncedValueList
    {
        public int Get()
        {
            return PlayerData.instance.nailSmithUpgrades;
        }

        public void Set(int value)
        {
            value = ValidationUtils.ValidateRange(value, 0, Constants.MAX_NAIL_UPGRADES, nameof(value));
            PlayerData.instance.nailSmithUpgrades = value;
        }

        public List<string> GetValueList()
        {
            return new List<string>
            {
                "Old Nail", "Sharpened Nail", "Channelled Nail", "Coiled Nail", "Pure Nail"
            };
        }

        public static void AddPanel()
        {
            InputFieldPanel<int> panel = new InputFieldPanel<int>(new NailPatch(), KeyCodeMap.ValidChars.Numeric, 1, Constants.PANEL_WIDTH_120, "Nail Upgrades (0-4)");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
