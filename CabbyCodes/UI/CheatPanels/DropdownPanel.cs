using CabbyCodes.SyncedReferences;
using CabbyCodes.UI.Modders;
using UnityEngine.UI;
using UnityEngine;
using CabbyCodes.UI.ReferenceControls;
using System.Collections.Generic;

namespace CabbyCodes.UI.CheatPanels
{
    public class DropdownPanel : CheatPanel
    {
        private readonly DropDownSync dropdown;

        public DropdownPanel(ISyncedValueList<int, List<string>> syncedValueReference, string description) : base(description)
        {
            int width = 500;

            GameObject dropdownPanel = DefaultControls.CreatePanel(new DefaultControls.Resources());
            dropdownPanel.name = "Dropdown Panel";
            new ImageMod(dropdownPanel.GetComponent<Image>()).SetColor(Color.clear);
            new Fitter(dropdownPanel).Attach(cheatPanel);
            dropdownPanel.transform.SetAsFirstSibling();

            LayoutElement dropdownPanelLayout = dropdownPanel.AddComponent<LayoutElement>();
            dropdownPanelLayout.flexibleHeight = 1;
            dropdownPanelLayout.minWidth = width;

            dropdown = new(syncedValueReference);
            new DropdownMod(dropdown.GetGameObject()).SetOptions(syncedValueReference.GetValueList()).SetSize(new Vector2(width, 60), 10).SetFontSize(36);
            new Fitter(dropdown.GetGameObject()).Attach(dropdownPanel).Anchor(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));
        }

        public DropDownSync GetDropdown()
        {
            return dropdown;
        }
    }
}
