using CabbyCodes.SyncedReferences;
using CabbyCodes.UI.CheatPanels;

namespace CabbyCodes.Patches.Inventory.Map
{
    public class HotSpringPinPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return PlayerData.instance.hasPinSpa;
        }

        public void Set(bool value)
        {
            PlayerData.instance.hasPinSpa = value;
            if (value)
            {
                PlayerData.instance.hasPin = true;
            }
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new HotSpringPinPatch(), "Hot Spring Pin"));
        }
    }
}
