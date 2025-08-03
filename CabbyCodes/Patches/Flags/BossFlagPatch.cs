using CabbyCodes.Flags;
using CabbyCodes.Patches.BasePatches;

namespace CabbyCodes.Patches.Flags
{
    public class BossFlagPatch : BasePatch
    {
        protected override FlagDef[] GetFlags()
        {
            return new FlagDef[]
            {
                FlagInstances.falseKnightDefeated,
                FlagInstances.Crossroads_09__Mawlek_Body,
            };
        }
    }
} 