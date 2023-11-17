using CabbyCodes.SyncedReferences;
using CabbyCodes.UI.CheatPanels;
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
            value = Math.Max(3, value);
            value = Math.Min(11, value);
            PlayerData.instance.charmSlots = value;
        }

        public static void AddPanel()
        {
            InputFieldPanel<int> panel = new(new NotchPatch(), KeyCodeMap.ValidChars.Numeric, 2, 120, "Notches (3-11)");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
