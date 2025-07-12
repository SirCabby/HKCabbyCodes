using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using System.Collections.Generic;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Abilities
{
    public class DreamNailPatch : ISyncedValueList
    {
        public int Get()
        {
            if (FlagManager.GetBoolFlag(FlagInstances.dreamNailUpgraded))
            {
                return 2;
            }
            else if (FlagManager.GetBoolFlag(FlagInstances.hasDreamNail))
            {
                return 1;
            }

            return 0;
        }

        public void Set(int value)
        {
            if (value == 2)
            {
                FlagManager.SetBoolFlag(FlagInstances.hasDreamNail, true);
                FlagManager.SetBoolFlag(FlagInstances.dreamNailUpgraded, true);
            }
            else if (value == 1)
            {
                FlagManager.SetBoolFlag(FlagInstances.hasDreamNail, true);
                FlagManager.SetBoolFlag(FlagInstances.dreamNailUpgraded, false);
            }
            else
            {
                FlagManager.SetBoolFlag(FlagInstances.hasDreamNail, false);
                FlagManager.SetBoolFlag(FlagInstances.dreamNailUpgraded, false);
            }
        }

        public List<string> GetValueList()
        {
            return new List<string>
            {
                "NONE", "Dream Nail", "Awoken Dream Nail"
            };
        }

        public static void AddPanel()
        {
            DreamNailPatch patch = new DreamNailPatch();
            DropdownPanel dropdownPanel = new DropdownPanel(patch, "Dream Nail / Awoken Dream Nail", Constants.DEFAULT_PANEL_HEIGHT);
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(dropdownPanel);
        }
    }
}
