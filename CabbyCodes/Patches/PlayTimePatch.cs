using CabbyCodes.SyncedReferences;
using CabbyCodes.UI.CheatPanels;

namespace CabbyCodes.Patches
{
    public class PlayTimePatch : ISyncedReference<float>
    {
        public float Get()
        {
            return PlayerData.instance.playTime;
        }

        public void Set(float value)
        {
            PlayerData.instance.playTime = value;
        }

        public static void AddPanel()
        {
            InputFieldPanel<float> panel = new(new PlayTimePatch(), KeyCodeMap.ValidChars.Numeric, 9, 180, "Playtime (seconds)");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
