using CabbyMenu.SyncedReferences;
using UnityEngine;
using UnityEngine.UI;
using CabbyMenu.UI.Modders;
using UnityEngine.EventSystems;
using BepInEx.Configuration;
using TMPro;

namespace CabbyCodes.Patches.Flags.Triage
{
    public class FlagMonitorReference : ISyncedReference<bool>
    {
        private static FlagMonitorReference instance;
        public static FlagMonitorReference Instance => instance ?? (instance = new FlagMonitorReference());

        private bool isEnabled = false;
        private static Image backgroundImage;
        private static Scrollbar verticalScrollbar;
        private static Color originalBackgroundColor;
        private static Color originalScrollbarColor;
        private static Color originalScrollbarHandleColor;
        private static bool isHovering = false;

        /// <summary>
        /// Configuration entry for flag monitor enabled state.
        /// </summary>
        private static ConfigEntry<bool> flagMonitorEnabled;

        public bool IsEnabled => isEnabled;

        /// <summary>
        /// Initializes the configuration entries for flag monitor settings.
        /// </summary>
        public static void InitializeConfig()
        {
            flagMonitorEnabled = CabbyCodesPlugin.configFile.Bind("FlagMonitor", "Enabled", false, 
                "Enable real-time flag change notifications on screen");
        }

        /// <summary>
        /// Ensures the notification panel is created if the monitor is enabled.
        /// This should be called after the configuration is loaded to ensure the panel exists.
        /// </summary>
        public static void EnsurePanelExists()
        {
            if (flagMonitorEnabled != null && flagMonitorEnabled.Value && FlagMonitorPatch.notificationPanel == null)
            {
                instance.CreateNotificationPanel();
                instance.Set(true); // This will show the panel and update the display
            }
        }

        public bool Get()
        {
            // Ensure config is initialized
            if (flagMonitorEnabled == null)
            {
                InitializeConfig();
            }
            
            // Synchronize the local field with the config value
            isEnabled = flagMonitorEnabled.Value;
            return isEnabled;
        }

        public void Set(bool newValue)
        {
            // Ensure config is initialized
            if (flagMonitorEnabled == null)
            {
                InitializeConfig();
            }
            
            isEnabled = newValue;
            flagMonitorEnabled.Value = newValue;
            
            // Create panel if it doesn't exist and we're enabling
            if (isEnabled && FlagMonitorPatch.notificationPanel == null)
            {
                CreateNotificationPanel();
            }
            
            if (FlagMonitorPatch.notificationPanel != null)
            {
                // Check if game is paused - hide panel when paused
                bool shouldShow = isEnabled && !IsGamePaused();
                FlagMonitorPatch.notificationPanel.SetActive(shouldShow);
                
                if (isEnabled && shouldShow)
                {
                    // Always update the display to ensure text is visible, even if no notifications yet
                    FlagMonitorPatch.UpdateNotificationDisplay();
                    
                    // Force canvas update to ensure everything is rendered
                    Canvas.ForceUpdateCanvases();
                }
            }
        }

        /// <summary>
        /// Checks if the game is currently paused, using the same logic as GameStateProvider.
        /// </summary>
        /// <returns>True if the game is paused, false otherwise.</returns>
        private static bool IsGamePaused()
        {
            return GameManager._instance != null && GameManager.instance.IsGamePaused();
        }
        
        /// <summary>
        /// Updates the panel visibility based on game pause state.
        /// This should be called regularly to ensure proper visibility.
        /// </summary>
        public static void UpdatePanelVisibility()
        {
            if (FlagMonitorPatch.notificationPanel != null && instance != null && instance.isEnabled)
            {
                bool shouldShow = !IsGamePaused();
                bool wasActive = FlagMonitorPatch.notificationPanel.activeSelf;
                
                FlagMonitorPatch.notificationPanel.SetActive(shouldShow);
                
                // If panel was hidden due to pausing, reset hover state
                if (wasActive && !shouldShow)
                {
                    isHovering = false;
                }
                
                // If panel is being shown again after being hidden, ensure correct transparency
                if (!wasActive && shouldShow)
                {
                    // Force reset hover state and ensure transparency is correct
                    isHovering = false;
                    EnsureBackgroundTransparency();
                }
            }
        }
        
        /// <summary>
        /// Ensures the background transparency is correct based on the current hover state.
        /// This should be called after any UI updates that might affect the background.
        /// </summary>
        public static void EnsureBackgroundTransparency()
        {
            if (backgroundImage != null)
            {
                backgroundImage.color = isHovering ? originalBackgroundColor : new Color(0, 0, 0, 0f);
            }
            
            if (verticalScrollbar != null)
            {
                Image scrollbarImage = verticalScrollbar.GetComponent<Image>();
                if (scrollbarImage != null)
                {
                    scrollbarImage.color = isHovering ? originalScrollbarColor : new Color(1f, 1f, 1f, 0f);
                }
                
                if (verticalScrollbar.handleRect != null)
                {
                    Image handleImage = verticalScrollbar.handleRect.GetComponent<Image>();
                    if (handleImage != null)
                    {
                        handleImage.color = isHovering ? originalScrollbarHandleColor : new Color(0.85f, 0.85f, 0.85f, 0f);
                    }
                }
            }
        }

        public void CreateNotificationPanel()
        {
            if (FlagMonitorPatch.notificationPanel != null) return;

            // Create the main panel
            FlagMonitorPatch.notificationPanel = new GameObject("FlagMonitorPanel");
            Object.DontDestroyOnLoad(FlagMonitorPatch.notificationPanel);
            Canvas canvas = FlagMonitorPatch.notificationPanel.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 999; // Lower than CabbyMainMenu (1000) so menu appears on top when paused
            
            // Add GraphicRaycaster for UI interaction (like CabbyMainMenu)
            FlagMonitorPatch.notificationPanel.AddComponent<GraphicRaycaster>();
            
            // Add a MonoBehaviour component for coroutines
            FlagMonitorPatch.notificationPanel.AddComponent<FlagMonitorMonoBehaviour>();
            
            // Add CanvasScaler for proper scaling
            CanvasScaler scaler = FlagMonitorPatch.notificationPanel.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            
            // Ensure EventSystem exists
            if (Object.FindObjectOfType<EventSystem>() == null)
            {
                GameObject eventSystem = new GameObject("EventSystem");
                eventSystem.AddComponent<EventSystem>();
                eventSystem.AddComponent<StandaloneInputModule>();
            }
            
            // Create background panel
            GameObject backgroundPanel = new GameObject("Background");
            backgroundPanel.transform.SetParent(FlagMonitorPatch.notificationPanel.transform, false);
            
            backgroundImage = backgroundPanel.AddComponent<Image>();
            backgroundImage.color = new Color(0, 0, 0, 0f); // Start fully transparent
            originalBackgroundColor = new Color(0, 0, 0, 0.8f); // Store original semi-transparent color for hover
            
            RectTransform backgroundRect = backgroundPanel.GetComponent<RectTransform>();
            backgroundRect.anchorMin = new Vector2(0, 0.7f);
            backgroundRect.anchorMax = new Vector2(0.455f, 1f);
            backgroundRect.offsetMin = Vector2.zero;
            backgroundRect.offsetMax = Vector2.zero;
            
            // Add hover detection to background panel
            EventTrigger eventTrigger = backgroundPanel.AddComponent<EventTrigger>();

            EventTrigger.Entry enterEntry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerEnter
            };
            enterEntry.callback.AddListener((data) => { OnPanelHoverEnter(); });
            eventTrigger.triggers.Add(enterEntry);

            EventTrigger.Entry exitEntry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerExit
            };
            exitEntry.callback.AddListener((data) => { OnPanelHoverExit(); });
            eventTrigger.triggers.Add(exitEntry);
            
            // Create scroll view using DefaultControls (like CabbyMainMenu)
            GameObject scrollViewObject = DefaultControls.CreateScrollView(new DefaultControls.Resources());
            scrollViewObject.name = "FlagMonitorScrollView";
            scrollViewObject.transform.SetParent(backgroundPanel.transform, false);
            
            // Remove the scroll view's background image to prevent grey background
            Image scrollViewImage = scrollViewObject.GetComponent<Image>();
            if (scrollViewImage != null)
            {
                scrollViewImage.color = new Color(0, 0, 0, 0f); // Transparent
            }
            
            // Configure scroll view positioning
            RectTransform scrollRectTransform = scrollViewObject.GetComponent<RectTransform>();
            scrollRectTransform.anchorMin = Vector2.zero;
            scrollRectTransform.anchorMax = Vector2.one;
            scrollRectTransform.offsetMin = new Vector2(10, 10);
            scrollRectTransform.offsetMax = new Vector2(-10, -10);
            
            // Configure scroll rect (like CabbyMainMenu)
            ScrollRect scrollRect = scrollViewObject.GetComponent<ScrollRect>();
            scrollRect.movementType = ScrollRect.MovementType.Clamped;
            scrollRect.scrollSensitivity = 50f;
            
            // Get the content and configure it (like CabbyMainMenu)
            GameObject viewportObject = scrollViewObject.transform.Find("Viewport").gameObject;
            
            // Keep viewport background semi-transparent for proper text rendering
            Image viewportImage = viewportObject.GetComponent<Image>();
            if (viewportImage != null)
            {
                viewportImage.color = new Color(0, 0, 0, 0.1f); // Very slight opacity for proper rendering
            }
            
            GameObject contentObject = viewportObject.transform.Find("Content").gameObject;
            VerticalLayoutGroup contentLayoutGroup = contentObject.AddComponent<VerticalLayoutGroup>();
            contentLayoutGroup.padding = new RectOffset(10, 10, 10, 10);
            contentLayoutGroup.spacing = 5f;
            contentLayoutGroup.childForceExpandHeight = false;
            contentLayoutGroup.childControlHeight = false;
            
            ContentSizeFitter contentSizeFitter = contentObject.AddComponent<ContentSizeFitter>();
            contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            
            // Configure scrollbar (like CabbyMainMenu)
            Transform scrollbarTransform = scrollViewObject.transform.Find("Scrollbar Vertical");
            if (scrollbarTransform != null)
            {
                verticalScrollbar = scrollbarTransform.GetComponent<Scrollbar>();
                if (verticalScrollbar != null)
                {
                    // Use ScrollBarMod to configure the scrollbar (like CabbyMainMenu)
                    new ScrollBarMod(verticalScrollbar).SetDefaults();
                    
                    // Match dropdown: set background and handle colors
                    Image scrollbarImage = verticalScrollbar.GetComponent<Image>();
                    if (scrollbarImage != null)
                    {
                        scrollbarImage.color = new Color(1f, 1f, 1f, 0f); // Start transparent
                        originalScrollbarColor = new Color(1f, 1f, 1f, 1f); // Store original white color for hover
                    }
                    if (verticalScrollbar.handleRect != null)
                    {
                        Image handleImage = verticalScrollbar.handleRect.GetComponent<Image>();
                        if (handleImage != null)
                        {
                            // Start transparent
                            handleImage.color = new Color(0.85f, 0.85f, 0.85f, 0f);
                            originalScrollbarHandleColor = new Color(0.85f, 0.85f, 0.85f, 1f); // Store original color for hover
                        }
                        // Set Button ColorBlock for hover/pressed/disabled
                        Button handleButton = verticalScrollbar.handleRect.GetComponent<Button>();
                        if (handleButton != null)
                        {
                            ColorBlock cb = handleButton.colors;
                            cb.normalColor = new Color(0.85f, 0.85f, 0.85f, 1f);
                            cb.highlightedColor = new Color(0.3f, 0.6f, 1f, 1f);
                            cb.pressedColor = new Color(0.2f, 0.4f, 0.8f, 1f);
                            cb.disabledColor = new Color(0.5f, 0.5f, 0.5f, 1f);
                            cb.colorMultiplier = 1f;
                            cb.fadeDuration = 0.1f;
                            handleButton.colors = cb;
                        }
                    }
                }
            }
            
            // Create text component
            GameObject textObject = new GameObject("NotificationText");
            textObject.transform.SetParent(contentObject.transform, false);
            
            FlagMonitorPatch.notificationText = textObject.AddComponent<TextMeshProUGUI>();
            FlagMonitorPatch.notificationText.fontSize = 16;
            FlagMonitorPatch.notificationText.fontStyle = FontStyles.Bold;
            FlagMonitorPatch.notificationText.color = Color.white;
            FlagMonitorPatch.notificationText.alignment = TextAlignmentOptions.TopLeft;
            FlagMonitorPatch.notificationText.enableWordWrapping = true;
            FlagMonitorPatch.notificationText.richText = true;
            
            // Configure text RectTransform to ensure visibility
            RectTransform textRect = FlagMonitorPatch.notificationText.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.pivot = new Vector2(0.5f, 0.5f);
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;
            
            // Add ContentSizeFitter to text to expand vertically
            ContentSizeFitter textFitter = textObject.AddComponent<ContentSizeFitter>();
            textFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            textFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            
            // Set initial text to ensure the component is properly initialized
            FlagMonitorPatch.notificationText.text = "<color=#FFFFFF><b>Flag Monitor Active - Total Notifications: 0</b></color>\n\n<color=#CCCCCC>Waiting for flag changes...</color>";
            
            // Force layout rebuild to ensure proper sizing
            LayoutRebuilder.ForceRebuildLayoutImmediate(textRect);
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentObject.GetComponent<RectTransform>());
            
            // Initially hide the panel
            FlagMonitorPatch.notificationPanel.SetActive(false);
        }
        
        private static void OnPanelHoverEnter()
        {
            if (isHovering) return;
            isHovering = true;
            
            // Restore original colors
            if (backgroundImage != null)
            {
                backgroundImage.color = originalBackgroundColor;
            }
            
            if (verticalScrollbar != null)
            {
                Image scrollbarImage = verticalScrollbar.GetComponent<Image>();
                if (scrollbarImage != null)
                {
                    scrollbarImage.color = originalScrollbarColor;
                }
                
                if (verticalScrollbar.handleRect != null)
                {
                    Image handleImage = verticalScrollbar.handleRect.GetComponent<Image>();
                    if (handleImage != null)
                    {
                        handleImage.color = originalScrollbarHandleColor;
                    }
                }
            }
        }
        
        private static void OnPanelHoverExit()
        {
            if (!isHovering) return;
            isHovering = false;
            
            // Make background fully transparent
            if (backgroundImage != null)
            {
                backgroundImage.color = new Color(0, 0, 0, 0f);
            }
            
            // Make scrollbar fully transparent
            if (verticalScrollbar != null)
            {
                Image scrollbarImage = verticalScrollbar.GetComponent<Image>();
                if (scrollbarImage != null)
                {
                    scrollbarImage.color = new Color(1f, 1f, 1f, 0f);
                }
                
                if (verticalScrollbar.handleRect != null)
                {
                    Image handleImage = verticalScrollbar.handleRect.GetComponent<Image>();
                    if (handleImage != null)
                    {
                        handleImage.color = new Color(0.85f, 0.85f, 0.85f, 0f);
                    }
                }
            }
        }
    }
} 