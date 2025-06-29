using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using System.Collections.Generic;

namespace CabbyCodes.Patches.Inventory.Spells
{
    public class DesolateDivePatch : ISyncedValueList
    {
        public int Get()
        {
            return PlayerData.instance.quakeLevel;
        }

        public void Set(int value)
        {
            PlayerData.instance.quakeLevel = value;
        }

        public List<string> GetValueList()
        {
            return
            [
                "NONE", "Desolate Dive", "Descending Dark"
            ];
        }

        public static void AddPanel()
        {
            DesolateDivePatch patch = new();
            DropdownPanel dropdownPanel = new(patch, 350, "Desolate Dive / Descending Dark");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(dropdownPanel);
        }
    }
}
