using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using System.Collections.Generic;

namespace CabbyCodes.Patches.Inventory.Abilities
{
    public class DreamNailPatch : ISyncedValueList
    {
        public int Get()
        {
            if (PlayerData.instance.dreamNailUpgraded)
            {
                return 2;
            }
            else if (PlayerData.instance.hasDreamNail)
            {
                return 1;
            }

            return 0;
        }

        public void Set(int value)
        {
            if (value == 2)
            {
                PlayerData.instance.hasDreamNail = true;
                PlayerData.instance.dreamNailUpgraded = true;
            }
            else if (value == 1)
            {
                PlayerData.instance.hasDreamNail = true;
                PlayerData.instance.dreamNailUpgraded = false;
            }
            else
            {
                PlayerData.instance.hasDreamNail = false;
                PlayerData.instance.dreamNailUpgraded = false;
            }
        }

        public List<string> GetValueList()
        {
            return new()
            {
                "NONE", "Dream Nail", "Awoken Dream Nail"
            };
        }

        public static void AddPanel()
        {
            DreamNailPatch patch = new();
            DropdownPanel dropdownPanel = new(patch, 350, "Dream Nail / Awoken Dream Nail");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(dropdownPanel);
        }
    }
}
