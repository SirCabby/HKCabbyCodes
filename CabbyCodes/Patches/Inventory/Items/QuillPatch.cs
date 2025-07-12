using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Items
{
    public class QuillPatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return FlagManager.GetBoolFlag(FlagInstances.hasQuill);
        }

        public void Set(bool value)
        {
            FlagManager.SetBoolFlag(FlagInstances.hasQuill, value);
        }

        public static void AddPanel()
        {
            TogglePanel buttonPanel = new TogglePanel(new QuillPatch(), "Quill");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(buttonPanel);
        }
    }
}
