using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.Utilities;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.BasePatches
{
    /// <summary>
    /// Integer patch implementation with configurable validation ranges and input constraints
    /// </summary>
    public class IntPatch : ISyncedReference<int>, IPatch
    {
        private readonly FlagDef flag;
        private readonly IntFlagValidationMetadata validationData;
        
        public IntPatch(FlagDef flagDef)
        {
            flag = flagDef;
            validationData = FlagValidationData.GetIntValidationData(flag);
        }
        
        public int Get() => FlagManager.GetIntFlag(flag);
        public void Set(int value) => FlagManager.SetIntFlag(flag, value);
        
        public CheatPanel CreatePanel()
        {
            // Use validation data if available, otherwise fall back to defaults
            if (validationData != null)
            {
                return new RangeInputFieldPanel<int>(
                    this, 
                    validationData.ValidChars, 
                    validationData.MinValue, 
                    validationData.MaxValue, 
                    flag.ReadableName
                );
            }
            
            // Fallback to default behavior for backward compatibility
            return new RangeInputFieldPanel<int>(this, KeyCodeMap.ValidChars.Numeric, 0, 9999, flag.ReadableName);
        }
    }
} 