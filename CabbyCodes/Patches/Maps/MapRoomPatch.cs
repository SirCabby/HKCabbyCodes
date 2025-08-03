using CabbyMenu.SyncedReferences;
using System.Collections.Generic;
using System.Linq;
using static CabbyCodes.Scenes.SceneManagement;
using CabbyCodes.Flags;
using CabbyCodes.SavedGames;

namespace CabbyCodes.Patches.Maps
{
    public class MapRoomPatch : ISyncedReference<bool>
    {
        public static readonly Dictionary<string, List<string>> roomsInMaps = GetFilteredAreaToScenesMapping();

        private readonly string roomName;

        public MapRoomPatch(string roomName)
        {
            this.roomName = roomName;
        }

        public bool Get()
        {
            return FlagManager.ListFlagContains(FlagInstances.scenesMapped, roomName);
        }

        public void Set(bool value)
        {
            if (value && !Get())
            {
                FlagManager.AddToListFlag(FlagInstances.scenesMapped, roomName);
            }
            else if (!value && Get())
            {
                // Remove the flag first
                FlagManager.RemoveFromListFlag(FlagInstances.scenesMapped, roomName);
                // Reload to refresh map ui
                GameReloadManager.SaveAndReload();
            }
        }

        /// <summary>
        /// Gets a filtered dictionary mapping area names to lists of scene names in that area.
        /// Only includes scenes that have Mappable = true.
        /// </summary>
        /// <returns>A dictionary where keys are area names and values are lists of scene names.</returns>
        private static Dictionary<string, List<string>> GetFilteredAreaToScenesMapping()
        {
            var unfilteredMapping = GetAreaToScenesMapping();
            var filteredMapping = new Dictionary<string, List<string>>();

            foreach (var kvp in unfilteredMapping)
            {
                string areaName = kvp.Key;
                var filteredScenes = kvp.Value.Where(sceneName => 
                {
                    var sceneData = GetSceneData(sceneName);
                    return sceneData != null && sceneData.Mappable;
                }).ToList();
                
                if (filteredScenes.Count > 0)
                {
                    filteredMapping[areaName] = filteredScenes;
                }
            }

            return filteredMapping;
        }
    }
}
