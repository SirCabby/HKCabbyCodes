using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using System.Collections.Generic;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Spells
{
    public class HowlingWraithsPatch : ISyncedValueList
    {
        public int Get()
        {
            return FlagManager.GetIntFlag(FlagInstances.screamLevel);
        }

        public void Set(int value)
        {
            FlagManager.SetIntFlag(FlagInstances.screamLevel, value);
        }

        public List<string> GetValueList()
        {
            return new List<string>
            {
                "NONE", "Howling Wraiths", "Abyss Shriek"
            };
        }

        public static void AddPanel()
        {
            HowlingWraithsPatch patch = new HowlingWraithsPatch();
            DropdownPanel dropdownPanel = new DropdownPanel(patch, "Howling Wraiths / Abyss Shriek", Constants.DEFAULT_PANEL_HEIGHT);
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(dropdownPanel);
        }
    }
}
