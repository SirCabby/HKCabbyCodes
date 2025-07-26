using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using System.Collections.Generic;
using System.Linq;
using static CabbyCodes.Scenes.SceneManagement;
using CabbyCodes.Flags;
using CabbyCodes.SavedGames;

namespace CabbyCodes.Patches.Maps
{
    public class MapRoomPatch : ISyncedReference<bool>
    {
        /// <summary>
        /// Scene names that should be excluded from map room patch functionality.
        /// These scenes will not appear in the map room toggle panels.
        /// </summary>
        private static readonly string[] ExcludedMapScenes = {
            "Room_temple"  // Temple of the Black Egg - should not be mappable
        };

        public static readonly Dictionary<string, List<string>> roomsInMaps = GetFilteredAreaToScenesMapping();

        private readonly string roomName;

        public MapRoomPatch(string roomName)
        {
            this.roomName = roomName;
        }

        public bool Get()
        {
            return FlagManager.ListFlagContains("scenesMapped", "Global", roomName);
        }

        public void Set(bool value)
        {
            if (value && !Get())
            {
                FlagManager.AddToListFlag("scenesMapped", "Global", roomName);
            }
            else if (!value && Get())
            {
                // Remove the flag first
                FlagManager.RemoveFromListFlag("scenesMapped", "Global", roomName);
                // Reload to refresh map ui
                GameReloadManager.SaveAndReload();
            }
        }

        /// <summary>
        /// Gets a filtered dictionary mapping area names to lists of scene names in that area.
        /// Excludes scenes that are in the ExcludedMapScenes array.
        /// </summary>
        /// <returns>A dictionary where keys are area names and values are lists of scene names.</returns>
        private static Dictionary<string, List<string>> GetFilteredAreaToScenesMapping()
        {
            var unfilteredMapping = GetAreaToScenesMapping();
            var filteredMapping = new Dictionary<string, List<string>>();

            foreach (var kvp in unfilteredMapping)
            {
                string areaName = kvp.Key;
                var filteredScenes = kvp.Value.Where(sceneName => !ExcludedMapScenes.Contains(sceneName)).ToList();
                
                if (filteredScenes.Count > 0)
                {
                    filteredMapping[areaName] = filteredScenes;
                }
            }

            return filteredMapping;
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
