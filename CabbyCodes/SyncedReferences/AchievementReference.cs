using CabbyCodes.UI.CheatPanels;

namespace CabbyCodes.SyncedReferences
{
    public class AchievementReference : ISyncedReference<bool>
    {
        private readonly Achievement achievement;
        private readonly AchievementPanel parent;

        public AchievementReference(Achievement achievement, AchievementPanel parent)
        {
            this.achievement = achievement;
            this.parent = parent;
        }

        public bool Get()
        {
            return GameManager.instance.IsAchievementAwarded(achievement.key);
        }

        public void Set(bool value)
        {
            if (value && !Get())
            {
                AchievementHandler achievementHandler = UnityEngine.Object.FindObjectOfType<AchievementHandler>();
                achievementHandler.AwardAchievementToPlayer(achievement.key);
                parent.AwardIcon();
            }
        }
    }
}
