using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.UI.Controls;
using CabbyMenu.UI.Controls.CustomDropdown;
using CabbyMenu.UI.Modders;
using CabbyMenu.UI;
using CabbyMenu.Utilities;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace CabbyCodes.Patches.Settings
{
    /// <summary>
    /// Custom panel that combines a toggle button with a dropdown for quick load settings.
    /// </summary>
    public class QuickLoadPanel : CheatPanel
    {
        private static readonly Vector2 defaultSize = CabbyMenu.Constants.DEFAULT_PANEL_SIZE;
        private static readonly Vector2 middle = CabbyMenu.Constants.MIDDLE_ANCHOR_VECTOR;

        private readonly ToggleButton toggleButton;
        private readonly DropDownSync dropdownSync;
        private readonly GameObject dropdownPanel;
        private readonly ISyncedReference<bool> toggleReference;

        public QuickLoadPanel(ISyncedReference<bool> toggleReference, ISyncedReference<int> inputReference, string description) : base(description)
        {
            this.toggleReference = toggleReference;

            // Create toggle button panel
            GameObject togglePanel = DefaultControls.CreatePanel(new DefaultControls.Resources());
            togglePanel.name = "Toggle Button Panel";
            new ImageMod(togglePanel.GetComponent<Image>()).SetColor(Color.clear);
            new Fitter(togglePanel).Attach(cheatPanel);
            togglePanel.transform.SetAsFirstSibling();

            LayoutElement togglePanelLayout = togglePanel.AddComponent<LayoutElement>();
            togglePanelLayout.flexibleHeight = CabbyMenu.Constants.FLEXIBLE_LAYOUT_VALUE;
            togglePanelLayout.minWidth = defaultSize.x;

            toggleButton = new ToggleButton(toggleReference);
            new Fitter(toggleButton.GetGameObject()).Attach(togglePanel).Anchor(middle, middle).Size(defaultSize);
            updateActions.Add(toggleButton.Update);

            // Subscribe to toggle value change to update dropdown interactable state immediately
            var unityButton = toggleButton.GetGameObject().GetComponent<Button>();
            unityButton?.onClick.AddListener(() => {
                    bool newValue = toggleReference.Get();
                    UpdateDropdownInteractable(newValue);
                });

            // Create dropdown panel
            dropdownPanel = DefaultControls.CreatePanel(new DefaultControls.Resources());
            dropdownPanel.name = "Dropdown Panel";
            new ImageMod(dropdownPanel.GetComponent<Image>()).SetColor(Color.clear);
            new Fitter(dropdownPanel).Attach(cheatPanel);

            LayoutElement dropdownPanelLayout = dropdownPanel.AddComponent<LayoutElement>();
            dropdownPanelLayout.flexibleHeight = CabbyMenu.Constants.FLEXIBLE_LAYOUT_VALUE;
            // Let the dropdown determine its own width dynamically
            dropdownPanelLayout.flexibleWidth = 0f;
            dropdownPanelLayout.minWidth = CabbyMenu.Constants.MIN_PANEL_WIDTH;

            // Create wrapper to convert between dropdown index and slot number
            var slotWrapper = new SaveSlotDropdownWrapper(inputReference);

            // Create dropdown for save slot selection
            var (dropDownSync, dropdownGameObject) = DropDownSync.Create(slotWrapper);
            dropdownSync = dropDownSync;
            
            // Enable dynamic sizing and set options
            dropdownSync.SetDynamicSizing(true);
            List<string> slotOptions = new List<string> { "Slot 1", "Slot 2", "Slot 3", "Slot 4" };
            dropdownSync.SetOptions(slotOptions);
            
            // Set height only (width will be calculated dynamically)
            dropdownSync.SetSize(0f, CabbyMenu.Constants.DEFAULT_PANEL_HEIGHT);
            var customDropdown = dropdownSync.GetCustomDropdown();
            customDropdown.SetFontSize(CabbyMenu.Constants.DEFAULT_FONT_SIZE);
            customDropdown.SetColors(CabbyMenu.Constants.DROPDOWN_NORMAL, CabbyMenu.Constants.DROPDOWN_HOVER, CabbyMenu.Constants.DROPDOWN_PRESSED);
            customDropdown.SetOptionColors(CabbyMenu.Constants.DROPDOWN_OPTION_BACKGROUND, CabbyMenu.Constants.DROPDOWN_OPTION_HOVER, CabbyMenu.Constants.DROPDOWN_OPTION_PRESSED);

            new Fitter(dropdownGameObject).Attach(dropdownPanel).Anchor(middle, middle);
            dropdownSync.Update();
            
            // Force width recalculation to ensure dynamic sizing is applied
            customDropdown.ForceWidthRecalculation();
            
            // Update the dropdown panel width to match the dynamically calculated dropdown width
            UpdateDropdownPanelWidth();
            
            updateActions.Add(dropdownSync.Update);
            updateActions.Add(() => UpdateDropdownInteractable());

            // Initialize the dropdown's interactable state
            UpdateDropdownInteractable();

        }

        public ToggleButton GetToggleButton()
        {
            return toggleButton;
        }

        public Toggle GetToggle()
        {
            return toggleButton.GetGameObject().GetComponent<Toggle>();
        }

        public DropDownSync GetDropdown()
        {
            return dropdownSync;
        }

        /// <summary>
        /// Updates the dropdown's interactable state based on the toggle state.
        /// </summary>
        private void UpdateDropdownInteractable(bool? overrideValue = null)
        {
            bool isEnabled = overrideValue ?? toggleReference.Get();
            var customDropdown = dropdownSync.GetCustomDropdown();
            customDropdown?.SetInteractable(isEnabled);
        }

        /// <summary>
        /// Updates the dropdown panel width to match the dynamically calculated dropdown width.
        /// </summary>
        private void UpdateDropdownPanelWidth()
        {
            var customDropdown = dropdownSync.GetCustomDropdown();
            if (customDropdown != null)
            {
                // Get the actual width of the dropdown button
                RectTransform dropdownRect = customDropdown.GetComponent<RectTransform>();
                if (dropdownRect != null)
                {
                    float dropdownWidth = dropdownRect.sizeDelta.x;
                    
                    // Ensure we have a reasonable width (at least the minimum)
                    if (dropdownWidth < CabbyMenu.Constants.MIN_PANEL_WIDTH)
                    {
                        dropdownWidth = CabbyMenu.Constants.MIN_PANEL_WIDTH;
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