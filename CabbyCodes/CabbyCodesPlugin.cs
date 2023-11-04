using BepInEx;
using BepInEx.Unity.Mono;
using CabbyCodes.Patches;
using CabbyCodes.UI.CheatPanels;

namespace CabbyCodes
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class CabbyCodesPlugin : BaseUnityPlugin
    {
        const string GUID = "cabby.cabbycodes";
        const string NAME = "Cabby Codes";
        const string VERSION = "0.0.1";

        private CabbyMenu cabbyMenu;

        private void BuildPlayerCheats()
        {
            cabbyMenu.AddCheatPanel(new TogglePanel(CodeState.Get(InvulPatch.key), new InvulPatch(), "Invulnerability"));

            for (int i = 0; i < 20; i++)
            {
                CheatPanel cheatPanel;
                if (i % 2  == 0)
                    cheatPanel = new TogglePanel(new BoxedReference(false), () => { }, "Cheat " + i, 80);
                else
                    cheatPanel = new CheatPanel("Cheat " + i, 100);
                cabbyMenu.AddCheatPanel(cheatPanel);
            }
        }

        private void BuildMapCheats()
        {
            for (int i = 0; i < 2; i++)
            {
                CheatPanel cheatPanel;
                if (i % 2 == 0)
                    cheatPanel = new TogglePanel(new BoxedReference(false), () => { }, "Cheat " + i, 80);
                else
                    cheatPanel = new CheatPanel("Cheat " + i, 100);
                cabbyMenu.AddCheatPanel(cheatPanel);
            }
        }

        private void BuildHunterCheats()
        {
            for (int i = 0; i < 4; i++)
            {
                CheatPanel cheatPanel;
                if (i % 2 == 0)
                    cheatPanel = new CheatPanel("Cheat " + i, 80);
                else
                    cheatPanel = new TogglePanel(new BoxedReference(false), () => { }, "Cheat " + i, 100);
                cabbyMenu.AddCheatPanel(cheatPanel);
            }
        }

        private void Awake()
        {
            Logger.LogInfo("Plugin cabby.cabbycodes is loaded!");
        }

        private void Start()
        {
            UnityExplorer.ExplorerStandalone.CreateInstance();

            cabbyMenu = new CabbyMenu(NAME, VERSION);
            cabbyMenu.RegisterCategory("Player", BuildPlayerCheats);
            cabbyMenu.RegisterCategory("Map", BuildMapCheats);
            cabbyMenu.RegisterCategory("Hunter", BuildHunterCheats);

            new CodeState().Add(InvulPatch.key, new(false));
        }

        private void Update()
        {
            cabbyMenu.Update();
        }
    }
}