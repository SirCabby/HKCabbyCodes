using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Abilities
{
    public class MonarchWingsPatch : ISyncedReference<bool>
    {
        private static readonly FlagDef flag = FlagInstances.hasDoubleJump;

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
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new MonarchWingsPatch(), flag.ReadableName));
        }
    }
}
