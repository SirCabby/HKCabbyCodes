Lumafly (HKAPI) Build Requirements
===================================

To build the Lumafly version of CabbyCodes, you need to place the 
HKAPI-patched Assembly-CSharp.dll in this folder.

How to obtain the Assembly-CSharp.dll:
--------------------------------------
1. Install the Hollow Knight Modding API using Lumafly mod manager
   - Download Lumafly from: https://themulhima.github.io/Lumafly/
   - Launch Lumafly and install any mod (this will auto-install the Modding API)

2. After the Modding API is installed, copy Assembly-CSharp.dll from:
   [Your Hollow Knight installation]\Hollow Knight_Data\Managed\Assembly-CSharp.dll
   
   To this folder (lib-lumafly)

Note: The Assembly-CSharp.dll in the regular 'lib' folder is for BepInEx builds
and does NOT include the Modding namespace required for HKAPI/Lumafly builds.

The HKAPI-patched DLL contains the 'Modding' namespace with:
- Modding.Mod base class
- Modding.ModHooks for game event hooks
- Other HKAPI features

