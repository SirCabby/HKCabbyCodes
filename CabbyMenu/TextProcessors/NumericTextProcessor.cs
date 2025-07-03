using System;

namespace CabbyMenu.TextProcessors
{
    /// <summary>
    /// Text processor for numeric integer types (int, long, short, byte, etc.).
    /// </summary>
    public class NumericTextProcessor<T> : BaseNumericProcessor<T>
    {
        public override bool CanInsertCharacter(char character, string currentText, int cursorPosition)
        {
            // Only allow numeric characters
            return char.IsDigit(character);
        }

        public override string ProcessTextAfterInsertion(string text, ref int cursorPosition)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            // Count leading zeros
            int leadingZeros = CountLeadingZeros(text);

            // If there are leading zeros and we have at least one non-zero digit, remove them
            if (leadingZeros > 0 && leadingZeros < text.Length)
            {
                // Adjust cursor position to account for removed zeros
                if (cursorPosition <= leadingZeros)
                {
                    // If cursor was within the leading zeros, move it to position 0
                    cursorPosition = 0;
                }
                else
                {
                    // If cursor was after the leading zeros, adjust it by the number of zeros removed
                    cursorPosition -= leadingZeros;
                }

                // Remove the leading zeros
                return text.Substring(leadingZeros);
            }

            return text;
        }

        public override string ProcessTextBeforeConversion(string text)
        {
            return RemoveLeadingZeros(text);
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

            // Remove leading zeros for comparison
            string cleanText = RemoveLeadingZeros(text);
            if (string.IsNullOrEmpty(cleanText) || cleanText == "0")
                return false;

            // Compare the length of the clean text with the maximum characters allowed
            return cleanText.Length >= maxCharacters;
        }

        public override int GetMaxCharacterLimit()
        {
            // For numeric types, we don't have a predefined limit
            // This should be passed in by the caller
            throw new InvalidOperationException("Numeric text processors require a character limit to be specified.");
        }
    }
} 