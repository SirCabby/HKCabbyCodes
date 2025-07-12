using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.NailArts
{
    public class DashSlashPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return FlagManager.GetBoolFlag(FlagInstances.hasUpwardSlash);
        }

        public void Set(bool value)
        {
            FlagManager.SetBoolFlag(FlagInstances.hasUpwardSlash, value);
        }

        public static void AddPanel()
        {
            TogglePanel buttonPanel = new TogglePanel(new DashSlashPatch(), "Dash Slash");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(buttonPanel);
        }
    }
}
