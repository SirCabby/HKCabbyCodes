using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Abilities
{
    public class DreamgatePatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return FlagManager.GetBoolFlag(FlagInstances.hasDreamGate);
        }

        public void Set(bool value)
        {
            FlagManager.SetBoolFlag(FlagInstances.hasDreamGate, value);
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new DreamgatePatch(), "Dream Gate"));
        }
    }
}
