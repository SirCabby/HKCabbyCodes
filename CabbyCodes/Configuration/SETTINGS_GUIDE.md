# CabbyCodes Settings Guide

This guide explains all the configuration options available in the CabbyCodes mod settings panel.

## 🎮 Accessing Settings

1. **In-Game**: Open the CabbyCodes menu and navigate to the "Settings" category
2. **Config File**: Settings are saved to `BepInEx/config/cabby.cabbycodes.cfg`

## 📋 Settings Categories

### 🖥️ UI Settings

#### Enable Input Validation
- **Default**: `true`
- **Description**: Validates all user input before applying changes
- **Effect**: Prevents invalid values from being applied, shows error messages
- **Recommendation**: Keep enabled for safety

#### Show Debug Info
- **Default**: `false`
- **Description**: Displays additional debug information in the UI
- **Effect**: Shows current values, validation status, and error details
- **Recommendation**: Enable for troubleshooting

#### Menu Position X
- **Default**: `100`
- **Range**: `0` to `1920`
- **Description**: Horizontal position of the CabbyCodes menu
- **Effect**: Moves the menu left or right on screen

#### Menu Position Y
- **Default**: `100`
- **Range**: `0` to `1080`
- **Description**: Vertical position of the CabbyCodes menu
- **Effect**: Moves the menu up or down on screen

### ⚡ Performance Settings

#### Enable Performance Logging
- **Default**: `false`
- **Description**: Logs performance metrics for operations
- **Effect**: Tracks timing of menu operations, patch applications
- **Recommendation**: Enable only when debugging performance issues

#### Max Log Entries
- **Default**: `1000`
- **Range**: `1` to `10000`
- **Description**: Maximum number of log entries to keep in memory
- **Effect**: Prevents memory issues with excessive logging
- **Recommendation**: Increase if you need more detailed logs

### 🎯 Gameplay Settings

#### Enable Undo/Redo
- **Default**: `true`
- **Description**: Allows undoing and redoing changes made through the mod
- **Effect**: Provides a safety net for accidental changes
- **Recommendation**: Keep enabled for safety

#### Undo History Size
- **Default**: `10`
- **Range**: `1` to `100`
- **Description**: Number of changes to remember for undo/redo
- **Effect**: More history = more memory usage but more safety
- **Recommendation**: 10-20 for most users

#### Confirm Destructive Changes
- **Default**: `true`
- **Description**: Shows confirmation dialog for potentially dangerous changes
- **Effect**: Prevents accidental deletion of important game data
- **Recommendation**: Keep enabled for safety

## 🔧 Action Buttons

### Reset to Defaults
- **Function**: Restores all settings to their default values
- **Use Case**: When settings become corrupted or you want a fresh start
- **Warning**: This action cannot be undone

### Save Settings
- **Function**: Manually saves current settings to disk
- **Use Case**: When you want to ensure settings are persisted immediately
- **Note**: Settings are usually saved automatically

### Reload Settings
- **Function**: Reloads settings from the config file
- **Use Case**: When you've manually edited the config file
- **Note**: Any unsaved changes will be lost

### Create Backup
- **Function**: Creates a timestamped backup of all current settings
- **Use Case**: Before making major changes or before updating the mod
- **Location**: `BepInEx/config/CabbyCodes/Backups/`
- **Format**: Human-readable INI format with timestamp

### Restore from Backup
- **Function**: Restores settings from the most recent backup file
- **Use Case**: When settings become corrupted or you want to revert changes
- **Note**: Automatically finds the most recent backup if multiple exist

### List Backups
- **Function**: Shows all available backup files in the console
- **Use Case**: To see what backups are available before restoring
- **Output**: Lists backup files with timestamps in the mod log

## 📁 Configuration File

The settings are stored in `BepInEx/config/cabby.cabbycodes.cfg` with this structure:

```ini
[UI]
EnableInputValidation = true
ShowDebugInfo = false
MenuPositionX = 100
MenuPositionY = 100

[Performance]
EnablePerformanceLogging = false
MaxLogEntries = 1000

[Gameplay]
EnableUndoRedo = true
UndoHistorySize = 10
ConfirmDestructiveChanges = true
```

## 💾 Backup System

### Backup Location
Backups are stored in: `BepInEx/config/CabbyCodes/Backups/`

### Backup Files
- **Settings Backup**: `CabbyCodes_Backup_YYYYMMDD_HHMMSS.cfg`
- **Original Config**: `Original_Config_Backup_YYYYMMDD_HHMMSS.cfg`

### Backup Format
```ini
# CabbyCodes Settings Backup
# Created: 2024-01-15 14:30:25
# Version: 1.0

[UI]
EnableInputValidation = true
ShowDebugInfo = false
MenuPositionX = 100
MenuPositionY = 100

[Performance]
EnablePerformanceLogging = false
MaxLogEntries = 1000

[Gameplay]
EnableUndoRedo = true
UndoHistorySize = 10
ConfirmDestructiveChanges = true
```

### Automatic Backups
- Backups are created automatically before major operations
- Manual backups can be created from the settings menu
- Backups are sorted by creation time (newest first)

## 🚨 Troubleshooting

### Settings Not Saving
1. Check if the game has write permissions to the BepInEx folder
2. Verify the config file isn't read-only
3. Try using the "Save Settings" button

### Menu Position Issues
1. Reset to default position (100, 100)
2. Ensure values are within valid ranges (0-1920 for X, 0-1080 for Y)
3. Check if other mods are interfering with UI positioning

### Performance Issues
1. Disable performance logging if not needed
2. Reduce max log entries
3. Disable debug info in production

### Undo/Redo Not Working
1. Ensure "Enable Undo/Redo" is enabled
2. Check if undo history size is sufficient
3. Verify no other mods are interfering with save data

## 🔄 Settings Migration

When updating the mod:
1. Old settings are automatically migrated
2. New settings use default values
3. Invalid settings are reset to defaults
4. A backup is created before migration

## 📞 Support

If you encounter issues with settings:
1. Check the mod log file for error messages
2. Try resetting to defaults
3. Report issues with your config file content
4. Include the mod version and game version 