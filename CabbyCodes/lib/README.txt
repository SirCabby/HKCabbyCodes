BepInEx Build Dependencies
==========================

This folder contains local dependencies required for building BepInEx versions 
(both BepInEx 5 and BepInEx 6) of CabbyCodes.

Required Files
--------------
1. Assembly-CSharp.dll - The VANILLA (unmodified) game assembly
2. PlayMaker.dll - PlayMaker FSM library used by the game

Optional Files
--------------
3. UnityExplorer.STANDALONE.Mono.dll - In-game debug inspector (optional)

How to Obtain Assembly-CSharp.dll and PlayMaker.dll
---------------------------------------------------
1. Locate your Hollow Knight installation folder:
   - Steam: Right-click Hollow Knight > Manage > Browse local files
   - GOG: Check your GOG Games folder

2. Navigate to the Managed folder:
   [Hollow Knight installation]\hollow_knight_Data\Managed\

3. Copy these files to this folder (lib):
   - Assembly-CSharp.dll
   - PlayMaker.dll

IMPORTANT: For BepInEx builds, you need the VANILLA Assembly-CSharp.dll
(the original game file without any modding API modifications).

If you have Lumafly/HKAPI installed, the Managed folder will contain the 
PATCHED version. To get the vanilla version:
- Verify game files through Steam (Properties > Installed Files > Verify)
- Or temporarily uninstall the Modding API through Lumafly

How to Obtain UnityExplorer (Optional)
--------------------------------------
1. Download from: https://github.com/sinai-dev/UnityExplorer/releases
2. Get the STANDALONE.Mono version
3. Place UnityExplorer.STANDALONE.Mono.dll in this folder

UnityExplorer provides an in-game debug interface for inspecting game objects.
Press F7 in-game to open it.

Build Configurations Using This Folder
--------------------------------------
- BepInEx5: Uses these files with BepInEx 5.x NuGet packages
- BepInEx6: Uses these files with BepInEx 6.x NuGet packages

For Lumafly builds, use the lib-lumafly folder instead with the 
HKAPI-patched Assembly-CSharp.dll.

Dependency Notes
----------------
- NuGet packages (BepInEx, MonoMod, Unity) are restored automatically
- Only Assembly-CSharp.dll and PlayMaker.dll need to be manually obtained
- These files are excluded from version control due to licensing
