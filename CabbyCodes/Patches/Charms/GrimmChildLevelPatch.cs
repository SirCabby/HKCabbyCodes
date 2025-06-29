using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.UI.Modders;
using System.Collections.Generic;
using CabbyMenu;

namespace CabbyCodes.Patches.Charms
{
    public class GrimmChildLevelPatch : ISyncedValueList
    {
        public int Get()
        {
            int result = PlayerData.instance.grimmChildLevel - 1;
            return result;
        }

        public void Set(int value)
        {
            int result = value + 1;
            PlayerData.instance.grimmChildLevel = result;
            CabbyCodesPlugin.cabbyMenu.UpdateCheatPanels();
        }

        public List<string> GetValueList()
        {
            return new List<string>
            {
                "1", "2", "3", "4", "CM"
            };
        }

        public static void AddPanel()
        {
            int charmId = 40;
            GrimmChildLevelPatch patch = new GrimmChildLevelPatch();
            DropdownPanel dropdownPanel = new DropdownPanel(patch, 120, "39: Grimm Child Level (1-4) or Carefree Melody");

            (_, ImageMod spriteImageMod) = PanelAdder.AddSprite(dropdownPanel, CharmIconList.Instance.GetSprite(charmId), 1);

            dropdownPanel.updateActions.Add(() =>
            {
                spriteImageMod.SetSprite(CharmPatch.GetCharmIcon(charmId));
            });
            dropdownPanel.Update();

            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(dropdownPanel);
        }
    }
}
