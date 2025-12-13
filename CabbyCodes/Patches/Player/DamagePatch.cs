using CabbyMenu.SyncedReferences;
using BepInEx.Configuration;
using MonoMod.RuntimeDetour;
using System;
using System.Reflection;

namespace CabbyCodes.Patches.Player
{
    public class DamagePatch : ISyncedReference<bool>
    {
        public const string key = "Damage_Patch";
        private static ConfigEntry<bool> configValue;
        
        // Cache the TakeDamage method for invoking in our hook
        private static readonly MethodInfo mTakeDamage = typeof(HealthManager).GetMethod("TakeDamage", BindingFlags.NonPublic | BindingFlags.Instance);

        // MonoMod.RuntimeDetour hooks - unified for all builds
        private static Hook hookHit;

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
            if (hookHit == null)
            {
                // Get the Hit method with HitInstance parameter
                var hitMethod = typeof(HealthManager).GetMethod(nameof(HealthManager.Hit), 
                    BindingFlags.Public | BindingFlags.Instance, 
                    null, 
                    new Type[] { typeof(HitInstance) }, 
                    null);
                    
                hookHit = new Hook(
                    hitMethod,
                    typeof(DamagePatch).GetMethod(nameof(OnHit), BindingFlags.NonPublic | BindingFlags.Static)
                );
            }
        }

        private static void RemoveHooks()
        {
            hookHit?.Dispose();
            hookHit = null;
        }

        // Hook handler - custom damage logic
        private static void OnHit(Action<HealthManager, HitInstance> orig, HealthManager self, HitInstance hitInstance)
        {
            // Modify the hit to do massive damage
            hitInstance.DamageDealt = Constants.ONE_HIT_KILL_DAMAGE;
            hitInstance.IgnoreInvulnerable = true;

            if (!self.isDead)
            {
                FSMUtility.SendEventToGameObject(hitInstance.Source, "DEALT DAMAGE");
                mTakeDamage.Invoke(self, new object[] { hitInstance });
            }
            // Don't call orig() - we've handled the hit ourselves
        }
    }
}
