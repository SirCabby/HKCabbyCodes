using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Abilities
{
    public class CrystalHeartPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return FlagManager.GetBoolFlag(FlagInstances.hasSuperDash);
        }

        public void Set(bool value)
        {
            FlagManager.SetBoolFlag(FlagInstances.hasSuperDash, value);
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new CrystalHeartPatch(), "Crystal Heart"));
        }
    }
}
