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
            
            inputFieldStatus = new InputFieldStatus(inputFieldGo, SetSelected, Submit, Cancel, validChars);
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
                inputField.text = Convert.ToString(InputValue.Get());
            }
        }

        public void Submit()
        {
            string text = inputField.text;
            T convertedValue = (T)Convert.ChangeType(text, typeof(T));
            InputValue.Set(convertedValue);
            
            // Immediately update the UI text to show the validated/capped value
            // This ensures users see the actual value that was set, not what they typed
            inputField.text = Convert.ToString(InputValue.Get());
        }

        public void Cancel()
        {
            var value = InputValue.Get();
            inputField.text = value?.ToString() ?? "0";
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
    }
}