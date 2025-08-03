using CabbyMenu.Utilities;

namespace CabbyCodes.Flags
{
    /// <summary>
    /// Metadata for float flag validation including ranges and input constraints.
    /// </summary>
    public class FloatFlagValidationMetadata
    {
        public float MinValue { get; }
        public float MaxValue { get; }
        public KeyCodeMap.ValidChars ValidChars { get; }

        public FloatFlagValidationMetadata(float minValue, float maxValue, KeyCodeMap.ValidChars validChars)
        {
            MinValue = minValue;
            MaxValue = maxValue;
            ValidChars = validChars;
        }
    }
} 