using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using System.Collections.Generic;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Spells
{
    public class DesolateDivePatch : ISyncedValueList
    {
        private static readonly FlagDef flag = FlagInstances.quakeLevel;

        public int Get()
        {
            return FlagManager.GetIntFlag(flag);
        }

        public void Set(int value)
        {
            FlagManager.SetIntFlag(flag, value);
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
            DropdownPanel dropdownPanel = new DropdownPanel(patch, flag.ReadableName, Constants.DEFAULT_PANEL_HEIGHT);
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(dropdownPanel);
        }
    }
}
