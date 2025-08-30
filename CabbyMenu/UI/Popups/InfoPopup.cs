using System;
using UnityEngine;
using UnityEngine.UI;
using CabbyMenu.UI.Controls;
using CabbyMenu.UI.Modders;

namespace CabbyMenu.UI.Popups
{
    /// <summary>
    /// Popup that displays a message with a single OK button.
    /// </summary>
    public class InfoPopup : PopupBase
    {
        private readonly TextMod okButtonTextMod;
        private readonly Button okButton;

        public InfoPopup(CabbyMainMenu menu, string headerText, string messageText, string okText = "OK", Action onOk = null)
            : base(menu, headerText, messageText, 600f, 400f)
        {
            (GameObject okButtonObj, _, TextMod textMod) = ButtonBuilder.BuildDefault(okText);
            okButtonObj.name = "OK Button";
            okButton = okButtonObj.GetComponent<Button>();
            okButton.onClick.AddListener(() =>
            {
                onOk?.Invoke();
                Destroy();
            });
            okButtonTextMod = textMod;

            okButtonObj.transform.SetParent(popupPanel.transform, false);
            int btnWidth = CalculateButtonWidth(okText);
            var okLE = okButtonObj.AddComponent<LayoutElement>();
            okLE.preferredWidth = btnWidth;
            okLE.preferredHeight = Constants.DEFAULT_PANEL_HEIGHT;

        }

        public void SetOkButtonText(string text) => okButtonTextMod.SetText(text);

        public void SetOkAction(Action action)
        {
            okButton.onClick.RemoveAllListeners();
            okButton.onClick.AddListener(() => { action?.Invoke(); Destroy(); });
        }
    }
} 