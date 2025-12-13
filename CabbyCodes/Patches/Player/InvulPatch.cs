using CabbyMenu.SyncedReferences;
using BepInEx.Configuration;
using CabbyCodes.Flags;
using CabbyCodes.CheatState;
using MonoMod.RuntimeDetour;
using System;
using System.Reflection;

namespace CabbyCodes.Patches.Player
{
    public class InvulPatch : ISyncedReference<bool>, ICheatStateRestorable
    {
        public const string key = "Invul_Patch";
        private static ConfigEntry<bool> configValue;
        private static readonly FlagDef flag = FlagInstances.isInvincible;

        // MonoMod.RuntimeDetour hooks - unified for all builds
        private static Hook hookTakeHealth;
        private static Hook hookWouldDie;
        private static Hook hookTakeDamage;

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
                CheatStateManager.UnregisterRestorableCheat(this);
                RemoveHooks();
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
            ApplyHooks();
            FlagManager.SetBoolFlag(flag, true);
            PlayerData.instance?.MaxHealth();
        }

        private static void ApplyHooks()
        {
            if (hookTakeHealth == null)
            {
                hookTakeHealth = new Hook(
                    typeof(PlayerData).GetMethod(nameof(PlayerData.TakeHealth), BindingFlags.Public | BindingFlags.Instance),
                    typeof(InvulPatch).GetMethod(nameof(OnTakeHealth), BindingFlags.NonPublic | BindingFlags.Static)
                );
            }
            if (hookWouldDie == null)
            {
                hookWouldDie = new Hook(
                    typeof(PlayerData).GetMethod(nameof(PlayerData.WouldDie), BindingFlags.Public | BindingFlags.Instance),
                    typeof(InvulPatch).GetMethod(nameof(OnWouldDie), BindingFlags.NonPublic | BindingFlags.Static)
                );
            }
            if (hookTakeDamage == null)
            {
                hookTakeDamage = new Hook(
                    typeof(HeroController).GetMethod(nameof(HeroController.TakeDamage), BindingFlags.Public | BindingFlags.Instance),
                    typeof(InvulPatch).GetMethod(nameof(OnTakeDamage), BindingFlags.NonPublic | BindingFlags.Static)
                );
            }
        }

        private static void RemoveHooks()
        {
            hookTakeHealth?.Dispose();
            hookTakeHealth = null;
            hookWouldDie?.Dispose();
            hookWouldDie = null;
            hookTakeDamage?.Dispose();
            hookTakeDamage = null;
        }

        // Hook handlers - skip original by not calling orig()
        private static void OnTakeHealth(Action<PlayerData, int> orig, PlayerData self, int amount)
        {
            // Skip original - don't call orig()
        }

        private static bool OnWouldDie(Func<PlayerData, int, bool> orig, PlayerData self, int damage)
        {
            // Always return false - player would never die
            return false;
        }

        private static void OnTakeDamage(Action<HeroController, UnityEngine.GameObject, GlobalEnums.CollisionSide, int, int> orig, HeroController self, UnityEngine.GameObject go, GlobalEnums.CollisionSide damageSide, int damageAmount, int hazardType)
        {
            // Skip original - don't call orig()
        }
    }
}
