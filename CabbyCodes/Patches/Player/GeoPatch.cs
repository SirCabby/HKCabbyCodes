using CabbyMenu.SyncedReferences;
using BepInEx.Configuration;
using MonoMod.RuntimeDetour;
using System;
using System.Reflection;

namespace CabbyCodes.Patches.Player
{
    public class GeoPatch : ISyncedReference<bool>
    {
        public const string key = "Geo_Patch";
        private static ConfigEntry<bool> configValue;

        // MonoMod.RuntimeDetour hooks - unified for all builds
        private static Hook hookTakeGeo;
        private static Hook hookGeoCounterTakeGeo;

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
                ApplyHooks();
            }
            else
            {
                RemoveHooks();
            }
        }

        private static void ApplyHooks()
        {
            if (hookTakeGeo == null)
            {
                hookTakeGeo = new Hook(
                    typeof(PlayerData).GetMethod(nameof(PlayerData.TakeGeo), BindingFlags.Public | BindingFlags.Instance),
                    typeof(GeoPatch).GetMethod(nameof(OnTakeGeo), BindingFlags.NonPublic | BindingFlags.Static)
                );
            }
            if (hookGeoCounterTakeGeo == null)
            {
                hookGeoCounterTakeGeo = new Hook(
                    typeof(GeoCounter).GetMethod(nameof(GeoCounter.TakeGeo), BindingFlags.Public | BindingFlags.Instance),
                    typeof(GeoPatch).GetMethod(nameof(OnGeoCounterTakeGeo), BindingFlags.NonPublic | BindingFlags.Static)
                );
            }
        }

        private static void RemoveHooks()
        {
            hookTakeGeo?.Dispose();
            hookTakeGeo = null;
            hookGeoCounterTakeGeo?.Dispose();
            hookGeoCounterTakeGeo = null;
        }

        // Hook handlers - skip original by not calling orig()
        private static void OnTakeGeo(Action<PlayerData, int> orig, PlayerData self, int amount)
        {
            // Skip original - don't call orig()
        }

        private static void OnGeoCounterTakeGeo(Action<GeoCounter, int> orig, GeoCounter self, int geo)
        {
            // Skip original - don't call orig()
        }
    }
}
