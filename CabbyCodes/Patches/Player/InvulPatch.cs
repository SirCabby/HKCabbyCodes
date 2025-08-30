using CabbyMenu.SyncedReferences;
using System.Reflection;
using HarmonyLib;
using CabbyMenu;
using CabbyCodes.Flags;
using CabbyCodes.CheatState;
using BepInEx.Configuration;

namespace CabbyCodes.Patches.Player
{
    public class InvulPatch : ISyncedReference<bool>, ICheatStateRestorable
    {
        public const string key = "Invul_Patch";
        private static ConfigEntry<bool> configValue;
        private static readonly Harmony harmony = new Harmony(key);
        private static readonly MethodInfo mOriginal = AccessTools.Method(typeof(PlayerData), nameof(PlayerData.TakeHealth));
        private static readonly MethodInfo mOriginal2 = AccessTools.Method(typeof(PlayerData), nameof(PlayerData.WouldDie));
        private static readonly MethodInfo mOriginal3 = AccessTools.Method(typeof(HeroController), nameof(HeroController.TakeDamage));
        private static readonly FlagDef flag = FlagInstances.isInvincible;

        /// <summary>
        /// Initializes the configuration entry.
        /// </summary>
        private static void InitializeConfig()
        {
            if (configValue == null)
            {
                configValue = CabbyCodesPlugin.configFile.Bind("Player", "Invulnerability", false, 
                    "Enable player invulnerability");
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
                ApplyInvulnerabilityState();
            }
            else
            {
                harmony.UnpatchSelf();
                FlagManager.SetBoolFlag(flag, false);
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
                ApplyInvulnerabilityState();
            }
        }

        private void ApplyInvulnerabilityState()
        {
            harmony.Patch(mOriginal, prefix: new HarmonyMethod(typeof(CommonPatches).GetMethod("Prefix_SkipOriginal")));
            harmony.Patch(mOriginal2, prefix: new HarmonyMethod(typeof(CommonPatches).GetMethod("Prefix_SkipOriginal")));
            harmony.Patch(mOriginal3, prefix: new HarmonyMethod(typeof(CommonPatches).GetMethod("Prefix_SkipOriginal")));
            
            FlagManager.SetBoolFlag(flag, true);
            
            PlayerData.instance?.MaxHealth();
        }
    }
}
