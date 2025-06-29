using System;
using UnityEngine;
using UnityEngine.UI;

namespace CabbyMenu.Types
{
    /// <summary>
    /// Manages the state and behavior of input fields in the mod UI.
    /// </summary>
    public class InputFieldStatus
    {
        /// <summary>
        /// The GameObject containing the input field.
        /// </summary>
        public GameObject InputFieldGo { get; private set; }

        /// <summary>
        /// The timestamp when this input field was last updated.
        /// </summary>
        public float LastUpdated { get; set; }

        /// <summary>
        /// The types of characters that are valid for this input field.
        /// </summary>
        public KeyCodeMap.ValidChars ValidChars { get; private set; }

        /// <summary>
        /// Callback invoked when the input field selection state changes.
        /// </summary>
        public readonly Action<bool> OnSelected;

        /// <summary>
        /// Callback invoked when the input field value is submitted.
        /// </summary>
        public readonly Action Submit;

        /// <summary>
        /// Callback invoked when the input field input is cancelled.
        /// </summary>
        public readonly Action Cancel;

        /// <summary>
        /// Whether this input field is currently selected.
        /// </summary>
        private bool isSelected = false;

        /// <summary>
        /// The time when this input field was last selected.
        /// </summary>
        private float selectionTime = 0f;

        /// <summary>
        /// Initializes a new instance of the InputFieldStatus class.
        /// </summary>
        /// <param name="inputFieldGo">The GameObject containing the input field.</param>
        /// <param name="onSelected">Callback for selection state changes.</param>
        /// <param name="submit">Callback for value submission.</param>
        /// <param name="cancel">Callback for input cancellation.</param>
        /// <param name="validChars">The types of characters valid for this input field.</param>
        public InputFieldStatus(GameObject inputFieldGo, Action<bool> onSelected, Action submit, Action cancel, KeyCodeMap.ValidChars validChars)
        {
            InputFieldGo = inputFieldGo;
            ValidChars = validChars;
            OnSelected = onSelected;
            Submit = submit;
            Cancel = cancel;
        }

        /// <summary>
        /// Sets the selection state of the input field.
        /// </summary>
        /// <param name="selected">Whether the input field should be selected.</param>
        public void SetSelected(bool selected)
        {
            if (InputFieldGo == null)
            {
                return;
            }

            if (selected != isSelected)
            {
                isSelected = selected;
                if (selected)
                {
                    selectionTime = Time.time;
                }
            }
        }

        /// <summary>
        /// Checks if the input field was selected since the last update.
        /// </summary>
        /// <returns>True if the input field was selected, false otherwise.</returns>
        public bool WasSelected()
        {
            if (isSelected && selectionTime > LastUpdated)
            {
                LastUpdated = selectionTime;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the timestamp when the input field was last selected.
        /// </summary>
        /// <returns>The selection timestamp.</returns>
        public float GetSelectedTime()
        {
            return selectionTime;
        }

        /// <summary>
        /// Gets the InputField component from the GameObject.
        /// </summary>
        /// <returns>The InputField component.</returns>
        public InputField GetInputField()
        {
            return InputFieldGo.GetComponent<InputField>();
        }

        /// <summary>
        /// Gets the sibling index of the input field's parent transform.
        /// </summary>
        /// <returns>The sibling index.</returns>
        public int GetIndex()
        {
            return InputFieldGo.transform.GetParent().GetSiblingIndex();
        }
    }
}