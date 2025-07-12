using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Items
{
    public class LumaflyLanternPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return FlagManager.GetBoolFlag(FlagInstances.hasLantern);
        }

        public void Set(bool value)
        {
            FlagManager.SetBoolFlag(FlagInstances.hasLantern, value);
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new LumaflyLanternPatch(), "Lumafly Lantern"));
        }
    }
}
