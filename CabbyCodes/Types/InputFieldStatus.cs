using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace CabbyCodes.Types
{
    public class InputFieldStatus
    {
        private static readonly FieldInfo blinkStartTimeFieldInfo = typeof(InputField).GetField("m_BlinkStartTime", BindingFlags.NonPublic | BindingFlags.Instance);

        public GameObject InputFieldGo { get; private set; }
        public float LastUpdated { get; set; }
        public KeyCodeMap.ValidChars ValidChars { get; private set; }
        public readonly Action<bool> OnSelected;
        public readonly Action Submit;
        public readonly Action Cancel;

        public InputFieldStatus(GameObject inputFieldGo, Action<bool> onSelected, Action submit, Action cancel, KeyCodeMap.ValidChars validChars)
        {
            InputFieldGo = inputFieldGo;
            ValidChars = validChars;
            OnSelected = onSelected;
            Submit = submit;
            Cancel = cancel;
        }

        public bool WasSelected()
        {
            if (GetSelectedTime() > LastUpdated)
            {
                LastUpdated = GetSelectedTime();
                return true;
            }

            return false;
        }

        public float GetSelectedTime()
        {
            return (float)blinkStartTimeFieldInfo.GetValue(GetInputField());
        }

        public InputField GetInputField()
        {
            return InputFieldGo.GetComponent<InputField>();
        }

        public int GetIndex()
        {
            return InputFieldGo.transform.GetParent().GetSiblingIndex();
        }
    }
}
