using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
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
            value = CabbyMenu.ValidationUtils.ValidateRange(value, 0, Constants.MAX_MASK_SHARDS, nameof(value));
            PlayerData.instance.heartPieces = value;
        }

        public static void AddPanel()
        {
            InputFieldPanel<int> panel = new(new MaskShardPatch(), CabbyMenu.KeyCodeMap.ValidChars.Numeric, 1, 120, "Mask Shards (0-3)");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
