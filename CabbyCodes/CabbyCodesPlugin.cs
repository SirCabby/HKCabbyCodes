using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.Mono;
using CabbyCodes.Patches;
using CabbyCodes.SyncedReferences;
using CabbyCodes.UI.CheatPanels;

namespace CabbyCodes
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class CabbyCodesPlugin : BaseUnityPlugin
    {
        const string GUID = "cabby.cabbycodes";
        const string NAME = "Cabby Codes";
        const string VERSION = "0.0.1";

        public static ManualLogSource BLogger;

        private CabbyMenu cabbyMenu;

        private void BuildPlayerCheats()
        {
            cabbyMenu.AddCheatPanel(new InfoPanel("Player Codes").SetColor(CheatPanel.headerColor));
            cabbyMenu.AddCheatPanel(new TogglePanel(new InvulPatch(), "Invulnerability"));
        }

        private void BuildAchievementCheats()
        {
            cabbyMenu.AddCheatPanel(new InfoPanel("Toggle Achievements <ON> to unlock in-game and in Online platform (Steam / GOG)").SetColor(CheatPanel.headerColor));
            cabbyMenu.AddCheatPanel(new InfoPanel("Warning: Unable to toggle off achievements independently").SetColor(CheatPanel.warningColor));
            cabbyMenu.AddCheatPanel(new TogglePanel(new ResetAchievementReference(), "Reset <ALL> Achievements to off. Requires Restart."));

            AchievementHandler achievementHandler = FindObjectOfType<AchievementHandler>();
            foreach (Achievement achievement in achievementHandler.achievementsList.achievements)
            {
                cabbyMenu.AddCheatPanel(new AchievementPanel(achievement));
            }
        }

        private void Awake()
        {
            Logger.LogInfo("Plugin cabby.cabbycodes is loaded!");
            BLogger = Logger;
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