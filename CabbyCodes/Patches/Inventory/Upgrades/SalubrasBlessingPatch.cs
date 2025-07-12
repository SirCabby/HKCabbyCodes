using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Upgrades
{
    public class SalubrasBlessingPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return FlagManager.GetBoolFlag(FlagInstances.salubraBlessing);
        }

        public void Set(bool value)
        {
            FlagManager.SetBoolFlag(FlagInstances.salubraBlessing, value);
        }

        public static void AddPanel()
        {
            TogglePanel buttonPanel = new TogglePanel(new SalubrasBlessingPatch(), "Salubra's Blessing");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(buttonPanel);
        }
    }
}
