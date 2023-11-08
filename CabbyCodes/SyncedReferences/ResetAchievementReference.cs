namespace CabbyCodes.SyncedReferences
{
    public class ResetAchievementReference : ISyncedReference<bool>
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
    }
}
