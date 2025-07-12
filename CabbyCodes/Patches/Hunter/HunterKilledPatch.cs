using CabbyMenu.SyncedReferences;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Hunter
{
    public class HunterKilledPatch : ISyncedReference<bool>
    {
        private readonly string targetName;

        public HunterKilledPatch(string targetName)
        {
            this.targetName = targetName;
        }

        public bool Get()
        {
            return FlagManager.GetBoolFlag("killed" + targetName, "Global");
        }

        public void Set(bool value)
        {
            FlagManager.SetBoolFlag("killed" + targetName, "Global", value);
        }
    }
}
