using CabbyCodes.SyncedReferences;
using CabbyCodes.UI.CheatPanels;
using System;

namespace CabbyCodes.Patches.Player
{
    public class HealthPatch : ISyncedReference<int>
    {
        public int Get()
        {
            return PlayerData.instance.maxHealthBase;
        }

        public void Set(int value)
        {
            value = Math.Max(5, value);
            value = Math.Min(9, value);
            PlayerData.instance.maxHealthBase = value;
            PlayerData.instance.maxHealth = value;
        }

        public static void AddPanel()
        {
            InputFieldPanel<int> panel = new(new HealthPatch(), KeyCodeMap.ValidChars.Numeric, 1, 120, "Max Health (5-9)");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
