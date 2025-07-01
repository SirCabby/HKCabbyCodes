using System;

namespace CabbyMenu.TextProcessors
{
    /// <summary>
    /// Text processor for decimal types (float, double, decimal).
    /// </summary>
    public class DecimalTextProcessor<T> : ITextProcessor<T>
    {
        public bool CanInsertCharacter(char character, string currentText, int cursorPosition)
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

        public string ProcessTextAfterInsertion(string text, ref int cursorPosition)
        {
            // For decimal types, we don't remove leading zeros as they might be significant
            // (e.g., 0.5 is different from .5)
            return text;
        }

        public string ProcessTextBeforeConversion(string text)
        {
            if (string.IsNullOrEmpty(text))
                return "0";

            // For decimal types, preserve the exact text format
            return text;
        }

        public T ConvertText(string text)
        {
            return (T)Convert.ChangeType(text, typeof(T));
        }

        public string ConvertValue(T value)
        {
            return value?.ToString() ?? "0";
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