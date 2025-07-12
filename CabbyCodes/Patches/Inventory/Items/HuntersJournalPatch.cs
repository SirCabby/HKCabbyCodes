using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Items
{
    public class HuntersJournalPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return FlagManager.GetBoolFlag(FlagInstances.hasJournal);
        }

        public void Set(bool value)
        {
            FlagManager.SetBoolFlag(FlagInstances.hasJournal, value);
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new HuntersJournalPatch(), "Hunter's Journal"));
        }
    }
}
