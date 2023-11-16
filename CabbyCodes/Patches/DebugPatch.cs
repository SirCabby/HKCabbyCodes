using CabbyCodes.UI.CheatPanels;
using UnityEngine;
using CabbyCodes.Debug;

namespace CabbyCodes.Patches
{
    public class DebugPatch
    {
        public static void AddPanels()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Debug Utilities: Prints information to BepInEx console").SetColor(CheatPanel.headerColor));
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new ButtonPanel(() =>
            {
                GameMap gm = GameManager._instance.gameMap.GetComponent<GameMap>();
                Vector3 heroPos = ((GameObject)GameData.heroFieldInfo.GetValue(gm)).transform.position;
                CabbyCodesPlugin.BLogger.LogInfo("Location: " + heroPos.x + ", " + heroPos.y);
                CabbyCodesPlugin.BLogger.LogInfo("Scene: " + GameManager.GetBaseSceneName(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name));
            }, "Print", "General Info"));
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new ButtonPanel(() =>
            {
                ObjectPrint.DisplayObjectInfo(GameManager._instance);
            }, "Print", "GameManager"));
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new ButtonPanel(() =>
            {
                ObjectPrint.DisplayObjectInfo(GameManager._instance.gameMap.GetComponent<GameMap>());
            }, "Print", "GameMap"));
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new ButtonPanel(() =>
            {
                ObjectPrint.DisplayObjectInfo(UIManager.instance);
            }, "Print", "UIManager"));
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new ButtonPanel(() =>
            {
                ObjectPrint.DisplayObjectInfo(GameManager._instance.playerData);
            }, "Print", "PlayerData"));
        }
    }
}
