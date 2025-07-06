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
        private const float OPTION_GAP = 5f;
        private const float DEFAULT_FONT_SIZE = 18f; // Increased from 14f
        private const float FONT_SIZE_RATIO = 0.7f; // Increased from 0.6f for better proportion
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
        private float mainButtonWidth = MIN_DROPDOWN_WIDTH; // Start with minimum width, will be calculated dynamically
        private float mainButtonHeight = 0f; // Set by SetSize method
        private bool useDynamicSizing = true; // Enable dynamic sizing by default
        private float dynamicOptionHeight = OPTION_HEIGHT; // Dynamic option height based on panel

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

        // Custom colors for button states
        private Color normalColor = Constants.DROPDOWN_NORMAL;
        private Color hoverColor = Constants.DROPDOWN_HOVER;
        private Color pressedColor = Constants.DROPDOWN_PRESSED;
        
        // Custom colors for option button states
        private Color optionNormalColor = Constants.DROPDOWN_OPTION_BACKGROUND;
        private Color optionHoverColor = Constants.DROPDOWN_OPTION_HOVER;
        private Color optionPressedColor = Constants.DROPDOWN_OPTION_PRESSED;

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
        /// Sets the option height based on the main button height and recalculates dynamic sizing.
        /// </summary>
        /// <param name="buttonHeight">The height of the main button</param>
        public void SetOptionHeightFromButtonHeight(float buttonHeight)
        {
            mainButtonHeight = buttonHeight;
            
            // Calculate option height based on button height
            // Use a ratio of the button height for option height
            float optionHeightRatio = 0.8f; // Option height as ratio of button height
            dynamicOptionHeight = buttonHeight * optionHeightRatio;
            
            // Ensure minimum height
            dynamicOptionHeight = Mathf.Max(dynamicOptionHeight, 30f);
            
            // Update existing option buttons if they exist
            if (optionButtons.Count > 0)
            {
                UpdateOptionButtonHeights();
                UpdateOptionTextFontSizes();
            }
        }

        /// <summary>
        /// Calculates the dynamic option height and font size based on the dropdown panel height.
        /// Only used when option height hasn't been explicitly set via SetOptionHeightFromButtonHeight.
        /// </summary>
        private void CalculateDynamicSizing()
        {
            if (dropdownPanel == null) return;

            RectTransform panelRect = dropdownPanel.GetComponent<RectTransform>();
            if (panelRect == null) return;

            // Only calculate dynamic sizing if mainButtonHeight hasn't been set yet
            if (mainButtonHeight <= 0f)
            {
                // Calculate available height for options (panel height minus margins)
                float availableHeight = panelRect.sizeDelta.y - OPTION_MARGIN;
                
                // Calculate how many options can fit in the visible area
                int visibleOptions = Mathf.Min(options.Count, MAX_VISIBLE_OPTIONS);
                
                if (visibleOptions > 0)
                {
                    // Calculate dynamic option height based on available space
                    // Account for gaps between options
                    float totalGaps = (visibleOptions - 1) * OPTION_GAP;
                    float heightForOptions = availableHeight - totalGaps;
                    dynamicOptionHeight = heightForOptions / visibleOptions;
                    
                    // Ensure minimum height
                    dynamicOptionHeight = Mathf.Max(dynamicOptionHeight, 30f);
                }
                else
                {
                    dynamicOptionHeight = OPTION_HEIGHT;
                }
            }
        }

        /// <summary>
        /// Calculates the font size for option text based on the dynamic option height.
        /// </summary>
        /// <returns>The calculated font size</returns>
        private int CalculateOptionFontSize()
        {
            // Use a ratio of the dynamic option height for font size
            // Leave some padding for text margins
            float fontHeightRatio = 0.6f; // Font height as ratio of option height
            int fontSize = Mathf.RoundToInt(dynamicOptionHeight * fontHeightRatio);
            
            // Ensure reasonable bounds
            fontSize = Mathf.Max(fontSize, 12);
            fontSize = Mathf.Min(fontSize, 24);
            
            return fontSize;
        }

        /// <summary>
        /// Updates the height of all existing option buttons.
        /// </summary>
        private void UpdateOptionButtonHeights()
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
                            buttonRect.sizeDelta = new Vector2(buttonRect.sizeDelta.x, dynamicOptionHeight);
                        }
                        
                        if (layoutElement != null)
                        {
                            layoutElement.minHeight = dynamicOptionHeight;
                            layoutElement.preferredHeight = dynamicOptionHeight;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Updates the font size of all existing option text components.
        /// </summary>
        private void UpdateOptionTextFontSizes()
        {
            foreach (GameObject optionButton in optionButtons)
            {
                if (optionButton != null)
                {
                    // Find the text component within the option button
                    Transform buttonTransform = optionButton.transform.Find($"Option_{optionButtons.IndexOf(optionButton)}");
                    if (buttonTransform != null)
                    {
                        Text textComponent = buttonTransform.GetComponentInChildren<Text>();
                        if (textComponent != null)
                        {
                            textComponent.fontSize = CalculateOptionFontSize();
                        }
                    }
                }
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
            float extraPadding = TEXT_PADDING + 16f + 21f; // Add 21px to account for option button reduction

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
                maxWidth = Mathf.Max(maxWidth, textWidth + extraPadding);
            }

            return maxWidth;
        }

        /// <summary>
        /// Updates the dropdown width based on the largest item if dynamic sizing is enabled.
        /// </summary>
        private void UpdateDropdownWidth()
        {
            if (!useDynamicSizing) return;

            float newWidth = CalculateRequiredWidth();
            if (Mathf.Abs(newWidth - mainButtonWidth) > 1f)
            {
                mainButtonWidth = newWidth;

                ConfigureMainButtonLayout();
                UpdateOptionButtonWidths();
                UpdateDropdownPanelWidth();

                // Log actual rendered width after layout
                RectTransform mainRect = GetComponent<RectTransform>();
                if (mainRect != null)
                {
                }
                if (mainButtonText != null)
                {
                    RectTransform textRect = mainButtonText.GetComponent<RectTransform>();
                }
            }
        }

        /// <summary>
        /// Forces a recalculation of the dropdown width based on current options.
        /// </summary>
        public void ForceWidthRecalculation()
        {
            if (useDynamicSizing && options.Count > 0)
            {
                UpdateDropdownWidth();
                // Force layout update to ensure changes are applied
                LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
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
                            buttonRect.sizeDelta = new Vector2(mainButtonWidth - 21f, dynamicOptionHeight);
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
            // Create the dropdown panel structure
            EnsureDropdownPanelCreated();

            // Don't set up onClick listener - we handle clicks through IPointerClickHandler
            // This prevents double-triggering of the dropdown toggle

            // Don't create options here - wait until dropdown is opened
        }

        private void InitializeBasicComponents()
        {
            // Create main button image first (this will be the target graphic)
            if (mainButtonImage == null)
            {
                mainButtonImage = gameObject.AddComponent<Image>();
                mainButtonImage.color = Constants.DROPDOWN_NORMAL;
            }

            // Create main button if not assigned
            if (mainButton == null)
            {
                mainButton = gameObject.AddComponent<Button>();
                // Set the target graphic for the button
                mainButton.targetGraphic = mainButtonImage;
            }

            // Create main button text if not assigned
            if (mainButtonText == null)
            {
                mainButtonText = CreateTextComponent("MainButtonText", transform);
            }

            // Create border for the button
            CreateButtonBorder();

            // Create background image that's smaller than the border
            CreateButtonBackground();

            // Ensure proper z-order: border (back) -> background (middle) -> text (front)
            EnsureProperButtonZOrder();

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
            textRect.offsetMin = new Vector2(TEXT_PADDING, 0); // Add left padding to position text after dropdown arrow
            textRect.offsetMax = new Vector2(-TEXT_PADDING, 0); // Add right padding for balance

            Text textComponent = textObj.AddComponent<Text>();
            textComponent.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            textComponent.fontSize = (int)DEFAULT_FONT_SIZE;
            textComponent.color = Color.black;
            textComponent.alignment = TextAnchor.MiddleLeft; // Left-align the text

            // Ensure text is on top of the button image
            textObj.transform.SetAsLastSibling();

            return textComponent;
        }

        private void CreateButtonBorder()
        {
            // Create border GameObject as a child of the main button
            GameObject borderObj = new GameObject("ButtonBorder");
            borderObj.transform.SetParent(transform, false);

            // Add RectTransform and Image components for the border
            RectTransform borderRect = borderObj.AddComponent<RectTransform>();
            Image borderImage = borderObj.AddComponent<Image>();

            // Configure border rect transform to be slightly larger than the button
            borderRect.anchorMin = Vector2.zero;
            borderRect.anchorMax = Vector2.one;
            borderRect.sizeDelta = new Vector2(4f, 4f); // 2px border on each side (2px * 2 = 4px total)
            borderRect.anchoredPosition = Vector2.zero;

            // Configure border image
            borderImage.color = Color.black; // Black border
            borderImage.sprite = CreateBorderSprite();

            // Ensure border is behind the main button image
            // Set border as first sibling so it renders behind the main button image
            borderObj.transform.SetAsFirstSibling();
        }

        private Sprite CreateBorderSprite()
        {
            // Create a simple border sprite using a white square texture
            Texture2D borderTexture = new Texture2D(4, 4, TextureFormat.RGBA32, false);
            
            // Fill with transparent pixels
            Color[] pixels = new Color[16];
            for (int i = 0; i < 16; i++)
            {
                pixels[i] = Color.white; // White pixels for the border
            }
            
            borderTexture.SetPixels(pixels);
            borderTexture.Apply();
            
            // Create sprite from texture
            Sprite borderSprite = Sprite.Create(borderTexture, new Rect(0, 0, 4, 4), new Vector2(0.5f, 0.5f));
            
            return borderSprite;
        }

        private void EnsureProperButtonZOrder()
        {
            // Find the border GameObject
            Transform borderTransform = transform.Find("ButtonBorder");
            
            // Find the background GameObject
            Transform backgroundTransform = transform.Find("ButtonBackground");

            // Ensure proper z-order: border (back) -> background (middle) -> text (front)
            // Set border to be first (renders in back)
            borderTransform?.SetAsFirstSibling();

            // Set background in the middle
            backgroundTransform?.SetAsLastSibling();
            
            // Ensure text is on top
            mainButtonText?.transform.SetAsLastSibling();
        }

        private void CreateButtonBackground()
        {
            // Create background GameObject as a child of the main button
            GameObject backgroundObj = new GameObject("ButtonBackground");
            backgroundObj.transform.SetParent(transform, false);

            // Add RectTransform and Image components for the background
            RectTransform backgroundRect = backgroundObj.AddComponent<RectTransform>();
            Image backgroundImage = backgroundObj.AddComponent<Image>();

            // Configure background rect transform to be slightly smaller than the button
            backgroundRect.anchorMin = Vector2.zero;
            backgroundRect.anchorMax = Vector2.one;
            backgroundRect.sizeDelta = new Vector2(-4f, -4f); // 2px smaller on each side
            backgroundRect.anchoredPosition = Vector2.zero;

            // Configure background image
            backgroundImage.color = Constants.DROPDOWN_NORMAL;
            backgroundImage.sprite = CreateBorderSprite(); // Use same sprite as border
        }



        private void ConfigureMainButtonLayout()
        {
            RectTransform mainRect = GetComponent<RectTransform>();
            if (mainRect != null)
            {
                mainRect.sizeDelta = new Vector2(mainButtonWidth, mainButtonHeight);
                mainRect.pivot = new Vector2(0.0f, 0.5f); // Left-center pivot for left-aligned positioning

            }

            // Update border size to be larger than the button
            Transform borderTransform = transform.Find("ButtonBorder");
            if (borderTransform != null)
            {
                RectTransform borderRect = borderTransform.GetComponent<RectTransform>();
                if (borderRect != null)
                {
                    borderRect.sizeDelta = new Vector2(4f, 4f); // 2px border on each side
                }
            }

            // Update background size to be smaller than the button
            Transform backgroundTransform = transform.Find("ButtonBackground");
            if (backgroundTransform != null)
            {
                RectTransform backgroundRect = backgroundTransform.GetComponent<RectTransform>();
                if (backgroundRect != null)
                {
                    backgroundRect.sizeDelta = new Vector2(-4f, -4f); // 2px smaller on each side
                }
            }

            // Ensure proper z-order is maintained after layout changes
            EnsureProperButtonZOrder();
        }

        private void EnsureDropdownPanelCreated()
        {
            if (dropdownPanel != null) return;

            // Find parent canvas
            Canvas parentCanvas = GetComponentInParent<Canvas>();
            if (parentCanvas == null)
            {
                return;
            }
            // Create dropdown panel
            dropdownPanel = CreateDropdownPanel(parentCanvas);

            // Create scroll view components
            CreateScrollViewComponents();

            // Initially hide the dropdown panel
            dropdownPanel.SetActive(false);
        }

        private GameObject CreateDropdownPanel(Canvas parentCanvas)
        {
            GameObject panel = new GameObject("DropdownPanel");
            panel.transform.SetParent(parentCanvas.transform, false);

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

            // Add panel image
            Image panelImage = panel.AddComponent<Image>();
            panelImage.color = Constants.DROPDOWN_PANEL_BACKGROUND;

            return panel;
        }

        private void CreateScrollViewComponents()
        {
            // Create scroll rect
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

        }

        public void SetOptions(List<string> options)
        {
            this.options = options.ToList();

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
            }
            else
            {
            }
        }

        private void CreateOptionButtons()
        {
            if (options == null || options.Count == 0) return;

            // Clear existing buttons
            ClearExistingOptionButtons();

            // Ensure we have the content area
            if (content == null)
            {
                return;
            }

            // Create new option buttons
            for (int i = 0; i < options.Count; i++)
            {
                CreateSingleOptionButton(i);
            }

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
            parentPanelRect.sizeDelta = new Vector2(0, dynamicOptionHeight); // Use dynamic height
            parentPanelRect.anchoredPosition = new Vector2(0, -topPadding - index * (dynamicOptionHeight + OPTION_GAP));
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

            // Create text component
            CreateOptionText(optionObj, options[index]);

            // Add click handler
            int capturedIndex = index; // Capture the index for the lambda
            optionButton.onClick.AddListener(() => OnOptionSelected(capturedIndex));

            // Apply custom colors to the option button
            ColorBlock colorBlock = optionButton.colors;
            colorBlock.normalColor = optionNormalColor;
            colorBlock.highlightedColor = optionHoverColor;
            colorBlock.pressedColor = optionPressedColor;
            optionButton.colors = colorBlock;

            // Store the parent panel reference (which contains the button)
            optionButtons.Add(parentPanel);

        }

        private void ConfigureOptionLayoutElement(LayoutElement layoutElement)
        {
            layoutElement.preferredHeight = dynamicOptionHeight;
            layoutElement.minHeight = dynamicOptionHeight;
            layoutElement.flexibleWidth = 0f;
            layoutElement.minWidth = mainButtonWidth - 21f;
            layoutElement.preferredWidth = mainButtonWidth - 21f;
        }

#pragma warning disable IDE0060 // Remove unused parameter
        private void ConfigureOptionRectTransform(RectTransform rectTransform, int index)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            // Left-align with 6px padding, fixed width, vertically centered
            rectTransform.anchorMin = new Vector2(0, 0.5f); // Left, center Y
            rectTransform.anchorMax = new Vector2(0, 0.5f); // Left, center Y
            rectTransform.pivot = new Vector2(0, 0.5f); // Left, center Y
            rectTransform.sizeDelta = new Vector2(mainButtonWidth - 21f, dynamicOptionHeight);
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
            optionText.fontSize = CalculateOptionFontSize();
            optionText.color = Color.black;
            optionText.alignment = TextAnchor.MiddleCenter;
        }

        private void ConfigureOptionButtons()
        {
            try
            {
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
                                parentPanelRect.sizeDelta = new Vector2(0, dynamicOptionHeight); // Use dynamic height

                                // Position parent panels sequentially from top to bottom with gap
                                // Start from top with padding, then position each element downward with gaps between them
                                parentPanelRect.anchoredPosition = new Vector2(0, -topPadding - i * (dynamicOptionHeight + OPTION_GAP));

                            }

                            // Debug: List all children of the parent panel
                            for (int j = 0; j < optionButtons[i].transform.childCount; j++)
                            {
                                Transform child = optionButtons[i].transform.GetChild(j);
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
                                    buttonRect.sizeDelta = new Vector2(mainButtonWidth - 21f, dynamicOptionHeight);
                                    buttonRect.anchoredPosition = new Vector2(6f, 0); // Left-aligned with 6px padding

                                }
                            }
                            else
                            {
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogWarning($"Exception in ConfigureOptionButtons for button {i}: {ex.Message}");
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
                        }
                    }
                }

            }
            catch (System.Exception ex)
            {
                Debug.LogWarning($"Exception in ConfigureOptionButtons: {ex.Message}");
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

            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ToggleDropdown();
        }

        private void ToggleDropdown()
        {
            if (isOpen)
            {
                HideDropdown();
            }
            else
            {
                OpenDropdown();
            }
        }

        private void OpenDropdown()
        {
            // Create option buttons if they don't exist
            if (optionButtons.Count == 0)
            {
                CreateOptionButtons();
            }

            // Position and show the dropdown panel
            PositionDropdownPanel();

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
            dropdownPanel?.SetActive(false);
            isOpen = false;
            
            // Remove from open dropdowns list
            openDropdowns.Remove(this);
            
        }

        private void PositionDropdownPanel()
        {
            if (dropdownPanel == null) return;

            // Get the main button's position and size
            RectTransform mainRect = GetComponent<RectTransform>();

            // Calculate panel height to match visible content height
            // Use MAX_VISIBLE_OPTIONS to limit panel height, but content will be sized for all options
            float panelHeight = Mathf.Min(options.Count, MAX_VISIBLE_OPTIONS) * dynamicOptionHeight +
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

        }

        private void ShowDropdownPanel()
        {
            if (dropdownPanel == null) return;

            RectTransform panelRect = dropdownPanel.GetComponent<RectTransform>();
            dropdownPanel.SetActive(true);

            ConfigurePanelComponents(panelRect);

            ConfigureScrollViewComponents(panelRect);

            VerifyScrollRectConnections();

            // Calculate dynamic sizing before configuring content and options
            CalculateDynamicSizing();

            ConfigureContentSizing();

            ConfigureOptionButtons();

            // Update font sizes for existing option text components
            UpdateOptionTextFontSizes();

            ResetScrollPosition();

            EnsureProperZOrder();

            LogDetailedPositions();

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

                }

                Image viewportImage = viewport.GetComponent<Image>();
                if (viewportImage != null)
                {
                    viewportImage.enabled = true;
                    viewportImage.color = new Color(0.2f, 0.2f, 0.2f, 0.9f);
                }

                Mask viewportMask = viewport.GetComponent<Mask>();
                if (viewportMask != null)
                {
                    viewportMask.enabled = true;
                    viewportMask.showMaskGraphic = false;

                }
                else
                {
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
                float contentHeight = OPTION_MARGIN + options.Count * dynamicOptionHeight + (options.Count - 1) * OPTION_GAP;

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

            }
        }

        private IEnumerator ForceContentLayoutUpdate(RectTransform contentRect)
        {
            yield return null; // Wait for next frame
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);
        }

        private void EnsureProperZOrder()
        {
            // Ensure proper z-order
            Canvas parentCanvas = dropdownPanel.GetComponentInParent<Canvas>();
            if (parentCanvas != null)
            {
                parentCanvas.sortingOrder = 1000;
            }

            scrollView?.transform.SetAsLastSibling();

            viewport?.transform.SetAsLastSibling();

            content?.transform.SetAsLastSibling();

            scrollbar?.transform.SetAsLastSibling();

            // Ensure dropdown panel is at the front
            dropdownPanel.transform.SetAsLastSibling();
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
                }
            }

            // Reset scrollbar value to top
            if (scrollbar != null)
            {
                Scrollbar scrollbarComponent = scrollbar.GetComponent<Scrollbar>();
                if (scrollbarComponent != null)
                {
                    scrollbarComponent.value = 1f; // 1 = top, 0 = bottom
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
                    }
                }

            }
        }

        private void LogDetailedPositions()
        {
        }

        private void CheckParentCanvasConfiguration()
        {
            Canvas parentCanvas = dropdownPanel.GetComponentInParent<Canvas>();
            if (parentCanvas != null)
            {
            }
        }

        // These methods are stubs for external compatibility - implement if needed
        public void SetValue(int value) 
        { 
            if (value >= 0 && value < options.Count)
            {
                selectedIndex = value;
                UpdateMainButtonText();
            }
            else
            {
            }
        }
        public void SetSize(float width) { SetSize(width, mainButtonHeight); }
        public void SetSize(float width, float height)
        {
            // If dynamic sizing is enabled, only allow height changes and preserve calculated width
            if (useDynamicSizing)
            {
                // Only set height when dynamic sizing is enabled
                // Don't override the dynamically calculated width
                SetOptionHeightFromButtonHeight(height);
            }
            else
            {
                mainButtonWidth = width;
                mainButtonHeight = height;
                SetOptionHeightFromButtonHeight(height);
            }

            // Update the main button layout
            ConfigureMainButtonLayout();

            // Update the main button text if it exists
            UpdateMainButtonText();
        }
#pragma warning disable IDE0060 // Remove unused parameter
        public void SetFontSize(int size) { /* TODO: implement if needed */ }
        
        /// <summary>
        /// Sets all button colors to the same color.
        /// </summary>
        /// <param name="color">The color to use for all states</param>
        public void SetColors(Color color)
        {
            normalColor = color;
            hoverColor = color;
            pressedColor = color;
            ApplyCustomColors();
        }

        /// <summary>
        /// Sets custom colors for normal, hover, and pressed states.
        /// </summary>
        /// <param name="normal">Normal state color</param>
        /// <param name="hover">Hover state color</param>
        /// <param name="pressed">Pressed state color</param>
        public void SetColors(Color normal, Color hover, Color pressed)
        {
            normalColor = normal;
            hoverColor = hover;
            pressedColor = pressed;
            ApplyCustomColors();
        }

        /// <summary>
        /// Sets custom colors for option button states.
        /// </summary>
        /// <param name="normal">Normal state color</param>
        /// <param name="hover">Hover state color</param>
        /// <param name="pressed">Pressed state color</param>
        public void SetOptionColors(Color normal, Color hover, Color pressed)
        {
            optionNormalColor = normal;
            optionHoverColor = hover;
            optionPressedColor = pressed;
            ApplyCustomColors();
        }

        /// <summary>
        /// Applies the custom colors to the main button and all option buttons.
        /// </summary>
        private void ApplyCustomColors()
        {
            // Apply to main button
            if (mainButton != null)
            {
                ColorBlock colorBlock = mainButton.colors;
                colorBlock.normalColor = normalColor;
                colorBlock.highlightedColor = hoverColor;
                colorBlock.pressedColor = pressedColor;
                mainButton.colors = colorBlock;
            }

            // Apply to all option buttons
            foreach (GameObject optionButton in optionButtons)
            {
                if (optionButton != null)
                {
                    Transform buttonTransform = optionButton.transform.Find($"Option_{optionButtons.IndexOf(optionButton)}");
                    if (buttonTransform != null)
                    {
                        Button button = buttonTransform.GetComponent<Button>();
                        if (button != null)
                        {
                            ColorBlock colorBlock = button.colors;
                            colorBlock.normalColor = optionNormalColor;
                            colorBlock.highlightedColor = optionHoverColor;
                            colorBlock.pressedColor = optionPressedColor;
                            button.colors = colorBlock;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Sets the border color of the dropdown button.
        /// </summary>
        /// <param name="borderColor">The color for the border</param>
        public void SetBorderColor(Color borderColor)
        {
            Transform borderTransform = transform.Find("ButtonBorder");
            if (borderTransform != null)
            {
                Image borderImage = borderTransform.GetComponent<Image>();
                if (borderImage != null)
                {
                    borderImage.color = borderColor;
                }
            }
        }
    }
}