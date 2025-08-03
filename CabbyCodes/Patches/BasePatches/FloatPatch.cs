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
        private readonly string description;
        
        public FloatPatch(FlagDef flagDef, string description = null)
        {
            flag = flagDef;
            this.description = description ?? flagDef.ReadableName;
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
                    description
                );
            }
            
            // Fallback to default behavior for backward compatibility
            return new RangeInputFieldPanel<float>(this, KeyCodeMap.ValidChars.Decimal, 0f, 9999f, description);
        }
    }
} 