using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Patches.Teleport;

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
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new ButtonPanel(() => {
                TeleportService.ShowConfirmationDialog(
                    "Confirm Reset",
                    "This will reset ALL achievements to off.\n\nWarning: This action cannot be undone and requires a restart.\n\nAre you sure you want to proceed?",
                    "Reset All",
                    "Cancel",
                    ResetAllAchievements);
            }, "Reset ALL Achievements", "Reset all achievements to off. Requires Restart.", ButtonStyle.Danger));
        }
    }
}
