using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Items
{
    public class HuntersMarkPatch : ISyncedReference<bool>
    {
        private static readonly FlagDef flag = FlagInstances.hasHuntersMark;

        public bool Get()
        {
            return FlagManager.GetBoolFlag(flag);
        }

        public void Set(bool value)
        {
            FlagManager.SetBoolFlag(flag, value);
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new HuntersMarkPatch(), flag.ReadableName));
        }
    }
}
