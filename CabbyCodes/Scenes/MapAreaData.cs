namespace CabbyCodes.Scenes
{
    /// <summary>
    /// Represents a map area with its internal name and human-readable display name.
    /// </summary>
    public class MapAreaData
    {
        /// <summary>
        /// The internal area name used by the game.
        /// </summary>
        public string MapName { get; }

        /// <summary>
        /// The human-readable display name for this area.
        /// </summary>
        public string ReadableName { get; }

        /// <summary>
        /// Initializes a new instance of the MapAreaData class.
        /// </summary>
        /// <param name="mapName">The internal area name.</param>
        /// <param name="readableName">The human-readable display name.</param>
        public MapAreaData(string mapName, string readableName)
        {
            MapName = mapName;
            ReadableName = readableName;
        }
    }
} 