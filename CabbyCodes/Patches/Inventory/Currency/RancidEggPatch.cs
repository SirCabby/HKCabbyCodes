using CabbyCodes.SyncedReferences;
using CabbyCodes.UI.CheatPanels;
using System;

namespace CabbyCodes.Patches.Inventory.Currency
{
    public class RancidEggPatch : ISyncedReference<int>
    {
        public int Get()
        {
            return PlayerData.instance.rancidEggs;
        }

        public void Set(int value)
        {
            value = Math.Max(0, value);
            value = Math.Min(80, value);
            PlayerData.instance.rancidEggs = value;
        }

        public static void AddPanel()
        {
            InputFieldPanel<int> panel = new(new RancidEggPatch(), KeyCodeMap.ValidChars.Numeric, 2, 120, "Rancid Eggs (0-80)");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
