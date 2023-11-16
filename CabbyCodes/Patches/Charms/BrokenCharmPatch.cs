using CabbyCodes.SyncedReferences;
using CabbyCodes.Types;
using CabbyCodes.UI.CheatPanels;
using CabbyCodes.UI.Modders;
using System.Linq;

namespace CabbyCodes.Patches.Charms
{
    public class BrokenCharmPatch : ISyncedReference<bool>
    {
        public static readonly string brokenCharmName = "brokenCharm_";

        private readonly int charmIndex;

        public BrokenCharmPatch(int charmIndex)
        {
            this.charmIndex = charmIndex;
        }

        public bool Get()
        {
            return PlayerData.instance.GetBool(brokenCharmName + charmIndex);
        }

        public void Set(bool value)
        {
            PlayerData.instance.SetBool(brokenCharmName + charmIndex, value);
            CabbyCodesPlugin.cabbyMenu.UpdateCheatPanels();
        }

        public static void AddPanels()
        {
            for (int i = 23; i < 26; i++)
            {
                Charm charm = CharmPatch.charms.Single(x => x.id == i);
                BrokenCharmPatch patch = new(charm.id);

                int index = CharmPatch.charms.IndexOf(charm) + 1;
                TogglePanel togglePanel = new(patch, index + ": " + charm.name + " is Broken");
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
