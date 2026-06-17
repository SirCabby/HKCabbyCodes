#if !LUMAFLY
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
#if BEPINEX6
using BepInEx.Unity.Mono;
#endif
using CabbyCodes.Patches;
using CabbyCodes.Patches.Achievements;
using CabbyCodes.Patches.Flags;
using CabbyCodes.Patches.Flags.Triage;
using CabbyCodes.Patches.Maps;
using CabbyCodes.Patches.Player;
using CabbyCodes.Patches.Settings;
using CabbyCodes.Patches.SpriteViewer;
using CabbyCodes.Patches.Teleport;
using CabbyMenu.UI;
using CabbyMenu.UI.Controls.InputField;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace CabbyCodes
{
    /// <summary>
    /// Main plugin class for CabbyCodes mod. Handles initialization, configuration, and menu setup.
    /// </summary>
    [BepInPlugin(Constants.GUID, Constants.NAME, Constants.VERSION)]
    public class CabbyCodesPlugin : BaseUnityPlugin
    {
        /// <summary>
        /// Logger instance for the plugin.
        /// </summary>
        public static ManualLogSource BLogger;

        /// <summary>
        /// Main menu instance for the mod.
        /// </summary>
        public static CabbyMainMenu cabbyMenu;

        /// <summary>
        /// Configuration file instance.
        /// </summary>
        public static ConfigFile configFile;

        /// <summary>
        /// Game state provider for the menu system.
        /// </summary>
        private static GameStateProvider gameStateProvider;

        /// <summary>
        /// Loader for custom/quick start loads, persistent across scenes.
        /// </summary>
        private GameObject quickStartLoaderGo;

        /// <summary>
        /// Singleton accessor for external helpers.
        /// </summary>
        public static CabbyCodesPlugin Instance { get; private set; }

        /// <summary>
        /// Called when the plugin is loaded. Initializes the configuration system and logging.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Unity lifecycle method called by Unity engine")]
        private void Awake()
        {
            Instance = this;
            BLogger = Logger;
            BLogger.LogInfo("Plugin cabby.cabbycodes is loaded!");
            BLogger.LogInfo(string.Format("Config location: {0}", Config.ConfigFilePath));
            configFile = Config;

            QuickOpenHotkeyManager.Initialize(configFile);
            CustomHotkeyManager.Initialize(configFile);

            // Initialize flag monitor configuration early so it's available when panels are created
            FlagMonitorReference.InitializeConfig();
            FlagFileLoggingReference.InitializeConfig();
            FlagMonitorSettings.InitializeConfig();

            // Create persistent loader
            if (quickStartLoaderGo == null)
            {
                quickStartLoaderGo = new GameObject("QuickStartLoader");
                quickStartLoaderGo.AddComponent<QuickStartLoader>();
                DontDestroyOnLoad(quickStartLoaderGo);
            }
        }

        /// <summary>
        /// Called after Awake. Sets up the Unity Explorer and initializes the mod menu with all categories.
        /// Each patch group is isolated in a NoInlining method so that a TypeLoadException from a missing
        /// game type only kills that one method's JIT compilation, not the entire Start() method.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Unity lifecycle method called by Unity engine")]
        private void Start()
        {
            // Initialize the core menu system - if this fails, the mod cannot function
            try
            {
                InitializeMenuAndCategories();
            }
            catch (System.Exception ex)
            {
                BLogger.LogError("Failed to initialize CabbyCodes menu system: " + ex.Message);
                BLogger.LogError("Stack trace: " + ex.StackTrace);
                return;
            }

            // Apply patches with individual error handling so one failure doesn't prevent others.
            // Each wrapper is [MethodImpl(NoInlining)] so a TypeLoadException from a missing game type
            // is caught here instead of taking down the entire Start() method at JIT time.
            try
            {
                ApplyQuickStartPatches();
            }
            catch (System.Exception ex)
            {
                BLogger.LogError("Failed to apply QuickStartPatch: " + ex.Message);
            }

            try
            {
                ApplyCustomSaveLoadPatches();
            }
            catch (System.Exception ex)
            {
                BLogger.LogError("Failed to apply CustomSaveLoadPatch: " + ex.Message);
            }

            try
            {
                ApplyFlagMonitorPatches();
            }
            catch (System.Exception ex)
            {
                BLogger.LogError("Failed to apply FlagMonitorPatch: " + ex.Message);
            }

            try
            {
                InitializePlayerPatchPanels();
            }
            catch (System.Exception ex)
            {
                BLogger.LogError("Failed to initialize PlayerPatch panels: " + ex.Message);
            }

            BLogger.LogInfo("CabbyCodes menu initialized successfully");
        }

        /// <summary>
        /// Initializes the menu system and registers all categories.
        /// Isolated with NoInlining so that type resolution failures in referenced patch classes
        /// are caught by the caller's try-catch rather than failing Start()'s JIT compilation.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private void InitializeMenuAndCategories()
        {
            // Create the game state provider
            gameStateProvider = new GameStateProvider();

            // Initialize the menu with the game state provider
            cabbyMenu = new CabbyMainMenu(Constants.NAME, Constants.VERSION, gameStateProvider);
            
            // Set up menu state callback for heart piece and vessel fragment tracking
            InventoryPatch.SetMenuStateCallback((menuOpen) => {
                if (menuOpen)
                {
                    // Menu opened - initialize starting states
                    InventoryPatch.InitializeHeartPieceStartingState();
                    InventoryPatch.InitializeVesselFragmentStartingState();
                }
                else
                {
                    // Menu closed - check if reloads are needed
                    InventoryPatch.CheckHeartPieceReloadNeeded();
                    InventoryPatch.CheckVesselFragmentReloadNeeded();
                }
            });
            
            // Set up input field registration for all used types
            BaseInputFieldSync<int>.RegisterInputFieldSync = (inputFieldStatus) => {
                cabbyMenu.RegisterInputFieldSync(inputFieldStatus);
            };
            BaseInputFieldSync<float>.RegisterInputFieldSync = (inputFieldStatus) => {
                cabbyMenu.RegisterInputFieldSync(inputFieldStatus);
            };
            BaseInputFieldSync<string>.RegisterInputFieldSync = (inputFieldStatus) => {
                cabbyMenu.RegisterInputFieldSync(inputFieldStatus);
            };
            
            cabbyMenu.RegisterCategory("Player", PlayerPatch.AddPanels);
            cabbyMenu.RegisterCategory("Teleport", TeleportPatch.AddPanels);
            cabbyMenu.RegisterCategory("Inventory", InventoryPatch.AddPanels);
            cabbyMenu.RegisterCategory("Charms", CharmPatch.AddPanels);
            cabbyMenu.RegisterCategory("Maps", MapPatch.AddPanels);
            cabbyMenu.RegisterCategory("Grubs", GrubPatch.AddPanels);
            cabbyMenu.RegisterCategory("Hunter", HunterPatch.AddPanels);
            cabbyMenu.RegisterCategory("Flags", FlagsPatch.AddPanels);
            
            // Only register Sprite Viewer category when dev options are enabled
            bool initialDevOptionsState = configFile.Bind("Settings", "EnableDevOptions", Constants.DEFAULT_DEV_OPTIONS_ENABLED, 
                "Enable developer options and advanced features").Value;
            
            if (initialDevOptionsState)
            {
                cabbyMenu.RegisterCategory("Sprite Viewer", SpriteViewerPatch.AddPanels);
            }
            
            cabbyMenu.RegisterCategory("Achievements", AchievementPatch.AddPanels);
            cabbyMenu.RegisterCategory("Settings", SettingsPatch.AddPanels);

            // NOW get the last selected category from config AFTER all categories are registered
            var lastSelectedCategoryConfig = configFile.Bind("MainMenu", "LastSelectedCategory", 0, 
                "Last selected main menu category (0-based)");
            
            // Set up the callback to save category changes AFTER config binding
            cabbyMenu.SetCategorySelectedCallback((categoryIndex) => {
                lastSelectedCategoryConfig.Value = categoryIndex;
            });
            
            // Store the last selected category for deferred restoration
            int lastSelectedCategory = lastSelectedCategoryConfig.Value;
            
            if (lastSelectedCategory > 0 && lastSelectedCategory < cabbyMenu.GetRegisteredCategories())
            {
                // Defer the restoration until the dropdown is initialized
                cabbyMenu.SetDeferredCategoryRestoration(lastSelectedCategory);
            }
        }

        /// <summary>
        /// Applies quick start patches. Isolated to contain JIT TypeLoadExceptions.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ApplyQuickStartPatches()
        {
            QuickStartPatch.ApplyPatches();
        }

        /// <summary>
        /// Applies custom save/load patches. Isolated to contain JIT TypeLoadExceptions.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ApplyCustomSaveLoadPatches()
        {
            CustomSaveLoadPatch.ApplyPatches();
        }

        /// <summary>
        /// Applies flag monitor patches and initializes monitoring UI.
        /// Isolated to contain JIT TypeLoadExceptions (e.g., from changed Unity text types).
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ApplyFlagMonitorPatches()
        {
            FlagMonitorPatch.ApplyPatches();
            FlagMonitorPatch.InitializeSceneMonitoring();
            FlagMonitorReference.EnsurePanelExists();
            FlagFileLoggingReference.EnsureFileExists();
        }

        /// <summary>
        /// Initializes player patch panels. Isolated to contain JIT TypeLoadExceptions.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void InitializePlayerPatchPanels()
        {
            PlayerPatch.AddPanels();
        }

        /// <summary>
        /// Called every frame. Updates the mod menu and handles user input.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Unity lifecycle method called by Unity engine")]
        private void Update()
        {
            // Guard against Start() failure - if initialization didn't complete, skip Update
            if (gameStateProvider == null || cabbyMenu == null)
            {
                return;
            }

            // Track menu state changes for heart piece reload logic
            bool currentMenuState = gameStateProvider.ShouldShowMenu();
            if (currentMenuState != lastMenuState)
            {
                // Menu state changed - notify InventoryPatch
                if (InventoryPatch.onMenuStateChanged != null)
                {
                    InventoryPatch.onMenuStateChanged(currentMenuState);
                }
                lastMenuState = currentMenuState;
            }
            
            // Update Enter key state for custom save/load functionality
            CustomSaveLoadPatch.UpdateEnterKeyState();
            
            // Update Enter key state for teleport functionality
            TeleportPatch.UpdateEnterKeyState();
            
            cabbyMenu.Update();
            FlagMonitorReference.UpdatePanelVisibility();
            QuickOpenHotkeyManager.Update();
            CustomHotkeyManager.Update();
        }
        
        // Track the last known menu state
        private bool lastMenuState = false;
    }
}
#endif