#if LUMAFLY
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BepInEx.Configuration
{
    /// <summary>
    /// Standalone ConfigFile implementation for Lumafly builds that mimics BepInEx's API.
    /// </summary>
    public class ConfigFile
    {
        private readonly string configPath;
        private readonly Dictionary<string, Dictionary<string, string>> sections = new Dictionary<string, Dictionary<string, string>>();

        public string ConfigFilePath { get { return configPath; } }

        public ConfigFile(string configPath, bool saveOnInit)
        {
            this.configPath = configPath;
            Load();
        }

        public ConfigEntry<T> Bind<T>(string section, string key, T defaultValue, string description = "")
        {
            return new ConfigEntry<T>(this, section, key, defaultValue, description);
        }

        public ConfigEntry<T> Bind<T>(string section, string key, T defaultValue, ConfigDescription description)
        {
            return new ConfigEntry<T>(this, section, key, defaultValue, description?.Description ?? "");
        }

        public ConfigEntry<T> Bind<T>(ConfigDefinition definition, T defaultValue, ConfigDescription description = null)
        {
            return new ConfigEntry<T>(this, definition.Section, definition.Key, defaultValue, description?.Description ?? "");
        }

        internal string GetValue(string section, string key, string defaultValue)
        {
            if (sections.TryGetValue(section, out var sectionDict))
            {
                if (sectionDict.TryGetValue(key, out var value))
                {
                    return value;
                }
            }
            return defaultValue;
        }

        internal void SetValue(string section, string key, string value)
        {
            if (!sections.ContainsKey(section))
            {
                sections[section] = new Dictionary<string, string>();
            }
            sections[section][key] = value;
            Save();
        }

        internal bool ContainsKey(ConfigDefinition definition)
        {
            if (sections.TryGetValue(definition.Section, out var sectionDict))
            {
                return sectionDict.ContainsKey(definition.Key);
            }
            return false;
        }

        internal bool Remove(ConfigDefinition definition)
        {
            if (sections.TryGetValue(definition.Section, out var sectionDict))
            {
                return sectionDict.Remove(definition.Key);
            }
            return false;
        }

        private void Load()
        {
            if (!File.Exists(configPath)) return;

            string currentSection = "";
            foreach (var line in File.ReadAllLines(configPath))
            {
                var trimmed = line.Trim();
                if (string.IsNullOrEmpty(trimmed) || trimmed.StartsWith("#") || trimmed.StartsWith(";"))
                    continue;

                if (trimmed.StartsWith("[") && trimmed.EndsWith("]"))
                {
                    currentSection = trimmed.Substring(1, trimmed.Length - 2);
                    if (!sections.ContainsKey(currentSection))
                        sections[currentSection] = new Dictionary<string, string>();
                }
                else if (trimmed.Contains("="))
                {
                    var idx = trimmed.IndexOf('=');
                    var key = trimmed.Substring(0, idx).Trim();
                    var value = trimmed.Substring(idx + 1).Trim();
                    if (!string.IsNullOrEmpty(currentSection) && !sections.ContainsKey(currentSection))
                        sections[currentSection] = new Dictionary<string, string>();
                    if (!string.IsNullOrEmpty(currentSection))
                        sections[currentSection][key] = value;
                }
            }
        }

        public void Save()
        {
            var sb = new StringBuilder();
            sb.AppendLine("# CabbyCodes Configuration (Lumafly)");
            sb.AppendLine();

            foreach (var section in sections)
            {
                sb.AppendLine(string.Format("[{0}]", section.Key));
                foreach (var kvp in section.Value)
                {
                    sb.AppendLine(string.Format("{0} = {1}", kvp.Key, kvp.Value));
                }
                sb.AppendLine();
            }

            File.WriteAllText(configPath, sb.ToString());
        }
    }
}
#endif

