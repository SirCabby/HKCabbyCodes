using CabbyCodes.Patches.BasePatches;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Maps
{
    /// <summary>
    /// Boolean patch for map flags that automatically manages the hasMap flag
    /// </summary>
    public class MapBoolPatch : BoolPatch
    {
        public MapBoolPatch(FlagDef flagDef, string description = null) : base(flagDef, description)
        {
        }

        public override void Set(bool value)
        {
            // Set the individual map flag
            FlagManager.SetBoolFlag(flag, value);
            
            // Update the hasMap flag based on whether any map flags are true
            UpdateHasMapFlag();
        }

        private void UpdateHasMapFlag()
        {
            // Check if any map flags are true
            bool anyMapTrue = FlagManager.GetBoolFlag(FlagInstances.mapAbyss) ||
                             FlagManager.GetBoolFlag(FlagInstances.mapCity) ||
                             FlagManager.GetBoolFlag(FlagInstances.mapCliffs) ||
                             FlagManager.GetBoolFlag(FlagInstances.mapCrossroads) ||
                             FlagManager.GetBoolFlag(FlagInstances.mapDeepnest) ||
                             FlagManager.GetBoolFlag(FlagInstances.mapFogCanyon) ||
                             FlagManager.GetBoolFlag(FlagInstances.mapFungalWastes) ||
                             FlagManager.GetBoolFlag(FlagInstances.mapGreenpath) ||
                             FlagManager.GetBoolFlag(FlagInstances.mapMines) ||
                             FlagManager.GetBoolFlag(FlagInstances.mapOutskirts) ||
                             FlagManager.GetBoolFlag(FlagInstances.mapRestingGrounds) ||
                             FlagManager.GetBoolFlag(FlagInstances.mapRoyalGardens) ||
                             FlagManager.GetBoolFlag(FlagInstances.mapWaterways);

            // Set hasMap flag accordingly
            FlagManager.SetBoolFlag(FlagInstances.hasMap, anyMapTrue);
        }
    }
} 