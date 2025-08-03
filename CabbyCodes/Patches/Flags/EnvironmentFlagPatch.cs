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
                FlagInstances.crossroadsInfected,
                FlagInstances.crossroadsMawlekWall,
                FlagInstances.hornetGreenpath,
                FlagInstances.jijiDoorUnlocked,
                FlagInstances.menderSignBroken,
                FlagInstances.noEyesPinned,
                FlagInstances.menderState,
                FlagInstances.openedMapperShop,
                FlagInstances.shamanPillar,
            };
        }
    }
} 