using CabbyCodes.SyncedReferences;
using CabbyCodes.UI.CheatPanels;
using System;

namespace CabbyCodes.Patches
{
    public class GeoValuePatch : ISyncedReference<int>
    {
        public int Get()
        {
            return PlayerData.instance.geo;
        }

        public void Set(int value)
        {
            value = Math.Max(0, value);
            value = Math.Min(9999999, value);
            PlayerData.instance.geo = value;
        }

        public static void AddPanel()
        {
            InputFieldPanel<int> panel = new(new GeoValuePatch(), KeyCodeMap.ValidChars.Numeric, 7, 180, "Edit Current Geo (0-9999999)");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
