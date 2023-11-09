using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.Mono;
using CabbyCodes.Debug;
using CabbyCodes.Patches;
using CabbyCodes.Structs;
using CabbyCodes.UI;
using CabbyCodes.UI.CheatPanels;
using System.Collections.Generic;
using System.Reflection;
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

        private void BuildPlayerCheats()
        {
            cabbyMenu.AddCheatPanel(new InfoPanel("Player Codes").SetColor(CheatPanel.headerColor));
            cabbyMenu.AddCheatPanel(new TogglePanel(new InvulPatch(), "Invulnerability"));
        }

        private void BuildTeleportCheats()
        {
            cabbyMenu.AddCheatPanel(new InfoPanel("Teleportation").SetColor(CheatPanel.headerColor));
            cabbyMenu.AddCheatPanel(new InfoPanel("Warning: Teleporting requires a pause / unpause to complete").SetColor(CheatPanel.warningColor));
            cabbyMenu.AddCheatPanel(new DropdownPanel(new TeleportPatch(), "Select Area to Teleport"));
            cabbyMenu.AddCheatPanel(new InfoPanel("Lloyd's Beacon: Save and recall teleportation locations").SetColor(CheatPanel.headerColor));
            cabbyMenu.AddCheatPanel(new ButtonPanel(TeleportPatch.SaveTeleportLocation, "Save", "Save a custom teleport at current position"));
            foreach (TeleportLocation location in (List<TeleportLocation>)TeleportPatch.savedTeleports.Get())
            {
                TeleportPatch.AddCustomTeleport(location);
            }
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
        }

        private void Start()
        {
            UnityExplorer.ExplorerStandalone.CreateInstance();

            cabbyMenu = new CabbyMenu(NAME, VERSION);
            cabbyMenu.RegisterCategory("Player", BuildPlayerCheats);
            cabbyMenu.RegisterCategory("Teleport", BuildTeleportCheats);
            cabbyMenu.RegisterCategory("Achievements", BuildAchievementCheats);
            cabbyMenu.RegisterCategory("Debug", BuildDebugCheats);

            new CodeState().Add(InvulPatch.key, new(false));
        }

        private void Update()
        {
            cabbyMenu.Update();
        }
    }
}