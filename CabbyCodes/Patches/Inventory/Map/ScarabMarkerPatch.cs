using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Map
{
    public class ScarabMarkerPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return FlagManager.GetBoolFlag(FlagInstances.hasMarker_b);
        }

        public void Set(bool value)
        {
            FlagManager.SetBoolFlag(FlagInstances.hasMarker_b, value);
            if (value)
                FlagManager.SetBoolFlag(FlagInstances.hasMarker, true);
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new ScarabMarkerPatch(), "Scarab Marker"));
        }
    }
}
