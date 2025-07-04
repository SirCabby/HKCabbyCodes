using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections;
using System.Linq;

namespace CabbyMenu.UI.Controls.CustomDropdown
{
    public class CustomDropdown : MonoBehaviour, IPointerClickHandler
    {
        private const float OPTION_HEIGHT = 40f;
        private const float OPTION_MARGIN = 10f;
        private const float OPTION_GAP = 3f;
        private const float DEFAULT_FONT_SIZE = 14f;
        private const float FONT_SIZE_RATIO = 0.6f;
        private const int MAX_VISIBLE_OPTIONS = 8;
        private const float SCROLLBAR_WIDTH = 10f;
        private const float MIN_DROPDOWN_WIDTH = 120f; // Minimum width for dropdown
        private const float TEXT_PADDING = 20f; // Padding for text within buttons

        /// <summary>
        /// Builds a custom dropdown GameObject with default styling.
        /// </summary>
        /// <returns>A tuple containing the GameObject and CustomDropdown for the created dropdown.</returns>
        public static (GameObject gameObject, CustomDropdown customDropdown) Build()
        {
            GameObject dropdownGO = new GameObject("CustomDropdown");
            CustomDropdown customDropdown = dropdownGO.AddComponent<CustomDropdown>();
            return (dropdownGO, customDropdown);
        }

        // Instance variables for customizable size
        private float mainButtonWidth = 200f;
        private float mainButtonHeight = 60f;
        private bool useDynamicSizing = true; // Enable dynamic sizing by default

        [Header("Custom Dropdown Settings")]
        [SerializeField] private Button mainButton;
        [SerializeField] private Text mainButtonText;
        [SerializeField] private Image mainButtonImage;
        [SerializeField] private GameObject dropdownPanel;

        [Header("Scroll View Components")]
        [SerializeField] private GameObject scrollView;
        [SerializeField] private GameObject viewport;
        [SerializeField] private GameObject content;
        [SerializeField] private GameObject scrollbar;

        // Private fields
        private List<string> options = new List<string>();
        private readonly List<GameObject> optionButtons = new List<GameObject>();
        private bool isOpen;
        private int selectedIndex;

        // Events - Using UnityEvent for consistency
        public UnityEvent<int> onValueChanged = new UnityEvent<int>();

        // Public properties
        public int Value => selectedIndex;
        public List<string> Options => options.ToList();

        // Static tracking for all open dropdowns
        private static readonly List<CustomDropdown> openDropdowns = new List<CustomDropdown>();

        /// <summary>
        /// Gets all currently open dropdowns.
        /// </summary>
        public static IReadOnlyList<CustomDropdown> OpenDropdowns => openDropdowns.AsReadOnly();

        /// <summary>
        /// Enables or disables dynamic sizing based on the largest dropdown item.
        /// </summary>
        /// <param name="enabled">True to enable dynamic sizing, false to use fixed width</param>
        public void SetDynamicSizing(bool enabled)
        {
            useDynamicSizing = enabled;
            if (enabled && options.Count > 0)
            {
                UpdateDropdownWidth();
            }
        }

        /// <summary>
        /// Calculates the width needed for the largest text item in the dropdown,
        /// using the same font, font size, and style as the main button's selected item view.
        /// </summary>
        /// <returns>The calculated width in pixels</returns>
        private float CalculateRequiredWidth()
        {
            if (options.Count == 0) return MIN_DROPDOWN_WIDTH;

            if (mainButtonText == null)
                return MIN_DROPDOWN_WIDTH;

            Font font = mainButtonText.font;
            // Calculate the actual font size that will be used in the main button text
            RectTransform mainRect = GetComponent<RectTransform>();
            int fontSize = Mathf.RoundToInt(mainRect.sizeDelta.y * FONT_SIZE_RATIO);
            FontStyle fontStyle = mainButtonText.fontStyle;

            float maxWidth = MIN_DROPDOWN_WIDTH;
            float extraPadding = TEXT_PADDING + 16f;

            UnityEngine.Debug.Log($"[DynamicSizing] Measuring dropdown options (font: {font?.name}, fontSize: {fontSize}, fontStyle: {fontStyle})");
            foreach (string option in options)
            {
                if (string.IsNullOrEmpty(option)) continue;

                var settings = mainButtonText.GetGenerationSettings(Vector2.zero);
                settings.fontSize = fontSize;
                settings.fontStyle = fontStyle;
                settings.generateOutOfBounds = true;
                settings.scaleFactor = mainButtonText.canvas != null ? mainButtonText.canvas.scaleFactor : 1f;

                TextGenerator generator = new TextGenerator();
                float textWidth = generator.GetPreferredWidth(option, settings) / settings.scaleFactor;
                UnityEngine.Debug.Log($"[DynamicSizing] Option: '{option}' | Measured width: {textWidth}");
                maxWidth = Mathf.Max(maxWidth, textWidth + extraPadding);
            }

            UnityEngine.Debug.Log($"[DynamicSizing] Final calculated required width: {maxWidth}px for {options.Count} options");
            return maxWidth;
        }

        /// <summary>
        /// Updates the dropdown width based on the largest item if dynamic sizing is enabled.
        /// </summary>
        private void UpdateDropdownWidth()
        {
            if (!useDynamicSizing) return;

            float newWidth = CalculateRequiredWidth();
            UnityEngine.Debug.Log($"[DynamicSizing] UpdateDropdownWidth: newWidth={newWidth}, oldWidth={mainButtonWidth}");
            if (Mathf.Abs(newWidth - mainButtonWidth) > 1f)
            {
                mainButtonWidth = newWidth;
                UnityEngine.Debug.Log($"[DynamicSizing] Setting mainButtonWidth to: {mainButtonWidth}");

                ConfigureMainButtonLayout();
                UpdateOptionButtonWidths();
                UpdateDropdownPanelWidth();

                // Log actual rendered width after layout
                RectTransform mainRect = GetComponent<RectTransform>();
                if (mainRect != null)
                {
                    UnityEngine.Debug.Log($"[DynamicSizing] After layout: mainRect.sizeDelta.x = {mainRect.sizeDelta.x}");
                }
                if (mainButtonText != null)
                {
                    RectTransform textRect = mainButtonText.GetComponent<RectTransform>();
                    UnityEngine.Debug.Log($"[DynamicSizing] mainButtonText.rect.width = {textRect.rect.width}, preferredWidth = {mainButtonText.preferredWidth}");
                }
            }
        }

        /// <summary>
        /// Updates the width of all existing option buttons.
        /// </summary>
        private void UpdateOptionButtonWidths()
        {
            foreach (GameObject optionButton in optionButtons)
            {
                if (optionButton != null)
                {
                    // Find the actual button within the parent panel
                    Transform buttonTransform = optionButton.transform.Find($"Option_{optionButtons.IndexOf(optionButton)}");
                    if (buttonTransform != null)
                    {
                        RectTransform buttonRect = buttonTransform.GetComponent<RectTransform>();
                        LayoutElement layoutElement = buttonTransform.GetComponent<LayoutElement>();
                        
                        if (buttonRect != null)
                        {
                            buttonRect.sizeDelta = new Vector2(mainButtonWidth - 21f, OPTION_HEIGHT);
                        }
                        
                        if (layoutElement != null)
                        {
                            layoutElement.minWidth = mainButtonWidth - 21f;
                            layoutElement.preferredWidth = mainButtonWidth - 21f;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Updates the dropdown panel width to match the new button width.
        /// </summary>
        private void UpdateDropdownPanelWidth()
        {
            if (dropdownPanel != null)
            {
                RectTransform panelRect = dropdownPanel.GetComponent<RectTransform>();
                if (panelRect != null)
                {
                    panelRect.sizeDelta = new Vector2(mainButtonWidth, panelRect.sizeDelta.y);
                }
            }
        }

        /// <summary>
        /// Checks if the mouse position is over any dropdown component (main button, panel, or options).
        /// </summary>
        /// <param name="mousePosition">The mouse position in screen coordinates.</param>
        /// <returns>True if the mouse is over any dropdown component, false otherwise.</returns>
        public static bool IsMouseOverAnyDropdown(Vector2 mousePosition)
        {
            // Clean up any destroyed dropdowns first
            CleanupDestroyedDropdowns();
            
            foreach (CustomDropdown dropdown in openDropdowns)
            {
                if (dropdown != null && dropdown.IsMouseOverDropdown(mousePosition))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Closes all open dropdowns.
        /// </summary>
        public static void CloseAllDropdowns()
        {
            // Clean up any destroyed dropdowns first
            CleanupDestroyedDropdowns();
            
            // Create a copy of the list to avoid modification during iteration
            var dropdownsToClose = openDropdowns.ToList();
            foreach (CustomDropdown dropdown in dropdownsToClose)
            {
                dropdown?.HideDropdown();
            }
        }

        /// <summary>
        /// Removes destroyed dropdowns from the open dropdowns list.
        /// </summary>
        private static void CleanupDestroyedDropdowns()
        {
            openDropdowns.RemoveAll(dropdown => dropdown == null);
        }

        /// <summary>
        /// Clears all dropdowns from the tracking list. Use when completely resetting the menu.
        /// </summary>
        public static void ClearAllDropdowns()
        {
            openDropdowns.Clear();
        }

        /// <summary>
        /// Checks if the mouse position is over this dropdown's components.
        /// </summary>
        /// <param name="mousePosition">The mouse position in screen coordinates.</param>
        /// <returns>True if the mouse is over this dropdown, false otherwise.</returns>
        public bool IsMouseOverDropdown(Vector2 mousePosition)
        {
            // Check main button
            if (IsMouseOverGameObject(gameObject, mousePosition))
            {
                return true;
            }

            // Check dropdown panel if open
            if (isOpen && dropdownPanel != null && IsMouseOverGameObject(dropdownPanel, mousePosition))
            {
                return true;
            }

            // Check option buttons if open
            if (isOpen && optionButtons != null)
            {
                foreach (GameObject optionButton in optionButtons)
                {
                    if (optionButton != null && IsMouseOverGameObject(optionButton, mousePosition))
                    {
                        return true;
                    }
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

        private void Awake()
        {
            InitializeBasicComponents();
        }

        private void OnDestroy()
        {
            // Remove from open dropdowns list when destroyed to prevent memory leaks
            openDropdowns.Remove(this);
        }

        private void Start()
        {
            UnityEngine.Debug.Log("CustomDropdown Start called");

            // Create the dropdown panel structure
            EnsureDropdownPanelCreated();

            // Don't set up onClick listener - we handle clicks through IPointerClickHandler
            // This prevents double-triggering of the dropdown toggle
            UnityEngine.Debug.Log("Button click handler not set up - using IPointerClickHandler instead");

            // Don't create options here - wait until dropdown is opened
            UnityEngine.Debug.Log("CustomDropdown Debug State:");
        }

        private void InitializeBasicComponents()
        {
            UnityEngine.Debug.Log("InitializeBasicComponents called");

            // Create main button image first (this will be the target graphic)
            if (mainButtonImage == null)
            {
                UnityEngine.Debug.Log("Creating mainButtonImage");
                mainButtonImage = gameObject.AddComponent<Image>();
                mainButtonImage.color = Constants.DROPDOWN_NORMAL;
            }

            // Create main button if not assigned
            if (mainButton == null)
            {
                UnityEngine.Debug.Log("Creating mainButton");
                mainButton = gameObject.AddComponent<Button>();
                // Set the target graphic for the button
                mainButton.targetGraphic = mainButtonImage;
            }

            // Create main button text if not assigned
            if (mainButtonText == null)
            {
                UnityEngine.Debug.Log("Creating mainButtonText");
                mainButtonText = CreateTextComponent("MainButtonText", transform);
            }

            // Configure main button size and positioning
            ConfigureMainButtonLayout();
        }

        private Text CreateTextComponent(string name, Transform parent)
        {
            GameObject textObj = new GameObject(name);
            textObj.transform.SetParent(parent);

            RectTransform textRect = textObj.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;

            Text textComponent = textObj.AddComponent<Text>();
            textComponent.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            textComponent.fontSize = (int)DEFAULT_FONT_SIZE;
            textComponent.color = Color.black;
            textComponent.alignment = TextAnchor.MiddleCenter;

            // Ensure text is on top of the button image
            textObj.transform.SetAsLastSibling();

            return textComponent;
        }

        private void ConfigureMainButtonLayout()
        {
            RectTransform mainRect = GetComponent<RectTransform>();
            if (mainRect != null)
            {
                mainRect.sizeDelta = new Vector2(mainButtonWidth, mainButtonHeight);
                mainRect.anchorMin = new Vector2(0.1f, 0.7f);
                mainRect.anchorMax = new Vector2(0.1f, 0.7f);
                mainRect.pivot = new Vector2(0.5f, 0.5f);

                UnityEngine.Debug.Log($"Main button sized to: {mainRect.sizeDelta}");
            }
        }

        private void EnsureDropdownPanelCreated()
        {
            if (dropdownPanel != null) return;

            UnityEngine.Debug.Log("EnsureDropdownPanelCreated called");

            // Find parent canvas
            Canvas parentCanvas = GetComponentInParent<Canvas>();
            if (parentCanvas == null)
            {
                UnityEngine.Debug.LogError("No parent canvas found for dropdown panel");
                return;
            }
            UnityEngine.Debug.Log($"Found parent: {parentCanvas.name}");

            // Create dropdown panel
            dropdownPanel = CreateDropdownPanel(parentCanvas);

            // Create scroll view components
            CreateScrollViewComponents();

            // Initially hide the dropdown panel
            dropdownPanel.SetActive(false);
            UnityEngine.Debug.Log($"Dropdown panel initially hidden - active: {dropdownPanel.activeSelf}");
            UnityEngine.Debug.Log($"Content reference set: {content != null}, name: {content?.name}");
            UnityEngine.Debug.Log("EnsureDropdownPanelCreated completed successfully");
        }

        private GameObject CreateDropdownPanel(Canvas parentCanvas)
        {
            GameObject panel = new GameObject("DropdownPanel");
            panel.transform.SetParent(parentCanvas.transform, false);
            UnityEngine.Debug.Log($"Created dropdownPanel GameObject: {panel.name}");

            // Configure panel rect transform - use absolute positioning relative to button
            RectTransform panelRect = panel.AddComponent<RectTransform>();
            RectTransform mainRect = GetComponent<RectTransform>();

            // Use absolute positioning (0,0) anchors so we can position relative to the button
            panelRect.anchorMin = Vector2.zero; // Bottom-left
            panelRect.anchorMax = Vector2.zero; // Bottom-left
            panelRect.sizeDelta = new Vector2(mainButtonWidth, 100f); // Use current mainButtonWidth, temporary height

            // Position the panel relative to the button's world position
            Vector3 buttonWorldPos = mainRect.position;
            Vector3 panelWorldPos = new Vector3(buttonWorldPos.x, buttonWorldPos.y - mainRect.sizeDelta.y / 2, buttonWorldPos.z);
            panelRect.position = panelWorldPos;

            panelRect.pivot = new Vector2(0f, 1f); // Top-left pivot for left-aligned dropdown positioning

            UnityEngine.Debug.Log($"Panel created as child of parent: {parentCanvas.name}");
            UnityEngine.Debug.Log($"Panel created with RectTransform: anchorMin={panelRect.anchorMin}, anchorMax={panelRect.anchorMax}, sizeDelta={panelRect.sizeDelta}");
            UnityEngine.Debug.Log($"Panel creation complete - active: {panel.activeSelf}, parent: {panel.transform.parent?.name}");
            UnityEngine.Debug.Log($"Button world position: {buttonWorldPos}, Panel world position: {panelWorldPos}");

            // Add panel image
            Image panelImage = panel.AddComponent<Image>();
            panelImage.color = Constants.DROPDOWN_PANEL_BACKGROUND;

            UnityEngine.Debug.Log($"Panel RectTransform - anchorMin: {panelRect.anchorMin}, anchorMax: {panelRect.anchorMax}, sizeDelta: {panelRect.sizeDelta}, anchoredPosition: {panelRect.anchoredPosition}");

            return panel;
        }

        private void CreateScrollViewComponents()
        {
            // Create scroll rect
            UnityEngine.Debug.Log("Creating scroll rect");
            scrollView = CreateScrollView();

            // Create viewport
            viewport = CreateViewport();

            // Create content
            content = CreateContent();

            // Create scrollbar
            scrollbar = CreateScrollbar();

            // Connect scroll rect components
            ScrollRect scrollRectComponent = scrollView.GetComponent<ScrollRect>();
            scrollRectComponent.viewport = viewport.GetComponent<RectTransform>();
            scrollRectComponent.content = content.GetComponent<RectTransform>();
            scrollRectComponent.verticalScrollbar = scrollbar.GetComponent<Scrollbar>();

            UnityEngine.Debug.Log($"Scroll view created - viewport: {viewport.name}, content: {content.name}, scrollbar: {scrollbar.name}");
        }

        private GameObject CreateScrollView()
        {
            GameObject scrollViewObj = new GameObject("ScrollView");
            scrollViewObj.transform.SetParent(dropdownPanel.transform, false);

            RectTransform scrollRect = scrollViewObj.AddComponent<RectTransform>();
            ScrollRect scrollRectComponent = scrollViewObj.AddComponent<ScrollRect>();

            // Configure scroll rect
            scrollRect.anchorMin = Vector2.zero; // Fill entire panel
            scrollRect.anchorMax = Vector2.one;  // Fill entire panel
            scrollRect.sizeDelta = Vector2.zero;
            scrollRect.anchoredPosition = Vector2.zero;

            // IMMEDIATE POSITION LOGGING - Track where positioning goes wrong
            RectTransform panelRect = dropdownPanel.GetComponent<RectTransform>();
            UnityEngine.Debug.Log("=== IMMEDIATE SCROLLVIEW CREATION LOGGING ===");
            UnityEngine.Debug.Log($"DropdownPanel - anchorMin: {panelRect.anchorMin}, anchorMax: {panelRect.anchorMax}, pivot: {panelRect.pivot}");
            UnityEngine.Debug.Log($"DropdownPanel - sizeDelta: {panelRect.sizeDelta}, anchoredPosition: {panelRect.anchoredPosition}");
            UnityEngine.Debug.Log($"DropdownPanel - world position: {panelRect.position}, local position: {panelRect.localPosition}");
            UnityEngine.Debug.Log($"ScrollView - anchorMin: {scrollRect.anchorMin}, anchorMax: {scrollRect.anchorMax}");
            UnityEngine.Debug.Log($"ScrollView - sizeDelta: {scrollRect.sizeDelta}, anchoredPosition: {scrollRect.anchoredPosition}");
            UnityEngine.Debug.Log($"ScrollView - world position: {scrollRect.position}, local position: {scrollRect.localPosition}");
            UnityEngine.Debug.Log("=== END IMMEDIATE SCROLLVIEW LOGGING ===");

            scrollRectComponent.horizontal = false;
            scrollRectComponent.vertical = true;
            scrollRectComponent.movementType = ScrollRect.MovementType.Clamped;
            scrollRectComponent.inertia = false;
            scrollRectComponent.scrollSensitivity = 10f;

            return scrollViewObj;
        }

        private GameObject CreateViewport()
        {
            GameObject viewportObj = new GameObject("Viewport");
            viewportObj.transform.SetParent(scrollView.transform, false);

            RectTransform viewportRect = viewportObj.AddComponent<RectTransform>();
            Image viewportImage = viewportObj.AddComponent<Image>();
            Mask viewportMask = viewportObj.AddComponent<Mask>();

            viewportRect.anchorMin = Vector2.zero;
            viewportRect.anchorMax = new Vector2(0.97f, 1f); // Leave space for scrollbar
            viewportRect.sizeDelta = Vector2.zero; // Let anchors control sizing
            viewportRect.anchoredPosition = Vector2.zero;
            viewportRect.pivot = new Vector2(0f, 1f); // Match panel's top-left pivot

            // Force layout update to ensure proper sizing
            LayoutRebuilder.ForceRebuildLayoutImmediate(viewportRect);

            UnityEngine.Debug.Log($"Viewport configured - anchorMin: {viewportRect.anchorMin}, anchorMax: {viewportRect.anchorMax}, sizeDelta: {viewportRect.sizeDelta}, anchoredPosition: {viewportRect.anchoredPosition}");

            // Additional debugging for viewport rect calculation
            UnityEngine.Debug.Log($"Viewport rect - width: {viewportRect.rect.width}, height: {viewportRect.rect.height}");
            UnityEngine.Debug.Log($"Viewport world bounds - min: {viewportRect.rect.min}, max: {viewportRect.rect.max}");

            // Check ScrollView height
            RectTransform scrollViewRect = scrollView.GetComponent<RectTransform>();
            UnityEngine.Debug.Log($"ScrollView rect - width: {scrollViewRect.rect.width}, height: {scrollViewRect.rect.height}");
            UnityEngine.Debug.Log($"ScrollView sizeDelta: {scrollViewRect.sizeDelta}");

            // Configure viewport image for visible background
            viewportImage.color = new Color(0.2f, 0.2f, 0.2f, 0.9f);
            viewportImage.enabled = true;
            viewportImage.raycastTarget = false;

            // Configure mask
            viewportMask.enabled = true;
            viewportMask.showMaskGraphic = false;

            return viewportObj;
        }

        private GameObject CreateContent()
        {
            GameObject contentObj = new GameObject("Content");
            contentObj.transform.SetParent(viewport.transform, false);

            RectTransform contentRect = contentObj.AddComponent<RectTransform>();
            VerticalLayoutGroup contentLayout = contentObj.AddComponent<VerticalLayoutGroup>();
            ContentSizeFitter contentFitter = contentObj.AddComponent<ContentSizeFitter>();

            // Configure content - top-aligned, fill horizontally
            contentRect.anchorMin = new Vector2(0, 1);
            contentRect.anchorMax = new Vector2(1, 1);
            contentRect.pivot = new Vector2(0.5f, 1f);
            contentRect.sizeDelta = new Vector2(0, 0);
            contentRect.anchoredPosition = Vector2.zero;

            // Configure layout group for proper button positioning
            contentLayout.enabled = false;
            contentLayout.spacing = 0f;
            contentLayout.padding = new RectOffset(0, 0, 0, 0);
            contentLayout.childAlignment = TextAnchor.UpperCenter;
            contentLayout.childControlWidth = false;
            contentLayout.childControlHeight = false;
            contentLayout.childForceExpandWidth = false;
            contentLayout.childForceExpandHeight = false;

            // Configure content size fitter
            contentFitter.enabled = false;
            contentFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            contentFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;

            UnityEngine.Debug.Log("Layout system disabled - using manual positioning");

            return contentObj;
        }

        private GameObject CreateScrollbar()
        {
            GameObject scrollbarObj = new GameObject("Scrollbar");
            scrollbarObj.transform.SetParent(dropdownPanel.transform, false);

            RectTransform scrollbarRect = scrollbarObj.AddComponent<RectTransform>();
            Scrollbar scrollbarComponent = scrollbarObj.AddComponent<Scrollbar>();
            Image scrollbarImage = scrollbarObj.AddComponent<Image>();

            // Configure scrollbar - align to right edge of panel
            scrollbarRect.anchorMin = new Vector2(1, 0);
            scrollbarRect.anchorMax = new Vector2(1, 1);
            scrollbarRect.sizeDelta = new Vector2(SCROLLBAR_WIDTH, 0); // Use constant for width
            scrollbarRect.anchoredPosition = Vector2.zero;
            scrollbarRect.pivot = new Vector2(1f, 1f); // Right-top pivot to align with right edge

            // Configure scrollbar image (background)
            scrollbarImage.color = new Color(0.3f, 0.3f, 0.3f, 0.8f);
            scrollbarComponent.targetGraphic = scrollbarImage;

            scrollbarComponent.value = 1f; // Start at top
            scrollbarComponent.direction = Scrollbar.Direction.BottomToTop;

            // Create scrollbar handle (the draggable part)
            CreateScrollbarHandle(scrollbarObj, scrollbarComponent);

            return scrollbarObj;
        }

        private void CreateScrollbarHandle(GameObject scrollbarObj, Scrollbar scrollbarComponent)
        {
            GameObject handleObj = new GameObject("Handle");
            handleObj.transform.SetParent(scrollbarObj.transform, false);

            RectTransform handleRect = handleObj.AddComponent<RectTransform>();
            Image handleImage = handleObj.AddComponent<Image>();
            Button handleButton = handleObj.AddComponent<Button>();

            // Configure handle rect transform
            handleRect.anchorMin = Vector2.zero;
            handleRect.anchorMax = Vector2.one;
            handleRect.sizeDelta = Vector2.zero;
            handleRect.anchoredPosition = Vector2.zero;

            // Configure handle image
            handleImage.color = new Color(0.6f, 0.6f, 0.6f, 1f);
            handleButton.targetGraphic = handleImage;

            // Set the handle for the scrollbar
            scrollbarComponent.handleRect = handleRect;

            UnityEngine.Debug.Log($"Created scrollbar handle with color: {handleImage.color}");
        }

        public void SetOptions(List<string> options)
        {
            this.options = options.ToList();
            UnityEngine.Debug.Log($"SetOptions called with {options.Count} options");

            // Update dropdown width if dynamic sizing is enabled
            if (useDynamicSizing)
            {
                UpdateDropdownWidth();
            }

            // Update the main button text to show the first option
            UpdateMainButtonText();
        }

        private void UpdateMainButtonText()
        {
            if (mainButtonText != null && options.Count > 0)
            {
                string displayText = selectedIndex >= 0 && selectedIndex < options.Count
                    ? options[selectedIndex]
                    : options[0];
                mainButtonText.text = displayText;

                // Dynamically set font size based on button height
                RectTransform mainRect = GetComponent<RectTransform>();
                int fontSize = Mathf.RoundToInt(mainRect.sizeDelta.y * FONT_SIZE_RATIO);
                mainButtonText.fontSize = fontSize;
                mainButtonText.color = Color.black;

                // Ensure text is active and on top
                mainButtonText.gameObject.SetActive(true);
                mainButtonText.transform.SetAsLastSibling();

                // Log z and screen position and font size
                RectTransform textRect = mainButtonText.GetComponent<RectTransform>();
                Vector3 textWorld = textRect.position;
                Vector3 textScreen = RectTransformUtility.WorldToScreenPoint(null, textWorld);
                UnityEngine.Debug.Log($"MainButtonText z: {textWorld.z}, screen: {textScreen}, fontSize: {fontSize}");

                UnityEngine.Debug.Log($"Updated main button text to: '{displayText}', color: {mainButtonText.color}, active: {mainButtonText.gameObject.activeSelf}");
            }
            else
            {
                UnityEngine.Debug.Log($"Cannot update main button text - mainButtonText: {mainButtonText != null}, options count: {options.Count}");
            }
        }

        private void CreateOptionButtons()
        {
            if (options == null || options.Count == 0) return;

            UnityEngine.Debug.Log($"Initializing dropdown options for the first time...");

            // Clear existing buttons
            ClearExistingOptionButtons();

            // Ensure we have the content area
            if (content == null)
            {
                UnityEngine.Debug.LogError("Content area is null - cannot create option buttons");
                return;
            }

            // Create new option buttons
            for (int i = 0; i < options.Count; i++)
            {
                CreateSingleOptionButton(i);
            }

            UnityEngine.Debug.Log($"Created {optionButtons.Count} option buttons");
        }

        private void ClearExistingOptionButtons()
        {
            foreach (var button in optionButtons)
            {
                if (button != null && button.gameObject != null)
                {
                    DestroyImmediate(button.gameObject);
                }
            }
            optionButtons.Clear();
        }

        private void CreateSingleOptionButton(int index)
        {
            // Create parent panel that is 15 pixels wider than the button
            GameObject parentPanel = new GameObject($"OptionPanel_{index}");
            parentPanel.transform.SetParent(content.transform, false);
            parentPanel.SetActive(true);

            // Add required components to parent panel
            RectTransform parentPanelRect = parentPanel.AddComponent<RectTransform>();
            Image parentPanelImage = parentPanel.AddComponent<Image>();

            // Calculate top padding to balance the OPTION_MARGIN
            float topPadding = OPTION_MARGIN / 2f;

            // Configure parent panel - stretch across full width, same height as button
            parentPanelRect.anchorMin = new Vector2(0, 1);
            parentPanelRect.anchorMax = new Vector2(1, 1);
            parentPanelRect.sizeDelta = new Vector2(0, OPTION_HEIGHT); // Stretch across width, fixed height
            parentPanelRect.anchoredPosition = new Vector2(0, -topPadding - index * (OPTION_HEIGHT + OPTION_GAP));
            parentPanelRect.pivot = new Vector2(0.5f, 1f); // Center X, Top Y

            // Configure parent panel image (transparent background)
            parentPanelImage.color = new Color(0, 0, 0, 0);

            // Create option button from scratch since we don't have a template
            GameObject optionObj = new GameObject($"Option_{index}");
            optionObj.transform.SetParent(parentPanel.transform, false);
            optionObj.SetActive(true);

            // Add required components
            RectTransform optionRect = optionObj.AddComponent<RectTransform>();
            Image optionImage = optionObj.AddComponent<Image>();
            Button optionButton = optionObj.AddComponent<Button>();
            LayoutElement optionLayout = optionObj.AddComponent<LayoutElement>();

            // Set pivot to top center for proper vertical stacking
            optionRect.pivot = new Vector2(0.5f, 1f); // Center X, Top Y

            // Configure layout element for proper sizing
            ConfigureOptionLayoutElement(optionLayout);

            // Configure rect transform - left-align within parent panel
            ConfigureOptionRectTransform(optionRect, index);

            // Configure image
            optionImage.color = Constants.DROPDOWN_OPTION_BACKGROUND;
            UnityEngine.Debug.Log($"Set option {index} button color to: {optionImage.color}");

            // Create text component
            CreateOptionText(optionObj, options[index]);

            // Add click handler
            int capturedIndex = index; // Capture the index for the lambda
            optionButton.onClick.AddListener(() => OnOptionSelected(capturedIndex));

            // Store the parent panel reference (which contains the button)
            optionButtons.Add(parentPanel);

            UnityEngine.Debug.Log($"Created option button {index}: '{options[index]}' with parent panel 15px wider and left-aligned");
        }

        private void ConfigureOptionLayoutElement(LayoutElement layoutElement)
        {
            layoutElement.preferredHeight = OPTION_HEIGHT;
            layoutElement.minHeight = OPTION_HEIGHT;
            layoutElement.flexibleWidth = 0f;
            layoutElement.minWidth = mainButtonWidth - 21f; // Reduced by 6px (3px on each side)
            layoutElement.preferredWidth = mainButtonWidth - 21f; // Reduced by 6px (3px on each side)
        }

#pragma warning disable IDE0060 // Remove unused parameter
        private void ConfigureOptionRectTransform(RectTransform rectTransform, int index)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            // Left-align with 6px padding, fixed width, vertically centered
            rectTransform.anchorMin = new Vector2(0, 0.5f); // Left, center Y
            rectTransform.anchorMax = new Vector2(0, 0.5f); // Left, center Y
            rectTransform.pivot = new Vector2(0, 0.5f); // Left, center Y
            rectTransform.sizeDelta = new Vector2(mainButtonWidth - 21f, OPTION_HEIGHT); // Reduced by 6px (3px on each side)
            rectTransform.anchoredPosition = new Vector2(6f, 0); // Left-aligned with 6px padding
        }

        private void CreateOptionText(GameObject parent, string text)
        {
            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(parent.transform, false);

            RectTransform textRect = textObj.AddComponent<RectTransform>();
            Text optionText = textObj.AddComponent<Text>();

            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;
            textRect.anchoredPosition = Vector2.zero;

            optionText.text = text;
            optionText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            optionText.fontSize = (int)DEFAULT_FONT_SIZE;
            optionText.color = Color.black;
            optionText.alignment = TextAnchor.MiddleCenter;
        }

        private void ConfigureOptionButtons()
        {
            try
            {
                UnityEngine.Debug.Log("=== ConfigureOptionButtons called ===");

                // Calculate top padding to balance the OPTION_MARGIN
                float topPadding = OPTION_MARGIN / 2f;

                // Ensure all option buttons are visible and properly positioned
                for (int i = 0; i < optionButtons.Count; i++)
                {
                    try
                    {
                        if (optionButtons[i] != null)
                        {
                            optionButtons[i].SetActive(true);

                            RectTransform parentPanelRect = optionButtons[i].GetComponent<RectTransform>();
                            if (parentPanelRect != null)
                            {
                                // Configure parent panel to stretch across content width
                                parentPanelRect.anchorMin = new Vector2(0, 1);
                                parentPanelRect.anchorMax = new Vector2(1, 1);
                                parentPanelRect.sizeDelta = new Vector2(0, OPTION_HEIGHT); // Stretch across width, fixed height

                                // Position parent panels sequentially from top to bottom with gap
                                // Start from top with padding, then position each element downward with gaps between them
                                parentPanelRect.anchoredPosition = new Vector2(0, -topPadding - i * (OPTION_HEIGHT + OPTION_GAP));

                                UnityEngine.Debug.Log($"Option parent panel {i} repositioned - anchorMin: {parentPanelRect.anchorMin}, anchorMax: {parentPanelRect.anchorMax}, sizeDelta: {parentPanelRect.sizeDelta}, anchoredPosition: {parentPanelRect.anchoredPosition}");
                            }

                            // Debug: List all children of the parent panel
                            UnityEngine.Debug.Log($"Parent panel {i} has {optionButtons[i].transform.childCount} children:");
                            for (int j = 0; j < optionButtons[i].transform.childCount; j++)
                            {
                                Transform child = optionButtons[i].transform.GetChild(j);
                                UnityEngine.Debug.Log($"  Child {j}: {child.name}");
                            }

                            // Configure the actual button within the parent panel
                            Transform buttonTransform = optionButtons[i].transform.Find($"Option_{i}");
                            if (buttonTransform != null)
                            {
                                RectTransform buttonRect = buttonTransform.GetComponent<RectTransform>();
                                if (buttonRect != null)
                                {
                                    // Configure button to be left-aligned with 6px padding within parent panel
                                    buttonRect.anchorMin = new Vector2(0, 0.5f); // Left, center Y
                                    buttonRect.anchorMax = new Vector2(0, 0.5f); // Left, center Y
                                    buttonRect.pivot = new Vector2(0, 0.5f); // Left, center Y
                                    buttonRect.sizeDelta = new Vector2(mainButtonWidth - 21f, OPTION_HEIGHT); // Reduced by 6px (3px on each side)
                                    buttonRect.anchoredPosition = new Vector2(6f, 0); // Left-aligned with 6px padding

                                    UnityEngine.Debug.Log($"Option button {i} configured - anchorMin: {buttonRect.anchorMin}, anchorMax: {buttonRect.anchorMax}, sizeDelta: {buttonRect.sizeDelta}, anchoredPosition: {buttonRect.anchoredPosition}");
                                }
                            }
                            else
                            {
                                UnityEngine.Debug.LogError($"Button Option_{i} not found in parent panel {i}");
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        UnityEngine.Debug.LogError($"Exception in ConfigureOptionButtons for button {i}: {ex.Message}");
                        UnityEngine.Debug.LogError($"Stack trace: {ex.StackTrace}");
                    }
                }

                // Log option button configurations
                for (int i = 0; i < Mathf.Min(optionButtons.Count, 3); i++) // Log first 3 buttons
                {
                    if (optionButtons[i] != null)
                    {
                        RectTransform parentPanelRect = optionButtons[i].GetComponent<RectTransform>();
                        if (parentPanelRect != null)
                        {
                            UnityEngine.Debug.Log($"Option parent panel OptionPanel_{i} - anchorMin: {parentPanelRect.anchorMin}, anchorMax: {parentPanelRect.anchorMax}, sizeDelta: {parentPanelRect.sizeDelta}");
                        }
                    }
                }

                UnityEngine.Debug.Log("ConfigureOptionButtons completed successfully!");
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.LogError($"Exception in ConfigureOptionButtons: {ex.Message}");
                UnityEngine.Debug.LogError($"Stack trace: {ex.StackTrace}");
            }
        }

        private void OnOptionSelected(int index)
        {
            if (index >= 0 && index < options.Count)
            {
                selectedIndex = index;

                UpdateMainButtonText();
                HideDropdown();

                // Notify listeners
                onValueChanged?.Invoke(selectedIndex);

                UnityEngine.Debug.Log($"Option selected: {options[selectedIndex]} (index: {selectedIndex})");
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            UnityEngine.Debug.Log("IPointerClickHandler.OnPointerClick called");
            UnityEngine.Debug.Log($"Click position: {eventData.position}, isOpen: {isOpen}");
            ToggleDropdown();
        }

        private void ToggleDropdown()
        {
            UnityEngine.Debug.Log($"ToggleDropdown called, isOpen: {isOpen}, options count: {options.Count}");
            if (isOpen)
            {
                UnityEngine.Debug.Log("Closing dropdown...");
                HideDropdown();
            }
            else
            {
                UnityEngine.Debug.Log("Opening dropdown...");
                OpenDropdown();
            }
        }

        private void OpenDropdown()
        {
            UnityEngine.Debug.Log($"OpenDropdown called, options count: {options.Count}");

            // Create option buttons if they don't exist
            if (optionButtons.Count == 0)
            {
                CreateOptionButtons();
            }

            // Position and show the dropdown panel
            PositionDropdownPanel();

            UnityEngine.Debug.Log("About to call ShowDropdownPanel from OpenDropdown...");
            ShowDropdownPanel();

            isOpen = true;
            
            // Add to open dropdowns list
            if (!openDropdowns.Contains(this))
            {
                openDropdowns.Add(this);
            }
        }

        private void HideDropdown()
        {
            UnityEngine.Debug.Log("HideDropdown called");
            if (dropdownPanel != null)
            {
                dropdownPanel.SetActive(false);
                UnityEngine.Debug.Log($"Dropdown panel hidden - active: {dropdownPanel.activeSelf}");
            }
            isOpen = false;
            
            // Remove from open dropdowns list
            openDropdowns.Remove(this);
            
            UnityEngine.Debug.Log($"Dropdown state - isOpen: {isOpen}");
        }

        private void PositionDropdownPanel()
        {
            if (dropdownPanel == null) return;

            // Get the main button's position and size
            RectTransform mainRect = GetComponent<RectTransform>();

            // Calculate panel height to match visible content height
            // Use MAX_VISIBLE_OPTIONS to limit panel height, but content will be sized for all options
            float panelHeight = Mathf.Min(options.Count, MAX_VISIBLE_OPTIONS) * OPTION_HEIGHT +
                               Mathf.Min(options.Count - 1, MAX_VISIBLE_OPTIONS - 1) * OPTION_GAP +
                               OPTION_MARGIN;

            // Update panel size and position - use absolute positioning
            RectTransform panelRect = dropdownPanel.GetComponent<RectTransform>();
            // Use the current mainButtonWidth instead of mainRect.sizeDelta.x to ensure consistency
            panelRect.sizeDelta = new Vector2(mainButtonWidth, panelHeight);

            // Get the button's actual world bounds
            Vector3[] buttonCorners = new Vector3[4];
            mainRect.GetWorldCorners(buttonCorners);
            
            // buttonCorners[0] = bottom-left, buttonCorners[1] = top-left, buttonCorners[2] = top-right, buttonCorners[3] = bottom-right
            Vector3 buttonBottomLeft = buttonCorners[0];  // Bottom-left corner
            Vector3 buttonTopLeft = buttonCorners[1];     // Top-left corner
            Vector3 buttonTopRight = buttonCorners[2];    // Top-right corner
            Vector3 buttonBottomRight = buttonCorners[3]; // Bottom-right corner

            // Position panel with its left edge at the button's left edge, and its top edge at the button's bottom edge
            Vector3 panelWorldPos = new Vector3(buttonBottomLeft.x, buttonBottomLeft.y, buttonBottomLeft.z);
            panelRect.position = panelWorldPos;

            // Force layout update to ensure proper positioning
            LayoutRebuilder.ForceRebuildLayoutImmediate(panelRect);

            UnityEngine.Debug.Log($"[FIXED] Panel positioning - anchorMin: {panelRect.anchorMin}, anchorMax: {panelRect.anchorMax}, pivot: {panelRect.pivot}");
            UnityEngine.Debug.Log($"[FIXED] Panel final - world position: {panelRect.position}, sizeDelta: {panelRect.sizeDelta}");
            UnityEngine.Debug.Log($"[FIXED] Panel matches main button width: {mainButtonWidth}, height for {Mathf.Min(options.Count, MAX_VISIBLE_OPTIONS)} visible options: {panelRect.sizeDelta.y}");
            UnityEngine.Debug.Log($"[FIXED] Button world position: {mainRect.position}, Panel world position: {panelWorldPos}");
            UnityEngine.Debug.Log($"[FIXED] Button corners - BL: {buttonBottomLeft}, TL: {buttonTopLeft}, TR: {buttonTopRight}, BR: {buttonBottomRight}");
            UnityEngine.Debug.Log($"[FIXED] Button left edge: {buttonBottomLeft.x}, Panel left edge: {panelWorldPos.x}");
            UnityEngine.Debug.Log($"[FIXED] Button bottom edge: {buttonBottomLeft.y}, Panel top edge: {panelWorldPos.y}");
            UnityEngine.Debug.Log($"[FIXED] Button size: {mainRect.sizeDelta}, Panel size: {panelRect.sizeDelta}");
            UnityEngine.Debug.Log($"[FIXED] Distance from button bottom to panel top: {buttonBottomLeft.y - panelWorldPos.y}");
            UnityEngine.Debug.Log($"[FIXED] Expected: 0 (panel top should be at button bottom)");
        }

        private void ShowDropdownPanel()
        {
            if (dropdownPanel == null) return;

            UnityEngine.Debug.Log("=== ShowDropdownPanel started ===");

            RectTransform panelRect = dropdownPanel.GetComponent<RectTransform>();
            dropdownPanel.SetActive(true);

            UnityEngine.Debug.Log("About to call ConfigurePanelComponents...");
            ConfigurePanelComponents(panelRect);

            UnityEngine.Debug.Log("About to call ConfigureScrollViewComponents...");
            ConfigureScrollViewComponents(panelRect);

            UnityEngine.Debug.Log("About to call VerifyScrollRectConnections...");
            VerifyScrollRectConnections();

            UnityEngine.Debug.Log("About to call ConfigureContentSizing...");
            ConfigureContentSizing();

            UnityEngine.Debug.Log("About to call ConfigureOptionButtons...");
            // Ensure all option buttons are visible and properly positioned
            ConfigureOptionButtons();

            UnityEngine.Debug.Log("About to call ResetScrollPosition...");
            ResetScrollPosition();

            UnityEngine.Debug.Log("About to call EnsureProperZOrder...");
            EnsureProperZOrder();

            UnityEngine.Debug.Log("About to call LogDetailedPositions...");
            LogDetailedPositions();

            UnityEngine.Debug.Log($"[FIXED] Dropdown panel activated and positioned. Panel height: {panelRect.sizeDelta.y}, AnchoredPosition: {panelRect.anchoredPosition}, SizeDelta: {panelRect.sizeDelta}");
        }

#pragma warning disable IDE0060 // Remove unused parameter
        private void ConfigurePanelComponents(RectTransform panelRect)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            // Ensure panel image is visible
            Image panelImage = dropdownPanel.GetComponent<Image>();
            if (panelImage != null)
            {
                panelImage.enabled = true;
                panelImage.color = Constants.DROPDOWN_PANEL_BACKGROUND;
                UnityEngine.Debug.Log($"Panel image enabled with color: {panelImage.color}");
            }
        }

#pragma warning disable IDE0060 // Remove unused parameter
        private void ConfigureScrollViewComponents(RectTransform panelRect)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            // Ensure scroll view is properly configured
            if (scrollView != null)
            {
                ScrollRect scrollRectComponent = scrollView.GetComponent<ScrollRect>();
                if (scrollRectComponent != null)
                {
                    scrollRectComponent.enabled = true;
                    scrollRectComponent.horizontal = false;
                    scrollRectComponent.vertical = true;
                    scrollRectComponent.movementType = ScrollRect.MovementType.Clamped;
                    scrollRectComponent.inertia = false;
                    scrollRectComponent.scrollSensitivity = 10f;
                    scrollRectComponent.elasticity = 0f; // Prevent over-scrolling

                    UnityEngine.Debug.Log($"Scroll view state - enabled: {scrollRectComponent.enabled}, viewport: {scrollRectComponent.viewport?.name}, content: {scrollRectComponent.content?.name}");
                }

                // Ensure ScrollView is properly positioned within the panel
                RectTransform scrollViewRect = scrollView.GetComponent<RectTransform>();
                if (scrollViewRect != null)
                {
                    // Configure ScrollView to fill the entire panel from top to bottom
                    scrollViewRect.anchorMin = Vector2.zero; // Bottom-left
                    scrollViewRect.anchorMax = Vector2.one;  // Top-right
                    scrollViewRect.sizeDelta = Vector2.zero; // No size offset
                    scrollViewRect.anchoredPosition = Vector2.zero; // No position offset
                    scrollViewRect.pivot = new Vector2(0f, 1f); // Match panel's top-left pivot

                    // Force layout update to ensure proper positioning
                    LayoutRebuilder.ForceRebuildLayoutImmediate(scrollViewRect);

                    UnityEngine.Debug.Log($"ScrollView repositioned - anchorMin: {scrollViewRect.anchorMin}, anchorMax: {scrollViewRect.anchorMax}, sizeDelta: {scrollViewRect.sizeDelta}, anchoredPosition: {scrollViewRect.anchoredPosition}");
                    UnityEngine.Debug.Log($"ScrollView local position after repositioning: {scrollViewRect.localPosition}");

                    // Additional debug: Check if ScrollView fills the panel properly
                    RectTransform dropdownPanelRectDebug = dropdownPanel.GetComponent<RectTransform>();
                    UnityEngine.Debug.Log($"Panel size: {dropdownPanelRectDebug.sizeDelta}, ScrollView size: {scrollViewRect.sizeDelta}");
                    UnityEngine.Debug.Log($"Panel pivot: {dropdownPanelRectDebug.pivot}, ScrollView pivot: {scrollViewRect.pivot}");
                }
            }

            // Configure viewport properly - fill the entire panel
            if (viewport != null)
            {
                RectTransform viewportRect = viewport.GetComponent<RectTransform>();
                if (viewportRect != null)
                {
                    // Set viewport to fill the panel but leave space for scrollbar
                    viewportRect.anchorMin = Vector2.zero;
                    viewportRect.anchorMax = new Vector2(0.97f, 1f); // Leave space for scrollbar
                    viewportRect.sizeDelta = Vector2.zero; // Let anchors control sizing
                    viewportRect.anchoredPosition = Vector2.zero;
                    viewportRect.pivot = new Vector2(0f, 1f); // Match panel's top-left pivot

                    // Force layout update to ensure proper sizing
                    LayoutRebuilder.ForceRebuildLayoutImmediate(viewportRect);

                    UnityEngine.Debug.Log($"Viewport configured - anchorMin: {viewportRect.anchorMin}, anchorMax: {viewportRect.anchorMax}, sizeDelta: {viewportRect.sizeDelta}, anchoredPosition: {viewportRect.anchoredPosition}");

                    // Additional debugging for viewport rect calculation
                    UnityEngine.Debug.Log($"Viewport rect - width: {viewportRect.rect.width}, height: {viewportRect.rect.height}");
                    UnityEngine.Debug.Log($"Viewport world bounds - min: {viewportRect.rect.min}, max: {viewportRect.rect.max}");

                    // Check ScrollView height
                    RectTransform scrollViewRect = scrollView.GetComponent<RectTransform>();
                    UnityEngine.Debug.Log($"ScrollView rect - width: {scrollViewRect.rect.width}, height: {scrollViewRect.rect.height}");
                    UnityEngine.Debug.Log($"ScrollView sizeDelta: {scrollViewRect.sizeDelta}");
                }

                Image viewportImage = viewport.GetComponent<Image>();
                if (viewportImage != null)
                {
                    viewportImage.enabled = true;
                    viewportImage.color = new Color(0.2f, 0.2f, 0.2f, 0.9f);
                    UnityEngine.Debug.Log($"Viewport image enabled with color: {viewportImage.color}");
                }

                Mask viewportMask = viewport.GetComponent<Mask>();
                if (viewportMask != null)
                {
                    viewportMask.enabled = true;
                    viewportMask.showMaskGraphic = false;

                    UnityEngine.Debug.Log($"Viewport mask - enabled: {viewportMask.enabled}, showMaskGraphic: {viewportMask.showMaskGraphic}, graphic: {viewportMask.graphic?.name}");
                }
                else
                {
                    UnityEngine.Debug.LogError("Viewport mask component is null!");
                }

                // Force the viewport to update its mask
                Canvas.ForceUpdateCanvases();
            }

            // Configure scrollbar
            if (scrollbar != null)
            {
                RectTransform scrollbarRect = scrollbar.GetComponent<RectTransform>();
                Scrollbar scrollbarComponent = scrollbar.GetComponent<Scrollbar>();
                Image scrollbarImage = scrollbar.GetComponent<Image>();

                if (scrollbarRect != null && scrollbarComponent != null)
                {
                    // Position scrollbar on the right side of the panel
                    scrollbarRect.anchorMin = new Vector2(1, 0);
                    scrollbarRect.anchorMax = new Vector2(1, 1);
                    scrollbarRect.sizeDelta = new Vector2(SCROLLBAR_WIDTH, 0);
                    scrollbarRect.anchoredPosition = Vector2.zero;
                    scrollbarRect.pivot = new Vector2(1f, 1f); // Right-top pivot to align with right edge

                    // Ensure scrollbar is visible and properly connected
                    scrollbar.SetActive(true);
                    scrollbarComponent.enabled = true;

                    // Ensure scrollbar image is visible
                    if (scrollbarImage != null)
                    {
                        scrollbarImage.enabled = true;
                        scrollbarImage.color = new Color(0.3f, 0.3f, 0.3f, 0.8f);
                    }

                    // Ensure handle is properly configured
                    if (scrollbarComponent.handleRect != null)
                    {
                        Image handleImage = scrollbarComponent.handleRect.GetComponent<Image>();
                        if (handleImage != null)
                        {
                            handleImage.enabled = true;
                            handleImage.color = new Color(0.6f, 0.6f, 0.6f, 1f);
                        }
                    }

                    UnityEngine.Debug.Log($"Scrollbar configured - sizeDelta: {scrollbarRect.sizeDelta}, enabled: {scrollbarComponent.enabled}, handle: {scrollbarComponent.handleRect?.name}");
                }
            }
        }

        private void ConfigureContentSizing()
        {
            if (content == null) return;

            RectTransform contentRect = content.GetComponent<RectTransform>();
            if (contentRect != null)
            {
                // Calculate content size based on ALL options, not just visible ones
                // This ensures scrolling works when there are more options than can fit
                // Include top and bottom padding (OPTION_MARGIN) to match the panel height calculation
                float contentHeight = OPTION_MARGIN + options.Count * OPTION_HEIGHT + (options.Count - 1) * OPTION_GAP;

                // Configure content to stretch across viewport width and position at top
                contentRect.anchorMin = new Vector2(0, 1);
                contentRect.anchorMax = new Vector2(1, 1);
                contentRect.pivot = new Vector2(0.5f, 1f);
                contentRect.sizeDelta = new Vector2(0, contentHeight);

                // Position content at the top of the viewport
                contentRect.anchoredPosition = Vector2.zero;

                // Force layout update to ensure proper positioning
                LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);

                // Force another layout update after a frame to ensure all positioning is correct
                StartCoroutine(ForceContentLayoutUpdate(contentRect));

                UnityEngine.Debug.Log($"Content sizing - sizeDelta: {contentRect.sizeDelta}, anchorMin: {contentRect.anchorMin}, anchorMax: {contentRect.anchorMax}");
                UnityEngine.Debug.Log($"Content height for {options.Count} total options: {contentHeight}");
                UnityEngine.Debug.Log($"Content anchoredPosition: {contentRect.anchoredPosition}");
                UnityEngine.Debug.Log($"Content local position: {contentRect.localPosition}");
            }
        }

        private IEnumerator ForceContentLayoutUpdate(RectTransform contentRect)
        {
            yield return null; // Wait for next frame
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);
            UnityEngine.Debug.Log($"Content layout updated after frame - local position: {contentRect.localPosition}");
        }

        private void EnsureProperZOrder()
        {
            // Ensure proper z-order
            Canvas parentCanvas = dropdownPanel.GetComponentInParent<Canvas>();
            if (parentCanvas != null)
            {
                parentCanvas.sortingOrder = 1000;
                UnityEngine.Debug.Log($"Brought parent canvas to front: {dropdownPanel.name}, sorting order: {parentCanvas.sortingOrder}");
            }

            if (scrollView != null)
            {
                scrollView.transform.SetAsLastSibling();
                UnityEngine.Debug.Log($"Brought scroll view to front: {scrollView.name}");
            }

            if (viewport != null)
            {
                viewport.transform.SetAsLastSibling();
                UnityEngine.Debug.Log($"Brought viewport to front: {viewport.name}");
            }

            if (content != null)
            {
                content.transform.SetAsLastSibling();
                UnityEngine.Debug.Log($"Brought content to front: {content.name}");
            }

            if (scrollbar != null)
            {
                scrollbar.transform.SetAsLastSibling();
                UnityEngine.Debug.Log($"Brought scrollbar to front: {scrollbar.name}");
            }

            // Ensure dropdown panel is at the front
            dropdownPanel.transform.SetAsLastSibling();
            UnityEngine.Debug.Log($"Dropdown panel visibility ensured - active: {dropdownPanel.activeSelf}, sibling index: {dropdownPanel.transform.GetSiblingIndex()}");
        }

        private void ResetScrollPosition()
        {
            if (scrollView != null)
            {
                ScrollRect scrollRectComponent = scrollView.GetComponent<ScrollRect>();
                if (scrollRectComponent != null)
                {
                    // Reset to top position (normalizedPosition.y = 1 for top)
                    scrollRectComponent.normalizedPosition = new Vector2(0, 1);
                    UnityEngine.Debug.Log($"Reset scroll position to top - normalizedPosition: {scrollRectComponent.normalizedPosition}");
                }
            }

            // Reset scrollbar value to top
            if (scrollbar != null)
            {
                Scrollbar scrollbarComponent = scrollbar.GetComponent<Scrollbar>();
                if (scrollbarComponent != null)
                {
                    scrollbarComponent.value = 1f; // 1 = top, 0 = bottom
                    UnityEngine.Debug.Log($"Reset scrollbar value to top: {scrollbarComponent.value}");
                }
            }
        }

        private void VerifyScrollRectConnections()
        {
            if (scrollView == null || viewport == null || content == null) return;

            ScrollRect scrollRectComponent = scrollView.GetComponent<ScrollRect>();
            if (scrollRectComponent != null)
            {
                // Ensure viewport and content are properly connected
                RectTransform viewportRect = viewport.GetComponent<RectTransform>();
                RectTransform contentRect = content.GetComponent<RectTransform>();

                scrollRectComponent.viewport = viewportRect;
                scrollRectComponent.content = contentRect;

                // Ensure scrollbar is connected
                if (scrollbar != null)
                {
                    Scrollbar scrollbarComponent = scrollbar.GetComponent<Scrollbar>();
                    if (scrollbarComponent != null)
                    {
                        scrollRectComponent.verticalScrollbar = scrollbarComponent;
                        UnityEngine.Debug.Log($"Connected scrollbar to ScrollRect: {scrollbar.name}");
                    }
                }

                UnityEngine.Debug.Log($"Verified scroll rect connections - viewport: {scrollRectComponent.viewport?.name}, content: {scrollRectComponent.content?.name}, scrollbar: {scrollRectComponent.verticalScrollbar?.name}");

                // Force the scroll rect to update its internal state
                scrollRectComponent.enabled = false;
                scrollRectComponent.enabled = true;

                UnityEngine.Debug.Log($"Scroll rect re-enabled - viewport: {scrollRectComponent.viewport?.name}, content: {scrollRectComponent.content?.name}");

                // Check scroll rect viewport and content references
                UnityEngine.Debug.Log($"ScrollRect viewport reference: {scrollRectComponent.viewport?.name}");
                UnityEngine.Debug.Log($"ScrollRect content reference: {scrollRectComponent.content?.name}");

                // Check scrollbar connection
                UnityEngine.Debug.Log($"ScrollRect vertical scrollbar reference: {scrollRectComponent.verticalScrollbar?.name}");
                if (scrollbar != null)
                {
                    UnityEngine.Debug.Log($"Scrollbar is connected to ScrollRect: {scrollRectComponent.verticalScrollbar == scrollbar.GetComponent<Scrollbar>()}");
                }
            }
        }

        private void LogDetailedPositions()
        {
            UnityEngine.Debug.Log("=== DETAILED POSITION ANALYSIS ===");

            CheckParentCanvasConfiguration();

            // Main button positions
            RectTransform mainRect = GetComponent<RectTransform>();
            Vector3 mainWorldPos = mainRect.position;
            Vector3 mainScreenPos = RectTransformUtility.WorldToScreenPoint(null, mainWorldPos);
            UnityEngine.Debug.Log($"MainButton - World: {mainWorldPos}, Screen: {mainScreenPos}, Local: {mainRect.localPosition}");
            UnityEngine.Debug.Log($"MainButton - sizeDelta: {mainRect.sizeDelta}, anchoredPosition: {mainRect.anchoredPosition}");

            // Panel positions
            RectTransform panelRect = dropdownPanel.GetComponent<RectTransform>();
            Vector3 panelWorldPos = panelRect.position;
            Vector3 panelScreenPos = RectTransformUtility.WorldToScreenPoint(null, panelWorldPos);
            UnityEngine.Debug.Log($"DropdownPanel - World: {panelWorldPos}, Screen: {panelScreenPos}, Local: {panelRect.localPosition}");
            UnityEngine.Debug.Log($"DropdownPanel - sizeDelta: {panelRect.sizeDelta}, anchoredPosition: {panelRect.anchoredPosition}");

            // Scroll view positions
            if (scrollView != null)
            {
                ScrollRect scrollRectComponent = scrollView.GetComponent<ScrollRect>();
                RectTransform scrollRectTransform = scrollView.GetComponent<RectTransform>();
                Vector3 scrollWorldPos = scrollRectTransform.position;
                Vector3 scrollScreenPos = RectTransformUtility.WorldToScreenPoint(null, scrollWorldPos);
                UnityEngine.Debug.Log($"ScrollView - World: {scrollWorldPos}, Screen: {scrollScreenPos}, Local: {scrollRectTransform.localPosition}");
                UnityEngine.Debug.Log($"ScrollView - sizeDelta: {scrollRectTransform.sizeDelta}, anchoredPosition: {scrollRectTransform.anchoredPosition}");

                // Log scroll rect properties
                if (scrollRectComponent != null)
                {
                    UnityEngine.Debug.Log($"ScrollRect - normalizedPosition: {scrollRectComponent.normalizedPosition}, velocity: {scrollRectComponent.velocity}");
                    UnityEngine.Debug.Log($"ScrollRect - horizontal: {scrollRectComponent.horizontal}, vertical: {scrollRectComponent.vertical}");
                    UnityEngine.Debug.Log($"ScrollRect - movementType: {scrollRectComponent.movementType}, elasticity: {scrollRectComponent.elasticity}");
                }
            }

            // Viewport positions
            if (scrollView != null && viewport != null)
            {
                RectTransform viewportRect = viewport.GetComponent<RectTransform>();
                Vector3 viewportWorldPos = viewportRect.position;
                Vector3 viewportScreenPos = RectTransformUtility.WorldToScreenPoint(null, viewportWorldPos);
                UnityEngine.Debug.Log($"Viewport - World: {viewportWorldPos}, Screen: {viewportScreenPos}, Local: {viewportRect.localPosition}");
                UnityEngine.Debug.Log($"Viewport - sizeDelta: {viewportRect.sizeDelta}, anchoredPosition: {viewportRect.anchoredPosition}");
            }

            // Content positions
            if (scrollView != null && content != null)
            {
                RectTransform contentRect = content.GetComponent<RectTransform>();
                Vector3 contentWorldPos = contentRect.position;
                Vector3 contentScreenPos = RectTransformUtility.WorldToScreenPoint(null, contentWorldPos);
                UnityEngine.Debug.Log($"Content - World: {contentWorldPos}, Screen: {contentScreenPos}, Local: {contentRect.localPosition}");
                UnityEngine.Debug.Log($"Content - sizeDelta: {contentRect.sizeDelta}, anchoredPosition: {contentRect.anchoredPosition}");
            }

            // Scrollbar positions
            if (scrollbar != null)
            {
                RectTransform scrollbarRect = scrollbar.GetComponent<RectTransform>();
                Scrollbar scrollbarComponent = scrollbar.GetComponent<Scrollbar>();
                Vector3 scrollbarWorldPos = scrollbarRect.position;
                Vector3 scrollbarScreenPos = RectTransformUtility.WorldToScreenPoint(null, scrollbarWorldPos);
                UnityEngine.Debug.Log($"Scrollbar - World: {scrollbarWorldPos}, Screen: {scrollbarScreenPos}, Local: {scrollbarRect.localPosition}");
                UnityEngine.Debug.Log($"Scrollbar - sizeDelta: {scrollbarRect.sizeDelta}, anchoredPosition: {scrollbarRect.anchoredPosition}");
                UnityEngine.Debug.Log($"Scrollbar - value: {scrollbarComponent.value}, enabled: {scrollbarComponent.enabled}");

                // Log handle information
                if (scrollbarComponent.handleRect != null)
                {
                    RectTransform handleRect = scrollbarComponent.handleRect;
                    Image handleImage = handleRect.GetComponent<Image>();
                    UnityEngine.Debug.Log($"Scrollbar Handle - name: {handleRect.name}, enabled: {handleImage?.enabled}, color: {handleImage?.color}");
                    UnityEngine.Debug.Log($"Scrollbar Handle - sizeDelta: {handleRect.sizeDelta}, anchoredPosition: {handleRect.anchoredPosition}");
                }
                else
                {
                    UnityEngine.Debug.Log("Scrollbar Handle - NOT FOUND!");
                }
            }

            // Option button positions (first few)
            for (int i = 0; i < Mathf.Min(3, optionButtons.Count); i++)
            {
                if (optionButtons[i] != null)
                {
                    RectTransform parentPanelRect = optionButtons[i].GetComponent<RectTransform>();
                    Vector3 parentWorldPos = parentPanelRect.position;
                    Vector3 parentScreenPos = RectTransformUtility.WorldToScreenPoint(null, parentWorldPos);
                    UnityEngine.Debug.Log($"OptionParentPanel_{i} - World: {parentWorldPos}, Screen: {parentScreenPos}, Local: {parentPanelRect.localPosition}");
                    UnityEngine.Debug.Log($"OptionParentPanel_{i} - sizeDelta: {parentPanelRect.sizeDelta}, anchoredPosition: {parentPanelRect.anchoredPosition}");

                    // Also log the actual button within the parent panel
                    Transform buttonTransform = optionButtons[i].transform.Find($"Option_{i}");
                    if (buttonTransform != null)
                    {
                        RectTransform buttonRect = buttonTransform.GetComponent<RectTransform>();
                        Vector3 buttonWorldPos = buttonRect.position;
                        Vector3 buttonScreenPos = RectTransformUtility.WorldToScreenPoint(null, buttonWorldPos);
                        UnityEngine.Debug.Log($"OptionButton_{i} - World: {buttonWorldPos}, Screen: {buttonScreenPos}, Local: {buttonRect.localPosition}");
                        UnityEngine.Debug.Log($"OptionButton_{i} - sizeDelta: {buttonRect.sizeDelta}, anchoredPosition: {buttonRect.anchoredPosition}");
                    }
                    else
                    {
                        UnityEngine.Debug.Log($"OptionButton_{i} - NOT FOUND within parent panel");
                    }
                }
            }

            // Check for overlap issues
            UnityEngine.Debug.Log("=== OVERLAP ANALYSIS ===");
            if (scrollView != null && viewport != null && content != null)
            {
                ScrollRect scrollRectComponent = scrollView.GetComponent<ScrollRect>();
                RectTransform viewportRect = viewport.GetComponent<RectTransform>();
                RectTransform contentRect = content.GetComponent<RectTransform>();

                // Get world positions for gap analysis
                Vector3 viewportWorldPos = viewportRect.position;
                Vector3 contentWorldPos = contentRect.position;
                Vector3 dropdownPanelWorldPos = dropdownPanel.GetComponent<RectTransform>().position;

                // NEW: Parent-child relationship analysis
                UnityEngine.Debug.Log("=== PARENT-CHILD RELATIONSHIP ANALYSIS ===");
                UnityEngine.Debug.Log($"DropdownPanel parent: {dropdownPanel.transform.parent?.name}");
                UnityEngine.Debug.Log($"ScrollView parent: {scrollView.transform.parent?.name}");
                UnityEngine.Debug.Log($"Viewport parent: {viewport.transform.parent?.name}");
                UnityEngine.Debug.Log($"Content parent: {content.transform.parent?.name}");

                // Check local positions relative to parents
                UnityEngine.Debug.Log($"ScrollView local position: {scrollView.transform.localPosition}");
                UnityEngine.Debug.Log($"Viewport local position: {viewport.transform.localPosition}");
                UnityEngine.Debug.Log($"Content local position: {content.transform.localPosition}");

                // Check if ScrollView is properly positioned within panel
                RectTransform scrollViewRect = scrollView.GetComponent<RectTransform>();
                UnityEngine.Debug.Log($"ScrollView anchorMin: {scrollViewRect.anchorMin}, anchorMax: {scrollViewRect.anchorMax}");
                UnityEngine.Debug.Log($"ScrollView sizeDelta: {scrollViewRect.sizeDelta}, anchoredPosition: {scrollViewRect.anchoredPosition}");

                // Check if Viewport is properly positioned within ScrollView
                UnityEngine.Debug.Log($"Viewport anchorMin: {viewportRect.anchorMin}, anchorMax: {viewportRect.anchorMax}");
                UnityEngine.Debug.Log($"Viewport sizeDelta: {viewportRect.sizeDelta}, anchoredPosition: {viewportRect.anchoredPosition}");

                // Check if Content is properly positioned within Viewport
                UnityEngine.Debug.Log($"Content anchorMin: {contentRect.anchorMin}, anchorMax: {contentRect.anchorMax}");
                UnityEngine.Debug.Log($"Content sizeDelta: {contentRect.sizeDelta}, anchoredPosition: {contentRect.anchoredPosition}");

                // Check if viewport and content are in the same position
                float viewportContentDistance = Vector3.Distance(viewportRect.position, contentRect.position);
                UnityEngine.Debug.Log($"Distance between viewport and content: {viewportContentDistance}");

                // Check if viewport size matches panel size
                RectTransform dropdownPanelRect = dropdownPanel.GetComponent<RectTransform>();
                float sizeDifference = Mathf.Abs(viewportRect.sizeDelta.x - dropdownPanelRect.sizeDelta.x);
                UnityEngine.Debug.Log($"Viewport vs Panel width difference: {sizeDifference}");

                // Check if content is properly positioned within viewport
                Vector3 viewportCenter = viewportRect.position;
                Vector3 contentCenter = contentRect.position;
                UnityEngine.Debug.Log($"Viewport center: {viewportCenter}, Content center: {contentCenter}");

                // NEW: Detailed gap analysis
                UnityEngine.Debug.Log("=== GAP ANALYSIS ===");
                UnityEngine.Debug.Log($"Panel height: {dropdownPanelRect.sizeDelta.y}");
                UnityEngine.Debug.Log($"Content height: {contentRect.sizeDelta.y}");
                UnityEngine.Debug.Log($"Viewport height: {viewportRect.sizeDelta.y}");
                UnityEngine.Debug.Log($"Panel top Y: {dropdownPanelWorldPos.y + dropdownPanelRect.sizeDelta.y / 2}");
                UnityEngine.Debug.Log($"Panel bottom Y: {dropdownPanelWorldPos.y - dropdownPanelRect.sizeDelta.y / 2}");
                UnityEngine.Debug.Log($"Content top Y: {contentWorldPos.y + contentRect.sizeDelta.y / 2}");
                UnityEngine.Debug.Log($"Content bottom Y: {contentWorldPos.y - contentRect.sizeDelta.y / 2}");
                UnityEngine.Debug.Log($"Viewport top Y: {viewportWorldPos.y + viewportRect.sizeDelta.y / 2}");
                UnityEngine.Debug.Log($"Viewport bottom Y: {viewportWorldPos.y - viewportRect.sizeDelta.y / 2}");

                // Calculate the actual gap
                float panelTop = dropdownPanelWorldPos.y + dropdownPanelRect.sizeDelta.y / 2;
                float contentTop = contentWorldPos.y + contentRect.sizeDelta.y / 2;
                float gapAtTop = panelTop - contentTop;
                UnityEngine.Debug.Log($"Gap at top of panel: {gapAtTop}");

                // Check scroll rect viewport and content references
                UnityEngine.Debug.Log($"ScrollRect viewport reference: {scrollRectComponent.viewport?.name}");
                UnityEngine.Debug.Log($"ScrollRect content reference: {scrollRectComponent.content?.name}");

                // Check scrollbar connection
                UnityEngine.Debug.Log($"ScrollRect vertical scrollbar reference: {scrollRectComponent.verticalScrollbar?.name}");
                if (scrollbar != null)
                {
                    UnityEngine.Debug.Log($"Scrollbar is connected to ScrollRect: {scrollRectComponent.verticalScrollbar == scrollbar.GetComponent<Scrollbar>()}");
                }
            }

            UnityEngine.Debug.Log("=== END POSITION ANALYSIS ===");
        }

        private void CheckParentCanvasConfiguration()
        {
            Canvas parentCanvas = dropdownPanel.GetComponentInParent<Canvas>();
            if (parentCanvas != null)
            {
                UnityEngine.Debug.Log($"Parent Canvas - name: {parentCanvas.name}, renderMode: {parentCanvas.renderMode}");
                UnityEngine.Debug.Log($"Parent Canvas - worldCamera: {parentCanvas.worldCamera?.name}, planeDistance: {parentCanvas.planeDistance}");
                UnityEngine.Debug.Log($"Parent Canvas - sortingOrder: {parentCanvas.sortingOrder}, targetDisplay: {parentCanvas.targetDisplay}");

                // Check if canvas is using screen space overlay or camera
                if (parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
                {
                    UnityEngine.Debug.Log("Canvas is using ScreenSpaceOverlay mode");
                }
                else if (parentCanvas.renderMode == RenderMode.ScreenSpaceCamera)
                {
                    UnityEngine.Debug.Log($"Canvas is using ScreenSpaceCamera mode with camera: {parentCanvas.worldCamera?.name}");
                }
                else if (parentCanvas.renderMode == RenderMode.WorldSpace)
                {
                    UnityEngine.Debug.Log("Canvas is using WorldSpace mode");
                }
            }
        }

        // These methods are stubs for external compatibility - implement if needed
        public void SetValue(int value) 
        { 
            if (value >= 0 && value < options.Count)
            {
                selectedIndex = value;
                UpdateMainButtonText();
                UnityEngine.Debug.Log($"SetValue called with index {value}: '{options[value]}'");
            }
            else
            {
                UnityEngine.Debug.LogWarning($"SetValue called with invalid index {value}, options count: {options.Count}");
            }
        }
        public void SetSize(float width) { SetSize(width, mainButtonHeight); }
        public void SetSize(float width, float height)
        {
            // If dynamic sizing is enabled, only allow height changes
            if (useDynamicSizing)
            {
                UnityEngine.Debug.Log($"Dynamic sizing is enabled - ignoring width parameter ({width}px), using calculated width ({mainButtonWidth}px)");
                mainButtonHeight = height;
            }
            else
            {
                mainButtonWidth = width;
                mainButtonHeight = height;
            }

            // Update the main button layout
            ConfigureMainButtonLayout();

            // Update the main button text if it exists
            UpdateMainButtonText();
        }
#pragma warning disable IDE0060 // Remove unused parameter
        public void SetFontSize(int size) { /* TODO: implement if needed */ }
#pragma warning disable IDE0060 // Remove unused parameter
        public void SetColors(Color color) { /* TODO: implement if needed */ }
#pragma warning disable IDE0060 // Remove unused parameter
        public void SetColors(Color normal, Color hover, Color pressed) { /* TODO: implement if needed */ }
    }
}