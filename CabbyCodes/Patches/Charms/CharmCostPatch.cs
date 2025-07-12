using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Charms
{
    public class CharmCostPatch : ISyncedReference<bool>
    {
        public const string key = "CharmCost_Patch";
        private static readonly BoxedReference<bool> value = CodeState.Get(key, false);

        public bool Get()
        {
            return value.Get();
        }

        public void Set(bool value)
        {
            if (value)
            {
                foreach (var charm in CharmPatch.charms)
                {
                    FlagManager.SetIntFlag(charm.CostFlag, 0);
                }
            }
            else
            {
                foreach (var charm in CharmPatch.charms)
                {
                    // Get the default cost from the game data
                    int defaultCost = FlagManager.GetIntFlag(charm.CostFlag);
                    FlagManager.SetIntFlag(charm.CostFlag, defaultCost);
                }
            }

            CharmCostPatch.value.Set(value);
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new CharmCostPatch(), "Remove Charm Notch Cost"));
        }
    }
}
