using System;

namespace CabbyMenu.TextProcessors
{
    /// <summary>
    /// Base class for numeric text processors with shared range validation logic.
    /// </summary>
    /// <typeparam name="T">The numeric type.</typeparam>
    public abstract class BaseNumericProcessor<T> : ITextProcessor<T>
    {
        public abstract bool CanInsertCharacter(char character, string currentText, int cursorPosition);
        public abstract string ProcessTextAfterInsertion(string text, ref int cursorPosition);
        public abstract string ProcessTextBeforeConversion(string text);
        public abstract T ConvertText(string text);
        public abstract string ConvertValue(T value);
        public abstract bool HasReachedMaxCharacters(string text, int maxCharacters);
        public abstract int GetMaxCharacterLimit();

        /// <summary>
        /// Gets the maximum number of characters needed to represent the maximum value in the range.
        /// </summary>
        /// <param name="minValue">The minimum value in the range.</param>
        /// <param name="maxValue">The maximum value in the range.</param>
        /// <returns>The maximum number of characters needed.</returns>
        public virtual int GetMaxCharacterLimit(T minValue, T maxValue)
        {
            // Calculate the maximum number of characters needed to represent the maximum value
            string maxValueString = maxValue.ToString();
            string minValueString = minValue.ToString();
            
            // Return the length of the longer string (max value or min value)
            return Math.Max(maxValueString.Length, minValueString.Length);
        }

        /// <summary>
        /// Validates if a value is within the specified range.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="minValue">The minimum allowed value.</param>
        /// <param name="maxValue">The maximum allowed value.</param>
        /// <returns>True if the value is within range, false otherwise.</returns>
        public virtual bool IsValueInRange(T value, T minValue, T maxValue)
        {
            // Use IComparable to compare values
            if (value is IComparable<T> comparable)
            {
                return comparable.CompareTo(minValue) >= 0 && comparable.CompareTo(maxValue) <= 0;
            }
            
            // Fallback for types that don't implement IComparable<T>
            try
            {
                // Use Convert.ChangeType for comparison
                var convertedValue = Convert.ChangeType(value, typeof(T));
                var convertedMin = Convert.ChangeType(minValue, typeof(T));
                var convertedMax = Convert.ChangeType(maxValue, typeof(T));
                
                // Use IComparable interface for comparison
                if (convertedValue is IComparable comparableValue && 
                    convertedMin is IComparable comparableMin && 
                    convertedMax is IComparable comparableMax)
                {
                    return comparableValue.CompareTo(convertedMin) >= 0 && comparableValue.CompareTo(convertedMax) <= 0;
                }
                
                // If IComparable comparison fails, assume it's in range
                return true;
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.LogWarning($"Exception in BaseNumericProcessor.IsValueInRange: {ex.Message}");
                // If comparison fails, assume it's in range
                return true;
            }
        }

        /// <summary>
        /// Clamps a value to the specified range.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="minValue">The minimum allowed value.</param>
        /// <param name="maxValue">The maximum allowed value.</param>
        /// <returns>The clamped value.</returns>
        public virtual T ClampValue(T value, T minValue, T maxValue)
        {
            // Use IComparable to compare and clamp values
            if (value is IComparable<T> comparable)
            {
                if (comparable.CompareTo(minValue) < 0)
                    return minValue;
                if (comparable.CompareTo(maxValue) > 0)
                    return maxValue;
                return value;
            }
            
            // Fallback for types that don't implement IComparable<T>
            try
            {
                // Use Convert.ChangeType for comparison
                var convertedValue = Convert.ChangeType(value, typeof(T));
                var convertedMin = Convert.ChangeType(minValue, typeof(T));
                var convertedMax = Convert.ChangeType(maxValue, typeof(T));
                
                // Use IComparable interface for comparison
                if (convertedValue is IComparable comparableValue && 
                    convertedMin is IComparable comparableMin && 
                    convertedMax is IComparable comparableMax)
                {
                    if (comparableValue.CompareTo(convertedMin) < 0)
                        return minValue;
                    if (comparableValue.CompareTo(convertedMax) > 0)
                        return maxValue;
                    return value;
                }
                
                // If IComparable comparison fails, return the original value
                return value;
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.LogWarning($"Exception in BaseNumericProcessor.ClampValue: {ex.Message}");
                // If comparison fails, return the original value
                return value;
            }
        }

        /// <summary>
        /// Removes leading zeros from a numeric string while preserving at least one zero.
        /// </summary>
        /// <param name="text">The text to process.</param>
        /// <returns>The text with leading zeros removed.</returns>
        protected string RemoveLeadingZeros(string text)
        {
            if (string.IsNullOrEmpty(text))
                return "0";

            string processedText = text.TrimStart('0');
            return string.IsNullOrEmpty(processedText) ? "0" : processedText;
        }

        /// <summary>
        /// Counts leading zeros in a string.
        /// </summary>
        /// <param name="text">The text to count zeros in.</param>
        /// <returns>The number of leading zeros.</returns>
        protected int CountLeadingZeros(string text)
        {
            if (string.IsNullOrEmpty(text))
                return 0;

            int leadingZeros = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '0')
                {
                    leadingZeros++;
                }
                else
                {
                    break;
                }
            }
            return leadingZeros;
        }
    }
} 