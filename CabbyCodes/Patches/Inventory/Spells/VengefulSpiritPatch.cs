using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using System.Collections.Generic;

namespace CabbyCodes.Patches.Inventory.Spells
{
    public class VengefulSpiritPatch : ISyncedValueList
    {
        public int Get()
        {
            return PlayerData.instance.fireballLevel;
        }

        public void Set(int value)
        {
            PlayerData.instance.fireballLevel = value;
        }

        public List<string> GetValueList()
        {
            return new()
            {
                "NONE", "Vengeful Spirit", "Shade Soul"
            };
        }

        public static void AddPanel()
        {
            VengefulSpiritPatch patch = new();
            DropdownPanel dropdownPanel = new(patch, 350, "Vengeful Spirit / Shade Soul");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(dropdownPanel);
        }
    }
}
