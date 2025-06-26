using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;

namespace CabbyCodes.Patches.Inventory.Items
{
    public class GodtunerPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return PlayerData.instance.hasGodfinder;
        }

        public void Set(bool value)
        {
            PlayerData.instance.hasGodfinder = value;
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new GodtunerPatch(), "Godtuner"));
        }
    }
}
