using CabbyCodes.SyncedReferences;
using CabbyCodes.Types;
using CabbyCodes.UI.CheatPanels;
using CabbyCodes.UI.Modders;
using System.Collections.Generic;

namespace CabbyCodes.Patches
{
    public class GrimmChildLevelPatch : ISyncedValueList<int>
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
            return new()
            {
                "1", "2", "3", "4", "CM"
            };
        }

        public static void AddPanel()
        {
            int charmId = 40;
            GrimmChildLevelPatch patch = new();
            DropdownPanel dropdownPanel = new(patch, 120, "39: Grimm Child Level (1-4) or Carefree Melody");

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
