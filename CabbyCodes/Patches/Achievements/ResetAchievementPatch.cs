using CabbyMenu.UI.CheatPanels;

namespace CabbyCodes.Patches.Achievements
{
    public class ResetAchievementPatch
    {
        public static void ResetAllAchievements()
        {
            AchievementHandler achievementHandler = UnityEngine.Object.FindObjectOfType<AchievementHandler>();
            achievementHandler.ResetAllAchievements();
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new ButtonPanel(ResetAllAchievements, "Reset ALL Achievements", "Reset all achievements to off. Requires Restart.", ButtonStyle.Danger));
        }
    }
}
