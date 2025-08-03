using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.Utilities;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.BasePatches
{
    /// <summary>
    /// Float patch implementation with configurable validation ranges and input constraints
    /// </summary>
    public class FloatPatch : ISyncedReference<float>, IPatch
    {
        private readonly FlagDef flag;
        private readonly FloatFlagValidationMetadata validationData;
        
        public FloatPatch(FlagDef flagDef)
        {
            flag = flagDef;
            validationData = FlagValidationData.GetFloatValidationData(flag);
        }
        
        public float Get() => FlagManager.GetFloatFlag(flag);
        public void Set(float value) => FlagManager.SetFloatFlag(flag, value);
        
        public CheatPanel CreatePanel()
        {
            // Use validation data if available, otherwise fall back to defaults
            if (validationData != null)
            {
                return new RangeInputFieldPanel<float>(
                    this, 
                    validationData.ValidChars, 
                    validationData.MinValue, 
                    validationData.MaxValue, 
                    flag.ReadableName
                );
            }
            
            // Fallback to default behavior for backward compatibility
            return new RangeInputFieldPanel<float>(this, KeyCodeMap.ValidChars.Decimal, 0f, 9999f, flag.ReadableName);
        }
    }
} 