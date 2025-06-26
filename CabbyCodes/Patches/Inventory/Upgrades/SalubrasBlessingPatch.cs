using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;

namespace CabbyCodes.Patches.Inventory.Upgrades
{
    public class SalubrasBlessingPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return PlayerData.instance.salubraBlessing;
        }

        public void Set(bool value)
        {
            PlayerData.instance.salubraBlessing = value;
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new SalubrasBlessingPatch(), "Salubra's Blessing"));
        }
    }
}
