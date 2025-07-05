using UnityEngine.UI;
using UnityEngine;
using CabbyMenu.UI.Controls.CustomDropdown;
using CabbyMenu.SyncedReferences;

namespace CabbyMenu.UI.CheatPanels
{
    public class DropdownPanel : CheatPanel
    {
        private static readonly Vector2 leftMiddleMinAnchor = new Vector2(0, 0.5f); // Left-middle min anchor
        
        private readonly DropDownSync dropdown;
        private readonly GameObject dropdownPanel;

        public DropdownPanel(ISyncedValueList syncedValueReference, string description, float height) : base(description)
        {
            dropdownPanel = DefaultControls.CreatePanel(new DefaultControls.Resources());
            dropdownPanel.name = "Dropdown Panel";
            var image = dropdownPanel.GetComponent<Image>();
            if (image != null) image.color = Color.clear;
            new Fitter(dropdownPanel).Attach(cheatPanel);
            dropdownPanel.transform.SetAsFirstSibling();

            LayoutElement dropdownPanelLayout = dropdownPanel.AddComponent<LayoutElement>();
            dropdownPanelLayout.flexibleHeight = Constants.FLEXIBLE_LAYOUT_VALUE;
            // Let the dropdown determine its own width dynamically
            dropdownPanelLayout.flexibleWidth = 0f;
            dropdownPanelLayout.preferredHeight = height;
            dropdownPanelLayout.minWidth = Constants.MIN_PANEL_WIDTH;

            // Create dropdown using the new API
            var (dropDownSync, dropdownGameObject) = DropDownSync.Create(syncedValueReference);
            dropdown = dropDownSync;
            
            // Enable dynamic sizing and set options
            dropdown.SetDynamicSizing(true);
            dropdown.SetOptions(syncedValueReference.GetValueList());
            
            // Set height only (width will be calculated dynamically)
            dropdown.SetSize(0f, height);
            
            var customDropdown = dropdown.GetCustomDropdown();
            customDropdown.SetFontSize(Constants.DEFAULT_FONT_SIZE);
            customDropdown.SetColors(Constants.DROPDOWN_NORMAL, Constants.DROPDOWN_HOVER, Constants.DROPDOWN_PRESSED);

            new Fitter(dropdownGameObject).Attach(dropdownPanel).Anchor(leftMiddleMinAnchor, leftMiddleMinAnchor);
            dropdown.Update();
            
            // Force width recalculation to ensure dynamic sizing is applied
            customDropdown.ForceWidthRecalculation();
            
            // Update the dropdown panel width to match the dynamically calculated dropdown width
            UpdateDropdownPanelWidth();
        }
        
        /// <summary>
        /// Updates the dropdown panel width to match the dynamically calculated dropdown width.
        /// </summary>
        private void UpdateDropdownPanelWidth()
        {
            var customDropdown = dropdown.GetCustomDropdown();
            if (customDropdown != null)
            {
                // Get the actual width of the dropdown button
                RectTransform dropdownRect = customDropdown.GetComponent<RectTransform>();
                if (dropdownRect != null)
                {
                    float dropdownWidth = dropdownRect.sizeDelta.x;
                    
                    // Ensure we have a reasonable width (at least the minimum)
                    if (dropdownWidth < Constants.MIN_PANEL_WIDTH)
                    {
                        dropdownWidth = Constants.MIN_PANEL_WIDTH;
                    }
                    
                    // Update the dropdown panel's preferred width to match the actual dropdown width
                    LayoutElement dropdownPanelLayout = dropdownPanel.GetComponent<LayoutElement>();
                    if (dropdownPanelLayout != null)
                    {
                        dropdownPanelLayout.preferredWidth = dropdownWidth;
                        dropdownPanelLayout.minWidth = dropdownWidth;
                    }
                    
                    // Force layout rebuild to apply the new width
                    LayoutRebuilder.ForceRebuildLayoutImmediate(cheatPanel.GetComponent<RectTransform>());
                }
            }
        }
    }
}