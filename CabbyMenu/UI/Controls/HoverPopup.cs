using UnityEngine;
using UnityEngine.UI;

namespace CabbyMenu.UI.Controls
{
    /// <summary>
    /// A reusable hover popup component that can be attached to any UI element to show hover messages.
    /// </summary>
    public class HoverPopup : MonoBehaviour
    {
        [Header("Hover Popup Settings")]
        [SerializeField] private GameObject popupBackground;
        [SerializeField] private Text popupText;
        [SerializeField] private Canvas popupCanvas;
        [SerializeField] private bool autoCreateComponents = true;

        private bool isVisible = false;
        private string currentMessage = "";
        private Vector3 lastMousePosition;

        // Events
        public System.Action<string> OnShowPopup;
        public System.Action OnHidePopup;

        private void Awake()
        {
            if (autoCreateComponents)
            {
                CreatePopupComponents();
            }
        }

        private void Start()
        {
            // Initially hide the popup
            if (popupBackground != null)
            {
                popupBackground.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            // Clean up when destroyed
            if (popupBackground != null)
            {
                DestroyImmediate(popupBackground);
            }
        }

        /// <summary>
        /// Creates the popup GameObject and components if they don't exist.
        /// </summary>
        private void CreatePopupComponents()
        {
            if (popupBackground != null) return;

            // Create popup GameObject - always attach to the root canvas to ensure proper z-order
            Canvas rootCanvas = GetComponentInParent<Canvas>();
            if (rootCanvas == null)
            {
                // Fallback to root transform if no canvas found
                popupBackground = new GameObject("HoverPopup");
                popupBackground.transform.SetParent(transform.root, false);
            }
            else
            {
                popupBackground = new GameObject("HoverPopup");
                popupBackground.transform.SetParent(rootCanvas.transform, false);
            }

            // Add Canvas component for proper rendering with maximum sorting order
            popupCanvas = popupBackground.AddComponent<Canvas>();
            popupCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            popupCanvas.sortingOrder = 32767; // Maximum sorting order value
            popupCanvas.overrideSorting = true; // Override any parent canvas sorting

            // Add GraphicRaycaster to prevent blocking UI events
            popupBackground.AddComponent<GraphicRaycaster>();

            // Create background image with proper layout system
            GameObject background = new GameObject("Background");
            background.transform.SetParent(popupBackground.transform, false);

            RectTransform backgroundRect = background.AddComponent<RectTransform>();
            Image backgroundImage = background.AddComponent<Image>();
            HorizontalLayoutGroup backgroundLayout = background.AddComponent<HorizontalLayoutGroup>();
            ContentSizeFitter backgroundFitter = background.AddComponent<ContentSizeFitter>();

            // Configure background with proper layout
            backgroundRect.anchorMin = Vector2.zero;
            backgroundRect.anchorMax = Vector2.zero;
            backgroundRect.sizeDelta = new Vector2(200f, 40f); // Initial size
            backgroundImage.color = new Color(0.1f, 0.1f, 0.1f, 1f); // Completely opaque
            
            // Configure HorizontalLayoutGroup for proper text sizing
            backgroundLayout.padding = new RectOffset(10, 10, 10, 10); // 10px padding on all sides
            backgroundLayout.spacing = 0f;
            backgroundLayout.childAlignment = TextAnchor.MiddleLeft;
            backgroundLayout.childControlWidth = true;
            backgroundLayout.childControlHeight = true;
            backgroundLayout.childForceExpandWidth = false;
            backgroundLayout.childForceExpandHeight = false;
            
            // Configure ContentSizeFitter to size based on content
            backgroundFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            backgroundFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            // Create text component
            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(background.transform, false);

            RectTransform textRect = textObj.AddComponent<RectTransform>();
            popupText = textObj.AddComponent<Text>();
            ContentSizeFitter textFitter = textObj.AddComponent<ContentSizeFitter>();

            // Configure text with proper sizing
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.zero;
            textRect.sizeDelta = new Vector2(180f, 20f); // Initial size, will be adjusted by ContentSizeFitter

            popupText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            popupText.fontSize = 36; // Match CustomDropdown DEFAULT_FONT_SIZE
            popupText.fontStyle = FontStyle.Bold; // Make text bold
            popupText.color = Color.white;
            popupText.alignment = TextAnchor.MiddleLeft; // Left-aligned text
            popupText.text = " "; // Start with a space to ensure proper sizing
            
            // Configure ContentSizeFitter for the text
            textFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            textFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            // Configure the popup's RectTransform for proper positioning
            RectTransform popupRect = popupBackground.GetComponent<RectTransform>();
            if (popupRect != null)
            {
                // Set anchors to top-left for absolute positioning
                popupRect.anchorMin = Vector2.zero;
                popupRect.anchorMax = Vector2.zero;
                popupRect.pivot = Vector2.zero; // Top-left pivot for easier positioning
                popupRect.sizeDelta = new Vector2(200f, 40f); // Initial size, will be adjusted by ContentSizeFitter
                
                // Don't set initial position here - let ShowPopup handle it
                popupRect.anchoredPosition = Vector2.zero;
            }

            // Ensure the popup is at the very front of the hierarchy
            popupBackground.transform.SetAsLastSibling();

            // Initially hide the popup
            popupBackground.SetActive(false);
        }

        /// <summary>
        /// Shows the hover popup at the mouse position with the specified message.
        /// </summary>
        /// <param name="message">Message to display in the popup</param>
        public void ShowPopup(string message)
        {
            if (string.IsNullOrEmpty(message)) return;

            if (popupBackground == null)
            {
                CreatePopupComponents();
            }

            if (popupBackground != null && popupText != null)
            {
                // Set text and resize BEFORE making popup visible
                popupText.text = message;
                currentMessage = message;
                ResizePopupToFitText(message);
                
                // Ensure popup is at the very front of the hierarchy
                Canvas rootCanvas = GetComponentInParent<Canvas>();
                if (rootCanvas != null)
                {
                    popupBackground.transform.SetParent(rootCanvas.transform, false);
                    // Force it to be the very last sibling (rendered on top)
                    popupBackground.transform.SetAsLastSibling();
                    
                    // Also ensure the root canvas itself is at the front
                    rootCanvas.transform.SetAsLastSibling();
                }
                
                // Force the popup's canvas to have the highest possible sorting order
                if (popupCanvas != null)
                {
                    popupCanvas.sortingOrder = 32767; // Maximum sorting order value
                    popupCanvas.overrideSorting = true;
                }
                
                // Force canvas update to ensure proper rendering and sizing
                Canvas.ForceUpdateCanvases();
                
                // Position the popup correctly BEFORE making it visible
                UpdatePopupPosition();
                
                // NOW make the popup visible - it should already be in the correct position
                popupBackground.SetActive(true);
                isVisible = true;
                
                // Additional safety: ensure popup is still at the front after canvas update
                if (rootCanvas != null)
                {
                    popupBackground.transform.SetAsLastSibling();
                }

                // Notify listeners
                OnShowPopup?.Invoke(message);
            }
        }

        /// <summary>
        /// Hides the hover popup.
        /// </summary>
        public void HidePopup()
        {
            if (popupBackground != null)
            {
                popupBackground.SetActive(false);
                isVisible = false;
                currentMessage = "";
                
                // Notify listeners
                OnHidePopup?.Invoke();
            }
        }

        /// <summary>
        /// Updates the popup position to follow the mouse.
        /// </summary>
        public void UpdatePopupPosition()
        {
            if (popupBackground == null || !isVisible) return;

            Vector3 mousePosition = Input.mousePosition;
            
            // Get popup size - ensure it's properly calculated
            RectTransform popupRect = popupBackground.GetComponent<RectTransform>();
            if (popupRect == null) return;
            
            // Force a layout update to ensure the popup size is accurate
            LayoutRebuilder.ForceRebuildLayoutImmediate(popupRect);
            
            Vector2 popupSize = popupRect.sizeDelta;
            
            // Ensure we have valid size before positioning
            if (popupSize.x <= 0 || popupSize.y <= 0)
            {
                // Fallback to default size if layout hasn't calculated properly yet
                popupSize = new Vector2(200f, 40f);
            }
            
            // Calculate screen center
            Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
            
            // Determine which side of the mouse to position the popup based on screen center
            Vector3 popupPosition = Vector3.zero;
            
            if (mousePosition.x < screenCenter.x)
            {
                // Mouse is on left side of screen - position popup to the right of mouse
                popupPosition.x = mousePosition.x + (popupSize.x / 2f);
            }
            else
            {
                // Mouse is on right side of screen - position popup to the left of mouse
                popupPosition.x = mousePosition.x - (popupSize.x / 2f);
            }
            
            if (mousePosition.y < screenCenter.y)
            {
                // Mouse is on bottom half of screen - position popup above mouse
                popupPosition.y = mousePosition.y + (popupSize.y / 2f);
            }
            else
            {
                // Mouse is on top half of screen - position popup below mouse
                popupPosition.y = mousePosition.y - (popupSize.y / 2f);
            }
            
            // Ensure popup doesn't go off-screen
            popupPosition.x = Mathf.Clamp(popupPosition.x, 0f, Screen.width - popupSize.x);
            popupPosition.y = Mathf.Clamp(popupPosition.y, 0f, Screen.height - popupSize.y);
            
            // Use position directly for ScreenSpaceOverlay Canvas
            popupRect.position = popupPosition;
            
            // Store the last mouse position for update detection
            lastMousePosition = mousePosition;
        }

        /// <summary>
        /// Resizes the popup to fit the text content.
        /// </summary>
        /// <param name="message">The message to fit</param>
        private void ResizePopupToFitText(string message)
        {
            if (popupBackground == null || popupText == null) return;

            // Set the text content
            popupText.text = message;
            
            // Force layout update to ensure ContentSizeFitter and HorizontalLayoutGroup calculate the correct size
            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(popupBackground.GetComponent<RectTransform>());
            
            // Get the updated size from the background after layout has been calculated
            Transform backgroundTransform = popupBackground.transform.Find("Background");
            if (backgroundTransform != null)
            {
                RectTransform backgroundRect = backgroundTransform.GetComponent<RectTransform>();
                if (backgroundRect != null)
                {
                    // Update the popup size to match the background size
                    RectTransform popupRect = popupBackground.GetComponent<RectTransform>();
                    if (popupRect != null)
                    {
                        popupRect.sizeDelta = backgroundRect.sizeDelta;
                    }
                }
            }
        }

        /// <summary>
        /// Sets the font size for the popup text.
        /// </summary>
        /// <param name="size">The font size to set</param>
        public void SetFontSize(int size)
        {
            if (popupText != null)
            {
                popupText.fontSize = size;
            }
        }

        /// <summary>
        /// Sets the background color of the popup.
        /// </summary>
        /// <param name="color">The color to set</param>
        public void SetBackgroundColor(Color color)
        {
            if (popupBackground != null)
            {
                Transform backgroundTransform = popupBackground.transform.Find("Background");
                if (backgroundTransform != null)
                {
                    Image backgroundImage = backgroundTransform.GetComponent<Image>();
                    if (backgroundImage != null)
                    {
                        backgroundImage.color = color;
                    }
                }
            }
        }

        /// <summary>
        /// Sets the text color of the popup.
        /// </summary>
        /// <param name="color">The color to set</param>
        public void SetTextColor(Color color)
        {
            if (popupText != null)
            {
                popupText.color = color;
            }
        }

        /// <summary>
        /// Checks if the popup is currently visible.
        /// </summary>
        /// <returns>True if the popup is visible, false otherwise</returns>
        public bool IsVisible()
        {
            return isVisible;
        }

        /// <summary>
        /// Gets the current message being displayed.
        /// </summary>
        /// <returns>The current message, or empty string if not visible</returns>
        public string GetCurrentMessage()
        {
            return currentMessage;
        }

        private void Update()
        {
            // Update popup position if it's visible and mouse has moved
            if (isVisible && popupBackground != null)
            {
                Vector3 currentMousePosition = Input.mousePosition;
                if (Vector3.Distance(currentMousePosition, lastMousePosition) > 1f)
                {
                    UpdatePopupPosition();
                }
                
                // Ensure popup stays on top
                EnsurePopupOnTop();
            }
        }

        /// <summary>
        /// Ensures the hover popup stays at the very front of the rendering order.
        /// </summary>
        private void EnsurePopupOnTop()
        {
            if (popupBackground != null && isVisible)
            {
                // Force the popup to be the last sibling (rendered on top)
                popupBackground.transform.SetAsLastSibling();
                
                // Ensure the popup's canvas has maximum sorting order
                if (popupCanvas != null)
                {
                    popupCanvas.sortingOrder = 32767;
                    popupCanvas.overrideSorting = true;
                }
            }
        }
    }
} 