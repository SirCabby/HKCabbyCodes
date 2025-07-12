using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.UI.Modders;
using System.Collections.Generic;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Charms
{
    public class GrimmChildLevelPatch : ISyncedValueList
    {
        public int Get()
        {
            int result = FlagManager.GetIntFlag(FlagInstances.grimmChildLevel) - 1;
            return result;
        }

        public void Set(int value)
        {
            int result = value + 1;
            FlagManager.SetIntFlag(FlagInstances.grimmChildLevel, result);
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
            DropdownPanel dropdownPanel = new DropdownPanel(patch, "39: Grimm Child Level (1-4) or Carefree Melody", Constants.DEFAULT_PANEL_HEIGHT);

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
