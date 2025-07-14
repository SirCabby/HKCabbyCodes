using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.UI.Modders;
using CabbyCodes.Flags.FlagData;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Charms
{
    public class UpgradeCharmPatch : ISyncedReference<bool>
    {
        private readonly FlagDef upgradeFlag;

        public UpgradeCharmPatch(FlagDef upgradeFlag)
        {
            this.upgradeFlag = upgradeFlag;
        }

        public bool Get()
        {
            return FlagManager.GetBoolFlag(upgradeFlag);
        }

        public void Set(bool value)
        {
            FlagManager.SetBoolFlag(upgradeFlag, value);
            CabbyCodesPlugin.cabbyMenu.UpdateCheatPanels();
        }

        public static void AddPanels()
        {
            var upgradeableCharms = CharmData.GetUpgradeableCharms();
            
            foreach (var charm in upgradeableCharms)
            {
                UpgradeCharmPatch patch = new UpgradeCharmPatch(charm.UpgradeFlag);

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
