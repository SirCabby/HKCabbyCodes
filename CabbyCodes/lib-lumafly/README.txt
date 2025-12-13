Lumafly (HKAPI) Build Dependencies
===================================

This folder contains local dependencies required for building the Lumafly 
version of CabbyCodes.

Required Files
--------------
1. Assembly-CSharp.dll - The HKAPI-PATCHED game assembly
2. MonoMod.RuntimeDetour.dll - Runtime method hooking library
3. MonoMod.Utils.dll - MonoMod utility library

How to Obtain These Files
-------------------------
1. Install the Hollow Knight Modding API using Lumafly:
   - Download Lumafly from: https://themulhima.github.io/Lumafly/
   - Launch Lumafly and install any mod (this auto-installs the Modding API)

2. Navigate to the Managed folder:
   [Hollow Knight installation]\hollow_knight_Data\Managed\

3. Copy these files to this folder (lib-lumafly):
   - Assembly-CSharp.dll (this will be the HKAPI-patched version)
   - MonoMod.RuntimeDetour.dll
   - MonoMod.Utils.dll

IMPORTANT: The Assembly-CSharp.dll here MUST be the HKAPI-patched version
that includes the Modding namespace. This is different from the vanilla 
version used for BepInEx builds.

Verifying You Have the Correct DLL
----------------------------------
The HKAPI-patched Assembly-CSharp.dll contains additional namespaces like:
- Modding (the main modding API namespace)

If you're unsure which version you have:
- Install any mod through Lumafly to ensure HKAPI is installed
- The patched DLL will be in the Managed folder afterward

Build Configuration Using This Folder
-------------------------------------
- Lumafly: Uses these files for HKAPI/Lumafly mod builds

For BepInEx builds (5 or 6), use the regular lib folder instead with the 
vanilla Assembly-CSharp.dll.

Why MonoMod DLLs Are Needed Locally
-----------------------------------
Unlike BepInEx builds that get MonoMod via NuGet packages, Lumafly builds 
use the MonoMod libraries that ship with the Hollow Knight Modding API.

This allows the mod to use the same MonoMod.RuntimeDetour.Hook approach 
for all builds, keeping the patch code identical across platforms.

Dependency Notes
----------------
- NuGet only provides UnityEngine.Modules for Lumafly builds
- All other dependencies come from this folder or are standalone implementations
- Config/logging types are provided by CabbyCodes/Lumafly/*.cs files
- These files are excluded from version control due to licensing
