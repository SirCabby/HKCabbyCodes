using CabbyCodes.Flags;
using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.Utilities;

namespace CabbyCodes.Patches.Inventory.Currency
{
    public class DreamEssencePatch : ISyncedReference<int>
    {
        private static readonly FlagDef flag = FlagInstances.dreamOrbs;
        
        public int Get()
        {
            return FlagManager.GetIntFlag(flag);
        }

        public void Set(int value)
        {
            value = ValidationUtils.ValidateRange(value, Constants.MIN_DREAM_ESSENCE, Constants.MAX_DREAM_ESSENCE, nameof(value));
            FlagManager.SetIntFlag(flag, value);
        }

        public static void AddPanel()
        {
            RangeInputFieldPanel<int> panel = new RangeInputFieldPanel<int>(new DreamEssencePatch(), KeyCodeMap.ValidChars.Numeric, Constants.MIN_DREAM_ESSENCE, Constants.MAX_DREAM_ESSENCE, flag.ReadableName);
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
