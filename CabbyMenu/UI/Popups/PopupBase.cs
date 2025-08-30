using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using CabbyMenu.UI.Modders;

namespace CabbyMenu.UI.Popups
{
    /// <summary>
    /// Base class for popups displayed above all other UI elements.
    /// Provides a header and message area but no buttons.
    /// </summary>
    public class PopupBase
    {
        protected readonly CabbyMenu.UI.CabbyMainMenu menu;
        protected readonly string headerText;
        protected readonly string messageText;
        protected readonly float width;
        protected readonly float height;
        protected readonly bool showHeader;
        protected readonly GameObject popupRoot;
        protected readonly GameObject popupPanel;
        protected readonly TextMod headerTextMod;
        protected readonly TextMod messageTextMod;

        // Static tracking for all open popups
        private static readonly List<PopupBase> openPopups = new List<PopupBase>();

        /// <summary>
        /// Gets all currently open popups.
        /// </summary>
        public static IReadOnlyList<PopupBase> OpenPopups => openPopups.AsReadOnly();

        /// <summary>
        /// Closes all open popups.
        /// </summary>
        public static void CloseAllPopups()
        {
            // Clean up any destroyed popups first
            CleanupDestroyedPopups();
            
            // Create a copy of the list to avoid modification during iteration
            var popupsToClose = openPopups.ToList();
            foreach (PopupBase popup in popupsToClose)
            {
                popup?.Hide();
            }
        }

        /// <summary>
        /// Removes destroyed popups from the open popups list.
        /// </summary>
        private static void CleanupDestroyedPopups()
        {
            openPopups.RemoveAll(popup => popup == null || popup.popupRoot == null);
        }

        /// <summary>
        /// Clears all popups from the tracking list. Use when completely resetting the menu.
        /// </summary>
        public static void ClearAllPopups()
        {
            openPopups.Clear();
        }

        /// <summary>
        /// Checks if the mouse position is over any popup component.
        /// </summary>
        /// <param name="mousePosition">The mouse position in screen coordinates.</param>
        /// <returns>True if the mouse is over any popup, false otherwise.</returns>
        public static bool IsMouseOverAnyPopup(Vector2 mousePosition)
        {
            // Clean up any destroyed popups first
            CleanupDestroyedPopups();
            
            foreach (PopupBase popup in openPopups)
            {
                if (popup != null && popup.IsMouseOverPopup(mousePosition))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if the mouse position is over any popup's interactive content (excluding background).
        /// This is more specific and only returns true for actual popup content like buttons.
        /// </summary>
        /// <param name="mousePosition">The mouse position in screen coordinates.</param>
        /// <returns>True if the mouse is over any popup content, false otherwise.</returns>
        public static bool IsMouseOverAnyPopupContent(Vector2 mousePosition)
        {
            // Clean up any destroyed popups first
            CleanupDestroyedPopups();
            
            foreach (PopupBase popup in openPopups)
            {
                if (popup != null && popup.IsMouseOverPopupContent(mousePosition))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if the mouse position is over this popup's components.
        /// </summary>
        /// <param name="mousePosition">The mouse position in screen coordinates.</param>
        /// <returns>True if the mouse is over this popup, false otherwise.</returns>
        public bool IsMouseOverPopup(Vector2 mousePosition)
        {
            if (popupRoot == null) return false;

            // Check popup panel (the main content area)
            if (popupPanel != null && IsMouseOverGameObject(popupPanel, mousePosition))
            {
                return true;
            }

            // Check all children of the popup panel recursively (buttons, text, etc.)
            if (popupPanel != null && IsMouseOverChildren(popupPanel, mousePosition))
            {
                return true;
            }

            // Don't count the background dimmed area as "over the popup"
            // This allows clicks on the background to close the popup
            return false;
        }

        /// <summary>
        /// Checks if the mouse position is over the popup's interactive content (excluding background).
        /// This is more specific than IsMouseOverPopup and only returns true for actual content.
        /// </summary>
        /// <param name="mousePosition">The mouse position in screen coordinates.</param>
        /// <returns>True if the mouse is over interactive popup content, false otherwise.</returns>
        public bool IsMouseOverPopupContent(Vector2 mousePosition)
        {
            if (popupRoot == null || popupPanel == null) return false;

            // Check popup panel (the main content area)
            if (IsMouseOverGameObject(popupPanel, mousePosition))
            {
                return true;
            }

            // Check all children of the popup panel recursively (buttons, text, etc.)
            return IsMouseOverChildren(popupPanel, mousePosition);
        }

        /// <summary>
        /// Recursively checks if the mouse is over any children of the specified GameObject.
        /// </summary>
        /// <param name="gameObject">The GameObject to check children of</param>
        /// <param name="mousePosition">The mouse position in screen coordinates</param>
        /// <returns>True if the mouse is over any child, false otherwise</returns>
        private bool IsMouseOverChildren(GameObject gameObject, Vector2 mousePosition)
        {
            if (gameObject == null) return false;

            // Check all children recursively
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                Transform child = gameObject.transform.GetChild(i);
                if (child == null) continue;

                // Check if mouse is over this child
                if (IsMouseOverGameObject(child.gameObject, mousePosition))
                {
                    return true;
                }

                // Recursively check children of this child
                if (IsMouseOverChildren(child.gameObject, mousePosition))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Helper method to check if mouse is over a specific GameObject.
        /// </summary>
        /// <param name="gameObject">The GameObject to check.</param>
        /// <param name="mousePosition">The mouse position in screen coordinates.</param>
        /// <returns>True if the mouse is over the GameObject, false otherwise.</returns>
        private bool IsMouseOverGameObject(GameObject gameObject, Vector2 mousePosition)
        {
            if (gameObject == null) return false;

            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            if (rectTransform == null) return false;

            return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, mousePosition);
        }

        /// <summary>
        /// Creates a new popup with the specified parameters.
        /// </summary>
        /// <param name="menu">The menu to attach the popup to.</param>
        /// <param name="headerText">The header text to display.</param>
        /// <param name="messageText">The message text to display.</param>
        /// <param name="width">The width of the popup.</param>
        /// <param name="height">The height of the popup.</param>
        /// <param name="showHeader">Whether to show the header (default: true).</param>
        public PopupBase(CabbyMenu.UI.CabbyMainMenu menu, string headerText, string messageText, float width, float height, bool showHeader = true)
        {
            this.menu = menu;
            this.headerText = headerText;
            this.messageText = messageText;
            this.width = width;
            this.height = height;
            this.showHeader = showHeader;

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
            // Use flexible sizing instead of fixed size to allow ContentSizeFitter to work
            new Fitter(popupPanel).Attach(popupRoot).Anchor(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));

            VerticalLayoutGroup verticalLayout = popupPanel.AddComponent<VerticalLayoutGroup>();
            verticalLayout.padding = new RectOffset(Constants.CHEAT_PANEL_PADDING, Constants.CHEAT_PANEL_PADDING, Constants.CHEAT_PANEL_PADDING, Constants.CHEAT_PANEL_PADDING);
            verticalLayout.spacing = Constants.CHEAT_PANEL_SPACING;
            verticalLayout.childAlignment = TextAnchor.UpperCenter;
            verticalLayout.childControlWidth = true;
            verticalLayout.childControlHeight = true; // allow children to control their height
            verticalLayout.childForceExpandWidth = true;
            verticalLayout.childForceExpandHeight = false;

            // Header container with colored background
            if (showHeader)
            {
                GameObject headerContainer = DefaultControls.CreatePanel(new DefaultControls.Resources());
                headerContainer.name = "Header Container";
                new ImageMod(headerContainer.GetComponent<Image>()).SetColor(Constants.BUTTON_PRIMARY_NORMAL);
                headerContainer.transform.SetParent(popupPanel.transform, false);
                LayoutElement headerContainerLE = headerContainer.AddComponent<LayoutElement>();
                headerContainerLE.preferredHeight = 80f;
                headerContainerLE.minHeight = 60f; // Allow header to shrink if needed
                headerContainerLE.flexibleHeight = 0f; // Don't expand header

                // Header text.
                var (gameObject, gameObjectMod, textMod) = TextMod.Build(headerText);
                GameObject headerObj = gameObject;
                headerTextMod = textMod;
                headerTextMod.SetFontStyle(FontStyle.Bold).SetFontSize(Constants.DEFAULT_FONT_SIZE).SetColor(Color.white).SetAlignment(TextAnchor.MiddleLeft);
                // Adjust padding inside header
                RectTransform headerRect = headerObj.GetComponent<RectTransform>();
                headerRect.offsetMin = new Vector2(40f, headerRect.offsetMin.y);
                headerRect.offsetMax = new Vector2(-20f, headerRect.offsetMax.y);
                new Fitter(headerObj).Attach(headerContainer).Anchor(Vector2.zero, Vector2.one).Size(Vector2.zero);
            }
            else
            {
                // For no-header popups, set headerTextMod to null
                headerTextMod = null;
            }

            // Message text.
            var messageFactory = TextMod.Build(messageText);
            GameObject messageObj = messageFactory.gameObject;
            messageTextMod = messageFactory.textMod;
            messageTextMod.SetFontSize(Constants.DEFAULT_FONT_SIZE).SetAlignment(TextAnchor.UpperCenter).SetColor(Color.white);
            new Fitter(messageObj).Attach(popupPanel).Anchor(Vector2.zero, Vector2.one).Size(Vector2.zero);
            
            // Configure message text for proper vertical sizing and word wrapping
            var messageTextComponent = messageObj.GetComponent<Text>();
            if (messageTextComponent != null)
            {
                // Enable word wrapping
                messageTextComponent.horizontalOverflow = HorizontalWrapMode.Wrap;
                messageTextComponent.verticalOverflow = VerticalWrapMode.Overflow;
                // Set alignment to top-center for better word wrapping appearance
                messageTextComponent.alignment = TextAnchor.UpperCenter;
            }
            
            ContentSizeFitter msgFitter = messageObj.AddComponent<ContentSizeFitter>();
            msgFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            msgFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            
            LayoutElement messageLayout = messageObj.AddComponent<LayoutElement>();
            messageLayout.minHeight = 50f; // Reduced from 150f to prevent layout issues
            messageLayout.preferredHeight = -1f; // let fitter decide
            messageLayout.flexibleHeight = 1f; // Allow expansion
            messageLayout.flexibleWidth = 1f; // Allow horizontal expansion

            // Add ContentSizeFitter to the main popup panel for dynamic sizing
            ContentSizeFitter panelFitter = popupPanel.AddComponent<ContentSizeFitter>();
            panelFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            panelFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            // Store initial dimensions for reference
            initialWidth = width;
            initialHeight = height;

            // Set initial size using LayoutElement to work with ContentSizeFitter
            var initialPanelLayout = popupPanel.GetComponent<LayoutElement>();
            if (initialPanelLayout != null)
            {
                initialPanelLayout.preferredWidth = width;
                initialPanelLayout.preferredHeight = height;
            }

            // Auto-resize the popup based on content after everything is set up
            if (showHeader)
            {
                AutoResizeToContent();
            }
            else
            {
                // For loading popups, force the exact size without auto-resizing
                var loadingPanelLayout = popupPanel.GetComponent<LayoutElement>();
                if (loadingPanelLayout != null)
                {
                    loadingPanelLayout.preferredWidth = width;
                    loadingPanelLayout.preferredHeight = height;
                    // Disable flexible sizing for loading popups
                    loadingPanelLayout.flexibleWidth = 0f;
                    loadingPanelLayout.flexibleHeight = 0f;
                }
                
                // Also set the RectTransform size directly for loading popups
                var rectTransform = popupPanel.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    rectTransform.sizeDelta = new Vector2(width, height);
                }
            }
        }

        // Store initial dimensions for reference
        private readonly float initialWidth;
        private readonly float initialHeight;

        public void SetHeaderText(string text) => headerTextMod?.SetText(text);
        public void SetMessageText(string text) 
        { 
            messageTextMod?.SetText(text);
            
            // Update text width constraints to ensure proper word wrapping
            if (popupPanel != null)
            {
                var panelLayout = popupPanel.GetComponent<LayoutElement>();
                if (panelLayout != null)
                {
                    UpdateTextWidthConstraints(panelLayout.preferredWidth);
                }
            }
            
            // Trigger a layout update after changing text to ensure proper sizing
            ForceLayoutUpdate();
        }

        public virtual void Show() 
        { 
            popupRoot.SetActive(true);
            
            // Add to open popups list when showing
            if (!openPopups.Contains(this))
            {
                openPopups.Add(this);
            }
            
            // Force layout update after showing to ensure proper sizing
            ForceLayoutUpdate();
        }
        
        public virtual void Hide() 
        { 
            popupRoot.SetActive(false);
            
            // Remove from open popups list when hiding
            openPopups.Remove(this);
        }

        public void Destroy() 
        { 
            // Remove from open popups list before destroying
            openPopups.Remove(this);
            UnityEngine.Object.Destroy(popupRoot);
        }

        /// <summary>
        /// Forces a layout update to ensure the popup is properly sized
        /// </summary>
        public void ForceLayoutUpdate()
        {
            if (popupPanel != null)
            {
                // Force the layout system to recalculate
                LayoutRebuilder.ForceRebuildLayoutImmediate(popupPanel.GetComponent<RectTransform>());
                
                // Also force updates on child elements
                var headerContainer = popupPanel.transform.Find("Header Container");
                if (headerContainer != null)
                {
                    LayoutRebuilder.ForceRebuildLayoutImmediate(headerContainer.GetComponent<RectTransform>());
                }
                
                Canvas.ForceUpdateCanvases();
            }
        }

        /// <summary>
        /// Manually triggers a content-based resize
        /// </summary>
        public void TriggerContentResize()
        {
            AutoResizeToContent();
        }

        /// <summary>
        /// Manually sets the popup size (useful for overriding dynamic sizing)
        /// </summary>
        /// <param name="width">Width in pixels</param>
        /// <param name="height">Height in pixels</param>
        public void SetSize(float width, float height)
        {
            if (popupPanel != null)
            {
                var rectTransform = popupPanel.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    rectTransform.sizeDelta = new Vector2(width, height);
                    ForceLayoutUpdate();
                }
            }
        }

        /// <summary>
        /// Resets the popup to its initial size
        /// </summary>
        public void ResetToInitialSize()
        {
            SetSize(initialWidth, initialHeight);
        }

        /// <summary>
        /// Sets the background color of the main popup panel
        /// </summary>
        /// <param name="color">The color to set for the popup panel background</param>
        public void SetPanelBackgroundColor(Color color)
        {
            if (popupPanel != null)
            {
                var image = popupPanel.GetComponent<Image>();
                if (image != null)
                {
                    image.color = color;
                }
            }
        }

        /// <summary>
        /// Sets the message text to bold
        /// </summary>
        public void SetMessageBold()
        {
            messageTextMod?.SetFontStyle(FontStyle.Bold);
        }

        /// <summary>
        /// Sets the message text to normal (not bold)
        /// </summary>
        public void SetMessageNormal()
        {
            messageTextMod?.SetFontStyle(FontStyle.Normal);
        }

        /// <summary>
        /// Sets the message text color
        /// </summary>
        /// <param name="color">The color to set for the message text</param>
        public void SetMessageColor(Color color)
        {
            messageTextMod?.SetColor(color);
        }

        /// <summary>
        /// Sets the header text color
        /// </summary>
        /// <param name="color">The color to set for the header text</param>
        public void SetHeaderColor(Color color)
        {
            headerTextMod?.SetColor(color);
        }

        /// <summary>
        /// Calculates the optimal popup size based on text content
        /// </summary>
        /// <param name="text">The text to measure</param>
        /// <param name="maxWidth">Maximum allowed width</param>
        /// <returns>Vector2 with optimal width and height</returns>
        public Vector2 CalculateOptimalSize(string text, float maxWidth = 800f)
        {
            if (string.IsNullOrEmpty(text)) return new Vector2(initialWidth, initialHeight);

            // Estimate text dimensions based on font size and character count
            float charWidth = Constants.DEFAULT_FONT_SIZE * 0.6f; // Approximate character width
            float lineHeight = Constants.DEFAULT_FONT_SIZE * 1.2f; // Approximate line height
            
            // Calculate available width for text (accounting for padding and margins)
            float availableWidth = maxWidth - 80f; // 80f for padding
            
            // Calculate how many characters fit per line
            int charsPerLine = Mathf.FloorToInt(availableWidth / charWidth);
            if (charsPerLine <= 0) charsPerLine = 1;
            
            // Calculate how many lines the text will take with word wrapping
            int lineCount = Mathf.CeilToInt((float)text.Length / charsPerLine);
            
            // Add extra height for multi-line text to account for word wrapping and spacing
            float extraHeight = 0f;
            if (lineCount > 1)
            {
                extraHeight = (lineCount - 1) * (lineHeight * 0.3f); // Reduced spacing for word wrapping
            }
            
            // Calculate optimal dimensions
            float optimalWidth = Mathf.Min(maxWidth, Mathf.Max(400f, availableWidth + 80f)); // Ensure minimum width
            float optimalHeight = Mathf.Max(300f, lineCount * lineHeight + extraHeight + 200f); // 200f for header and padding
            
            // Ensure height is sufficient for the content
            optimalHeight = Mathf.Max(optimalHeight, 300f + (lineCount * lineHeight));
            
            // Add extra height for buttons and proper spacing
            optimalHeight += 100f; // Space for buttons and margins
            
            return new Vector2(optimalWidth, optimalHeight);
        }

        /// <summary>
        /// Automatically resizes the popup to fit its content
        /// </summary>
        public void AutoResizeToContent()
        {
            if (messageTextMod != null)
            {
                string currentText = messageTextMod.Get().text;
                
                // For loading popups (no header), use the initial size instead of calculating optimal size
                Vector2 optimalSize;
                if (!showHeader)
                {
                    // Use the initial requested size for loading popups
                    optimalSize = new Vector2(initialWidth, initialHeight);
                }
                else
                {
                    // Calculate optimal size for popups with headers
                    optimalSize = CalculateOptimalSize(currentText);
                }
                
                // Set the size using LayoutElement instead of directly setting sizeDelta
                var panelLayout = popupPanel.GetComponent<LayoutElement>();
                if (panelLayout != null)
                {
                    panelLayout.preferredWidth = optimalSize.x;
                    panelLayout.preferredHeight = optimalSize.y;
                }
                
                // Also ensure the message text can expand properly
                var messageLayout = messageTextMod.Get().GetComponent<LayoutElement>();
                if (messageLayout != null)
                {
                    // Allow the message to determine its own height
                    messageLayout.preferredHeight = -1f;
                    messageLayout.flexibleHeight = 1f;
                }
                
                // Update text width constraints for proper word wrapping
                UpdateTextWidthConstraints(optimalSize.x);
                
                // Force layout update to apply the new size
                ForceLayoutUpdate();
                
                // Additional force update after a frame to ensure proper sizing
                CabbyMenu.Utilities.CoroutineRunner.RunNextFrame(() => {
                    ForceLayoutUpdate();
                });
            }
        }

        /// <summary>
        /// Updates the text width constraints to ensure proper word wrapping
        /// </summary>
        /// <param name="popupWidth">The width of the popup panel</param>
        private void UpdateTextWidthConstraints(float popupWidth)
        {
            if (messageTextMod != null)
            {
                var messageObj = messageTextMod.Get().gameObject;
                var messageLayout = messageObj.GetComponent<LayoutElement>();
                var messageText = messageObj.GetComponent<Text>();
                
                if (messageLayout != null && messageText != null)
                {
                    // Calculate available width for text (accounting for padding)
                    float availableWidth = popupWidth - 80f; // 80f for padding
                    
                    // Set the preferred width to ensure text wraps properly
                    messageLayout.preferredWidth = availableWidth;
                    messageLayout.minWidth = availableWidth * 0.8f; // Allow some flexibility
                    
                    // Ensure the text component respects the width
                    messageText.horizontalOverflow = HorizontalWrapMode.Wrap;
                }
            }
        }

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