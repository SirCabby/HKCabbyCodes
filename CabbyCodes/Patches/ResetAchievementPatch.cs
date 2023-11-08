using CabbyCodes.SyncedReferences;

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
    }
}
