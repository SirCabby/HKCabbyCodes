using CabbyCodes.SyncedReferences;
using CabbyCodes.UI.CheatPanels;
using System;

namespace CabbyCodes.Patches.Inventory.Keys
{
    public class SimpleKeyPatch : ISyncedReference<int>
    {
        public int Get()
        {
            return PlayerData.instance.simpleKeys;
        }

        public void Set(int value)
        {
            value = Math.Max(0, value);
            value = Math.Min(4, value);
            PlayerData.instance.simpleKeys = value;
        }

        public static void AddPanel()
        {
            InputFieldPanel<int> panel = new(new SimpleKeyPatch(), KeyCodeMap.ValidChars.Numeric, 1, 120, "Simple Keys (0-4)");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
