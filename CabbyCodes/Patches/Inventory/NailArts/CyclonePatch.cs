using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;

namespace CabbyCodes.Patches.Inventory.NailArts
{
    public class CyclonePatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return PlayerData.instance.hasCyclone;
        }

        public void Set(bool value)
        {
            PlayerData.instance.hasCyclone = value;
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new CyclonePatch(), "Cyclone Slash"));
        }
    }
}
