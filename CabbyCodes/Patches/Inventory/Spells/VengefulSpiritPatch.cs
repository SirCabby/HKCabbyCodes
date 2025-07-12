using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.Utilities;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Spells
{
    public class VengefulSpiritPatch : ISyncedReference<int>
    {
        public int Get()
        {
            return FlagManager.GetIntFlag(FlagInstances.fireballLevel);
        }

        public void Set(int value)
        {
            FlagManager.SetIntFlag(FlagInstances.fireballLevel, value);
        }

        public static void AddPanel()
        {
            RangeInputFieldPanel<int> panel = new RangeInputFieldPanel<int>(new VengefulSpiritPatch(), KeyCodeMap.ValidChars.Decimal, 0, 2, "Vengeful Spirit / Shade Soul");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
