using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Map
{
    public class TokenMarkerPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return FlagManager.GetBoolFlag(FlagInstances.hasMarker_y);
        }

        public void Set(bool value)
        {
            FlagManager.SetBoolFlag(FlagInstances.hasMarker_y, value);
            if (value)
                FlagManager.SetBoolFlag(FlagInstances.hasMarker, true);
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new TokenMarkerPatch(), "Token Marker"));
        }
    }
}
