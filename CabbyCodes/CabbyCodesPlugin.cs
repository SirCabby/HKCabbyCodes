using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.Mono;
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
using CabbyCodes.Patches.Flags.RoomFlags;
using CabbyCodes.SavedGames;
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
        /// Called when the plugin is loaded. Initializes the configuration system and logging.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Unity lifecycle method called by Unity engine")]
        private void Awake()
        {
            BLogger = Logger;
            BLogger.LogInfo("Plugin cabby.cabbycodes is loaded!");
            BLogger.LogInfo(string.Format("Config location: {0}", Config.ConfigFilePath));
            configFile = Config;

            // Initialize flag monitor configuration early so it's available when panels are created
            FlagMonitorReference.InitializeConfig();
            FlagFileLoggingReference.InitializeConfig();

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
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Unity lifecycle method called by Unity engine")]
        private void Start()
        {
            UnityExplorer.ExplorerStandalone.CreateInstance();

            // Create the game state provider
            gameStateProvider = new GameStateProvider();

            // Initialize the menu with the game state provider
            cabbyMenu = new CabbyMainMenu(Constants.NAME, Constants.VERSION, gameStateProvider);
            
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

            BLogger.LogInfo("CabbyCodes menu initialized successfully");
        }

        /// <summary>
        /// Called every frame. Updates the mod menu and handles user input.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Unity lifecycle method called by Unity engine")]
        private void Update()
        {
            cabbyMenu.Update();
            FlagMonitorReference.UpdatePanelVisibility();
        }
    }
}