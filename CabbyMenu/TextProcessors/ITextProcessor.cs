using System;

namespace CabbyMenu.TextProcessors
{
    /// <summary>
    /// Interface for type-specific text processing operations.
    /// </summary>
    /// <typeparam name="T">The target type for conversion.</typeparam>
    public interface ITextProcessor<T>
    {
        /// <summary>
        /// Validates if a character can be inserted at the current position.
        /// </summary>
        /// <param name="character">The character to validate.</param>
        /// <param name="currentText">The current text in the input field.</param>
        /// <param name="cursorPosition">The current cursor position.</param>
        /// <returns>True if the character can be inserted, false otherwise.</returns>
        bool CanInsertCharacter(char character, string currentText, int cursorPosition);

        /// <summary>
        /// Processes text after insertion to apply type-specific formatting.
        /// </summary>
        /// <param name="text">The text to process.</param>
        /// <param name="cursorPosition">The current cursor position (will be updated if needed).</param>
        /// <returns>The processed text.</returns>
        string ProcessTextAfterInsertion(string text, ref int cursorPosition);

        /// <summary>
        /// Processes text before conversion to the target type.
        /// </summary>
        /// <param name="text">The text to process.</param>
        /// <returns>The processed text ready for conversion.</returns>
        string ProcessTextBeforeConversion(string text);

        /// <summary>
        /// Converts text to the target type.
        /// </summary>
        /// <param name="text">The text to convert.</param>
        /// <returns>The converted value.</returns>
        T ConvertText(string text);

        /// <summary>
        /// Converts a value back to string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The string representation.</returns>
        string ConvertValue(T value);
    }
} 