using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;

namespace CabbyCodes.Patches.Inventory.Map
{
    public class TramPinPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return PlayerData.instance.hasPinTram;
        }

        public void Set(bool value)
        {
            PlayerData.instance.hasPinTram = value;
            if (value)
            {
                PlayerData.instance.hasPin = true;
            }
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new TramPinPatch(), "Tram Pin"));
        }
    }
}
