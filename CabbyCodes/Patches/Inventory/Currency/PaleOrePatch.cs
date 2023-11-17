using CabbyCodes.SyncedReferences;
using CabbyCodes.UI.CheatPanels;
using System;

namespace CabbyCodes.Patches.Inventory.Currency
{
    public class PaleOrePatch : ISyncedReference<int>
    {
        public int Get()
        {
            return PlayerData.instance.ore;
        }

        public void Set(int value)
        {
            value = Math.Max(0, value);
            value = Math.Min(6, value);
            PlayerData.instance.ore = value;
        }

        public static void AddPanel()
        {
            InputFieldPanel<int> panel = new(new PaleOrePatch(), KeyCodeMap.ValidChars.Numeric, 1, 120, "Ore (0-6)");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
