using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Spells
{
    public class FocusPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return FlagManager.GetBoolFlag(FlagInstances.hasSpell);
        }

        public void Set(bool value)
        {
            FlagManager.SetBoolFlag(FlagInstances.hasSpell, value);
        }

        public static void AddPanel()
        {
            TogglePanel buttonPanel = new TogglePanel(new FocusPatch(), "Focus");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(buttonPanel);
        }
    }
}
