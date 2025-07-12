using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Items
{
    public class GodtunerPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return FlagManager.GetBoolFlag(FlagInstances.hasGodfinder);
        }

        public void Set(bool value)
        {
            FlagManager.SetBoolFlag(FlagInstances.hasGodfinder, value);
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new GodtunerPatch(), "Godtuner"));
        }
    }
}
