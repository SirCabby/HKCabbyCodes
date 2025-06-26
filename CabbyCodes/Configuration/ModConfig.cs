using BepInEx.Configuration;
using System;

namespace CabbyCodes.Configuration
{
    /// <summary>
    /// Manages mod configuration and user preferences.
    /// </summary>
    public static class ModConfig
    {
        private static ConfigFile _config;

        // UI Settings
        public static ConfigEntry<bool> EnableInputValidation { get; private set; }
        public static ConfigEntry<bool> ShowDebugInfo { get; private set; }
        public static ConfigEntry<int> MenuPositionX { get; private set; }
        public static ConfigEntry<int> MenuPositionY { get; private set; }

        // Performance Settings
        public static ConfigEntry<bool> EnablePerformanceLogging { get; private set; }
        public static ConfigEntry<int> MaxLogEntries { get; private set; }

        // Gameplay Settings
        public static ConfigEntry<bool> EnableUndoRedo { get; private set; }
        public static ConfigEntry<int> UndoHistorySize { get; private set; }
        public static ConfigEntry<bool> ConfirmDestructiveChanges { get; private set; }

        /// <summary>
        /// Initializes the configuration system.
        /// </summary>
        /// <param name="configFile">The BepInEx config file.</param>
        public static void Initialize(ConfigFile configFile)
        {
            if (configFile == null)
            {
                CabbyCodesPlugin.BLogger.LogWarning("Config file is null, creating default configuration");
                // Create a default config file path if none provided
                string defaultConfigPath = System.IO.Path.Combine(
                    BepInEx.Paths.ConfigPath,
                    "CabbyCodes.cfg"
                );

                try
                {
                    _config = new ConfigFile(defaultConfigPath, true);
                    CabbyCodesPlugin.BLogger.LogInfo($"Created default config file at: {defaultConfigPath}");
                }
                catch (Exception ex)
                {
                    CabbyCodesPlugin.BLogger.LogError($"Failed to create default config file: {ex.Message}");
                    throw;
                }
            }
            else
            {
                _config = configFile;
            }

            InitializeUISettings();
            InitializePerformanceSettings();
            InitializeGameplaySettings();

            // Save the configuration to ensure the file is created with default values
            Save();

            CabbyCodesPlugin.BLogger.LogInfo("Mod configuration initialized successfully");
        }

        private static void InitializeUISettings()
        {
            EnableInputValidation = _config.Bind(
                "UI",
                "EnableInputValidation",
                true,
                "Enable input validation for all cheat values"
            );

            ShowDebugInfo = _config.Bind(
                "UI",
                "ShowDebugInfo",
                false,
                "Show debug information in the menu"
            );

            MenuPositionX = _config.Bind(
                "UI",
                "MenuPositionX",
                100,
                "Horizontal position of the menu (0-1920)"
            );

            MenuPositionY = _config.Bind(
                "UI",
                "MenuPositionY",
                100,
                "Vertical position of the menu (0-1080)"
            );
        }

        private static void InitializePerformanceSettings()
        {
            EnablePerformanceLogging = _config.Bind(
                "Performance",
                "EnablePerformanceLogging",
                false,
                "Enable performance logging for debugging"
            );

            MaxLogEntries = _config.Bind(
                "Performance",
                "MaxLogEntries",
                1000,
                "Maximum number of log entries to keep in memory"
            );
        }

        private static void InitializeGameplaySettings()
        {
            EnableUndoRedo = _config.Bind(
                "Gameplay",
                "EnableUndoRedo",
                true,
                "Enable undo/redo functionality for changes"
            );

            UndoHistorySize = _config.Bind(
                "Gameplay",
                "UndoHistorySize",
                10,
                "Number of changes to keep in undo history"
            );

            ConfirmDestructiveChanges = _config.Bind(
                "Gameplay",
                "ConfirmDestructiveChanges",
                true,
                "Show confirmation dialog for destructive changes"
            );
        }

        /// <summary>
        /// Saves the current configuration to disk.
        /// </summary>
        public static void Save()
        {
            try
            {
                _config?.Save();
                CabbyCodesPlugin.BLogger.LogDebug("Configuration saved successfully");
            }
            catch (Exception ex)
            {
                CabbyCodesPlugin.BLogger.LogException(ex, "ModConfig.Save");
            }
        }

        /// <summary>
        /// Reloads configuration from disk.
        /// </summary>
        public static void Reload()
        {
            try
            {
                _config?.Reload();
                CabbyCodesPlugin.BLogger.LogDebug("Configuration reloaded successfully");
            }
            catch (Exception ex)
            {
                CabbyCodesPlugin.BLogger.LogException(ex, "ModConfig.Reload");
            }
        }

        /// <summary>
        /// Resets all configuration to default values.
        /// </summary>
        public static void ResetToDefaults()
        {
            try
            {
                EnableInputValidation.Value = true;
                ShowDebugInfo.Value = false;
                MenuPositionX.Value = 100;
                MenuPositionY.Value = 100;
                EnablePerformanceLogging.Value = false;
                MaxLogEntries.Value = 1000;
                EnableUndoRedo.Value = true;
                UndoHistorySize.Value = 10;
                ConfirmDestructiveChanges.Value = true;

                Save();
                CabbyCodesPlugin.BLogger.LogInfo("Configuration reset to defaults");
            }
            catch (Exception ex)
            {
                CabbyCodesPlugin.BLogger.LogException(ex, "ModConfig.ResetToDefaults");
            }
        }
    }
}