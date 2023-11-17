using CabbyCodes.Patches.Inventory.NailArts;
using CabbyCodes.SyncedReferences;
using CabbyCodes.UI.CheatPanels;

namespace CabbyCodes.Patches.Inventory.Abilities
{
    public class IsmasTearPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return PlayerData.instance.hasAcidArmour;
        }

        public void Set(bool value)
        {
            PlayerData.instance.hasAcidArmour = value;
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new IsmasTearPatch(), "Isma's Tear"));
        }
    }
}
