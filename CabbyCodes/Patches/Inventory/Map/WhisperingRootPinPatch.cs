using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;

namespace CabbyCodes.Patches.Inventory.Items
{
    public class WhisperingRootPinPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return PlayerData.instance.hasPinDreamPlant;
        }

        public void Set(bool value)
        {
            PlayerData.instance.hasPinDreamPlant = value;
            if (value)
            {
                PlayerData.instance.hasPin = true;
            }
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new WhisperingRootPinPatch(), "Whispering Root Pin"));
        }
    }
}
