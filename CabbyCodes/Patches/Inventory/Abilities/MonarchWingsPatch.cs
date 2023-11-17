using CabbyCodes.SyncedReferences;
using CabbyCodes.UI.CheatPanels;

namespace CabbyCodes.Patches.Inventory.Abilities
{
    public class MonarchWingsPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return PlayerData.instance.hasDoubleJump;
        }

        public void Set(bool value)
        {
            PlayerData.instance.hasDoubleJump = value;
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new MonarchWingsPatch(), "Monarch Wings"));
        }
    }
}
