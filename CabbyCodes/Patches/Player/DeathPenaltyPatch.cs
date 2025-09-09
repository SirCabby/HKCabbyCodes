using CabbyMenu.SyncedReferences;
using System.Reflection;
using HarmonyLib;
using BepInEx.Configuration;
using CabbyCodes.Flags;
using System.Collections;
using CabbyMenu.Utilities;

namespace CabbyCodes.Patches.Player
{
    public class DeathPenaltyPatch : ISyncedReference<bool>
    {
        public const string key = "DeathPenalty_Patch";
        private static ConfigEntry<bool> configValue;
        private static readonly Harmony harmony = new Harmony(key);
        private static readonly MethodInfo mOriginal = AccessTools.Method(typeof(PlayerData), nameof(PlayerData.StartSoulLimiter));
        private static readonly MethodInfo mOriginal2 = AccessTools.Method(typeof(GameManager), "PlayerDead");
        private static readonly MethodInfo mHeroDie = AccessTools.Method(typeof(HeroController), "Die");
        private static int storedGeoAmount;

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
                harmony.UnpatchSelf();
            }
        }


        private static void EnableDeathPenaltyPrevention()
        {
            try
            {
                harmony.UnpatchSelf();
                harmony.Patch(mOriginal, prefix: new HarmonyMethod(typeof(DeathPenaltyPatch).GetMethod(nameof(StartSoulLimiter_Prefix), BindingFlags.Static | BindingFlags.Public)));
                
                // Patch PlayerDead to apply death penalty prevention logic
                if (mOriginal2 != null)
                {
                    harmony.Patch(mOriginal2, prefix: new HarmonyMethod(typeof(DeathPenaltyPatch).GetMethod(nameof(PlayerDead_Prefix), BindingFlags.Static | BindingFlags.Public)));
                }

                // Patch HeroController.Die to capture geo early
                if (mHeroDie != null)
                {
                    harmony.Patch(mHeroDie, prefix: new HarmonyMethod(typeof(DeathPenaltyPatch).GetMethod(nameof(HeroDie_Prefix), BindingFlags.Static | BindingFlags.Public)));
                }
            }
            catch (System.Exception ex)
            {
                CabbyCodesPlugin.BLogger.LogError("DeathPenaltyPatch: Failed to apply death penalty patches - " + ex.Message);
            }
        }

        /// <summary>
        /// Prefix method for GameManager.PlayerDead() to apply death penalty prevention logic.
        /// </summary>
        public static bool PlayerDead_Prefix()
        {
            if (PlayerData.instance != null)
            {
                CoroutineRunner.Instance.StartCoroutine(ApplyDeathPenaltyLogic());
            }

            return true;
        }

        /// <summary>
        /// Prefix method for PlayerData.StartSoulLimiter() to skip execution.
        /// </summary>
        public static bool StartSoulLimiter_Prefix()
        {
            // Return false to skip the original method
            return false;
        }

        /// <summary>
        /// Prefix for HeroController.Die to capture geo amount immediately when death sequence starts.
        /// </summary>
        public static bool HeroDie_Prefix()
        {
            if (PlayerData.instance != null)
            {
                storedGeoAmount = PlayerData.instance.geo;
            }

            return true;
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