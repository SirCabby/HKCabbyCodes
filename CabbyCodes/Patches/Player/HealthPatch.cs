using CabbyMenu.SyncedReferences;
using CabbyMenu.Utilities;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Player
{
    public class HealthPatch : ISyncedReference<int>
    {
        private static readonly FlagDef flag1 = FlagInstances.maxHealthBase;
        private static readonly FlagDef flag2 = FlagInstances.maxHealth;

        public int Get()
        {
            return FlagManager.GetIntFlag(flag1);
        }

        public void Set(int value)
        {
            value = ValidationUtils.ValidateRange(value, Constants.MIN_HEALTH, Constants.MAX_HEALTH, nameof(value));

            FlagManager.SetIntFlag(flag1, value);
            FlagManager.SetIntFlag(flag2, value);

            CabbyCodesPlugin.BLogger.LogDebug(string.Format("Health updated to {0}", value));
        }
    }
}
