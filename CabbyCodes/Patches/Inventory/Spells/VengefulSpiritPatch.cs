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
            return new List<string>
            {
                "NONE", "Vengeful Spirit", "Shade Soul"
            };
        }

        public static void AddPanel()
        {
            VengefulSpiritPatch patch = new VengefulSpiritPatch();
            DropdownPanel dropdownPanel = new DropdownPanel(patch, 350, "Vengeful Spirit / Shade Soul");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(dropdownPanel);
        }
    }
}
