using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;

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
            InputFieldPanel<int> panel = new InputFieldPanel<int>(new MaskShardPatch(), CabbyMenu.KeyCodeMap.ValidChars.Numeric, 1, "Mask Shards (0-4)");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
