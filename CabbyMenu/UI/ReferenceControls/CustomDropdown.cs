using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace CabbyMenu.UI.ReferenceControls
{
    public class CustomDropdown : MonoBehaviour, IPointerClickHandler
    {
        [Header("Custom Dropdown Settings")]
        [SerializeField] private Button mainButton;
        [SerializeField] private Text mainButtonText;
        [SerializeField] private Image mainButtonImage;
        [SerializeField] private GameObject dropdownPanel;
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private RectTransform contentRect;
        [SerializeField] private Button itemTemplate;

        [Header("Styling")]
        [SerializeField] private Color normalColor = Constants.DROPDOWN_NORMAL;
        [SerializeField] private Color hoverColor = Constants.DROPDOWN_HOVER;
        [SerializeField] private Color pressedColor = Constants.DROPDOWN_PRESSED;
        [SerializeField] private Color itemNormalColor = Constants.DROPDOWN_ITEM_NORMAL;
        [SerializeField] private Color itemHoverColor = Constants.DROPDOWN_ITEM_HOVER;
        [SerializeField] private Color itemPressedColor = Constants.DROPDOWN_ITEM_PRESSED;

        private GameObject scrollView;
        private GameObject viewport;
        private GameObject content;
        private List<GameObject> optionButtons = new List<GameObject>();
        private bool isOpen = false;
        private int selectedIndex = 0;
        private List<string> options = new List<string>();
        private UnityAction<int> onValueChanged;

        public System.Action<int> OnValueChanged;
        public int Value => selectedIndex;
        public List<string> Options => new List<string>(options);

        private void Awake()
        {
            InitializeBasicComponents();
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
            LogDebugState();
        }

        private void InitializeBasicComponents()
        {
            UnityEngine.Debug.Log("InitializeBasicComponents called");

            // Create main button image first (this will be the target graphic)
            if (mainButtonImage == null)
            {
                UnityEngine.Debug.Log("Creating mainButtonImage");
                mainButtonImage = gameObject.AddComponent<Image>();
                mainButtonImage.color = normalColor;
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
                GameObject textObj = new GameObject("MainButtonText");
                textObj.transform.SetParent(transform);
                RectTransform textRect = textObj.AddComponent<RectTransform>();
                textRect.anchorMin = Vector2.zero;
                textRect.anchorMax = Vector2.one;
                textRect.offsetMin = Vector2.zero;
                textRect.offsetMax = Vector2.zero;

                mainButtonText = textObj.AddComponent<Text>();
                mainButtonText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                mainButtonText.fontSize = 14;
                mainButtonText.color = Color.black;
                mainButtonText.alignment = TextAnchor.MiddleCenter;

                // Ensure text is on top of the button image
                textObj.transform.SetAsLastSibling();
            }

            // Set proper size for the main button to fit text content
            RectTransform mainRect = GetComponent<RectTransform>();
            if (mainRect != null)
            {
                // Set a reasonable width for the dropdown button (wider than a square)
                float buttonWidth = 200f; // Wide enough for dropdown text
                float buttonHeight = 60f;  // Taller for better padding
                mainRect.sizeDelta = new Vector2(buttonWidth, buttonHeight);

                // Ensure the button is properly anchored
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
            dropdownPanel = new GameObject("DropdownPanel");
            dropdownPanel.transform.SetParent(parentCanvas.transform, false);
            UnityEngine.Debug.Log($"Created dropdownPanel GameObject: {dropdownPanel.name}");

            // Configure panel rect transform
            RectTransform panelRect = dropdownPanel.AddComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0.1f, 0.6f);
            panelRect.anchorMax = new Vector2(0.1f, 0.6f);
            panelRect.sizeDelta = new Vector2(280f, 100f);
            panelRect.anchoredPosition = Vector2.zero;

            UnityEngine.Debug.Log($"Panel created as child of parent: {parentCanvas.name}");
            UnityEngine.Debug.Log($"Panel created with RectTransform: anchorMin={panelRect.anchorMin}, anchorMax={panelRect.anchorMax}, sizeDelta={panelRect.sizeDelta}");
            UnityEngine.Debug.Log($"Panel creation complete - active: {dropdownPanel.activeSelf}, parent: {dropdownPanel.transform.parent?.name}");

            // Add panel image
            Image panelImage = dropdownPanel.AddComponent<Image>();
            panelImage.color = Constants.DROPDOWN_PANEL_BACKGROUND;

            UnityEngine.Debug.Log($"Panel RectTransform - anchorMin: {panelRect.anchorMin}, anchorMax: {panelRect.anchorMax}, sizeDelta: {panelRect.sizeDelta}, anchoredPosition: {panelRect.anchoredPosition}");

            // Create scroll rect
            UnityEngine.Debug.Log("Creating scroll rect");
            GameObject scrollView = new GameObject("ScrollView");
            scrollView.transform.SetParent(dropdownPanel.transform, false);

            RectTransform scrollRect = scrollView.AddComponent<RectTransform>();
            ScrollRect scrollRectComponent = scrollView.AddComponent<ScrollRect>();

            // Configure scroll rect
            scrollRect.anchorMin = Vector2.zero;
            scrollRect.anchorMax = Vector2.one;
            scrollRect.sizeDelta = Vector2.zero;
            scrollRect.anchoredPosition = Vector2.zero;

            scrollRectComponent.horizontal = false;
            scrollRectComponent.vertical = true;
            scrollRectComponent.movementType = ScrollRect.MovementType.Clamped;
            scrollRectComponent.inertia = false;
            scrollRectComponent.scrollSensitivity = 10f;

            // Create viewport
            GameObject viewport = new GameObject("Viewport");
            viewport.transform.SetParent(scrollView.transform, false);

            RectTransform viewportRect = viewport.AddComponent<RectTransform>();
            Image viewportImage = viewport.AddComponent<Image>();
            Mask viewportMask = viewport.AddComponent<Mask>();

            // Configure viewport
            viewportRect.anchorMin = Vector2.zero;
            viewportRect.anchorMax = Vector2.one;
            viewportRect.sizeDelta = Vector2.zero;
            viewportRect.anchoredPosition = Vector2.zero;

            viewportImage.color = new Color(0.2f, 0.2f, 0.2f, 0.9f);
            viewportMask.showMaskGraphic = false;
            viewportMask.enabled = true;

            // Create content
            GameObject content = new GameObject("Content");
            content.transform.SetParent(viewport.transform, false);

            RectTransform contentRect = content.AddComponent<RectTransform>();
            VerticalLayoutGroup contentLayout = content.AddComponent<VerticalLayoutGroup>();
            ContentSizeFitter contentFitter = content.AddComponent<ContentSizeFitter>();

            // Configure content - anchor to stretch across viewport width and position at top
            contentRect.anchorMin = new Vector2(0, 1);
            contentRect.anchorMax = new Vector2(1, 1);
            contentRect.sizeDelta = new Vector2(0, 0);
            contentRect.anchoredPosition = Vector2.zero;

            // Configure layout group for proper button positioning
            contentLayout.enabled = false;
            contentLayout.spacing = 0f;
            contentLayout.padding = new RectOffset(0, 0, 0, 0);
            contentLayout.childAlignment = TextAnchor.UpperCenter;
            contentLayout.childControlWidth = true;
            contentLayout.childControlHeight = false;
            contentLayout.childForceExpandWidth = true;
            contentLayout.childForceExpandHeight = false;

            // Configure content size fitter
            contentFitter.enabled = false;
            contentFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            contentFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;

            UnityEngine.Debug.Log("Layout system disabled - using manual positioning");

            // Connect scroll rect components
            scrollRectComponent.viewport = viewportRect;
            scrollRectComponent.content = contentRect;

            // Store references
            this.scrollView = scrollView;
            this.viewport = viewport;
            this.content = content;

            UnityEngine.Debug.Log($"Scroll view created - viewport: {viewport.name}, content: {content.name}");

            // Create item template
            UnityEngine.Debug.Log("Creating item template");
            GameObject itemTemplate = new GameObject("ItemTemplate");
            itemTemplate.transform.SetParent(content.transform, false);
            itemTemplate.SetActive(false);

            RectTransform templateRect = itemTemplate.AddComponent<RectTransform>();
            Image templateImage = itemTemplate.AddComponent<Image>();
            Button templateButton = itemTemplate.AddComponent<Button>();
            LayoutElement templateLayout = itemTemplate.AddComponent<LayoutElement>();

            // Configure template
            templateRect.anchorMin = Vector2.zero;
            templateRect.anchorMax = Vector2.one;
            templateRect.sizeDelta = Vector2.zero;
            templateRect.anchoredPosition = Vector2.zero;

            templateImage.color = new Color(0.2f, 0.2f, 0.2f, 1f);
            templateLayout.preferredHeight = 30f;
            templateLayout.minHeight = 30f;
            templateLayout.preferredWidth = 200f;
            templateLayout.minWidth = 200f;
            templateLayout.flexibleWidth = 0f;

            // Create text for template
            GameObject templateText = new GameObject("Text");
            templateText.transform.SetParent(itemTemplate.transform, false);

            RectTransform textRect = templateText.AddComponent<RectTransform>();
            Text textComponent = templateText.AddComponent<Text>();

            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;
            textRect.anchoredPosition = Vector2.zero;

            textComponent.text = "Option";
            textComponent.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            textComponent.fontSize = 14;
            textComponent.color = Color.black;
            textComponent.alignment = TextAnchor.MiddleCenter;

            UnityEngine.Debug.Log($"Item template created successfully - active: {itemTemplate.activeSelf}, has Image: {templateImage != null}, has Text: {textComponent != null}, has LayoutElement: {templateLayout != null}, sizeDelta: {templateRect.sizeDelta}");

            // Initially hide the dropdown panel
            dropdownPanel.SetActive(false);

            UnityEngine.Debug.Log($"Dropdown panel initially hidden - active: {dropdownPanel.activeSelf}");
            UnityEngine.Debug.Log($"Content reference set: {content != null}, name: {content?.name}");
            UnityEngine.Debug.Log("EnsureDropdownPanelCreated completed successfully");
        }

        public void SetOptions(List<string> options)
        {
            this.options = new List<string>(options);
            UnityEngine.Debug.Log($"SetOptions called with {options.Count} options");

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
                int fontSize = Mathf.RoundToInt(mainRect.sizeDelta.y * 0.6f);
                mainButtonText.fontSize = fontSize;
                mainButtonText.color = Color.black; // Dark text for better contrast

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
            foreach (var button in optionButtons)
            {
                if (button != null && button.gameObject != null)
                {
                    DestroyImmediate(button.gameObject);
                }
            }
            optionButtons.Clear();

            // Ensure we have the content area
            if (content == null)
            {
                UnityEngine.Debug.LogError("Content area is null - cannot create option buttons");
                return;
            }

            // Create new option buttons
            for (int i = 0; i < options.Count; i++)
            {
                // Create option button from scratch since we don't have a template
                GameObject optionObj = new GameObject($"Option_{i}");
                optionObj.transform.SetParent(content.transform, false);
                optionObj.SetActive(true);

                // Add required components
                RectTransform optionRect = optionObj.AddComponent<RectTransform>();
                Image optionImage = optionObj.AddComponent<Image>();
                Button optionButton = optionObj.AddComponent<Button>();
                LayoutElement optionLayout = optionObj.AddComponent<LayoutElement>();

                // Configure layout element for proper sizing
                optionLayout.preferredHeight = 30f;
                optionLayout.minHeight = 30f;
                optionLayout.flexibleWidth = 0f;
                optionLayout.minWidth = 0f;
                optionLayout.preferredWidth = 180f; // Fixed width for option buttons

                // Configure rect transform - let layout system handle positioning
                optionRect.anchorMin = new Vector2(0, 1);
                optionRect.anchorMax = new Vector2(1, 1);
                optionRect.sizeDelta = new Vector2(-20f, 30f); // 10px margin on each side (170px wide)
                optionRect.anchoredPosition = new Vector2(0, -i * 30f);

                // Configure image
                optionImage.color = Constants.DROPDOWN_OPTION_BACKGROUND;
                UnityEngine.Debug.Log($"Set option {i} button color to: {optionImage.color}");

                // Create text component
                GameObject textObj = new GameObject("Text");
                textObj.transform.SetParent(optionObj.transform, false);

                RectTransform textRect = textObj.AddComponent<RectTransform>();
                Text optionText = textObj.AddComponent<Text>();

                textRect.anchorMin = Vector2.zero;
                textRect.anchorMax = Vector2.one;
                textRect.sizeDelta = Vector2.zero;
                textRect.anchoredPosition = Vector2.zero;

                optionText.text = options[i];
                optionText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                optionText.fontSize = 14;
                optionText.color = Color.black;
                optionText.alignment = TextAnchor.MiddleCenter;

                // Add click handler
                int index = i; // Capture the index for the lambda
                optionButton.onClick.AddListener(() => OnOptionSelected(index));

                // Store the button reference
                optionButtons.Add(optionObj);

                UnityEngine.Debug.Log($"Created option button {i}: '{options[i]}' with proper layout configuration");
            }

            UnityEngine.Debug.Log($"Created {optionButtons.Count} option buttons");
        }

        private void OnOptionSelected(int index)
        {
            if (index >= 0 && index < options.Count)
            {
                selectedIndex = index;

                // Update the main button text
                UpdateMainButtonText();

                // Hide the dropdown
                HideDropdown();

                // Notify listeners
                OnValueChanged?.Invoke(selectedIndex);

                UnityEngine.Debug.Log($"Option selected: {options[selectedIndex]} (index: {selectedIndex})");
            }
        }

        private void OnPointerClick()
        {
            UnityEngine.Debug.Log("OnPointerClick called");
            ToggleDropdown();
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
            ShowDropdownPanel();

            isOpen = true;
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
            UnityEngine.Debug.Log($"Dropdown state - isOpen: {isOpen}");
        }

        private void PositionDropdownPanel()
        {
            if (dropdownPanel == null) return;

            // Get the main button's position and size
            RectTransform mainRect = GetComponent<RectTransform>();
            Vector3[] mainCorners = new Vector3[4];
            mainRect.GetWorldCorners(mainCorners);

            // Position the dropdown panel below the main button
            RectTransform panelRect = dropdownPanel.GetComponent<RectTransform>();

            // Match the main button's anchor and position exactly
            panelRect.anchorMin = mainRect.anchorMin;
            panelRect.anchorMax = mainRect.anchorMax;
            panelRect.pivot = new Vector2(0.5f, 1f); // Pivot at top center

            // Calculate panel size based on number of options
            float panelWidth = mainRect.sizeDelta.x; // Match main button width exactly
            float panelHeight = Mathf.Min(options.Count * 30f + 10f, 8 * 30f + 10f); // Max 8 options visible, 30px each + padding

            panelRect.sizeDelta = new Vector2(panelWidth, panelHeight);
            panelRect.anchoredPosition = new Vector2(0, -mainRect.sizeDelta.y - 2f); // 2px gap below button

            UnityEngine.Debug.Log($"Panel positioning - anchorMin: {panelRect.anchorMin}, anchorMax: {panelRect.anchorMax}, pivot: {panelRect.pivot}");
            UnityEngine.Debug.Log($"Panel final - anchoredPosition: {panelRect.anchoredPosition}, sizeDelta: {panelRect.sizeDelta}");
            UnityEngine.Debug.Log($"Panel matches main button width: {panelWidth}, height for {Mathf.Min(options.Count, 8)} options: {panelHeight}");
        }

        private void ShowDropdownPanel()
        {
            if (dropdownPanel == null) return;

            // Get panel rect transform
            RectTransform panelRect = dropdownPanel.GetComponent<RectTransform>();

            // Ensure the panel is active
            dropdownPanel.SetActive(true);

            // Ensure panel image is visible
            Image panelImage = dropdownPanel.GetComponent<Image>();
            if (panelImage != null)
            {
                panelImage.enabled = true;
                panelImage.color = Constants.DROPDOWN_PANEL_BACKGROUND;
                UnityEngine.Debug.Log($"Panel image enabled with color: {panelImage.color}");
            }

            // Ensure scroll view is properly configured
            if (scrollView != null)
            {
                ScrollRect scrollRectComponent = scrollView.GetComponent<ScrollRect>();
                if (scrollRectComponent != null)
                {
                    scrollRectComponent.enabled = true;
                    UnityEngine.Debug.Log($"Scroll view state - enabled: {scrollRectComponent.enabled}, viewport: {scrollRectComponent.viewport?.name}, content: {scrollRectComponent.content?.name}");
                }
            }

            // Configure viewport properly FIRST
            if (viewport != null)
            {
                RectTransform viewportRect = viewport.GetComponent<RectTransform>();
                if (viewportRect != null)
                {
                    // Set viewport to fill the panel with small margins
                    viewportRect.sizeDelta = new Vector2(panelRect.sizeDelta.x - 10f, panelRect.sizeDelta.y - 10f);
                    viewportRect.anchoredPosition = Vector2.zero;

                    UnityEngine.Debug.Log($"Viewport state - active: {viewport.activeSelf}, sizeDelta: {viewportRect.sizeDelta}, anchoredPosition: {viewportRect.anchoredPosition}");
                    UnityEngine.Debug.Log($"Set viewport sizeDelta to: {viewportRect.sizeDelta}");
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
                    UnityEngine.Debug.Log($"Viewport mask - enabled: {viewportMask.enabled}, showMaskGraphic: {viewportMask.showMaskGraphic}");
                }
            }

            // Configure content sizing
            if (content != null)
            {
                RectTransform contentRect = content.GetComponent<RectTransform>();
                if (contentRect != null)
                {
                    // Force content to have proper size for the options
                    float contentHeight = options.Count * 30f;
                    float contentWidth = 190f; // Match viewport width exactly
                    contentRect.sizeDelta = new Vector2(contentWidth, contentHeight);

                    // Ensure content is positioned at the top of the viewport
                    contentRect.anchoredPosition = Vector2.zero;

                    UnityEngine.Debug.Log($"Content sizing - sizeDelta: {contentRect.sizeDelta}, anchorMin: {contentRect.anchorMin}, anchorMax: {contentRect.anchorMax}");
                    UnityEngine.Debug.Log($"Content height for {options.Count} options: {contentHeight}, width: {contentWidth}");
                    UnityEngine.Debug.Log($"Content anchoredPosition: {contentRect.anchoredPosition}");
                }
            }

            // Ensure all option buttons are visible
            for (int i = 0; i < optionButtons.Count; i++)
            {
                if (optionButtons[i] != null)
                {
                    optionButtons[i].SetActive(true);

                    // Ensure button is properly positioned
                    RectTransform buttonRect = optionButtons[i].GetComponent<RectTransform>();
                    if (buttonRect != null)
                    {
                        buttonRect.anchoredPosition = new Vector2(0, -i * 30f);
                    }

                    Image buttonImage = optionButtons[i].GetComponent<Image>();
                    if (buttonImage != null)
                    {
                        buttonImage.enabled = true;
                        buttonImage.color = Constants.DROPDOWN_OPTION_BACKGROUND;
                        UnityEngine.Debug.Log($"Button Option_{i} - active: {optionButtons[i].activeSelf}, image enabled: {buttonImage.enabled}, color: {buttonImage.color}, alpha: {buttonImage.color.a}");
                    }
                }
            }

            // Log option button configurations
            for (int i = 0; i < Mathf.Min(optionButtons.Count, 3); i++) // Log first 3 buttons
            {
                if (optionButtons[i] != null)
                {
                    RectTransform buttonRect = optionButtons[i].GetComponent<RectTransform>();
                    if (buttonRect != null)
                    {
                        UnityEngine.Debug.Log($"Option button Option_{i} - anchorMin: {buttonRect.anchorMin}, anchorMax: {buttonRect.anchorMax}, sizeDelta: {buttonRect.sizeDelta}");
                    }
                }
            }

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

            // Ensure dropdown panel is at the front
            dropdownPanel.transform.SetAsLastSibling();
            UnityEngine.Debug.Log($"Dropdown panel visibility ensured - active: {dropdownPanel.activeSelf}, sibling index: {dropdownPanel.transform.GetSiblingIndex()}");

            // Log detailed position analysis
            LogDetailedPositions();

            UnityEngine.Debug.Log($"Dropdown panel activated and positioned. Panel height: {panelRect.sizeDelta.y}, AnchoredPosition: {panelRect.anchoredPosition}, SizeDelta: {panelRect.sizeDelta}");
        }

        private void LogDetailedPositions()
        {
            UnityEngine.Debug.Log("=== DETAILED POSITION ANALYSIS ===");

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
            if (scrollRect != null)
            {
                RectTransform scrollRectTransform = scrollRect.GetComponent<RectTransform>();
                Vector3 scrollWorldPos = scrollRectTransform.position;
                Vector3 scrollScreenPos = RectTransformUtility.WorldToScreenPoint(null, scrollWorldPos);
                UnityEngine.Debug.Log($"ScrollView - World: {scrollWorldPos}, Screen: {scrollScreenPos}, Local: {scrollRectTransform.localPosition}");
                UnityEngine.Debug.Log($"ScrollView - sizeDelta: {scrollRectTransform.sizeDelta}, anchoredPosition: {scrollRectTransform.anchoredPosition}");
            }

            // Viewport positions
            if (scrollRect != null && scrollRect.viewport != null)
            {
                RectTransform viewportRect = scrollRect.viewport;
                Vector3 viewportWorldPos = viewportRect.position;
                Vector3 viewportScreenPos = RectTransformUtility.WorldToScreenPoint(null, viewportWorldPos);
                UnityEngine.Debug.Log($"Viewport - World: {viewportWorldPos}, Screen: {viewportScreenPos}, Local: {viewportRect.localPosition}");
                UnityEngine.Debug.Log($"Viewport - sizeDelta: {viewportRect.sizeDelta}, anchoredPosition: {viewportRect.anchoredPosition}");
            }

            // Content positions
            if (scrollRect != null && scrollRect.content != null)
            {
                RectTransform contentRect = scrollRect.content;
                Vector3 contentWorldPos = contentRect.position;
                Vector3 contentScreenPos = RectTransformUtility.WorldToScreenPoint(null, contentWorldPos);
                UnityEngine.Debug.Log($"Content - World: {contentWorldPos}, Screen: {contentScreenPos}, Local: {contentRect.localPosition}");
                UnityEngine.Debug.Log($"Content - sizeDelta: {contentRect.sizeDelta}, anchoredPosition: {contentRect.anchoredPosition}");
            }

            // Option button positions (first few)
            for (int i = 0; i < Mathf.Min(3, optionButtons.Count); i++)
            {
                if (optionButtons[i] != null)
                {
                    RectTransform buttonRect = optionButtons[i].GetComponent<RectTransform>();
                    Vector3 buttonWorldPos = buttonRect.position;
                    Vector3 buttonScreenPos = RectTransformUtility.WorldToScreenPoint(null, buttonWorldPos);
                    UnityEngine.Debug.Log($"OptionButton_{i} - World: {buttonWorldPos}, Screen: {buttonScreenPos}, Local: {buttonRect.localPosition}");
                    UnityEngine.Debug.Log($"OptionButton_{i} - sizeDelta: {buttonRect.sizeDelta}, anchoredPosition: {buttonRect.anchoredPosition}");
                }
            }

            // Check for overlap issues
            UnityEngine.Debug.Log("=== OVERLAP ANALYSIS ===");
            if (scrollRect != null && scrollRect.viewport != null && scrollRect.content != null)
            {
                RectTransform viewportRect = scrollRect.viewport;
                RectTransform contentRect = scrollRect.content;

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
            }

            UnityEngine.Debug.Log("=== END POSITION ANALYSIS ===");
        }

        private void CheckPanelVisibility()
        {
            if (dropdownPanel == null) return;

            RectTransform panelRect = dropdownPanel.GetComponent<RectTransform>();
            if (panelRect == null) return;

            // Get world corners
            Vector3[] corners = new Vector3[4];
            panelRect.GetWorldCorners(corners);

            UnityEngine.Debug.Log($"Panel world corners: {corners[0]}, {corners[1]}, {corners[2]}, {corners[3]}");

            // Check if any corner is on screen
            bool anyCornerOnScreen = false;
            for (int i = 0; i < 4; i++)
            {
                Vector3 screenPoint = Camera.main != null ? Camera.main.WorldToScreenPoint(corners[i]) : corners[i];
                if (screenPoint.x >= 0 && screenPoint.x <= Screen.width &&
                    screenPoint.y >= 0 && screenPoint.y <= Screen.height)
                {
                    anyCornerOnScreen = true;
                    break;
                }
            }

            UnityEngine.Debug.Log($"Panel visibility check - any corner on screen: {anyCornerOnScreen}");

            // Check panel's local position and size
            UnityEngine.Debug.Log($"Panel local position: {dropdownPanel.transform.localPosition}");
            UnityEngine.Debug.Log($"Panel local scale: {dropdownPanel.transform.localScale}");
            UnityEngine.Debug.Log($"Panel sizeDelta: {panelRect.sizeDelta}");
            UnityEngine.Debug.Log($"Panel anchoredPosition: {panelRect.anchoredPosition}");

            // Check if panel is active in hierarchy
            UnityEngine.Debug.Log($"Panel active in hierarchy: {dropdownPanel.activeInHierarchy}");
            UnityEngine.Debug.Log($"Panel active self: {dropdownPanel.activeSelf}");

            // Check canvas group alpha if it exists
            CanvasGroup canvasGroup = dropdownPanel.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                UnityEngine.Debug.Log($"Canvas group alpha: {canvasGroup.alpha}, interactable: {canvasGroup.interactable}, blocksRaycasts: {canvasGroup.blocksRaycasts}");
            }
        }

        private void ForceContentSizing()
        {
            if (contentRect == null) return;

            RectTransform contentRectTransform = contentRect.GetComponent<RectTransform>();
            if (contentRectTransform == null) return;

            // Calculate content size based on number of options
            float optionHeight = 30f;
            float contentHeight = options.Count * optionHeight;

            // Get panel width to match content width
            RectTransform panelRect = dropdownPanel.GetComponent<RectTransform>();
            float contentWidth = panelRect.sizeDelta.x;

            contentRectTransform.sizeDelta = new Vector2(contentWidth, contentHeight);
            contentRectTransform.anchorMin = new Vector2(0, 1);
            contentRectTransform.anchorMax = new Vector2(1, 1);
            contentRectTransform.pivot = new Vector2(0.5f, 1f);
            contentRectTransform.anchoredPosition = Vector2.zero;

            // Don't force local position - let the viewport handle content positioning

            UnityEngine.Debug.Log($"Content sizing - sizeDelta: {contentRectTransform.sizeDelta}, anchorMin: {contentRectTransform.anchorMin}, anchorMax: {contentRectTransform.anchorMax}");
            UnityEngine.Debug.Log($"Content height for {options.Count} options: {contentHeight}, width: {contentWidth}");
        }

        private void LogZAndScreenPositions()
        {
            // Main button
            RectTransform mainRect = GetComponent<RectTransform>();
            Vector3 mainWorld = mainRect.position;
            Vector3 mainScreen = RectTransformUtility.WorldToScreenPoint(null, mainWorld);
            UnityEngine.Debug.Log($"MainButton z: {mainWorld.z}, screen: {mainScreen}");

            // Main button text
            if (mainButtonText != null)
            {
                RectTransform textRect = mainButtonText.GetComponent<RectTransform>();
                Vector3 textWorld = textRect.position;
                Vector3 textScreen = RectTransformUtility.WorldToScreenPoint(null, textWorld);
                UnityEngine.Debug.Log($"MainButtonText z: {textWorld.z}, screen: {textScreen}");
            }

            // Dropdown panel
            if (dropdownPanel != null)
            {
                RectTransform panelRect = dropdownPanel.GetComponent<RectTransform>();
                Vector3 panelWorld = panelRect.position;
                Vector3 panelScreen = RectTransformUtility.WorldToScreenPoint(null, panelWorld);
                UnityEngine.Debug.Log($"DropdownPanel z: {panelWorld.z}, screen: {panelScreen}");
            }

            // Scroll view
            if (scrollRect != null)
            {
                RectTransform scrollRectT = scrollRect.GetComponent<RectTransform>();
                Vector3 scrollWorld = scrollRectT.position;
                Vector3 scrollScreen = RectTransformUtility.WorldToScreenPoint(null, scrollWorld);
                UnityEngine.Debug.Log($"ScrollView z: {scrollWorld.z}, screen: {scrollScreen}");
            }

            // Viewport
            if (scrollRect != null && scrollRect.viewport != null)
            {
                RectTransform viewportRect = scrollRect.viewport;
                Vector3 viewportWorld = viewportRect.position;
                Vector3 viewportScreen = RectTransformUtility.WorldToScreenPoint(null, viewportWorld);
                UnityEngine.Debug.Log($"Viewport z: {viewportWorld.z}, screen: {viewportScreen}");
            }

            // Content
            if (contentRect != null)
            {
                Vector3 contentWorld = contentRect.position;
                Vector3 contentScreen = RectTransformUtility.WorldToScreenPoint(null, contentWorld);
                UnityEngine.Debug.Log($"Content z: {contentWorld.z}, screen: {contentScreen}");
            }
        }

        private void LogDebugState()
        {
        }

        // Compatibility methods for external calls
        public void SetValue(int value) { /* TODO: implement if needed */ }
        public void SetSize(float width) { /* TODO: implement if needed */ }
        public void SetSize(float width, float height) { /* TODO: implement if needed */ }
        public void SetFontSize(int size) { /* TODO: implement if needed */ }
        public void SetColors(Color color) { /* TODO: implement if needed */ }
        public void SetColors(Color normal, Color hover, Color pressed) { /* TODO: implement if needed */ }
    }
}