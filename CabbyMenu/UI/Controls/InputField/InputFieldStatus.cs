using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using CabbyMenu.TextProcessors;
using CabbyMenu.Utilities;
using CabbyMenu.UI.Controls.InputField;

namespace CabbyMenu.UI.Controls.InputField
{
    /// <summary>
    /// Manages the state and behavior of input fields in the mod UI.
    /// </summary>
    public class InputFieldStatus<T> : IInputFieldStatus
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
        /// The text processor for type-specific operations.
        /// </summary>
        private readonly ITextProcessor<T> textProcessor;

        /// <summary>
        /// The logical maximum value for this input field.
        /// </summary>
        private readonly T logicalMaxValue;

        /// <summary>
        /// Callback invoked when the input field selection state changes.
        /// </summary>
        public Action<bool> OnSelected { get; }

        /// <summary>
        /// Callback invoked when the input field value is submitted.
        /// </summary>
        public Action Submit { get; }

        /// <summary>
        /// Callback invoked when the input field input is cancelled.
        /// </summary>
        public Action Cancel { get; }

        /// <summary>
        /// Whether this input field is currently selected.
        /// </summary>
        private bool isSelected = false;

        /// <summary>
        /// Gets whether this input field is currently selected.
        /// </summary>
        public bool IsSelected => isSelected;

        /// <summary>
        /// The time when this input field was last selected.
        /// </summary>
        private float selectionTime = 0f;

        /// <summary>
        /// The current cursor position within the input field text.
        /// </summary>
        public int CursorPosition { get; set; } = 0;

        /// <summary>
        /// The horizontal offset for text scrolling when text is longer than the display area.
        /// </summary>
        private int horizontalOffset = 0;

        /// <summary>
        /// The maximum number of characters that can be displayed in the input field at once.
        /// </summary>
        private readonly int maxVisibleCharacters;

        /// <summary>
        /// The complete text value, separate from the display text shown in Unity's InputField.
        /// </summary>
        private string fullText = string.Empty;

        /// <summary>
        /// Initializes a new instance of the InputFieldStatus class.
        /// </summary>
        /// <param name="inputFieldGo">The GameObject containing the input field.</param>
        /// <param name="onSelected">Callback for selection state changes.</param>
        /// <param name="submit">Callback for value submission.</param>
        /// <param name="cancel">Callback for input cancellation.</param>
        /// <param name="validChars">The types of characters valid for this input field.</param>
        /// <param name="maxVisibleCharacters">The maximum number of characters that can be displayed in the input field at once.</param>
        /// <param name="textProcessor">The text processor for type-specific operations.</param>
        /// <param name="logicalMaxValue">The logical maximum value for this input field.</param>
        public InputFieldStatus(GameObject inputFieldGo, Action<bool> onSelected, Action submit, Action cancel, KeyCodeMap.ValidChars validChars, int maxVisibleCharacters, ITextProcessor<T> textProcessor, T logicalMaxValue)
        {
            InputFieldGo = inputFieldGo;
            ValidChars = validChars;
            OnSelected = onSelected;
            Submit = submit;
            Cancel = cancel;
            this.maxVisibleCharacters = maxVisibleCharacters;
            this.textProcessor = textProcessor;
            this.logicalMaxValue = logicalMaxValue;
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
                else
                {
                    // Reset horizontal offset when losing focus to show the beginning of the text
                    ResetHorizontalOffset();
                    // Update the display text to show the beginning characters
                    UpdateDisplayText();
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
            UnityEngine.UI.InputField inputField = GetInputField();
            if (inputField != null)
            {
                // Get Unity's current cursor position
                int unityCursorPosition = inputField.caretPosition;
                
                // Convert Unity's visible cursor position to our full text cursor position
                int fullTextCursorPosition = horizontalOffset + unityCursorPosition;
                
                // Update our logical cursor position to match, considering both text length and character limit
                int maxPosition = Mathf.Min(fullText.Length, inputField.characterLimit);
                CursorPosition = Mathf.Clamp(fullTextCursorPosition, 0, maxPosition);
                
                // Update horizontal offset to ensure cursor is visible
                UpdateHorizontalOffsetForCursor();
                
                // Update Unity's InputField cursor position to match our visible cursor position
                UpdateUnityCursorPosition();
            }
        }

        /// <summary>
        /// Synchronizes our selection state with Unity's InputField selection.
        /// </summary>
        public void SyncSelectionFromUnity()
        {
            UnityEngine.UI.InputField inputField = GetInputField();
            if (inputField != null)
            {
                // Get Unity's current selection positions
                int unityAnchorPos = inputField.selectionAnchorPosition;
                int unityFocusPos = inputField.selectionFocusPosition;
                
                // Update Unity's selection positions to reflect the full text positions
                inputField.selectionAnchorPosition = unityAnchorPos;
                inputField.selectionFocusPosition = unityFocusPos;
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
        public UnityEngine.UI.InputField GetInputField()
        {
            return InputFieldGo.GetComponent<UnityEngine.UI.InputField>();
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
            UnityEngine.UI.InputField inputField = GetInputField();
            if (inputField != null)
            {
                // Temporarily hide the cursor by setting selection to -1
                inputField.selectionAnchorPosition = -1;
                inputField.selectionFocusPosition = -1;
                
                // Force a frame update
                inputField.ForceLabelUpdate();
                
                // Immediately restore the cursor position
                UpdateUnityCursorPosition();
            }
        }

        /// <summary>
        /// Centralized method to update Unity's InputField cursor position.
        /// This ensures consistent cursor positioning across all operations.
        /// </summary>
        private void UpdateUnityCursorPosition()
        {
            UnityEngine.UI.InputField inputField = GetInputField();
            if (inputField != null)
            {
                int visibleCursorPosition = GetVisibleCursorPosition();
                inputField.selectionAnchorPosition = visibleCursorPosition;
                inputField.selectionFocusPosition = visibleCursorPosition;
                inputField.caretPosition = visibleCursorPosition;
            }
        }

        /// <summary>
        /// Moves the cursor position by the specified offset, ensuring it stays within bounds.
        /// </summary>
        /// <param name="offset">The offset to move the cursor by (positive for right, negative for left).</param>
        public void MoveCursor(int offset)
        {
            UnityEngine.Debug.Log($"MoveCursor: offset={offset}, currentCursor={CursorPosition}, fullText='{fullText}'");
            int newPosition = CursorPosition + offset;
            int maxPosition = fullText.Length;
            CursorPosition = Mathf.Clamp(newPosition, 0, maxPosition);
            UnityEngine.Debug.Log($"MoveCursor: newCursor={CursorPosition}");
            
            // Update horizontal offset to ensure cursor is visible
            UpdateHorizontalOffsetForCursor();
            
            // Update Unity's InputField cursor position immediately
            UpdateUnityCursorPosition();
            
            // Force cursor to reset blink cycle
            ForceCursorBlinkReset();
        }

        /// <summary>
        /// Sets the cursor position to a specific index, ensuring it stays within bounds.
        /// </summary>
        /// <param name="position">The new cursor position.</param>
        public void SetCursorPosition(int position)
        {
            int maxPosition = fullText.Length;
            CursorPosition = Mathf.Clamp(position, 0, maxPosition);
            
            // Update horizontal offset to ensure cursor is visible
            UpdateHorizontalOffsetForCursor();
            
            // Update Unity's InputField cursor position immediately
            UpdateUnityCursorPosition();
            
            // Force cursor to reset blink cycle
            ForceCursorBlinkReset();
        }

        /// <summary>
        /// Updates the horizontal offset to ensure the cursor is always visible within the display area.
        /// </summary>
        private void UpdateHorizontalOffsetForCursor()
        {
            int textLength = fullText.Length;

            // If text is shorter than or equal to max visible characters, no offset needed
            if (textLength <= maxVisibleCharacters)
            {
                horizontalOffset = 0;
                return;
            }

            // Calculate the visible range
            int visibleStart = horizontalOffset;
            int visibleEnd = horizontalOffset + maxVisibleCharacters - 1;
            int cursorAfterVisibleEnd = horizontalOffset + maxVisibleCharacters; // Position after last visible character

            // Debug logging
            UnityEngine.Debug.Log($"UpdateHorizontalOffsetForCursor: fullText='{fullText}', length={textLength}, cursor={CursorPosition}, offset={horizontalOffset}, visible={visibleStart}-{visibleEnd}, cursorAfterVisible={cursorAfterVisibleEnd}, maxVisible={maxVisibleCharacters}");

            // Only slide the window if the cursor is outside the visible range AND beyond the position after the last visible character
            if (CursorPosition < visibleStart)
            {
                // Cursor is to the left of visible area, slide window left by one character
                horizontalOffset = Mathf.Max(0, horizontalOffset - 1);
                UnityEngine.Debug.Log($"Sliding left: new offset = {horizontalOffset}");
            }
            else if (CursorPosition > cursorAfterVisibleEnd)
            {
                // Cursor is beyond the position after the last visible character, slide window right by one character
                horizontalOffset = Mathf.Min(textLength - maxVisibleCharacters, horizontalOffset + 1);
                UnityEngine.Debug.Log($"Sliding right: new offset = {horizontalOffset}");
            }

            // Ensure offset doesn't go negative or beyond text length
            horizontalOffset = Mathf.Clamp(horizontalOffset, 0, Mathf.Max(0, textLength - maxVisibleCharacters));
            UnityEngine.Debug.Log($"Final offset after clamp: {horizontalOffset}");
        }

        /// <summary>
        /// Resets the horizontal offset to show the beginning of the text.
        /// This is called when the input field loses focus.
        /// </summary>
        public void ResetHorizontalOffset()
        {
            horizontalOffset = 0;
            UnityEngine.Debug.Log($"ResetHorizontalOffset: offset set to 0");
        }

        /// <summary>
        /// Gets the visible portion of the text based on the current horizontal offset.
        /// </summary>
        /// <returns>The visible portion of the text.</returns>
        public string GetVisibleText()
        {
            int textLength = fullText.Length;

            // If text is shorter than max visible characters, return the full text
            if (textLength <= maxVisibleCharacters)
            {
                UnityEngine.Debug.Log($"GetVisibleText: text shorter than maxVisible, returning full text: '{fullText}'");
                return fullText;
            }

            // Calculate the visible range
            int visibleStart = horizontalOffset;
            int visibleLength = Mathf.Min(maxVisibleCharacters, textLength - horizontalOffset);

            // Return the visible portion of the text
            string visibleText = fullText.Substring(visibleStart, visibleLength);
            UnityEngine.Debug.Log($"GetVisibleText: fullText='{fullText}', offset={horizontalOffset}, visibleStart={visibleStart}, visibleLength={visibleLength}, result='{visibleText}'");
            return visibleText;
        }

        /// <summary>
        /// Gets the cursor position relative to the visible text.
        /// </summary>
        /// <returns>The cursor position relative to the visible text.</returns>
        public int GetVisibleCursorPosition()
        {
            return CursorPosition - horizontalOffset;
        }

        /// <summary>
        /// Inserts a character at the current cursor position.
        /// </summary>
        /// <param name="character">The character to insert.</param>
        /// <param name="characterLimit">The maximum number of characters allowed.</param>
        public void InsertCharacter(char character, int characterLimit)
        {
            // First, check if there's a text selection and handle it
            if (ReplaceSelectedText(character, characterLimit))
            {
                return; // Selection was replaced, we're done
            }
            
            // Use text processor to validate character insertion
            if (!textProcessor.CanInsertCharacter(character, fullText, CursorPosition))
            {
                return;
            }
            
            // Ensure cursor position is within valid bounds before any operation
            CursorPosition = Mathf.Clamp(CursorPosition, 0, fullText.Length);
            
            // Create a temporary text to check if adding this character would reach max digits
            string tempText = fullText.Substring(0, CursorPosition) + character + fullText.Substring(CursorPosition);
            
            // Check if this would reach maximum characters for the type
            if (textProcessor.HasReachedMaxCharacters(tempText, characterLimit))
            {
                // Set the text to the logical maximum value
                fullText = textProcessor.ConvertValue(logicalMaxValue);
                CursorPosition = fullText.Length; // Set cursor to end
                
                // Update the display text
                UpdateDisplayText();
                
                // Update horizontal offset to ensure cursor is visible
                UpdateHorizontalOffsetForCursor();
                
                // Update Unity's InputField cursor position immediately
                UpdateUnityCursorPosition();
                
                // Force cursor to reset blink cycle
                ForceCursorBlinkReset();
                return;
            }
            
            // If at character limit, replace the character at cursor position
            if (fullText.Length >= characterLimit && CursorPosition <= fullText.Length)
            {
                // If cursor is at the end, replace the last character
                if (CursorPosition == fullText.Length)
                {
                    fullText = fullText.Substring(0, fullText.Length - 1) + character;
                    // Keep cursor at the end
                }
                else
                {
                    // Replace character at cursor position
                    fullText = fullText.Substring(0, CursorPosition) + character + fullText.Substring(CursorPosition + 1);
                    // Move cursor to the right when replacing a character
                    CursorPosition++;
                }
            }
            // Otherwise insert at cursor position if we haven't reached the limit
            else if (fullText.Length < characterLimit)
            {
                fullText = fullText.Substring(0, CursorPosition) + character + fullText.Substring(CursorPosition);
                CursorPosition++;
            }
            // If we're at the limit and cursor is at the end, don't allow insertion
            else
            {
                return;
            }
            
            // Use text processor to process text after insertion (e.g., remove leading zeros)
            int cursorPos = CursorPosition;
            fullText = textProcessor.ProcessTextAfterInsertion(fullText, ref cursorPos);
            CursorPosition = cursorPos;
            
            // Update the display text
            UpdateDisplayText();
            
            // Ensure cursor position is still within bounds after text modification
            int maxPosition = Mathf.Min(fullText.Length, characterLimit);
            CursorPosition = Mathf.Clamp(CursorPosition, 0, maxPosition);
            
            // Update horizontal offset to ensure cursor is visible
            UpdateHorizontalOffsetForCursor();
            
            // Update Unity's InputField cursor position immediately
            UpdateUnityCursorPosition();
            
            // Force cursor to reset blink cycle
            ForceCursorBlinkReset();
        }

        /// <summary>
        /// Deletes the character at the current cursor position (backspace).
        /// </summary>
        public void DeleteCharacter()
        {
            // First, check if there's a text selection and handle it
            if (DeleteSelectedText())
            {
                return; // Selection was deleted, we're done
            }
            
            if (CursorPosition > 0)
            {
                fullText = fullText.Substring(0, CursorPosition - 1) + fullText.Substring(CursorPosition);
                CursorPosition--;
                
                // Update the display text
                UpdateDisplayText();
                
                // Update horizontal offset to ensure cursor is visible
                UpdateHorizontalOffsetForCursor();
                
                // Update Unity's InputField cursor position immediately
                UpdateUnityCursorPosition();
                
                // Force cursor to reset blink cycle
                ForceCursorBlinkReset();
            }
        }

        /// <summary>
        /// Deletes the character after the current cursor position (delete key).
        /// </summary>
        public void DeleteForwardCharacter()
        {
            // First, check if there's a text selection and handle it
            if (DeleteSelectedText())
            {
                return; // Selection was deleted, we're done
            }
            
            if (CursorPosition < fullText.Length)
            {
                fullText = fullText.Substring(0, CursorPosition) + fullText.Substring(CursorPosition + 1);
                
                // Update the display text
                UpdateDisplayText();
                
                // Update horizontal offset to ensure cursor is visible
                UpdateHorizontalOffsetForCursor();
                
                // Update Unity's InputField cursor position immediately
                UpdateUnityCursorPosition();
                
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
            UnityEngine.UI.InputField inputField = GetInputField();
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

            string text = GetVisibleText();
            if (string.IsNullOrEmpty(text)) return 0;

            // Get the text generation settings
            TextGenerationSettings settings = textComponent.GetGenerationSettings(textComponent.rectTransform.rect.size);
            TextGenerator textGenerator = new TextGenerator();
            textGenerator.Populate(text, settings);

            // Find the character index at the mouse position
            int characterIndex = 0;
            
            // If we clicked to the left of the first character, position at the beginning of visible text
            if (textGenerator.characters.Count > 0 && localPoint.x < textGenerator.characters[0].cursorPos.x)
            {
                characterIndex = 0;
                UnityEngine.Debug.Log($"Clicked left of first character: localPoint.x={localPoint.x}, firstCharPos={textGenerator.characters[0].cursorPos.x}, setting characterIndex=0");
            }
            else
            {
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

                // If we clicked beyond the last character in the visible text, position at the end of visible text
                if (textGenerator.characters.Count > 0 && localPoint.x > textGenerator.characters[textGenerator.characters.Count - 1].cursorPos.x)
                {
                    characterIndex = textGenerator.characters.Count;
                }
            }

            // Convert the visible text position to the full text position
            int fullTextPosition = horizontalOffset + characterIndex;
            
            UnityEngine.Debug.Log($"CalculateCursorPositionFromMouse: horizontalOffset={horizontalOffset}, characterIndex={characterIndex}, fullTextPosition={fullTextPosition}");
            
            // Ensure the position is within bounds of the full text
            fullTextPosition = Mathf.Clamp(fullTextPosition, 0, fullText.Length);
            
            // When clicking, only allow positioning within the currently visible text area
            // Don't trigger any sliding - that should only happen with arrow key navigation
            int visibleStart = horizontalOffset;
            int cursorAfterVisibleEnd = horizontalOffset + maxVisibleCharacters;
            
            // Clamp to the visible range (including the position after the last visible character)
            if (fullTextPosition < visibleStart)
            {
                fullTextPosition = visibleStart;
                UnityEngine.Debug.Log($"Clamped to visible start: {fullTextPosition}");
            }
            else if (fullTextPosition > cursorAfterVisibleEnd)
            {
                fullTextPosition = Mathf.Min(cursorAfterVisibleEnd, fullText.Length);
                UnityEngine.Debug.Log($"Clamped to cursorAfterVisibleEnd: {fullTextPosition}");
            }
            
            UnityEngine.Debug.Log($"Final calculated position: {fullTextPosition}");
            return fullTextPosition;
        }

        /// <summary>
        /// Sets the cursor position directly and updates Unity's InputField.
        /// </summary>
        /// <param name="position">The cursor position to set.</param>
        public void SetCursorPositionDirectly(int position)
        {
            int maxPosition = fullText.Length;
            CursorPosition = Mathf.Clamp(position, 0, maxPosition);
            
            UnityEngine.Debug.Log($"SetCursorPositionDirectly: requested={position}, clamped={CursorPosition}, fullText='{fullText}', horizontalOffset={horizontalOffset}");
            
            // Update horizontal offset to ensure cursor is visible
            UpdateHorizontalOffsetForCursor();
            
            // Update Unity's InputField cursor position immediately
            UpdateUnityCursorPosition();
            
            // Force cursor to reset blink cycle
            ForceCursorBlinkReset();
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
            yield return new WaitForEndOfFrame();
            
            // Set the cursor position directly
            SetCursorPositionDirectly(position);
        }

        public void SyncCursorPositionNextFrame()
        {
            CoroutineRunner.Instance.StartCoroutine(SyncCursorCoroutine());
        }

        private IEnumerator SyncCursorCoroutine()
        {
            yield return new WaitForEndOfFrame();
            
            UnityEngine.UI.InputField inputField = GetInputField();
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

        /// <summary>
        /// Sets the complete text value and updates the display.
        /// </summary>
        /// <param name="text">The complete text to set.</param>
        public void SetFullText(string text)
        {
            fullText = text ?? string.Empty;
            
            // Always update the display text to ensure Unity's InputField is synchronized
            // This is especially important when validation/capping has occurred
            UpdateDisplayText();
            
            // If the field is currently selected, ensure cursor position is valid
            if (isSelected)
            {
                // Ensure cursor position is within bounds of the new text
                CursorPosition = Mathf.Clamp(CursorPosition, 0, fullText.Length);
                
                // Update horizontal offset to ensure cursor is visible
                UpdateHorizontalOffsetForCursor();
                
                // Update Unity's InputField cursor position to match our visible cursor position
                UpdateUnityCursorPosition();
                
                // Force cursor to reset blink cycle
                ForceCursorBlinkReset();
            }
        }

        /// <summary>
        /// Gets the complete text value.
        /// </summary>
        /// <returns>The complete text value.</returns>
        public string GetFullText()
        {
            return fullText;
        }

        /// <summary>
        /// Updates the display text in Unity's InputField to show the visible portion.
        /// </summary>
        private void UpdateDisplayText()
        {
            UnityEngine.UI.InputField inputField = GetInputField();
            if (inputField != null)
            {
                string visibleText = GetVisibleText();
                UnityEngine.Debug.Log($"UpdateDisplayText: fullText='{fullText}', visibleText='{visibleText}', currentInputFieldText='{inputField.text}'");
                if (inputField.text != visibleText)
                {
                    inputField.text = visibleText;
                    UnityEngine.Debug.Log($"Updated InputField text to: '{visibleText}'");
                }
            }
        }

        /// <summary>
        /// Gets the current text selection range from Unity's InputField.
        /// </summary>
        /// <returns>A tuple containing (start, end) positions, or null if no selection.</returns>
        public (int start, int end)? GetTextSelection()
        {
            UnityEngine.UI.InputField inputField = GetInputField();
            if (inputField == null) return null;

            int anchorPos = inputField.selectionAnchorPosition;
            int focusPos = inputField.selectionFocusPosition;

            // If anchor and focus are the same, there's no selection
            if (anchorPos == focusPos) return null;

            // Convert visible positions to full text positions
            int fullTextAnchorPos = horizontalOffset + anchorPos;
            int fullTextFocusPos = horizontalOffset + focusPos;

            // Ensure positions are within bounds
            fullTextAnchorPos = Mathf.Clamp(fullTextAnchorPos, 0, fullText.Length);
            fullTextFocusPos = Mathf.Clamp(fullTextFocusPos, 0, fullText.Length);

            // Return start and end positions (start should be the smaller value)
            int start = Mathf.Min(fullTextAnchorPos, fullTextFocusPos);
            int end = Mathf.Max(fullTextAnchorPos, fullTextFocusPos);

            return (start, end);
        }

        /// <summary>
        /// Deletes the currently selected text.
        /// </summary>
        /// <returns>True if text was deleted, false if no selection.</returns>
        public bool DeleteSelectedText()
        {
            var selection = GetTextSelection();
            if (!selection.HasValue) return false;

            int start = selection.Value.start;
            int end = selection.Value.end;

            // Remove the selected text
            fullText = fullText.Substring(0, start) + fullText.Substring(end);
            
            // Set cursor position to the start of the deleted selection
            CursorPosition = start;
            
            // Update the display text
            UpdateDisplayText();
            
            // Update horizontal offset to ensure cursor is visible
            UpdateHorizontalOffsetForCursor();
            
            // Update Unity's InputField cursor position immediately
            UpdateUnityCursorPosition();
            
            // Clear the selection in Unity's InputField
            UnityEngine.UI.InputField replaceInputField = GetInputField();
            if (replaceInputField != null)
            {
                replaceInputField.selectionAnchorPosition = replaceInputField.caretPosition;
                replaceInputField.selectionFocusPosition = replaceInputField.caretPosition;
            }
            
            // Force cursor to reset blink cycle
            ForceCursorBlinkReset();
            
            return true;
        }

        /// <summary>
        /// Replaces the currently selected text with a new character.
        /// </summary>
        /// <param name="character">The character to insert.</param>
        /// <param name="characterLimit">The maximum number of characters allowed.</param>
        /// <returns>True if text was replaced, false if no selection.</returns>
        public bool ReplaceSelectedText(char character, int characterLimit)
        {
            var selection = GetTextSelection();
            if (!selection.HasValue) return false;

            int start = selection.Value.start;
            int end = selection.Value.end;
            int selectionLength = end - start;

            // Use text processor to validate character insertion
            if (!textProcessor.CanInsertCharacter(character, fullText, start))
            {
                return false;
            }

            // Calculate the new text length after replacement
            int newLength = fullText.Length - selectionLength + 1;
            
            // Check if this would exceed the character limit
            if (newLength > characterLimit)
            {
                return false;
            }

            // Create a temporary text to check if replacing would reach max characters
            string tempText = fullText.Substring(0, start) + character + fullText.Substring(end);
            
            // Check if this would reach maximum characters for the type
            if (textProcessor.HasReachedMaxCharacters(tempText, characterLimit))
            {
                // Set the text to the logical maximum value
                fullText = textProcessor.ConvertValue(logicalMaxValue);
                CursorPosition = fullText.Length; // Set cursor to end
                
                // Update the display text
                UpdateDisplayText();
                
                // Update horizontal offset to ensure cursor is visible
                UpdateHorizontalOffsetForCursor();
                
                // Update Unity's InputField cursor position immediately
                UpdateUnityCursorPosition();
                
                // Clear the selection in Unity's InputField
                UnityEngine.UI.InputField replaceInputField2 = GetInputField();
                if (replaceInputField2 != null)
                {
                    replaceInputField2.selectionAnchorPosition = replaceInputField2.caretPosition;
                    replaceInputField2.selectionFocusPosition = replaceInputField2.caretPosition;
                }
                
                // Force cursor to reset blink cycle
                ForceCursorBlinkReset();
                return true;
            }

            // Replace the selected text with the new character
            fullText = fullText.Substring(0, start) + character + fullText.Substring(end);
            
            // Set cursor position after the inserted character
            CursorPosition = start + 1;
            
            // Update the display text
            UpdateDisplayText();
            
            // Update horizontal offset to ensure cursor is visible
            UpdateHorizontalOffsetForCursor();
            
            // Update Unity's InputField cursor position immediately
            UpdateUnityCursorPosition();
            
            // Clear the selection in Unity's InputField
            UnityEngine.UI.InputField replaceInputField3 = GetInputField();
            if (replaceInputField3 != null)
            {
                replaceInputField3.selectionAnchorPosition = replaceInputField3.caretPosition;
                replaceInputField3.selectionFocusPosition = replaceInputField3.caretPosition;
            }
            
            // Force cursor to reset blink cycle
            ForceCursorBlinkReset();
            
            return true;
        }
    }
}