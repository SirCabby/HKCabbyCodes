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
        /// The current cursor position within the input field text.
        /// </summary>
        public int CursorPosition { get; set; } = 0;

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
                    // Sync our cursor position with Unity's InputField cursor position
                    SyncCursorPositionFromUnity();
                }
                
                // Call the OnSelected callback to update the InputFieldSync
                OnSelected?.Invoke(selected);
            }
        }

        /// <summary>
        /// Synchronizes our logical cursor position with Unity's InputField cursor position.
        /// </summary>
        public void SyncCursorPositionFromUnity()
        {
            InputField inputField = GetInputField();
            if (inputField != null)
            {
                // Get Unity's current cursor position
                int unityCursorPosition = inputField.caretPosition;
                
                // Update our logical cursor position to match
                CursorPosition = Mathf.Clamp(unityCursorPosition, 0, inputField.text.Length);
                
                // Ensure Unity's cursor position is properly set
                inputField.selectionAnchorPosition = CursorPosition;
                inputField.selectionFocusPosition = CursorPosition;
                inputField.caretPosition = CursorPosition;
                
                // Force cursor to reset blink cycle
                ForceCursorBlinkReset();
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

        /// <summary>
        /// Forces the cursor to reset its blink cycle immediately.
        /// </summary>
        private void ForceCursorBlinkReset()
        {
            InputField inputField = GetInputField();
            if (inputField != null)
            {
                // Temporarily hide the cursor by setting selection to -1
                inputField.selectionAnchorPosition = -1;
                inputField.selectionFocusPosition = -1;
                
                // Force a frame update
                inputField.ForceLabelUpdate();
                
                // Immediately restore the cursor position
                inputField.selectionAnchorPosition = CursorPosition;
                inputField.selectionFocusPosition = CursorPosition;
                inputField.caretPosition = CursorPosition;
            }
        }

        /// <summary>
        /// Moves the cursor position by the specified offset, ensuring it stays within bounds.
        /// </summary>
        /// <param name="offset">The offset to move the cursor by (positive for right, negative for left).</param>
        public void MoveCursor(int offset)
        {
            InputField inputField = GetInputField();
            if (inputField != null)
            {
                int newPosition = CursorPosition + offset;
                CursorPosition = Mathf.Clamp(newPosition, 0, inputField.text.Length);
                
                // Update Unity's InputField cursor position immediately
                inputField.selectionAnchorPosition = CursorPosition;
                inputField.selectionFocusPosition = CursorPosition;
                inputField.caretPosition = CursorPosition;
                
                // Force cursor to reset blink cycle
                ForceCursorBlinkReset();
            }
        }

        /// <summary>
        /// Sets the cursor position to a specific index, ensuring it stays within bounds.
        /// </summary>
        /// <param name="position">The new cursor position.</param>
        public void SetCursorPosition(int position)
        {
            InputField inputField = GetInputField();
            if (inputField != null)
            {
                CursorPosition = Mathf.Clamp(position, 0, inputField.text.Length);
                
                // Update Unity's InputField cursor position immediately
                inputField.selectionAnchorPosition = CursorPosition;
                inputField.selectionFocusPosition = CursorPosition;
                inputField.caretPosition = CursorPosition;
                
                // Force cursor to reset blink cycle
                ForceCursorBlinkReset();
            }
        }

        /// <summary>
        /// Inserts a character at the current cursor position.
        /// </summary>
        /// <param name="character">The character to insert.</param>
        /// <param name="characterLimit">The maximum number of characters allowed.</param>
        public void InsertCharacter(char character, int characterLimit)
        {
            InputField inputField = GetInputField();
            if (inputField != null)
            {
                string text = inputField.text;
                
                // If at character limit, replace the character at cursor position
                if (text.Length >= characterLimit && CursorPosition < text.Length)
                {
                    text = text.Substring(0, CursorPosition) + character + text.Substring(CursorPosition + 1);
                }
                // Otherwise insert at cursor position
                else if (text.Length < characterLimit)
                {
                    text = text.Substring(0, CursorPosition) + character + text.Substring(CursorPosition);
                }
                
                inputField.text = text;
                CursorPosition++;
                
                // Update Unity's InputField cursor position immediately
                inputField.selectionAnchorPosition = CursorPosition;
                inputField.selectionFocusPosition = CursorPosition;
                inputField.caretPosition = CursorPosition;
                
                // Force cursor to reset blink cycle
                ForceCursorBlinkReset();
            }
        }

        /// <summary>
        /// Deletes the character at the current cursor position (backspace).
        /// </summary>
        public void DeleteCharacter()
        {
            InputField inputField = GetInputField();
            if (inputField != null && CursorPosition > 0)
            {
                string text = inputField.text;
                text = text.Substring(0, CursorPosition - 1) + text.Substring(CursorPosition);
                inputField.text = text;
                CursorPosition--;
                
                // Update Unity's InputField cursor position immediately
                inputField.selectionAnchorPosition = CursorPosition;
                inputField.selectionFocusPosition = CursorPosition;
                inputField.caretPosition = CursorPosition;
                
                // Force cursor to reset blink cycle
                ForceCursorBlinkReset();
            }
        }

        /// <summary>
        /// Deletes the character after the current cursor position (delete key).
        /// </summary>
        public void DeleteForwardCharacter()
        {
            InputField inputField = GetInputField();
            if (inputField != null && CursorPosition < inputField.text.Length)
            {
                string text = inputField.text;
                text = text.Substring(0, CursorPosition) + text.Substring(CursorPosition + 1);
                inputField.text = text;
                
                // Cursor position stays the same when deleting forward
                
                // Update Unity's InputField cursor position immediately
                inputField.selectionAnchorPosition = CursorPosition;
                inputField.selectionFocusPosition = CursorPosition;
                inputField.caretPosition = CursorPosition;
                
                // Force cursor to reset blink cycle
                ForceCursorBlinkReset();
            }
        }
    }
}