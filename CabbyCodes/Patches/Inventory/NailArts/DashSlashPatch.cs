using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.NailArts
{
    public class DashSlashPatch : ISyncedReference<bool>
    {
        private static readonly FlagDef flag1 = FlagInstances.hasUpwardSlash;

        public bool Get()
        {
            return FlagManager.GetBoolFlag(flag1);
        }

        public void Set(bool value)
        {
            FlagManager.SetBoolFlag(flag1, value);
        }

        public static void AddPanel()
        {
            TogglePanel buttonPanel = new TogglePanel(new DashSlashPatch(), flag1.ReadableName);
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(buttonPanel);
        }
    }
}
