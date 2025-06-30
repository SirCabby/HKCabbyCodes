using BepInEx.Configuration;
using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu;
using HarmonyLib;
using System.Reflection;
using UnityEngine;
using System.Collections;
using GlobalEnums;

namespace CabbyCodes.Patches.Settings
{
    /// <summary>
    /// Handles quick start settings including intro skip and automatic save loading.
    /// </summary>
    public class QuickStartPatch : ISyncedReference<bool>
    {
        /// <summary>
        /// Configuration key for quick start settings.
        /// </summary>
        private const string CONFIG_KEY = "QuickStart";

        /// <summary>
        /// Harmony instance for patching game methods.
        /// </summary>
        private static readonly Harmony harmony = new Harmony("cabby.quickstart");

        /// <summary>
        /// Configuration entry for enabling quick start.
        /// </summary>
        private static ConfigEntry<bool> quickStartEnabled;

        /// <summary>
        /// Configuration entry for the save slot to load.
        /// </summary>
        private static ConfigEntry<int> saveSlot;

        /// <summary>
        /// Configuration entry for skipping intro screens.
        /// </summary>
        private static ConfigEntry<bool> skipIntroScreens;

        /// <summary>
        /// Flag to track if patches have been applied.
        /// </summary>
        private static bool patchesApplied = false;

        /// <summary>
        /// Flag to track if we've already performed quick start actions.
        /// </summary>
        private static bool quickStartPerformed = false;

        /// <summary>
        /// Gets the current quick start enabled state.
        /// </summary>
        /// <returns>True if quick start is enabled, false otherwise.</returns>
        public bool Get()
        {
            return quickStartEnabled.Value;
        }

        /// <summary>
        /// Sets the quick start enabled state.
        /// </summary>
        /// <param name="value">The new enabled state.</param>
        public void Set(bool value)
        {
            quickStartEnabled.Value = value;
            CabbyCodesPlugin.BLogger.LogInfo(string.Format("Quick start enabled: {0}", value));
        }

        /// <summary>
        /// Gets the current save slot setting.
        /// </summary>
        /// <returns>The save slot number (1-4).</returns>
        public static int GetSaveSlot()
        {
            return saveSlot.Value;
        }

        /// <summary>
        /// Sets the save slot for quick loading.
        /// </summary>
        /// <param name="slot">The save slot number (1-4).</param>
        public static void SetSaveSlot(int slot)
        {
            slot = ValidationUtils.ValidateRange(slot, 1, 4, nameof(slot));
            saveSlot.Value = slot;
            CabbyCodesPlugin.BLogger.LogInfo(string.Format("Save slot set to: {0}", slot));
        }

        /// <summary>
        /// Gets the current intro skip setting.
        /// </summary>
        /// <returns>True if intro screens should be skipped, false otherwise.</returns>
        public static bool GetSkipIntroScreens()
        {
            return skipIntroScreens.Value;
        }

        /// <summary>
        /// Sets the intro skip setting.
        /// </summary>
        /// <param name="value">True to skip intro screens, false otherwise.</param>
        public static void SetSkipIntroScreens(bool value)
        {
            skipIntroScreens.Value = value;
            CabbyCodesPlugin.BLogger.LogInfo(string.Format("Skip intro screens: {0}", value));
        }

        /// <summary>
        /// Adds the quick start settings panel to the mod menu.
        /// </summary>
        public static void AddPanel()
        {
            // Add toggle for quick start
            TogglePanel quickStartToggle = new TogglePanel(new QuickStartPatch(), "Enable Quick Start");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(quickStartToggle);

            // Add input field for save slot
            InputFieldPanel<int> saveSlotPanel = new InputFieldPanel<int>(
                new SaveSlotReference(), 
                KeyCodeMap.ValidChars.Numeric, 
                CabbyMenu.Constants.DEFAULT_CHARACTER_LIMIT, 
                "Save Slot (1-4)");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(saveSlotPanel);

            // Add toggle for skipping intro screens
            TogglePanel skipIntroToggle = new TogglePanel(new SkipIntroReference(), "Skip Intro Screens");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(skipIntroToggle);
        }

        /// <summary>
        /// Applies Harmony patches for quick start functionality.
        /// </summary>
        public static void ApplyPatches()
        {
            if (patchesApplied) 
            {
                CabbyCodesPlugin.BLogger.LogInfo("QuickStart patches already applied, skipping...");
                return;
            }

            InitializeConfig();

            CabbyCodesPlugin.BLogger.LogInfo("=== Applying QuickStart Patches ===");
            CabbyCodesPlugin.BLogger.LogInfo(string.Format("Skip intro screens setting: {0}", skipIntroScreens.Value));
            CabbyCodesPlugin.BLogger.LogInfo(string.Format("Quick start enabled setting: {0}", quickStartEnabled.Value));
            CabbyCodesPlugin.BLogger.LogInfo(string.Format("Save slot setting: {0}", saveSlot.Value));

            // Apply patches based on settings
            if (skipIntroScreens.Value)
            {
                CabbyCodesPlugin.BLogger.LogInfo("Applying intro skip patches...");
                ApplyIntroSkipPatches();
            }
            else
            {
                CabbyCodesPlugin.BLogger.LogInfo("Intro skip patches not applied (setting disabled)");
            }

            if (quickStartEnabled.Value)
            {
                CabbyCodesPlugin.BLogger.LogInfo("Applying quick load patches...");
                ApplyQuickLoadPatches();
            }
            else
            {
                CabbyCodesPlugin.BLogger.LogInfo("Quick load patches not applied (setting disabled)");
            }

            patchesApplied = true;
            CabbyCodesPlugin.BLogger.LogInfo("=== QuickStart Patches Applied ===");
            CabbyCodesPlugin.BLogger.LogInfo(string.Format("Quick start enabled: {0}", quickStartEnabled.Value));
            CabbyCodesPlugin.BLogger.LogInfo(string.Format("Save slot: {0}", saveSlot.Value));
        }

        /// <summary>
        /// Initializes the configuration entries.
        /// </summary>
        private static void InitializeConfig()
        {
            quickStartEnabled = CabbyCodesPlugin.configFile.Bind(CONFIG_KEY, "EnableQuickStart", false, 
                "Enable automatic loading of a save slot when reaching the main menu");
            
            saveSlot = CabbyCodesPlugin.configFile.Bind(CONFIG_KEY, "SaveSlot", 1, 
                "The save slot to load when quick start is enabled (1-4)");
            
            skipIntroScreens = CabbyCodesPlugin.configFile.Bind(CONFIG_KEY, "SkipIntroScreens", false, 
                "Skip the intro splash screens and go directly to main menu");
        }

        /// <summary>
        /// Applies patches to skip intro screens.
        /// </summary>
        private static void ApplyIntroSkipPatches()
        {
            // Patch StartManager.Start to intercept intro scene loading
            MethodInfo startManagerStartMethod = typeof(StartManager).GetMethod("Start", BindingFlags.NonPublic | BindingFlags.Instance);
            if (startManagerStartMethod != null)
            {
                harmony.Patch(startManagerStartMethod, 
                    prefix: new HarmonyMethod(typeof(QuickStartPatch).GetMethod(nameof(StartManagerStartPrefix), BindingFlags.NonPublic | BindingFlags.Static)));
                CabbyCodesPlugin.BLogger.LogInfo("StartManager.Start patch applied successfully");
            }
            else
            {
                CabbyCodesPlugin.BLogger.LogWarning("StartManager.Start method not found");
            }
        }

        /// <summary>
        /// Applies patches for quick loading functionality.
        /// </summary>
        private static void ApplyQuickLoadPatches()
        {
            // Patch GameManager.Update to check for quick load opportunity
            MethodInfo gameManagerUpdateMethod = typeof(GameManager).GetMethod("Update", BindingFlags.NonPublic | BindingFlags.Instance);
            if (gameManagerUpdateMethod != null)
            {
                harmony.Patch(gameManagerUpdateMethod, 
                    postfix: new HarmonyMethod(typeof(QuickStartPatch).GetMethod(nameof(GameManagerUpdatePostfix), BindingFlags.NonPublic | BindingFlags.Static)));
                CabbyCodesPlugin.BLogger.LogInfo("GameManager.Update patch applied successfully");
            }
            else
            {
                CabbyCodesPlugin.BLogger.LogWarning("GameManager.Update method not found");
            }
        }

        /// <summary>
        /// Prefix method for StartManager.Start to skip intro screens.
        /// </summary>
        private static bool StartManagerStartPrefix(StartManager __instance)
        {
            if (!skipIntroScreens.Value) return true; // Continue with original method

            CabbyCodesPlugin.BLogger.LogInfo("StartManagerStartPrefix called - skipping intro screens");
            
            // Start the coroutine that skips intro and goes directly to main menu
            __instance.StartCoroutine(SkipIntroAndGoToMainMenu(__instance));
            
            return false; // Skip the original method
        }

        /// <summary>
        /// Coroutine to skip intro and go directly to main menu.
        /// </summary>
        private static IEnumerator SkipIntroAndGoToMainMenu(StartManager startManager)
        {
            CabbyCodesPlugin.BLogger.LogInfo("SkipIntroAndGoToMainMenu coroutine started");
            
            // Wait a short moment for any initialization
            yield return new WaitForSeconds(0.1f);
            
            CabbyCodesPlugin.BLogger.LogInfo("Loading main menu directly");
            
            // Load the main menu scene directly
            AsyncOperation loadOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Menu_Title");
            loadOperation.allowSceneActivation = true;
            yield return loadOperation;
            
            CabbyCodesPlugin.BLogger.LogInfo("Main menu loaded successfully");
        }

        /// <summary>
        /// Postfix method for GameManager.Update to handle quick loading.
        /// </summary>
        private static void GameManagerUpdatePostfix(GameManager __instance)
        {
            if (!quickStartEnabled.Value || quickStartPerformed) return;

            // Check if we're in the main menu state and ready for quick load
            if (__instance.gameState == GameState.MAIN_MENU && 
                __instance.ui != null)
            {
                CabbyCodesPlugin.BLogger.LogInfo("GameManagerUpdatePostfix - Main menu ready, attempting quick load");
                
                // Perform quick load
                PerformQuickLoad(__instance);
            }
        }

        /// <summary>
        /// Performs the quick load operation.
        /// </summary>
        private static void PerformQuickLoad(GameManager gameManager)
        {
            if (quickStartPerformed) return;
            
            quickStartPerformed = true;
            int slotNumber = saveSlot.Value;
            
            CabbyCodesPlugin.BLogger.LogInfo(string.Format("Performing quick load for save slot {0}", slotNumber));
            
            // Check if save file exists
            gameManager.HasSaveFile(slotNumber, (hasSave) =>
            {
                if (hasSave)
                {
                    CabbyCodesPlugin.BLogger.LogInfo(string.Format("Save slot {0} exists, loading game", slotNumber));
                    gameManager.LoadGameFromUI(slotNumber);
                }
                else
                {
                    CabbyCodesPlugin.BLogger.LogWarning(string.Format("Save slot {0} does not exist, quick load failed", slotNumber));
                }
            });
        }

        /// <summary>
        /// Reference class for save slot input field.
        /// </summary>
        private class SaveSlotReference : ISyncedReference<int>
        {
            public int Get() => GetSaveSlot();
            public void Set(int value) => SetSaveSlot(value);
        }

        /// <summary>
        /// Reference class for skip intro toggle.
        /// </summary>
        private class SkipIntroReference : ISyncedReference<bool>
        {
            public bool Get() => GetSkipIntroScreens();
            public void Set(bool value) => SetSkipIntroScreens(value);
        }
    }
} 