using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.Controls.InputField;
using CabbyMenu.Utilities;
using UnityEngine.UI;
using UnityEngine;

namespace CabbyMenu.UI.CheatPanels
{
    public class InputFieldPanel<T> : CheatPanel
    {
        private readonly BaseInputFieldSync<T> inputFieldSync;

        public InputFieldPanel(ISyncedReference<T> syncedReference, KeyCodeMap.ValidChars validChars, int characterLimit, string description) : base(description)
        {
            // Calculate the width based on character limit
            int calculatedWidth = CalculatePanelWidth(characterLimit);
            
            inputFieldSync = InputFieldSync.Create(syncedReference, validChars, new Vector2(calculatedWidth, Constants.DEFAULT_PANEL_HEIGHT), characterLimit);
            new Fitter(inputFieldSync.GetGameObject()).Attach(cheatPanel);
            inputFieldSync.GetGameObject().transform.SetAsFirstSibling();
            
            // Add LayoutElement to set the calculated width as preferred width (not flexible)
            LayoutElement inputFieldLayout = inputFieldSync.GetGameObject().AddComponent<LayoutElement>();
            inputFieldLayout.preferredWidth = calculatedWidth;
            inputFieldLayout.minWidth = calculatedWidth;
            
            updateActions.Add(inputFieldSync.Update);
        }

        /// <summary>
        /// Creates an input field panel with a custom width instead of calculating it from character limit.
        /// </summary>
        /// <param name="syncedReference">The synced reference for the value.</param>
        /// <param name="validChars">The valid character types.</param>
        /// <param name="characterLimit">The maximum number of characters allowed.</param>
        /// <param name="customWidth">The custom width in pixels for the input field.</param>
        /// <param name="description">The description text for the panel.</param>
        public InputFieldPanel(ISyncedReference<T> syncedReference, KeyCodeMap.ValidChars validChars, int characterLimit, int customWidth, string description) : base(description)
        {
            // Use the custom width instead of calculating it
            int width = Mathf.Max(Constants.MIN_PANEL_WIDTH, customWidth);
            
            inputFieldSync = InputFieldSync.Create(syncedReference, validChars, new Vector2(width, Constants.DEFAULT_PANEL_HEIGHT), characterLimit);
            new Fitter(inputFieldSync.GetGameObject()).Attach(cheatPanel);
            inputFieldSync.GetGameObject().transform.SetAsFirstSibling();
            
            // Add LayoutElement to set the custom width as preferred width (not flexible)
            LayoutElement inputFieldLayout = inputFieldSync.GetGameObject().AddComponent<LayoutElement>();
            inputFieldLayout.preferredWidth = width;
            inputFieldLayout.minWidth = width;
            
            updateActions.Add(inputFieldSync.Update);
        }

        public InputField GetInputField()
        {
            return inputFieldSync.GetGameObject().GetComponent<InputField>();
        }

        /// <summary>
        /// Forces an update of the input field UI regardless of selection state.
        /// </summary>
        public void ForceUpdate()
        {
            inputFieldSync.ForceUpdate();
        }

        /// <summary>
        /// Calculates the panel width based on character limit, ensuring it's never less than the minimum width.
        /// </summary>
        /// <param name="characterLimit">The maximum number of characters allowed.</param>
        /// <returns>The calculated width in pixels.</returns>
        private static int CalculatePanelWidth(int characterLimit)
        {
            // Use the cursor character width for panel sizing to match visible character logic
            float estimatedCharWidth = CalculateCursorCharacterWidth(Constants.DEFAULT_FONT_SIZE);
            float uiBuffer = 10f;
            float calculatedWidth = (characterLimit * estimatedCharWidth) + uiBuffer;
            return Mathf.Max(Constants.MIN_PANEL_WIDTH, Mathf.RoundToInt(calculatedWidth));
        }

        /// <summary>
        /// Calculates the character width for cursor positioning calculations.
        /// </summary>
        /// <param name="fontSize">The font size in pixels.</param>
        /// <returns>The estimated character width for cursor positioning.</returns>
        private static float CalculateCursorCharacterWidth(int fontSize)
        {
            return fontSize * 0.65f;
        }
    }
}