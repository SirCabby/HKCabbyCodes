# Assembly-CSharp.dll Reference for Hollow Knight Modding

## Overview
This document provides a comprehensive reference for the key classes and methods in Assembly-CSharp.dll that are relevant for Hollow Knight modding.

## Key Classes

### 1. HeroController
**Location**: Main player controller class
**Base Type**: MonoBehaviour
**Key Features**:
- Controls player movement, abilities, and state
- Contains events: `OnTakenDamage`, `OnDeath`, `heroInPosition`
- Has state management through `HeroControllerStates`

**Important Methods**:
- `SetHeroParent(Transform)` - Sets the hero's parent transform
- `get_instance()` - Static accessor for the singleton instance

**Important Fields**:
- `cState` (HeroControllerStates) - Current player state
- `isHeroInPosition` (bool) - Whether hero is in position
- `_instance` (static HeroController) - Singleton instance

### 2. GameManager
**Location**: Main game management class
**Base Type**: MonoBehaviour
**Implements**: Platform/IDisengageHandler
**Key Features**:
- Manages overall game state and flow
- Handles scene loading and transitions
- Controls game progression

**Important Fields**:
- `gameState` (GlobalEnums.GameState) - Current game state
- `verboseMode` (bool) - Debug mode flag

### 3. PlayerData
**Location**: Player save data and game state
**Base Type**: System.Object
**Key Features**:
- Stores all player progress and game state
- Contains methods for getting/setting various data types
- Central hub for save data management

**Important Methods**:
- `GetPlayerDataBool(string boolName)` - Get boolean value
- `SetPlayerDataBoolTrue(string boolName)` - Set boolean to true
- `SetPlayerDataBoolFalse(string boolName)` - Set boolean to false
- `GetPlayerDataInt(string intName)` - Get integer value
- `IncrementPlayerDataInt(string intName)` - Increment integer
- `DecrementPlayerDataInt(string intName)` - Decrement integer
- `GetPlayerDataFloat(string floatName)` - Get float value

**Important Fields**:
- `version` (string) - Save data version
- `awardAllAchievements` (bool) - Achievement flag

### 4. GameCameras
**Location**: Camera management system
**Base Type**: MonoBehaviour
**Key Features**:
- Manages game cameras and camera behavior
- Controls HUD camera and main game camera

**Important Fields**:
- `hudCamera` (Camera) - HUD camera reference

### 5. HeroControllerStates
**Location**: Player state information
**Base Type**: System.Object
**Key Features**:
- Contains current player state information
- Tracks movement and ability states

**Important Fields**:
- `facingRight` (bool) - Player facing direction
- `onGround` (bool) - Whether player is on ground

## PlayMaker Integration Classes

The game uses PlayMaker extensively for visual scripting. Key PlayMaker action classes for PlayerData manipulation:

### PlayerData Actions
- `GetPlayerDataBool` - Retrieve boolean from PlayerData
- `SetPlayerDataBool` - Set boolean in PlayerData
- `GetPlayerDataInt` - Retrieve integer from PlayerData
- `SetPlayerDataInt` - Set integer in PlayerData
- `GetPlayerDataFloat` - Retrieve float from PlayerData
- `SetPlayerDataFloat` - Set float in PlayerData
- `GetPlayerDataString` - Retrieve string from PlayerData
- `SetPlayerDataString` - Set string in PlayerData
- `GetPlayerDataVector3` - Retrieve Vector3 from PlayerData
- `SetPlayerDataVector3` - Set Vector3 in PlayerData
- `IncrementPlayerDataInt` - Increment integer in PlayerData
- `DecrementPlayerDataInt` - Decrement integer in PlayerData
- `PlayerDataIntAdd` - Add to integer in PlayerData

### PlayerData Boolean Actions
- `PlayerDataBoolTest` - Test boolean value
- `PlayerDataBoolAllTrue` - Check if all booleans are true
- `PlayerDataBoolTrueAndFalse` - Check specific boolean combinations

## Game-Specific Classes

### Combat & Damage
- `HitInstance` - Damage instance information
- `HitTaker` - Interface for objects that can take damage
- `IHitResponder` - Interface for objects that respond to hits
- `AttackTypes` - Enum for different attack types
- `SpecialTypes` - Enum for special abilities

### Game Objects
- `Bullet` - Projectile class
- `Explosion` - Explosion effect class
- `Turret` - Turret enemy class
- `GrimmEnemyRange` - Grimm enemy behavior

### UI & Localization
- `UIWindowBase` - Base class for UI windows
- `LocalizedTextMesh` - Localized text component
- `LocalizationSettings` - Localization configuration
- `ChangeFontByLanguage` - Font scaling by language

## Modding Patterns

### Common Patch Targets
1. **HeroController** - For player ability modifications
2. **PlayerData** - For save data manipulation
3. **GameManager** - For game state changes
4. **GameCameras** - For camera behavior modifications

### Harmony Patch Examples
```csharp
// Example: Modifying player damage
[HarmonyPatch(typeof(HeroController), "TakeDamage")]
public class DamagePatch
{
    static void Prefix(HitInstance damageInstance, ref HealthManager healthManager)
    {
        // Modify damage here
    }
}

// Example: Modifying PlayerData
[HarmonyPatch(typeof(PlayerData), "GetPlayerDataBool")]
public class PlayerDataPatch
{
    static void Postfix(string boolName, ref bool __result)
    {
        // Modify return value here
    }
}
```

### Key Events and Delegates
- `HeroController.OnTakenDamage` - Fired when player takes damage
- `HeroController.OnDeath` - Fired when player dies
- `HeroController.heroInPosition` - Fired when hero reaches position

## Namespace Organization

The assembly contains classes organized in various namespaces:
- **Global namespace** - Core game classes (HeroController, GameManager, PlayerData)
- **HutongGames.PlayMaker.Actions** - PlayMaker action classes
- **UnityEngine** - Unity engine classes and extensions

## Important Notes for Modding

1. **Singleton Pattern**: Many key classes use singleton pattern (HeroController, GameManager)
2. **Event-Driven**: The game heavily uses events for communication between systems
3. **PlayMaker Integration**: Many game mechanics are controlled through PlayMaker actions
4. **PlayerData Centralization**: All save data goes through PlayerData class
5. **MonoBehaviour Inheritance**: Most game classes inherit from MonoBehaviour

## Assembly Dependencies

The Assembly-CSharp.dll depends on:
- **UnityEngine** - Core Unity engine
- **PlayMaker** - Visual scripting system
- **GalaxyCSharp** - GOG Galaxy integration
- **InControl** - Input management

This reference provides the foundation for understanding and modifying Hollow Knight's game systems through Harmony patches and other modding techniques. 