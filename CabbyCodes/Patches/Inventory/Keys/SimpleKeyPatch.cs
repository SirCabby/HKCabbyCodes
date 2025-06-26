using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
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
            value = CabbyMenu.ValidationUtils.ValidateRange(value, 0, Constants.MAX_SIMPLE_KEYS, nameof(value));
            PlayerData.instance.simpleKeys = value;
        }

        public static void AddPanel()
        {
            InputFieldPanel<int> panel = new(new SimpleKeyPatch(), CabbyMenu.KeyCodeMap.ValidChars.Numeric, 1, 120, "Simple Keys (0-4)");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
