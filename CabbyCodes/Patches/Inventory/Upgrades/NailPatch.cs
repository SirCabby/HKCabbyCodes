using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using System.Collections.Generic;
using CabbyMenu.Utilities;

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
            RangeInputFieldPanel<int> panel = new RangeInputFieldPanel<int>(new NailPatch(), KeyCodeMap.ValidChars.Numeric, 0, Constants.MAX_NAIL_UPGRADES, "Nail Upgrades (0-4)");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
