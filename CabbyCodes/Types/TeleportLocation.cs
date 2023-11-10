using UnityEngine;

namespace CabbyCodes.Types
{
    public class TeleportLocation
    {
        public virtual string SceneName { get; protected set; }
        public virtual string DisplayName { get; protected set; }
        public virtual Vector2 Location { get; set; }

        protected TeleportLocation() { }

        public TeleportLocation(string sceneName, string displayName, Vector2 location)
        {
            SceneName = sceneName;
            DisplayName = displayName;
            Location = location;
        }
    }
}
