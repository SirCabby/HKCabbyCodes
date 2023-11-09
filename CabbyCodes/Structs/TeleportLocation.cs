using UnityEngine;

namespace CabbyCodes.Structs
{
    public struct TeleportLocation
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

        public void SetLocation(Vector2 location)
        {
            this.location = location;
        }
    }
}
