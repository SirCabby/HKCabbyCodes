using BepInEx.Configuration;
using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using HarmonyLib;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
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
        /// Reference to the current quick load popup for cleanup.
        /// </summary>
        private static CabbyMenu.UI.IPersistentPopup currentQuickLoadPopup;

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
            slot = ValidationUtils.ValidateRange(slot, 1, 4);
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
            // Add toggle for skipping intro screens using delegate reference
            TogglePanel skipIntroToggle = new TogglePanel(
                new DelegateReference<bool>(GetSkipIntroScreens, SetSkipIntroScreens),
                "Skip Intro Screens");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(skipIntroToggle);

            // Add combined quick load panel with toggle and save slot input using delegate reference
            QuickLoadPanel quickLoadPanel = new QuickLoadPanel(
                new QuickStartPatch(),
                new DelegateReference<int>(GetSaveSlot, SetSaveSlot),
                "Enable Quick Load");
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
                    SavedGameManager.LoadCustomGame(fileToLoad, (success) => {
                        if (success)
                        {
                            // Call OnGameLoadComplete after custom file load to restore menu state
                            GameReloadManager.OnGameLoadComplete();
                        }
                        else
                        {
                            CabbyCodesPlugin.BLogger.LogWarning(string.Format("QuickStartPatch: Failed to load custom file '{0}'.", fileToLoad));
                        }
                    });
                }
                return;
            }
            
            // Quick load support - show popup early and keep it visible
            if (quickLoadEnabled.Value && !quickStartPerformed && currentQuickLoadPopup == null)
            {
                // Show popup immediately when quick load is enabled and we haven't started yet
                if (__instance.gameState == GameState.MAIN_MENU && 
                    __instance.ui != null &&
                    __instance.ui.menuState == MainMenuState.MAIN_MENU &&
                    !__instance.ui.IsAnimatingMenus && !__instance.ui.IsFadingMenu)
                {
                    // Create and show loading popup immediately
                    var loadingPopup = CreateQuickLoadPopup(saveSlot.Value);
                    if (loadingPopup != null)
                    {
                        loadingPopup.Show();
                        currentQuickLoadPopup = loadingPopup;
                        CabbyCodesPlugin.BLogger.LogDebug("QuickStartPatch: Created and showed quick load popup");
                    }
                }
            }
            
            // Check if we're ready to perform the actual quick load
            if (quickLoadEnabled.Value && !quickStartPerformed && currentQuickLoadPopup != null)
            {
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
        }

        /// <summary>
        /// Performs the quick load operation using the normal game flow.
        /// </summary>
        private static void PerformQuickLoad(GameManager gameManager)
        {
            int slotNumber = saveSlot.Value;
            
            // Use the existing popup that was already created and shown
            var loadingPopup = currentQuickLoadPopup;
            
            // Check if save file exists
            gameManager.HasSaveFile(slotNumber, (hasSave) =>
            {
                if (hasSave)
                {
                    // Start a coroutine to ensure main menu initialization is complete and then load game
                    gameManager.StartCoroutine(WaitForMainMenuInitializationAndLoadGame(gameManager, slotNumber, loadingPopup));
                }
                else
                {
                    // Reset the flag if save doesn't exist
                    quickStartPerformed = false;
                    
                    // Hide popup if save doesn't exist
                    if (loadingPopup != null)
                    {
                        loadingPopup.Hide();
                        loadingPopup.Destroy();
                        currentQuickLoadPopup = null;
                    }
                }
            });
        }

        /// <summary>
        /// Creates a loading popup for quick load operations.
        /// </summary>
        /// <param name="slotNumber">The save slot number being loaded.</param>
        /// <returns>The created loading popup, or null if creation fails.</returns>
        private static CabbyMenu.UI.IPersistentPopup CreateQuickLoadPopup(int slotNumber)
        {
            try
            {
                // Create a scene-independent popup that can persist across scene changes
                var persistentGo = new GameObject("QuickLoadLoadingPopup");
                Object.DontDestroyOnLoad(persistentGo);
                
                // Add Canvas components for UI rendering
                Canvas canvas = persistentGo.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay; // This is crucial for visibility
                canvas.overrideSorting = true;
                canvas.sortingOrder = 1000; // Ensure popup appears on top of all UI
                persistentGo.AddComponent<GraphicRaycaster>();
                
                CanvasScaler scaler = persistentGo.AddComponent<CanvasScaler>();
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.referenceResolution = new Vector2(1920, 1080); // Standard reference resolution
                
                // Create the popup content
                CreateQuickLoadPopupContent(persistentGo, slotNumber);
                
                // Return a simple popup wrapper that manages the persistent GameObject
                return new CabbyMenu.UI.PersistentPopupWrapper(persistentGo);
            }
            catch (System.Exception ex)
            {
                CabbyCodesPlugin.BLogger.LogError(string.Format("Failed to create quick load popup: {0}", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Creates the visual content for the quick load popup.
        /// </summary>
        /// <param name="parent">The parent GameObject to attach content to.</param>
        /// <param name="slotNumber">The save slot number being loaded.</param>
        private static void CreateQuickLoadPopupContent(GameObject parent, int slotNumber)
        {
            // Create dimmed background
            GameObject background = DefaultControls.CreatePanel(new DefaultControls.Resources());
            background.name = "Popup Background";
            var backgroundImage = background.GetComponent<Image>();
            backgroundImage.color = new Color(0f, 0f, 0f, 0.6f);
            background.transform.SetParent(parent.transform, false);
            
            // Set background to fill the screen
            var backgroundRect = background.GetComponent<RectTransform>();
            backgroundRect.anchorMin = Vector2.zero;
            backgroundRect.anchorMax = Vector2.one;
            backgroundRect.sizeDelta = Vector2.zero;

            // Create main popup panel
            GameObject popupPanel = DefaultControls.CreatePanel(new DefaultControls.Resources());
            popupPanel.name = "Popup Panel";
            var panelImage = popupPanel.GetComponent<Image>();
            panelImage.color = new Color(0f, 0f, 0f, 1f);
            popupPanel.transform.SetParent(parent.transform, false);
            
            // Center the panel
            var panelRect = popupPanel.GetComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0.5f, 0.5f);
            panelRect.anchorMax = new Vector2(0.5f, 0.5f);
            panelRect.sizeDelta = new Vector2(400, 200);
            panelRect.anchoredPosition = Vector2.zero;

            // Add layout components
            VerticalLayoutGroup verticalLayout = popupPanel.AddComponent<VerticalLayoutGroup>();
            verticalLayout.padding = new RectOffset(20, 20, 20, 20);
            verticalLayout.spacing = 10;
            verticalLayout.childAlignment = TextAnchor.UpperCenter;
            verticalLayout.childControlWidth = true;
            verticalLayout.childControlHeight = true;
            verticalLayout.childForceExpandWidth = true;
            verticalLayout.childForceExpandHeight = false;

            // Create header
            GameObject headerContainer = DefaultControls.CreatePanel(new DefaultControls.Resources());
            headerContainer.name = "Header Container";
            var headerImage = headerContainer.GetComponent<Image>();
            headerImage.color = new Color(0.2f, 0.2f, 0.2f, 1f);
            headerContainer.transform.SetParent(popupPanel.transform, false);
            
            var headerLayout = headerContainer.AddComponent<LayoutElement>();
            headerLayout.preferredHeight = 60;
            headerLayout.minHeight = 50;

            // Header text
            GameObject headerText = new GameObject("Header Text");
            headerText.transform.SetParent(headerContainer.transform, false);
            var headerTextComponent = headerText.AddComponent<Text>();
            headerTextComponent.text = string.Format("Loading Game - Slot {0}", slotNumber);
            headerTextComponent.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            headerTextComponent.fontSize = 24;
            headerTextComponent.fontStyle = FontStyle.Bold;
            headerTextComponent.color = Color.white;
            headerTextComponent.alignment = TextAnchor.MiddleCenter;
            
            var headerTextRect = headerText.GetComponent<RectTransform>();
            headerTextRect.anchorMin = Vector2.zero;
            headerTextRect.anchorMax = Vector2.one;
            headerTextRect.sizeDelta = Vector2.zero;

            // Create message text
            GameObject messageText = new GameObject("Message Text");
            messageText.transform.SetParent(popupPanel.transform, false);
            var messageTextComponent = messageText.AddComponent<Text>();
            messageTextComponent.text = "Please Wait . . .";
            messageTextComponent.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            messageTextComponent.fontSize = 18;
            messageTextComponent.color = Color.white;
            messageTextComponent.alignment = TextAnchor.MiddleCenter;
            
            var messageLayout = messageText.AddComponent<LayoutElement>();
            messageLayout.preferredHeight = 80;
            messageLayout.minHeight = 60;
            
            var messageTextRect = messageText.GetComponent<RectTransform>();
            messageTextRect.anchorMin = Vector2.zero;
            messageTextRect.anchorMax = Vector2.one;
            messageTextRect.sizeDelta = Vector2.zero;
        }

        /// <summary>
        /// Coroutine to wait for main menu initialization to complete, then load the game.
        /// </summary>
        private static IEnumerator WaitForMainMenuInitializationAndLoadGame(GameManager gameManager, int slotNumber, CabbyMenu.UI.IPersistentPopup loadingPopup)
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

            // Wait until the player is actually back in the game and can move
            // This ensures the popup stays visible until the game is fully loaded and playable
            yield return new WaitForSeconds(1f); // Give the game time to load
            
            // Wait for the player to be in a playable state
            while (PlayerData.instance == null || gameManager.gameState != GameState.PLAYING)
            {
                yield return null;
            }
            
            // Wait a bit more to ensure everything is fully settled
            yield return new WaitForSeconds(0.5f);

            // Restore menu state after the game has finished loading
            GameReloadManager.OnGameLoadComplete();

            // Hide and destroy loading popup only when the player is actually back in the game
            if (loadingPopup != null)
            {
                loadingPopup.Hide();
                loadingPopup.Destroy();
                currentQuickLoadPopup = null;
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
    }
} 