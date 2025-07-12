using System.Collections.Generic;
using System.Linq;

namespace CabbyCodes.Scenes
{
    /// <summary>
    /// Provides lookup and mapping functions for Hollow Knight map areas, similar to Scenes for SceneInstances.
    /// </summary>
    public static class Areas
    {
        private static readonly List<MapAreaData> areaData = GetAllAreaDataFromStaticReferences();
        private static readonly Dictionary<string, MapAreaData> areaLookup = areaData.ToDictionary(a => a.MapName, a => a);

        /// <summary>
        /// Gets the MapAreaData for a given area name.
        /// </summary>
        /// <param name="areaName">The internal area name.</param>
        /// <returns>The MapAreaData object, or null if not found.</returns>
        public static MapAreaData GetAreaData(string areaName)
        {
            if (string.IsNullOrEmpty(areaName))
                return null;
            areaLookup.TryGetValue(areaName, out var areaDataObj);
            return areaDataObj;
        }

        /// <summary>
        /// Gets all available area names.
        /// </summary>
        public static IEnumerable<string> GetAllAreaNames()
        {
            return areaData.Select(a => a.MapName);
        }

        /// <summary>
        /// Gets all area mapping data.
        /// </summary>
        public static IReadOnlyCollection<MapAreaData> GetAllAreaData()
        {
            return areaData.AsReadOnly();
        }

        /// <summary>
        /// Gets all readable area names.
        /// </summary>
        public static IEnumerable<string> GetReadableNames()
        {
            return areaData.Select(a => a.ReadableName);
        }

        /// <summary>
        /// Gets all area data from the static AreaInstances class using reflection.
        /// </summary>
        private static List<MapAreaData> GetAllAreaDataFromStaticReferences()
        {
            var areaDataList = new List<MapAreaData>();
            var areaType = typeof(AreaInstances);
            var fields = areaType.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            foreach (var field in fields)
            {
                if (field.FieldType == typeof(MapAreaData))
                {
                    var area = (MapAreaData)field.GetValue(null);
                    areaDataList.Add(area);
                }
            }
            return areaDataList;
        }
    }
} 