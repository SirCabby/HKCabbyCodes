using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using System.Collections.Generic;

namespace CabbyCodes.Patches.Inventory.Spells
{
    public class HowlingWraithsPatch : ISyncedValueList
    {
        public int Get()
        {
            return PlayerData.instance.screamLevel;
        }

        public void Set(int value)
        {
            PlayerData.instance.screamLevel = value;
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
            DropdownPanel dropdownPanel = new DropdownPanel(patch, "Howling Wraiths / Abyss Shriek");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(dropdownPanel);
        }
    }
}
