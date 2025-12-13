using CabbyMenu.SyncedReferences;
using BepInEx.Configuration;
using MonoMod.RuntimeDetour;
using System;
using System.Reflection;

namespace CabbyCodes.Patches.Player
{
    public class SoulPatch : ISyncedReference<bool>
    {
        public const string key = "Soul_Patch";
        private static ConfigEntry<bool> configValue;
        
        // MonoMod.RuntimeDetour hooks - unified for all builds
        private static Hook hookTakeMP;
        private static Hook hookTakeReserveMP;
        private static Hook hookClearMP;

        /// <summary>
        /// Initializes the configuration entry.
        /// </summary>
        private static void InitializeConfig()
        {
            if (configValue == null)
            {
                configValue = CabbyCodesPlugin.configFile.Bind("Player", "InfiniteSoul", false, 
                    "Enable infinite soul");
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
                PlayerData.instance?.AddMPCharge(Constants.INFINITE_SOUL_CHARGE);
            }
            else
            {
                RemoveHooks();
            }
        }

        private static void ApplyHooks()
        {
            if (hookTakeMP == null)
            {
                hookTakeMP = new Hook(
                    typeof(PlayerData).GetMethod(nameof(PlayerData.TakeMP), BindingFlags.Public | BindingFlags.Instance),
                    typeof(SoulPatch).GetMethod(nameof(OnTakeMP), BindingFlags.NonPublic | BindingFlags.Static)
                );
            }
            if (hookTakeReserveMP == null)
            {
                hookTakeReserveMP = new Hook(
                    typeof(PlayerData).GetMethod(nameof(PlayerData.TakeReserveMP), BindingFlags.Public | BindingFlags.Instance),
                    typeof(SoulPatch).GetMethod(nameof(OnTakeReserveMP), BindingFlags.NonPublic | BindingFlags.Static)
                );
            }
            if (hookClearMP == null)
            {
                hookClearMP = new Hook(
                    typeof(PlayerData).GetMethod(nameof(PlayerData.ClearMP), BindingFlags.Public | BindingFlags.Instance),
                    typeof(SoulPatch).GetMethod(nameof(OnClearMP), BindingFlags.NonPublic | BindingFlags.Static)
                );
            }
        }

        private static void RemoveHooks()
        {
            hookTakeMP?.Dispose();
            hookTakeMP = null;
            hookTakeReserveMP?.Dispose();
            hookTakeReserveMP = null;
            hookClearMP?.Dispose();
            hookClearMP = null;
        }

        // Hook handlers - skip original by not calling orig()
        private static void OnTakeMP(Action<PlayerData, int> orig, PlayerData self, int amount)
        {
            // Skip original - don't call orig()
        }

        private static void OnTakeReserveMP(Action<PlayerData, int> orig, PlayerData self, int amount)
        {
            // Skip original - don't call orig()
        }

        private static void OnClearMP(Action<PlayerData> orig, PlayerData self)
        {
            // Skip original - don't call orig()
        }
    }
}
