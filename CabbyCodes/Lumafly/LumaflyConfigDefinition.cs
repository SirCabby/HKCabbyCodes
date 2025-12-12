#if LUMAFLY
namespace BepInEx.Configuration
{
    /// <summary>
    /// Config key definition for Lumafly builds.
    /// </summary>
    public class ConfigDefinition
    {
        public string Section { get; private set; }
        public string Key { get; private set; }

        public ConfigDefinition(string section, string key)
        {
            Section = section;
            Key = key;
        }

        public override bool Equals(object obj)
        {
            var other = obj as ConfigDefinition;
            if (other == null) return false;
            return Section == other.Section && Key == other.Key;
        }

        public override int GetHashCode()
        {
            return (Section + Key).GetHashCode();
        }
    }
}
#endif

