using System.Collections.Generic;
using System.Linq;

namespace CabbyCodes.Scenes
{
    /// <summary>
    /// Maps Hollow Knight scene names to readable display names and areas.
    /// </summary>
    public static class Scenes
    {
        /// <summary>
        /// Collection of all scene mapping data from the static SceneInstances class.
        /// </summary>
        private static readonly List<SceneMapData> sceneMapData = GetAllSceneDataFromStaticReferences();

        /// <summary>
        /// Dictionary for O(1) lookup of scene data by scene name.
        /// </summary>
        private static readonly Dictionary<string, SceneMapData> sceneLookup = sceneMapData.ToDictionary(s => s.SceneName, s => s);

        /// <summary>
        /// Gets the SceneMapData for a given scene name.
        /// </summary>
        /// <param name="sceneName">The internal scene name.</param>
        /// <returns>The SceneMapData object, or null if not found.</returns>
        public static SceneMapData GetSceneData(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                return null;
            }

            sceneLookup.TryGetValue(sceneName, out var sceneData);
            return sceneData;
        }

        /// <summary>
        /// Gets all available scene names.
        /// </summary>
        /// <returns>A collection of all scene names.</returns>
        public static IEnumerable<string> GetAllSceneNames()
        {
            return sceneMapData.Select(s => s.SceneName);
        }

        /// <summary>
        /// Gets all scene mapping data.
        /// </summary>
        /// <returns>A collection of all scene mapping data.</returns>
        public static IReadOnlyCollection<SceneMapData> GetAllSceneData()
        {
            return sceneMapData.AsReadOnly();
        }

        /// <summary>
        /// Gets all unique area names, sorted alphabetically.
        /// </summary>
        /// <returns>A collection of unique area names.</returns>
        public static IEnumerable<string> GetAreaNames()
        {
            return sceneMapData
                .Select(s => s.AreaName)
                .Distinct()
                .OrderBy(area => area);
        }

        /// <summary>
        /// Gets all scene data from the static SceneInstances class using reflection.
        /// </summary>
        /// <returns>A list of all SceneMapData objects from the static SceneInstances class.</returns>
        private static List<SceneMapData> GetAllSceneDataFromStaticReferences()
        {
            var sceneDataList = new List<SceneMapData>();
            var scenesType = typeof(SceneInstances);
            var fields = scenesType.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

            foreach (var field in fields)
            {
                if (field.FieldType == typeof(SceneMapData))
                {
                    var sceneData = (SceneMapData)field.GetValue(null);
                    sceneDataList.Add(sceneData);
                }
            }

            return sceneDataList;
        }
    }
} 