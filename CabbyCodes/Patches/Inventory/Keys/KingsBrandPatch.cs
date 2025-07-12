using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Keys
{
    public class KingsBrandPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return FlagManager.GetBoolFlag(FlagInstances.hasKingsBrand);
        }

        public void Set(bool value)
        {
            FlagManager.SetBoolFlag(FlagInstances.hasKingsBrand, value);
        }

        public static void AddPanel()
        {
            TogglePanel buttonPanel = new TogglePanel(new KingsBrandPatch(), "King's Brand");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(buttonPanel);
        }
    }
}
