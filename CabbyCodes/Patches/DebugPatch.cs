using CabbyCodes.UI.CheatPanels;
using UnityEngine;
using CabbyCodes.Debug;
using System.Reflection;
using System.Collections.Generic;

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
                Dictionary<string, List<string>> sceneValues = new();

                // Build bools
                foreach (PersistentBoolData pbd in SceneData.instance.persistentBoolItems)
                {
                    if (!sceneValues.ContainsKey(pbd.sceneName))
                    {
                        sceneValues.Add(pbd.sceneName, new());
                    }

                    sceneValues[pbd.sceneName].Add(pbd.id + " - " + pbd.activated);
                }

                // Build ints
                foreach (PersistentIntData pid in SceneData.instance.persistentIntItems)
                {
                    if (!sceneValues.ContainsKey(pid.sceneName))
                    {
                        sceneValues.Add(pid.sceneName, new());
                    }

                    sceneValues[pid.sceneName].Add(pid.id + " - " + pid.value);
                }

                // Print
                foreach (KeyValuePair<string, List<string>> kvp in sceneValues)
                {
                    CabbyCodesPlugin.BLogger.LogInfo("    Scene: " + kvp.Key);

                    foreach (string value in kvp.Value)
                    {
                        CabbyCodesPlugin.BLogger.LogInfo("        " + value);
                    }
                }
            }, "Print", "SceneData"));
        }
    }
}
