using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Map
{
    public class HotSpringPinPatch : ISyncedReference<bool>
    {
        private static readonly FlagDef flag1 = FlagInstances.hasPinSpa;
        private static readonly FlagDef flag2 = FlagInstances.hasPin;

        public bool Get()
        {
            return FlagManager.GetBoolFlag(flag1);
        }

        public void Set(bool value)
        {
            FlagManager.SetBoolFlag(flag1, value);
            if (value)
                FlagManager.SetBoolFlag(flag2, true);
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new HotSpringPinPatch(), flag1.ReadableName));
        }
    }
}
