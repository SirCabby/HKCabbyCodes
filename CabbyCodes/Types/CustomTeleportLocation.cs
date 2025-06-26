using BepInEx.Configuration;
using UnityEngine;

namespace CabbyCodes.Types
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

        /// <summary>
        /// Gets the scene name from the configuration key.
        /// </summary>
        public override string SceneName { get => configDef.Key; }

        /// <summary>
        /// Gets the display name from the configuration key.
        /// </summary>
        public override string DisplayName { get => configDef.Key; }

        /// <summary>
        /// Gets or sets the location coordinates by parsing the configuration value.
        /// </summary>
        public override Vector2 Location
        {
            get
            {
                string[] locSplit = configEntry.Value.Split(',');
                return new Vector2(int.Parse(locSplit[0]), int.Parse(locSplit[1]));
            }
            set
            {
                configEntry.Value = value.x.ToString() + "," + value.y.ToString();
            }
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
        }
    }
}
