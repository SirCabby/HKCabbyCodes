using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.Utilities;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Charms
{
    public class NotchPatch : ISyncedReference<int>
    {
        public int Get()
        {
            return FlagManager.GetIntFlag(FlagInstances.charmSlots);
        }

        public void Set(int value)
        {
            value = ValidationUtils.ValidateRange(value, Constants.MIN_CHARM_NOTCHES, Constants.MAX_CHARM_NOTCHES, nameof(value));
            FlagManager.SetIntFlag(FlagInstances.charmSlots, value);
        }

        public static void AddPanel()
        {
            RangeInputFieldPanel<int> panel = new RangeInputFieldPanel<int>(new NotchPatch(), KeyCodeMap.ValidChars.Numeric, Constants.MIN_CHARM_NOTCHES, Constants.MAX_CHARM_NOTCHES, "Notches (3-11)");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
