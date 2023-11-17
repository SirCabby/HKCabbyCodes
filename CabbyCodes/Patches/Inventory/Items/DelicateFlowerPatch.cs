using CabbyCodes.SyncedReferences;
using CabbyCodes.UI.CheatPanels;
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
            if (value == 2)
            {
                PlayerData.instance.hasXunFlower = true;
                PlayerData.instance.xunFlowerBroken = false;
            }
            else if (value == 1)
            {
                PlayerData.instance.hasXunFlower = true;
                PlayerData.instance.xunFlowerBroken = true;
            }
            else
            {
                PlayerData.instance.hasXunFlower = false;
                PlayerData.instance.xunFlowerBroken = false;
            }
        }

        public List<string> GetValueList()
        {
            return new()
            {
                "NONE", "Ruined Flower", "Delicate Flower"
            };
        }

        public static void AddPanel()
        {
            DelicateFlowerPatch patch = new();
            DropdownPanel dropdownPanel = new(patch, 350, "Delicate Flower");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(dropdownPanel);
        }
    }
}
