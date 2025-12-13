using CabbyMenu.SyncedReferences;
using BepInEx.Configuration;
using CabbyCodes.Flags;
using System.Collections;
using CabbyMenu.Utilities;
using MonoMod.RuntimeDetour;
using System;
using System.Reflection;

namespace CabbyCodes.Patches.Player
{
    public class DeathPenaltyPatch : ISyncedReference<bool>
    {
        public const string key = "DeathPenalty_Patch";
        private static ConfigEntry<bool> configValue;
        private static int storedGeoAmount;

        // MonoMod.RuntimeDetour hooks - unified for all builds
        private static Hook hookStartSoulLimiter;
        private static Hook hookPlayerDead;
        private static Hook hookDie;

        /// <summary>
        /// Initializes the configuration entry.
        /// </summary>
        private static void InitializeConfig()
        {
            if (configValue == null)
            {
                configValue = CabbyCodesPlugin.configFile.Bind("Player", "RemoveDeathPenalty", false, 
                    "Remove death penalty");
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
                EnableDeathPenaltyPrevention();
            }
            else
            {
                RemoveHooks();
            }
        }

        private static void EnableDeathPenaltyPrevention()
        {
            try
            {
                RemoveHooks();
                ApplyHooks();
            }
            catch (System.Exception ex)
            {
                CabbyCodesPlugin.BLogger.LogError("DeathPenaltyPatch: Failed to apply death penalty patches - " + ex.Message);
            }
        }

        private static void ApplyHooks()
        {
            if (hookStartSoulLimiter == null)
            {
                hookStartSoulLimiter = new Hook(
                    typeof(PlayerData).GetMethod(nameof(PlayerData.StartSoulLimiter), BindingFlags.Public | BindingFlags.Instance),
                    typeof(DeathPenaltyPatch).GetMethod(nameof(OnStartSoulLimiter), BindingFlags.NonPublic | BindingFlags.Static)
                );
            }
            if (hookPlayerDead == null)
            {
                var playerDeadMethod = typeof(GameManager).GetMethod("PlayerDead", BindingFlags.NonPublic | BindingFlags.Instance);
                if (playerDeadMethod != null)
                {
                    hookPlayerDead = new Hook(
                        playerDeadMethod,
                        typeof(DeathPenaltyPatch).GetMethod(nameof(OnPlayerDead), BindingFlags.NonPublic | BindingFlags.Static)
                    );
                }
            }
            if (hookDie == null)
            {
                var dieMethod = typeof(HeroController).GetMethod("Die", BindingFlags.NonPublic | BindingFlags.Instance);
                if (dieMethod != null)
                {
                    hookDie = new Hook(
                        dieMethod,
                        typeof(DeathPenaltyPatch).GetMethod(nameof(OnDie), BindingFlags.NonPublic | BindingFlags.Static)
                    );
                }
            }
        }

        private static void RemoveHooks()
        {
            hookStartSoulLimiter?.Dispose();
            hookStartSoulLimiter = null;
            hookPlayerDead?.Dispose();
            hookPlayerDead = null;
            hookDie?.Dispose();
            hookDie = null;
        }

        /// <summary>
        /// Hook for PlayerData.StartSoulLimiter() - skip execution.
        /// </summary>
        private static void OnStartSoulLimiter(Action<PlayerData> orig, PlayerData self)
        {
            // Skip original - don't call orig()
        }

        /// <summary>
        /// Hook for GameManager.PlayerDead() - apply death penalty prevention logic before calling original.
        /// </summary>
        private static void OnPlayerDead(Action<GameManager, float> orig, GameManager self, float waitTime)
        {
            if (PlayerData.instance != null)
            {
                CoroutineRunner.Instance.StartCoroutine(ApplyDeathPenaltyLogic());
            }
            orig(self, waitTime); // Allow original to run
        }

        /// <summary>
        /// Hook for HeroController.Die() - capture geo amount immediately when death sequence starts.
        /// </summary>
        private static void OnDie(Action<HeroController> orig, HeroController self)
        {
            if (PlayerData.instance != null)
            {
                storedGeoAmount = PlayerData.instance.geo;
            }
            orig(self); // Allow original to run
        }

        /// <summary>
        /// Applies death penalty prevention logic by clearing shade scene, removing soul limitation, and restoring geo.
        /// Runs as a coroutine.
        /// </summary>
        private static IEnumerator ApplyDeathPenaltyLogic()
        {
            FlagManager.SetStringFlag(FlagInstances.shadeScene, "None");
            FlagManager.SetBoolFlag(FlagInstances.soulLimited, false);
            FlagManager.SetIntFlag(FlagInstances.geo, storedGeoAmount);
            FlagManager.SetIntFlag(FlagInstances.geoPool, 0);
            FlagManager.SetIntFlag(FlagInstances.maxMP, 99);
            yield break;
        }
    }
}
