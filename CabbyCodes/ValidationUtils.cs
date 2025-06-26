using System;

namespace CabbyCodes
{
    /// <summary>
    /// Utility class for input validation and data verification.
    /// </summary>
    public static class ValidationUtils
    {
        /// <summary>
        /// Validates if a value is within the specified range.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="min">Minimum allowed value.</param>
        /// <param name="max">Maximum allowed value.</param>
        /// <param name="paramName">Name of the parameter for error messages.</param>
        /// <returns>The validated value (capped to the range).</returns>
        public static int ValidateRange(int value, int min, int max, string paramName)
        {
            return Math.Max(min, Math.Min(max, value));
        }

        /// <summary>
        /// Validates if a string is not null or empty.
        /// </summary>
        /// <param name="value">The string to validate.</param>
        /// <param name="paramName">Name of the parameter for error messages.</param>
        /// <returns>The validated string or empty string if validation fails.</returns>
        public static string ValidateNotNullOrEmpty(string value, string paramName)
        {
            if (string.IsNullOrEmpty(value))
            {
                CabbyCodesPlugin.BLogger?.LogWarning($"{paramName} cannot be null or empty. Using empty string as fallback.");
                return string.Empty;
            }
            return value;
        }

        /// <summary>
        /// Validates if an object is not null.
        /// </summary>
        /// <param name="value">The object to validate.</param>
        /// <param name="paramName">Name of the parameter for error messages.</param>
        /// <returns>The validated object or null if validation fails.</returns>
        public static T ValidateNotNull<T>(T value, string paramName) where T : class
        {
            if (value == null)
            {
                CabbyCodesPlugin.BLogger?.LogWarning($"{paramName} cannot be null. Using null as fallback.");
                return null;
            }
            return value;
        }

        /// <summary>
        /// Safely converts a string to an integer with validation.
        /// </summary>
        /// <param name="value">The string to convert.</param>
        /// <param name="defaultValue">Default value if conversion fails.</param>
        /// <returns>The converted integer or default value.</returns>
        public static int SafeParseInt(string value, int defaultValue = 0)
        {
            if (string.IsNullOrEmpty(value))
                return defaultValue;
            return int.TryParse(value, out int result) ? result : defaultValue;
        }
    }
} 