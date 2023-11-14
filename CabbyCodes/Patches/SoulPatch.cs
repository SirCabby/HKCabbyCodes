using CabbyCodes.SyncedReferences;
using CabbyCodes.UI.CheatPanels;
using HarmonyLib;
using System.Reflection;

namespace CabbyCodes.Patches
{
    public class SoulPatch : ISyncedReference<bool>
    {
        public const string key = "Soul_Patch";
        private static readonly BoxedReference value = CodeState.Get(key, false);
        private static readonly Harmony harmony = new(key);
        private static readonly MethodInfo mOriginal = AccessTools.Method(typeof(PlayerData), nameof(PlayerData.TakeMP));
        private static readonly MethodInfo mOriginal2 = AccessTools.Method(typeof(PlayerData), nameof(PlayerData.TakeReserveMP));
        private static readonly MethodInfo mOriginal3 = AccessTools.Method(typeof(PlayerData), nameof(PlayerData.ClearMP));

        public bool Get()
        {
            return (bool)value.Get();
        }

        public void Set(bool value)
        {
            SoulPatch.value.Set(value);

            if (Get())
            {
                harmony.Patch(mOriginal, prefix: new HarmonyMethod(typeof(CommonPatches).GetMethod("Prefix_SkipOriginal")));
                harmony.Patch(mOriginal2, prefix: new HarmonyMethod(typeof(CommonPatches).GetMethod("Prefix_SkipOriginal")));
                harmony.Patch(mOriginal3, prefix: new HarmonyMethod(typeof(CommonPatches).GetMethod("Prefix_SkipOriginal")));
                PlayerData.instance.AddMPCharge(999);
            }
            else
            {
                harmony.UnpatchSelf();
            }
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new SoulPatch(), "Infinite Soul"));
        }
    }
}
