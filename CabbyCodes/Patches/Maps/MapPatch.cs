using CabbyMenu.UI.CheatPanels;
using CabbyMenu.SyncedReferences;
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

            // Add area map panels using delegate pattern
            panels.AddRange(CreateMapPanels());
            
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

        private List<CheatPanel> CreateMapPanels()
        {
            var panels = new List<CheatPanel>();
            var flags = GetFlags();
            
            foreach (var flag in flags)
            {
                panels.Add(new TogglePanel(
                    new DelegateReference<bool>(
                        () => FlagManager.GetBoolFlag(flag),
                        value =>
                        {
                            // Set the individual map flag
                            FlagManager.SetBoolFlag(flag, value);
                            
                            // Update the hasMap flag based on whether any map flags are true
                            UpdateHasMapFlag();
                        }
                    ), GetDescription(flag)));
            }
            
            return panels;
        }

        private void UpdateHasMapFlag()
        {
            // Check if any map flags are true
            bool anyMapTrue = FlagManager.GetBoolFlag(FlagInstances.mapAbyss) ||
                             FlagManager.GetBoolFlag(FlagInstances.mapCity) ||
                             FlagManager.GetBoolFlag(FlagInstances.mapCliffs) ||
                             FlagManager.GetBoolFlag(FlagInstances.mapCrossroads) ||
                             FlagManager.GetBoolFlag(FlagInstances.mapDeepnest) ||
                             FlagManager.GetBoolFlag(FlagInstances.mapFogCanyon) ||
                             FlagManager.GetBoolFlag(FlagInstances.mapFungalWastes) ||
                             FlagManager.GetBoolFlag(FlagInstances.mapGreenpath) ||
                             FlagManager.GetBoolFlag(FlagInstances.mapMines) ||
                             FlagManager.GetBoolFlag(FlagInstances.mapOutskirts) ||
                             FlagManager.GetBoolFlag(FlagInstances.mapRestingGrounds) ||
                             FlagManager.GetBoolFlag(FlagInstances.mapRoyalGardens) ||
                             FlagManager.GetBoolFlag(FlagInstances.mapWaterways);

            // Set hasMap flag accordingly
            FlagManager.SetBoolFlag(FlagInstances.hasMap, anyMapTrue);
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
            foreach (string roomName in MapRoomPatch.roomsInMaps[mapName])
            {
                if (setToOn && !FlagManager.ContainsInListFlag(FlagInstances.scenesMapped, roomName))
                {
                    FlagManager.AddToListFlag(FlagInstances.scenesMapped, roomName);
                    // Cancel reload request since we're adding the room back
                    GameReloadManager.CancelReload($"MapRoom_{roomName}");
                }
                else if (!setToOn && FlagManager.ContainsInListFlag(FlagInstances.scenesMapped, roomName))
                {
                    FlagManager.RemoveFromListFlag(FlagInstances.scenesMapped, roomName);
                    // Request reload for this room
                    GameReloadManager.RequestReload($"MapRoom_{roomName}");
                }
            }

            // Note: Individual room reloads are now handled by the room-specific requests above
            // No need to call SaveAndReload here since each room manages its own reload state
        }
    }
}
