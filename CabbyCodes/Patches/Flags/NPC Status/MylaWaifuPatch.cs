using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Flags.NPC_Status
{
    public class MylaWaifuPatch : ISyncedReference<bool>
    {
        private static readonly FlagDef flag = FlagInstances.Crossroads_45__Zombie_Myla;

        public bool Get()
        {
            return FlagManager.GetBoolFlag(flag);
        }

        public void Set(bool value)
        {
            FlagManager.SetBoolFlag(flag, value);
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new MylaWaifuPatch(), flag.ReadableName));
        }
    }
}
