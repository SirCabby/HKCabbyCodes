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
- **CommonPatches**: Common Harmony patch methods

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

### Creating a Simple Button Panel
```csharp
using CabbyMenu.UI.CheatPanels;

var buttonPanel = new ButtonPanel(
    action: () => Debug.Log("Button clicked!"),
    buttonText: "Click Me",
    description: "A simple button",
    width: CabbyMenu.Constants.DEFAULT_PANEL_WIDTH
);
```

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
    width: CabbyMenu.Constants.DEFAULT_PANEL_WIDTH,
    description: "Enter a number"
);
```

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
var panelWidth = Constants.DEFAULT_PANEL_WIDTH;  // 120
var panelHeight = Constants.DEFAULT_PANEL_HEIGHT; // 60
var charLimit = Constants.DEFAULT_CHARACTER_LIMIT; // 1
var clickDelay = Constants.CLICK_TIMER_DELAY;     // 0.2
```

## 🏛️ Architecture

### Namespace Structure
- `CabbyMenu`: Core utilities and base classes
- `CabbyMenu.UI`: UI-related components
- `CabbyMenu.UI.CheatPanels`: Pre-built UI panels
- `CabbyMenu.UI.ReferenceControls`: Synchronized UI controls
- `CabbyMenu.UI.Factories`: UI element factories
- `CabbyMenu.UI.Modders`: UI component modifiers
- `CabbyMenu.SyncedReferences`: Data synchronization interfaces
- `CabbyMenu.Types`: Type definitions
- `CabbyMenu.Debug`: Debug utilities

### Project Structure
```
CabbyMenu/
├── UI/                    # UI components and controls
│   ├── CheatPanels/      # Pre-built UI panels
│   ├── ReferenceControls/ # Synchronized controls
│   ├── Factories/        # UI element factories
│   ├── Modders/          # UI component modifiers
│   └── CabbyMenu.cs      # Main menu system
├── SyncedReferences/     # Data synchronization
│   ├── ISyncedReference.cs
│   ├── ISyncedValueList.cs
│   └── BoxedReference.cs
├── Types/                # Type definitions
├── Debug/                # Debug utilities
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
    public const int DEFAULT_PANEL_WIDTH = 120;
    public const int DEFAULT_PANEL_HEIGHT = 60;
    public const double CLICK_TIMER_DELAY = 0.2;
}
```

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
- BepInEx.Unity.Mono
- HarmonyX
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