using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;

namespace CabbyCodes.Patches.Inventory.Keys
{
    public class KingsBrandPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return PlayerData.instance.hasKingsBrand;
        }

        public void Set(bool value)
        {
            PlayerData.instance.hasKingsBrand = value;
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new KingsBrandPatch(), "King's Brand"));
        }
    }
}
