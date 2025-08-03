using CabbyCodes.Flags;
using CabbyCodes.Patches.BasePatches;

namespace CabbyCodes.Patches.Flags
{
    public class NpcFlagPatch : BasePatch
    {
        protected override FlagDef[] GetFlags()
        {
            return new FlagDef[]
            {
                FlagInstances.corn_crossroadsLeft,
                FlagInstances.corn_greenpathEncountered,
                FlagInstances.corniferIntroduced,
                FlagInstances.Crossroads_45__Zombie_Myla,
                FlagInstances.elderbugFirstCall,
                FlagInstances.elderbugHistory,
                FlagInstances.elderbugHistory1,
                FlagInstances.elderbugHistory2,
                FlagInstances.elderbugHistory3,
                FlagInstances.elderbugSpeechEggTemple,
                FlagInstances.elderbugSpeechMapShop,
                FlagInstances.elderbugSpeechSly,
                FlagInstances.hunterRoared,
                FlagInstances.metCornifer,
                FlagInstances.metElderbug,
                FlagInstances.metHunter,
                FlagInstances.metIselda,
                FlagInstances.jijiMet,
                FlagInstances.metMiner,
                FlagInstances.metQuirrel,
                FlagInstances.metSlyShop,
                FlagInstances.quirrelEggTemple,
                FlagInstances.quirrelLeftEggTemple,
                FlagInstances.shaman,
                FlagInstances.slyRescued,
                FlagInstances.Town__Gravedigger_NPC,
            };
        }
    }
} 