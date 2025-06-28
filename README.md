# CabbyCodes - Hollow Knight Cheat Menu Mod

A comprehensive cheat menu mod for Hollow Knight that provides extensive control over player stats, inventory, and game state through an intuitive in-game interface.

## 📋 Table of Contents
- [What is CabbyCodes?](#-what-is-cabbycodes)
- [Installation](#-installation)
- [Usage](#-usage)
- [Configuration](#️-configuration)
- [User Troubleshooting](#-user-troubleshooting)
- [Development](#️-development)
- [Contributing](#-contributing)
- [Support](#-support)

## 🎮 What is CabbyCodes?

CabbyCodes is a powerful cheat menu mod that gives you complete control over your Hollow Knight experience. Whether you want to experiment with different builds, test game mechanics, or simply have fun with unlimited resources, CabbyCodes provides an easy-to-use interface accessible directly from the pause menu.

### Key Features

#### 🛡️ Player Modifications
- **Health Control**: Adjust max health (5-9 masks)
- **Geo Management**: Set geo amount (0-9,999,999)
- **Soul Control**: Modify soul reserves and focus
- **Damage Control**: Toggle one-hit kills for enemies
- **Invulnerability**: Toggle player invincibility
- **Movement**: Control dash, wall jump, and other abilities

#### 📦 Inventory Management
- **Currency Items**: 
  - Wanderer's Journals (0-14)
  - Hallownest Seals (0-17)
  - King's Idols (0-8)
  - Arcane Eggs (0-4)
  - Rancid Eggs (0-80)
  - Pale Ore (0-6)
- **Keys**: All game keys and access items
- **Spells**: Control spell levels and upgrades
- **Nail Arts**: Manage nail art abilities
- **Charms**: Complete charm management system

#### 🎯 Game State Control
- **Hunter's Journal**: Complete enemy kill tracking
- **Grub Rescue**: Control grub collection status (46 total grubs)
- **Map Control**: Manage map exploration and pins
- **Achievements**: Achievement tracking and control
- **Flags**: Various game state flags and triggers

#### 🚀 Teleportation
- **Custom Locations**: Add and manage custom teleport points
- **Quick Travel**: Instant travel to any discovered location

## 🚀 Installation

### Prerequisites
- Hollow Knight (Steam version)
- BepInEx 6.0 or later

### Installation Steps
1. **Install BepInEx**:
   - Download BepInEx 6.0+ from [GitHub](https://github.com/BepInEx/BepInEx/releases)
   - Extract to your Hollow Knight directory
   - Run the game once to generate BepInEx folders

2. **Install CabbyCodes**:
   - Download the latest release from the releases page
   - Extract both `CabbyCodes.dll` and `CabbyMenu.dll` to `BepInEx/plugins/`
   - Start the game

3. **Verify Installation**:
   - Check BepInEx console for "Plugin cabby.cabbycodes is loaded!"
   - Pause the game to access the cheat menu

## 🎯 Usage

### Accessing the Menu
1. **Pause the game** (ESC key)
2. **Click the "Cabby Codes" button** that appears
3. **Navigate categories** using the dropdown menu
4. **Modify values** by clicking on input fields

### Menu Categories
- **Player**: Health, geo, soul, abilities
- **Inventory**: All collectible items and currency
- **Charms**: Charm management and costs
- **Maps**: Map exploration and pins
- **Grubs**: Grub rescue tracking
- **Hunter**: Enemy kill tracking
- **Flags**: Game state flags
- **Achievements**: Achievement control
- **Debug**: Debug utilities and information

## ⚙️ Configuration

### Config File Location
```
BepInEx/config/cabby.cabbycodes.cfg
```

### Configuration System
The mod uses BepInEx's built-in configuration system for:
- **Teleport Locations**: Custom teleport points are automatically saved and loaded
- **Game State**: Various game modifications are persisted between sessions
- **Menu Settings**: Basic menu configuration options

### Teleport System
Custom teleport locations are automatically saved to the config file and will persist between game sessions. The system maintains:
- Location coordinates (X, Y)
- Scene names
- Display names for easy identification

## 🐛 User Troubleshooting

### Common Issues

**Menu doesn't appear:**
- Ensure both `CabbyCodes.dll` and `CabbyMenu.dll` are in the plugins folder
- Check BepInEx console for error messages
- Verify BepInEx is properly installed

**Values not updating:**
- Check that input values are within the required range

## 🛠️ Development

### Project Structure

This project uses a **multi-project solution** with two main components:

- **CabbyCodes**: Main mod plugin with game patches and logic
- **CabbyMenu**: Reusable UI library for creating mod menus

#### Architecture
```
HKCabbyCodes/
├── CabbyCodes/          # Main mod project
│   ├── Patches/         # Game modification patches
│   ├── Types/           # Game-specific types
│   └── CabbyCodesPlugin.cs  # Main plugin entry point
├── CabbyMenu/           # UI library project
│   ├── UI/              # UI components and controls
│   ├── SyncedReferences/ # Data synchronization
│   ├── Types/           # UI-specific types
│   └── Constants.cs     # UI constants (moved from CabbyCodes)
└── CabbyCodes.sln       # Solution file
```

### 📚 Required Library Files

Before building the project, you need to obtain the following DLL files and place them in the `CabbyCodes/lib/` directory:

#### Required Files:
- **Assembly-CSharp.dll** - Main game assembly (from Hollow Knight installation)
- **PlayMaker.dll** - PlayMaker FSM library (from Hollow Knight installation)
- **UnityExplorer.STANDALONE.Mono.dll** - UnityExplorer in-game inspector (from UnityExplorer release) *(optional, see note below)*

#### File Structure:
```
CabbyCodes/
└── lib/
    ├── Assembly-CSharp.dll
    ├── PlayMaker.dll
    ├── UnityExplorer.STANDALONE.Mono.dll
```

**Note**: These files are excluded from version control due to licensing restrictions. You must obtain them manually before building and debugging the project.

**UnityExplorer is optional:**
- If you do **not** want to use UnityExplorer, you can comment out the following line in `CabbyCodes/CabbyCodesPlugin.cs` (inside the `Start` method):
  ```csharp
  UnityExplorer.ExplorerStandalone.CreateInstance();
  ```
- This will allow you to build and run the mod without needing `UnityExplorer.STANDALONE.Mono.dll` in your `lib` folder.

### 🔧 Optional / Useful Files

#### UnityExplorer - In-Game Debug Tool

**What is UnityExplorer?**
UnityExplorer is a powerful in-game inspector and debugger that provides a graphical interface for examining game objects, components, and values at runtime. It's essential for development and debugging of Hollow Knight mods.

**What does it do?**
- **Object Inspector**: Browse and examine all game objects in the scene
- **Component Viewer**: View and modify component properties in real-time
- **Value Editor**: Change game values on-the-fly for testing
- **Method Invoker**: Call game methods directly for debugging
- **Scene Browser**: Navigate through different game scenes and objects

**How to Install UnityExplorer:**
1. **Download**: Get the latest release from [UnityExplorer GitHub](https://github.com/sinai-dev/UnityExplorer/releases)
2. **Extract**: Download the `UnityExplorer.STANDALONE.Mono.dll` file
3. **Install**: Place the DLL in your Hollow Knight `BepInEx/plugins/` folder
4. **Activate**: The mod automatically creates a UnityExplorer instance when the game starts

**Using UnityExplorer:**
- **Access**: Press `F7` in-game to open the UnityExplorer window
- **Debugging**: Use it to inspect game state, player data, and scene objects
- **Development**: Essential for understanding game structure and testing mod features

#### UniverseLib - Reflection and Debugging Library

**What is UniverseLib?**
UniverseLib is a powerful reflection and debugging library that provides advanced tools for accessing private fields, methods, and game objects at runtime. It's commonly used in Hollow Knight modding for deep game state inspection and manipulation.

**What does it do?**
- **Reflection Utilities**: Access private fields and methods
- **Object Inspection**: Deep examination of game objects and components
- **Memory Manipulation**: Advanced memory reading and writing capabilities
- **Runtime Type Creation**: Create and modify types at runtime
- **Debugging Tools**: Enhanced debugging and development features

**How to Install UniverseLib:**
1. **Download**: Get the latest release from [UniverseLib releases](https://github.com/sinai-dev/UniverseLib/releases)
2. **Extract**: Download the `UniverseLib.Mono.dll` file
3. **Install**: Place the DLL in your Hollow Knight `BepInEx/plugins/` folder
4. **Usage**: Access through UnityExplorer or other modding tools

**Using UniverseLib:**
- **Integration**: Works with UnityExplorer for enhanced debugging
- **Development**: Essential for advanced mod development and testing
- **Reflection**: Access game internals that aren't normally accessible

### 🛠️ Building from Source

#### Using Make (Recommended)
```bash
# Build the entire solution
make build

# Build individual projects
make build-cabbycodes
make build-cabbymenu

# Clean build artifacts
make clean

# Deploy to game directory
make deploy
```

#### Using .NET CLI
```bash
# Restore dependencies
dotnet restore

# Build the solution
dotnet build --configuration Release

# Build individual projects
dotnet build CabbyCodes/CabbyCodes.csproj --configuration Release
dotnet build CabbyMenu/CabbyMenu.csproj --configuration Release
```

### 🔗 Project Dependencies
- **CabbyCodes** depends on **CabbyMenu** for UI functionality
- **CabbyMenu** is a standalone library that can be used by other mods
- Both projects target .NET Framework 4.7.2

### 📦 Build Output
- **CabbyCodes.dll**: Main mod plugin (contains game patches)
- **CabbyMenu.dll**: UI library (required by CabbyCodes)

### 🔧 Key Components

#### CabbyCodes Project
- **CabbyCodesPlugin**: Main plugin entry point
- **Patches/**: Game modification patches organized by category
- **Constants.cs**: Game-specific constants (health limits, currency limits, etc.)

#### CabbyMenu Project
- **UI/**: Complete UI system with panels, controls, and factories
- **SyncedReferences/**: Data synchronization interfaces
- **Constants.cs**: UI constants (panel sizes, character limits, etc.)

## 🐛 Developer Troubleshooting

### Common Issues

**Build errors:**
- Ensure .NET 6.0 SDK is installed
- Run `dotnet restore` to restore dependencies
- Check that both projects are included in the solution

**Deployment issues:**
- Use `make deploy` to automatically copy both DLLs
- Verify the Hollow Knight path in the Makefile
- Check file permissions in the plugins directory

### Debug Information
Check the BepInEx console for detailed information about:
- Menu interactions
- Value changes
- Error conditions
- Configuration loading/saving

## 🤝 Contributing

### How to Contribute
1. **Fork the repository**
2. **Create a feature branch**: `git checkout -b feature/new-feature`
3. **Make your changes** and add tests
4. **Commit your changes**: `git commit -am 'Add new feature'`
5. **Push to the branch**: `git push origin feature/new-feature`
6. **Submit a pull request**

### Development Guidelines
- Follow C# coding conventions
- Add XML documentation to public methods
- Include error handling for all user inputs
- Test changes thoroughly before submitting
- Update documentation for new features
- Keep UI constants in CabbyMenu project
- Keep game-specific constants in CabbyCodes project

### Project Organization
- **Game Logic**: Add to CabbyCodes project
- **UI Components**: Add to CabbyMenu project
- **Shared Types**: Place in appropriate project based on usage
- **Constants**: UI constants in CabbyMenu, game constants in CabbyCodes

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- **Team Cherry** for creating Hollow Knight
- **BepInEx Team** for the modding framework
- **HarmonyX** for runtime patching capabilities
- **Unity Technologies** for the game engine

## 📞 Support

### Getting Help
- **GitHub Issues**: Report bugs and request features

### Reporting Bugs
When reporting bugs, please include:
- Game version and mod version
- Steps to reproduce the issue
- BepInEx console output
- Any error messages
- System specifications

---

**Note**: This mod is for educational and entertainment purposes. Enjoy.