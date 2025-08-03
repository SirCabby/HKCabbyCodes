using CabbyMenu.Utilities;

namespace CabbyCodes.Flags
{
    /// <summary>
    /// Contains validation metadata for flags, including acceptable ranges and valid character types.
    /// </summary>
    public static class FlagValidationData
    {
        /// <summary>
        /// Gets the validation metadata for a specific float flag.
        /// </summary>
        /// <param name="flag">The flag to get validation data for.</param>
        /// <returns>The validation metadata, or null if no specific validation is defined.</returns>
        public static FloatFlagValidationMetadata GetFloatValidationData(FlagDef flag)
        {
            if (flag == null) return null;

            // Check for specific float flag validations
            switch (flag.Id)
            {
                case "playTime":
                    return new FloatFlagValidationMetadata(0f, 999999f, KeyCodeMap.ValidChars.Decimal);
                
                case "completionPercent":
                case "completionPercentage":
                    return new FloatFlagValidationMetadata(0f, 100f, KeyCodeMap.ValidChars.Decimal);
                
                default:
                    // For unknown float flags, provide reasonable defaults
                    if (flag.Type == "PlayerData_Single")
                    {
                        return new FloatFlagValidationMetadata(0f, 9999f, KeyCodeMap.ValidChars.Decimal);
                    }
                    return null;
            }
        }

        /// <summary>
        /// Gets the validation metadata for a specific integer flag.
        /// </summary>
        /// <param name="flag">The flag to get validation data for.</param>
        /// <returns>The validation metadata, or null if no specific validation is defined.</returns>
        public static IntFlagValidationMetadata GetIntValidationData(FlagDef flag)
        {
            if (flag == null) return null;

            // Check for specific integer flag validations
            switch (flag.Id)
            {
                case "heartPieces":
                    return new IntFlagValidationMetadata(0, 4, KeyCodeMap.ValidChars.Numeric);
                
                case "ore":
                    return new IntFlagValidationMetadata(0, 6, KeyCodeMap.ValidChars.Numeric);
                
                case "rancidEggs":
                    return new IntFlagValidationMetadata(0, 80, KeyCodeMap.ValidChars.Numeric);
                
                case "simpleKeys":
                    return new IntFlagValidationMetadata(0, 4, KeyCodeMap.ValidChars.Numeric);
                
                case "grubsCollected":
                    return new IntFlagValidationMetadata(0, 46, KeyCodeMap.ValidChars.Numeric);
                
                case "grubRewards":
                    return new IntFlagValidationMetadata(0, 23, KeyCodeMap.ValidChars.Numeric);
                
                case "permadeathMode":
                    return new IntFlagValidationMetadata(0, 1, KeyCodeMap.ValidChars.Numeric);
                
                case "profileID":
                    return new IntFlagValidationMetadata(1, 4, KeyCodeMap.ValidChars.Numeric);
                
                case "currentArea":
                    return new IntFlagValidationMetadata(0, 999, KeyCodeMap.ValidChars.Numeric);

                case "stationsOpened":
                    return new IntFlagValidationMetadata(0, 999, KeyCodeMap.ValidChars.Numeric);
                
                default:
                    // For unknown integer flags, provide reasonable defaults
                    if (flag.Type == "PlayerData_Int")
                    {
                        return new IntFlagValidationMetadata(0, 9999, KeyCodeMap.ValidChars.Numeric);
                    }
                    return null;
            }
        }
    }
} 