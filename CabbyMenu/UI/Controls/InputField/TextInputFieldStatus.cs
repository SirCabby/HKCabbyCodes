using System;
using UnityEngine;
using CabbyMenu.TextProcessors;

namespace CabbyMenu.UI.Controls.InputField
{
    /// <summary>
    /// Input field status for string/text fields.
    /// </summary>
    public class TextInputFieldStatus : InputFieldStatusBase
    {
        private readonly StringTextProcessor textProcessor;
        private bool isSelected = false;

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
                    SyncCursorPositionFromUnity();
                }
                else
                {
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
            }
        }

        public override UnityEngine.UI.InputField GetInputField() => InputFieldGo.GetComponent<UnityEngine.UI.InputField>();
        public override int GetIndex() => InputFieldGo.transform.GetParent().GetSiblingIndex();
        public override (int start, int end)? GetTextSelection() { return null; } // Implement if selection logic is needed
        public override int CalculateCursorPositionFromMouse(Vector2 mousePosition) { return 0; } // Implement if needed
        public override void SetCursorPositionFromMouse(Vector2 mousePosition) { }
        public override void SyncCursorPositionFromUnity() { }
        public override void SyncSelectionFromUnity() { }
        public override void SyncCursorPositionNextFrame() { }
        public override bool WasSelected() { return false; }

        public override Utilities.KeyCodeMap.ValidChars ValidChars => Utilities.KeyCodeMap.ValidChars.AlphaNumeric;
    }
} 