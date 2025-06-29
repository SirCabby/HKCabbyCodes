using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu;

namespace CabbyCodes.Patches.Flags.NPC_Status
{
    public class MylaWaifuPatch : ISyncedReference<bool>
    {
        private static readonly string id = "Zombie Myla";
        private static readonly string sceneName = "Crossroads_45";

        public bool Get()
        {
            // true = alive
            return !PbdMaker.GetPbd(id, sceneName).activated;
        }

        public void Set(bool value)
        {
            bool wasAlive = Get();

            if (value && !wasAlive)
            {
                PersistentBoolData pbd = PbdMaker.GetPbd(id, sceneName);
                pbd.activated = false;
                SceneData.instance.SaveMyState(pbd);
            }
            else if (!value && wasAlive)
            {
                PersistentBoolData pbd = PbdMaker.GetPbd(id, sceneName);
                pbd.activated = true;
                SceneData.instance.SaveMyState(pbd);
            }
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new MylaWaifuPatch(), "Myla Alive"));
        }
    }
}
