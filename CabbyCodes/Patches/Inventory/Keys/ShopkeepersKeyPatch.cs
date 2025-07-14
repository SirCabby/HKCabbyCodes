using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Keys
{
    public class ShopkeepersKeyPatch : ISyncedReference<bool>
    {
        private static readonly FlagDef flag = FlagInstances.hasSlykey;

        public bool Get()
        {
            return FlagManager.GetBoolFlag(flag);
        }

        public void Set(bool value)
        {
            FlagManager.SetBoolFlag(flag, value);
        }

        public static void AddPanel()
        {
            TogglePanel buttonPanel = new TogglePanel(new ShopkeepersKeyPatch(), flag.ReadableName);
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(buttonPanel);
        }
    }
}
