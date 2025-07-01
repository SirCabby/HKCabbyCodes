using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.Utilities;

namespace CabbyCodes.Patches.Inventory.Upgrades
{
    public class VesselFragmentPatch : ISyncedReference<int>
    {
        public int Get()
        {
            return PlayerData.instance.vesselFragments;
        }

        public void Set(int value)
        {
            value = ValidationUtils.ValidateRange(value, 0, Constants.MAX_VESSEL_FRAGMENTS, nameof(value));
            PlayerData.instance.vesselFragments = value;
        }

        public static void AddPanel()
        {
            InputFieldPanel<int> panel = new InputFieldPanel<int>(new VesselFragmentPatch(), KeyCodeMap.ValidChars.Numeric, 1, "Vessel Fragments (0-2)");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
