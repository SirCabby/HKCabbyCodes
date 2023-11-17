using CabbyCodes.SyncedReferences;
using CabbyCodes.UI.CheatPanels;
using System;

namespace CabbyCodes.Patches.Inventory.Currency
{
    public class WanderersJournalPatch : ISyncedReference<int>
    {
        public int Get()
        {
            return PlayerData.instance.trinket1;
        }

        public void Set(int value)
        {
            value = Math.Max(0, value);
            value = Math.Min(14, value);
            if (value > 0)
            {
                PlayerData.instance.foundTrinket1 = true;
            }
            PlayerData.instance.trinket1 = value;
        }

        public static void AddPanel()
        {
            InputFieldPanel<int> panel = new(new WanderersJournalPatch(), KeyCodeMap.ValidChars.Numeric, 2, 120, "Wanderer's Journals (0-14)");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
