using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.Utilities;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Player
{
    public class PlayTimePatch : ISyncedReference<float>
    {
        public float Get()
        {
            return FlagManager.GetFloatFlag("playTime", "Global");
        }

        public void Set(float value)
        {
            FlagManager.SetFloatFlag("playTime", "Global", value);
        }

        public static void AddPanel()
        {
            RangeInputFieldPanel<float> panel = new RangeInputFieldPanel<float>(new PlayTimePatch(), KeyCodeMap.ValidChars.Decimal, 0f, 999999f, "Playtime (seconds)");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
