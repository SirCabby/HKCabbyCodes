using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Keys
{
    public class CityCrestPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return FlagManager.GetBoolFlag(FlagInstances.hasCityKey);
        }

        public void Set(bool value)
        {
            FlagManager.SetBoolFlag(FlagInstances.hasCityKey, value);
        }

        public static void AddPanel()
        {
            TogglePanel buttonPanel = new TogglePanel(new CityCrestPatch(), "City Crest");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(buttonPanel);
        }
    }
}
