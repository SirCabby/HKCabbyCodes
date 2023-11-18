using CabbyCodes.SyncedReferences;
using CabbyCodes.UI.CheatPanels;

namespace CabbyCodes.Patches.Flags.NPC_Status
{
    public class MylaWaifuPatch : ISyncedReference<bool>
    {
        private static readonly string id = "Zombie Myla";
        private static readonly string sceneName = "Crossroads_45";

        public bool Get()
        {
            // true = alive
            return !MakePbi().activated;
        }

        public void Set(bool value)
        {
            bool wasAlive = Get();

            if (value && !wasAlive)
            {
                PersistentBoolData pbd = MakePbi();
                pbd.activated = false;
                SceneData.instance.SaveMyState(pbd);
            }
            else if (!value && wasAlive)
            {
                PersistentBoolData pbd = MakePbi();
                pbd.activated = true;
                SceneData.instance.SaveMyState(pbd);
            }
        }

        private PersistentBoolData MakePbi()
        {
            PersistentBoolData pbd = new()
            {
                id = id,
                sceneName = sceneName,
                activated = false,
                semiPersistent = false
            };

            PersistentBoolData result = SceneData.instance.FindMyState(pbd);
            if (result == null)
            {
                SceneData.instance.SaveMyState(pbd);
                result = SceneData.instance.FindMyState(pbd);
            }

            return result;
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new MylaWaifuPatch(), "Myla Alive"));
        }
    }
}
