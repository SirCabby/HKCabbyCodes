using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using System.Reflection;
using HarmonyLib;
using CabbyMenu;

namespace CabbyCodes.Patches.Player
{
    public class InvulPatch : ISyncedReference<bool>
    {
        public const string key = "Invul_Patch";
        private static readonly BoxedReference<bool> value = CodeState.Get(key, false);
        private static readonly Harmony harmony = new Harmony(key);
        private static readonly MethodInfo mOriginal = AccessTools.Method(typeof(PlayerData), nameof(PlayerData.TakeHealth));
        private static readonly MethodInfo mOriginal2 = AccessTools.Method(typeof(PlayerData), nameof(PlayerData.WouldDie));
        private static bool sceneTransitionHandlerRegistered = false;

        public bool Get()
        {
            return value.Get();
        }

        public void Set(bool value)
        {
            InvulPatch.value.Set(value);

            if (Get())
            {
                harmony.Patch(mOriginal, prefix: new HarmonyMethod(typeof(CommonPatches).GetMethod("Prefix_SkipOriginal")));
                harmony.Patch(mOriginal2, prefix: new HarmonyMethod(typeof(CommonPatches).GetMethod("Prefix_SkipOriginal")));
                PlayerData.instance.isInvincible = true;
                PlayerData.instance.MaxHealth();
                
                // Register for scene transition events if not already registered
                if (!sceneTransitionHandlerRegistered)
                {
                    RegisterSceneTransitionHandler();
                }
            }
            else
            {
                harmony.UnpatchSelf();
                PlayerData.instance.isInvincible = false;
            }
        }

        /// <summary>
        /// Registers a handler for scene transition events to reapply invulnerability state.
        /// </summary>
        private static void RegisterSceneTransitionHandler()
        {
            if (GameManager._instance != null)
            {
                GameManager._instance.OnFinishedEnteringScene += OnFinishedEnteringSceneInvul;
                sceneTransitionHandlerRegistered = true;
                CabbyCodesPlugin.BLogger.LogDebug("Registered invulnerability scene transition handler");
            }
        }

        /// <summary>
        /// Event handler that fires after entering a new scene to reapply invulnerability state.
        /// </summary>
        private static void OnFinishedEnteringSceneInvul()
        {
            // Reapply invulnerability state if it's currently enabled
            if (value.Get())
            {
                PlayerData.instance.isInvincible = true;
                PlayerData.instance.MaxHealth();
                CabbyCodesPlugin.BLogger.LogDebug("Reapplied invulnerability state after scene transition");
            }
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new InvulPatch(), "Invulnerability"));
        }
    }
}
