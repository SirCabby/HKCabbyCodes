using System;
using UnityEngine;
using UnityEngine.UI;
using CabbyMenu.TextProcessors;
using CabbyMenu.Utilities;

namespace CabbyMenu.UI.Controls.InputField
{
    /// <summary>
    /// Input field status for string/text fields.
    /// </summary>
    public class TextInputFieldStatus : InputFieldStatusBase
    {
        private readonly StringTextProcessor textProcessor;
        private bool isSelected = false;
        private bool pendingFirstClickCaret = false;

        public override bool IsSelected => isSelected;

        public TextInputFieldStatus(GameObject inputFieldGo, Action<bool> onSelected, Action submit, Action cancel, int maxVisibleCharacters, StringTextProcessor textProcessor)
            : base(inputFieldGo, onSelected, submit, cancel, maxVisibleCharacters)
        {
            this.textProcessor = textProcessor;
        }

        public override void SetSelected(bool selected)
        {
            if (InputFieldGo == null) return;
            if (selected != isSelected)
            {
                isSelected = selected;
                if (selected)
                {
                    pendingFirstClickCaret = true;
                    // Ensure cursor is on top when selected
                    EnsureCursorIsOnTop();
                }
                else
                {
                    // Deactivate when deselected
                    var inputField = GetInputField();
                    inputField?.DeactivateInputField();
                    ResetHorizontalOffset();
                    UpdateDisplayText();
                }
                OnSelected?.Invoke(selected);
            }
        }

        public override void InsertCharacter(char character, int characterLimit)
        {
            if (!textProcessor.CanInsertCharacter(character, fullText, CursorPosition))
                return;
            if (fullText.Length >= characterLimit)
                return;
            CursorPosition = Mathf.Clamp(CursorPosition, 0, fullText.Length);
            fullText = fullText.Substring(0, CursorPosition) + character + fullText.Substring(CursorPosition);
            CursorPosition++;
            int cursorPos = CursorPosition;
            fullText = textProcessor.ProcessTextAfterInsertion(fullText, ref cursorPos);
            CursorPosition = cursorPos;
            UpdateDisplayText();
            int maxPosition = Mathf.Min(fullText.Length, characterLimit);
            CursorPosition = Mathf.Clamp(CursorPosition, 0, maxPosition);
            UpdateHorizontalOffsetForCursor();
            UpdateUnityCursorPosition();
            ForceCursorBlinkReset();
        }

        public override void SetFullText(string text)
        {
            fullText = text ?? string.Empty;
            UpdateDisplayText();
            if (isSelected)
            {
                CursorPosition = Mathf.Clamp(CursorPosition, 0, fullText.Length);
                UpdateHorizontalOffsetForCursor();
                UpdateUnityCursorPosition();
                ForceCursorBlinkReset();
            }
        }

        public override string GetFullText() => fullText;

        public override void DeleteCharacter()
        {
            if (CursorPosition > 0)
            {
                fullText = fullText.Substring(0, CursorPosition - 1) + fullText.Substring(CursorPosition);
                CursorPosition--;
                UpdateDisplayText();
                UpdateHorizontalOffsetForCursor();
                UpdateUnityCursorPosition();
                ForceCursorBlinkReset();
            }
        }

        public override void DeleteForwardCharacter()
        {
            if (CursorPosition < fullText.Length)
            {
                fullText = fullText.Substring(0, CursorPosition) + fullText.Substring(CursorPosition + 1);
                UpdateDisplayText();
                UpdateHorizontalOffsetForCursor();
                UpdateUnityCursorPosition();
                ForceCursorBlinkReset();
            }
        }

        public override bool DeleteSelectedText() { return false; } // Implement if selection logic is needed
        public override bool ReplaceSelectedText(char character, int characterLimit) { return false; } // Implement if selection logic is needed

        public override string GetVisibleText()
        {
            int textLength = fullText.Length;
            if (textLength <= maxVisibleCharacters)
                return fullText;
            int visibleStart = horizontalOffset;
            int visibleLength = Mathf.Min(maxVisibleCharacters, textLength - horizontalOffset);
            return fullText.Substring(visibleStart, visibleLength);
        }

        public override int GetVisibleCursorPosition() => CursorPosition - horizontalOffset;

        public override void SetCursorPositionDirectly(int position)
        {
            int maxPosition = fullText.Length;
            CursorPosition = Mathf.Clamp(position, 0, maxPosition);
            UpdateHorizontalOffsetForCursor();
            UpdateUnityCursorPosition();
            ForceCursorBlinkReset();
        }

        public override void MoveCursor(int offset)
        {
            int newPosition = CursorPosition + offset;
            int maxPosition = fullText.Length;
            CursorPosition = Mathf.Clamp(newPosition, 0, maxPosition);
            UpdateHorizontalOffsetForCursor();
            UpdateUnityCursorPosition();
            ForceCursorBlinkReset();
        }

        public override void SetCursorPosition(int position)
        {
            int maxPosition = fullText.Length;
            CursorPosition = Mathf.Clamp(position, 0, maxPosition);
            UpdateHorizontalOffsetForCursor();
            UpdateUnityCursorPosition();
            ForceCursorBlinkReset();
        }

        public override void ResetHorizontalOffset()
        {
            horizontalOffset = 0;
        }

        public override void UpdateDisplayText()
        {
            var inputField = GetInputField();
            if (inputField != null)
            {
                string visibleText = GetVisibleText();
                if (inputField.text != visibleText)
                    inputField.text = visibleText;
            }
        }

        public override void UpdateHorizontalOffsetForCursor()
        {
            int textLength = fullText.Length;
            if (textLength <= maxVisibleCharacters)
            {
                horizontalOffset = 0;
                return;
            }
            int visibleStart = horizontalOffset;
            int cursorAfterVisibleEnd = horizontalOffset + maxVisibleCharacters;
            if (CursorPosition < visibleStart)
                horizontalOffset = Mathf.Max(0, horizontalOffset - 1);
            else if (CursorPosition > cursorAfterVisibleEnd)
                horizontalOffset = Mathf.Min(textLength - maxVisibleCharacters, horizontalOffset + 1);
            horizontalOffset = Mathf.Clamp(horizontalOffset, 0, Mathf.Max(0, textLength - maxVisibleCharacters));
        }

        public override void UpdateUnityCursorPosition()
        {
            var inputField = GetInputField();
            if (inputField != null)
            {
                int visibleCursorPosition = GetVisibleCursorPosition();
                inputField.selectionAnchorPosition = visibleCursorPosition;
                inputField.selectionFocusPosition = visibleCursorPosition;
                inputField.caretPosition = visibleCursorPosition;
            }
        }

        public override void ForceCursorBlinkReset()
        {
            var inputField = GetInputField();
            if (inputField != null)
            {
                inputField.selectionAnchorPosition = -1;
                inputField.selectionFocusPosition = -1;
                inputField.ForceLabelUpdate();
                UpdateUnityCursorPosition();
                
                // Ensure cursor is on top after forcing update
                EnsureCursorIsOnTop();
            }
        }

        public override UnityEngine.UI.InputField GetInputField() => InputFieldGo.GetComponent<UnityEngine.UI.InputField>();
        public override int GetIndex() => InputFieldGo.transform.GetParent().GetSiblingIndex();
        public override (int start, int end)? GetTextSelection() { return null; } // Implement if selection logic is needed
        public override int CalculateCursorPositionFromMouse(Vector2 mousePosition)
        {
            if (InputFieldGo == null) return 0;
            
            // Convert mouse position to local position within the input field
            RectTransform rectTransform = InputFieldGo.GetComponent<RectTransform>();
            if (rectTransform == null) return 0;

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, mousePosition, null, out Vector2 localPoint))
                return 0;

            // Find the text component to get font information
            Transform textTransform = InputFieldGo.transform.Find("Text");
            if (textTransform == null) return 0;
            
            Text textComponent = textTransform.GetComponent<Text>();
            if (textComponent == null) return 0;
            
            // Calculate cursor position based on mouse X position
            float charWidth = CalculateCharacterWidth(textComponent.fontSize);
            float textStartX = -rectTransform.rect.width / 2f + 5f; // Approximate text start position with padding
            
            // Calculate which character the mouse is closest to (relative to visible text)
            float relativeX = localPoint.x - textStartX;
            int visiblePosition = Mathf.RoundToInt(relativeX / charWidth);
            
            // Convert visible position to full text position by adding horizontal offset
            int calculatedPosition = visiblePosition + horizontalOffset;
            
            // Clamp to valid range
            return Mathf.Clamp(calculatedPosition, 0, fullText.Length);
        }
        
        public override void SetCursorPositionFromMouse(Vector2 mousePosition)
        {
            var inputField = GetInputField();
            if (inputField == null) return;

            if (pendingFirstClickCaret)
            {
                // For first click, let Unity handle the cursor positioning naturally
                // Then sync from Unity's position to our reference
                pendingFirstClickCaret = false;
                
                // Start a coroutine to sync from Unity after Unity has processed the click
                StartCoroutine(SyncFromUnityAfterClick(inputField));
            }
            else
            {
                // For subsequent clicks, let Unity handle positioning and sync from it
                StartCoroutine(SyncFromUnityAfterClick(inputField));
            }
        }

        private System.Collections.IEnumerator SyncFromUnityAfterClick(UnityEngine.UI.InputField inputField)
        {
            // Wait for end of frame to ensure Unity has processed the click naturally
            yield return new WaitForEndOfFrame();
            
            // Now sync our cursor position from Unity's position
            SyncCursorPositionFromUnity();
            
            // Update our display to match Unity's positioning
            UpdateDisplayText();
            UpdateHorizontalOffsetForCursor();
            ForceCursorBlinkReset();
            
            // Ensure cursor is on top after syncing from Unity
            EnsureCursorIsOnTop();
        }

        public override void SyncCursorPositionFromUnity()
        {
            var inputField = GetInputField();
            if (inputField != null)
            {
                CursorPosition = inputField.caretPosition + horizontalOffset;
                CursorPosition = Mathf.Clamp(CursorPosition, 0, fullText.Length);
            }
        }
        
        public override void SyncSelectionFromUnity()
        {
            // For now, just sync cursor position
            SyncCursorPositionFromUnity();
        }
        
        public override bool WasSelected()
        {
            return isSelected;
        }
        
        /// <summary>
        /// Calculates the estimated width of a single character based on font size.
        /// </summary>
        /// <param name="fontSize">The font size in pixels.</param>
        /// <returns>The estimated character width in pixels.</returns>
        private static float CalculateCharacterWidth(int fontSize)
        {
            return fontSize * 0.65f;
        }

        public override KeyCodeMap.ValidChars ValidChars => KeyCodeMap.ValidChars.AlphaNumeric;
    }
} 