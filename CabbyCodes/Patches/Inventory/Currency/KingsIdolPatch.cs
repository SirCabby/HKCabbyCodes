using CabbyCodes.SyncedReferences;
using CabbyCodes.UI.CheatPanels;
using System;

namespace CabbyCodes.Patches.Inventory.Currency
{
    public class KingsIdolPatch : ISyncedReference<int>
    {
        public int Get()
        {
            return PlayerData.instance.trinket3;
        }

        public void Set(int value)
        {
            value = Math.Max(0, value);
            value = Math.Min(8, value);
            if (value > 0)
            {
                PlayerData.instance.foundTrinket3 = true;
            }
            PlayerData.instance.trinket3 = value;
        }

        public static void AddPanel()
        {
            InputFieldPanel<int> panel = new(new KingsIdolPatch(), KeyCodeMap.ValidChars.Numeric, 1, 120, "King's Idols (0-8)");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
