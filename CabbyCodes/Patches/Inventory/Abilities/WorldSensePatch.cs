using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Inventory.Abilities
{
    public class WorldSensePatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return FlagManager.GetBoolFlag(FlagInstances.unlockedCompletionRate);
        }

        public void Set(bool value)
        {
            FlagManager.SetBoolFlag(FlagInstances.unlockedCompletionRate, value);
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new WorldSensePatch(), "World Sense"));
        }
    }
}
