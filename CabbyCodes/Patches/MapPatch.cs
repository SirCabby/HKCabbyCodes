using CabbyCodes.SyncedReferences;
using CabbyCodes.UI.CheatPanels;
using System;
using System.Reflection;

namespace CabbyCodes.Patches
{
    public class MapPatch : ISyncedReference<bool>
    {
        private readonly string mapName;

        public MapPatch(string mapName)
        {
            this.mapName = mapName;
        }

        public bool Get()
        {
            return (bool)typeof(PlayerData).GetField(mapName, BindingFlags.Public | BindingFlags.Instance).GetValue(PlayerData.instance);
        }

        public void Set(bool value)
        {
            typeof(PlayerData).GetField(mapName, BindingFlags.Public | BindingFlags.Instance).SetValue(PlayerData.instance, value);
        }

        public static void BuildMapPanels()
        {
            Type type = typeof(PlayerData).Assembly.GetType("PlayerData+MapBools");
            foreach (string name in Enum.GetNames(type))
            {
                string fixedName = char.ToLower(name[0]).ToString() + name.Substring(1);
                TogglePanel buttonPanel = new(new MapPatch(fixedName), fixedName);
                CabbyCodesPlugin.cabbyMenu.AddCheatPanel(buttonPanel);
            }
        }
    }
}
