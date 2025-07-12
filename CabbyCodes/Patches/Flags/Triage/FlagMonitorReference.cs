using CabbyMenu.SyncedReferences;
using UnityEngine;
using UnityEngine.UI;

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
                    FlagMonitorPatch.notificationText.text = "Flag Monitor Active\n\nWaiting for flag changes...";
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
            
            // Add CanvasScaler for proper scaling
            CanvasScaler scaler = FlagMonitorPatch.notificationPanel.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            
            // Create background panel
            GameObject backgroundPanel = new GameObject("Background");
            backgroundPanel.transform.SetParent(FlagMonitorPatch.notificationPanel.transform, false);
            
            Image backgroundImage = backgroundPanel.AddComponent<Image>();
            backgroundImage.color = new Color(0, 0, 0, 0.8f);
            
            RectTransform backgroundRect = backgroundPanel.GetComponent<RectTransform>();
            backgroundRect.anchorMin = new Vector2(0, 0.7f);
            backgroundRect.anchorMax = new Vector2(0.4f, 1f);
            backgroundRect.offsetMin = Vector2.zero;
            backgroundRect.offsetMax = Vector2.zero;
            
            // Create scroll view for text
            GameObject scrollViewObject = new GameObject("ScrollView");
            scrollViewObject.transform.SetParent(backgroundPanel.transform, false);
            
            ScrollRect scrollRect = scrollViewObject.AddComponent<ScrollRect>();
            RectTransform scrollRectTransform = scrollViewObject.GetComponent<RectTransform>();
            scrollRectTransform.anchorMin = Vector2.zero;
            scrollRectTransform.anchorMax = Vector2.one;
            scrollRectTransform.offsetMin = new Vector2(10, 10);
            scrollRectTransform.offsetMax = new Vector2(-10, -10);
            
            // Create viewport
            GameObject viewportObject = new GameObject("Viewport");
            viewportObject.transform.SetParent(scrollViewObject.transform, false);
            
            RectTransform viewportRect = viewportObject.AddComponent<RectTransform>();
            viewportRect.anchorMin = Vector2.zero;
            viewportRect.anchorMax = Vector2.one;
            viewportRect.offsetMin = Vector2.zero;
            viewportRect.offsetMax = Vector2.zero;
            
            Image viewportImage = viewportObject.AddComponent<Image>();
            viewportImage.color = new Color(0, 0, 0, 0.5f);
            
            Mask viewportMask = viewportObject.AddComponent<Mask>();
            viewportMask.showMaskGraphic = false;
            
            // Create content area
            GameObject contentObject = new GameObject("Content");
            contentObject.transform.SetParent(viewportObject.transform, false);
            
            RectTransform contentRect = contentObject.AddComponent<RectTransform>();
            contentRect.anchorMin = new Vector2(0, 1);
            contentRect.anchorMax = new Vector2(1, 1);
            contentRect.pivot = new Vector2(0.5f, 1);
            contentRect.offsetMin = new Vector2(10, 0);
            contentRect.offsetMax = new Vector2(-10, 0);
            
            // Create text component
            GameObject textObject = new GameObject("NotificationText");
            textObject.transform.SetParent(contentObject.transform, false);
            
            FlagMonitorPatch.notificationText = textObject.AddComponent<Text>();
            FlagMonitorPatch.notificationText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            FlagMonitorPatch.notificationText.fontSize = 12;
            FlagMonitorPatch.notificationText.color = Color.white;
            FlagMonitorPatch.notificationText.alignment = TextAnchor.UpperLeft;
            FlagMonitorPatch.notificationText.horizontalOverflow = HorizontalWrapMode.Wrap;
            FlagMonitorPatch.notificationText.verticalOverflow = VerticalWrapMode.Overflow;
            
            RectTransform textRect = textObject.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;
            
            // Setup scroll rect
            scrollRect.viewport = viewportRect;
            scrollRect.content = contentRect;
            scrollRect.horizontal = false;
            scrollRect.vertical = true;
            scrollRect.scrollSensitivity = 10f;
            
            // Initially hide the panel
            FlagMonitorPatch.notificationPanel.SetActive(false);
        }
    }
} 