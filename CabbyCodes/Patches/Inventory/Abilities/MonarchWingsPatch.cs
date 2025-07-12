using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Abilities
{
    public class MonarchWingsPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return FlagManager.GetBoolFlag(FlagInstances.hasDoubleJump);
        }

        public void Set(bool value)
        {
            FlagManager.SetBoolFlag(FlagInstances.hasDoubleJump, value);
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new MonarchWingsPatch(), "Monarch Wings"));
        }
    }
}
