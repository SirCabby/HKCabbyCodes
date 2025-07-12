using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Keys
{
    public class ShopkeepersKeyPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return FlagManager.GetBoolFlag(FlagInstances.hasSlykey);
        }

        public void Set(bool value)
        {
            FlagManager.SetBoolFlag(FlagInstances.hasSlykey, value);
        }

        public static void AddPanel()
        {
            TogglePanel buttonPanel = new TogglePanel(new ShopkeepersKeyPatch(), "Shopkeeper's Key");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(buttonPanel);
        }
    }
}
