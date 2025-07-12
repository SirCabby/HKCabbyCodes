using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Abilities
{
    public class IsmasTearPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return FlagManager.GetBoolFlag(FlagInstances.hasAcidArmour);
        }

        public void Set(bool value)
        {
            FlagManager.SetBoolFlag(FlagInstances.hasAcidArmour, value);
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new IsmasTearPatch(), "Isma's Tear"));
        }
    }
}
