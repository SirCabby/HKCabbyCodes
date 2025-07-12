using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.Utilities;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Upgrades
{
    public class NailPatch : ISyncedReference<int>
    {
        public int Get()
        {
            return FlagManager.GetIntFlag(FlagInstances.nailSmithUpgrades);
        }

        public void Set(int value)
        {
            FlagManager.SetIntFlag(FlagInstances.nailSmithUpgrades, value);
        }

        public static void AddPanel()
        {
            RangeInputFieldPanel<int> panel = new RangeInputFieldPanel<int>(new NailPatch(), KeyCodeMap.ValidChars.Decimal, 0, 4, "Nail Upgrades (0-4)");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
