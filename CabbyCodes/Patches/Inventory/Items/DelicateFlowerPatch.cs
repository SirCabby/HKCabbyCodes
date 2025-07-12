using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using System.Collections.Generic;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Items
{
    public class DelicateFlowerPatch : ISyncedValueList
    {
        public int Get()
        {
            if (FlagManager.GetBoolFlag(FlagInstances.hasXunFlower) && !FlagManager.GetBoolFlag(FlagInstances.xunFlowerBroken))
            {
                return 2;
            }
            else if (FlagManager.GetBoolFlag(FlagInstances.hasXunFlower) && FlagManager.GetBoolFlag(FlagInstances.xunFlowerBroken))
            {
                return 1;
            }

            return 0;
        }

        public void Set(int value)
        {
            if (value == Constants.DELICATE_FLOWER_BROKEN_STATE)
            {
                FlagManager.SetBoolFlag(FlagInstances.hasXunFlower, true);
                FlagManager.SetBoolFlag(FlagInstances.xunFlowerBroken, true);
            }
            else if (value == Constants.DELICATE_FLOWER_RETURNED_STATE)
            {
                FlagManager.SetBoolFlag(FlagInstances.hasXunFlower, true);
                FlagManager.SetBoolFlag(FlagInstances.xunFlowerBroken, false);
            }
            else
            {
                FlagManager.SetBoolFlag(FlagInstances.hasXunFlower, false);
                FlagManager.SetBoolFlag(FlagInstances.xunFlowerBroken, false);
            }
        }

        public List<string> GetValueList()
        {
            return new List<string>
            {
                "NONE", "Ruined Flower", "Delicate Flower"
            };
        }

        public static void AddPanel()
        {
            DelicateFlowerPatch patch = new DelicateFlowerPatch();
            DropdownPanel dropdownPanel = new DropdownPanel(patch, "Delicate Flower", Constants.DEFAULT_PANEL_HEIGHT);
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(dropdownPanel);
        }
    }
}
