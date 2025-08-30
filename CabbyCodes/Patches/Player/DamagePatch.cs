using CabbyMenu.SyncedReferences;
using System.Reflection;
using HarmonyLib;
using BepInEx.Configuration;

namespace CabbyCodes.Patches.Player
{
    public class DamagePatch : ISyncedReference<bool>
    {
        public const string key = "Damage_Patch";
        private static ConfigEntry<bool> configValue;
        private static readonly Harmony harmony = new Harmony(key);
        private static readonly MethodInfo mOriginal1 = AccessTools.Method(typeof(HealthManager), nameof(HealthManager.Hit), new System.Type[] { typeof(HitInstance) });
        private static readonly MethodInfo mOriginal2 = typeof(HealthManager).GetMethod("TakeDamage", BindingFlags.NonPublic | BindingFlags.Instance);

        /// <summary>
        /// Initializes the configuration entry.
        /// </summary>
        private static void InitializeConfig()
        {
            if (configValue == null)
            {
                configValue = CabbyCodesPlugin.configFile.Bind("Player", "OneHitKills", false, 
                    "Enable one-hit kills for enemies");
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
                harmony.Patch(mOriginal1, prefix: new HarmonyMethod(typeof(DamagePatch).GetMethod(nameof(Hit_Override), BindingFlags.Static | BindingFlags.Public)));
            }
            else
            {
                harmony.UnpatchSelf();
            }
        }

        //HealthManager.Hit();
        public static bool Hit_Override(HitInstance hitInstance, HealthManager __instance)
        {
            hitInstance.DamageDealt = Constants.ONE_HIT_KILL_DAMAGE;
            hitInstance.IgnoreInvulnerable = true;

            if (!__instance.isDead)
            {
                FSMUtility.SendEventToGameObject(hitInstance.Source, "DEALT DAMAGE");
                mOriginal2.Invoke(__instance, new object[] { hitInstance });
            }

            return false;
        }
    }
}
