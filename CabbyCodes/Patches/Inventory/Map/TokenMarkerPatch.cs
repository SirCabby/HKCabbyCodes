using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;

namespace CabbyCodes.Patches.Inventory.Map
{
    public class TokenMarkerPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return PlayerData.instance.hasMarker_y;
        }

        public void Set(bool value)
        {
            PlayerData.instance.hasMarker_y = value;
            if (value)
            {
                PlayerData.instance.hasMarker = true;
            }
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new TokenMarkerPatch(), "Token Marker"));
        }
    }
}
