using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.Utilities;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Player
{
    public class HealthPatch : ISyncedReference<int>
    {
        public int Get()
        {
            return FlagManager.GetIntFlag(FlagInstances.maxHealthBase);
        }

        public void Set(int value)
        {
            value = ValidationUtils.ValidateRange(value, Constants.MIN_HEALTH, Constants.MAX_HEALTH, nameof(value));

            FlagManager.SetIntFlag(FlagInstances.maxHealthBase, value);
            FlagManager.SetIntFlag(FlagInstances.maxHealth, value);

            CabbyCodesPlugin.BLogger.LogDebug(string.Format("Health updated to {0}", value));
        }

        public static void AddPanel()
        {
            RangeInputFieldPanel<int> panel = new RangeInputFieldPanel<int>(new HealthPatch(), KeyCodeMap.ValidChars.Numeric, Constants.MIN_HEALTH, Constants.MAX_HEALTH, "Max Health (5-9)");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
