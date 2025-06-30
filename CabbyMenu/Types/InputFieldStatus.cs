using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using CabbyMenu; // For CoroutineRunner

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
        /// Validates if a decimal point can be inserted at the current cursor position.
        /// </summary>
        /// <param name="currentText">The current text in the input field.</param>
        /// <returns>True if the decimal point can be inserted, false otherwise.</returns>
        private bool CanInsertDecimalPoint(string currentText)
        {
            // Check if a decimal point already exists in the text
            if (currentText.Contains("."))
            {
                return false;
            }
            
            // Check if trying to insert decimal point at the beginning (which would create ".123")
            if (CursorPosition == 0)
            {
                return false;
            }
            
            return true;
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
                // Validate decimal point for decimal input fields
                if (character == '.' && ValidChars == KeyCodeMap.ValidChars.Decimal)
                {
                    if (!CanInsertDecimalPoint(inputField.text))
                    {
                        return;
                    }
                }
                
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

        /// <summary>
        /// Calculates the cursor position based on mouse click coordinates within the input field.
        /// Uses Unity's TextGenerator for accurate character positioning.
        /// </summary>
        /// <param name="mousePosition">The mouse position in screen coordinates.</param>
        /// <returns>The calculated cursor position.</returns>
        public int CalculateCursorPositionFromMouse(Vector2 mousePosition)
        {
            InputField inputField = GetInputField();
            if (inputField == null) return 0;

            // Get the text component that displays the input field text
            Text textComponent = inputField.textComponent;
            if (textComponent == null) return 0;

            // Convert screen position to local position in the text component
            RectTransform textRectTransform = textComponent.GetComponent<RectTransform>();
            if (textRectTransform == null) return 0;

            Vector2 localPoint;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(textRectTransform, mousePosition, null, out localPoint))
            {
                return 0;
            }

            string text = inputField.text;
            if (string.IsNullOrEmpty(text)) return 0;

            // Get the text generation settings
            TextGenerationSettings settings = textComponent.GetGenerationSettings(textComponent.rectTransform.rect.size);
            TextGenerator textGenerator = new TextGenerator();
            textGenerator.Populate(text, settings);

            // Find the character index at the mouse position
            int characterIndex = 0;
            
            for (int i = 0; i < textGenerator.characters.Count; i++)
            {
                UICharInfo charInfo = textGenerator.characters[i];
                
                // If we're past the mouse position, we've found the closest character
                if (localPoint.x <= charInfo.cursorPos.x)
                {
                    characterIndex = i;
                    break;
                }
                characterIndex = i + 1;
            }

            // If we clicked beyond the last character, position at the end
            if (textGenerator.characters.Count > 0 && localPoint.x > textGenerator.characters[textGenerator.characters.Count - 1].cursorPos.x)
            {
                characterIndex = textGenerator.characters.Count;
            }

            // Ensure the position is within bounds
            return Mathf.Clamp(characterIndex, 0, text.Length);
        }

        /// <summary>
        /// Sets the cursor position directly and updates Unity's InputField.
        /// </summary>
        /// <param name="position">The cursor position to set.</param>
        public void SetCursorPositionDirectly(int position)
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
        /// Calculates and sets the cursor position based on mouse click coordinates.
        /// </summary>
        /// <param name="mousePosition">The mouse position in screen coordinates.</param>
        public void SetCursorPositionFromMouse(Vector2 mousePosition)
        {
            // Store the calculated position and set it after Unity processes the click
            int calculatedPosition = CalculateCursorPositionFromMouse(mousePosition);
            
            // Use coroutine to set cursor position after Unity has processed the click
            CoroutineRunner.Instance.StartCoroutine(SetCursorPositionCoroutine(calculatedPosition));
        }

        private IEnumerator SetCursorPositionCoroutine(int position)
        {
            yield return new UnityEngine.WaitForEndOfFrame();
            
            // Set the cursor position directly
            SetCursorPositionDirectly(position);
        }

        public void SyncCursorPositionNextFrame()
        {
            CoroutineRunner.Instance.StartCoroutine(SyncCursorCoroutine());
        }

        private IEnumerator SyncCursorCoroutine()
        {
            yield return new UnityEngine.WaitForEndOfFrame();
            
            InputField inputField = GetInputField();
            if (inputField != null && inputField.caretPosition == 0 && !string.IsNullOrEmpty(inputField.text))
            {
                // Unity didn't set the caret position properly on first click
                // Use mouse coordinate calculation as fallback
                // We need to get the mouse position from the main menu
                // For now, default to end of text
                SetCursorPositionDirectly(inputField.text.Length);
            }
            else
            {
                SyncCursorPositionFromUnity();
            }
        }
    }
}