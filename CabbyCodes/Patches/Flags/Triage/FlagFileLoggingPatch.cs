using CabbyMenu.SyncedReferences;
using UnityEngine;
using System.IO;
using BepInEx.Configuration;

namespace CabbyCodes.Patches.Flags.Triage
{
    public class FlagFileLoggingReference : ISyncedReference<bool>
    {
        private static FlagFileLoggingReference instance;
        public static FlagFileLoggingReference Instance => instance ?? (instance = new FlagFileLoggingReference());

        private bool isEnabled = false;
        private string logFilePath;

        /// <summary>
        /// Configuration entry for file logging enabled state.
        /// </summary>
        private static ConfigEntry<bool> fileLoggingEnabled;

        public bool IsEnabled => isEnabled;
        public string LogFilePath => logFilePath;

        /// <summary>
        /// Initializes the configuration entry for file logging settings.
        /// </summary>
        public static void InitializeConfig()
        {
            fileLoggingEnabled = CabbyCodesPlugin.configFile.Bind("FlagMonitor", "FileLogging", false, 
                "Enable logging flag changes to a file in CabbySaves folder");
        }

        /// <summary>
        /// Ensures the log file is created if file logging is enabled.
        /// This should be called after the configuration is loaded to ensure the file exists.
        /// </summary>
        public static void EnsureFileExists()
        {
            if (fileLoggingEnabled != null && fileLoggingEnabled.Value)
            {
                // Always create the file if config says it should be enabled
                string savesPath = Path.Combine(Application.persistentDataPath, "CabbySaves");
                
                if (!Directory.Exists(savesPath))
                {
                    Directory.CreateDirectory(savesPath);
                }
                
                instance.logFilePath = Path.Combine(savesPath, "flag_monitor.txt");
                
                // Check if file exists - if not, write header
                if (!File.Exists(instance.logFilePath))
                {
                    string header = $"Flag Monitor Log - Started at {System.DateTime.Now}\n" +
                                   "==========================================\n";
                    File.WriteAllText(instance.logFilePath, header);
                }
                else
                {
                    // Append session separator to existing file
                    string sessionHeader = $"\n\n--- New Session Started at {System.DateTime.Now} ---\n";
                    File.AppendAllText(instance.logFilePath, sessionHeader);
                }
                
                // Ensure the local state is synchronized
                instance.isEnabled = true;
            }
        }



        public bool Get()
        {
            // Ensure config is initialized
            if (fileLoggingEnabled == null)
            {
                InitializeConfig();
            }
            
            // Synchronize the local field with the config value
            isEnabled = fileLoggingEnabled.Value;
            return isEnabled;
        }

        public void Set(bool newValue)
        {
            // Ensure config is initialized
            if (fileLoggingEnabled == null)
            {
                InitializeConfig();
            }
            
            isEnabled = newValue;
            fileLoggingEnabled.Value = newValue;
            
            if (isEnabled)
            {
                // Create log file in CabbySaves folder
                string savesPath = Path.Combine(Application.persistentDataPath, "CabbySaves");
                if (!Directory.Exists(savesPath))
                {
                    Directory.CreateDirectory(savesPath);
                }
                logFilePath = Path.Combine(savesPath, "flag_monitor.txt");
                
                // Check if file exists - if not, write header
                if (!File.Exists(logFilePath))
                {
                    string header = $"Flag Monitor Log - Started at {System.DateTime.Now}\n" +
                                   "==========================================\n";
                    File.WriteAllText(logFilePath, header);
                }
                else
                {
                    // Append session separator to existing file
                    string sessionHeader = $"\n\n--- New Session Started at {System.DateTime.Now} ---\n";
                    File.AppendAllText(logFilePath, sessionHeader);
                }
                Debug.Log($"[Flag Monitor] File logging enabled: {logFilePath}");
            }
            else
            {
                Debug.Log("[Flag Monitor] File logging disabled");
                logFilePath = null;
            }
        }

        public void LogMessage(string message)
        {
            if (!isEnabled || string.IsNullOrEmpty(logFilePath)) return;
            
            // Check if we should log this type of message based on settings
            if (!ShouldLogMessageType(message))
            {
                return;
            }
            
            try
            {
                string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                string logEntry = $"[{timestamp}] {message}";
                File.AppendAllText(logFilePath, logEntry + System.Environment.NewLine);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[Flag Monitor] Failed to write to log file: {ex.Message}");
            }
        }

        /// <summary>
        /// Determines if a message should be logged based on current Flag Monitor settings
        /// </summary>
        private bool ShouldLogMessageType(string message)
        {
            // Import the FlagMonitorSettings to check configuration
            // We need to use reflection since this class doesn't have direct access to FlagMonitorSettings
            try
            {
                var settingsType = System.Type.GetType("CabbyCodes.Patches.Flags.Triage.FlagMonitorSettings");
                if (settingsType != null)
                {
                    // Scene transitions
                    if (message.StartsWith("[SceneChange]"))
                    {
                        var showSceneTransitions = (bool)settingsType.GetProperty("ShowSceneTransitions").GetValue(null);
                        return showSceneTransitions;
                    }
                    
                    // New discoveries
                    if (message.StartsWith("[NEW PLAYERDATA FLAG]") || 
                        message.StartsWith("[NEW SCENE FLAG") ||
                        message.StartsWith("[NEW SCENE DISCOVERED]") ||
                        message.StartsWith("[NEW BOSSSTATUE FLAG]"))
                    {
                        var showNewDiscoveries = (bool)settingsType.GetProperty("ShowNewDiscoveries").GetValue(null);
                        return showNewDiscoveries;
                    }
                    
                    // Value changes
                    if (message.StartsWith("[PlayerData]:") || message.StartsWith("[Scene:"))
                    {
                        var showChangedValues = (bool)settingsType.GetProperty("ShowChangedValues").GetValue(null);
                        return showChangedValues;
                    }
                }
            }
            catch
            {
                // If reflection fails, fall back to logging everything
                return true;
            }
            
            // Default to logging other types
            return true;
        }
    }
} 