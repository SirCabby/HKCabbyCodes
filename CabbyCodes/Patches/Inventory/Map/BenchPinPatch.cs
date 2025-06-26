using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;

namespace CabbyCodes.Patches.Inventory.Map
{
    public class BenchPinPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return PlayerData.instance.hasPinBench;
        }

        public void Set(bool value)
        {
            PlayerData.instance.hasPinBench = value;
            if (value)
            {
                PlayerData.instance.hasPin = true;
            }
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new BenchPinPatch(), "Bench Pin"));
        }
    }
}
