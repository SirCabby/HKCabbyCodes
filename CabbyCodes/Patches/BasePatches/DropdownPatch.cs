using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Flags;
using System.Collections.Generic;

namespace CabbyCodes.Patches.BasePatches
{
    /// <summary>
    /// Dropdown patch implementation for custom value lists
    /// </summary>
    public class DropdownPatch : ISyncedValueList, IPatch
    {
        private readonly FlagDef flag;
        private readonly List<string> valueList;
        private readonly string description;
        
        public DropdownPatch(FlagDef flagDef, List<string> values, string desc)
        {
            flag = flagDef;
            valueList = values;
            description = desc;
        }
        
        public int Get() => FlagManager.GetIntFlag(flag);
        public void Set(int value) => FlagManager.SetIntFlag(flag, value);
        
        public List<string> GetValueList() => valueList;
        
        public CheatPanel CreatePanel()
        {
            return new DropdownPanel(this, description, Constants.DEFAULT_PANEL_HEIGHT);
        }
    }
} 