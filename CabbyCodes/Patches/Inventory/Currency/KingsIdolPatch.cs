using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.Utilities;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Currency
{
    public class KingsIdolPatch : ISyncedReference<int>
    {
        public int Get()
        {
            return FlagManager.GetIntFlag(FlagInstances.trinket3);
        }

        public void Set(int value)
        {
            value = ValidationUtils.ValidateRange(value, 0, Constants.MAX_KINGS_IDOLS, nameof(value));
            if (value > 0)
            {
                FlagManager.SetBoolFlag(FlagInstances.foundTrinket3, true);
            }
            FlagManager.SetIntFlag(FlagInstances.trinket3, value);
        }

        public static void AddPanel()
        {
            RangeInputFieldPanel<int> panel = new RangeInputFieldPanel<int>(new KingsIdolPatch(), KeyCodeMap.ValidChars.Numeric, 0, Constants.MAX_KINGS_IDOLS, "King's Idols (0-8)");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
