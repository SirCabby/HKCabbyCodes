using CabbyCodes.SyncedReferences;
using CabbyCodes.UI.CheatPanels;
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
            NailPatch patch = new();
            DropdownPanel dropdownPanel = new(patch, 350, "Nail Level");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(dropdownPanel);
        }
    }
}
