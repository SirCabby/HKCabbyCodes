using HarmonyLib;
using System.Reflection;

namespace CabbyCodes.Patches
{
    public class InvulPatch : BasePatch
    {
        public const string key = "Invul_Patch";
        private static readonly Harmony harmony = new(key);
        private static readonly MethodInfo mOriginal = AccessTools.Method(typeof(PlayerData), nameof(PlayerData.TakeHealth));

        public override void Patch()
        {
            harmony.Patch(mOriginal, prefix: new HarmonyMethod(typeof(BasePatch).GetMethod(nameof(Prefix_SkipOriginal))));
            CodeState.Get(key).Value = true;
        }

        public override void UnPatch()
        {
            harmony.UnpatchSelf();
            CodeState.Get(key).Value = false;
        }
    }
}
