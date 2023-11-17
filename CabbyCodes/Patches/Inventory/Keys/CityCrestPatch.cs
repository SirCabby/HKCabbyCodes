using CabbyCodes.SyncedReferences;
using CabbyCodes.UI.CheatPanels;

namespace CabbyCodes.Patches.Inventory.Keys
{
    public class CityCrestPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return PlayerData.instance.hasCityKey;
        }

        public void Set(bool value)
        {
            PlayerData.instance.hasCityKey = value;
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new CityCrestPatch(), "City Crest"));
        }
    }
}
