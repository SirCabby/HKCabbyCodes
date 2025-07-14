using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using System.Collections.Generic;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Spells
{
    public class VengefulSpiritPatch : ISyncedValueList
    {
        private static readonly FlagDef flag = FlagInstances.fireballLevel;

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
                "NONE", "Vengeful Spirit", "Shade Soul"
            };
        }

        public static void AddPanel()
        {
            VengefulSpiritPatch patch = new VengefulSpiritPatch();
            DropdownPanel dropdownPanel = new DropdownPanel(patch, flag.ReadableName, Constants.DEFAULT_PANEL_HEIGHT);
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(dropdownPanel);
        }
    }
}
