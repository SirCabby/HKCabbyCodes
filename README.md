# CabbyCodes - Hollow Knight Cheat Menu Mod

A comprehensive cheat menu mod for Hollow Knight that provides extensive control over player stats, inventory, and game state through an intuitive in-game interface.

## 🎮 Features

### Player Modifications
- **Health Control**: Adjust max health (5-9 masks)
- **Geo Management**: Set geo amount (0-9,999,999)
- **Soul Control**: Modify soul reserves and focus
- **Damage Control**: Toggle one-hit kills for enemies
- **Invulnerability**: Toggle player invincibility
- **Movement**: Control dash, wall jump, and other abilities

### Inventory Management
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

### Game State Control
- **Hunter's Journal**: Complete enemy kill tracking
- **Grub Rescue**: Control grub collection status
- **Map Control**: Manage map exploration and pins
- **Achievements**: Achievement tracking and control
- **Flags**: Various game state flags and triggers

### Teleportation
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
   - Extract `CabbyCodes.dll` to `BepInEx/plugins/`
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

### Input Controls
- **Click** to select input fields
- **Type** to enter new values
- **Enter** to confirm changes
- **Escape** to cancel changes
- **Backspace** to delete characters

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

## 🛠️ Development

### Building from Source
1. **Clone the repository**:
   ```bash
   git clone https://github.com/yourusername/CabbyCodes.git
   cd CabbyCodes
   ```

2. **Restore dependencies**:
   ```bash
   dotnet restore
   ```

3. **Build the project**:
   ```bash
   dotnet build --configuration Release
   ```

4. **Copy to game directory**:
   ```bash
   copy "bin\Release\net472\CabbyCodes.dll" "C:\Program Files (x86)\Steam\steamapps\common\Hollow Knight\BepInEx\plugins\"
   ```

### Project Structure
```
CabbyCodes/
├── Patches/           # Game modification patches
│   ├── Player/       # Player-related modifications
│   ├── Inventory/    # Inventory management
│   ├── Charms/       # Charm system
│   └── ...
├── UI/               # User interface components
│   ├── CheatPanels/  # Individual cheat panels
│   ├── Factories/    # UI component factories
│   └── Modders/      # UI modification utilities
├── SyncedReferences/ # Data synchronization
├── Types/            # Custom data types
├── Configuration/    # Configuration management
└── Utils/            # Utility classes
```

### Key Components
- **CabbyCodesPlugin**: Main plugin entry point
- **CabbyMenu**: Core UI management
- **ModConfig**: Configuration system
- **ISyncedReference**: Data synchronization interface
- **ValidationUtils**: Input validation utilities
- **LoggingExtensions**: Enhanced logging capabilities

## 🐛 Troubleshooting

### Common Issues

**Menu doesn't appear:**
- Ensure BepInEx is properly installed
- Check BepInEx console for error messages
- Verify the DLL is in the correct plugins folder

**Values not updating:**
- Make sure you're paused in the game
- Check that input validation isn't blocking changes
- Verify the game state allows the modification

**Teleport locations not saving:**
- Check that the config file is writable
- Verify BepInEx has proper permissions
- Look for error messages in the BepInEx console

**Performance issues:**
- Close other mods that might conflict
- Check for memory leaks in long sessions
- Restart the game if issues persist

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
- **Discord**: Join our community server for discussions
- **Wiki**: Check the wiki for detailed guides

### Reporting Bugs
When reporting bugs, please include:
- Game version and mod version
- Steps to reproduce the issue
- BepInEx console output
- Any error messages
- System specifications

---

**Note**: This mod is for educational and entertainment purposes. Use responsibly and respect the game's intended experience. 