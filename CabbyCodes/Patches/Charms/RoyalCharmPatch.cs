using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using System.Collections.Generic;

namespace CabbyCodes.Patches
{
    public class RoyalCharmPatch : ISyncedValueList
    {
        public int Get()
        {
            if (PlayerData.instance.gotCharm_36)
            {
                if (PlayerData.instance.royalCharmState == 3)
                {
                    return 1;
                }
                else if (PlayerData.instance.royalCharmState == 4)
                {
                    return 2;
                }
            }

            return 0;
        }

        public void Set(int value)
        {
            if (value == 2)
            {
                PlayerData.instance.gotCharm_36 = true;
                PlayerData.instance.royalCharmState = 4;
                PlayerData.instance.gotShadeCharm = true;
            }
            else if (value == 1)
            {
                PlayerData.instance.gotCharm_36 = true;
                PlayerData.instance.royalCharmState = 3;
                PlayerData.instance.gotShadeCharm = false;
            }
            else
            {
                PlayerData.instance.gotCharm_36 = false;
                PlayerData.instance.royalCharmState = 0;
                PlayerData.instance.gotShadeCharm = false;
            }
        }

        public List<string> GetValueList()
        {
            return new()
            {
                "NONE", "Kingsoul", "Void Heart"
            };
        }

        public static void AddPanel()
        {
            RoyalCharmPatch patch = new();
            DropdownPanel dropdownPanel = new(patch, 230, "40: Kingsoul / Void Heart");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(dropdownPanel);
        }
    }
}
