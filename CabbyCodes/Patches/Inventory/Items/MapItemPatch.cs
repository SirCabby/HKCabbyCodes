using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;

namespace CabbyCodes.Patches.Inventory.Items
{
    public class MapItemPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return PlayerData.instance.hasMap;
        }

        public void Set(bool value)
        {
            PlayerData.instance.hasMap = value;
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new MapItemPatch(), "Map"));
        }
    }
}
