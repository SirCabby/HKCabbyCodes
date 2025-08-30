using CabbyMenu.SyncedReferences;
using System.Reflection;
using HarmonyLib;
using CabbyMenu;
using BepInEx.Configuration;

namespace CabbyCodes.Patches.Player
{
    public class SoulPatch : ISyncedReference<bool>
    {
        public const string key = "Soul_Patch";
        private static ConfigEntry<bool> configValue;
        private static readonly Harmony harmony = new Harmony(key);
        private static readonly MethodInfo mOriginal = AccessTools.Method(typeof(PlayerData), nameof(PlayerData.TakeMP));
        private static readonly MethodInfo mOriginal2 = AccessTools.Method(typeof(PlayerData), nameof(PlayerData.TakeReserveMP));
        private static readonly MethodInfo mOriginal3 = AccessTools.Method(typeof(PlayerData), nameof(PlayerData.ClearMP));

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

            if (Get())
            {
                harmony.Patch(mOriginal, prefix: new HarmonyMethod(typeof(CommonPatches).GetMethod("Prefix_SkipOriginal")));
                harmony.Patch(mOriginal2, prefix: new HarmonyMethod(typeof(CommonPatches).GetMethod("Prefix_SkipOriginal")));
                harmony.Patch(mOriginal3, prefix: new HarmonyMethod(typeof(CommonPatches).GetMethod("Prefix_SkipOriginal")));
                PlayerData.instance.AddMPCharge(Constants.INFINITE_SOUL_CHARGE);
            }
            else
            {
                harmony.UnpatchSelf();
            }
        }
    }
}
