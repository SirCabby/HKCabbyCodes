using System;
using CabbyMenu.Utilities;

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
    }

    /// <summary>
    /// Factory for creating text processors based on type and validation requirements.
    /// </summary>
    public static class TextProcessorFactory
    {
        /// <summary>
        /// Creates an appropriate text processor for the given type and validation requirements.
        /// </summary>
        /// <typeparam name="T">The target type.</typeparam>
        /// <param name="validChars">The validation requirements.</param>
        /// <returns>A text processor instance.</returns>
        public static ITextProcessor<T> Create<T>(KeyCodeMap.ValidChars validChars)
        {
            Type type = typeof(T);

            // Handle string types
            if (type == typeof(string))
            {
                return (ITextProcessor<T>)(object)new StringTextProcessor();
            }

            // Handle decimal types
            if (validChars == KeyCodeMap.ValidChars.Decimal || 
                type == typeof(float) || type == typeof(double) || type == typeof(decimal))
            {
                return new DecimalTextProcessor<T>();
            }

            // Handle numeric types (default for int, long, short, byte, etc.)
            if (validChars == KeyCodeMap.ValidChars.Numeric || 
                type == typeof(int) || type == typeof(long) || type == typeof(short) || 
                type == typeof(byte) || type == typeof(uint) || type == typeof(ulong) || 
                type == typeof(ushort) || type == typeof(sbyte))
            {
                return new NumericTextProcessor<T>();
            }

            // Default to string processor for unknown types
            return (ITextProcessor<T>)(object)new StringTextProcessor();
        }
    }
} 