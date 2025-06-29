using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using System.Reflection;
using HarmonyLib;
using CabbyMenu;

namespace CabbyCodes.Patches.Player
{
    public class InvulPatch : ISyncedReference<bool>
    {
        public const string key = "Invul_Patch";
        private static readonly BoxedReference value = CodeState.Get(key, false);
        private static readonly Harmony harmony = new Harmony(key);
        private static readonly MethodInfo mOriginal = AccessTools.Method(typeof(PlayerData), nameof(PlayerData.TakeHealth));
        private static readonly MethodInfo mOriginal2 = AccessTools.Method(typeof(PlayerData), nameof(PlayerData.WouldDie));

        public bool Get()
        {
            return (bool)value.Get();
        }

        public void Set(bool value)
        {
            InvulPatch.value.Set(value);

            if (Get())
            {
                harmony.Patch(mOriginal, prefix: new HarmonyMethod(typeof(CommonPatches).GetMethod("Prefix_SkipOriginal")));
                harmony.Patch(mOriginal2, prefix: new HarmonyMethod(typeof(CommonPatches).GetMethod("Prefix_SkipOriginal")));
                PlayerData.instance.isInvincible = true;
                PlayerData.instance.MaxHealth();
            }
            else
            {
                harmony.UnpatchSelf();
                PlayerData.instance.isInvincible = false;
            }
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new InvulPatch(), "Invulnerability"));
        }
    }
}
