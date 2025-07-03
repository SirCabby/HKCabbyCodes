using System;
using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.Controls.InputField;
using CabbyMenu.Utilities;
using CabbyMenu.TextProcessors;
using UnityEngine.UI;
using UnityEngine;

namespace CabbyMenu.UI.CheatPanels
{
    /// <summary>
    /// Input field panel that automatically calculates character limits from min/max values.
    /// Only works with numeric types.
    /// </summary>
    /// <typeparam name="T">The numeric type (must be a value type that can be compared).</typeparam>
    public class RangeInputFieldPanel<T> : CheatPanel where T : struct, IComparable<T>
    {
        private readonly BaseInputFieldSync<T> inputFieldSync;

        /// <summary>
        /// Creates a new range-based input field panel.
        /// </summary>
        /// <param name="syncedReference">The synced reference for the value.</param>
        /// <param name="validChars">The valid character types (must be Numeric or Decimal).</param>
        /// <param name="minValue">The minimum allowed value.</param>
        /// <param name="maxValue">The maximum allowed value.</param>
        /// <param name="description">The description text for the panel.</param>
        public RangeInputFieldPanel(ISyncedReference<T> syncedReference, KeyCodeMap.ValidChars validChars, T minValue, T maxValue, string description) : base(description)
        {
            // Validate that validChars is appropriate for numeric types
            if (validChars != KeyCodeMap.ValidChars.Numeric && validChars != KeyCodeMap.ValidChars.Decimal)
            {
                throw new ArgumentException($"RangeInputFieldPanel requires Numeric or Decimal validChars, but got {validChars}", nameof(validChars));
            }

            // Validate that minValue is less than or equal to maxValue
            if (minValue.CompareTo(maxValue) > 0)
            {
                throw new ArgumentException($"minValue ({minValue}) must be less than or equal to maxValue ({maxValue})", nameof(minValue));
            }

            // Create text processor to calculate character limit
            var textProcessor = TextProcessor.Create<T>(validChars);
            
            // Since we know this is a numeric type, we can safely cast to BaseNumericProcessor
            if (textProcessor is BaseNumericProcessor<T> numericProcessor)
            {
                int characterLimit = numericProcessor.GetMaxCharacterLimit(minValue, maxValue);
                
                // Calculate the width based on character limit
                int calculatedWidth = CalculatePanelWidth(characterLimit);
                
                // Debug logging to verify width calculation
                UnityEngine.Debug.Log($"RangeInputFieldPanel: minValue={minValue}, maxValue={maxValue}, characterLimit={characterLimit}, calculatedWidth={calculatedWidth}, description='{description}'");
                
                inputFieldSync = InputFieldSync.Create(syncedReference, validChars, new Vector2(calculatedWidth, Constants.DEFAULT_PANEL_HEIGHT), characterLimit, minValue, maxValue);
                new Fitter(inputFieldSync.GetGameObject()).Attach(cheatPanel);
                inputFieldSync.GetGameObject().transform.SetAsFirstSibling();
                
                // Add LayoutElement to set the calculated width as preferred width (not flexible)
                LayoutElement inputFieldLayout = inputFieldSync.GetGameObject().AddComponent<LayoutElement>();
                inputFieldLayout.preferredWidth = calculatedWidth;
                inputFieldLayout.minWidth = calculatedWidth;
                
                updateActions.Add(inputFieldSync.Update);
            }
            else
            {
                // This should never happen due to the factory logic, but provide a clear error
                throw new InvalidOperationException($"TextProcessor.Create returned a non-numeric processor for type {typeof(T).Name}");
            }
        }

        public InputField GetInputField()
        {
            return inputFieldSync.GetGameObject().GetComponent<InputField>();
        }

        /// <summary>
        /// Calculates the panel width based on character limit, ensuring it's never less than the minimum width.
        /// </summary>
        /// <param name="characterLimit">The maximum number of characters allowed.</param>
        /// <returns>The calculated width in pixels.</returns>
        private static int CalculatePanelWidth(int characterLimit)
        {
            // Use the cursor character width for panel sizing to match visible character logic
            float estimatedCharWidth = CalculateCursorCharacterWidth(Constants.DEFAULT_FONT_SIZE);
            float uiBuffer = 10f;
            float calculatedWidth = (characterLimit * estimatedCharWidth) + uiBuffer;
            return Mathf.Max(Constants.MIN_PANEL_WIDTH, Mathf.RoundToInt(calculatedWidth));
        }

        /// <summary>
        /// Calculates the character width for cursor positioning calculations.
        /// </summary>
        /// <param name="fontSize">The font size in pixels.</param>
        /// <returns>The estimated character width for cursor positioning.</returns>
        private static float CalculateCursorCharacterWidth(int fontSize)
        {
            return fontSize * 0.65f;
        }
    }
} 