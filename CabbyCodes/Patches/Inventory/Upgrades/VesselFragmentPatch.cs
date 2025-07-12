using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.Utilities;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Upgrades
{
    public class VesselFragmentPatch : ISyncedReference<int>
    {
        public int Get()
        {
            return FlagManager.GetIntFlag(FlagInstances.vesselFragments);
        }

        public void Set(int value)
        {
            FlagManager.SetIntFlag(FlagInstances.vesselFragments, value);
        }

        public static void AddPanel()
        {
            RangeInputFieldPanel<int> panel = new RangeInputFieldPanel<int>(new VesselFragmentPatch(), KeyCodeMap.ValidChars.Decimal, 0, 9, "Vessel Fragments (0-2)");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
