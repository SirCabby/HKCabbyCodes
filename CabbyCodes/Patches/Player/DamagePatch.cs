using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using System.Reflection;
using HarmonyLib;
using CabbyMenu;

namespace CabbyCodes.Patches.Player
{
    public class DamagePatch : ISyncedReference<bool>
    {
        public const string key = "Damage_Patch";
        private static readonly BoxedReference<bool> value = CodeState.Get(key, false);
        private static readonly Harmony harmony = new Harmony(key);
        private static readonly MethodInfo mOriginal1 = AccessTools.Method(typeof(HealthManager), nameof(HealthManager.Hit), new System.Type[] { typeof(HitInstance) });
        private static readonly MethodInfo mOriginal2 = typeof(HealthManager).GetMethod("TakeDamage", BindingFlags.NonPublic | BindingFlags.Instance);

        public bool Get()
        {
            return value.Get();
        }

        public void Set(bool value)
        {
            DamagePatch.value.Set(value);

            if (Get())
            {
                harmony.Patch(mOriginal1, prefix: new HarmonyMethod(typeof(DamagePatch).GetMethod(nameof(Hit_Override), BindingFlags.Static | BindingFlags.Public)));
            }
            else
            {
                harmony.UnpatchSelf();
            }
        }

        //HealthManager.Hit();
        public static bool Hit_Override(HitInstance hitInstance, HealthManager __instance)
        {
            hitInstance.DamageDealt = Constants.ONE_HIT_KILL_DAMAGE;
            hitInstance.IgnoreInvulnerable = true;

            if (!__instance.isDead)
            {
                FSMUtility.SendEventToGameObject(hitInstance.Source, "DEALT DAMAGE");
                mOriginal2.Invoke(__instance, new object[] { hitInstance });
            }

            return false;
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new DamagePatch(), "One Hit Kills (Enemies can't block)"));
        }
    }
}
