using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;

namespace CabbyCodes.Patches.Inventory.Items
{
    public class GleamingMarkerPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return PlayerData.instance.hasMarker_w;
        }

        public void Set(bool value)
        {
            PlayerData.instance.hasMarker_w = value;
            if (value)
            {
                PlayerData.instance.hasMarker = true;
            }
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new GleamingMarkerPatch(), "Gleaming Marker"));
        }
    }
}
