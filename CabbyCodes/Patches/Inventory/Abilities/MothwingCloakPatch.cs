using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using System.Collections.Generic;

namespace CabbyCodes.Patches.Inventory.Abilities
{
    public class MothwingCloakPatch : ISyncedValueList
    {
        public int Get()
        {
            if (!PlayerData.instance.hasDash)
            {
                return 0;
            }
            else if (!PlayerData.instance.hasSuperDash)
            {
                return 1;
            }

            return 2;
        }

        public void Set(int value)
        {
            if (value > 1)
            {
                PlayerData.instance.hasDash = true;
                PlayerData.instance.hasShadowDash = true;
            }
            else if (value == 1)
            {
                PlayerData.instance.hasDash = true;
                PlayerData.instance.hasShadowDash = false;
            }
            else
            {
                PlayerData.instance.hasDash = false;
                PlayerData.instance.hasShadowDash = false;
            }
        }

        public List<string> GetValueList()
        {
            return
            [
                "NONE", "Mothwing Cloak", "Shade Cloak"
            ];
        }

        public static void AddPanel()
        {
            MothwingCloakPatch patch = new();
            DropdownPanel dropdownPanel = new(patch, 350, "Mothwing Cloak / Shade Cloak");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(dropdownPanel);
        }
    }
}
