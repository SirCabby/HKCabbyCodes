using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using System.Collections.Generic;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Abilities
{
    public class MothwingCloakPatch : ISyncedValueList
    {
        private static readonly FlagDef flag1 = FlagInstances.hasDash;
        private static readonly FlagDef flag2 = FlagInstances.hasShadowDash;

        public int Get()
        {
            if (!FlagManager.GetBoolFlag(flag1))
            {
                return 0;
            }
            else if (!FlagManager.GetBoolFlag(flag2))
            {
                return 1;
            }

            return 2;
        }

        public void Set(int value)
        {
            if (value > 1)
            {
                FlagManager.SetBoolFlag(flag1, true);
                FlagManager.SetBoolFlag(flag2, true);
            }
            else if (value == 1)
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
                "NONE", flag1.ReadableName, flag2.ReadableName
            };
        }

        public static void AddPanel()
        {
            MothwingCloakPatch patch = new MothwingCloakPatch();
            DropdownPanel dropdownPanel = new DropdownPanel(patch, flag1.ReadableName + " / " + flag2.ReadableName, Constants.DEFAULT_PANEL_HEIGHT);
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(dropdownPanel);
        }
    }
}
