using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;

namespace CabbyCodes.Patches.Inventory.Map
{
    public class StagStationPinPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return PlayerData.instance.hasPinStag;
        }

        public void Set(bool value)
        {
            PlayerData.instance.hasPinStag = value;
            if (value)
            {
                PlayerData.instance.hasPin = true;
            }
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new StagStationPinPatch(), "Stag Station Pin"));
        }
    }
}
