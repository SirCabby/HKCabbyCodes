using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu;
using CabbyMenu.Utilities;

namespace CabbyCodes.Patches.Inventory.Currency
{
    public class GeoValuePatch : ISyncedReference<int>
    {
        public int Get()
        {
            return PlayerData.instance.geo;
        }

        public void Set(int value)
        {
            value = ValidationUtils.ValidateRange(value, Constants.MIN_GEO, Constants.MAX_GEO, nameof(value));

            PlayerData.instance.geo = value;

            CabbyCodesPlugin.BLogger.LogDebug(string.Format("Geo updated to {0}", value));
        }

        public static void AddPanel()
        {
            InputFieldPanel<int> panel = new InputFieldPanel<int>(new GeoValuePatch(), KeyCodeMap.ValidChars.Numeric, 6, "Geo (0-999999)");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
