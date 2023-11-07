using CabbyCodes.UI.CheatPanels;

namespace CabbyCodes.SyncedReferences
{
    public class AchievementReference : SyncedReference<bool>
    {
        public AchievementReference(Achievement achievement, AchievementPanel parent)
        {
            Get = () =>
            {
                return GameManager.instance.IsAchievementAwarded(achievement.key);
            };

            Set = (isAchieved) =>
            {
                if (isAchieved && !Get())
                {
                    AchievementHandler achievementHandler = UnityEngine.Object.FindObjectOfType<AchievementHandler>();
                    achievementHandler.AwardAchievementToPlayer(achievement.key);
                    parent.AwardIcon();
                }
            };
        }
    }
}
