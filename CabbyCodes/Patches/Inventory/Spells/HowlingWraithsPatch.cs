using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.Utilities;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Spells
{
    public class HowlingWraithsPatch : ISyncedReference<int>
    {
        public int Get()
        {
            return FlagManager.GetIntFlag(FlagInstances.screamLevel);
        }

        public void Set(int value)
        {
            FlagManager.SetIntFlag(FlagInstances.screamLevel, value);
        }

        public static void AddPanel()
        {
            RangeInputFieldPanel<int> panel = new RangeInputFieldPanel<int>(new HowlingWraithsPatch(), KeyCodeMap.ValidChars.Decimal, 0, 2, "Howling Wraiths / Abyss Shriek");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
