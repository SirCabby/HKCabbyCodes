using CabbyCodes.SyncedReferences;
using CabbyCodes.UI.CheatPanels;
using System;

namespace CabbyCodes.Patches.Inventory.Upgrades
{
    public class MaskShardPatch : ISyncedReference<int>
    {
        public int Get()
        {
            return PlayerData.instance.heartPieces;
        }

        public void Set(int value)
        {
            value = Math.Max(0, value);
            value = Math.Min(3, value);
            PlayerData.instance.heartPieces = value;
        }

        public static void AddPanel()
        {
            InputFieldPanel<int> panel = new(new MaskShardPatch(), KeyCodeMap.ValidChars.Numeric, 1, 120, "Mask Shards (0-3)");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
