using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Items
{
    public class TramPassPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return FlagManager.GetBoolFlag(FlagInstances.hasTramPass);
        }

        public void Set(bool value)
        {
            FlagManager.SetBoolFlag(FlagInstances.hasTramPass, value);
        }

        public static void AddPanel()
        {
            TogglePanel buttonPanel = new TogglePanel(new TramPassPatch(), "Tram Pass");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(buttonPanel);
        }
    }
}
