using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using System.Collections.Generic;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Charms
{
    public class RoyalCharmPatch : ISyncedValueList
    {
        private static readonly int ROYAL_CHARM_ID = 36;

        public int Get()
        {
            var royalCharm = CharmData.GetCharm(ROYAL_CHARM_ID);
            if (PlayerData.instance.GetBool(royalCharm.GotFlag.Id))
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
            var royalCharm = CharmData.GetCharm(ROYAL_CHARM_ID);
            if (value == 2)
            {
                PlayerData.instance.SetBool(royalCharm.GotFlag.Id, true);
                PlayerData.instance.royalCharmState = 4;
                PlayerData.instance.gotShadeCharm = true;
            }
            else if (value == 1)
            {
                PlayerData.instance.SetBool(royalCharm.GotFlag.Id, true);
                PlayerData.instance.royalCharmState = 3;
                PlayerData.instance.gotShadeCharm = false;
            }
            else
            {
                PlayerData.instance.SetBool(royalCharm.GotFlag.Id, false);
                PlayerData.instance.royalCharmState = 0;
                PlayerData.instance.gotShadeCharm = false;
            }
        }

        public List<string> GetValueList()
        {
            return new List<string>
            {
                "NONE", "Kingsoul", "Void Heart"
            };
        }

        public static void AddPanel()
        {
            RoyalCharmPatch patch = new RoyalCharmPatch();
            DropdownPanel dropdownPanel = new DropdownPanel(patch, "40: Kingsoul / Void Heart", Constants.DEFAULT_PANEL_HEIGHT);
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(dropdownPanel);
        }
    }
}
