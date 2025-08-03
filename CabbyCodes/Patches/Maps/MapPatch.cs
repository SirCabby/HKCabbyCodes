using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            
            // Add Rooms section
            var roomsSection = new CategorizedPanelSection(
                "Select Area",
                Scenes.Areas.GetAllAreaNames().ToList(),
                CreateRoomPanels,
                1, // insertionIndex
                currentAreaIndex // defaultSelection
            );
            roomsSection.AddToMenu(CabbyCodesPlugin.cabbyMenu);
            
            return panels;
        }

        protected override FlagDef[] GetFlags()
        {
            return new FlagDef[]
            {
                FlagInstances.mapAbyss,
                FlagInstances.mapAllRooms,
                FlagInstances.mapCity,
                FlagInstances.mapCliffs,
                FlagInstances.mapCrossroads,
                FlagInstances.mapDeepnest,
                FlagInstances.mapDirtmouth,
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
