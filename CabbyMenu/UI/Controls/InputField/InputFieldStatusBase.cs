using System;
using UnityEngine;

namespace CabbyMenu.UI.Controls.InputField
{
    /// <summary>
    /// Abstract base class for input field status, containing UI state and logic.
    /// </summary>
    public abstract class InputFieldStatusBase
    {
        public GameObject InputFieldGo { get; protected set; }
        public float LastUpdated { get; set; }
        public abstract bool IsSelected { get; }
        public int CursorPosition { get; protected set; } = 0;
        protected int horizontalOffset = 0;
        protected int maxVisibleCharacters;
        protected string fullText = string.Empty;
        public Action<bool> OnSelected { get; protected set; }
        public Action Submit { get; protected set; }
        public Action Cancel { get; protected set; }
        public abstract Utilities.KeyCodeMap.ValidChars ValidChars { get; }

        protected InputFieldStatusBase(GameObject inputFieldGo, Action<bool> onSelected, Action submit, Action cancel, int maxVisibleCharacters)
        {
            InputFieldGo = inputFieldGo;
            OnSelected = onSelected;
            Submit = submit;
            Cancel = cancel;
            this.maxVisibleCharacters = maxVisibleCharacters;
        }

        public abstract void SetSelected(bool selected);
        public abstract void InsertCharacter(char character, int characterLimit);
        public abstract void SetFullText(string text);
        public abstract string GetFullText();
        public abstract void DeleteCharacter();
        public abstract void DeleteForwardCharacter();
        public abstract bool DeleteSelectedText();
        public abstract bool ReplaceSelectedText(char character, int characterLimit);
        public abstract string GetVisibleText();
        public abstract int GetVisibleCursorPosition();
        public abstract void SetCursorPositionDirectly(int position);
        public abstract void MoveCursor(int offset);
        public abstract void SetCursorPosition(int position);
        public abstract void ResetHorizontalOffset();
        public abstract void UpdateDisplayText();
        public abstract void UpdateHorizontalOffsetForCursor();
        public abstract void UpdateUnityCursorPosition();
        public abstract void ForceCursorBlinkReset();
        public abstract UnityEngine.UI.InputField GetInputField();
        public abstract int GetIndex();
        public abstract (int start, int end)? GetTextSelection();
        public abstract int CalculateCursorPositionFromMouse(Vector2 mousePosition);
        public abstract void SetCursorPositionFromMouse(Vector2 mousePosition);
        public abstract void SyncCursorPositionFromUnity();
        public abstract void SyncSelectionFromUnity();
        public abstract void SyncCursorPositionNextFrame();
        public abstract bool WasSelected();
    }
} 