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
            value = Math.Max(0, value);
            value = Math.Min(2, value);
            PlayerData.instance.vesselFragments = value;
        }

        public static void AddPanel()
        {
            InputFieldPanel<int> panel = new(new VesselFragmentPatch(), KeyCodeMap.ValidChars.Numeric, 1, 120, "Vessel Fragments (0-2)");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
