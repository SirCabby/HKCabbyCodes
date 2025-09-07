using CabbyMenu.SyncedReferences;
using System.Reflection;
using HarmonyLib;
using BepInEx.Configuration;
using CabbyCodes.Flags;
using CabbyCodes.CheatState;
using System.Collections;

namespace CabbyCodes.Patches.Player
{
    public class DeathPenaltyPatch : ISyncedReference<bool>, ICheatStateRestorable
    {
        public const string key = "DeathPenalty_Patch";
        private static ConfigEntry<bool> configValue;
        private static readonly Harmony harmony = new Harmony(key);
        private static readonly MethodInfo mOriginal = AccessTools.Method(typeof(PlayerData), nameof(PlayerData.StartSoulLimiter));
        private static readonly MethodInfo mOriginal2 = typeof(HeroController).GetMethod("Die", BindingFlags.NonPublic | BindingFlags.Instance);

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
                ApplyDeathPenaltyPatches();
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
                ApplyDeathPenaltyPatches();
            }
        }

        private void ApplyDeathPenaltyPatches()
        {
            // Patch StartSoulLimiter to skip execution
            harmony.Patch(mOriginal, prefix: new HarmonyMethod(typeof(DeathPenaltyPatch).GetMethod(nameof(StartSoulLimiter_Prefix), BindingFlags.Static | BindingFlags.Public)));
            
            // Patch Die method to prevent shade creation
            if (mOriginal2 != null)
            {
                harmony.Patch(mOriginal2, postfix: new HarmonyMethod(typeof(DeathPenaltyPatch).GetMethod(nameof(Die_Postfix), BindingFlags.Static | BindingFlags.Public)));
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
        /// Postfix method for HeroController.Die() to prevent shade creation.
        /// </summary>
        public static void Die_Postfix()
        {
            // Start coroutine with 1-frame delay
            GameManager.instance.StartCoroutine(Die_PostfixDelayed());
        }

        /// <summary>
        /// Delayed execution of death penalty removal after 1 frame.
        /// </summary>
        private static IEnumerator Die_PostfixDelayed()
        {
            // Wait for 1 frame
            yield return null;
            
            // Execute death penalty removal logic
            FlagManager.SetStringFlag(FlagInstances.shadeScene, "None");
            FlagManager.SetBoolFlag(FlagInstances.soulLimited, false);
            FlagManager.SetIntFlag(FlagInstances.geo, FlagManager.GetIntFlag(FlagInstances.geoPool));
            FlagManager.SetIntFlag(FlagInstances.geoPool, 0);
        }
    }
} 