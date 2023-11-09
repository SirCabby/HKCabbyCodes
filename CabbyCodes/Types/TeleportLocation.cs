using UnityEngine;

namespace CabbyCodes.Types
{
    public class TeleportLocation
    {
        public string sceneName;
        public string displayName;
        public Vector2 location;

        public TeleportLocation(string sceneName, string displayName, Vector2 location)
        {
            this.sceneName = sceneName;
            this.displayName = displayName;
            this.location = location;
        }
    }
}
