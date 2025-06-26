using CabbyMenu.SyncedReferences;

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
            return PlayerData.instance.GetBool("killed" + targetName);
        }

        public void Set(bool value)
        {
            PlayerData.instance.SetBool("killed" + targetName, value);
        }
    }
}
