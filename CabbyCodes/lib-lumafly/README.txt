Lumafly (HKAPI) Build Requirements
===================================

To build the Lumafly version of CabbyCodes, you need to place the following 
DLLs from your HKAPI-patched Hollow Knight installation in this folder:

Required Files:
---------------
1. Assembly-CSharp.dll - The HKAPI-patched game assembly
2. MonoMod.RuntimeDetour.dll - For runtime method hooking
3. MonoMod.Utils.dll - MonoMod utility library

How to obtain these DLLs:
-------------------------
1. Install the Hollow Knight Modding API using Lumafly mod manager
   - Download Lumafly from: https://themulhima.github.io/Lumafly/
   - Launch Lumafly and install any mod (this will auto-install the Modding API)

2. After the Modding API is installed, copy from:
   [Your Hollow Knight installation]\hollow_knight_Data\Managed\

   Copy these files to this folder (lib-lumafly):
   - Assembly-CSharp.dll
   - MonoMod.RuntimeDetour.dll
   - MonoMod.Utils.dll

Note: The Assembly-CSharp.dll in the regular 'lib' folder is for BepInEx builds
and does NOT include the Modding namespace required for HKAPI/Lumafly builds.

Architecture Note:
------------------
This mod uses MonoMod.RuntimeDetour.Hook for ALL builds (BepInEx and Lumafly).
This allows the patch code to be identical across all platforms, avoiding
code duplication with #if conditionals.
