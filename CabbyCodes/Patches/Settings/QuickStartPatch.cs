using BepInEx.Configuration;
using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using HarmonyLib;
using System.Reflection;
using UnityEngine;
using System.Collections;
using GlobalEnums;
using CabbyMenu.Utilities;
using CabbyCodes.SavedGames;

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
        private static ConfigEntry<bool> quickLoadEnabled;

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
        /// If set, triggers a custom file load after reaching the main menu.
        /// </summary>
        public static string CustomFileToLoad = null;

        /// <summary>
        /// Gets the current quick start enabled state.
        /// </summary>
        /// <returns>True if quick start is enabled, false otherwise.</returns>
        public bool Get()
        {
            return quickLoadEnabled.Value;
        }

        /// <summary>
        /// Sets the quick start enabled state.
        /// </summary>
        /// <param name="value">The new enabled state.</param>
        public void Set(bool value)
        {
            quickLoadEnabled.Value = value;
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
        }

        /// <summary>
        /// Adds the quick start settings panel to the mod menu.
        /// </summary>
        public static void AddPanel()
        {
            // Add toggle for skipping intro screens
            TogglePanel skipIntroToggle = new TogglePanel(new SkipIntroReference(), "Skip Intro Screens");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(skipIntroToggle);

            // Add combined quick load panel with toggle and save slot input
            QuickLoadPanel quickLoadPanel = new QuickLoadPanel(new QuickStartPatch(), new SaveSlotReference(), "Enable Quick Load");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(quickLoadPanel);
        }

        /// <summary>
        /// Applies Harmony patches for quick start functionality.
        /// </summary>
        public static void ApplyPatches()
        {
            if (patchesApplied) 
            {
                return;
            }

            InitializeConfig();

            // Apply patches based on settings
            if (skipIntroScreens.Value)
            {
                ApplyIntroSkipPatches();
            }

            // Always apply the quick load patch so custom file loads work
            ApplyQuickLoadPatches();

            patchesApplied = true;
        }

        /// <summary>
        /// Initializes the configuration entries.
        /// </summary>
        private static void InitializeConfig()
        {
            skipIntroScreens = CabbyCodesPlugin.configFile.Bind(CONFIG_KEY, "SkipIntroScreens", false, 
                "Skip the intro splash screens and go directly to main menu");

            quickLoadEnabled = CabbyCodesPlugin.configFile.Bind(CONFIG_KEY, "EnableQuickLoad", false, 
                "Enable automatic loading of a save slot when reaching the main menu");
            
            saveSlot = CabbyCodesPlugin.configFile.Bind(CONFIG_KEY, "SaveSlot", 1, 
                "The save slot to load when quick load is enabled (1-4)");
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
            }
        }

        /// <summary>
        /// Postfix method for GameManager.Update to handle quick loading.
        /// </summary>
        private static void GameManagerUpdatePostfix(GameManager __instance)
        {
            // Custom file quick load support
            if (!string.IsNullOrEmpty(CustomFileToLoad) && !quickStartPerformed)
            {
                if (__instance.gameState == GameState.MAIN_MENU && 
                    __instance.ui != null &&
                    __instance.ui.menuState == MainMenuState.MAIN_MENU &&
                    !__instance.ui.IsAnimatingMenus && !__instance.ui.IsFadingMenu)
                {
                    quickStartPerformed = true;
                    string fileToLoad = CustomFileToLoad;
                    CustomFileToLoad = null;
                    CabbyCodesPlugin.BLogger.LogDebug(string.Format("QuickStartPatch: Loading custom file '{0}' after main menu.", fileToLoad));
                    SavedGameManager.LoadCustomGame(fileToLoad, (success) => {
                        if (!success)
                        {
                            CabbyCodesPlugin.BLogger.LogWarning(string.Format("QuickStartPatch: Failed to load custom file '{0}'.", fileToLoad));
                        }
                    });
                }
                return;
            }
            if (!quickLoadEnabled.Value || quickStartPerformed) return;

            // Check if we're in the main menu state and ready for quick load
            if (__instance.gameState == GameState.MAIN_MENU && 
                __instance.ui != null &&
                __instance.ui.menuState == MainMenuState.MAIN_MENU)
            {
                // Wait a short moment for the menu to be fully ready
                if (!__instance.ui.IsAnimatingMenus && !__instance.ui.IsFadingMenu)
                {
                    // Set the flag immediately to prevent multiple calls
                    quickStartPerformed = true;
                    
                    // Perform quick load using the normal game flow
                    PerformQuickLoad(__instance);
                }
            }
        }

        /// <summary>
        /// Performs the quick load operation using the normal game flow.
        /// </summary>
        private static void PerformQuickLoad(GameManager gameManager)
        {
            int slotNumber = saveSlot.Value;
            
            // Check if save file exists
            gameManager.HasSaveFile(slotNumber, (hasSave) =>
            {
                if (hasSave)
                {
                    // Start a coroutine to ensure main menu initialization is complete and then load game
                    gameManager.StartCoroutine(WaitForMainMenuInitializationAndLoadGame(gameManager, slotNumber));
                }
                else
                {
                    // Reset the flag if save doesn't exist
                    quickStartPerformed = false;
                }
            });
        }

        /// <summary>
        /// Coroutine to wait for main menu initialization to complete, then load the game.
        /// </summary>
        private static IEnumerator WaitForMainMenuInitializationAndLoadGame(GameManager gameManager, int slotNumber)
        {
            // Wait for main menu to be fully initialized
            // This ensures all the normal main menu initialization steps have completed
            for (int attempt = 0; attempt < 30; attempt++) // Wait up to 30 frames
            {
                yield return null;
                
                // Check if main menu is fully ready
                if (gameManager.ui != null && 
                    gameManager.ui.menuState == MainMenuState.MAIN_MENU &&
                    !gameManager.ui.IsAnimatingMenus && 
                    !gameManager.ui.IsFadingMenu &&
                    gameManager.playerData != null)
                {
                    break;
                }
            }
            
            // Now that main menu is initialized, load the game directly
            gameManager.LoadGameFromUI(slotNumber);
            
            // Update the slot cache when loading a normal slot
            if (PlayerData.instance != null)
            {
                CustomSaveLoadPatch.LastLoadedSlot = PlayerData.instance.profileID;
            }
        }

        /// <summary>
        /// Prefix method for StartManager.Start to skip intro screens.
        /// </summary>
        private static bool StartManagerStartPrefix(StartManager __instance)
        {
            if (!skipIntroScreens.Value) return true; // Continue with original method

            // Start the coroutine that skips intro and goes directly to main menu
            __instance.StartCoroutine(SkipIntroAndGoToMainMenu());
            
            return false; // Skip the original method
        }

        /// <summary>
        /// Coroutine to skip intro and go directly to main menu.
        /// </summary>
        private static IEnumerator SkipIntroAndGoToMainMenu()
        {
            // Wait a short moment for any initialization
            yield return new WaitForSeconds(0.1f);
            
            // Load the main menu scene directly
            AsyncOperation loadOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Menu_Title");
            loadOperation.allowSceneActivation = true;
            yield return loadOperation;
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