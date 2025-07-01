using System;
using UnityEngine;
using UnityEngine.UI;
using CabbyMenu.SyncedReferences;
using CabbyMenu.TextProcessors;
using CabbyMenu.UI.Modders;

namespace CabbyMenu.UI.Controls.InputField
{
    public class InputFieldSync<T>
    {
        private static readonly Color selectedColor = Constants.SELECTED_COLOR;

        private readonly GameObject inputFieldGo;
        private readonly UnityEngine.UI.InputField inputField;
        private readonly ISyncedReference<T> InputValue;
        private readonly InputFieldStatus<T> inputFieldStatus;
        private readonly ITextProcessor<T> textProcessor;

        /// <summary>
        /// Action for registering input field sync. Can be set by the consuming project.
        /// </summary>
        public static Action<InputFieldStatus<T>> RegisterInputFieldSync { get; set; } = null;

        public InputFieldSync(ISyncedReference<T> inputValue, Utilities.KeyCodeMap.ValidChars validChars, Vector2 size, int characterLimit = Constants.DEFAULT_CHARACTER_LIMIT)
        {
            InputValue = inputValue;
            textProcessor = TextProcessorFactory.Create<T>(validChars);

            inputFieldGo = DefaultControls.CreateInputField(new DefaultControls.Resources());
            inputFieldGo.name = "InputFieldSync";
            inputField = inputFieldGo.GetComponent<UnityEngine.UI.InputField>();
            inputField.characterLimit = characterLimit;

            // Ensure input field starts in deactivated state to prevent initial focus issues
            inputField.DeactivateInputField();

            // Remove automatic onEndEdit submission to prevent conflicts with manual text manipulation
            // The main menu will handle submission manually through InputFieldStatus.Submit
            // inputField.onEndEdit.AddListener((text) => {
            //     Submit();
            // });

            inputField.text = textProcessor.ConvertValue(InputValue.Get());

            LayoutElement inputFieldPanelLayout = inputFieldGo.AddComponent<LayoutElement>();
            inputFieldPanelLayout.preferredWidth = size.x;
            inputFieldPanelLayout.preferredHeight = size.y;

            Transform textTransform = inputFieldGo.transform.Find("Text");
            Text textComponent = textTransform.GetComponent<Text>();
            new TextMod(textComponent).SetFontStyle(FontStyle.Bold).SetFontSize(Constants.DEFAULT_FONT_SIZE).SetAlignment(TextAnchor.MiddleLeft);
            
            // Calculate maxVisibleCharacters based on input field width and font size
            int maxVisibleCharacters = CalculateMaxVisibleCharacters(size.x, Constants.DEFAULT_FONT_SIZE);
            inputFieldStatus = new InputFieldStatus<T>(inputFieldGo, SetSelected, Submit, Cancel, validChars, maxVisibleCharacters, textProcessor);
            // Initialize the full text with the current value
            inputFieldStatus.SetFullText(textProcessor.ConvertValue(InputValue.Get()));
            RegisterInputFieldSync?.Invoke(inputFieldStatus);
        }

        public GameObject GetGameObject()
        {
            return inputFieldGo;
        }

        public void Update()
        {
            // Only update the input field text if it's not currently selected
            // This prevents overwriting user input while they're editing
            if (inputFieldStatus == null || !inputFieldStatus.IsSelected)
            {
                string fullText = textProcessor.ConvertValue(InputValue.Get());
                // Update the full text in InputFieldStatus, which will handle Unity InputField synchronization
                inputFieldStatus.SetFullText(fullText);
                
                // Reset horizontal offset when not selected to show the beginning of the text
                if (inputFieldStatus != null)
                {
                    inputFieldStatus.ResetHorizontalOffset();
                    inputFieldStatus.SetCursorPositionDirectly(0);
                }
            }
        }

        public void Submit()
        {
            string text = inputFieldStatus.GetFullText();
            
            // If the text is empty, don't try to convert it - keep the current value
            if (string.IsNullOrEmpty(text))
            {
                // Get the current value and update the display to show it
                T currentValue = InputValue.Get();
                string currentText = textProcessor.ConvertValue(currentValue);
                inputFieldStatus.SetFullText(currentText);
                
                // Reset horizontal offset and cursor position to show the beginning characters
                if (inputFieldStatus != null)
                {
                    inputFieldStatus.ResetHorizontalOffset();
                    inputFieldStatus.SetCursorPositionDirectly(0);
                }
                return;
            }
            
            // Use text processor to process text before conversion
            string processedText = textProcessor.ProcessTextBeforeConversion(text);
            
            // Try to convert the text to the target type
            T convertedValue;
            try
            {
                convertedValue = textProcessor.ConvertText(processedText);
            }
            catch (Exception)
            {
                // If conversion fails, keep the current value
                T currentValue = InputValue.Get();
                string currentText = textProcessor.ConvertValue(currentValue);
                inputFieldStatus.SetFullText(currentText);
                
                // Reset horizontal offset and cursor position to show the beginning characters
                if (inputFieldStatus != null)
                {
                    inputFieldStatus.ResetHorizontalOffset();
                    inputFieldStatus.SetCursorPositionDirectly(0);
                }
                return;
            }
            
            InputValue.Set(convertedValue);
            
            // Get the validated/capped value that was actually set
            T validatedValue = InputValue.Get();
            string validatedText = textProcessor.ConvertValue(validatedValue);
            
            // Update InputFieldStatus which will handle Unity InputField synchronization
            inputFieldStatus.SetFullText(validatedText);
            
            // Reset horizontal offset and cursor position to show the beginning characters
            if (inputFieldStatus != null)
            {
                inputFieldStatus.ResetHorizontalOffset();
                inputFieldStatus.SetCursorPositionDirectly(0);
            }
        }

        public void Cancel()
        {
            var value = InputValue.Get();
            string fullText = textProcessor.ConvertValue(value);
            
            // Update InputFieldStatus which will handle Unity InputField synchronization
            inputFieldStatus.SetFullText(fullText);
            
            // Reset horizontal offset and cursor position to show the beginning characters
            if (inputFieldStatus != null)
            {
                inputFieldStatus.ResetHorizontalOffset();
                inputFieldStatus.SetCursorPositionDirectly(0);
            }
            
            // Don't call SetSelected here - the main menu handles selection state
        }

        public void SetSelected(bool isSelected)
        {
            if (isSelected)
            {
                inputField.ActivateInputField();
            }
            else
                inputField.DeactivateInputField();

            // Set color after focus change
            Image imageComponent = inputFieldGo.GetComponent<Image>();
            imageComponent.color = isSelected ? selectedColor : Color.white;
        }

        public T Get()
        {
            return InputValue.Get();
        }

        public void Set(T value)
        {
            InputValue.Set(value);
        }

        /// <summary>
        /// Calculates the maximum number of characters that can be displayed in the input field based on width and font size.
        /// </summary>
        /// <param name="width">The width of the input field in pixels.</param>
        /// <param name="fontSize">The font size in pixels.</param>
        /// <returns>The maximum number of characters that can be displayed.</returns>
        private int CalculateMaxVisibleCharacters(float width, int fontSize)
        {
            float estimatedCharWidth = CalculateCursorCharacterWidth(fontSize);
            float usableWidth = width;
            int maxChars = Mathf.FloorToInt(usableWidth / estimatedCharWidth);
            return Mathf.Max(1, maxChars);
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