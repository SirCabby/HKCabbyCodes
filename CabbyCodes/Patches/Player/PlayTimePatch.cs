using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.Utilities;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Player
{
    public class PlayTimePatch : ISyncedReference<float>
    {
        public float Get()
        {
            return FlagManager.GetFloatFlag("playTime", "Global");
        }

        public void Set(float value)
        {
            FlagManager.SetFloatFlag("playTime", "Global", value);
        }
    }
}
