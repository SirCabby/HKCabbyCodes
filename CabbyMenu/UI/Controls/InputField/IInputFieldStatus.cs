using System;
using UnityEngine;
using UnityEngine.UI;
using CabbyMenu.Utilities;

namespace CabbyMenu.UI.Controls.InputField
{
    /// <summary>
    /// Non-generic interface for InputFieldStatus to allow type-safe handling of different generic types.
    /// </summary>
    public interface IInputFieldStatus
    {
        GameObject InputFieldGo { get; }
        float LastUpdated { get; set; }
        KeyCodeMap.ValidChars ValidChars { get; }
        bool IsSelected { get; }
        int CursorPosition { get; set; }
        Action<bool> OnSelected { get; }
        Action Submit { get; }
        Action Cancel { get; }
        
        void SetSelected(bool selected);
        void SyncCursorPositionFromUnity();
        void SyncSelectionFromUnity();
        bool WasSelected();
        UnityEngine.UI.InputField GetInputField();
        int GetIndex();
        void MoveCursor(int offset);
        void SetCursorPosition(int position);
        void ResetHorizontalOffset();
        string GetVisibleText();
        int GetVisibleCursorPosition();
        void InsertCharacter(char character, int characterLimit);
        void DeleteCharacter();
        void DeleteForwardCharacter();
        int CalculateCursorPositionFromMouse(Vector2 mousePosition);
        void SetCursorPositionDirectly(int position);
        void SetCursorPositionFromMouse(Vector2 mousePosition);
        void SyncCursorPositionNextFrame();
        void SetFullText(string text);
        string GetFullText();
        (int start, int end)? GetTextSelection();
        bool DeleteSelectedText();
        bool ReplaceSelectedText(char character, int characterLimit);
    }
} 