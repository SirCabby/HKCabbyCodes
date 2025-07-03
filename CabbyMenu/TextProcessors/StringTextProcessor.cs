using System;

namespace CabbyMenu.TextProcessors
{
    /// <summary>
    /// Text processor for string types.
    /// </summary>
    public class StringTextProcessor : ITextProcessor<string>
    {
        public bool CanInsertCharacter(char character, string currentText, int cursorPosition)
        {
            // Allow any printable character
            return !char.IsControl(character);
        }

        public string ProcessTextAfterInsertion(string text, ref int cursorPosition)
        {
            // No special processing for strings
            return text;
        }

        public string ProcessTextBeforeConversion(string text)
        {
            return text ?? string.Empty;
        }

        public string ConvertText(string text)
        {
            return text ?? string.Empty;
        }

        public string ConvertValue(string value)
        {
            return value ?? string.Empty;
        }

        public bool HasReachedMaxCharacters(string text, int maxCharacters)
        {
            // For strings, check if we've reached the maximum character limit
            return text.Length >= maxCharacters;
        }

        public int GetMaxCharacterLimit()
        {
            // For strings, we don't have a predefined limit
            // This should be passed in by the caller
            throw new InvalidOperationException("String text processors require a character limit to be specified.");
        }
    }
} 