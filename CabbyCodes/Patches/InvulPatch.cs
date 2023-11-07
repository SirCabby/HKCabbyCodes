using CabbyCodes.SyncedReferences;
using HarmonyLib;
using System.Reflection;

namespace CabbyCodes.Patches
{
    public class InvulPatch : SyncedReference<bool>
    {
        public const string key = "Invul_Patch";
        private static readonly BoxedReference value = CodeState.Get(key, false);
        private static readonly Harmony harmony = new(key);
        private static readonly MethodInfo mOriginal = AccessTools.Method(typeof(PlayerData), nameof(PlayerData.TakeHealth));

        public InvulPatch()
        {
            Get = () =>
            {
                return (bool)value.Get();
            };

            Set = (isOn) =>
            {
                value.Set(isOn);

                if (Get())
                {
                    harmony.Patch(mOriginal, prefix: new HarmonyMethod(typeof(CommonPatches).GetMethod("Prefix_SkipOriginal")));
                }
                else
                {
                    harmony.UnpatchSelf();
                }
            };
        }
    }
}
