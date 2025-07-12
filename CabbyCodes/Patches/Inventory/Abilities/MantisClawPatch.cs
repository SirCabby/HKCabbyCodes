using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Abilities
{
    public class MantisClawPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return FlagManager.GetBoolFlag(FlagInstances.hasWalljump);
        }

        public void Set(bool value)
        {
            FlagManager.SetBoolFlag(FlagInstances.hasWalljump, value);
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new MantisClawPatch(), "Mantis Claw"));
        }
    }
}
