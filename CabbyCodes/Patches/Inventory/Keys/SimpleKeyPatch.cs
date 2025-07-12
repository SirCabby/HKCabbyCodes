using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.Utilities;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Keys
{
    public class SimpleKeyPatch : ISyncedReference<int>
    {
        public int Get()
        {
            return FlagManager.GetIntFlag(FlagInstances.simpleKeys);
        }

        public void Set(int value)
        {
            FlagManager.SetIntFlag(FlagInstances.simpleKeys, value);
        }

        public static void AddPanel()
        {
            RangeInputFieldPanel<int> panel = new RangeInputFieldPanel<int>(new SimpleKeyPatch(), KeyCodeMap.ValidChars.Decimal, 0, 10, "Simple Keys (0-4)");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
