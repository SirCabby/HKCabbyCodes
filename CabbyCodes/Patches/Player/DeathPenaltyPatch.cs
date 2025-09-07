using CabbyMenu.SyncedReferences;
using System.Reflection;
using HarmonyLib;
using BepInEx.Configuration;
using CabbyCodes.Flags;
using CabbyCodes.CheatState;
using UnityEngine;

namespace CabbyCodes.Patches.Player
{
    public class DeathPenaltyPatch : ISyncedReference<bool>, ICheatStateRestorable
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
            CheatStateManager.RegisterCheatState(key, value);
            
            if (value)
            {
                CheatStateManager.RegisterRestorableCheat(this);
                EnableDeathPenaltyPrevention();
            }
            else
            {
                harmony.UnpatchSelf();
            }
        }

        public string GetCheatKey()
        {
            return key;
        }

        public void RestoreState()
        {
            // This method is called by CheatStateManager after scene transitions
            if (Get())
            {
                EnableDeathPenaltyPrevention();
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

        private static void DisableDeathPenaltyPrevention()
        {
            // No cleanup needed for direct execution approach
        }


        /// <summary>
        /// Prefix method for GameManager.PlayerDead() to apply death penalty prevention logic.
        /// </summary>
        public static void PlayerDead_Prefix()
        {
            if (PlayerData.instance != null)
            {
                ApplyDeathPenaltyLogic();
            }
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
        public static void HeroDie_Prefix()
        {
            if (PlayerData.instance != null)
            {
                storedGeoAmount = PlayerData.instance.geo;
            }
        }

        /// <summary>
        /// Applies death penalty prevention logic by clearing shade scene, removing soul limitation, and restoring geo.
        /// </summary>
        private static void ApplyDeathPenaltyLogic()
        {
            FlagManager.SetStringFlag(FlagInstances.shadeScene, "None");
            FlagManager.SetBoolFlag(FlagInstances.soulLimited, false);
            FlagManager.SetIntFlag(FlagInstances.geo, storedGeoAmount);
            FlagManager.SetIntFlag(FlagInstances.geoPool, 0);
            FlagManager.SetIntFlag(FlagInstances.maxMP, 99);
        }
    }
} 