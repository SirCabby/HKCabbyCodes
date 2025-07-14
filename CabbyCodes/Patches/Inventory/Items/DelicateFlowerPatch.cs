using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using System.Collections.Generic;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Items
{
    public class DelicateFlowerPatch : ISyncedValueList
    {
        private static readonly FlagDef flag1 = FlagInstances.hasXunFlower;
        private static readonly FlagDef flag2 = FlagInstances.xunFlowerBroken;

        public int Get()
        {
            if (FlagManager.GetBoolFlag(flag1) && !FlagManager.GetBoolFlag(flag2))
            {
                return 2;
            }
            else if (FlagManager.GetBoolFlag(flag1) && FlagManager.GetBoolFlag(flag2))
            {
                return 1;
            }

            return 0;
        }

        public void Set(int value)
        {
            if (value == Constants.DELICATE_FLOWER_BROKEN_STATE)
            {
                FlagManager.SetBoolFlag(flag1, true);
                FlagManager.SetBoolFlag(flag2, true);
            }
            else if (value == Constants.DELICATE_FLOWER_RETURNED_STATE)
            {
                FlagManager.SetBoolFlag(flag1, true);
                FlagManager.SetBoolFlag(flag2, false);
            }
            else
            {
                FlagManager.SetBoolFlag(flag1, false);
                FlagManager.SetBoolFlag(flag2, false);
            }
        }

        public List<string> GetValueList()
        {
            return new List<string>
            {
                "NONE", flag2.ReadableName, flag1.ReadableName
            };
        }

        public static void AddPanel()
        {
            DelicateFlowerPatch patch = new DelicateFlowerPatch();
            DropdownPanel dropdownPanel = new DropdownPanel(patch, flag1.ReadableName, Constants.DEFAULT_PANEL_HEIGHT);
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(dropdownPanel);
        }
    }
}
