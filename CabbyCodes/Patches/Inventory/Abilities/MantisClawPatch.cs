using CabbyCodes.SyncedReferences;
using CabbyCodes.UI.CheatPanels;

namespace CabbyCodes.Patches.Inventory.Abilities
{
    public class MantisClawPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return PlayerData.instance.hasWalljump;
        }

        public void Set(bool value)
        {
            PlayerData.instance.hasWalljump = value;
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new MantisClawPatch(), "Mantis Claw"));
        }
    }
}
