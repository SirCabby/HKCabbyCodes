using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using CabbyMenu.UI.Controls.CustomDropdown;
using CabbyMenu.SyncedReferences;

namespace CabbyMenu.UI.CheatPanels
{
    public class DropdownPanel : CheatPanel
    {
        private static readonly Vector2 middle = Constants.MIDDLE_ANCHOR_VECTOR;

        private readonly DropDownSync dropdown;

        public DropdownPanel(ISyncedValueList syncedValueReference, string description) : base(description)
        {
            // Calculate width based on the longest option text
            int calculatedWidth = CalculateDropdownWidth(syncedValueReference.GetValueList());
            
            GameObject dropdownPanel = DefaultControls.CreatePanel(new DefaultControls.Resources());
            dropdownPanel.name = "Dropdown Panel";
            var image = dropdownPanel.GetComponent<Image>();
            if (image != null) image.color = Color.clear;
            new Fitter(dropdownPanel).Attach(cheatPanel);
            dropdownPanel.transform.SetAsFirstSibling();

            LayoutElement dropdownPanelLayout = dropdownPanel.AddComponent<LayoutElement>();
            dropdownPanelLayout.flexibleHeight = Constants.FLEXIBLE_LAYOUT_VALUE;
            dropdownPanelLayout.preferredWidth = calculatedWidth;
            dropdownPanelLayout.minWidth = calculatedWidth;

            dropdown = new DropDownSync(syncedValueReference);
            var customDropdown = dropdown.GetGameObject().GetComponent<CustomDropdown>();
            customDropdown.SetOptions(syncedValueReference.GetValueList());
            customDropdown.SetSize(calculatedWidth, Constants.DEFAULT_PANEL_HEIGHT);
            customDropdown.SetFontSize(Constants.DEFAULT_FONT_SIZE);
            customDropdown.SetColors(Constants.DROPDOWN_NORMAL, Constants.DROPDOWN_HOVER, Constants.DROPDOWN_PRESSED);

            new Fitter(dropdown.GetGameObject()).Attach(dropdownPanel).Anchor(middle, middle);
            dropdown.Update();
        }

        /// <summary>
        /// Calculates the dropdown width based on the longest option text.
        /// </summary>
        /// <param name="options">The list of dropdown options.</param>
        /// <returns>The calculated width in pixels.</returns>
        private int CalculateDropdownWidth(List<string> options)
        {
            if (options == null || options.Count == 0)
                return Constants.MIN_PANEL_WIDTH;
                
            // Find the longest option text
            string longestOption = options.OrderByDescending(opt => opt.Length).First();
            
            // Use the same calculation as button width but with dropdown-specific padding
            return CalculateButtonWidth(longestOption);
        }

        /// <summary>
        /// Calculates the button width based on text length, ensuring it's never less than the minimum width.
        /// </summary>
        /// <param name="text">The button text.</param>
        /// <returns>The calculated width in pixels.</returns>
        private static int CalculateButtonWidth(string text)
        {
            if (string.IsNullOrEmpty(text))
                return Constants.MIN_PANEL_WIDTH;
                
            float estimatedCharWidth = CalculateCharacterWidth(Constants.DEFAULT_FONT_SIZE);
            
            // Account for padding/margins (add about 20 pixels for button borders/padding)
            float calculatedWidth = (text.Length * estimatedCharWidth) + 20f;
            
            // Ensure the width is never less than the minimum
            return Mathf.Max(Constants.MIN_PANEL_WIDTH, Mathf.RoundToInt(calculatedWidth));
        }

        /// <summary>
        /// Calculates the estimated width of a single character based on font size.
        /// This is used for button sizing calculations.
        /// </summary>
        /// <param name="fontSize">The font size in pixels.</param>
        /// <returns>The estimated character width in pixels.</returns>
        private static float CalculateCharacterWidth(int fontSize)
        {
            // Estimate character width based on font size (more realistic for most fonts)
            // For most fonts, character width is roughly 0.4-0.5 times the font size
            return fontSize * 0.45f;
        }
    }
}