using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Flags;
using System.Collections.Generic;

namespace CabbyCodes.Patches.BasePatches
{
    /// <summary>
    /// Dropdown patch implementation for custom value lists
    /// Supports both simple single-flag cases and complex multi-flag cases
    /// </summary>
    public class DropdownPatch : ISyncedValueList, IPatch
    {
        protected readonly FlagDef flag;
        protected readonly List<string> valueList;
        protected readonly string description;
        
        public DropdownPatch(FlagDef flagDef, List<string> values, string desc)
        {
            flag = flagDef;
            valueList = values;
            description = desc;
        }
        
        public virtual int Get() => FlagManager.GetIntFlag(flag);
        public virtual void Set(int value) => FlagManager.SetIntFlag(flag, value);
        
        public List<string> GetValueList() => valueList;
        
        public CheatPanel CreatePanel()
        {
            return new DropdownPanel(this, description, Constants.DEFAULT_PANEL_HEIGHT);
        }
    }
} 