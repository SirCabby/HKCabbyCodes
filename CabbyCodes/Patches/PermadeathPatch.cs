using CabbyCodes.SyncedReferences;

namespace CabbyCodes.Patches
{
    public class PermadeathPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return PlayerData.instance.permadeathMode == 1;
        }

        public void Set(bool value)
        {
            PlayerData.instance.permadeathMode = value ? 1 : 0;
        }
    }
}
