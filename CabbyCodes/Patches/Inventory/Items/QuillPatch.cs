using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;

namespace CabbyCodes.Patches.Inventory.Items
{
    public class QuillPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return PlayerData.instance.hasQuill;
        }

        public void Set(bool value)
        {
            PlayerData.instance.hasQuill = value;
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new QuillPatch(), "Quill"));
        }
    }
}
