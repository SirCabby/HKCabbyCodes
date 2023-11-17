using CabbyCodes.SyncedReferences;
using CabbyCodes.UI.CheatPanels;

namespace CabbyCodes.Patches.Inventory.Abilities
{
    public class DreamgatePatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return PlayerData.instance.hasDreamGate;
        }

        public void Set(bool value)
        {
            PlayerData.instance.hasDreamGate = value;
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new DreamgatePatch(), "Dreamgate"));
        }
    }
}
