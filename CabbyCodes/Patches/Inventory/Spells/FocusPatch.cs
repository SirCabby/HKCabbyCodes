using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;

namespace CabbyCodes.Patches.Inventory.Spells
{
    public class FocusPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return PlayerData.instance.hasSpell;
        }

        public void Set(bool value)
        {
            PlayerData.instance.hasSpell = value;
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new FocusPatch(), "Focus"));
        }
    }
}
