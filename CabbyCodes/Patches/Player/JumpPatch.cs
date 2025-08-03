using CabbyMenu.SyncedReferences;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Player
{
    public class JumpPatch : ISyncedReference<bool>
    {
        private static readonly FlagDef flag = FlagInstances.infiniteAirJump;

        public bool Get()
        {
            return FlagManager.GetBoolFlag(flag);
        }

        public void Set(bool value)
        {
            FlagManager.SetBoolFlag(flag, value);
        }
    }
}
