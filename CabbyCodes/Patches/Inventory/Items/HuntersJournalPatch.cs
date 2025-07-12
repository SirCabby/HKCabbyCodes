using CabbyMenu.UI.CheatPanels;

namespace CabbyCodes.Patches.Inventory.Items
{
    public class HuntersJournalPatch : PlayerDataSyncedBool
    {
        public HuntersJournalPatch() : base("hasJournal")
        {
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new HuntersJournalPatch(), "Hunter's Journal"));
        }
    }
}
