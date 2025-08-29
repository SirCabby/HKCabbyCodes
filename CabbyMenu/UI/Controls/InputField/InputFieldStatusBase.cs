using System;
using UnityEngine;
using CabbyMenu.Utilities;

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
        public abstract KeyCodeMap.ValidChars ValidChars { get; }

        public GameObject GetGameObject() => InputFieldGo;

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
        public abstract bool WasSelected();
        
        /// <summary>
        /// Starts a coroutine on the input field GameObject.
        /// </summary>
        /// <param name="coroutine">The coroutine to start.</param>
        public Coroutine StartCoroutine(System.Collections.IEnumerator coroutine)
        {
            if (InputFieldGo != null)
            {
                var coroutineRunner = InputFieldGo.GetComponent<CoroutineRunner>() ?? InputFieldGo.AddComponent<CoroutineRunner>();
                return coroutineRunner.StartCoroutine(coroutine);
            }
            return null;
        }
        
        /// <summary>
        /// Ensures the cursor is brought to the front when the input field is selected.
        /// </summary>
        public void EnsureCursorIsOnTop()
        {
            if (InputFieldGo == null) return;
            
            // Unity's InputField cursor is typically a child GameObject that gets created when the input field is activated
            // We need to find it and ensure it's rendered on top of other UI elements
            
            // Look for Unity's cursor GameObject - it's typically named "Caret" or similar
            Transform caretTransform = InputFieldGo.transform.Find("Caret");
            if (caretTransform != null)
            {
                // Move the caret to the very front
                caretTransform.SetAsLastSibling();
                
                // Also ensure the caret's Canvas component has the highest sorting order if it exists
                Canvas caretCanvas = caretTransform.GetComponent<Canvas>();
                if (caretCanvas != null)
                {
                    caretCanvas.sortingOrder = 32767; // Maximum sorting order
                }
            }
            
            // Also check for any child with "caret" in the name (case insensitive)
            for (int i = 0; i < InputFieldGo.transform.childCount; i++)
            {
                Transform child = InputFieldGo.transform.GetChild(i);
                if (child.name.ToLowerInvariant().Contains("caret"))
                {
                    child.SetAsLastSibling();
                    
                    // Ensure the caret's Canvas component has the highest sorting order if it exists
                    Canvas childCanvas = child.GetComponent<Canvas>();
                    if (childCanvas != null)
                    {
                        childCanvas.sortingOrder = 32767; // Maximum sorting order
                    }
                }
            }
            
            // Force the input field to update its rendering order
            Canvas inputFieldCanvas = InputFieldGo.GetComponent<Canvas>();
            if (inputFieldCanvas != null)
            {
                inputFieldCanvas.sortingOrder = 32766; // High sorting order, just below caret
            }
        }
    }
} 