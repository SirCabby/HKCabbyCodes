using CabbyMenu.SyncedReferences;
using System.Collections.Generic;
using System.Linq;
using CabbyCodes.Scenes;
using CabbyCodes.Flags.FlagData;
using static CabbyCodes.Scenes.SceneManagement;

namespace CabbyCodes.Patches.Flags
{
    public class SceneFlagsAreaSelector : ISyncedValueList
    {
        private static readonly List<string> areaNames = GetAreasWithSceneFlags().ToList();
        private int selectedAreaIndex = 0;

        public SceneFlagsAreaSelector()
        {
            // Set to current player area on initialization
            SetToCurrentPlayerArea();
        }

        public int Get()
        {
            return selectedAreaIndex;
        }

        public void Set(int value)
        {
            if (value >= 0 && value < areaNames.Count)
            {
                selectedAreaIndex = value;
            }
        }

        public List<string> GetValueList()
        {
            return areaNames;
        }

        public string GetSelectedAreaName()
        {
            if (selectedAreaIndex >= 0 && selectedAreaIndex < areaNames.Count)
            {
                return areaNames[selectedAreaIndex];
            }
            // Default fallback - get the first area name from the list
            return areaNames.Count > 0 ? areaNames[0] : AreaInstances.Dirtmouth.MapName;
        }

        /// <summary>
        /// Gets only areas that have scenes with flags defined in SceneFlagData.
        /// </summary>
        /// <returns>Collection of area names that have scene flags</returns>
        private static IEnumerable<string> GetAreasWithSceneFlags()
        {
            // Get all scene names that have flags defined in SceneFlagData
            var scenesWithFlags = SceneFlagData.GetAllSceneNamesWithFlags();
            
            // Get area names for those scenes
            var areasWithFlags = new HashSet<string>();
            
            foreach (var sceneName in scenesWithFlags)
            {
                var sceneData = GetSceneData(sceneName);
                if (sceneData != null)
                {
                    areasWithFlags.Add(sceneData.AreaName);
                }
            }
            
            // Return sorted area names
            return areasWithFlags.OrderBy(area => area);
        }

        /// <summary>
        /// Sets the selected area to the player's current area if possible.
        /// If the player is in an area without scene flags, defaults to the first area in the list.
        /// </summary>
        private void SetToCurrentPlayerArea()
        {
            try
            {
                var currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
                var sceneData = GetSceneData(currentScene);
                if (sceneData != null)
                {
                    var currentAreaIndex = areaNames.IndexOf(sceneData.AreaName);
                    if (currentAreaIndex >= 0)
                    {
                        selectedAreaIndex = currentAreaIndex;
                        return; // Successfully set to current area
                    }
                }
            }
            catch
            {
                // If we can't get the current scene, fall through to default
            }
            
            // Default to the first area in the filtered list (areas with scene flags)
            selectedAreaIndex = 0;
        }
    }
} 