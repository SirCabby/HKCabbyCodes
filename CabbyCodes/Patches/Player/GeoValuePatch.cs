using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu;

namespace CabbyCodes.Patches.Player
{
    public class GeoValuePatch : ISyncedReference<int>
    {
        public int Get()
        {
            return PlayerData.instance.geo;
        }

        public void Set(int value)
        {
            value = ValidationUtils.ValidateRange(value, 0, Constants.MAX_GEO, nameof(value));
            PlayerData.instance.geo = value;
        }

        public static void AddPanel()
        {
            InputFieldPanel<int> panel = new InputFieldPanel<int>(new GeoValuePatch(), KeyCodeMap.ValidChars.Numeric, 4, Constants.PANEL_WIDTH_120, "Geo (0-999999)");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}