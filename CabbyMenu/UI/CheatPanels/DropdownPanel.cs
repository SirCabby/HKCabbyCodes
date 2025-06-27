using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.Modders;
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
            new ImageMod(dropdownPanel.GetComponent<Image>()).SetColor(Color.clear);
            new Fitter(dropdownPanel).Attach(cheatPanel);
            dropdownPanel.transform.SetAsFirstSibling();

            LayoutElement dropdownPanelLayout = dropdownPanel.AddComponent<LayoutElement>();
            dropdownPanelLayout.flexibleHeight = Constants.FLEXIBLE_LAYOUT_VALUE;
            dropdownPanelLayout.minWidth = width;

            dropdown = new(syncedValueReference);
            new DropdownMod(dropdown.GetGameObject()).SetOptions(syncedValueReference.GetValueList()).SetSize(new Vector2(width, Constants.DEFAULT_PANEL_HEIGHT), Constants.DROPDOWN_SHOW_SIZE).SetFontSize(Constants.DEFAULT_FONT_SIZE);
            new Fitter(dropdown.GetGameObject()).Attach(dropdownPanel).Anchor(middle, middle);
            dropdown.Update();
        }
    }
}