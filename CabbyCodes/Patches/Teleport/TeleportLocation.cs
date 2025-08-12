using UnityEngine;
using CabbyCodes.Scenes;

namespace CabbyCodes.Patches.Teleport
{
    /// <summary>
    /// Represents a teleport location in the game with scene and position information.
    /// </summary>
    public class TeleportLocation
    {
        /// <summary>
        /// Strongly-typed reference to the scene of this teleport.
        /// </summary>
        public virtual SceneMapData Scene { get; protected set; }

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
        /// Initializes a new instance of the TeleportLocation class using a SceneMapData instance.
        /// </summary>
        /// <param name="sceneData">The scene data containing scene name and readable name.</param>
        /// <param name="location">The 2D position coordinates of this teleport location.</param>
        public TeleportLocation(SceneMapData sceneData, string displayName, Vector2 location)
        {
            Scene = sceneData ?? throw new System.ArgumentNullException(nameof(sceneData));
            DisplayName = displayName;
            Location = location;
        }

        /// <summary>
        /// Convenience constructor matching the historical signature that only required a <see cref="SceneMapData"/> and coordinates.
        /// The <paramref name="location"/> parameter is required; the display name will default to <see cref="SceneMapData.ReadableName"/>.
        /// </summary>
        public TeleportLocation(SceneMapData sceneData, Vector2 location)
            : this(sceneData, sceneData?.ReadableName, location)
        {
        }
    }
} 