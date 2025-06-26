using CabbyCodes.UI.CheatPanels;
using CabbyCodes.Configuration;
using UnityEngine.UI;
using UnityEngine;
using System;
using CabbyCodes.SyncedReferences;

namespace CabbyCodes.UI.CheatPanels
{
    /// <summary>
    /// Panel for configuring mod settings in-game.
    /// </summary>
    public class SettingsPanel : CheatPanel
    {
        private Toggle inputValidationToggle;
        private Toggle debugInfoToggle;
        private Toggle performanceLoggingToggle;
        private Toggle undoRedoToggle;
        private Toggle confirmChangesToggle;
        private InputField maxLogEntriesInput;
        private InputField undoHistoryInput;
        private InputField menuPosXInput;
        private InputField menuPosYInput;

        public SettingsPanel() : base("Mod Settings")
        {
            CreateSettingsUI();
        }

        private void CreateSettingsUI()
        {
            // UI Settings Section
            AddSectionHeader("UI Settings");
            
            inputValidationToggle = CreateToggle("Enable Input Validation", 
                ModConfig.EnableInputValidation.Value,
                (value) => {
                    ModConfig.EnableInputValidation.Value = value;
                    ModConfig.Save();
                    CabbyCodesPlugin.BLogger.LogInfo("Input validation {0}", value ? "enabled" : "disabled");
                });

            debugInfoToggle = CreateToggle("Show Debug Info", 
                ModConfig.ShowDebugInfo.Value,
                (value) => {
                    ModConfig.ShowDebugInfo.Value = value;
                    ModConfig.Save();
                    CabbyCodesPlugin.BLogger.LogInfo("Debug info {0}", value ? "enabled" : "disabled");
                });

            menuPosXInput = CreateInputField("Menu Position X", 
                ModConfig.MenuPositionX.Value.ToString(),
                (value) => {
                    if (int.TryParse(value, out int x) && x >= 0 && x <= 1920)
                    {
                        ModConfig.MenuPositionX.Value = x;
                        ModConfig.Save();
                        CabbyCodesPlugin.BLogger.LogInfo("Menu X position set to {0}", x);
                    }
                });

            menuPosYInput = CreateInputField("Menu Position Y", 
                ModConfig.MenuPositionY.Value.ToString(),
                (value) => {
                    if (int.TryParse(value, out int y) && y >= 0 && y <= 1080)
                    {
                        ModConfig.MenuPositionY.Value = y;
                        ModConfig.Save();
                        CabbyCodesPlugin.BLogger.LogInfo("Menu Y position set to {0}", y);
                    }
                });

            // Performance Settings Section
            AddSectionHeader("Performance Settings");
            
            performanceLoggingToggle = CreateToggle("Enable Performance Logging", 
                ModConfig.EnablePerformanceLogging.Value,
                (value) => {
                    ModConfig.EnablePerformanceLogging.Value = value;
                    ModConfig.Save();
                    CabbyCodesPlugin.BLogger.LogInfo("Performance logging {0}", value ? "enabled" : "disabled");
                });

            maxLogEntriesInput = CreateInputField("Max Log Entries", 
                ModConfig.MaxLogEntries.Value.ToString(),
                (value) => {
                    if (int.TryParse(value, out int max) && max > 0 && max <= 10000)
                    {
                        ModConfig.MaxLogEntries.Value = max;
                        ModConfig.Save();
                        CabbyCodesPlugin.BLogger.LogInfo("Max log entries set to {0}", max);
                    }
                });

            // Gameplay Settings Section
            AddSectionHeader("Gameplay Settings");
            
            undoRedoToggle = CreateToggle("Enable Undo/Redo", 
                ModConfig.EnableUndoRedo.Value,
                (value) => {
                    ModConfig.EnableUndoRedo.Value = value;
                    ModConfig.Save();
                    CabbyCodesPlugin.BLogger.LogInfo("Undo/Redo {0}", value ? "enabled" : "disabled");
                });

            undoHistoryInput = CreateInputField("Undo History Size", 
                ModConfig.UndoHistorySize.Value.ToString(),
                (value) => {
                    if (int.TryParse(value, out int size) && size > 0 && size <= 100)
                    {
                        ModConfig.UndoHistorySize.Value = size;
                        ModConfig.Save();
                        CabbyCodesPlugin.BLogger.LogInfo("Undo history size set to {0}", size);
                    }
                });

            confirmChangesToggle = CreateToggle("Confirm Destructive Changes", 
                ModConfig.ConfirmDestructiveChanges.Value,
                (value) => {
                    ModConfig.ConfirmDestructiveChanges.Value = value;
                    ModConfig.Save();
                    CabbyCodesPlugin.BLogger.LogInfo("Destructive change confirmation {0}", value ? "enabled" : "disabled");
                });

            // Action Buttons
            AddSectionHeader("Actions");
            CreateButton("Reset to Defaults", () => {
                ModConfig.ResetToDefaults();
                RefreshUI();
                CabbyCodesPlugin.BLogger.LogInfo("Settings reset to defaults");
            });

            CreateButton("Save Settings", () => {
                ModConfig.Save();
                CabbyCodesPlugin.BLogger.LogInfo("Settings saved successfully");
            });

            CreateButton("Reload Settings", () => {
                ModConfig.Reload();
                RefreshUI();
                CabbyCodesPlugin.BLogger.LogInfo("Settings reloaded from disk");
            });

            // Backup and Restore Section
            AddSectionHeader("Backup & Restore");
            CreateButton("Create Backup", () => {
                if (SettingsManager.CreateBackup())
                {
                    CabbyCodesPlugin.BLogger.LogInfo("Settings backup created successfully");
                }
                else
                {
                    CabbyCodesPlugin.BLogger.LogWarning("Failed to create settings backup");
                }
            });

            CreateButton("Restore from Backup", () => {
                if (SettingsManager.RestoreFromBackup())
                {
                    RefreshUI();
                    CabbyCodesPlugin.BLogger.LogInfo("Settings restored from backup successfully");
                }
                else
                {
                    CabbyCodesPlugin.BLogger.LogWarning("Failed to restore settings from backup");
                }
            });

            CreateButton("List Backups", () => {
                var backups = SettingsManager.GetAvailableBackups();
                if (backups.Length > 0)
                {
                    CabbyCodesPlugin.BLogger.LogInfo("Available backups:");
                    foreach (var backup in backups)
                    {
                        CabbyCodesPlugin.BLogger.LogInfo("  - {0}", backup);
                    }
                }
                else
                {
                    CabbyCodesPlugin.BLogger.LogInfo("No backup files found");
                }
            });
        }

        private void AddSectionHeader(string title)
        {
            var headerPanel = new InfoPanel(title).SetColor(new Color(0.8f, 0.8f, 0.8f, 1f));
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(headerPanel);
        }

        private Toggle CreateToggle(string label, bool initialValue, Action<bool> onValueChanged)
        {
            var togglePanel = new TogglePanel(new ToggleReference(initialValue, onValueChanged), label);
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(togglePanel);
            return togglePanel.GetToggle();
        }

        private InputField CreateInputField(string label, string initialValue, Action<string> onValueChanged)
        {
            var inputPanel = new InputFieldPanel<string>(
                new StringReference(initialValue, onValueChanged), 
                KeyCodeMap.ValidChars.Numeric, 
                10, 
                200, 
                label
            );
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(inputPanel);
            return inputPanel.GetInputField();
        }

        private void CreateButton(string label, Action onClick)
        {
            var buttonPanel = new ButtonPanel(onClick, label, label, 120);
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(buttonPanel);
        }

        private void RefreshUI()
        {
            // Update UI elements with current config values
            if (inputValidationToggle != null)
                inputValidationToggle.isOn = ModConfig.EnableInputValidation.Value;
            
            if (debugInfoToggle != null)
                debugInfoToggle.isOn = ModConfig.ShowDebugInfo.Value;
            
            if (performanceLoggingToggle != null)
                performanceLoggingToggle.isOn = ModConfig.EnablePerformanceLogging.Value;
            
            if (undoRedoToggle != null)
                undoRedoToggle.isOn = ModConfig.EnableUndoRedo.Value;
            
            if (confirmChangesToggle != null)
                confirmChangesToggle.isOn = ModConfig.ConfirmDestructiveChanges.Value;
            
            if (maxLogEntriesInput != null)
                maxLogEntriesInput.text = ModConfig.MaxLogEntries.Value.ToString();
            
            if (undoHistoryInput != null)
                undoHistoryInput.text = ModConfig.UndoHistorySize.Value.ToString();
            
            if (menuPosXInput != null)
                menuPosXInput.text = ModConfig.MenuPositionX.Value.ToString();
            
            if (menuPosYInput != null)
                menuPosYInput.text = ModConfig.MenuPositionY.Value.ToString();
        }

        public new void Update()
        {
            // Update settings panel if needed
            base.Update();
        }
    }

    /// <summary>
    /// Reference class for toggle controls in settings.
    /// </summary>
    public class ToggleReference : ISyncedReference<bool>
    {
        private bool _value;
        private readonly Action<bool> _onValueChanged;

        public ToggleReference(bool initialValue, Action<bool> onValueChanged)
        {
            _value = initialValue;
            _onValueChanged = onValueChanged;
        }

        public bool Get() => _value;

        public void Set(bool value)
        {
            _value = value;
            _onValueChanged?.Invoke(value);
        }
    }

    /// <summary>
    /// Reference class for string input controls in settings.
    /// </summary>
    public class StringReference : ISyncedReference<string>
    {
        private string _value;
        private readonly Action<string> _onValueChanged;

        public StringReference(string initialValue, Action<string> onValueChanged)
        {
            _value = initialValue;
            _onValueChanged = onValueChanged;
        }

        public string Get() => _value;

        public void Set(string value)
        {
            _value = value;
            _onValueChanged?.Invoke(value);
        }
    }
} 