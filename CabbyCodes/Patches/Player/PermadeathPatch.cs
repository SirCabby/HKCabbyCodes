using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;

namespace CabbyCodes.Patches
{
    public class PermadeathPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return PlayerData.instance.permadeathMode == Constants.PERMADEATH_MODE_ENABLED;
        }

        public void Set(bool value)
        {
            PlayerData.instance.permadeathMode = value ? 1 : 0;
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new PermadeathPatch(), "Steel Soul Mode"));
        }
    }
}
