using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using System.Collections.Generic;
using UnityEngine;
using static CabbyCodes.Scenes.SceneManagement;
using static CabbyCodes.Scenes.Areas;

namespace CabbyCodes.Patches.Maps
{
    public class MapRoomPatch : ISyncedReference<bool>
    {
        public static readonly Dictionary<string, List<string>> roomsInMaps = GetAreaToScenesMapping();
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
            // Add area selector dropdown
            MapAreaSelector areaSelector = new MapAreaSelector();
            DropdownPanel areaDropdownPanel = new DropdownPanel(areaSelector, "Select Area", Constants.DEFAULT_PANEL_HEIGHT);
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(areaDropdownPanel);

            // Add dynamic room manager
            MapRoomPanelManager roomManager = new MapRoomPanelManager(areaSelector);
            roomManager.AddAllPanelsToMenu();

            // Add update action to handle area changes (for menu update loop)
            areaDropdownPanel.updateActions.Add(roomManager.UpdateVisibleArea);
            // Hook dropdown value change event for immediate update
            areaDropdownPanel.GetDropDownSync().GetCustomDropdown().onValueChanged.AddListener(_ => roomManager.UpdateVisibleArea());
        }


    }
}
