using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Keys
{
    public class ElegantKeyPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return FlagManager.GetBoolFlag(FlagInstances.hasWhiteKey);
        }

        public void Set(bool value)
        {
            FlagManager.SetBoolFlag(FlagInstances.hasWhiteKey, value);
        }

        public static void AddPanel()
        {
            TogglePanel buttonPanel = new TogglePanel(new ElegantKeyPatch(), "Elegant Key");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(buttonPanel);
        }
    }
}
