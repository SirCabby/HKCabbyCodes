#if LUMAFLY
namespace BepInEx.Configuration
{
    /// <summary>
    /// Config description placeholder for Lumafly builds.
    /// </summary>
    public class ConfigDescription
    {
        public string Description { get; private set; }

        public ConfigDescription(string description)
        {
            Description = description;
        }
    }
}
#endif

