using CabbyMenu.SyncedReferences;
using System.Reflection;
using HarmonyLib;
using CabbyMenu;
using BepInEx.Configuration;

namespace CabbyCodes.Patches.Player
{
    public class GeoPatch : ISyncedReference<bool>
    {
        public const string key = "Geo_Patch";
        private static ConfigEntry<bool> configValue;
        private static readonly Harmony harmony = new Harmony(key);
        private static readonly MethodInfo mOriginal1 = AccessTools.Method(typeof(PlayerData), nameof(PlayerData.TakeGeo));
        private static readonly MethodInfo mOriginal2 = AccessTools.Method(typeof(GeoCounter), nameof(GeoCounter.TakeGeo));

        /// <summary>
        /// Initializes the configuration entry.
        /// </summary>
        private static void InitializeConfig()
        {
            if (configValue == null)
            {
                configValue = CabbyCodesPlugin.configFile.Bind("Player", "InfiniteGeo", false, 
                    "Enable infinite geo");
            }
        }

        public bool Get()
        {
            InitializeConfig();
            return configValue.Value;
        }

        public void Set(bool value)
        {
            InitializeConfig();
            configValue.Value = value;

            if (value)
            {
                harmony.Patch(mOriginal1, prefix: new HarmonyMethod(typeof(CommonPatches).GetMethod("Prefix_SkipOriginal")));
                harmony.Patch(mOriginal2, prefix: new HarmonyMethod(typeof(CommonPatches).GetMethod("Prefix_SkipOriginal")));
            }
            else
            {
                harmony.UnpatchSelf();
            }
        }
    }
}
