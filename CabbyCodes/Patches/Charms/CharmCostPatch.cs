using CabbyMenu.SyncedReferences;
using CabbyMenu.Types;
using CabbyMenu.UI.CheatPanels;
using System;
using CabbyCodes.Types;
using CabbyMenu;

namespace CabbyCodes.Patches
{
    public class CharmCostPatch : ISyncedReference<bool>
    {
        public const string key = "CharmCost_Patch";
        private static readonly string charmCostName = "charmCost_";
        private static readonly BoxedReference value = CodeState.Get(key, false);

        public bool Get()
        {
            return (bool)value.Get();
        }

        public void Set(bool value)
        {
            if (value)
            {
                foreach (Charm charm in CharmPatch.charms)
                {
                    PlayerData.instance.SetInt(charmCostName + charm.id, 0);
                }
            }
            else
            {
                foreach (Charm charm in CharmPatch.charms)
                {
                    PlayerData.instance.SetInt(charmCostName + charm.id, charm.defaultCost);
                }
            }

            CharmCostPatch.value.Set(value);
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new TogglePanel(new CharmCostPatch(), "Remove Charm Notch Cost"));
        }
    }
}
