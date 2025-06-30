using UnityEngine;

namespace CabbyMenu.UI.Utilities
{
    /// <summary>
    /// Utility class for calculating UI element widths based on character counts and text content.
    /// </summary>
    public static class UIWidthCalculator
    {
        /// <summary>
        /// Calculates the estimated width of a single character based on font size.
        /// This is used for panel sizing calculations.
        /// </summary>
        /// <param name="fontSize">The font size in pixels.</param>
        /// <returns>The estimated character width in pixels.</returns>
        public static float CalculateCharacterWidth(int fontSize)
        {
            // Estimate character width based on font size (more realistic for most fonts)
            // For most fonts, character width is roughly 0.4-0.5 times the font size
            return fontSize * 0.45f;
        }

        /// <summary>
        /// Calculates the character width for cursor positioning calculations.
        /// This uses a different multiplier for consistency with Unity's text rendering.
        /// </summary>
        /// <param name="fontSize">The font size in pixels.</param>
        /// <returns>The estimated character width for cursor positioning.</returns>
        public static float CalculateCursorCharacterWidth(int fontSize)
        {
            // Use a different multiplier for cursor positioning to match Unity's text rendering
            return fontSize * 0.65f;
        }

        /// <summary>
        /// Calculates the panel width based on character limit, ensuring it's never less than the minimum width.
        /// </summary>
        /// <param name="characterLimit">The maximum number of characters allowed.</param>
        /// <returns>The calculated width in pixels.</returns>
        public static int CalculatePanelWidth(int characterLimit)
        {
            float estimatedCharWidth = CalculateCharacterWidth(Constants.DEFAULT_FONT_SIZE);
            
            // Account for UI borders, padding, and margins that take up space on the sides
            float uiBuffer = 10f; // Reduced buffer for tighter panel width while keeping visible characters the same
            
            // Calculate width needed for the characters plus UI buffer
            float calculatedWidth = (characterLimit * estimatedCharWidth) + uiBuffer;
            
            // Ensure the width is never less than the minimum
            return Mathf.Max(Constants.MIN_PANEL_WIDTH, Mathf.RoundToInt(calculatedWidth));
        }

        /// <summary>
        /// Calculates the button width based on text length, ensuring it's never less than the minimum width.
        /// </summary>
        /// <param name="text">The button text.</param>
        /// <returns>The calculated width in pixels.</returns>
        public static int CalculateButtonWidth(string text)
        {
            if (string.IsNullOrEmpty(text))
                return Constants.MIN_PANEL_WIDTH;
                
            float estimatedCharWidth = CalculateCharacterWidth(Constants.DEFAULT_FONT_SIZE);
            
            // Account for padding/margins (add about 20 pixels for button borders/padding)
            float calculatedWidth = (text.Length * estimatedCharWidth) + 20f;
            
            // Ensure the width is never less than the minimum
            return Mathf.Max(Constants.MIN_PANEL_WIDTH, Mathf.RoundToInt(calculatedWidth));
        }
    }
} 