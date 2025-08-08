namespace CabbyCodes.Scenes
{
    /// <summary>
    /// Represents mapping data for a single scene including its name, area, and readable display name.
    /// </summary>
    public class SceneMapData
    {
        private readonly string _readableName;

        /// <summary>
        /// The internal scene name used by the game.
        /// </summary>
        public string SceneName { get; }

        /// <summary>
        /// The area name this scene belongs to.
        /// </summary>
        public string AreaName { get; }

        /// <summary>
        /// The human-readable display name for this scene.
        /// Returns SceneName if ReadableName is null or empty.
        /// </summary>
        public string ReadableName => string.IsNullOrEmpty(_readableName) ? SceneName : _readableName;

        /// <summary>
        /// Whether this scene can be mapped (appears in map room toggle panels).
        /// </summary>
        public bool Mappable { get; }

        /// <summary>
        /// Initializes a new instance of the SceneMapData class.
        /// </summary>
        /// <param name="sceneName">The internal scene name.</param>
        /// <param name="areaName">The area name. Can be null for system scenes.</param>
        /// <param name="readableName">The human-readable display name.</param>
        /// <param name="mappable">Whether this scene can be mapped. Defaults to true.</param>
        public SceneMapData(string sceneName, string areaName, string readableName = "", bool mappable = true)
        {
            SceneName = sceneName;
            AreaName = areaName;
            _readableName = readableName;
            Mappable = mappable;
        }
    }
} 