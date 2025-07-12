using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Scenes;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Flags.NPC_Status
{
    public class MylaWaifuPatch : ISyncedReference<bool>
    {
        private static readonly string id = "Zombie Myla";
        private static readonly string sceneName = SceneInstances.Crossroads_45.SceneName;

        public bool Get()
        {
            // true = alive
            return !FlagManager.GetBoolFlag(id, sceneName);
        }

        public void Set(bool value)
        {
            bool wasAlive = Get();

            if (value && !wasAlive)
            {
                // Set to false (alive) when we want Myla to be alive
                FlagManager.SetBoolFlag(id, sceneName, false);
            }
            else if (!value && wasAlive)
            {
                // Set to true (zombie) when we want Myla to be dead
                FlagManager.SetBoolFlag(id, sceneName, true);
            }
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new MylaWaifuPatch(), "Myla Alive"));
        }
    }
}
