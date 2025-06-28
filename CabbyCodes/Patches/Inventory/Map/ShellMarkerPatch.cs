using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;

namespace CabbyCodes.Patches.Inventory.Map
{
    public class ShellMarkerPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return PlayerData.instance.hasMarker_r;
        }

        public void Set(bool value)
        {
            PlayerData.instance.hasMarker_r = value;
            if (value)
            {
                PlayerData.instance.hasMarker = true;
            }
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new ShellMarkerPatch(), "Shell Marker"));
        }
    }
}
