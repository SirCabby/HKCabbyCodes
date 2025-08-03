using CabbyMenu.UI.CheatPanels;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static CabbyCodes.Scenes.SceneManagement;
using CabbyMenu.UI.DynamicPanels;
using CabbyCodes.Flags;
using CabbyCodes.SavedGames;
using CabbyCodes.Patches.BasePatches;

namespace CabbyCodes.Patches.Maps
{
    public class MapPatch : BasePatch
    {
        public static void AddPanels()
        {
            var mapPatch = new MapPatch();
            var panels = mapPatch.CreatePanels();
            foreach (var panel in panels)
            {
                CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
            }
        }

        public override List<CheatPanel> CreatePanels()
        {
            var panels = new List<CheatPanel>
            {
                new InfoPanel("Maps: Enable to have map for area").SetColor(CheatPanel.headerColor),
                new InfoPanel("Warning: Still requires Inventory Map and Quill items to view / fill maps out").SetColor(CheatPanel.warningColor)
            };

            // Add area map panels using BasePatch functionality
            panels.AddRange(base.CreatePanels());
            
            // Get current area for default selection
            int currentAreaIndex = GetCurrentAreaIndex();
            
            // Add Rooms header
            panels.Add(new InfoPanel("Rooms: Enable to have room mapped out").SetColor(CheatPanel.headerColor));
            
            // Create area selector and dropdown panel (following RoomFlagsPatch pattern)
            var areaSelector = new MapAreaSelector(currentAreaIndex);
            var dropdownPanel = new DropdownPanel(areaSelector, "Select Area", Constants.DEFAULT_PANEL_HEIGHT);
            panels.Add(dropdownPanel);
            
            // Set up dynamic panel management (following RoomFlagsPatch pattern)
            var container = new MainMenuPanelContainer(CabbyCodesPlugin.cabbyMenu);
            var panelManager = new DynamicPanelManager(
                dropdownPanel,
                CreateRoomPanels,
                container,
                insertionIndex: 1  // insert directly after the dropdown
            );
            
            // Selection change listener will rebuild panels
            dropdownPanel.GetDropDownSync().GetCustomDropdown().onValueChanged.AddListener(_ => panelManager.RecreateDynamicPanels());
            
            // Defer initial panel creation to next frame, after the dropdown has been inserted
            CabbyMenu.Utilities.CoroutineRunner.RunNextFrame(() => panelManager.CreateInitialPanels(currentAreaIndex));
            
            return panels;
        }

        protected override FlagDef[] GetFlags()
        {
            return new FlagDef[]
            {
                FlagInstances.mapAbyss,
                FlagInstances.mapCity,
                FlagInstances.mapCliffs,
                FlagInstances.mapCrossroads,
                FlagInstances.mapDeepnest,
                FlagInstances.mapFogCanyon,
                FlagInstances.mapFungalWastes,
                FlagInstances.mapGreenpath,
                FlagInstances.mapMines,
                FlagInstances.mapOutskirts,
                FlagInstances.mapRestingGrounds,
                FlagInstances.mapRoyalGardens,
                FlagInstances.mapWaterways
            };
        }

        protected override IPatch CreatePatch(FlagDef flag)
        {
            // Use MapBoolPatch for map flags to manage hasMap flag
            if (flag.Type == "PlayerData_Bool")
            {
                return new MapBoolPatch(flag, GetDescription(flag));
            }
            
            // Fall back to base implementation for other types
            return base.CreatePatch(flag);
        }

        protected override string GetDescription(FlagDef flag)
        {
            // Convert flag name to readable area name
            string flagName = flag.Id;
            if (flagName.StartsWith("map"))
            {
                string areaName = flagName.Substring(3); // Remove "map" prefix
                return areaName;
            }
            return flag.ReadableName;
        }

        private static int GetCurrentAreaIndex()
        {
            try
            {
                // Get current scene name
                var (sceneName, _) = Teleport.TeleportService.GetCurrentPlayerPosition();
                
                // Get area name from scene
                var sceneData = GetSceneData(sceneName);
                if (sceneData != null)
                {
                    string areaName = sceneData.AreaName;
                    
                    // Find the index of this area in the list
                    var areaNames = Scenes.Areas.GetAllAreaNames().ToList();
                    return areaNames.IndexOf(areaName);
                }
            }
            catch
            {
                // If anything goes wrong, default to 0
            }
            
            return 0; // Default to first area if we can't determine current area
        }

        private static List<CheatPanel> CreateRoomPanels(int areaIndex)
        {
            var panels = new List<CheatPanel>
            {
                new InfoPanel("Warning: Turning rooms OFF requires a save and reload").SetColor(CheatPanel.warningColor)
            };
            
            // Get the selected area
            var areaNames = Scenes.Areas.GetAllAreaNames().ToList();
            if (areaIndex >= 0 && areaIndex < areaNames.Count)
            {
                string selectedArea = areaNames[areaIndex];
                
                // Create panels for the selected area using the existing MapRoomPanelManager logic
                if (MapRoomPatch.roomsInMaps.ContainsKey(selectedArea))
                {
                    List<string> roomNames = MapRoomPatch.roomsInMaps[selectedArea];
                    
                    // Add toggle all panel for this area
                    ButtonPanel toggleAllPanel = new ButtonPanel(() => ToggleAllRooms(selectedArea, true), "ON", "Toggle All Rooms");
                    PanelAdder.AddButton(toggleAllPanel, 1, () => ToggleAllRooms(selectedArea, false), "OFF", new Vector2(120, 60));
                    panels.Add(toggleAllPanel);

                    // Add individual room panels for this area
                    foreach (string roomName in roomNames)
                    {
                        var sceneData = GetSceneData(roomName);
                        string displayName = sceneData?.ReadableName ?? roomName;
                        panels.Add(new TogglePanel(new MapRoomPatch(roomName), displayName));
                    }
                }
            }
            
            return panels;
        }

        private static void ToggleAllRooms(string mapName, bool setToOn)
        {
            bool anyToggledOff = false;
            foreach (string roomName in MapRoomPatch.roomsInMaps[mapName])
            {
                if (setToOn && !FlagManager.ListFlagContains(FlagInstances.scenesMapped, roomName))
                {
                    FlagManager.AddToListFlag(FlagInstances.scenesMapped, roomName);
                }
                else if (!setToOn && FlagManager.ListFlagContains(FlagInstances.scenesMapped, roomName))
                {
                    FlagManager.RemoveFromListFlag(FlagInstances.scenesMapped, roomName);
                    anyToggledOff = true;
                }
            }

            // If any room was toggled off, trigger the reload
            if (!setToOn && anyToggledOff)
            {
                GameReloadManager.SaveAndReload();
            }
        }
    }
}
