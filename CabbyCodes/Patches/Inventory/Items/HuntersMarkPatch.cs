using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Items
{
    public class HuntersMarkPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return FlagManager.GetBoolFlag(FlagInstances.hasHuntersMark);
        }

        public void Set(bool value)
        {
            FlagManager.SetBoolFlag(FlagInstances.hasHuntersMark, value);
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new HuntersMarkPatch(), "Hunter's Mark"));
        }
    }
}
