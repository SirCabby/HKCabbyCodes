using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.Utilities;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Upgrades
{
    public class VesselFragmentPatch : ISyncedReference<int>
    {
        private static readonly FlagDef flag = FlagInstances.vesselFragments;

        public int Get()
        {
            return FlagManager.GetIntFlag(flag);
        }

        public void Set(int value)
        {
            FlagManager.SetIntFlag(flag, value);
        }

        public static void AddPanel()
        {
            RangeInputFieldPanel<int> panel = new RangeInputFieldPanel<int>(new VesselFragmentPatch(), KeyCodeMap.ValidChars.Decimal, 0, 9, flag.ReadableName);
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
