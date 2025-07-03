using System;
using UnityEngine;
using CabbyMenu.SyncedReferences;
using CabbyMenu.Utilities;

namespace CabbyMenu.UI.Controls.InputField
{
    /// <summary>
    /// Factory class for creating appropriate input field synchronization instances.
    /// </summary>
    public static class InputFieldSync
    {
        /// <summary>
        /// Creates an appropriate input field sync instance based on the type and parameters.
        /// </summary>
        /// <typeparam name="T">The type of the input field value.</typeparam>
        /// <param name="inputValue">The synced reference for the value.</param>
        /// <param name="validChars">The valid character types.</param>
        /// <param name="size">The size of the input field.</param>
        /// <param name="characterLimit">The character limit for the input field.</param>
        /// <returns>The appropriate input field sync instance.</returns>
        public static BaseInputFieldSync<T> Create<T>(ISyncedReference<T> inputValue, KeyCodeMap.ValidChars validChars, Vector2 size, int characterLimit)
        {
            Type type = typeof(T);

            // Handle string types
            if (type == typeof(string))
            {
                return (BaseInputFieldSync<T>)(object)new StringInputFieldSync((ISyncedReference<string>)(object)inputValue, validChars, size, characterLimit);
            }

            throw new InvalidOperationException("For numeric types, use the overload that requires minValue and maxValue.");
        }

        /// <summary>
        /// Creates an appropriate input field sync instance based on the type and parameters.
        /// </summary>
        /// <typeparam name="T">The type of the input field value.</typeparam>
        /// <param name="inputValue">The synced reference for the value.</param>
        /// <param name="validChars">The valid character types.</param>
        /// <param name="size">The size of the input field.</param>
        /// <param name="characterLimit">The character limit for the input field.</param>
        /// <param name="minValue">The minimum value (for numeric types).</param>
        /// <param name="maxValue">The maximum value (for numeric types).</param>
        /// <returns>The appropriate input field sync instance.</returns>
        public static BaseInputFieldSync<T> Create<T>(ISyncedReference<T> inputValue, KeyCodeMap.ValidChars validChars, Vector2 size, int characterLimit, T minValue, T maxValue)
        {
            Type type = typeof(T);
            if (type == typeof(string))
            {
                throw new InvalidOperationException("minValue and maxValue should not be provided for string types.");
            }
            // Handle numeric types with ranges
            if (IsNumericType(type) || validChars == KeyCodeMap.ValidChars.Numeric || validChars == KeyCodeMap.ValidChars.Decimal)
            {
                return new NumericInputFieldSync<T>(inputValue, validChars, size, characterLimit, minValue, maxValue);
            }
            // Default to numeric for unknown types
            return new NumericInputFieldSync<T>(inputValue, validChars, size, characterLimit, minValue, maxValue);
        }

        /// <summary>
        /// Determines if a type is considered numeric for input field purposes.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>True if the type is numeric, false otherwise.</returns>
        private static bool IsNumericType(Type type)
        {
            return type == typeof(int) || type == typeof(long) || type == typeof(short) || 
                   type == typeof(byte) || type == typeof(uint) || type == typeof(ulong) || 
                   type == typeof(ushort) || type == typeof(sbyte) || type == typeof(float) || 
                   type == typeof(double) || type == typeof(decimal);
        }
    }
}