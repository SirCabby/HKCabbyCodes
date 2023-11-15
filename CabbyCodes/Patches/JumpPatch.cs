using CabbyCodes.SyncedReferences;
using CabbyCodes.UI.CheatPanels;

namespace CabbyCodes.Patches
{
    public class JumpPatch : ISyncedReference<bool>
    {
        public const string key = "Soul_Patch";

        public bool Get()
        {
            return PlayerData.instance.infiniteAirJump;
        }

        public void Set(bool value)
        {
            PlayerData.instance.infiniteAirJump = value;
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new JumpPatch(), "Infinite Jumps"));
        }
    }
}
