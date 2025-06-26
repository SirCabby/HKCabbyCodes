using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.Mono;
using CabbyCodes.Patches;
using CabbyCodes.Patches.Achievements;
using CabbyCodes.Patches.Flags;
using CabbyCodes.Patches.Hunter;
using CabbyCodes.Patches.Inventory;
using CabbyCodes.Patches.Maps;
using CabbyCodes.UI;
using CabbyCodes.Configuration;

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
        public static CabbyMenu cabbyMenu;

        /// <summary>
        /// Configuration file instance.
        /// </summary>
        public static ConfigFile configFile;

        /// <summary>
        /// Called when the plugin is loaded. Initializes the configuration system and logging.
        /// </summary>
        private void Awake()
        {
            BLogger = Logger;
            BLogger.LogInfo("Plugin cabby.cabbycodes is loaded!");
            BLogger.LogInfo("Config location: {0}", Config.ConfigFilePath);
            configFile = Config;

            // Initialize configuration system
            ModConfig.Initialize(configFile);
            SettingsManager.Initialize();
        }

        /// <summary>
        /// Called after Awake. Sets up the Unity Explorer and initializes the mod menu with all categories.
        /// </summary>
        private void Start()
        {
            UnityExplorer.ExplorerStandalone.CreateInstance();

            cabbyMenu = new CabbyMenu(Constants.NAME, Constants.VERSION);
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
            cabbyMenu.RegisterCategory("Debug", DebugPatch.AddPanels);

            BLogger.LogInfo("CabbyCodes menu initialized successfully");

            // Apply all settings
            SettingsManager.ApplyAllSettings();
        }

        /// <summary>
        /// Called every frame. Updates the mod menu and handles user input.
        /// </summary>
        private void Update()
        {
            cabbyMenu.Update();
        }
    }
}