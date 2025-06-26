using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CabbyCodes.Configuration
{
    /// <summary>
    /// Manages real-time settings updates and provides validation for configuration changes.
    /// </summary>
    public static class SettingsManager
    {
        private static readonly Dictionary<string, Action<object>> _settingChangeCallbacks = new();
        private static bool _isInitialized = false;
        private static readonly string _backupDirectory = Path.Combine(BepInEx.Paths.ConfigPath, "CabbyCodes", "Backups");

        /// <summary>
        /// Event fired when any setting is changed.
        /// </summary>
        public static event Action<string, object> OnSettingChanged;

        /// <summary>
        /// Initializes the settings manager with change callbacks.
        /// </summary>
        public static void Initialize()
        {
            if (_isInitialized) return;

            RegisterSettingCallbacks();
            EnsureBackupDirectoryExists();
            _isInitialized = true;
            
            CabbyCodesPlugin.BLogger.LogInfo("Settings manager initialized");
        }

        private static void EnsureBackupDirectoryExists()
        {
            try
            {
                if (!Directory.Exists(_backupDirectory))
                {
                    Directory.CreateDirectory(_backupDirectory);
                    CabbyCodesPlugin.BLogger.LogDebug("Created backup directory: {0}", _backupDirectory);
                }
            }
            catch (Exception ex)
            {
                CabbyCodesPlugin.BLogger.LogWarning("Failed to create backup directory: {0}", ex.Message);
            }
        }

        private static void RegisterSettingCallbacks()
        {
            // UI Settings
            ModConfig.EnableInputValidation.SettingChanged += (sender, e) => 
                OnSettingChanged?.Invoke("EnableInputValidation", ModConfig.EnableInputValidation.Value);
            ModConfig.ShowDebugInfo.SettingChanged += (sender, e) => 
                OnSettingChanged?.Invoke("ShowDebugInfo", ModConfig.ShowDebugInfo.Value);
            ModConfig.MenuPositionX.SettingChanged += (sender, e) => 
            {
                OnSettingChanged?.Invoke("MenuPositionX", ModConfig.MenuPositionX.Value);
                ApplyMenuPosition(); // Apply position change immediately
            };
            ModConfig.MenuPositionY.SettingChanged += (sender, e) => 
            {
                OnSettingChanged?.Invoke("MenuPositionY", ModConfig.MenuPositionY.Value);
                ApplyMenuPosition(); // Apply position change immediately
            };
            // Performance Settings
            ModConfig.EnablePerformanceLogging.SettingChanged += (sender, e) => 
                OnSettingChanged?.Invoke("EnablePerformanceLogging", ModConfig.EnablePerformanceLogging.Value);
            ModConfig.MaxLogEntries.SettingChanged += (sender, e) => 
                OnSettingChanged?.Invoke("MaxLogEntries", ModConfig.MaxLogEntries.Value);
            // Gameplay Settings
            ModConfig.EnableUndoRedo.SettingChanged += (sender, e) => 
                OnSettingChanged?.Invoke("EnableUndoRedo", ModConfig.EnableUndoRedo.Value);
            ModConfig.UndoHistorySize.SettingChanged += (sender, e) => 
                OnSettingChanged?.Invoke("UndoHistorySize", ModConfig.UndoHistorySize.Value);
            ModConfig.ConfirmDestructiveChanges.SettingChanged += (sender, e) => 
                OnSettingChanged?.Invoke("ConfirmDestructiveChanges", ModConfig.ConfirmDestructiveChanges.Value);
        }

        /// <summary>
        /// Registers a callback for a specific setting change.
        /// </summary>
        /// <param name="settingName">Name of the setting to monitor.</param>
        /// <param name="callback">Callback to execute when the setting changes.</param>
        public static void RegisterSettingCallback(string settingName, Action<object> callback)
        {
            if (string.IsNullOrEmpty(settingName))
            {
                CabbyCodesPlugin.BLogger?.LogWarning("Setting name cannot be null or empty. Callback registration skipped.");
                return;
            }

            if (callback == null)
            {
                CabbyCodesPlugin.BLogger?.LogWarning("Callback cannot be null. Callback registration skipped.");
                return;
            }

            _settingChangeCallbacks[settingName] = callback;
            CabbyCodesPlugin.BLogger.LogDebug("Registered callback for setting: {0}", settingName);
        }

        /// <summary>
        /// Unregisters a callback for a specific setting.
        /// </summary>
        /// <param name="settingName">Name of the setting to unregister.</param>
        public static void UnregisterSettingCallback(string settingName)
        {
            if (_settingChangeCallbacks.Remove(settingName))
            {
                CabbyCodesPlugin.BLogger.LogDebug("Unregistered callback for setting: {0}", settingName);
            }
        }

        /// <summary>
        /// Validates a setting value and applies it if valid.
        /// </summary>
        /// <typeparam name="T">Type of the setting value.</typeparam>
        /// <param name="setting">The configuration entry to update.</param>
        /// <param name="newValue">The new value to set.</param>
        /// <param name="validator">Optional validation function.</param>
        /// <returns>True if the value was applied, false if validation failed.</returns>
        public static bool ValidateAndSetSetting<T>(ConfigEntry<T> setting, T newValue, Func<T, bool> validator = null)
        {
            try
            {
                if (validator != null && !validator(newValue))
                {
                    CabbyCodesPlugin.BLogger.LogWarning("Setting validation failed for {0}: {1}", setting.Definition.Key, newValue);
                    return false;
                }

                setting.Value = newValue;
                ModConfig.Save();
                
                CabbyCodesPlugin.BLogger.LogDebug("Setting updated: {0} = {1}", setting.Definition.Key, newValue);
                return true;
            }
            catch (Exception ex)
            {
                CabbyCodesPlugin.BLogger.LogException(ex, $"ValidateAndSetSetting for {setting.Definition.Key}");
                return false;
            }
        }

        /// <summary>
        /// Gets a setting value with type safety.
        /// </summary>
        /// <typeparam name="T">Type of the setting value.</typeparam>
        /// <param name="setting">The configuration entry.</param>
        /// <param name="defaultValue">Default value if setting is null or invalid.</param>
        /// <returns>The setting value or default value.</returns>
        public static T GetSettingValue<T>(ConfigEntry<T> setting, T defaultValue = default(T))
        {
            try
            {
                return setting != null ? setting.Value : defaultValue;
            }
            catch (Exception ex)
            {
                CabbyCodesPlugin.BLogger.LogException(ex, $"GetSettingValue for {setting?.Definition.Key}");
                return defaultValue;
            }
        }

        /// <summary>
        /// Applies all current settings to the mod.
        /// </summary>
        public static void ApplyAllSettings()
        {
            try
            {
                // Apply UI settings
                ApplyUISettings();
                
                // Apply performance settings
                ApplyPerformanceSettings();
                
                // Apply gameplay settings
                ApplyGameplaySettings();
                
                CabbyCodesPlugin.BLogger.LogInfo("All settings applied successfully");
            }
            catch (Exception ex)
            {
                CabbyCodesPlugin.BLogger.LogException(ex, "ApplyAllSettings");
            }
        }

        private static void ApplyUISettings()
        {
            // Apply menu position if needed
            if (ModConfig.MenuPositionX.Value != 100 || ModConfig.MenuPositionY.Value != 100)
            {
                ApplyMenuPosition();
                CabbyCodesPlugin.BLogger.LogDebug("Menu position: ({0}, {1})", 
                    ModConfig.MenuPositionX.Value, ModConfig.MenuPositionY.Value);
            }
        }

        private static void ApplyMenuPosition()
        {
            try
            {
                if (CabbyCodesPlugin.cabbyMenu != null)
                {
                    // Get the root canvas GameObject
                    var rootCanvas = UnityEngine.GameObject.Find("CC Root Canvas");
                    if (rootCanvas != null)
                    {
                        // Calculate normalized position (0-1 range)
                        float normalizedX = ModConfig.MenuPositionX.Value / 1920f;
                        float normalizedY = ModConfig.MenuPositionY.Value / 1080f;
                        
                        // Clamp values to valid range
                        normalizedX = Math.Max(0f, Math.Min(1f, normalizedX));
                        normalizedY = Math.Max(0f, Math.Min(1f, normalizedY));
                        
                        // Apply position to the menu button
                        var menuButton = rootCanvas.transform.Find("Open Menu Button");
                        if (menuButton != null)
                        {
                            var rectTransform = menuButton.GetComponent<UnityEngine.RectTransform>();
                            if (rectTransform != null)
                            {
                                rectTransform.anchorMin = new UnityEngine.Vector2(normalizedX, normalizedY);
                                rectTransform.anchorMax = new UnityEngine.Vector2(normalizedX + 0.05f, normalizedY + 0.03f);
                                rectTransform.anchoredPosition = UnityEngine.Vector2.zero;
                                
                                CabbyCodesPlugin.BLogger.LogDebug("Menu position applied: ({0}, {1})", normalizedX, normalizedY);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CabbyCodesPlugin.BLogger.LogException(ex, "ApplyMenuPosition");
            }
        }

        private static void ApplyPerformanceSettings()
        {
            // Apply performance logging setting
            if (ModConfig.EnablePerformanceLogging.Value)
            {
                CabbyCodesPlugin.BLogger.LogDebug("Performance logging enabled");
            }
        }

        private static void ApplyGameplaySettings()
        {
            // Apply undo/redo setting
            if (ModConfig.EnableUndoRedo.Value)
            {
                CabbyCodesPlugin.BLogger.LogDebug("Undo/Redo functionality enabled");
            }
        }

        /// <summary>
        /// Creates a backup of current settings.
        /// </summary>
        /// <returns>True if backup was created successfully.</returns>
        public static bool CreateBackup()
        {
            try
            {
                EnsureBackupDirectoryExists();
                
                // Create backup filename with timestamp
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string backupFileName = $"CabbyCodes_Backup_{timestamp}.cfg";
                string backupPath = Path.Combine(_backupDirectory, backupFileName);
                
                // Create backup content in INI format
                var backupContent = new StringBuilder();
                backupContent.AppendLine("# CabbyCodes Settings Backup");
                backupContent.AppendLine($"# Created: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                backupContent.AppendLine($"# Version: 1.0");
                backupContent.AppendLine();
                
                // UI Settings
                backupContent.AppendLine("[UI]");
                backupContent.AppendLine($"EnableInputValidation = {ModConfig.EnableInputValidation.Value}");
                backupContent.AppendLine($"ShowDebugInfo = {ModConfig.ShowDebugInfo.Value}");
                backupContent.AppendLine($"MenuPositionX = {ModConfig.MenuPositionX.Value}");
                backupContent.AppendLine($"MenuPositionY = {ModConfig.MenuPositionY.Value}");
                backupContent.AppendLine();
                
                // Performance Settings
                backupContent.AppendLine("[Performance]");
                backupContent.AppendLine($"EnablePerformanceLogging = {ModConfig.EnablePerformanceLogging.Value}");
                backupContent.AppendLine($"MaxLogEntries = {ModConfig.MaxLogEntries.Value}");
                backupContent.AppendLine();
                
                // Gameplay Settings
                backupContent.AppendLine("[Gameplay]");
                backupContent.AppendLine($"EnableUndoRedo = {ModConfig.EnableUndoRedo.Value}");
                backupContent.AppendLine($"UndoHistorySize = {ModConfig.UndoHistorySize.Value}");
                backupContent.AppendLine($"ConfirmDestructiveChanges = {ModConfig.ConfirmDestructiveChanges.Value}");
                
                // Save backup
                File.WriteAllText(backupPath, backupContent.ToString());
                
                // Also create a copy of the original config file
                string configBackupPath = Path.Combine(_backupDirectory, $"Original_Config_Backup_{timestamp}.cfg");
                if (File.Exists(CabbyCodesPlugin.configFile.ConfigFilePath))
                {
                    File.Copy(CabbyCodesPlugin.configFile.ConfigFilePath, configBackupPath);
                }
                
                CabbyCodesPlugin.BLogger.LogInfo("Settings backup created: {0}", backupPath);
                return true;
            }
            catch (Exception ex)
            {
                CabbyCodesPlugin.BLogger.LogException(ex, "CreateBackup");
                return false;
            }
        }

        /// <summary>
        /// Restores settings from backup.
        /// </summary>
        /// <param name="backupFileName">Name of the backup file to restore from. If null, uses the most recent backup.</param>
        /// <returns>True if restore was successful.</returns>
        public static bool RestoreFromBackup(string backupFileName = null)
        {
            try
            {
                if (!Directory.Exists(_backupDirectory))
                {
                    CabbyCodesPlugin.BLogger.LogWarning("Backup directory does not exist");
                    return false;
                }
                
                // Find backup file
                string backupPath;
                if (string.IsNullOrEmpty(backupFileName))
                {
                    // Find most recent backup
                    var backupFiles = Directory.GetFiles(_backupDirectory, "CabbyCodes_Backup_*.cfg");
                    if (backupFiles.Length == 0)
                    {
                        CabbyCodesPlugin.BLogger.LogWarning("No backup files found");
                        return false;
                    }
                    
                    // Sort by creation time and get the most recent
                    Array.Sort(backupFiles, (a, b) => File.GetCreationTime(b).CompareTo(File.GetCreationTime(a)));
                    backupPath = backupFiles[0];
                }
                else
                {
                    backupPath = Path.Combine(_backupDirectory, backupFileName);
                    if (!File.Exists(backupPath))
                    {
                        CabbyCodesPlugin.BLogger.LogWarning("Backup file not found: {0}", backupPath);
                        return false;
                    }
                }
                
                // Read backup file
                string[] lines = File.ReadAllLines(backupPath);
                string currentSection = "";
                
                foreach (string line in lines)
                {
                    string trimmedLine = line.Trim();
                    
                    // Skip comments and empty lines
                    if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith("#"))
                        continue;
                    
                    // Check for section headers
                    if (trimmedLine.StartsWith("[") && trimmedLine.EndsWith("]"))
                    {
                        currentSection = trimmedLine.Substring(1, trimmedLine.Length - 2);
                        continue;
                    }
                    
                    // Parse key-value pairs
                    int equalsIndex = trimmedLine.IndexOf('=');
                    if (equalsIndex > 0)
                    {
                        string key = trimmedLine.Substring(0, equalsIndex).Trim();
                        string value = trimmedLine.Substring(equalsIndex + 1).Trim();
                        
                        // Restore settings based on section and key
                        RestoreSetting(currentSection, key, value);
                    }
                }
                
                // Save the restored settings
                ModConfig.Save();
                
                // Apply the restored settings
                ApplyAllSettings();
                
                CabbyCodesPlugin.BLogger.LogInfo("Settings restored from backup: {0}", backupPath);
                return true;
            }
            catch (Exception ex)
            {
                CabbyCodesPlugin.BLogger.LogException(ex, "RestoreFromBackup");
                return false;
            }
        }

        private static void RestoreSetting(string section, string key, string value)
        {
            try
            {
                switch (section.ToLower())
                {
                    case "ui":
                        switch (key.ToLower())
                        {
                            case "enableinputvalidation":
                                if (bool.TryParse(value, out bool enableInputValidation))
                                    ModConfig.EnableInputValidation.Value = enableInputValidation;
                                break;
                            case "showdebuginfo":
                                if (bool.TryParse(value, out bool showDebugInfo))
                                    ModConfig.ShowDebugInfo.Value = showDebugInfo;
                                break;
                            case "menupositionx":
                                if (int.TryParse(value, out int menuPosX))
                                    ModConfig.MenuPositionX.Value = menuPosX;
                                break;
                            case "menupositiony":
                                if (int.TryParse(value, out int menuPosY))
                                    ModConfig.MenuPositionY.Value = menuPosY;
                                break;
                        }
                        break;
                        
                    case "performance":
                        switch (key.ToLower())
                        {
                            case "enableperformancelogging":
                                if (bool.TryParse(value, out bool enablePerfLogging))
                                    ModConfig.EnablePerformanceLogging.Value = enablePerfLogging;
                                break;
                            case "maxlogentries":
                                if (int.TryParse(value, out int maxLogEntries))
                                    ModConfig.MaxLogEntries.Value = maxLogEntries;
                                break;
                        }
                        break;
                        
                    case "gameplay":
                        switch (key.ToLower())
                        {
                            case "enableundoredo":
                                if (bool.TryParse(value, out bool enableUndoRedo))
                                    ModConfig.EnableUndoRedo.Value = enableUndoRedo;
                                break;
                            case "undohistorysize":
                                if (int.TryParse(value, out int undoHistorySize))
                                    ModConfig.UndoHistorySize.Value = undoHistorySize;
                                break;
                            case "confirmdestructivechanges":
                                if (bool.TryParse(value, out bool confirmChanges))
                                    ModConfig.ConfirmDestructiveChanges.Value = confirmChanges;
                                break;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                CabbyCodesPlugin.BLogger.LogWarning("Failed to restore setting {0}.{1}: {2}", section, key, ex.Message);
            }
        }

        /// <summary>
        /// Gets a list of available backup files.
        /// </summary>
        /// <returns>Array of backup file names.</returns>
        public static string[] GetAvailableBackups()
        {
            try
            {
                if (!Directory.Exists(_backupDirectory))
                    return new string[0];
                
                var backupFiles = Directory.GetFiles(_backupDirectory, "CabbyCodes_Backup_*.cfg");
                Array.Sort(backupFiles, (a, b) => File.GetCreationTime(b).CompareTo(File.GetCreationTime(a)));
                
                return Array.ConvertAll(backupFiles, Path.GetFileName);
            }
            catch (Exception ex)
            {
                CabbyCodesPlugin.BLogger.LogException(ex, "GetAvailableBackups");
                return new string[0];
            }
        }

        /// <summary>
        /// Deletes a backup file.
        /// </summary>
        /// <param name="backupFileName">Name of the backup file to delete.</param>
        /// <returns>True if deletion was successful.</returns>
        public static bool DeleteBackup(string backupFileName)
        {
            try
            {
                string backupPath = Path.Combine(_backupDirectory, backupFileName);
                if (File.Exists(backupPath))
                {
                    File.Delete(backupPath);
                    CabbyCodesPlugin.BLogger.LogInfo("Backup deleted: {0}", backupFileName);
                    return true;
                }
                else
                {
                    CabbyCodesPlugin.BLogger.LogWarning("Backup file not found: {0}", backupFileName);
                    return false;
                }
            }
            catch (Exception ex)
            {
                CabbyCodesPlugin.BLogger.LogException(ex, "DeleteBackup");
                return false;
            }
        }

        /// <summary>
        /// Reads all configuration keys from a specific section in the config file.
        /// </summary>
        /// <param name="sectionName">The name of the section to read keys from.</param>
        /// <returns>A list of configuration keys found in the specified section.</returns>
        public static List<string> GetConfigKeys(string sectionName)
        {
            List<string> result = new();
            try
            {
                if (string.IsNullOrEmpty(sectionName))
                {
                    CabbyCodesPlugin.BLogger.LogWarning("Section name cannot be null or empty");
                    return result;
                }

                string filePath = Path.GetFullPath(CabbyCodesPlugin.configFile.ConfigFilePath);
                if (!File.Exists(filePath))
                {
                    CabbyCodesPlugin.BLogger.LogWarning("Config file not found: {0}", filePath);
                    return result;
                }

                string[] lines = File.ReadAllLines(filePath);
                string currentSection = "";
                
                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i].Trim();
                    
                    // Skip comments and empty lines
                    if (string.IsNullOrEmpty(line) || line.StartsWith("#"))
                    {
                        continue;
                    }

                    // Check for section headers
                    if (line.StartsWith("[") && line.EndsWith("]"))
                    {
                        currentSection = line.Substring(1, line.Length - 2);
                        continue;
                    }

                    // Only process lines in the target section
                    if (currentSection != sectionName) continue;

                    // Parse key-value pairs
                    string[] keyValue = line.Split(new char[1] { '=' }, 2);
                    if (keyValue.Length == 2)
                    {
                        result.Add(keyValue[0].Trim());
                    }
                }

                CabbyCodesPlugin.BLogger.LogDebug("Found {0} keys in section '{1}'", result.Count, sectionName);
            }
            catch (Exception ex)
            {
                CabbyCodesPlugin.BLogger.LogException(ex, $"GetConfigKeys for section '{sectionName}'");
            }

            return result;
        }

        /// <summary>
        /// Gets a configuration definition for a specific section and key.
        /// </summary>
        /// <param name="sectionName">The name of the section.</param>
        /// <param name="key">The configuration key.</param>
        /// <returns>The ConfigDefinition if found, null otherwise.</returns>
        public static ConfigDefinition GetConfigDefinition(string sectionName, string key)
        {
            try
            {
                if (string.IsNullOrEmpty(sectionName))
                {
                    CabbyCodesPlugin.BLogger.LogWarning("Section name cannot be null or empty");
                    return null;
                }

                if (string.IsNullOrEmpty(key))
                {
                    CabbyCodesPlugin.BLogger.LogWarning("Key cannot be null or empty");
                    return null;
                }

                foreach (ConfigDefinition def in CabbyCodesPlugin.configFile.Keys)
                {
                    if (def.Section == sectionName && def.Key == key)
                    {
                        CabbyCodesPlugin.BLogger.LogDebug("Found ConfigDefinition for {0}.{1}", sectionName, key);
                        return def;
                    }
                }

                CabbyCodesPlugin.BLogger.LogDebug("ConfigDefinition not found for {0}.{1}", sectionName, key);
                return null;
            }
            catch (Exception ex)
            {
                CabbyCodesPlugin.BLogger.LogException(ex, $"GetConfigDefinition for {sectionName}.{key}");
                return null;
            }
        }

        /// <summary>
        /// Gets or creates a configuration entry for a specific section and key.
        /// </summary>
        /// <typeparam name="T">Type of the configuration value.</typeparam>
        /// <param name="sectionName">The name of the section.</param>
        /// <param name="key">The configuration key.</param>
        /// <param name="defaultValue">Default value if the entry doesn't exist.</param>
        /// <param name="description">Optional description for the configuration entry.</param>
        /// <returns>The ConfigEntry for the specified section and key.</returns>
        public static ConfigEntry<T> GetOrCreateConfigEntry<T>(string sectionName, string key, T defaultValue, ConfigDescription description = null)
        {
            try
            {
                if (string.IsNullOrEmpty(sectionName))
                {
                    CabbyCodesPlugin.BLogger.LogWarning("Section name cannot be null or empty");
                    return null;
                }

                if (string.IsNullOrEmpty(key))
                {
                    CabbyCodesPlugin.BLogger.LogWarning("Key cannot be null or empty");
                    return null;
                }

                // Try to get existing entry first
                ConfigDefinition def = new(sectionName, key);
                if (CabbyCodesPlugin.configFile.TryGetEntry(def, out ConfigEntry<T> existingEntry))
                {
                    CabbyCodesPlugin.BLogger.LogDebug("Found existing ConfigEntry for {0}.{1}", sectionName, key);
                    return existingEntry;
                }

                // Create new entry if it doesn't exist
                ConfigEntry<T> newEntry = CabbyCodesPlugin.configFile.Bind(def, defaultValue, description);
                CabbyCodesPlugin.BLogger.LogDebug("Created new ConfigEntry for {0}.{1} with default value {2}", sectionName, key, defaultValue);
                return newEntry;
            }
            catch (Exception ex)
            {
                CabbyCodesPlugin.BLogger.LogException(ex, $"GetOrCreateConfigEntry for {sectionName}.{key}");
                return null;
            }
        }

        /// <summary>
        /// Gets a configuration entry value with type safety and default fallback.
        /// </summary>
        /// <typeparam name="T">Type of the configuration value.</typeparam>
        /// <param name="sectionName">The name of the section.</param>
        /// <param name="key">The configuration key.</param>
        /// <param name="defaultValue">Default value if the entry doesn't exist or is invalid.</param>
        /// <returns>The configuration value or default value.</returns>
        public static T GetConfigValue<T>(string sectionName, string key, T defaultValue = default(T))
        {
            try
            {
                ConfigEntry<T> entry = GetOrCreateConfigEntry(sectionName, key, defaultValue);
                return GetSettingValue(entry, defaultValue);
            }
            catch (Exception ex)
            {
                CabbyCodesPlugin.BLogger.LogException(ex, $"GetConfigValue for {sectionName}.{key}");
                return defaultValue;
            }
        }

        /// <summary>
        /// Sets a configuration entry value with validation.
        /// </summary>
        /// <typeparam name="T">Type of the configuration value.</typeparam>
        /// <param name="sectionName">The name of the section.</param>
        /// <param name="key">The configuration key.</param>
        /// <param name="newValue">The new value to set.</param>
        /// <param name="validator">Optional validation function.</param>
        /// <returns>True if the value was applied, false if validation failed.</returns>
        public static bool SetConfigValue<T>(string sectionName, string key, T newValue, Func<T, bool> validator = null)
        {
            try
            {
                ConfigEntry<T> entry = GetOrCreateConfigEntry(sectionName, key, newValue);
                return ValidateAndSetSetting(entry, newValue, validator);
            }
            catch (Exception ex)
            {
                CabbyCodesPlugin.BLogger.LogException(ex, $"SetConfigValue for {sectionName}.{key}");
                return false;
            }
        }

        /// <summary>
        /// Removes a configuration entry from the config file.
        /// </summary>
        /// <param name="sectionName">The name of the section.</param>
        /// <param name="key">The configuration key.</param>
        /// <returns>True if the entry was removed, false otherwise.</returns>
        public static bool RemoveConfigEntry(string sectionName, string key)
        {
            try
            {
                if (string.IsNullOrEmpty(sectionName))
                {
                    CabbyCodesPlugin.BLogger.LogWarning("Section name cannot be null or empty");
                    return false;
                }

                if (string.IsNullOrEmpty(key))
                {
                    CabbyCodesPlugin.BLogger.LogWarning("Key cannot be null or empty");
                    return false;
                }

                ConfigDefinition def = new(sectionName, key);
                if (CabbyCodesPlugin.configFile.Remove(def))
                {
                    ModConfig.Save();
                    CabbyCodesPlugin.BLogger.LogDebug("Removed ConfigEntry for {0}.{1}", sectionName, key);
                    return true;
                }
                else
                {
                    CabbyCodesPlugin.BLogger.LogDebug("ConfigEntry not found for removal: {0}.{1}", sectionName, key);
                    return false;
                }
            }
            catch (Exception ex)
            {
                CabbyCodesPlugin.BLogger.LogException(ex, $"RemoveConfigEntry for {sectionName}.{key}");
                return false;
            }
        }

        /// <summary>
        /// Gets all configuration keys from a specific section as ConfigEntry objects.
        /// </summary>
        /// <typeparam name="T">Type of the configuration values.</typeparam>
        /// <param name="sectionName">The name of the section to read keys from.</param>
        /// <param name="defaultValue">Default value for entries that don't exist.</param>
        /// <returns>A dictionary of key names to ConfigEntry objects.</returns>
        public static Dictionary<string, ConfigEntry<T>> GetConfigEntries<T>(string sectionName, T defaultValue = default(T))
        {
            Dictionary<string, ConfigEntry<T>> result = new();
            try
            {
                List<string> keys = GetConfigKeys(sectionName);
                foreach (string key in keys)
                {
                    ConfigEntry<T> entry = GetOrCreateConfigEntry(sectionName, key, defaultValue);
                    if (entry != null)
                    {
                        result[key] = entry;
                    }
                }

                CabbyCodesPlugin.BLogger.LogDebug("Retrieved {0} ConfigEntry objects from section '{1}'", result.Count, sectionName);
            }
            catch (Exception ex)
            {
                CabbyCodesPlugin.BLogger.LogException(ex, $"GetConfigEntries for section '{sectionName}'");
            }

            return result;
        }
    }
} 