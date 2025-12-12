#if LUMAFLY
using System;

namespace BepInEx.Configuration
{
    /// <summary>
    /// Standalone ConfigEntry implementation for Lumafly builds.
    /// </summary>
    public class ConfigEntry<T>
    {
        private readonly ConfigFile config;
        private readonly string section;
        private readonly string key;
        private readonly T defaultValue;
        private T cachedValue;
        private bool hasValue;

        public ConfigDefinition Definition { get { return new ConfigDefinition(section, key); } }

        public T Value
        {
            get
            {
                if (!hasValue)
                {
                    var strValue = config.GetValue(section, key, null);
                    if (strValue != null)
                    {
                        cachedValue = ConvertFromString(strValue);
                    }
                    else
                    {
                        cachedValue = defaultValue;
                        config.SetValue(section, key, ConvertToString(defaultValue));
                    }
                    hasValue = true;
                }
                return cachedValue;
            }
            set
            {
                cachedValue = value;
                hasValue = true;
                config.SetValue(section, key, ConvertToString(value));
            }
        }

        public ConfigEntry(ConfigFile config, string section, string key, T defaultValue, string description)
        {
            this.config = config;
            this.section = section;
            this.key = key;
            this.defaultValue = defaultValue;
            this.hasValue = false;
        }

        private string ConvertToString(T value)
        {
            if (value == null) return "";
            if (typeof(T) == typeof(UnityEngine.KeyCode))
            {
                return value.ToString();
            }
            return value.ToString();
        }

        private T ConvertFromString(string str)
        {
            if (string.IsNullOrEmpty(str)) return defaultValue;

            var type = typeof(T);
            if (type == typeof(string)) return (T)(object)str;
            if (type == typeof(int)) return (T)(object)int.Parse(str);
            if (type == typeof(float)) return (T)(object)float.Parse(str);
            if (type == typeof(bool)) return (T)(object)bool.Parse(str);
            if (type == typeof(double)) return (T)(object)double.Parse(str);
            if (type == typeof(UnityEngine.KeyCode))
            {
                return (T)(object)Enum.Parse(typeof(UnityEngine.KeyCode), str);
            }
            if (type.IsEnum) return (T)Enum.Parse(type, str);

            return defaultValue;
        }
    }
}
#endif

