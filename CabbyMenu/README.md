# CabbyMenu UI Library

A reusable Unity UI library for creating mod menus and cheat panels in Hollow Knight mods. This library provides a comprehensive set of UI components, utilities, and patterns for building intuitive mod interfaces.

## 🏗️ Project Overview

CabbyMenu is designed as a **standalone library** that can be referenced by other Hollow Knight mods. It provides:

- **Modular UI Components**: Pre-built panels and controls for common mod functionality
- **Data Synchronization**: Automatic synchronization between UI and game data
- **Input Validation**: Built-in validation for different input types
- **Consistent Styling**: Unified look and feel across all UI components
- **Extensible Architecture**: Easy to extend and customize for specific needs

## 🎯 Features

### Core Components
- **Fitter**: Utility for positioning and sizing UI GameObjects
- **KeyCodeMap**: Input validation and key code mapping
- **CodeState**: Shared state management system
- **ValidationUtils**: Input validation utilities
- **Constants**: Centralized UI constants (panel sizes, character limits, etc.)

### Game State Management
- **IGameStateProvider**: Interface for determining when the menu should be visible
- **ShouldShowMenu()**: Method that controls menu visibility based on game state

### UI Components
- **CheatPanel**: Base class for all cheat panels
- **ButtonPanel**: Panel with clickable buttons
- **TogglePanel**: Panel with toggle switches
- **InputFieldPanel**: Panel with text input fields
- **DropdownPanel**: Panel with dropdown selections
- **InfoPanel**: Simple information display panel

### Reference Controls
- **ToggleButton**: Synchronized toggle button
- **InputFieldSync**: Synchronized text input field
- **DropDownSync**: Synchronized dropdown control

### Utilities
- **ObjectPrint**: Debug utility for object inspection
- **LoggingExtensions**: Extended logging methods
- **PbdMaker**: Persistent data management
- **CommonPatches**: Common utility methods for patches

## 🚀 Setup

### 1. Add Project Reference
Add the CabbyMenu project as a reference to your main mod project:

```xml
<ItemGroup>
  <ProjectReference Include="..\CabbyMenu\CabbyMenu.csproj" />
</ItemGroup>
```

### 2. Configure Logging
Set up logging actions in your main plugin:

```csharp
// In your plugin's Awake method
CabbyMenu.Debug.ObjectPrint.Logger = BepInEx.Logging.Logger.CreateLogSource("YourMod").LogInfo;
CabbyMenu.ValidationUtils.WarningLogger = BepInEx.Logging.Logger.CreateLogSource("YourMod").LogWarning;
CabbyMenu.UI.ReferenceControls.InputFieldSync.ErrorLogger = BepInEx.Logging.Logger.CreateLogSource("YourMod").LogError;
CabbyMenu.UI.ReferenceControls.InputFieldSync.WarningLogger = BepInEx.Logging.Logger.CreateLogSource("YourMod").LogWarning;
```

### 3. Configure Input Field Registration
Set up input field registration if you're using InputFieldSync:

```csharp
CabbyMenu.UI.ReferenceControls.InputFieldSync.RegisterInputFieldSync = (inputFieldStatus) => {
    // Register with your menu system
    yourMenu.RegisterInputFieldSync(inputFieldStatus);
};
```

## 📖 Usage Examples

### Creating a Button Panel
```csharp
using CabbyMenu.UI.CheatPanels;

var buttonPanel = new ButtonPanel(
    action: () => Debug.Log("Button clicked!"),
    buttonText: "Click Me",
    description: "A simple button"
);
```

**Note**: Button width is automatically calculated based on the button text length to ensure all text is visible.

### Creating a Toggle Panel
```csharp
using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;

var toggleRef = new BoxedReference<bool>(false);
var togglePanel = new TogglePanel(toggleRef, "Enable Feature");
```

### Creating an Input Field Panel
```csharp
using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;

var valueRef = new BoxedReference<int>(0);
var inputPanel = new InputFieldPanel<int>(
    syncedReference: valueRef,
    validChars: KeyCodeMap.ValidChars.Numeric,
    characterLimit: CabbyMenu.Constants.DEFAULT_CHARACTER_LIMIT,
    description: "Enter a number"
);
```

**Note**: Panel width is automatically calculated based on the character limit to ensure all characters can be displayed.

### Creating a Dropdown Panel
```csharp
using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;

var dropdownRef = new BoxedReference<int>(0);
var dropdownPanel = new DropdownPanel(dropdownRef, "Select an option");
```

**Note**: Dropdown width is automatically calculated based on the longest option text to ensure all options are visible.

### Using Synced References
```csharp
using CabbyMenu.SyncedReferences;

// Create a reference to a game value
var healthRef = new BoxedReference<int>(5);

// The UI will automatically sync with this value
healthRef.Set(8); // Updates the UI
int currentHealth = healthRef.Get(); // Gets current value
```

### Using UI Constants
```csharp
using CabbyMenu;

// Use predefined UI constants for consistent sizing
var panelHeight = Constants.DEFAULT_PANEL_HEIGHT; // 60
var charLimit = Constants.DEFAULT_CHARACTER_LIMIT; // 1
var clickDelay = Constants.CLICK_TIMER_DELAY;     // 0.2

// Calculate character width for custom calculations
var charWidth = Constants.CalculateCharacterWidth(36); // ~16.2 pixels per character

// Calculate panel width based on character limit
var inputWidth = Constants.CalculatePanelWidth(5); // Width for 5 characters

// Calculate button width based on text length
var buttonWidth = Constants.CalculateButtonWidth("Click Me"); // Width for button text
```

### Using Game State Management
The `IGameStateProvider` interface allows you to control when the menu should be visible based on game state. This keeps the UI library independent of game-specific assemblies.

#### Implementing IGameStateProvider
```csharp
using CabbyMenu.Types;

public class MyGameStateProvider : IGameStateProvider
{
    public bool ShouldShowMenu()
    {
        // Example: Show menu when game is paused
        return GameManager.instance != null && GameManager.instance.IsGamePaused();
        
        // Example: Show menu when a specific key is pressed
        // return Input.GetKey(KeyCode.F1);
        
        // Example: Show menu in specific game states
        // return GameManager.instance.gameState == GameState.PAUSED;
    }
}
```

#### Using with CabbyMainMenu
```csharp
using CabbyMenu.UI;
using CabbyMenu.Types;

// Create your game state provider
var gameStateProvider = new MyGameStateProvider();

// Initialize the menu with the provider
var cabbyMenu = new CabbyMainMenu("My Mod", "1.0.1", gameStateProvider);

// The menu will automatically show/hide based on your provider
cabbyMenu.Update(); // Called every frame
```

#### Menu Visibility Behavior
- **When `ShouldShowMenu()` returns `true`**: The menu button becomes visible and interactive
- **When `ShouldShowMenu()` returns `false`**: The entire menu system is hidden and deactivated
- **Automatic cleanup**: When the menu is hidden, it automatically resets to a clean state
- **Performance optimized**: The menu only renders when it should be visible

## 🏛️ Architecture

### Namespace Structure
- `CabbyMenu`: Core utilities and base classes
- `CabbyMenu.UI`: UI-related components
- `CabbyMenu.UI.CheatPanels`: Pre-built UI panels
- `CabbyMenu.UI.Controls`: UI controls (ToggleButton, InputField, etc.)
- `CabbyMenu.UI.DynamicPanels`: Dynamic panel management
- `CabbyMenu.UI.Modders`: UI component modifiers
- `CabbyMenu.UI.Popups`: Popup dialogs
- `CabbyMenu.SyncedReferences`: Data synchronization interfaces
- `CabbyMenu.TextProcessors`: Text processing utilities
- `CabbyMenu.Utilities`: Utility classes

### Project Structure
```
CabbyMenu/
├── UI/                    # UI components and controls
│   ├── CheatPanels/      # Pre-built UI panels
│   ├── Controls/          # UI controls (ToggleButton, InputField, etc.)
│   ├── DynamicPanels/    # Dynamic panel management
│   ├── Modders/          # UI component modifiers
│   ├── Popups/           # Popup dialogs
│   ├── CabbyMainMenu.cs  # Main menu system
│   ├── Fitter.cs         # UI layout utility
│   └── IPersistentPopup.cs # Popup interface
├── SyncedReferences/     # Data synchronization
│   ├── ISyncedReference.cs
│   ├── ISyncedValueList.cs
│   ├── BoxedReference.cs
│   ├── DelegateReference.cs
│   └── DelegateValueList.cs
├── TextProcessors/       # Text processing utilities
│   ├── ITextProcessor.cs
│   ├── TextProcessor.cs
│   ├── BaseNumericProcessor.cs
│   ├── NumericTextProcessor.cs
│   ├── StringTextProcessor.cs
│   └── DecimalTextProcessor.cs
├── Utilities/            # Utility classes
│   ├── ValidationUtils.cs
│   ├── KeyCodeMap.cs
│   └── CoroutineRunner.cs
├── Constants.cs          # UI constants
└── README.md            # This file
```

## ⚙️ Configuration

### UI Constants
The library provides centralized UI constants in `Constants.cs`:

```csharp
public static class Constants
{
    // UI Constants
    public const int DEFAULT_CHARACTER_LIMIT = 1;
    public const int MIN_PANEL_WIDTH = 120;
    public const int DEFAULT_PANEL_HEIGHT = 60;
    public const double CLICK_TIMER_DELAY = 0.2;
    
    // Font Sizes
    public const int DEFAULT_FONT_SIZE = 36;
    public const int TITLE_FONT_SIZE = 60;
    
    // Colors
    public static readonly Color ON_COLOR = new Color(0f, 0.8f, 1f, 1f);
    public static readonly Color OFF_COLOR = new Color(0.6f, 0.6f, 0.6f, 1f);
    
    // Layout Settings
    public const int CHEAT_PANEL_PADDING = 20;
    public const int CHEAT_PANEL_SPACING = 50;
}
```

**Note**: Utility methods for calculating panel widths are implemented in specific UI components rather than in the Constants class:
- `ButtonPanel.CalculateButtonWidth(string text)` - Calculates button width based on text
- `InputFieldPanel.CalculatePanelWidth(int characterLimit)` - Calculates input field width
- `TextInputFieldStatus.CalculateCharacterWidth(int fontSize)` - Calculates character width

### Customization
You can override these constants in your mod or extend the library to add new ones:

```csharp
// In your mod, you can define your own constants
public static class MyModConstants
{
    public const int CUSTOM_PANEL_WIDTH = 200;
    public const int CUSTOM_CHAR_LIMIT = 5;
}
```

## 🔧 Development

### Building the Library
```bash
# Build the library
dotnet build CabbyMenu/CabbyMenu.csproj --configuration Release

# Or use the Makefile
make build-cabbymenu
```

### Dependencies
- BepInEx.Unity.Mono (for BepInEx builds)
- MonoMod.RuntimeDetour (for runtime method hooking)
- Unity3D.UnityEngine.UI
- UnityEngine.Modules
- Assembly-CSharp (Hollow Knight)

### Extending the Library
To add new UI components:

1. **Create new panel classes** in `UI/CheatPanels/`
2. **Extend base classes** like `CheatPanel` or `ISyncedReference`
3. **Add new constants** to `Constants.cs` if needed
4. **Update documentation** and examples

### Best Practices
- Use the provided constants for consistent sizing
- Implement `ISyncedReference` for data synchronization
- Follow the existing naming conventions
- Add XML documentation to public methods
- Test components thoroughly before committing
- Implement `IGameStateProvider` for proper menu visibility control

## 📦 Distribution

### For Mod Users
- **CabbyMenu.dll** must be included with any mod that uses this library
- The DLL should be placed in the `BepInEx/plugins/` directory
- No additional configuration is required

### For Mod Developers
- Reference the CabbyMenu project in your solution
- Build both projects together
- Include both DLLs in your mod distribution

## 🤝 Contributing

### Guidelines
- Follow C# coding conventions
- Add XML documentation to public methods
- Include error handling for all user inputs
- Test changes thoroughly before submitting
- Update documentation for new features
- Keep UI constants centralized in `Constants.cs`

### Adding New Features
1. **Create feature branch**: `git checkout -b feature/new-ui-component`
2. **Implement changes** following existing patterns
3. **Add tests** and documentation
4. **Submit pull request** with detailed description

## 📄 License

This library is provided as-is for use in Hollow Knight mods. Feel free to use, modify, and distribute as needed.