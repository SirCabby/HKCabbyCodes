using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using System.Collections.Generic;

namespace CabbyCodes.Patches.Inventory.Items
{
    public class DelicateFlowerPatch : ISyncedValueList
    {
        public int Get()
        {
            if (PlayerData.instance.hasXunFlower && !PlayerData.instance.xunFlowerBroken)
            {
                return 2;
            }
            else if (PlayerData.instance.hasXunFlower && PlayerData.instance.xunFlowerBroken)
            {
                return 1;
            }

            return 0;
        }

        public void Set(int value)
        {
            if (value == Constants.DELICATE_FLOWER_BROKEN_STATE)
            {
                PlayerData.instance.hasXunFlower = true;
                PlayerData.instance.xunFlowerBroken = true;
            }
            else if (value == Constants.DELICATE_FLOWER_RETURNED_STATE)
            {
                PlayerData.instance.hasXunFlower = true;
                PlayerData.instance.xunFlowerBroken = false;
            }
            else
            {
                PlayerData.instance.hasXunFlower = false;
                PlayerData.instance.xunFlowerBroken = false;
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
