using System;
using UnityEngine;
using UnityEngine.UI;
using CabbyMenu.UI.Controls;
using CabbyMenu.UI.Modders;

namespace CabbyMenu.UI.Popups
{
    /// <summary>
    /// Popup that requests user confirmation with Confirm and Cancel buttons.
    /// </summary>
    public class ConfirmationPopup : PopupBase
    {
        private readonly TextMod confirmTextMod;
        private readonly TextMod cancelTextMod;
        private readonly Button confirmButton;
        private readonly Button cancelButton;

        public ConfirmationPopup(
            CabbyMainMenu menu,
            string headerText,
            string messageText,
            string confirmText = "Confirm",
            string cancelText = "Cancel",
            Action onConfirm = null,
            Action onCancel = null)
            : base(menu, headerText, messageText)
        {
            // Container for buttons
            GameObject buttonContainer = DefaultControls.CreatePanel(new DefaultControls.Resources());
            buttonContainer.name = "Button Container";
            new ImageMod(buttonContainer.GetComponent<Image>()).SetColor(Color.clear);
            // Use manual anchoring for left/right buttons instead of layout group
            buttonContainer.transform.SetParent(popupPanel.transform, false);
            var containerLE = buttonContainer.AddComponent<LayoutElement>();
            containerLE.preferredHeight = Constants.DEFAULT_PANEL_HEIGHT + 20;

            // Confirm button (red)
            (GameObject confirmObj, _, TextMod confirmMod) = ButtonBuilder.BuildDanger(confirmText);
            confirmObj.name = "Confirm Button";
            confirmButton = confirmObj.GetComponent<Button>();
            confirmButton.onClick.AddListener(() =>
            {
                onConfirm?.Invoke();
                Destroy();
            });
            confirmTextMod = confirmMod;
            confirmObj.transform.SetParent(buttonContainer.transform, false);
            int confirmWidth = CalculateButtonWidth(confirmText);
            var confirmRect = confirmObj.GetComponent<RectTransform>();
            confirmRect.sizeDelta = new Vector2(confirmWidth, Constants.DEFAULT_PANEL_HEIGHT);
            confirmRect.anchorMin = new Vector2(0f,0.5f);
            confirmRect.anchorMax = new Vector2(0f,0.5f);
            confirmRect.pivot = new Vector2(0f,0.5f);
            confirmRect.anchoredPosition = new Vector2(0f,0f);

            // Cancel button (blue)
            (GameObject cancelObj, _, TextMod cancelMod) = ButtonBuilder.BuildDefault(cancelText);
            cancelObj.name = "Cancel Button";
            cancelButton = cancelObj.GetComponent<Button>();
            cancelButton.onClick.AddListener(() =>
            {
                onCancel?.Invoke();
                Destroy();
            });
            cancelTextMod = cancelMod;
            cancelObj.transform.SetParent(buttonContainer.transform, false);
            int cancelWidth = CalculateButtonWidth(cancelText);
            var cancelRect = cancelObj.GetComponent<RectTransform>();
            cancelRect.sizeDelta = new Vector2(cancelWidth, Constants.DEFAULT_PANEL_HEIGHT);
            cancelRect.anchorMin = new Vector2(1f,0.5f);
            cancelRect.anchorMax = new Vector2(1f,0.5f);
            cancelRect.pivot = new Vector2(1f,0.5f);
            cancelRect.anchoredPosition = new Vector2(0f,0f);
        }

        public void SetConfirmButtonText(string text) => confirmTextMod.SetText(text);
        public void SetCancelButtonText(string text) => cancelTextMod.SetText(text);

        public void SetConfirmAction(Action action)
        {
            confirmButton.onClick.RemoveAllListeners();
            confirmButton.onClick.AddListener(() => { action?.Invoke(); Destroy(); });
        }

        public void SetCancelAction(Action action)
        {
            cancelButton.onClick.RemoveAllListeners();
            cancelButton.onClick.AddListener(() => { action?.Invoke(); Destroy(); });
        }
    }
} 