using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu;

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
            value = ValidationUtils.ValidateRange(value, Constants.MIN_HEALTH, Constants.MAX_HEALTH, nameof(value));

            PlayerData.instance.maxHealthBase = value;
            PlayerData.instance.maxHealth = value;

            CabbyCodesPlugin.BLogger.LogDebug(string.Format("Health updated to {0}", value));
        }

        public static void AddPanel()
        {
            InputFieldPanel<int> panel = new InputFieldPanel<int>(new HealthPatch(), KeyCodeMap.ValidChars.Numeric, CabbyMenu.Constants.DEFAULT_CHARACTER_LIMIT, "Max Health (5-9)");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
