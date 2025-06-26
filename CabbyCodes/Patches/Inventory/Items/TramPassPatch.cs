using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;

namespace CabbyCodes.Patches.Inventory.Items
{
    public class TramPassPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return PlayerData.instance.hasTramPass;
        }

        public void Set(bool value)
        {
            PlayerData.instance.hasTramPass = value;
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new TramPassPatch(), "Tram Pass"));
        }
    }
}
