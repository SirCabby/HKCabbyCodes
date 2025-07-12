using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.Utilities;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Spells
{
    public class DesolateDivePatch : ISyncedReference<int>
    {
        public int Get()
        {
            return FlagManager.GetIntFlag(FlagInstances.quakeLevel);
        }

        public void Set(int value)
        {
            FlagManager.SetIntFlag(FlagInstances.quakeLevel, value);
        }

        public static void AddPanel()
        {
            RangeInputFieldPanel<int> panel = new RangeInputFieldPanel<int>(new DesolateDivePatch(), KeyCodeMap.ValidChars.Decimal, 0, 2, "Desolate Dive / Descending Dark");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
