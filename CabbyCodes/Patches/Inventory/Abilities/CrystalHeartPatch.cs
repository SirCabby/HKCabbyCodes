using CabbyCodes.SyncedReferences;
using CabbyCodes.UI.CheatPanels;

namespace CabbyCodes.Patches.Inventory.Abilities
{
    public class CrystalHeartPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return PlayerData.instance.hasSuperDash;
        }

        public void Set(bool value)
        {
            PlayerData.instance.hasSuperDash = value;
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new CrystalHeartPatch(), "Crystal Heart"));
        }
    }
}
