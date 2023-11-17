using CabbyCodes.UI.CheatPanels;
using UnityEngine;
using CabbyCodes.Debug;
using System.Reflection;

namespace CabbyCodes.Patches
{
    public class DebugPatch
    {
        private static readonly FieldInfo heroFieldInfo = typeof(GameMap).GetField("hero", BindingFlags.NonPublic | BindingFlags.Instance);

        public static void AddPanels()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Debug Utilities: Prints information to BepInEx console").SetColor(CheatPanel.headerColor));

            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new ButtonPanel(() =>
            {
                GameMap gm = GameManager._instance.gameMap.GetComponent<GameMap>();
                Vector3 heroPos = ((GameObject)heroFieldInfo.GetValue(gm)).transform.position;
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

            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new ButtonPanel(() =>
            {
                ObjectPrint.DisplayObjectInfo(CharmIconList.Instance);
                CabbyCodesPlugin.BLogger.LogInfo("Sprite list:");
                foreach (Sprite sprite in CharmIconList.Instance.spriteList)
                {
                    CabbyCodesPlugin.BLogger.LogInfo("    " + sprite.name);
                }
            }, "Print", "CharmIconList"));

            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new ButtonPanel(() =>
            {
                CabbyCodesPlugin.BLogger.LogInfo("SceneData:");
                foreach (PersistentBoolData pbd in SceneData.instance.persistentBoolItems)
                {
                    ObjectPrint.DisplayObjectInfo(pbd);
                }
                foreach (PersistentIntData pbd in SceneData.instance.persistentIntItems)
                {
                    ObjectPrint.DisplayObjectInfo(pbd);
                }
            }, "Print", "SceneData"));
        }
    }
}
