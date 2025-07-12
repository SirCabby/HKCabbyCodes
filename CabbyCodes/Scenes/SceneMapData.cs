namespace CabbyCodes.Scenes
{
    /// <summary>
    /// Represents mapping data for a single scene including its name, area, and readable display name.
    /// </summary>
    public class SceneMapData
    {
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
        /// </summary>
        public string ReadableName { get; }

        /// <summary>
        /// Initializes a new instance of the SceneMapData class.
        /// </summary>
        /// <param name="sceneName">The internal scene name.</param>
        /// <param name="areaName">The area name.</param>
        /// <param name="readableName">The human-readable display name.</param>
        public SceneMapData(string sceneName, string areaName, string readableName = "")
        {
            SceneName = sceneName;
            AreaName = areaName;
            ReadableName = readableName;
        }
    }
} 