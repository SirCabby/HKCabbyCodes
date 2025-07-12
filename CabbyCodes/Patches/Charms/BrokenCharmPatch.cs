using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.UI.Modders;
using CabbyCodes.Flags.FlagData;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Charms
{
    public class BrokenCharmPatch : ISyncedReference<bool>
    {
        private readonly int charmIndex;

        public BrokenCharmPatch(int charmIndex)
        {
            this.charmIndex = charmIndex;
        }

        public bool Get()
        {
            var charm = CharmData.GetCharm(charmIndex);
            return FlagManager.GetBoolFlag(charm.BrokenFlag);
        }

        public void Set(bool value)
        {
            var charm = CharmData.GetCharm(charmIndex);
            FlagManager.SetBoolFlag(charm.BrokenFlag, value);
            CabbyCodesPlugin.cabbyMenu.UpdateCheatPanels();
        }

        public static void AddPanels()
        {
            var breakableCharms = CharmData.GetBreakableCharms();
            
            foreach (var charm in breakableCharms)
            {
                BrokenCharmPatch patch = new BrokenCharmPatch(charm.Id);

                int index = CharmPatch.charms.IndexOf(charm) + 1;
                TogglePanel togglePanel = new TogglePanel(patch, index + ": " + charm.Name + " is Broken");
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
