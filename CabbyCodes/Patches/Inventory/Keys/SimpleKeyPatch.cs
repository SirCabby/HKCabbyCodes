using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.Utilities;

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
            value = ValidationUtils.ValidateRange(value, 0, Constants.MAX_SIMPLE_KEYS, nameof(value));
            PlayerData.instance.simpleKeys = value;
        }

        public static void AddPanel()
        {
            InputFieldPanel<int> panel = new InputFieldPanel<int>(new SimpleKeyPatch(), KeyCodeMap.ValidChars.Numeric, 1, "Simple Keys (0-4)");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
