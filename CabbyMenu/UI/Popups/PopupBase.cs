using System;
using UnityEngine;
using UnityEngine.UI;
using CabbyMenu;
using CabbyMenu.UI.Modders;
using CabbyMenu.UI.Controls;
using CabbyMenu.UI;

namespace CabbyMenu.UI.Popups
{
    /// <summary>
    /// Abstract base class for popups displayed above all other UI elements.
    /// Provides a header and message area but no buttons.
    /// </summary>
    public abstract class PopupBase
    {
        protected readonly GameObject popupRoot;
        protected readonly GameObject popupPanel;
        protected readonly TextMod headerTextMod;
        protected readonly TextMod messageTextMod;

        protected PopupBase(CabbyMainMenu menu, string headerText, string messageText, float width = 600f, float height = 400f)
        {
            if (menu == null) throw new ArgumentNullException(nameof(menu));
            GameObject parent = menu.GetRootGameObject();
            if (parent == null) throw new InvalidOperationException("CabbyMainMenu has no root GameObject.");

            // Overlay root that contains its own canvas so we can override sorting order.
            popupRoot = new GameObject("Popup Overlay");
            popupRoot.transform.SetParent(parent.transform, false);

            Canvas canvas = popupRoot.AddComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = 1000; // Ensure popup appears on top of all UI.
            popupRoot.AddComponent<GraphicRaycaster>();

            CanvasScaler scaler = popupRoot.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(Constants.REFERENCE_RESOLUTION_WIDTH, Constants.REFERENCE_RESOLUTION_HEIGHT);

            // Dimmed background that blocks input underneath.
            GameObject background = DefaultControls.CreatePanel(new DefaultControls.Resources());
            background.name = "Popup Background";
            new ImageMod(background.GetComponent<Image>()).SetColor(new Color(0f, 0f, 0f, 0.6f));
            new Fitter(background).Attach(popupRoot).Anchor(Vector2.zero, Vector2.one).Size(Vector2.zero);

            // Main popup panel.
            popupPanel = DefaultControls.CreatePanel(new DefaultControls.Resources());
            popupPanel.name = "Popup Panel";
            new ImageMod(popupPanel.GetComponent<Image>()).SetColor(new Color(0f,0f,0f,1f));
            new Fitter(popupPanel).Attach(popupRoot).Anchor(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f)).Size(new Vector2(width, height));

            VerticalLayoutGroup verticalLayout = popupPanel.AddComponent<VerticalLayoutGroup>();
            verticalLayout.padding = new RectOffset(Constants.CHEAT_PANEL_PADDING, Constants.CHEAT_PANEL_PADDING, Constants.CHEAT_PANEL_PADDING, Constants.CHEAT_PANEL_PADDING);
            verticalLayout.spacing = Constants.CHEAT_PANEL_SPACING;
            verticalLayout.childAlignment = TextAnchor.UpperCenter;
            verticalLayout.childControlWidth = true;
            verticalLayout.childControlHeight = true; // allow children to control their height
            verticalLayout.childForceExpandWidth = true;
            verticalLayout.childForceExpandHeight = false;

            // Header container with colored background
            GameObject headerContainer = DefaultControls.CreatePanel(new DefaultControls.Resources());
            headerContainer.name = "Header Container";
            new ImageMod(headerContainer.GetComponent<Image>()).SetColor(Constants.BUTTON_PRIMARY_NORMAL);
            headerContainer.transform.SetParent(popupPanel.transform, false);
            LayoutElement headerContainerLE = headerContainer.AddComponent<LayoutElement>();
            headerContainerLE.preferredHeight = 80f;

            // Header text.
            var headerFactory = TextMod.Build(headerText);
            GameObject headerObj = headerFactory.gameObject;
            headerTextMod = headerFactory.textMod;
            headerTextMod.SetFontStyle(FontStyle.Bold).SetFontSize(Constants.DEFAULT_FONT_SIZE).SetColor(Color.white).SetAlignment(TextAnchor.MiddleLeft);
            // Adjust padding inside header
            RectTransform headerRect = headerObj.GetComponent<RectTransform>();
            headerRect.offsetMin = new Vector2(40f, headerRect.offsetMin.y);
            headerRect.offsetMax = new Vector2(-20f, headerRect.offsetMax.y);
            new Fitter(headerObj).Attach(headerContainer).Anchor(Vector2.zero, Vector2.one).Size(Vector2.zero);

            // Message text.
            var messageFactory = TextMod.Build(messageText);
            GameObject messageObj = messageFactory.gameObject;
            messageTextMod = messageFactory.textMod;
            messageTextMod.SetFontSize(Constants.DEFAULT_FONT_SIZE).SetAlignment(TextAnchor.UpperCenter).SetColor(Color.white);
            new Fitter(messageObj).Attach(popupPanel).Anchor(Vector2.zero, Vector2.one).Size(Vector2.zero);
            // Allow text to determine its own height but have a minimum
            ContentSizeFitter msgFitter = messageObj.AddComponent<ContentSizeFitter>();
            msgFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            LayoutElement messageLayout = messageObj.AddComponent<LayoutElement>();
            messageLayout.minHeight = 150f;
            messageLayout.preferredHeight = -1f; // let fitter decide
            messageLayout.flexibleHeight = 1;
        }

        public void SetHeaderText(string text) => headerTextMod.SetText(text);
        public void SetMessageText(string text) => messageTextMod.SetText(text);

        public virtual void Show() => popupRoot.SetActive(true);
        public virtual void Hide() => popupRoot.SetActive(false);

        public void Destroy() => UnityEngine.Object.Destroy(popupRoot);

        /// <summary>
        /// Calculates button width based on text length similar to ButtonPanel logic.
        /// </summary>
        /// <param name="text">The button text.</param>
        /// <param name="minWidth">Minimum width in pixels (default: 160).</param>
        /// <returns>The calculated width in pixels.</returns>
        protected static int CalculateButtonWidth(string text, int minWidth = 160)
        {
            if (string.IsNullOrEmpty(text)) return minWidth;
            float estimatedCharWidth = Constants.DEFAULT_FONT_SIZE * 0.45f;
            float width = text.Length * estimatedCharWidth + 40f; // padding
            return Mathf.RoundToInt(Mathf.Max(minWidth, width));
        }
    }
} 