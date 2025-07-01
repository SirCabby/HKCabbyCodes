using System;

namespace CabbyMenu.TextProcessors
{
    /// <summary>
    /// Text processor for numeric integer types (int, long, short, byte, etc.).
    /// </summary>
    public class NumericTextProcessor<T> : ITextProcessor<T>
    {
        public bool CanInsertCharacter(char character, string currentText, int cursorPosition)
        {
            // Only allow numeric characters
            return char.IsDigit(character);
        }

        public string ProcessTextAfterInsertion(string text, ref int cursorPosition)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            // Count leading zeros
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

        public string ProcessTextBeforeConversion(string text)
        {
            if (string.IsNullOrEmpty(text))
                return "0";

            // Remove leading zeros for numeric types before conversion
            string processedText = text.TrimStart('0');
            // If all characters were zeros, keep at least one zero
            return string.IsNullOrEmpty(processedText) ? "0" : processedText;
        }

        public T ConvertText(string text)
        {
            return (T)Convert.ChangeType(text, typeof(T));
        }

        public string ConvertValue(T value)
        {
            return value?.ToString() ?? "0";
        }
    }
} 