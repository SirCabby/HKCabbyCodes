using CabbyCodes.SyncedReferences;
using CabbyCodes.UI.CheatPanels;
using System.Collections.Generic;

namespace CabbyCodes.Patches
{
    public class GrimmChildLevelPatch : ISyncedValueList<int>
    {
        public int Get()
        {
            int result = PlayerData.instance.grimmChildLevel - 1;
            CabbyCodesPlugin.BLogger.LogInfo("returning out value: " + result);
            return result;
        }

        public void Set(int value)
        {
            int result = value + 1;
            CabbyCodesPlugin.BLogger.LogInfo("setting inc value: " + result);
            PlayerData.instance.grimmChildLevel = result;
        }

        public List<string> GetValueList()
        {
            return new()
            {
                "1", "2", "3", "4", "5"
            };
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new DropdownPanel(new GrimmChildLevelPatch(), 100, "Grimm Child Level (1-5)"));
        }
    }
}
