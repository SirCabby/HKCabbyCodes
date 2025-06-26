using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;

namespace CabbyCodes.Patches.Inventory.NailArts
{
    public class DashSlashPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return PlayerData.instance.hasUpwardSlash;
        }

        public void Set(bool value)
        {
            PlayerData.instance.hasUpwardSlash = value;
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new DashSlashPatch(), "Dash Slash"));
        }
    }
}
