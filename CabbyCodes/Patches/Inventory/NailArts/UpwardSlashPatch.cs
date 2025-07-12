using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.NailArts
{
    public class UpwardSlashPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return FlagManager.GetBoolFlag(FlagInstances.hasDashSlash);
        }

        public void Set(bool value)
        {
            FlagManager.SetBoolFlag(FlagInstances.hasDashSlash, value);
        }

        public static void AddPanel()
        {
            TogglePanel buttonPanel = new TogglePanel(new UpwardSlashPatch(), "Great Slash");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(buttonPanel);
        }
    }
}
