using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Items
{
    public class MapItemPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return FlagManager.GetBoolFlag(FlagInstances.hasMap);
        }

        public void Set(bool value)
        {
            FlagManager.SetBoolFlag(FlagInstances.hasMap, value);
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new MapItemPatch(), "Map"));
        }
    }
}
