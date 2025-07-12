using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using System.Collections.Generic;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Spells
{
    public class VengefulSpiritPatch : ISyncedValueList
    {
        public int Get()
        {
            return FlagManager.GetIntFlag(FlagInstances.fireballLevel);
        }

        public void Set(int value)
        {
            FlagManager.SetIntFlag(FlagInstances.fireballLevel, value);
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
            DropdownPanel dropdownPanel = new DropdownPanel(patch, "Vengeful Spirit / Shade Soul", Constants.DEFAULT_PANEL_HEIGHT);
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(dropdownPanel);
        }
    }
}
