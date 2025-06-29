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
            return
            [
                "NONE", "Howling Wraiths", "Abyss Shriek"
            ];
        }

        public static void AddPanel()
        {
            HowlingWraithsPatch patch = new();
            DropdownPanel dropdownPanel = new(patch, 350, "Howling Wraiths / Abyss Shriek");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(dropdownPanel);
        }
    }
}
