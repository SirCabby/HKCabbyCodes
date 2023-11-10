using BepInEx.Configuration;
using System.Collections.Generic;
using System.IO;

namespace CabbyCodes
{
    public class ConfigUtils
    {
        public static List<string> GetConfigKeys(string sectionName)
        {
            List<string> result = new();
            string filePath = Path.GetFullPath(CabbyCodesPlugin.configFile.ConfigFilePath);
            if (File.Exists(filePath))
            {
                string[] array = File.ReadAllLines(filePath);
                string section = "";
                for (int i = 0; i < array.Length; i++)
                {
                    string text = array[i].Trim();
                    if (text.StartsWith("#"))
                    {
                        continue;
                    }

                    if (text.StartsWith("[") && text.EndsWith("]"))
                    {
                        section = text.Substring(1, text.Length - 2);
                        continue;
                    }

                    if (section != sectionName) continue;

                    string[] array2 = text.Split(new char[1] { '=' }, 2);
                    if (array2.Length == 2)
                    {
                        result.Add(array2[0].Trim());
                    }
                }
            }

            return result;
        }

        public static ConfigDefinition GetConfigDefinition(string sectionName, string key)
        {
            foreach (ConfigDefinition def in CabbyCodesPlugin.configFile.Keys)
            {
                if (def.Section == sectionName && def.Key == key)
                {
                    return def;
                }
            }

            return null;
        }
    }
}
