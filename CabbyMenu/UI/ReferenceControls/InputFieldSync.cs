using System;
using UnityEngine;
using UnityEngine.UI;
using CabbyMenu.SyncedReferences;
using CabbyMenu.Types;
using CabbyMenu.UI.Modders;

namespace CabbyMenu.UI.ReferenceControls
{
    public class InputFieldSync<T>
    {
        private static readonly Color selectedColor = Constants.SELECTED_COLOR;

        private readonly GameObject inputFieldGo;
        private readonly InputField inputField;
        private readonly ISyncedReference<T> InputValue;
        private readonly KeyCodeMap.ValidChars validChars;
        private readonly int characterLimit;
        private InputFieldStatus inputFieldStatus;

        /// <summary>
        /// Action for registering input field sync. Can be set by the consuming project.
        /// </summary>
        public static Action<InputFieldStatus> RegisterInputFieldSync { get; set; } = null;

        public InputFieldSync(ISyncedReference<T> inputValue, KeyCodeMap.ValidChars validChars, Vector2 size, int characterLimit = Constants.DEFAULT_CHARACTER_LIMIT)
        {
            InputValue = inputValue;
            this.validChars = validChars;
            this.characterLimit = characterLimit;

            inputFieldGo = DefaultControls.CreateInputField(new DefaultControls.Resources());
            inputFieldGo.name = "InputFieldSync";
            inputField = inputFieldGo.GetComponent<InputField>();
            inputField.characterLimit = characterLimit;

            // Ensure input field starts in deactivated state to prevent initial focus issues
            inputField.DeactivateInputField();

            // Remove automatic onEndEdit submission to prevent conflicts with manual text manipulation
            // The main menu will handle submission manually through InputFieldStatus.Submit
            // inputField.onEndEdit.AddListener((text) => {
            //     Submit();
            // });

            inputField.text = Convert.ToString(InputValue.Get());

            LayoutElement inputFieldPanelLayout = inputFieldGo.AddComponent<LayoutElement>();
            inputFieldPanelLayout.preferredWidth = size.x;
            inputFieldPanelLayout.preferredHeight = size.y;

            Transform textTransform = inputFieldGo.transform.Find("Text");
            Text textComponent = textTransform.GetComponent<Text>();
            new TextMod(textComponent).SetFontStyle(FontStyle.Bold).SetFontSize(Constants.DEFAULT_FONT_SIZE).SetAlignment(TextAnchor.MiddleLeft);
            
            // Calculate maxVisibleCharacters based on input field width and font size
            int maxVisibleCharacters = CalculateMaxVisibleCharacters(size.x, Constants.DEFAULT_FONT_SIZE);
            inputFieldStatus = new InputFieldStatus(inputFieldGo, SetSelected, Submit, Cancel, validChars, maxVisibleCharacters);
            // Initialize the full text with the current value
            inputFieldStatus.SetFullText(Convert.ToString(InputValue.Get()));
            RegisterInputFieldSync(inputFieldStatus);
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
                string fullText = Convert.ToString(InputValue.Get());
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
                string currentText = Convert.ToString(currentValue);
                inputFieldStatus.SetFullText(currentText);
                
                // Reset horizontal offset and cursor position to show the beginning characters
                if (inputFieldStatus != null)
                {
                    inputFieldStatus.ResetHorizontalOffset();
                    inputFieldStatus.SetCursorPositionDirectly(0);
                }
                return;
            }
            
            // Try to convert the text to the target type
            T convertedValue;
            try
            {
                convertedValue = (T)Convert.ChangeType(text, typeof(T));
            }
            catch (Exception)
            {
                // If conversion fails, keep the current value
                T currentValue = InputValue.Get();
                string currentText = Convert.ToString(currentValue);
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
            string validatedText = Convert.ToString(validatedValue);
            
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
            string fullText = value?.ToString() ?? "0";
            
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
            // Estimate character width based on font size (monospace font assumption)
            // For most fonts, character width is roughly 0.6-0.7 times the font size
            float estimatedCharWidth = fontSize * 0.65f;
            
            // Account for some padding/margins (subtract about 10-20 pixels for borders/padding)
            float usableWidth = width - 20f;
            
            // Calculate how many characters can fit
            int maxChars = Mathf.FloorToInt(usableWidth / estimatedCharWidth);
            
            // Ensure we have at least 1 character visible
            return Mathf.Max(1, maxChars);
        }
    }
}