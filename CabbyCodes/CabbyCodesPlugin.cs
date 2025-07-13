using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.Mono;
using CabbyCodes.Patches;
using CabbyCodes.Patches.Achievements;
using CabbyCodes.Patches.Flags;
using CabbyCodes.Patches.Flags.Triage;
using CabbyCodes.Patches.Hunter;
using CabbyCodes.Patches.Inventory;
using CabbyCodes.Patches.Maps;
using CabbyCodes.Patches.Player;
using CabbyCodes.Patches.Charms;
using CabbyCodes.Patches.Settings;
using CabbyCodes.Patches.Teleport;
using CabbyMenu.UI;
using CabbyMenu.UI.Controls.InputField;

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
            cabbyMenu.RegisterCategory("Achievements", AchievementPatch.AddPanels);
            cabbyMenu.RegisterCategory("Settings", SettingsPatch.AddPanels);

            // Apply quick start patches
            QuickStartPatch.ApplyPatches();
            
            // Apply custom save/load patches
            CustomSaveLoadPatch.ApplyPatches();

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