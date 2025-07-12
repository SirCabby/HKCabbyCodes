using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Player
{
    public class PermadeathPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return FlagManager.GetIntFlag(FlagInstances.permadeathMode) == Constants.PERMADEATH_MODE_ENABLED;
        }

        public void Set(bool value)
        {
            FlagManager.SetIntFlag(FlagInstances.permadeathMode, value ? 1 : 0);
        }

        public static void AddPanel()
        {
            TogglePanel buttonPanel = new TogglePanel(new PermadeathPatch(), "Steel Soul Mode (Permadeath)");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(buttonPanel);
        }
    }
}
