using CabbyCodes.SyncedReferences;
using UnityEngine.UI;
using UnityEngine;
using CabbyCodes.UI.Modders;
using CabbyCodes.Types;
using System.ComponentModel;
using System;

namespace CabbyCodes.UI.ReferenceControls
{
    public class InputFieldSync<T>
    {
        private static readonly Color selectedColor = new(1, 1, 0.56f, 1);

        private readonly GameObject inputFieldGo;
        private readonly InputField inputField;
        private readonly ISyncedReference<T> InputValue;

        public InputFieldSync(ISyncedReference<T> inputValue, KeyCodeMap.ValidChars validChars, Vector2 size, int characterLimit = 0)
        {
            InputValue = inputValue;

            inputFieldGo = DefaultControls.CreateInputField(new DefaultControls.Resources());
            inputFieldGo.name = "InputFieldSync";

            inputField = inputFieldGo.GetComponent<InputField>();
            inputField.characterLimit = characterLimit;
            inputField.text = Convert.ToString(InputValue.Get());

            LayoutElement inputFieldPanelLayout = inputFieldGo.AddComponent<LayoutElement>();
            inputFieldPanelLayout.preferredWidth = size.x;
            inputFieldPanelLayout.preferredHeight = size.y;

            new TextMod(inputFieldGo.transform.Find("Text").GetComponent<Text>()).SetFontStyle(FontStyle.Bold).SetFontSize(36).SetAlignment(TextAnchor.MiddleLeft);

            CabbyCodesPlugin.cabbyMenu.RegisterInputFieldSync(new InputFieldStatus(inputFieldGo, SetSelected, Submit, Cancel, validChars));
        }

        public GameObject GetGameObject()
        {
            return inputFieldGo;
        }

        public void Update()
        {
            inputField.text = Convert.ToString(InputValue.Get());
        }

        public void Submit()
        {
            string text = inputField.text;

            T result = (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(text);
            if (result != null)
            {
                InputValue.Set(result);
            }
            else
            {
                CabbyCodesPlugin.BLogger.LogInfo("Failed to convert text: " + text);
            }
        }

        public void Cancel()
        {
            inputField.text = InputValue.Get().ToString();
            SetSelected(false);
        }

        public void SetSelected(bool isSelected)
        {
            if (isSelected)
            {
                inputFieldGo.GetComponent<Image>().color = selectedColor;
            }
            else
            {
                inputFieldGo.GetComponent<Image>().color = Color.white;
                inputField.DeactivateInputField();
            }
        }
    }
}
