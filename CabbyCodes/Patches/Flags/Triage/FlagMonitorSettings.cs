using BepInEx.Configuration;

namespace CabbyCodes.Patches.Flags.Triage
{
    /// <summary>
    /// Centralized settings for Flag Monitor configuration
    /// </summary>
    public static class FlagMonitorSettings
    {
        // Configuration entries
        private static ConfigEntry<bool> showNewDiscoveries;
        private static ConfigEntry<bool> showChangedValues;
        private static ConfigEntry<bool> showSceneTransitions;
        private static ConfigEntry<bool> includePlayerDataFlags;
        private static ConfigEntry<bool> includeSceneFlags;

        // Public properties with default values
        public static bool ShowNewDiscoveries => showNewDiscoveries?.Value ?? true;
        public static bool ShowChangedValues => showChangedValues?.Value ?? true;
        public static bool ShowSceneTransitions => showSceneTransitions?.Value ?? true;
        public static bool IncludePlayerDataFlags => includePlayerDataFlags?.Value ?? true;
        public static bool IncludeSceneFlags => includeSceneFlags?.Value ?? true;

        /// <summary>
        /// Initializes all configuration entries for Flag Monitor settings
        /// </summary>
        public static void InitializeConfig()
        {
            showNewDiscoveries = CabbyCodesPlugin.configFile.Bind("FlagMonitor", "ShowNewDiscoveries", true,
                "Show notifications when new flags are discovered");
            
            showChangedValues = CabbyCodesPlugin.configFile.Bind("FlagMonitor", "ShowChangedValues", true,
                "Show notifications when flag values change");
            
            showSceneTransitions = CabbyCodesPlugin.configFile.Bind("FlagMonitor", "ShowSceneTransitions", true,
                "Show notifications when transitioning between scenes");
            
            includePlayerDataFlags = CabbyCodesPlugin.configFile.Bind("FlagMonitor", "IncludePlayerDataFlags", true,
                "Monitor and display PlayerData flag changes");
            
            includeSceneFlags = CabbyCodesPlugin.configFile.Bind("FlagMonitor", "IncludeSceneFlags", true,
                "Monitor and display Scene flag changes");
        }

        /// <summary>
        /// Updates the ShowNewDiscoveries setting
        /// </summary>
        public static void SetShowNewDiscoveries(bool value)
        {
            if (showNewDiscoveries != null)
            {
                showNewDiscoveries.Value = value;
            }
        }

        /// <summary>
        /// Updates the ShowChangedValues setting
        /// </summary>
        public static void SetShowChangedValues(bool value)
        {
            if (showChangedValues != null)
            {
                showChangedValues.Value = value;
            }
        }

        /// <summary>
        /// Updates the ShowSceneTransitions setting
        /// </summary>
        public static void SetShowSceneTransitions(bool value)
        {
            if (showSceneTransitions != null)
            {
                showSceneTransitions.Value = value;
            }
        }

        /// <summary>
        /// Updates the IncludePlayerDataFlags setting
        /// </summary>
        public static void SetIncludePlayerDataFlags(bool value)
        {
            if (includePlayerDataFlags != null)
            {
                includePlayerDataFlags.Value = value;
            }
        }

        /// <summary>
        /// Updates the IncludeSceneFlags setting
        /// </summary>
        public static void SetIncludeSceneFlags(bool value)
        {
            if (includeSceneFlags != null)
            {
                includeSceneFlags.Value = value;
            }
        }

        /// <summary>
        /// Checks if any notifications should be shown based on current settings
        /// </summary>
        public static bool ShouldShowNotifications()
        {
            return ShowNewDiscoveries || ShowChangedValues || ShowSceneTransitions;
        }

        /// <summary>
        /// Checks if PlayerData monitoring should be active
        /// </summary>
        public static bool ShouldMonitorPlayerData()
        {
            return IncludePlayerDataFlags && (ShowNewDiscoveries || ShowChangedValues);
        }

        /// <summary>
        /// Checks if Scene monitoring should be active
        /// </summary>
        public static bool ShouldMonitorScenes()
        {
            return IncludeSceneFlags && (ShowNewDiscoveries || ShowChangedValues);
        }
    }
} 