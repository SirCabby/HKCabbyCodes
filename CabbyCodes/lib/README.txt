BepInEx Build Requirements
==========================

To build the BepInEx version of CabbyCodes, you need to place the 
vanilla Assembly-CSharp.dll in this folder.

How to obtain the Assembly-CSharp.dll:
--------------------------------------
1. Locate your Hollow Knight installation folder
   - Steam: Right-click Hollow Knight > Manage > Browse local files
   - GOG: Check your GOG Games folder

2. Copy Assembly-CSharp.dll from:
   [Your Hollow Knight installation]\Hollow Knight_Data\Managed\Assembly-CSharp.dll
   
   To this folder (lib)

Note: This folder is for vanilla (unmodified) Hollow Knight builds using BepInEx.
If you need to build for Lumafly/HKAPI, use the 'lib-lumafly' folder instead
with the HKAPI-patched Assembly-CSharp.dll.

The vanilla DLL contains the base game classes without the Modding API:
- PlayerData for save data access
- HeroController for player control
- GameManager for game state
- Other base game components

