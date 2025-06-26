using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;

namespace CabbyCodes.Patches.Inventory.Items
{
    public class HuntersMarkPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return PlayerData.instance.hasHuntersMark;
        }

        public void Set(bool value)
        {
            PlayerData.instance.hasHuntersMark = value;
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new HuntersMarkPatch(), "Hunter's Mark"));
        }
    }
}
