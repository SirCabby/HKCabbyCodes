using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;

namespace CabbyCodes.Patches
{
    public class ResetAchievementPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return false;
        }

        public void Set(bool value)
        {
            AchievementHandler achievementHandler = UnityEngine.Object.FindObjectOfType<AchievementHandler>();
            achievementHandler.ResetAllAchievements();
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new ResetAchievementPatch(), "Reset <ALL> Achievements to off. Requires Restart."));
        }
    }
}
