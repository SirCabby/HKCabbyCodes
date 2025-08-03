using CabbyCodes.Flags;
using CabbyCodes.Patches.BasePatches;

namespace CabbyCodes.Patches.Flags
{
    public class EnvironmentFlagPatch : BasePatch
    {
        protected override FlagDef[] GetFlags()
        {
            return new FlagDef[]
            {
                FlagInstances.openedMapperShop,
                FlagInstances.jijiDoorUnlocked,
                FlagInstances.menderSignBroken,
                FlagInstances.menderState, // Might need to merge with prior flag
                FlagInstances.crossroadsInfected,
                FlagInstances.crossroadsMawlekWall,
                FlagInstances.shamanPillar,

                FlagInstances.noEyesPinned, // keep?
            };
        }
    }
} 