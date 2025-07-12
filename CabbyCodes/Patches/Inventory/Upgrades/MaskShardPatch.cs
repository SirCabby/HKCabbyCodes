using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.Utilities;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Upgrades
{
    public class MaskShardPatch : ISyncedReference<int>
    {
        public int Get()
        {
            return FlagManager.GetIntFlag(FlagInstances.heartPieces);
        }

        public void Set(int value)
        {
            FlagManager.SetIntFlag(FlagInstances.heartPieces, value);
        }

        public static void AddPanel()
        {
            RangeInputFieldPanel<int> panel = new RangeInputFieldPanel<int>(new MaskShardPatch(), KeyCodeMap.ValidChars.Decimal, 0, 4, "Mask Shards (0-4)");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
