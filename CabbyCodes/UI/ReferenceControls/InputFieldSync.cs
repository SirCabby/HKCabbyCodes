using CabbyCodes.SyncedReferences;
using UnityEngine.UI;
using UnityEngine;
using CabbyCodes.UI.Modders;
using CabbyCodes.Types;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;
using BepInEx.Configuration;

namespace CabbyCodes.UI.ReferenceControls
{
    public class InputFieldSync<T>
    {
        private static readonly Color selectedColor = new(1, 1, 0.56f, 1);
        private static readonly Dictionary<string, ConfigDefinition> _configCache = new();

        private readonly GameObject inputFieldGo;
        private readonly InputField inputField;
        private readonly ISyncedReference<T> InputValue;

        public InputFieldSync(ISyncedReference<T> inputValue, KeyCodeMap.ValidChars validChars, Vector2 size, int characterLimit = 0)
        {
            InputValue = inputValue;

            try
            {
                inputFieldGo = DefaultControls.CreateInputField(new DefaultControls.Resources());
                if (inputFieldGo == null)
                {
                    CabbyCodesPlugin.BLogger?.LogError("Failed to create input field GameObject");
                    return;
                }

                inputFieldGo.name = "InputFieldSync";

                inputField = inputFieldGo.GetComponent<InputField>();
                if (inputField == null)
                {
                    CabbyCodesPlugin.BLogger?.LogError("Failed to get InputField component");
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
                    CabbyCodesPlugin.BLogger?.LogWarning($"Failed to get initial value for input field: {ex.Message}");
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
                            new TextMod(textComponent).SetFontStyle(FontStyle.Bold).SetFontSize(36).SetAlignment(TextAnchor.MiddleLeft);
                        }
                    }
                }
                catch (Exception ex)
                {
                    CabbyCodesPlugin.BLogger?.LogWarning($"Failed to configure text component: {ex.Message}");
                }

                // Safely register the input field
                try
                {
                    if (CabbyCodesPlugin.cabbyMenu != null)
                    {
                        CabbyCodesPlugin.cabbyMenu.RegisterInputFieldSync(new InputFieldStatus(inputFieldGo, SetSelected, Submit, Cancel, validChars));
                    }
                }
                catch (Exception ex)
                {
                    CabbyCodesPlugin.BLogger?.LogWarning($"Failed to register input field sync: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                CabbyCodesPlugin.BLogger?.LogError($"Error in InputFieldSync constructor: {ex.Message}");
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
                CabbyCodesPlugin.BLogger?.LogWarning($"Error updating input field: {ex.Message}");
            }
        }

        public void Submit()
        {
            try
            {
                if (inputField == null)
                {
                    CabbyCodesPlugin.BLogger?.LogWarning("Input field is null, cannot submit");
                    return;
                }

                string text = inputField.text;
                if (string.IsNullOrEmpty(text))
                {
                    CabbyCodesPlugin.BLogger.LogWarning("Input field is empty");
                    return;
                }

                if (InputValue == null)
                {
                    CabbyCodesPlugin.BLogger?.LogWarning("Input value reference is null, cannot submit");
                    return;
                }

                T result = (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(text);
                if (result != null)
                {
                    InputValue.Set(result);
                }
                else
                {
                    CabbyCodesPlugin.BLogger.LogWarning($"Failed to convert text '{text}' to type {typeof(T).Name}");
                }
            }
            catch (Exception ex)
            {
                CabbyCodesPlugin.BLogger.LogError($"Error in InputFieldSync.Submit: {ex.Message}");
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
                        CabbyCodesPlugin.BLogger?.LogWarning($"Failed to get value for cancel: {ex.Message}");
                        inputField.text = "0";
                    }
                }
                SetSelected(false);
            }
            catch (Exception ex)
            {
                CabbyCodesPlugin.BLogger?.LogWarning($"Error canceling input field: {ex.Message}");
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
                CabbyCodesPlugin.BLogger?.LogWarning($"Error setting input field selection: {ex.Message}");
            }
        }
    }
}
