using CabbyCodes.SyncedReferences;
using CabbyCodes.UI.CheatPanels;

namespace CabbyCodes.Patches.Inventory.Items
{
    public class HuntersJournalPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return PlayerData.instance.hasJournal;
        }

        public void Set(bool value)
        {
            PlayerData.instance.hasJournal = value;
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new HuntersJournalPatch(), "Hunter's Journal"));
        }
    }
}
