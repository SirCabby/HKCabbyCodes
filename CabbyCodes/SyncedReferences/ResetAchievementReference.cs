namespace CabbyCodes.SyncedReferences
{
    public class ResetAchievementReference : SyncedReference<bool>
    {
        public ResetAchievementReference()
        {
            Get = () =>
            {
                return false;
            };

            Set = (doReset) =>
            {
                AchievementHandler achievementHandler = UnityEngine.Object.FindObjectOfType<AchievementHandler>();
                achievementHandler.ResetAllAchievements();
            };
        }
    }
}
