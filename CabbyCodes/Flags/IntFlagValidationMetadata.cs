using CabbyMenu.Utilities;

namespace CabbyCodes.Flags
{
    /// <summary>
    /// Metadata for integer flag validation including ranges and input constraints.
    /// </summary>
    public class IntFlagValidationMetadata
    {
        public int MinValue { get; }
        public int MaxValue { get; }
        public KeyCodeMap.ValidChars ValidChars { get; }

        public IntFlagValidationMetadata(int minValue, int maxValue, KeyCodeMap.ValidChars validChars)
        {
            MinValue = minValue;
            MaxValue = maxValue;
            ValidChars = validChars;
        }
    }
} 