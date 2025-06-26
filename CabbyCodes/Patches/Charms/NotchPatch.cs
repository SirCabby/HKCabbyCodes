using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using System;

namespace CabbyCodes.Patches.Charms
{
    public class NotchPatch : ISyncedReference<int>
    {
        public int Get()
        {
            return PlayerData.instance.charmSlots;
        }

        public void Set(int value)
        {
            value = CabbyMenu.ValidationUtils.ValidateRange(value, Constants.MIN_CHARM_NOTCHES, Constants.MAX_CHARM_NOTCHES, nameof(value));
            PlayerData.instance.charmSlots = value;
        }

        public static void AddPanel()
        {
            InputFieldPanel<int> panel = new(new NotchPatch(), CabbyMenu.KeyCodeMap.ValidChars.Numeric, 2, 120, "Notches (3-11)");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
