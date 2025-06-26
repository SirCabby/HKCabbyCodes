using CabbyCodes.SyncedReferences;
using CabbyCodes.UI.CheatPanels;
using System;

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
            InputFieldPanel<int> panel = new(new VesselFragmentPatch(), KeyCodeMap.ValidChars.Numeric, 1, 120, "Vessel Fragments (0-2)");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
