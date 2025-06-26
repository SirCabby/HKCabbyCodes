using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;

namespace CabbyCodes.Patches.Inventory.Map
{
    public class ScarabMarkerPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return PlayerData.instance.hasMarker_b;
        }

        public void Set(bool value)
        {
            PlayerData.instance.hasMarker_b = value;
            if (value)
            {
                PlayerData.instance.hasMarker = true;
            }
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new ScarabMarkerPatch(), "Scarab Marker"));
        }
    }
}
