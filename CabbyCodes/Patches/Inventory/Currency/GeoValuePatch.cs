using CabbyCodes.SyncedReferences;
using CabbyCodes.UI.CheatPanels;
using System;

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
            
            CabbyCodesPlugin.BLogger.LogDebug("Geo updated to {0}", value);
        }

        public static void AddPanel()
        {
            InputFieldPanel<int> panel = new(new GeoValuePatch(), KeyCodeMap.ValidChars.Numeric, 7, 180, "Geo (0-9999999)");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
