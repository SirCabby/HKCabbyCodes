using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.NailArts
{
    public class CyclonePatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return FlagManager.GetBoolFlag(FlagInstances.hasCyclone);
        }

        public void Set(bool value)
        {
            FlagManager.SetBoolFlag(FlagInstances.hasCyclone, value);
        }

        public static void AddPanel()
        {
            TogglePanel buttonPanel = new TogglePanel(new CyclonePatch(), "Cyclone Slash");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(buttonPanel);
        }
    }
}
