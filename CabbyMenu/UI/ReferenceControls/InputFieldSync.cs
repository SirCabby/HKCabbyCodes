using CabbyMenu.SyncedReferences;
using UnityEngine.UI;
using UnityEngine;
using CabbyMenu.UI.Modders;
using CabbyMenu.Types;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

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

        /// <summary>
        /// Action for logging error messages. Can be set by the consuming project.
        /// </summary>
        public static Action<string> ErrorLogger { get; set; } = Console.WriteLine;

        /// <summary>
        /// Action for logging warning messages. Can be set by the consuming project.
        /// </summary>
        public static Action<string> WarningLogger { get; set; } = Console.WriteLine;

        /// <summary>
        /// Action for registering input field sync. Can be set by the consuming project.
        /// </summary>
        public static Action<InputFieldStatus> RegisterInputFieldSync { get; set; } = null;

        public InputFieldSync(ISyncedReference<T> inputValue, KeyCodeMap.ValidChars validChars, Vector2 size, int characterLimit = Constants.DEFAULT_CHARACTER_LIMIT)
        {
            InputValue = inputValue;
            this.validChars = validChars;
            this.characterLimit = characterLimit;

            try
            {
                inputFieldGo = DefaultControls.CreateInputField(new DefaultControls.Resources());
                if (inputFieldGo == null)
                {
                    ErrorLogger("Failed to create input field GameObject");
                    return;
                }

                inputFieldGo.name = "InputFieldSync";

                inputField = inputFieldGo.GetComponent<InputField>();
                if (inputField == null)
                {
                    ErrorLogger("Failed to get InputField component");
                    return;
                }

                inputField.characterLimit = characterLimit;

                // Safely get the initial value
                try
                {
                    if (InputValue != null)
                    {
                        inputField.text = Convert.ToString(InputValue.Get());
                    }
                    else
                    {
                        inputField.text = "0";
                    }
                }
                catch (Exception ex)
                {
                    WarningLogger($"Failed to get initial value for input field: {ex.Message}");
                    inputField.text = "0";
                }

                LayoutElement inputFieldPanelLayout = inputFieldGo.AddComponent<LayoutElement>();
                inputFieldPanelLayout.preferredWidth = size.x;
                inputFieldPanelLayout.preferredHeight = size.y;

                // Safely find and modify the text component
                try
                {
                    Transform textTransform = inputFieldGo.transform.Find("Text");
                    if (textTransform != null)
                    {
                        Text textComponent = textTransform.GetComponent<Text>();
                        if (textComponent != null)
                        {
                            new TextMod(textComponent).SetFontStyle(FontStyle.Bold).SetFontSize(Constants.DEFAULT_FONT_SIZE).SetAlignment(TextAnchor.MiddleLeft);
                        }
                    }
                }
                catch (Exception ex)
                {
                    WarningLogger($"Failed to configure text component: {ex.Message}");
                }

                // Safely register the input field
                try
                {
                    if (RegisterInputFieldSync != null)
                    {
                        RegisterInputFieldSync(new InputFieldStatus(inputFieldGo, SetSelected, Submit, Cancel, validChars));
                    }
                }
                catch (Exception ex)
                {
                    WarningLogger($"Failed to register input field sync: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                ErrorLogger($"Error in InputFieldSync constructor: {ex.Message}");
            }
        }

        public GameObject GetGameObject()
        {
            return inputFieldGo;
        }

        public void Update()
        {
            try
            {
                if (inputField != null && InputValue != null)
                {
                    inputField.text = Convert.ToString(InputValue.Get());
                }
            }
            catch (Exception ex)
            {
                WarningLogger($"Error updating input field: {ex.Message}");
            }
        }

        public void Submit()
        {
            try
            {
                if (inputField == null)
                {
                    WarningLogger("Input field is null, cannot submit");
                    return;
                }

                string text = inputField.text;
                if (string.IsNullOrEmpty(text))
                {
                    WarningLogger("Input field is empty");
                    return;
                }

                if (InputValue == null)
                {
                    WarningLogger("Input value reference is null, cannot submit");
                    return;
                }

                try
                {
                    T convertedValue = (T)Convert.ChangeType(text, typeof(T));
                    InputValue.Set(convertedValue);
                }
                catch (System.Exception)
                {
                    WarningLogger("Failed to convert text '" + text + "' to type " + typeof(T).Name);
                }
            }
            catch (System.Exception ex)
            {
                ErrorLogger("Error in InputFieldSync.Submit: " + ex.Message);
            }
        }

        public void Cancel()
        {
            try
            {
                if (inputField != null && InputValue != null)
                {
                    try
                    {
                        var value = InputValue.Get();
                        inputField.text = value?.ToString() ?? "0";
                    }
                    catch (Exception ex)
                    {
                        WarningLogger($"Failed to get value for cancel: {ex.Message}");
                        inputField.text = "0";
                    }
                }
                SetSelected(false);
            }
            catch (Exception ex)
            {
                WarningLogger($"Error canceling input field: {ex.Message}");
            }
        }

        public void SetSelected(bool isSelected)
        {
            try
            {
                if (inputFieldGo == null || inputField == null)
                {
                    return;
                }

                Image imageComponent = inputFieldGo.GetComponent<Image>();
                if (imageComponent != null)
                {
                    imageComponent.color = isSelected ? selectedColor : Color.white;
                }

                if (!isSelected)
                {
                    inputField.DeactivateInputField();
                }
            }
            catch (Exception ex)
            {
                WarningLogger($"Error setting input field selection: {ex.Message}");
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
    }
}