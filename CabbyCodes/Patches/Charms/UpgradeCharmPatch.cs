using CabbyCodes.SyncedReferences;
using CabbyCodes.Types;
using CabbyCodes.UI.CheatPanels;
using CabbyCodes.UI.Modders;
using System.Linq;

namespace CabbyCodes.Patches
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
            for (int i = 23; i < 26; i++)
            {
                string charmBool = "";
                switch (i)
                {
                    case 23:
                        charmBool = "fragileHealth_unbreakable";
                        break;
                    case 24:
                        charmBool = "fragileGreed_unbreakable";
                        break;
                    case 25:
                        charmBool = "fragileStrength_unbreakable";
                        break;
                }

                Charm charm = CharmPatch.charms.Single(x => x.id == i);
                UpgradeCharmPatch patch = new(charmBool);

                int index = CharmPatch.charms.IndexOf(charm) + 1;
                TogglePanel togglePanel = new(patch, index + ": " + charm.name + " is Unbreakable");
                (_, ImageMod spriteImageMod) = PanelAdder.AddSprite(togglePanel, CharmIconList.Instance.GetSprite(charm.id), 1);

                togglePanel.updateActions.Add(() =>
                {
                    spriteImageMod.SetSprite(CharmPatch.GetCharmIcon(charm.id));
                });
                togglePanel.Update();

                CabbyCodesPlugin.cabbyMenu.AddCheatPanel(togglePanel);
            }
        }
    }
}
