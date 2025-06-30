using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;

namespace CabbyCodes.Patches.Player
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
            InputFieldPanel<float> panel = new InputFieldPanel<float>(new PlayTimePatch(), CabbyMenu.KeyCodeMap.ValidChars.Decimal, 14, "Playtime (seconds)");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
