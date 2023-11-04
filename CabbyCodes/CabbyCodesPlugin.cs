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
        }

        private void BuildAchievementCheats()
        {
            AchievementHandler achievementHandler = FindObjectOfType<AchievementHandler>();
            cabbyMenu.AddCheatPanel(new InfoPanel("Warning: Changes are immediate and irreversible"));
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
            cabbyMenu.RegisterCategory("Achievements", BuildAchievementCheats);

            new CodeState().Add(InvulPatch.key, new(false));
        }

        private void Update()
        {
            cabbyMenu.Update();
        }
    }
}