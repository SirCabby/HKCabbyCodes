using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;

namespace CabbyCodes.Patches.Inventory.Keys
{
    public class ShopkeepersKeyPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return PlayerData.instance.hasSlykey;
        }

        public void Set(bool value)
        {
            PlayerData.instance.hasSlykey = value;
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new ShopkeepersKeyPatch(), "Shopkeeper's Key"));
        }
    }
}
