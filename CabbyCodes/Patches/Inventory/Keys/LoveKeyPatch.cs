using CabbyCodes.SyncedReferences;
using CabbyCodes.UI.CheatPanels;

namespace CabbyCodes.Patches.Inventory.Keys
{
    public class LoveKeyPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return PlayerData.instance.hasLoveKey;
        }

        public void Set(bool value)
        {
            PlayerData.instance.hasLoveKey = value;
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new LoveKeyPatch(), "Love Key"));
        }
    }
}
