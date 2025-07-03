using UnityEngine;
using CabbyMenu.SyncedReferences;
using CabbyMenu.TextProcessors;
using CabbyMenu.Utilities;
using System;

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

            // Create the numeric input field status directly
            if (textProcessor is BaseNumericProcessor<T> numericProcessor)
            {
                var tType = typeof(T);
                if (tType.IsValueType && typeof(IComparable<T>).IsAssignableFrom(tType))
                {
                    var status = (InputFieldStatusBase)Activator.CreateInstance(
                        typeof(NumericInputFieldStatus<>).MakeGenericType(tType),
                        new object[] { inputFieldGo, (Action<bool>)SetSelected, (Action)Submit, (Action)Cancel, CalculateMaxVisibleCharacters(size.x, Constants.DEFAULT_FONT_SIZE), numericProcessor, minValue, maxValue });
                    // Use the protected method from base class
                    SetInputFieldStatus(status);
                }
                else
                {
                    throw new InvalidOperationException("NumericInputFieldSync requires a value type implementing IComparable<T>.");
                }
            }
            else
            {
                throw new InvalidOperationException("NumericInputFieldSync requires a numeric text processor.");
            }
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