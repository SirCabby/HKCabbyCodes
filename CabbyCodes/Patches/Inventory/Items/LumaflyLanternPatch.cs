using CabbyCodes.SyncedReferences;
using CabbyCodes.UI.CheatPanels;

namespace CabbyCodes.Patches.Inventory.Items
{
    public class LumaflyLanternPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return PlayerData.instance.hasLantern;
        }

        public void Set(bool value)
        {
            PlayerData.instance.hasLantern = value;
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new LumaflyLanternPatch(), "Lumafly Lantern"));
        }
    }
}
