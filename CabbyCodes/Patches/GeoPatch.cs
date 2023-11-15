using CabbyCodes.SyncedReferences;
using CabbyCodes.UI.CheatPanels;
using HarmonyLib;
using System.Reflection;

namespace CabbyCodes.Patches
{
    public class GeoPatch : ISyncedReference<bool>
    {
        public const string key = "Geo_Patch";
        private static readonly BoxedReference value = CodeState.Get(key, false);
        private static readonly Harmony harmony = new(key);
        private static readonly MethodInfo mOriginal = AccessTools.Method(typeof(PlayerData), nameof(PlayerData.TakeGeo));

        public bool Get()
        {
            return (bool)value.Get();
        }

        public void Set(bool value)
        {
            GeoPatch.value.Set(value);

            if (Get())
            {
                harmony.Patch(mOriginal, prefix: new HarmonyMethod(typeof(CommonPatches).GetMethod("Prefix_SkipOriginal")));
            }
            else
            {
                harmony.UnpatchSelf();
            }
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new GeoPatch(), "Infinite Geo"));
        }
    }
}
