using HarmonyLib;
using System.Reflection;

namespace CabbyCodes.Patches
{
    public class InvulPatch : BasePatch
    {
        static readonly Harmony harmony = new("Invul_Patch");
        static readonly MethodInfo mOriginal = AccessTools.Method(typeof(PlayerData), nameof(PlayerData.TakeHealth));

        public override void Patch()
        {
            harmony.Patch(mOriginal, prefix: new HarmonyMethod(typeof(BasePatch).GetMethod(nameof(Prefix_SkipOriginal))));
        }

        public override void UnPatch()
        {
            harmony.UnpatchSelf();
        }
    }
}
