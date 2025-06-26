using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;

namespace CabbyCodes.Patches.Inventory.Abilities
{
    public class WorldSensePatch : ISyncedReference<bool>
    {
        public bool Get()
        {
            return PlayerData.instance.unlockedCompletionRate;
        }

        public void Set(bool value)
        {
            PlayerData.instance.unlockedCompletionRate = value;
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new WorldSensePatch(), "World Sense"));
        }
    }
}
