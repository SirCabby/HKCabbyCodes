using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using System.Reflection;
using HarmonyLib;
using CabbyMenu;

namespace CabbyCodes.Patches.Player
{
    public class SoulPatch : ISyncedReference<bool>
    {
        public const string key = "Soul_Patch";
        private static readonly BoxedReference<bool> value = CodeState.Get(key, false);
        private static readonly Harmony harmony = new Harmony(key);
        private static readonly MethodInfo mOriginal = AccessTools.Method(typeof(PlayerData), nameof(PlayerData.TakeMP));
        private static readonly MethodInfo mOriginal2 = AccessTools.Method(typeof(PlayerData), nameof(PlayerData.TakeReserveMP));
        private static readonly MethodInfo mOriginal3 = AccessTools.Method(typeof(PlayerData), nameof(PlayerData.ClearMP));

        public bool Get()
        {
            return value.Get();
        }

        public void Set(bool value)
        {
            SoulPatch.value.Set(value);

            if (Get())
            {
                harmony.Patch(mOriginal, prefix: new HarmonyMethod(typeof(CommonPatches).GetMethod("Prefix_SkipOriginal")));
                harmony.Patch(mOriginal2, prefix: new HarmonyMethod(typeof(CommonPatches).GetMethod("Prefix_SkipOriginal")));
                harmony.Patch(mOriginal3, prefix: new HarmonyMethod(typeof(CommonPatches).GetMethod("Prefix_SkipOriginal")));
                PlayerData.instance.AddMPCharge(Constants.INFINITE_SOUL_CHARGE);
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
