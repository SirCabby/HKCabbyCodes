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
        protected readonly FlagDef flag;
        protected readonly string description;
        
        public BoolPatch(FlagDef flagDef, string description = null)
        {
            flag = flagDef;
            this.description = description ?? flagDef.ReadableName;
        }
        
        public virtual bool Get() => FlagManager.GetBoolFlag(flag);
        public virtual void Set(bool value) => FlagManager.SetBoolFlag(flag, value);
        
        public virtual CheatPanel CreatePanel()
        {
            return new TogglePanel(this, description);
        }
    }
} 