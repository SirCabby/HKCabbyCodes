using BepInEx.Configuration;
using UnityEngine;

namespace CabbyCodes.Types
{
    public class CustomTeleportLocation : TeleportLocation
    {
        public readonly ConfigDefinition configDef;
        private readonly ConfigEntry<string> configEntry;

        public override string SceneName { get => configDef.Key; }
        public override string DisplayName { get => configDef.Key; }
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

        public CustomTeleportLocation(ConfigDefinition configDef, ConfigEntry<string> entry)
        {
            this.configDef = configDef;
            configEntry = entry;
        }
    }
}
