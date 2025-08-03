using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CabbyCodes.Scenes
{
    /// <summary>
    /// Maps Hollow Knight scene names to readable display names and areas.
    /// </summary>
    public static class SceneManagement
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
        /// Gets area names that have at least one scene with flags defined.
        /// </summary>
        /// <returns>A collection of area names that have flags, sorted alphabetically.</returns>
        public static IEnumerable<string> GetAreaNamesWithFlags()
        {
            return GetAreaFlags().Keys.OrderBy(area => area);
        }

        /// <summary>
        /// Gets a dictionary mapping area names to lists of flags for that area.
        /// </summary>
        /// <returns>A dictionary where keys are area names and values are lists of flags.</returns>
        public static Dictionary<string, List<Flags.FlagDef>> GetAreaFlags()
        {
            var areaFlags = new Dictionary<string, List<Flags.FlagDef>>();
            
            // Get all FlagDef instances from FlagInstances class using reflection
            var flagInstancesType = typeof(Flags.FlagInstances);
            var fields = flagInstancesType.GetFields(BindingFlags.Public | BindingFlags.Static);
            
            foreach (var field in fields)
            {
                if (field.FieldType == typeof(Flags.FlagDef))
                {
                    var flagDef = (Flags.FlagDef)field.GetValue(null);
                    
                    // Skip global flags (scene name is "Global")
                    if (flagDef.SceneName == "Global")
                        continue;
                    
                    // Get the area name for this scene
                    var sceneData = GetSceneData(flagDef.SceneName);
                    if (sceneData != null)
                    {
                        string areaName = sceneData.AreaName;
                        
                        if (!areaFlags.ContainsKey(areaName))
                        {
                            areaFlags[areaName] = new List<Flags.FlagDef>();
                        }
                        
                        areaFlags[areaName].Add(flagDef);
                    }
                }
            }
            
            // Sort flags within each area by readable name
            foreach (var area in areaFlags.Keys.ToList())
            {
                areaFlags[area] = areaFlags[area].OrderBy(f => f.ReadableName).ToList();
            }
            
            return areaFlags;
        }

        /// <summary>
        /// Gets a dictionary mapping area names to lists of scene names in that area.
        /// </summary>
        /// <returns>A dictionary where keys are area names and values are lists of scene names.</returns>
        public static Dictionary<string, List<string>> GetAreaToScenesMapping()
        {
            var areaToScenes = new Dictionary<string, List<string>>();

            foreach (var sceneData in sceneMapData)
            {
                string areaName = sceneData.AreaName;
                string sceneName = sceneData.SceneName;

                if (!areaToScenes.ContainsKey(areaName))
                {
                    areaToScenes[areaName] = new List<string>();
                }

                areaToScenes[areaName].Add(sceneName);
            }

            return areaToScenes;
        }

        /// <summary>
        /// Gets all scene data from the static SceneInstances class using reflection.
        /// </summary>
        /// <returns>A list of all SceneMapData objects from the static SceneInstances class.</returns>
        private static List<SceneMapData> GetAllSceneDataFromStaticReferences()
        {
            var sceneDataList = new List<SceneMapData>();
            var scenesType = typeof(SceneInstances);
            var fields = scenesType.GetFields(BindingFlags.Public | BindingFlags.Static);

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