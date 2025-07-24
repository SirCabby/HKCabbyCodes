using CabbyMenu.UI.CheatPanels;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CabbyCodes.Scenes;
using static CabbyCodes.Scenes.SceneManagement;
using CabbyCodes.Flags;
using CabbyCodes.SavedGames;

namespace CabbyCodes.Patches.Maps
{
    public class MapRoomPanelManager
    {
        private static readonly Vector2 buttonSize = new Vector2(120, 60);
        private readonly MapAreaSelector areaSelector;
        private readonly Dictionary<string, List<string>> areaRooms = new Dictionary<string, List<string>>();
        private readonly List<CheatPanel> currentlyAddedPanels = new List<CheatPanel>();

        public MapRoomPanelManager(MapAreaSelector areaSelector)
        {
            this.areaSelector = areaSelector;
            InitializeAreaRooms();
        }

        private void InitializeAreaRooms()
        {
            // Store just the room names for each area, not the panels
            foreach (KeyValuePair<string, List<string>> kvp in MapRoomPatch.roomsInMaps)
            {
                areaRooms[kvp.Key] = new List<string>(kvp.Value);
            }
        }

        private List<CheatPanel> CreatePanelsForArea(string areaName)
        {
            List<CheatPanel> panels = new List<CheatPanel>();
            
            if (!areaRooms.ContainsKey(areaName))
                return panels;

            List<string> roomNames = areaRooms[areaName];

            // Add toggle all panel for this area
            ButtonPanel toggleAllPanel = new ButtonPanel(() => ToggleAllRooms(areaName, true), "ON", "Toggle All Rooms");
            PanelAdder.AddButton(toggleAllPanel, 1, () => ToggleAllRooms(areaName, false), "OFF", buttonSize);
            panels.Add(toggleAllPanel);

            // Add individual room panels for this area
            foreach (string roomName in roomNames)
            {
                var sceneData = GetSceneData(roomName);
                string displayName = sceneData?.ReadableName ?? roomName;
                panels.Add(new TogglePanel(new MapRoomPatch(roomName), displayName));
            }

            return panels;
        }

        public void ShowAreaPanels(string areaName)
        {
            // Check if the menu is in a valid state
            if (CabbyCodesPlugin.cabbyMenu == null)
            {
                return;
            }

            // Remove all currently added panels
            foreach (var panel in currentlyAddedPanels)
            {
                CabbyCodesPlugin.cabbyMenu.RemoveCheatPanel(panel);
            }
            currentlyAddedPanels.Clear();

            // Create new panels for the selected area
            List<CheatPanel> panels = CreatePanelsForArea(areaName);
            foreach (CheatPanel panel in panels)
            {
                CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
                currentlyAddedPanels.Add(panel);
            }
        }

        public void HideAllPanels()
        {
            if (CabbyCodesPlugin.cabbyMenu == null)
            {
                return;
            }

            foreach (var panel in currentlyAddedPanels)
            {
                CabbyCodesPlugin.cabbyMenu.RemoveCheatPanel(panel);
            }
            currentlyAddedPanels.Clear();
        }

        public void AddAllPanelsToMenu()
        {
            // Get the selected area from the selector and show panels for that area
            var areaNames = GetAreaNames().ToList();
            var selectedAreaIndex = areaSelector.Get();
            string selectedArea = (selectedAreaIndex >= 0 && selectedAreaIndex < areaNames.Count)
                ? areaNames[selectedAreaIndex]
                : areaNames.Count > 0 ? areaNames[0] : AreaInstances.Dirtmouth.MapName;
            ShowAreaPanels(selectedArea);
        }

        private void ToggleAllRooms(string mapName, bool setToOn)
        {
            bool anyToggledOff = false;
            foreach (string roomName in MapRoomPatch.roomsInMaps[mapName])
            {
                if (setToOn && !FlagManager.ListFlagContains("scenesMapped", "Global", roomName))
                {
                    FlagManager.AddToListFlag("scenesMapped", "Global", roomName);
                }
                else if (!setToOn && FlagManager.ListFlagContains("scenesMapped", "Global", roomName))
                {
                    FlagManager.RemoveFromListFlag("scenesMapped", "Global", roomName);
                    anyToggledOff = true;
                }
            }

            // Update the toggle panels to reflect the changes
            foreach (var panel in currentlyAddedPanels)
            {
                if (panel is TogglePanel togglePanel)
                {
                    togglePanel.Update();
                }
            }

            // If any room was toggled off, trigger the reload
            if (!setToOn && anyToggledOff)
            {
                GameReloadManager.SaveAndReload();
            }
        }

        public void UpdateVisibleArea()
        {
            var areaNames = GetAreaNames().ToList();
            var selectedAreaIndex = areaSelector.Get();
            string selectedArea = (selectedAreaIndex >= 0 && selectedAreaIndex < areaNames.Count)
                ? areaNames[selectedAreaIndex]
                : areaNames.Count > 0 ? areaNames[0] : AreaInstances.Dirtmouth.MapName;
            // Always update panels, even if it's the same area (in case panels were cleared)
            ShowAreaPanels(selectedArea);
        }
    }
} 