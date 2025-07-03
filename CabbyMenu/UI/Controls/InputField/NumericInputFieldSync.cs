using System;
using UnityEngine;
using CabbyMenu.SyncedReferences;
using CabbyMenu.TextProcessors;
using CabbyMenu.Utilities;

namespace CabbyMenu.UI.Controls.InputField
{
    /// <summary>
    /// Input field synchronization for numeric types with range validation and clamping.
    /// </summary>
    /// <typeparam name="T">The numeric type.</typeparam>
    public class NumericInputFieldSync<T> : BaseInputFieldSync<T>
    {
        private readonly T minValue;
        private readonly T maxValue;

        public NumericInputFieldSync(ISyncedReference<T> inputValue, KeyCodeMap.ValidChars validChars, Vector2 size, int characterLimit, T minValue, T maxValue) 
            : base(inputValue, validChars, size, characterLimit)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
        }

        protected override InputFieldStatus<T> CreateInputFieldStatus(KeyCodeMap.ValidChars validChars, int maxVisibleCharacters)
        {
            return new InputFieldStatus<T>(inputFieldGo, SetSelected, Submit, Cancel, validChars, maxVisibleCharacters, textProcessor, maxValue);
        }

        protected override void HandleSubmit(string text)
        {
            // Convert the text to the target type
            T convertedValue = textProcessor.ConvertText(text);
            
            // Apply range validation and clamping
            if (textProcessor is BaseNumericProcessor<T> numericProcessor)
            {
                if (numericProcessor.IsValueInRange(convertedValue, minValue, maxValue))
                {
                    InputValue.Set(convertedValue);
                }
                else
                {
                    // Clamp the value to the valid range
                    T clampedValue = numericProcessor.ClampValue(convertedValue, minValue, maxValue);
                    InputValue.Set(clampedValue);
                }
            }
            else
            {
                // Fallback for non-numeric processors
                InputValue.Set(convertedValue);
            }
        }
    }
} 