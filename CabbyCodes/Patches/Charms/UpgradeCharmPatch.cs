using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.UI.Modders;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Charms
{
    public class UpgradeCharmPatch : ISyncedReference<bool>
    {
        private readonly string boolName;

        public UpgradeCharmPatch(string boolName)
        {
            this.boolName = boolName;
        }

        public bool Get()
        {
            return PlayerData.instance.GetBool(boolName);
        }

        public void Set(bool value)
        {
            PlayerData.instance.SetBool(boolName, value);
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
