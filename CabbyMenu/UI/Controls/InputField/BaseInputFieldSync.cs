using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CabbyMenu.SyncedReferences;
using CabbyMenu.TextProcessors;
using CabbyMenu.UI.Modders;
using CabbyMenu.Utilities;

namespace CabbyMenu.UI.Controls.InputField
{
    /// <summary>
    /// Base class for input field synchronization with shared UI logic.
    /// </summary>
    /// <typeparam name="T">The type of the input field value.</typeparam>
    public abstract class BaseInputFieldSync<T>
    {
        protected static readonly Color selectedColor = Constants.SELECTED_COLOR;

        protected readonly GameObject inputFieldGo;
        protected readonly UnityEngine.UI.InputField inputField;
        protected readonly ISyncedReference<T> InputValue;
        protected readonly InputFieldStatusBase inputFieldStatus;
        protected readonly ITextProcessor<T> textProcessor;

        /// <summary>
        /// Action for registering input field sync. Can be set by the consuming project.
        /// </summary>
        public static Action<InputFieldStatusBase> RegisterInputFieldSync { get; set; } = null;

        protected BaseInputFieldSync(ISyncedReference<T> inputValue, KeyCodeMap.ValidChars validChars, Vector2 size, int characterLimit)
        {
            InputValue = inputValue;
            textProcessor = TextProcessor.Create<T>(validChars);

            inputFieldGo = DefaultControls.CreateInputField(new DefaultControls.Resources());
            inputFieldGo.name = "InputFieldSync";
            inputField = inputFieldGo.GetComponent<UnityEngine.UI.InputField>();
            inputField.characterLimit = characterLimit;

            // Ensure input field starts in deactivated state to prevent initial focus issues
            inputField.DeactivateInputField();

            inputField.text = textProcessor.ConvertValue(InputValue.Get());

            LayoutElement inputFieldPanelLayout = inputFieldGo.AddComponent<LayoutElement>();
            inputFieldPanelLayout.preferredWidth = size.x;
            inputFieldPanelLayout.preferredHeight = size.y;

            Transform textTransform = inputFieldGo.transform.Find("Text");
            Text textComponent = textTransform.GetComponent<Text>();
            new TextMod(textComponent).SetFontStyle(FontStyle.Bold).SetFontSize(Constants.DEFAULT_FONT_SIZE).SetAlignment(TextAnchor.MiddleLeft);
            
            // Add border to the input field
            CreateInputFieldBorder();
            CreateInputFieldBackground();

            // Ensure proper z-order: border (back) -> background (middle) -> input field content (front)
            EnsureProperInputFieldZOrder();

            // Calculate maxVisibleCharacters based on input field width and font size
            int maxVisibleCharacters = CalculateMaxVisibleCharacters(size.x, Constants.DEFAULT_FONT_SIZE);

            // Only create status for non-numeric types; numeric types will handle it in their constructor
            if (typeof(T) == typeof(string))
            {
                inputFieldStatus = CreateInputFieldStatus(validChars, maxVisibleCharacters);
                // Initialize the full text with the current value
                inputFieldStatus.SetFullText(textProcessor.ConvertValue(InputValue.Get()));
                RegisterInputFieldSync?.Invoke(inputFieldStatus);
            }
            else
            {
                // For numeric types, let the subclass handle status creation
                inputFieldStatus = null;
            }
        }

        /// <summary>
        /// Creates the appropriate InputFieldStatus instance for this type.
        /// </summary>
        /// <param name="validChars">The valid character types.</param>
        /// <param name="maxVisibleCharacters">The maximum visible characters.</param>
        /// <returns>The InputFieldStatus instance.</returns>
        protected virtual InputFieldStatusBase CreateInputFieldStatus(KeyCodeMap.ValidChars validChars, int maxVisibleCharacters)
        {
            if (typeof(T) == typeof(string))
            {
                return new TextInputFieldStatus(inputFieldGo, SetSelected, Submit, Cancel, maxVisibleCharacters, (StringTextProcessor)textProcessor);
            }
            
            // Fallback to text for any other non-numeric types
            return new TextInputFieldStatus(inputFieldGo, SetSelected, Submit, Cancel, maxVisibleCharacters, (StringTextProcessor)textProcessor);
        }

        /// <summary>
        /// Sets the input field status. Used by subclasses to set their status after creation.
        /// </summary>
        /// <param name="status">The status to set.</param>
        protected void SetInputFieldStatus(InputFieldStatusBase status)
        {
            // Use reflection to set the readonly field
            var field = typeof(BaseInputFieldSync<T>).GetField("inputFieldStatus", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field.SetValue(this, status);
            
            // Initialize the full text with the current value
            status.SetFullText(textProcessor.ConvertValue(InputValue.Get()));
            RegisterInputFieldSync?.Invoke(status);
        }

        /// <summary>
        /// Handles the submission logic specific to this input field type.
        /// </summary>
        /// <param name="text">The text to submit.</param>
        protected abstract void HandleSubmit(string text);

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

        /// <summary>
        /// Forces an update of the input field UI regardless of selection state.
        /// Use this when you need to ensure the UI reflects the current value.
        /// </summary>
        public void ForceUpdate()
        {
            if (inputFieldStatus != null)
            {
                string fullText = textProcessor.ConvertValue(InputValue.Get());
                inputFieldStatus.SetFullText(fullText);
                inputFieldStatus.ResetHorizontalOffset();
                inputFieldStatus.SetCursorPositionDirectly(0);
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
            catch (Exception ex)
            {
                Debug.LogWarning($"Exception in BaseInputFieldSync.Submit text conversion: {ex.Message}");
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
            
            // Handle submission with type-specific logic
            HandleSubmit(processedText);
            
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
            
            // Don't call SetSelected here
        }

        public void SetSelected(bool isSelected)
        {
            // Set color after focus change
            Image imageComponent = inputFieldGo.GetComponent<Image>();
            imageComponent.color = isSelected ? selectedColor : Color.white;

            // Update background color to match
            Transform backgroundTransform = inputFieldGo.transform.Find("InputFieldBackground");
            if (backgroundTransform != null)
            {
                Image backgroundImage = backgroundTransform.GetComponent<Image>();
                if (backgroundImage != null)
                {
                    backgroundImage.color = isSelected ? selectedColor : Color.white;
                }
            }

            // Ensure cursor is brought to front when selected
            if (isSelected && inputFieldStatus != null)
            {
                inputFieldStatus.EnsureCursorIsOnTop();
            }
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
        protected int CalculateMaxVisibleCharacters(float width, int fontSize)
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
        protected static float CalculateCursorCharacterWidth(int fontSize)
        {
            return fontSize * 0.65f;
        }

        /// <summary>
        /// Creates a border around the input field.
        /// </summary>
        private void CreateInputFieldBorder()
        {
            // Create border GameObject as a child of the input field
            GameObject borderObj = new GameObject("InputFieldBorder");
            borderObj.transform.SetParent(inputFieldGo.transform, false);

            // Add RectTransform and Image components for the border
            RectTransform borderRect = borderObj.AddComponent<RectTransform>();
            Image borderImage = borderObj.AddComponent<Image>();

            // Configure border rect transform to be slightly larger than the input field
            borderRect.anchorMin = Vector2.zero;
            borderRect.anchorMax = Vector2.one;
            borderRect.sizeDelta = new Vector2(4f, 4f); // 2px border on each side (2px * 2 = 4px total)
            borderRect.anchoredPosition = Vector2.zero;

            // Configure border image
            borderImage.color = Color.black; // Black border
            borderImage.sprite = CreateBorderSprite();

            // Ensure border is behind the input field image
            // Set border as first sibling so it renders behind the input field image
            borderObj.transform.SetAsFirstSibling();
        }



        /// <summary>
        /// Creates a background for the input field that's smaller than the border.
        /// </summary>
        private void CreateInputFieldBackground()
        {
            // Create background GameObject as a child of the input field
            GameObject backgroundObj = new GameObject("InputFieldBackground");
            backgroundObj.transform.SetParent(inputFieldGo.transform, false);

            // Add RectTransform and Image components for the background
            RectTransform backgroundRect = backgroundObj.AddComponent<RectTransform>();
            Image backgroundImage = backgroundObj.AddComponent<Image>();

            // Configure background rect transform to be slightly smaller than the input field
            backgroundRect.anchorMin = Vector2.zero;
            backgroundRect.anchorMax = Vector2.one;
            backgroundRect.sizeDelta = new Vector2(-4f, -4f); // 2px smaller on each side
            backgroundRect.anchoredPosition = Vector2.zero;

            // Configure background image
            backgroundImage.color = Color.white; // White background (default input field color)
            backgroundImage.sprite = CreateBorderSprite(); // Use same sprite as border
        }

        /// <summary>
        /// Ensures proper z-order for input field components.
        /// </summary>
        private void EnsureProperInputFieldZOrder()
        {
            // Find the border GameObject
            Transform borderTransform = inputFieldGo.transform.Find("InputFieldBorder");
            
            // Find the background GameObject
            Transform backgroundTransform = inputFieldGo.transform.Find("InputFieldBackground");

            // Ensure proper z-order: border (back) -> background (middle) -> input field content (front)
            // Set border to be first (renders in back)
            borderTransform?.SetAsFirstSibling();

            // Set background in the middle (after border but before content)
            // Move background to be after border but before all other content
            backgroundTransform?.SetSiblingIndex(1);

            // Preserve the original Unity InputField child order
            // Unity InputField has specific child elements that need to maintain their relative order
            // The cursor is managed internally by Unity and needs to be on top

            // Move all Unity-created children to the top while preserving their relative order
            List<Transform> unityChildren = new List<Transform>();
            
            // Collect all Unity-created children (excluding our border and background)
            for (int i = 0; i < inputFieldGo.transform.childCount; i++)
            {
                Transform child = inputFieldGo.transform.GetChild(i);
                if (child.name != "InputFieldBorder" && child.name != "InputFieldBackground")
                {
                    unityChildren.Add(child);
                }
            }
            
            // Move all Unity children to the top in their original order
            foreach (Transform child in unityChildren)
            {
                child.SetAsLastSibling();
            }
        }



        /// <summary>
        /// Creates a simple border sprite using a white square texture.
        /// </summary>
        /// <returns>The created border sprite.</returns>
        private Sprite CreateBorderSprite()
        {
            // Create a simple border sprite using a white square texture
            Texture2D borderTexture = new Texture2D(4, 4, TextureFormat.RGBA32, false);
            
            // Fill with white pixels
            Color[] pixels = new Color[16];
            for (int i = 0; i < 16; i++)
            {
                pixels[i] = Color.white; // White pixels for the border
            }
            
            borderTexture.SetPixels(pixels);
            borderTexture.Apply();
            
            // Create sprite from texture
            Sprite borderSprite = Sprite.Create(borderTexture, new Rect(0, 0, 4, 4), new Vector2(0.5f, 0.5f));
            
            return borderSprite;
        }
    }
} 