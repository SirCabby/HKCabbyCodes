using CabbyMenu.SyncedReferences;
using System.Reflection;
using HarmonyLib;
using CabbyMenu;

namespace CabbyCodes.Patches.Player
{
    public class GeoPatch : ISyncedReference<bool>
    {
        public const string key = "Geo_Patch";
        private static readonly BoxedReference<bool> value = CodeState.Get(key, false);
        private static readonly Harmony harmony = new Harmony(key);
        private static readonly MethodInfo mOriginal1 = AccessTools.Method(typeof(PlayerData), nameof(PlayerData.TakeGeo));
        private static readonly MethodInfo mOriginal2 = AccessTools.Method(typeof(GeoCounter), nameof(GeoCounter.TakeGeo));

        public bool Get()
        {
            return value.Get();
        }

        public void Set(bool value)
        {
            GeoPatch.value.Set(value);

            if (Get())
            {
                harmony.Patch(mOriginal1, prefix: new HarmonyMethod(typeof(CommonPatches).GetMethod("Prefix_SkipOriginal")));
                harmony.Patch(mOriginal2, prefix: new HarmonyMethod(typeof(CommonPatches).GetMethod("Prefix_SkipOriginal")));
            }
            else
            {
                harmony.UnpatchSelf();
            }
        }
    }
}
