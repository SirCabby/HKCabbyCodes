using BepInEx.Configuration;
using UnityEngine;
using CabbyCodes.Scenes;

namespace CabbyCodes.Patches.Teleport
{
    /// <summary>
    /// Represents a custom teleport location that is stored in the configuration system.
    /// </summary>
    public class CustomTeleportLocation : TeleportLocation
    {
        /// <summary>
        /// The configuration definition for this custom location.
        /// </summary>
        public readonly ConfigDefinition configDef;

        /// <summary>
        /// The configuration entry that stores the location data.
        /// </summary>
        private readonly ConfigEntry<string> configEntry;

        private readonly string displayName;
        private SceneMapData scene;
        private Vector2 location;

        /// <summary>
        /// Gets the scene reference for teleporting.
        /// </summary>
        public override SceneMapData Scene { get => scene; }

        /// <summary>
        /// Gets the display name for the UI.
        /// </summary>
        public override string DisplayName { get => displayName; }

        /// <summary>
        /// Gets or sets the location coordinates.
        /// </summary>
        public override Vector2 Location
        {
            get => location;
            set
            {
                location = value;
                SaveToConfig();
            }
        }

        /// <summary>
        /// Updates both the scene and location coordinates, then saves to config.
        /// </summary>
        /// <param name="newSceneName">The new scene name.</param>
        /// <param name="newLocation">The new location coordinates.</param>
        public void UpdateLocation(string newSceneName, Vector2 newLocation)
        {
            scene = SceneManagement.GetSceneData(newSceneName) ?? new SceneMapData(newSceneName);
            location = newLocation;
            SaveToConfig();
        }

        /// <summary>
        /// Initializes a new instance of the CustomTeleportLocation class.
        /// </summary>
        /// <param name="configDef">The configuration definition for this location.</param>
        /// <param name="entry">The configuration entry that stores the location data.</param>
        public CustomTeleportLocation(ConfigDefinition configDef, ConfigEntry<string> entry)
        {
            this.configDef = configDef;
            configEntry = entry;
            displayName = configDef.Key; // Display name is the config key
            ParseConfigValue(entry.Value);
        }

        private void ParseConfigValue(string value)
        {
            // Format: sceneName|x|y (validated before creation)
            var parts = value.Split('|');
            var sceneName = parts[0];
            scene = SceneManagement.GetSceneData(sceneName) ?? new SceneMapData(sceneName);
            float.TryParse(parts[1], out float x);
            float.TryParse(parts[2], out float y);
            location = new Vector2(x, y);
        }

        private void SaveToConfig()
        {
            configEntry.Value = $"{scene.SceneName}|{location.x}|{location.y}";
        }
    }
} 