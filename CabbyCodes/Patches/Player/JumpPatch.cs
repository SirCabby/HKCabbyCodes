using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Player
{
    public class JumpPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return FlagManager.GetBoolFlag(FlagInstances.infiniteAirJump);
        }

        public void Set(bool value)
        {
            FlagManager.SetBoolFlag(FlagInstances.infiniteAirJump, value);
        }

        public static void AddPanel()
        {
            TogglePanel buttonPanel = new TogglePanel(new JumpPatch(), "Infinite Air Jump");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(buttonPanel);
        }
    }
}
