using CabbyMenu.UI.CheatPanels;
using CabbyMenu.SyncedReferences;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static CabbyCodes.Scenes.SceneManagement;
using static CabbyCodes.Scenes.Areas;
using CabbyMenu.UI.DynamicPanels;
using CabbyCodes.Flags;
using CabbyCodes.SavedGames;
using CabbyCodes.Patches.BasePatches;
using BepInEx.Configuration;

namespace CabbyCodes.Patches.Maps
{
    public class MapPatch : BasePatch
    {
        private static bool sceneEventHandlerRegistered = false;
        private static ConfigEntry<bool> autoMapConfig;

        public static void AddPanels()
        {
            var mapPatch = new MapPatch();
            var panels = mapPatch.CreatePanels();
            foreach (var panel in panels)
            {
                CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
            }

            // Initialize scene monitoring for auto map functionality
            InitializeSceneMonitoring();
        }

        /// <summary>
        /// Initializes the auto map configuration entry.
        /// </summary>
        private static void InitializeAutoMapConfig()
        {
            if (autoMapConfig == null)
            {
                autoMapConfig = CabbyCodesPlugin.configFile.Bind("Maps", "AutoMap", false, 
                    "Automatically give map for current area and enable room mapping when entering scenes");
            }
        }

        /// <summary>
        /// Initialize scene change monitoring for auto map functionality.
        /// </summary>
        private static void InitializeSceneMonitoring()
        {
            if (sceneEventHandlerRegistered) return;
            
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnActiveSceneChanged;
            sceneEventHandlerRegistered = true;
        }

        private static void OnActiveSceneChanged(UnityEngine.SceneManagement.Scene oldScene, UnityEngine.SceneManagement.Scene newScene)
        {
            // Call auto map functionality when entering a new scene
            EnableAutoMapForCurrentScene();
        }

        public override List<CheatPanel> CreatePanels()
        {
            var panels = new List<CheatPanel>
            {
                new InfoPanel("Maps").SetColor(CheatPanel.headerColor),
                CreateAutoMapPanel(),
                new InfoPanel("Enable to have map for area").SetColor(CheatPanel.subHeaderColor),
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
                insertionIndex: 1,  // insert directly after the dropdown
                parentManager: null,
                onPanelsChanged: null,
                menu: CabbyCodesPlugin.cabbyMenu
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
                            UpdateHasMapFlagStatic();
                        }
                    ), GetDescription(flag)));
            }
            
            return panels;
        }

        private static void UpdateHasMapFlagStatic()
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
                    ButtonPanel toggleAllPanel = new ButtonPanel(() => {
                        var areaData = GetAreaData(selectedArea);
                        string readableAreaName = areaData?.ReadableName ?? selectedArea;
                        Teleport.TeleportService.ShowConfirmationDialog(
                            "Confirm Action",
                            $"This will map ALL rooms in {readableAreaName}.\n\nAre you sure you want to proceed?",
                            "Map All",
                            "Cancel",
                            () => ToggleAllRooms(selectedArea, true));
                    }, "ON", "Toggle All Rooms");
                    PanelAdder.AddButton(toggleAllPanel, 1, () => {
                        var areaData = GetAreaData(selectedArea);
                        string readableAreaName = areaData?.ReadableName ?? selectedArea;
                        Teleport.TeleportService.ShowConfirmationDialog(
                            "Confirm Action",
                            $"This will unmap ALL rooms in {readableAreaName}.\n\nWarning: This requires a save and reload.\n\nAre you sure you want to proceed?",
                            "Unmap All",
                            "Cancel",
                            () => ToggleAllRooms(selectedArea, false));
                    }, "OFF", new Vector2(120, 60));
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

        private TogglePanel CreateAutoMapPanel()
        {
            return new TogglePanel(new DelegateReference<bool>(
                () => {
                    InitializeAutoMapConfig();
                    return autoMapConfig.Value;
                },
                value =>
                {
                    InitializeAutoMapConfig();
                    autoMapConfig.Value = value;
                    
                    if (value)
                    {
                        // When enabling auto map, also enable the map flag
                        FlagManager.SetBoolFlag(FlagInstances.hasMap, true);
                        
                        // Enable auto map for current scene if we're in a valid area
                        EnableAutoMapForCurrentScene();
                    }
                }
            ), "Auto Map");
        }

        /// <summary>
        /// Enables auto map for the current scene. Should be called when entering a new scene.
        /// </summary>
        public static void EnableAutoMapForCurrentScene()
        {
            InitializeAutoMapConfig();
            if (!autoMapConfig.Value)
            {
                return; // Auto map is disabled
            }

            try
            {
                // Get current scene name
                string currentSceneName = GameStateProvider.GetCurrentSceneName();
                
                // Get scene data to determine area
                var sceneData = GetSceneData(currentSceneName);
                if (sceneData == null || string.IsNullOrEmpty(sceneData.AreaName))
                {
                    // Scene doesn't belong to any area or is not mappable
                    return;
                }

                string areaName = sceneData.AreaName;

                // Enable the area map flag
                EnableAreaMapFlag(areaName);

                // Enable the room mapping for this scene
                if (!FlagManager.ContainsInListFlag(FlagInstances.scenesMapped, currentSceneName))
                {
                    FlagManager.AddToListFlag(FlagInstances.scenesMapped, currentSceneName);
                    // Cancel reload request since we're adding the room back
                    GameReloadManager.CancelReload($"MapRoom_{currentSceneName}");
                }
            }
            catch
            {
                // If anything goes wrong, silently fail to avoid breaking the game
            }
        }

        /// <summary>
        /// Enables the map flag for a specific area.
        /// </summary>
        private static void EnableAreaMapFlag(string areaName)
        {
            switch (areaName)
            {
                case "Abyss":
                    FlagManager.SetBoolFlag(FlagInstances.mapAbyss, true);
                    break;
                case "City":
                    FlagManager.SetBoolFlag(FlagInstances.mapCity, true);
                    break;
                case "Cliffs":
                    FlagManager.SetBoolFlag(FlagInstances.mapCliffs, true);
                    break;
                case "Crossroads":
                    FlagManager.SetBoolFlag(FlagInstances.mapCrossroads, true);
                    break;
                case "Deepnest":
                    FlagManager.SetBoolFlag(FlagInstances.mapDeepnest, true);
                    break;
                case "FogCanyon":
                    FlagManager.SetBoolFlag(FlagInstances.mapFogCanyon, true);
                    break;
                case "FungalWastes":
                    FlagManager.SetBoolFlag(FlagInstances.mapFungalWastes, true);
                    break;
                case "Greenpath":
                    FlagManager.SetBoolFlag(FlagInstances.mapGreenpath, true);
                    break;
                case "Mines":
                    FlagManager.SetBoolFlag(FlagInstances.mapMines, true);
                    break;
                case "Outskirts":
                    FlagManager.SetBoolFlag(FlagInstances.mapOutskirts, true);
                    break;
                case "RestingGrounds":
                    FlagManager.SetBoolFlag(FlagInstances.mapRestingGrounds, true);
                    break;
                case "RoyalGardens":
                    FlagManager.SetBoolFlag(FlagInstances.mapRoyalGardens, true);
                    break;
                case "Waterways":
                    FlagManager.SetBoolFlag(FlagInstances.mapWaterways, true);
                    break;
            }

            // Update the hasMap flag based on whether any map flags are true
            UpdateHasMapFlagStatic();
        }
    }
}
