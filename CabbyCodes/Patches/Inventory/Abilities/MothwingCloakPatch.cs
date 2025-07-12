using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using System.Collections.Generic;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Abilities
{
    public class MothwingCloakPatch : ISyncedValueList
    {
        public int Get()
        {
            if (!FlagManager.GetBoolFlag(FlagInstances.hasDash))
            {
                return 0;
            }
            else if (!FlagManager.GetBoolFlag(FlagInstances.hasSuperDash))
            {
                return 1;
            }

            return 2;
        }

        public void Set(int value)
        {
            if (value > 1)
            {
                FlagManager.SetBoolFlag(FlagInstances.hasDash, true);
                FlagManager.SetBoolFlag(FlagInstances.hasShadowDash, true);
            }
            else if (value == 1)
            {
                FlagManager.SetBoolFlag(FlagInstances.hasDash, true);
                FlagManager.SetBoolFlag(FlagInstances.hasShadowDash, false);
            }
            else
            {
                FlagManager.SetBoolFlag(FlagInstances.hasDash, false);
                FlagManager.SetBoolFlag(FlagInstances.hasShadowDash, false);
            }
        }

        public List<string> GetValueList()
        {
            return new List<string>
            {
                "NONE", "Mothwing Cloak", "Shade Cloak"
            };
        }

        public static void AddPanel()
        {
            MothwingCloakPatch patch = new MothwingCloakPatch();
            DropdownPanel dropdownPanel = new DropdownPanel(patch, "Mothwing Cloak / Shade Cloak", Constants.DEFAULT_PANEL_HEIGHT);
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(dropdownPanel);
        }
    }
}
