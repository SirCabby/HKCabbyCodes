using CabbyMenu.SyncedReferences;
using UnityEngine.UI;
using UnityEngine;
using CabbyMenu.UI.ReferenceControls;

namespace CabbyMenu.UI.CheatPanels
{
    public class DropdownPanel : CheatPanel
    {
        private static readonly Vector2 middle = Constants.MIDDLE_ANCHOR_VECTOR;

        private readonly DropDownSync dropdown;

        public DropdownPanel(ISyncedValueList syncedValueReference, int width, string description) : base(description)
        {
            GameObject dropdownPanel = DefaultControls.CreatePanel(new DefaultControls.Resources());
            dropdownPanel.name = "Dropdown Panel";
            var image = dropdownPanel.GetComponent<Image>();
            if (image != null) image.color = Color.clear;
            new Fitter(dropdownPanel).Attach(cheatPanel);
            dropdownPanel.transform.SetAsFirstSibling();

            LayoutElement dropdownPanelLayout = dropdownPanel.AddComponent<LayoutElement>();
            dropdownPanelLayout.flexibleHeight = Constants.FLEXIBLE_LAYOUT_VALUE;
            dropdownPanelLayout.minWidth = width;

            dropdown = new(syncedValueReference);
            var customDropdown = dropdown.GetGameObject().GetComponent<CustomDropdown>();
            customDropdown.SetOptions(syncedValueReference.GetValueList());
            customDropdown.SetSize(width, Constants.DEFAULT_PANEL_HEIGHT);
            customDropdown.SetFontSize(Constants.DEFAULT_FONT_SIZE);
            customDropdown.SetColors(Constants.DROPDOWN_NORMAL, Constants.DROPDOWN_HOVER, Constants.DROPDOWN_PRESSED);

            new Fitter(dropdown.GetGameObject()).Attach(dropdownPanel).Anchor(middle, middle);
            dropdown.Update();
        }
    }
}