using CabbyMenu.SyncedReferences;
using UnityEngine;
using UnityEngine.UI;
using CabbyMenu.UI.Modders;

namespace CabbyCodes.Patches.Flags.Triage
{
    public class FlagMonitorReference : ISyncedReference<bool>
    {
        private static FlagMonitorReference instance;
        public static FlagMonitorReference Instance => instance ?? (instance = new FlagMonitorReference());

        private bool isEnabled = false;

        public bool IsEnabled => isEnabled;

        public bool Get()
        {
            return isEnabled;
        }

        public void Set(bool newValue)
        {
            isEnabled = newValue;
            
            // Create panel if it doesn't exist and we're enabling
            if (isEnabled && FlagMonitorPatch.notificationPanel == null)
            {
                CreateNotificationPanel();
            }
            
            if (FlagMonitorPatch.notificationPanel != null)
            {
                FlagMonitorPatch.notificationPanel.SetActive(isEnabled);
                
                if (isEnabled)
                {
                    // Update the display to show existing notifications instead of default message
                    FlagMonitorPatch.UpdateNotificationDisplay();
                }
            }
        }

        public void CreateNotificationPanel()
        {
            if (FlagMonitorPatch.notificationPanel != null) return;

            // Create the main panel
            FlagMonitorPatch.notificationPanel = new GameObject("FlagMonitorPanel");
            Canvas canvas = FlagMonitorPatch.notificationPanel.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 1000; // Ensure it's on top
            
            // Add GraphicRaycaster for UI interaction (like CabbyMainMenu)
            FlagMonitorPatch.notificationPanel.AddComponent<GraphicRaycaster>();
            
            // Add a MonoBehaviour component for coroutines
            FlagMonitorPatch.notificationPanel.AddComponent<FlagMonitorMonoBehaviour>();
            
            // Add CanvasScaler for proper scaling
            CanvasScaler scaler = FlagMonitorPatch.notificationPanel.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            
            // Ensure EventSystem exists (like CabbyMainMenu)
            if (UnityEngine.Object.FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
            {
                GameObject eventSystem = new GameObject("EventSystem");
                eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
                eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
            }
            
            // Create background panel
            GameObject backgroundPanel = new GameObject("Background");
            backgroundPanel.transform.SetParent(FlagMonitorPatch.notificationPanel.transform, false);
            
            Image backgroundImage = backgroundPanel.AddComponent<Image>();
            backgroundImage.color = new Color(0, 0, 0, 0.8f); // Semi-transparent black
            
            RectTransform backgroundRect = backgroundPanel.GetComponent<RectTransform>();
            backgroundRect.anchorMin = new Vector2(0, 0.7f);
            backgroundRect.anchorMax = new Vector2(0.4f, 1f);
            backgroundRect.offsetMin = Vector2.zero;
            backgroundRect.offsetMax = Vector2.zero;
            
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
                Scrollbar verticalScrollbar = scrollbarTransform.GetComponent<Scrollbar>();
                if (verticalScrollbar != null)
                {
                    // Use ScrollBarMod to configure the scrollbar (like CabbyMainMenu)
                    new ScrollBarMod(verticalScrollbar).SetDefaults();
                    
                    // Match dropdown: set background and handle colors
                    Image scrollbarImage = verticalScrollbar.GetComponent<Image>();
                    if (scrollbarImage != null)
                    {
                        scrollbarImage.color = Color.white; // White background
                    }
                    if (verticalScrollbar.handleRect != null)
                    {
                        Image handleImage = verticalScrollbar.handleRect.GetComponent<Image>();
                        if (handleImage != null)
                        {
                            // Normal state: very light gray
                            handleImage.color = new Color(0.85f, 0.85f, 0.85f, 1f);
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
            
            FlagMonitorPatch.notificationText = textObject.AddComponent<Text>();
            FlagMonitorPatch.notificationText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            FlagMonitorPatch.notificationText.fontSize = 12;
            FlagMonitorPatch.notificationText.fontStyle = FontStyle.Bold; // Make text bold
            FlagMonitorPatch.notificationText.color = Color.white; // Ensure white color
            FlagMonitorPatch.notificationText.alignment = TextAnchor.UpperLeft;
            FlagMonitorPatch.notificationText.horizontalOverflow = HorizontalWrapMode.Wrap;
            FlagMonitorPatch.notificationText.verticalOverflow = VerticalWrapMode.Overflow;
            
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
            

            
            // Initially hide the panel
            FlagMonitorPatch.notificationPanel.SetActive(false);
        }
    }
} 