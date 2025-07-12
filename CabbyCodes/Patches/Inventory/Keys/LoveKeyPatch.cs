using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Keys
{
    public class LoveKeyPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return FlagManager.GetBoolFlag(FlagInstances.hasLoveKey);
        }

        public void Set(bool value)
        {
            FlagManager.SetBoolFlag(FlagInstances.hasLoveKey, value);
        }

        public static void AddPanel()
        {
            TogglePanel buttonPanel = new TogglePanel(new LoveKeyPatch(), "Love Key");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(buttonPanel);
        }
    }
}
