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
        private readonly string description;
        
        public BoolPatch(FlagDef flagDef, string description = null)
        {
            flag = flagDef;
            this.description = description ?? flagDef.ReadableName;
        }
        
        public bool Get() => FlagManager.GetBoolFlag(flag);
        public void Set(bool value) => FlagManager.SetBoolFlag(flag, value);
        
        public CheatPanel CreatePanel()
        {
            return new TogglePanel(this, description);
        }
    }
} 