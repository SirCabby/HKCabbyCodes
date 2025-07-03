using System;

namespace CabbyMenu.TextProcessors
{
    /// <summary>
    /// Text processor for decimal types (float, double, decimal).
    /// </summary>
    public class DecimalTextProcessor<T> : BaseNumericProcessor<T>
    {
        public override bool CanInsertCharacter(char character, string currentText, int cursorPosition)
        {
            // Allow numeric characters
            if (char.IsDigit(character))
                return true;

            // Allow decimal point with validation
            if (character == '.')
            {
                return CanInsertDecimalPoint(currentText, cursorPosition);
            }

            return false;
        }

        public override string ProcessTextAfterInsertion(string text, ref int cursorPosition)
        {
            // For decimal types, we don't remove leading zeros as they might be significant
            // (e.g., 0.5 is different from .5)
            return text;
        }

        public override string ProcessTextBeforeConversion(string text)
        {
            if (string.IsNullOrEmpty(text))
                return "0";

            // For decimal types, preserve the exact text format
            return text;
        }

        public override T ConvertText(string text)
        {
            return (T)Convert.ChangeType(text, typeof(T));
        }

        public override string ConvertValue(T value)
        {
            return value?.ToString() ?? "0";
        }

        public override bool HasReachedMaxCharacters(string text, int maxCharacters)
        {
            if (string.IsNullOrEmpty(text))
                return false;

            // For decimal types, we need to be more careful about character counting
            // We'll count characters including the decimal point
            
            // Simple character count check
            return text.Length >= maxCharacters;
        }

        public override int GetMaxCharacterLimit()
        {
            // For decimal types, we don't have a predefined limit
            // This should be passed in by the caller
            throw new InvalidOperationException("Decimal text processors require a character limit to be specified.");
        }

        public override int GetMaxCharacterLimit(T minValue, T maxValue)
        {
            // For decimal types, we need to account for the decimal point
            string maxValueString = maxValue.ToString();
            string minValueString = minValue.ToString();
            
            // Return the length of the longer string (max value or min value)
            return Math.Max(maxValueString.Length, minValueString.Length);
        }

        /// <summary>
        /// Validates if a decimal point can be inserted at the current cursor position.
        /// </summary>
        /// <param name="currentText">The current text in the input field.</param>
        /// <param name="cursorPosition">The current cursor position.</param>
        /// <returns>True if the decimal point can be inserted, false otherwise.</returns>
        private bool CanInsertDecimalPoint(string currentText, int cursorPosition)
        {
            // Check if a decimal point already exists in the text
            if (currentText.Contains("."))
            {
                return false;
            }

            // Check if trying to insert decimal point at the beginning (which would create ".123")
            if (cursorPosition == 0)
            {
                return false;
            }

                return true;
        }
    }
} 