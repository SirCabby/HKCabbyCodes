using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using CabbyMenu.Utilities;

namespace CabbyMenu.UI.Controls.InputField
{
    /// <summary>
    /// Manages input field selection, focus, and keyboard input handling.
    /// </summary>
    public class InputFieldManager
    {
        /// <summary>
        /// List of registered input fields for synchronization.
        /// </summary>
        private readonly List<InputFieldStatusBase> registeredInputs = new List<InputFieldStatusBase>();

        /// <summary>
        /// The last selected input field.
        /// </summary>
        private InputFieldStatusBase lastSelected;

        /// <summary>
        /// Registers an input field for synchronization.
        /// </summary>
        /// <param name="inputFieldStatus">The input field status to register.</param>
        public void RegisterInputFieldSync(InputFieldStatusBase inputFieldStatus)
        {
            registeredInputs.Add(inputFieldStatus);
        }

        /// <summary>
        /// Gets all registered input fields.
        /// </summary>
        /// <returns>A read-only list of registered input fields.</returns>
        public IReadOnlyList<InputFieldStatusBase> GetRegisteredInputs()
        {
            return registeredInputs.AsReadOnly();
        }

        /// <summary>
        /// Clears all registered input fields.
        /// </summary>
        public void ClearInputFields()
        {
            // Clear Unity's internal focus on all input fields before clearing the list
            foreach (InputFieldStatusBase input in registeredInputs)
            {
                if (input?.InputFieldGo != null)
                {
                    UnityEngine.UI.InputField inputField = input.InputFieldGo.GetComponent<UnityEngine.UI.InputField>();
                    inputField?.DeactivateInputField();
                }
            }
            registeredInputs.Clear();
        }

        /// <summary>
        /// Main update method for handling input field interactions.
        /// </summary>
        public void Update()
        {
            // Handle mouse clicks for input field selection
            if (Input.GetMouseButtonDown(0))
            {
                HandleMouseClick();
            }

            // Handle keyboard input for selected input field
            if (Input.anyKeyDown && lastSelected != null)
            {
                HandleKeyboardInput();
            }
        }

        /// <summary>
        /// Handles mouse click events for input field selection.
        /// </summary>
        private void HandleMouseClick()
        {
            Vector2 mousePosition = Input.mousePosition;
            InputFieldStatusBase clickedInput = null;

            // Check if registeredInputs is valid
            if (registeredInputs != null)
            {
                foreach (InputFieldStatusBase input in registeredInputs)
                {
                    if (input != null && IsMouseOverInputField(input, mousePosition))
                    {
                        clickedInput = input;
                        break;
                    }
                }
            }

            // Update selection
            if (clickedInput != null && clickedInput != lastSelected)
            {
                lastSelected?.Submit();
                lastSelected = clickedInput;
                
                if (clickedInput.InputFieldGo != null)
                {
                    // Let Unity's InputField process the click naturally to initialize its visual system
                    // Then apply our visual override after Unity has processed the click
                    clickedInput.StartCoroutine(ApplyVisualOverrideAfterUnityProcessing(clickedInput, mousePosition));
                }
            }
            else if (clickedInput == null && lastSelected != null)
            {
                // Clicked outside any input field, deselect current
                lastSelected.Submit();
                lastSelected = null;
                
                // Clear Unity's EventSystem selection
                var eventSystem = EventSystem.current;
                if (eventSystem != null)
                {
                    eventSystem.SetSelectedGameObject(null);
                }
            }
            else if (clickedInput == lastSelected && clickedInput != null)
            {
                // Clicked on the same input field - allow Unity's natural processing first
                if (clickedInput.InputFieldGo != null)
                {
                    // Let Unity's InputField process the click naturally to initialize its visual system
                    // Then apply our visual override after Unity has processed the click
                    clickedInput.StartCoroutine(ApplyVisualOverrideAfterUnityProcessing(clickedInput, mousePosition));
                }
            }

            // Update selection states
            SetInputFieldSelection(lastSelected);
        }

        /// <summary>
        /// Applies visual override after Unity has processed the click naturally.
        /// This ensures Unity's visual rendering system is properly initialized.
        /// </summary>
        /// <param name="inputStatus">The input field status to apply visual override to.</param>
        /// <param name="mousePosition">The mouse position for cursor calculation.</param>
        private System.Collections.IEnumerator ApplyVisualOverrideAfterUnityProcessing(InputFieldStatusBase inputStatus, Vector2 mousePosition)
        {
            // Wait for end of frame to ensure Unity has processed the click naturally
            yield return new WaitForEndOfFrame();
            
            // Now that Unity has processed the click naturally, apply our visual override
            inputStatus.SetCursorPositionFromMouse(mousePosition);
            UpdateInputFieldDisplay(inputStatus);
        }

        /// <summary>
        /// Handles keyboard input for the selected input field.
        /// </summary>
        private void HandleKeyboardInput()
        {
            // Sync selection state from Unity before processing input
            lastSelected.SyncSelectionFromUnity();
            
            // Handle arrow keys for cursor movement
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                lastSelected.MoveCursor(-1);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                lastSelected.MoveCursor(1);
            }
            // Handle Home key to move cursor to beginning
            else if (Input.GetKeyDown(KeyCode.Home))
            {
                lastSelected.SetCursorPosition(0);
            }
            // Handle End key to move cursor to end
            else if (Input.GetKeyDown(KeyCode.End))
            {
                lastSelected.SetCursorPosition(lastSelected.GetFullText().Length);
            }
            else
            {
                // Handle character input
                char? keyPressed = KeyCodeMap.GetChar(lastSelected.ValidChars);
                if (keyPressed.HasValue && lastSelected.InputFieldGo != null)
                {
                    UnityEngine.UI.InputField inputField = lastSelected.InputFieldGo.GetComponent<UnityEngine.UI.InputField>();
                    if (inputField != null)
                    {
                        lastSelected.InsertCharacter(keyPressed.Value, inputField.characterLimit);
                    }
                }

                // Handle backspace
                if (Input.GetKeyDown(KeyCode.Backspace))
                {
                    lastSelected.DeleteCharacter();
                }

                // Handle delete key
                if (Input.GetKeyDown(KeyCode.Delete))
                {
                    lastSelected.DeleteForwardCharacter();
                }
            }

            // Always update the display after any input handling
            UpdateInputFieldDisplay(lastSelected);

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                lastSelected.Submit();
                lastSelected = null;
                SetInputFieldSelection(lastSelected);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                lastSelected.Cancel();
                lastSelected = null;
                SetInputFieldSelection(lastSelected);
            }
        }

        /// <summary>
        /// Checks if the mouse is over a specific input field.
        /// </summary>
        /// <param name="inputStatus">The input field status to check.</param>
        /// <param name="mousePosition">The current mouse position.</param>
        /// <returns>True if the mouse is over the input field, false otherwise.</returns>
        private bool IsMouseOverInputField(InputFieldStatusBase inputStatus, Vector2 mousePosition)
        {
            if (inputStatus?.InputFieldGo == null) return false;
            RectTransform rectTransform = inputStatus.InputFieldGo.GetComponent<RectTransform>();
            if (rectTransform == null) return false;
            return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, mousePosition);
        }

        /// <summary>
        /// Sets selection state for all input fields, ensuring only one is selected.
        /// </summary>
        /// <param name="selected">The input field to select, or null to deselect all.</param>
        private void SetInputFieldSelection(InputFieldStatusBase selected)
        {
            if (registeredInputs == null) return;
            
            foreach (var input in registeredInputs)
            {
                if (input != null)
                {
                    bool shouldBeSelected = input == selected;
                    input.SetSelected(shouldBeSelected);
                }
            }
        }

        /// <summary>
        /// Updates the input field display.
        /// </summary>
        /// <param name="inputStatus">The input field status to update.</param>
        private void UpdateInputFieldDisplay(InputFieldStatusBase inputStatus)
        {
            if (inputStatus?.InputFieldGo == null) return;

            UnityEngine.UI.InputField inputField = inputStatus.InputFieldGo.GetComponent<UnityEngine.UI.InputField>();
            if (inputField != null)
            {
                // Update the display text to show the visible portion
                string visibleText = inputStatus.GetVisibleText();
                if (inputField.text != visibleText)
                {
                    inputField.text = visibleText;
                }
                
                // Update Unity's cursor position to match the visible cursor position
                int visibleCursorPos = inputStatus.GetVisibleCursorPosition();
                inputField.caretPosition = visibleCursorPos;
                
                // Only update selection if there's no active selection (to preserve user's selection)
                var selection = inputStatus.GetTextSelection();
                if (!selection.HasValue)
                {
                    inputField.selectionAnchorPosition = visibleCursorPos;
                    inputField.selectionFocusPosition = visibleCursorPos;
                }
            }
        }

        /// <summary>
        /// Submits the current input field value and deselects it.
        /// </summary>
        public void SubmitAndDeselect()
        {
            lastSelected?.Submit();
            lastSelected = null;
            SetInputFieldSelection(lastSelected);
        }

        /// <summary>
        /// Gets the currently selected input field.
        /// </summary>
        /// <returns>The currently selected input field, or null if none is selected.</returns>
        public InputFieldStatusBase GetSelectedInputField()
        {
            return lastSelected;
        }
    }
} 