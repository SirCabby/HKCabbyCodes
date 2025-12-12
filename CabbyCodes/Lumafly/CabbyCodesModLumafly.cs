#if LUMAFLY
using BepInEx.Configuration;
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
using Modding;
using System.IO;
using UnityEngine;

namespace CabbyCodes
{
    /// <summary>
    /// Main mod class for CabbyCodes when using Lumafly/HKAPI. Handles initialization and menu setup.
    /// </summary>
    public class CabbyCodesModLumafly : Mod
    {
        /// <summary>
        /// Main menu instance for the mod.
        /// </summary>
        public static CabbyMainMenu cabbyMenu;

        /// <summary>
        /// Game state provider for the menu system.
        /// </summary>
        private static GameStateProvider gameStateProvider;

        /// <summary>
        /// Singleton accessor for external helpers.
        /// </summary>
        public static CabbyCodesModLumafly Instance { get; private set; }

        /// <summary>
        /// Update handler MonoBehaviour.
        /// </summary>
        private GameObject updateHandlerGo;

        /// <summary>
        /// Loader for custom/quick start loads, persistent across scenes.
        /// </summary>
        private GameObject quickStartLoaderGo;

        /// <summary>
        /// Tracks the last menu state for change detection.
        /// </summary>
        private bool lastMenuState = false;

        /// <summary>
        /// Constructor required by HKAPI.
        /// </summary>
        public CabbyCodesModLumafly() : base(Constants.NAME)
        {
            Instance = this;
        }

        /// <summary>
        /// Returns the mod version.
        /// </summary>
        public override string GetVersion()
        {
            return Constants.VERSION;
        }

        /// <summary>
        /// Called when the mod is loaded. Initializes the mod menu and patches.
        /// </summary>
        public override void Initialize()
        {
            Log("Initializing CabbyCodes...");

            // Initialize the compatibility stub for CabbyCodesPlugin
            CabbyCodesPlugin.BLogger = new LumaflyLoggerWrapper(Constants.NAME);

            // Create ConfigFile for HKAPI (stores in Mods folder)
            string configPath = Path.Combine(
                Path.GetDirectoryName(typeof(CabbyCodesModLumafly).Assembly.Location),
                Constants.NAME.Replace(" ", "") + ".cfg"
            );
            CabbyCodesPlugin.configFile = new ConfigFile(configPath, true);
            CabbyCodesPlugin.BLogger.LogInfo(string.Format("Config location: {0}", configPath));

            // Initialize config-dependent features
            QuickOpenHotkeyManager.Initialize(CabbyCodesPlugin.configFile);
            FlagMonitorReference.InitializeConfig();
            FlagFileLoggingReference.InitializeConfig();
            FlagMonitorSettings.InitializeConfig();

            // Create persistent update handler
            if (updateHandlerGo == null)
            {
                updateHandlerGo = new GameObject("CabbyCodesUpdateHandler");
                updateHandlerGo.AddComponent<CabbyCodesUpdateHandler>();
                GameObject.DontDestroyOnLoad(updateHandlerGo);
            }

            // Create persistent loader for quick start
            if (quickStartLoaderGo == null)
            {
                quickStartLoaderGo = new GameObject("QuickStartLoader");
                quickStartLoaderGo.AddComponent<QuickStartLoader>();
                GameObject.DontDestroyOnLoad(quickStartLoaderGo);
            }

            // Create the game state provider
            gameStateProvider = new GameStateProvider();

            // Initialize the menu with the game state provider
            cabbyMenu = new CabbyMainMenu(Constants.NAME, Constants.VERSION, gameStateProvider);

            // Set up menu state callback for heart piece and vessel fragment tracking
            InventoryPatch.SetMenuStateCallback((menuOpen) =>
            {
                if (menuOpen)
                {
                    InventoryPatch.InitializeHeartPieceStartingState();
                    InventoryPatch.InitializeVesselFragmentStartingState();
                }
                else
                {
                    InventoryPatch.CheckHeartPieceReloadNeeded();
                    InventoryPatch.CheckVesselFragmentReloadNeeded();
                }
            });

            // Set up input field registration for all used types
            BaseInputFieldSync<int>.RegisterInputFieldSync = (inputFieldStatus) =>
            {
                cabbyMenu.RegisterInputFieldSync(inputFieldStatus);
            };
            BaseInputFieldSync<float>.RegisterInputFieldSync = (inputFieldStatus) =>
            {
                cabbyMenu.RegisterInputFieldSync(inputFieldStatus);
            };
            BaseInputFieldSync<string>.RegisterInputFieldSync = (inputFieldStatus) =>
            {
                cabbyMenu.RegisterInputFieldSync(inputFieldStatus);
            };

            // Register categories
            cabbyMenu.RegisterCategory("Player", PlayerPatch.AddPanels);
            cabbyMenu.RegisterCategory("Teleport", TeleportPatch.AddPanels);
            cabbyMenu.RegisterCategory("Inventory", InventoryPatch.AddPanels);
            cabbyMenu.RegisterCategory("Charms", CharmPatch.AddPanels);
            cabbyMenu.RegisterCategory("Maps", MapPatch.AddPanels);
            cabbyMenu.RegisterCategory("Grubs", GrubPatch.AddPanels);
            cabbyMenu.RegisterCategory("Hunter", HunterPatch.AddPanels);
            cabbyMenu.RegisterCategory("Flags", FlagsPatch.AddPanels);

            // Register dev options category if enabled
            bool initialDevOptionsState = CabbyCodesPlugin.configFile.Bind("Settings", "EnableDevOptions", Constants.DEFAULT_DEV_OPTIONS_ENABLED,
                "Enable developer options and advanced features").Value;

            if (initialDevOptionsState)
            {
                cabbyMenu.RegisterCategory("Sprite Viewer", SpriteViewerPatch.AddPanels);
            }

            cabbyMenu.RegisterCategory("Achievements", AchievementPatch.AddPanels);
            cabbyMenu.RegisterCategory("Settings", SettingsPatch.AddPanels);

            // Get the last selected category from config AFTER all categories are registered
            var lastSelectedCategoryConfig = CabbyCodesPlugin.configFile.Bind("MainMenu", "LastSelectedCategory", 0,
                "Last selected main menu category (0-based)");

            // Set up the callback to save category changes
            cabbyMenu.SetCategorySelectedCallback((categoryIndex) =>
            {
                lastSelectedCategoryConfig.Value = categoryIndex;
            });

            // Store the last selected category for deferred restoration
            int lastSelectedCategory = lastSelectedCategoryConfig.Value;

            if (lastSelectedCategory > 0 && lastSelectedCategory < cabbyMenu.GetRegisteredCategories())
            {
                cabbyMenu.SetDeferredCategoryRestoration(lastSelectedCategory);
            }

            // Apply quick start patches
            QuickStartPatch.ApplyPatches();

            // Apply custom save/load patches
            CustomSaveLoadPatch.ApplyPatches();

            // Apply flag monitor patches
            FlagMonitorPatch.ApplyPatches();

            // Initialize scene monitoring for flag monitor
            FlagMonitorPatch.InitializeSceneMonitoring();

            // Ensure flag monitor panel exists if it was previously enabled
            FlagMonitorReference.EnsurePanelExists();

            // Ensure file logging is set up if it was previously enabled
            FlagFileLoggingReference.EnsureFileExists();

            // Initialize player patches immediately on startup
            PlayerPatch.AddPanels();

            Log("CabbyCodes initialized successfully");
        }

        /// <summary>
        /// Called during the Update loop by the update handler.
        /// </summary>
        internal void OnUpdate()
        {
            if (gameStateProvider == null || cabbyMenu == null) return;

            // Track menu state changes for heart piece reload logic
            bool currentMenuState = gameStateProvider.ShouldShowMenu();
            if (currentMenuState != lastMenuState)
            {
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
        }
    }
}
#endif

