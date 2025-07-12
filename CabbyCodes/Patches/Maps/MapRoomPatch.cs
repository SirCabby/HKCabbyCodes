using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using System.Collections.Generic;
using UnityEngine;
using CabbyCodes.Scenes;
using static CabbyCodes.Scenes.Scenes;
using static CabbyCodes.Scenes.Areas;

namespace CabbyCodes.Patches.Maps
{
    public class MapRoomPatch : ISyncedReference<bool>
    {
        public static readonly Dictionary<string, List<string>> roomsInMaps = BuildMapRoomDictFromScenes();
        private static readonly Vector2 buttonSize = new Vector2(120, 60);

        private readonly string roomName;

        public MapRoomPatch(string roomName)
        {
            this.roomName = roomName;
        }

        public bool Get()
        {
            return PlayerData.instance.scenesMapped.Contains(roomName);
        }

        public void Set(bool value)
        {
            if (value && !Get())
            {
                PlayerData.instance.scenesMapped.Add(roomName);
            }
            else if (!value && Get())
            {
                PlayerData.instance.scenesMapped.Remove(roomName);
            }
        }

        private static void ToggleAllRooms(string mapName, bool setToOn)
        {
            List<string> scenesMapped = PlayerData.instance.scenesMapped;

            foreach (string roomName in roomsInMaps[mapName])
            {
                if (setToOn && !scenesMapped.Contains(roomName))
                {
                    scenesMapped.Add(roomName);
                }
                else if (!setToOn && scenesMapped.Contains(roomName))
                {
                    scenesMapped.Remove(roomName);
                }
            }

            CabbyCodesPlugin.cabbyMenu.UpdateCheatPanels();
        }

        public static void AddPanels()
        {
            foreach (KeyValuePair<string, List<string>> kvp in roomsInMaps)
            {
                // Get readable area name
                var areaData = GetAreaData(kvp.Key);
                string readableAreaName = areaData?.ReadableName ?? kvp.Key;
                
                CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Area: " + readableAreaName).SetColor(CheatPanel.subHeaderColor));
                ButtonPanel toggleAllPanel = new ButtonPanel(() => ToggleAllRooms(kvp.Key, true), "ON", "Toggle All Rooms");
                PanelAdder.AddButton(toggleAllPanel, 1, () => ToggleAllRooms(kvp.Key, false), "OFF", buttonSize);
                CabbyCodesPlugin.cabbyMenu.AddCheatPanel(toggleAllPanel);

                foreach (string roomName in kvp.Value)
                {
                    CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new MapRoomPatch(roomName), roomName));
                }
            }
        }

        private static Dictionary<string, List<string>> BuildMapRoomDictFromScenes()
        {
            var roomsInMaps = new Dictionary<string, List<string>>();
            
            // Get all scene data and group by area
            var allSceneData = GetAllSceneData();
            
            foreach (var sceneData in allSceneData)
            {
                string areaName = sceneData.AreaName;
                string sceneName = sceneData.SceneName;
                
                if (!roomsInMaps.ContainsKey(areaName))
                {
                    roomsInMaps[areaName] = new List<string>();
                }
                
                roomsInMaps[areaName].Add(sceneName);
            }
            
            return roomsInMaps;
        }
    }
}
