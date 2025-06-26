using UnityEngine;

namespace CabbyCodes.Types
{
    /// <summary>
    /// Represents a teleport location in the game with scene and position information.
    /// </summary>
    public class TeleportLocation
    {
        /// <summary>
        /// Gets or sets the name of the scene where this location is located.
        /// </summary>
        public virtual string SceneName { get; protected set; }

        /// <summary>
        /// Gets or sets the display name for this teleport location.
        /// </summary>
        public virtual string DisplayName { get; protected set; }

        /// <summary>
        /// Gets or sets the 2D position coordinates of this teleport location.
        /// </summary>
        public virtual Vector2 Location { get; set; }

        /// <summary>
        /// Protected constructor for derived classes.
        /// </summary>
        protected TeleportLocation() { }

        /// <summary>
        /// Initializes a new instance of the TeleportLocation class.
        /// </summary>
        /// <param name="sceneName">The name of the scene where this location is located.</param>
        /// <param name="displayName">The display name for this teleport location.</param>
        /// <param name="location">The 2D position coordinates of this teleport location.</param>
        public TeleportLocation(string sceneName, string displayName, Vector2 location)
        {
            SceneName = sceneName;
            DisplayName = displayName;
            Location = location;
        }
    }
}
