using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;

namespace CabbyCodes.Patches.Inventory.NailArts
{
    public class UpwardSlashPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return PlayerData.instance.hasDashSlash;
        }

        public void Set(bool value)
        {
            PlayerData.instance.hasDashSlash = value;
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new UpwardSlashPatch(), "Great Slash"));
        }
    }
}
