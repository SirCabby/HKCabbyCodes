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
        private static readonly Vector2 leftMiddleMinAnchor = new Vector2(0, 0.5f); // Left-middle min anchor
        
        private readonly DropDownSync dropdown;

        public DropdownPanel(ISyncedValueList syncedValueReference, string description) : base(description)
        {
            GameObject dropdownPanel = DefaultControls.CreatePanel(new DefaultControls.Resources());
            dropdownPanel.name = "Dropdown Panel";
            var image = dropdownPanel.GetComponent<Image>();
            if (image != null) image.color = Color.clear;
            new Fitter(dropdownPanel).Attach(cheatPanel);
            dropdownPanel.transform.SetAsFirstSibling();

            LayoutElement dropdownPanelLayout = dropdownPanel.AddComponent<LayoutElement>();
            dropdownPanelLayout.flexibleHeight = Constants.FLEXIBLE_LAYOUT_VALUE;
            // Use flexible width to allow dynamic sizing
            dropdownPanelLayout.flexibleWidth = Constants.FLEXIBLE_LAYOUT_VALUE;
            dropdownPanelLayout.minWidth = Constants.MIN_PANEL_WIDTH;

            // Create dropdown using the new API
            var (dropDownSync, dropdownGameObject) = DropDownSync.Create(syncedValueReference);
            dropdown = dropDownSync;
            
            // Enable dynamic sizing and set options
            dropdown.SetDynamicSizing(true);
            dropdown.SetOptions(syncedValueReference.GetValueList());
            
            // Set height only (width will be calculated dynamically)
            dropdown.SetSize(Constants.MIN_PANEL_WIDTH, Constants.DEFAULT_PANEL_HEIGHT);
            
            var customDropdown = dropdown.GetCustomDropdown();
            customDropdown.SetFontSize(Constants.DEFAULT_FONT_SIZE);
            customDropdown.SetColors(Constants.DROPDOWN_NORMAL, Constants.DROPDOWN_HOVER, Constants.DROPDOWN_PRESSED);

            new Fitter(dropdownGameObject).Attach(dropdownPanel).Anchor(leftMiddleMinAnchor, leftMiddleMinAnchor);
            dropdown.Update();
        }
    }
}