using CabbyMenu.SyncedReferences;
using CabbyMenu.UI;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.UI.Controls;
using CabbyMenu.UI.Modders;
using UnityEngine;
using UnityEngine.UI;

namespace CabbyCodes.Patches.Settings
{
    /// <summary>
    /// Panel that combines an enable toggle with a button for capturing the quick open hotkey.
    /// </summary>
    public class HotkeyBindingPanel : CheatPanel
    {
        private static readonly Vector2 DefaultSize = CabbyMenu.Constants.DEFAULT_PANEL_SIZE;
        private static readonly Vector2 MiddleAnchor = CabbyMenu.Constants.MIDDLE_ANCHOR_VECTOR;

        private readonly ToggleButton toggleButton;
        private readonly Button bindingButton;
        private readonly TextMod bindingTextMod;
        private readonly ISyncedReference<bool> toggleReference;

        public HotkeyBindingPanel(ISyncedReference<bool> toggleReference, string description)
            : base(description)
        {
            this.toggleReference = toggleReference;

            // Toggle area
            GameObject togglePanel = DefaultControls.CreatePanel(new DefaultControls.Resources());
            togglePanel.name = "Hotkey Toggle Panel";
            new ImageMod(togglePanel.GetComponent<Image>()).SetColor(Color.clear);
            new Fitter(togglePanel).Attach(cheatPanel);
            togglePanel.transform.SetAsFirstSibling();

            LayoutElement toggleLayout = togglePanel.AddComponent<LayoutElement>();
            toggleLayout.flexibleHeight = CabbyMenu.Constants.FLEXIBLE_LAYOUT_VALUE;
            toggleLayout.minWidth = DefaultSize.x;

            toggleButton = new ToggleButton(toggleReference);
            GameObject toggleButtonObject = toggleButton.GetGameObject();
            new Fitter(toggleButtonObject).Attach(togglePanel).Anchor(MiddleAnchor, MiddleAnchor).Size(DefaultSize);

            updateActions.Add(toggleButton.Update);

            // Spacer between description text and binding button
            GameObject spacer = new GameObject("Hotkey Spacer", typeof(RectTransform));
            spacer.transform.SetParent(cheatPanel.transform, false);
            LayoutElement spacerLayout = spacer.AddComponent<LayoutElement>();
            spacerLayout.minWidth = 30f;
            spacerLayout.preferredWidth = 30f;
            spacerLayout.flexibleWidth = 0f;

            // Binding button area
            GameObject bindingPanel = DefaultControls.CreatePanel(new DefaultControls.Resources());
            bindingPanel.name = "Hotkey Binding Panel";
            new ImageMod(bindingPanel.GetComponent<Image>()).SetColor(Color.clear);
            new Fitter(bindingPanel).Attach(cheatPanel);

            LayoutElement bindingLayout = bindingPanel.AddComponent<LayoutElement>();
            bindingLayout.flexibleHeight = CabbyMenu.Constants.FLEXIBLE_LAYOUT_VALUE;
            bindingLayout.flexibleWidth = 0f;

            (GameObject buttonObject, GameObjectMod _, TextMod textMod) = ButtonBuilder.BuildDefault(QuickOpenHotkeyManager.GetBindingDisplay());
            bindingButton = buttonObject.GetComponent<Button>();
            bindingTextMod = textMod;

            float bindingButtonWidth = CabbyMenu.Constants.MIN_PANEL_WIDTH * 2f;
            bindingLayout.minWidth = bindingButtonWidth;
            bindingLayout.preferredWidth = bindingButtonWidth;
            new Fitter(buttonObject).Attach(bindingPanel).Anchor(MiddleAnchor, MiddleAnchor).Size(new Vector2(bindingButtonWidth, CabbyMenu.Constants.DEFAULT_PANEL_HEIGHT));

            bindingButton.onClick.AddListener(OnBindingButtonClicked);

            updateActions.Add(UpdateBindingDisplay);
            QuickOpenHotkeyManager.BindingChanged += HandleBindingChanged;
        }

        private void OnBindingButtonClicked()
        {
            if (QuickOpenHotkeyManager.IsListening())
            {
                QuickOpenHotkeyManager.CancelListening();
            }
            else
            {
                QuickOpenHotkeyManager.BeginListeningForBinding();
            }
        }

        private void UpdateBindingDisplay()
        {
            string displayText = QuickOpenHotkeyManager.GetBindingDisplay();
            bindingTextMod.SetText(displayText);
            bindingButton.interactable = toggleReference.Get();
        }

        private void HandleBindingChanged()
        {
            UpdateBindingDisplay();
        }
    }
}


