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
            return new List<string>
            {
                "NONE", "Desolate Dive", "Descending Dark"
            };
        }

        public static void AddPanel()
        {
            DesolateDivePatch patch = new DesolateDivePatch();
            DropdownPanel dropdownPanel = new DropdownPanel(patch, "Desolate Dive / Descending Dark", Constants.DEFAULT_PANEL_HEIGHT);
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(dropdownPanel);
        }
    }
}
