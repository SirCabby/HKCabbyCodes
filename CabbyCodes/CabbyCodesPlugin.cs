using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.Mono;
using CabbyCodes.Debug;
using CabbyCodes.Patches;
using CabbyCodes.Types;
using CabbyCodes.UI;
using CabbyCodes.UI.CheatPanels;
using UnityEngine;

namespace CabbyCodes
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class CabbyCodesPlugin : BaseUnityPlugin
    {
        const string GUID = "cabby.cabbycodes";
        const string NAME = "Cabby Codes";
        const string VERSION = "0.0.1";

        public static ManualLogSource BLogger;
        public static CabbyMenu cabbyMenu;
        public static ConfigFile configFile;

        private void BuildPlayerCheats()
        {
            cabbyMenu.AddCheatPanel(new InfoPanel("Player Codes").SetColor(CheatPanel.headerColor));
            cabbyMenu.AddCheatPanel(new TogglePanel(new InvulPatch(), "Invulnerability"));
            cabbyMenu.AddCheatPanel(new TogglePanel(new SoulPatch(), "Infinite Soul"));
            cabbyMenu.AddCheatPanel(new TogglePanel(new GeoPatch(), "Infinite Geo"));
            cabbyMenu.AddCheatPanel(new TogglePanel(new PermadeathPatch(), "Permadeath Mode"));
        }

        private void BuildTeleportCheats()
        {
            cabbyMenu.AddCheatPanel(new InfoPanel("Teleportation: Select common point of interest to travel to").SetColor(CheatPanel.headerColor));
            cabbyMenu.AddCheatPanel(new InfoPanel("Warning: Teleporting requires a pause / unpause to complete").SetColor(CheatPanel.warningColor));
            cabbyMenu.AddCheatPanel(new DropdownPanel(new TeleportPatch(), "Select Area to Teleport"));
            cabbyMenu.AddCheatPanel(new InfoPanel("Lloyd's Beacon: Save and recall custom teleportation locations").SetColor(CheatPanel.headerColor));
            cabbyMenu.AddCheatPanel(new ButtonPanel(TeleportPatch.SaveTeleportLocation, "Save", "Save a custom teleport at current position"));
            foreach (TeleportLocation location in TeleportPatch.savedTeleports)
            {
                TeleportPatch.AddTeleportPanel(location);
            }
        }

        private void BuildMapCheats()
        {
            cabbyMenu.AddCheatPanel(new InfoPanel("Maps: Enable to have map for area").SetColor(CheatPanel.headerColor));
            cabbyMenu.AddCheatPanel(new InfoPanel("Warning: Still requires Quill item to fill maps out").SetColor(CheatPanel.warningColor));
            MapPatch.BuildMapPanels();
        }

        private void BuildAchievementCheats()
        {
            cabbyMenu.AddCheatPanel(new InfoPanel("Toggle Achievements <ON> to unlock in-game and in Online platform (Steam / GOG)").SetColor(CheatPanel.headerColor));
            cabbyMenu.AddCheatPanel(new InfoPanel("Warning: Unable to toggle off achievements independently").SetColor(CheatPanel.warningColor));
            cabbyMenu.AddCheatPanel(new TogglePanel(new ResetAchievementPatch(), "Reset <ALL> Achievements to off. Requires Restart."));

            AchievementHandler achievementHandler = FindObjectOfType<AchievementHandler>();
            foreach (Achievement achievement in achievementHandler.achievementsList.achievements)
            {
                cabbyMenu.AddCheatPanel(new AchievementPanel(achievement));
            }
        }

        private void BuildDebugCheats()
        {
            cabbyMenu.AddCheatPanel(new InfoPanel("Debug Utilities: Prints information to BepInEx console").SetColor(CheatPanel.headerColor));
            cabbyMenu.AddCheatPanel(new ButtonPanel(() => 
            {
                GameMap gm = GameManager._instance.gameMap.GetComponent<GameMap>();
                Vector3 heroPos = ((GameObject)GameData.heroFieldInfo.GetValue(gm)).transform.position;
                Logger.LogInfo("Location: " + heroPos.x + ", " + heroPos.y);
                Logger.LogInfo("Scene: " + GameManager.GetBaseSceneName(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name));
            }, "Print", "General Info"));
            cabbyMenu.AddCheatPanel(new ButtonPanel(() =>
            {
                ObjectPrint.DisplayObjectInfo(GameManager._instance);
            }, "Print", "GameManager"));
            cabbyMenu.AddCheatPanel(new ButtonPanel(() =>
            {
                ObjectPrint.DisplayObjectInfo(GameManager._instance.gameMap.GetComponent<GameMap>());
            }, "Print", "GameMap"));
            cabbyMenu.AddCheatPanel(new ButtonPanel(() =>
            {
                ObjectPrint.DisplayObjectInfo(UIManager.instance);
            }, "Print", "UIManager"));
            cabbyMenu.AddCheatPanel(new ButtonPanel(() =>
            {
                ObjectPrint.DisplayObjectInfo(GameManager._instance.playerData);
            }, "Print", "PlayerData"));
        }

        private void Awake()
        {
            Logger.LogInfo("Plugin cabby.cabbycodes is loaded!");
            BLogger = Logger;
            Logger.LogInfo("config location: " + Config.ConfigFilePath);
            configFile = Config;
        }

        private void Start()
        {
            UnityExplorer.ExplorerStandalone.CreateInstance();

            cabbyMenu = new CabbyMenu(NAME, VERSION);
            cabbyMenu.RegisterCategory("Player", BuildPlayerCheats);
            cabbyMenu.RegisterCategory("Teleport", BuildTeleportCheats);
            cabbyMenu.RegisterCategory("Maps", BuildMapCheats);
            cabbyMenu.RegisterCategory("Achievements", BuildAchievementCheats);
            cabbyMenu.RegisterCategory("Debug", BuildDebugCheats);
        }

        private void Update()
        {
            cabbyMenu.Update();
        }
    }
}