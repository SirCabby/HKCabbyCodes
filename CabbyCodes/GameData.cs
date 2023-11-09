using System.Reflection;

namespace CabbyCodes
{
    public class GameData
    {
        // GameManager
        public static readonly FieldInfo targetSceneFieldInfo = typeof(GameManager).GetField("targetScene", BindingFlags.NonPublic | BindingFlags.Instance);
        public static readonly FieldInfo entryDelayFieldInfo = typeof(GameManager).GetField("entryDelay", BindingFlags.NonPublic | BindingFlags.Instance);

        // GameMap
        public static readonly FieldInfo heroFieldInfo = typeof(GameMap).GetField("hero", BindingFlags.NonPublic | BindingFlags.Instance);
    }
}
