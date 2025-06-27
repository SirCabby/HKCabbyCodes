using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using System;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using CabbyMenu;

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

    public class GeoValuePatch : ISyncedReference<int>
    {
        public int Get()
        {
            return PlayerData.instance.geo;
        }

        public void Set(int value)
        {
            value = CabbyMenu.ValidationUtils.ValidateRange(value, 0, Constants.MAX_GEO, nameof(value));
            PlayerData.instance.geo = value;
        }

        public static void AddPanel()
        {
            InputFieldPanel<int> panel = new(new GeoValuePatch(), CabbyMenu.KeyCodeMap.ValidChars.Numeric, 4, Constants.PANEL_WIDTH_120, "Geo (0-999999)");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
        }
    }
}
