using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.BasePatches
{
    /// <summary>
    /// Boolean patch implementation
    /// </summary>
    public class BoolPatch : ISyncedReference<bool>, IPatch
    {
        private readonly FlagDef flag;
        
        public BoolPatch(FlagDef flagDef)
        {
            flag = flagDef;
        }
        
        public bool Get() => FlagManager.GetBoolFlag(flag);
        public void Set(bool value) => FlagManager.SetBoolFlag(flag, value);
        
        public CheatPanel CreatePanel()
        {
            return new TogglePanel(this, flag.ReadableName);
        }
    }
} 