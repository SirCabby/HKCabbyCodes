using CabbyMenu.UI.CheatPanels;
using CabbyMenu.UI.Modders;
using CabbyCodes.Flags.FlagData;

namespace CabbyCodes.Patches.Charms
{
    public class UpgradeCharmPatch : PlayerDataSyncedBool
    {
        public UpgradeCharmPatch(string boolName) : base(boolName)
        {
        }

        public override void Set(bool value)
        {
            base.Set(value);
            CabbyCodesPlugin.cabbyMenu.UpdateCheatPanels();
        }

        public static void AddPanels()
        {
            var upgradeableCharms = CharmData.GetUpgradeableCharms();
            
            foreach (var charm in upgradeableCharms)
            {
                UpgradeCharmPatch patch = new UpgradeCharmPatch(charm.UpgradeFlag.Id);

                int index = CharmPatch.charms.IndexOf(charm) + 1;
                TogglePanel togglePanel = new TogglePanel(patch, index + ": " + charm.Name + " is Unbreakable");
                (_, ImageMod spriteImageMod) = PanelAdder.AddSprite(togglePanel, CharmIconList.Instance.GetSprite(charm.Id), 1);

                togglePanel.updateActions.Add(() =>
                {
                    spriteImageMod.SetSprite(CharmPatch.GetCharmIcon(charm.Id));
                });
                togglePanel.Update();

                CabbyCodesPlugin.cabbyMenu.AddCheatPanel(togglePanel);
            }
        }
    }
}
