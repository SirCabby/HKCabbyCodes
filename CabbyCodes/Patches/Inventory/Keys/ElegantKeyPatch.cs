using CabbyCodes.SyncedReferences;
using CabbyCodes.UI.CheatPanels;

namespace CabbyCodes.Patches.Inventory.Keys
{
    public class ElegantKeyPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return PlayerData.instance.hasWhiteKey;
        }

        public void Set(bool value)
        {
            PlayerData.instance.hasWhiteKey = value;
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new ElegantKeyPatch(), "Elegant Key"));
        }
    }
}
