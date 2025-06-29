using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace CabbyMenu.UI.ReferenceControls
{
    public class CustomDropdown : MonoBehaviour, IPointerClickHandler
    {
        #region Constants
        private const float OPTION_HEIGHT = 30f;
        private const float OPTION_MARGIN = 10f;
        private const float OPTION_GAP = 3f; // Gap between option buttons
        private const float PANEL_MARGIN = 20f;
        private const float DEFAULT_FONT_SIZE = 14f;
        private const float FONT_SIZE_RATIO = 0.6f;
        private const float BUTTON_GAP = 2f;
        private const int MAX_VISIBLE_OPTIONS = 8;
        private const float SCROLLBAR_WIDTH = 10f; // Width of the scrollbar
        #endregion

        // Instance variables for customizable size
        private float mainButtonWidth = 200f;
        private float mainButtonHeight = 60f;

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
        private List<GameObject> optionButtons = new List<GameObject>();
        private bool isOpen = false;
        private int selectedIndex = 0;

        // Events - Using UnityEvent for consistency
        public UnityEvent<int> onValueChanged = new UnityEvent<int>();

        // Public properties
        public int Value => selectedIndex;
        public List<string> Options => new List<string>(options);

        #region Unity Lifecycle Methods

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
        }

        #endregion

        #region Initialization Methods

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

            // Configure panel rect transform
            RectTransform panelRect = panel.AddComponent<RectTransform>();
            // Align to top center of parent canvas
            panelRect.anchorMin = new Vector2(0.5f, 1f);
            panelRect.anchorMax = new Vector2(0.5f, 1f);
            panelRect.sizeDelta = new Vector2(280f, 100f);
            panelRect.anchoredPosition = new Vector2(0, 0); // Will be positioned below button later
            panelRect.pivot = new Vector2(0.5f, 1f); // Top-center pivot for proper dropdown positioning

            UnityEngine.Debug.Log($"Panel created as child of parent: {parentCanvas.name}");
            UnityEngine.Debug.Log($"Panel created with RectTransform: anchorMin={panelRect.anchorMin}, anchorMax={panelRect.anchorMax}, sizeDelta={panelRect.sizeDelta}");
            UnityEngine.Debug.Log($"Panel creation complete - active: {panel.activeSelf}, parent: {panel.transform.parent?.name}");

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

            // Configure viewport - leave space for scrollbar on the right
            viewportRect.anchorMin = Vector2.zero;
            viewportRect.anchorMax = Vector2.one;
            viewportRect.pivot = new Vector2(0.5f, 0.5f);
            viewportRect.sizeDelta = new Vector2(-SCROLLBAR_WIDTH, 0); // Leave space for scrollbar
            viewportRect.anchoredPosition = Vector2.zero;

            viewportImage.color = new Color(0.2f, 0.2f, 0.2f, 0.9f);
            viewportMask.showMaskGraphic = false;
            viewportMask.enabled = true;

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

            // Configure scrollbar
            scrollbarRect.anchorMin = new Vector2(1, 0);
            scrollbarRect.anchorMax = new Vector2(1, 1);
            scrollbarRect.sizeDelta = new Vector2(SCROLLBAR_WIDTH, 0); // Use constant for width
            scrollbarRect.anchoredPosition = Vector2.zero;

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

        #endregion

        #region Public Interface Methods

        public void SetOptions(List<string> options)
        {
            this.options = new List<string>(options);
            UnityEngine.Debug.Log($"SetOptions called with {options.Count} options");

            // Update the main button text to show the first option
            UpdateMainButtonText();
        }

        #endregion

        #region UI Update Methods

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

        #endregion

        #region Option Button Management

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

            // Configure parent panel - stretch across full width, same height as button
            parentPanelRect.anchorMin = new Vector2(0, 1);
            parentPanelRect.anchorMax = new Vector2(1, 1);
            parentPanelRect.sizeDelta = new Vector2(0, OPTION_HEIGHT); // Stretch across width, fixed height
            parentPanelRect.anchoredPosition = new Vector2(0, -OPTION_GAP - index * (OPTION_HEIGHT + OPTION_GAP));
            parentPanelRect.pivot = new Vector2(0.5f, 1f); // Center X, Top Y

            // Configure parent panel image (transparent background)
            parentPanelImage.color = new Color(0, 0, 0, 0); // Transparent

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
            layoutElement.minWidth = mainButtonWidth - 15f; // 185px
            layoutElement.preferredWidth = mainButtonWidth - 15f; // 185px
        }

        private void ConfigureOptionRectTransform(RectTransform rectTransform, int index)
        {
            // Left-align, fixed width, vertically centered
            rectTransform.anchorMin = new Vector2(0, 0.5f); // Left, center Y
            rectTransform.anchorMax = new Vector2(0, 0.5f); // Left, center Y
            rectTransform.pivot = new Vector2(0, 0.5f); // Left, center Y
            rectTransform.sizeDelta = new Vector2(mainButtonWidth - 15f, OPTION_HEIGHT); // 185px width, 30px height
            rectTransform.anchoredPosition = new Vector2(0, 0); // Left-aligned within parent
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

                // Ensure all option buttons are visible and properly positioned
                for (int i = 0; i < optionButtons.Count; i++)
                {
                    try
                    {
                        if (optionButtons[i] != null)
                        {
                            optionButtons[i].SetActive(true);

                            // Configure parent panel to stretch across content width
                            RectTransform parentPanelRect = optionButtons[i].GetComponent<RectTransform>();
                            if (parentPanelRect != null)
                            {
                                // Configure parent panel to stretch across content width
                                parentPanelRect.anchorMin = new Vector2(0, 1);
                                parentPanelRect.anchorMax = new Vector2(1, 1);
                                parentPanelRect.sizeDelta = new Vector2(0, OPTION_HEIGHT); // Stretch across width, fixed height

                                // Position parent panels sequentially from top to bottom with gap
                                // Start with OPTION_GAP at the top, then position each element with gaps between them
                                parentPanelRect.anchoredPosition = new Vector2(0, -OPTION_GAP - i * (OPTION_HEIGHT + OPTION_GAP));

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
                                    // Configure button to be left-aligned within parent panel with 185px width
                                    buttonRect.anchorMin = new Vector2(0, 0.5f); // Left, center Y
                                    buttonRect.anchorMax = new Vector2(0, 0.5f); // Left, center Y
                                    buttonRect.pivot = new Vector2(0, 0.5f); // Left, center Y
                                    buttonRect.sizeDelta = new Vector2(mainButtonWidth - 15f, OPTION_HEIGHT); // 185px width, full height
                                    buttonRect.anchoredPosition = new Vector2(0, 0); // Left-aligned within parent

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

        #endregion

        #region Event Handling

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

        #endregion

        #region Dropdown State Management

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

        #endregion

        #region Panel Positioning and Display

        private void PositionDropdownPanel()
        {
            if (dropdownPanel == null) return;

            // Get the main button's position and size
            RectTransform mainRect = GetComponent<RectTransform>();

            // Calculate panel height including gaps
            // Add OPTION_GAP at the top and bottom to match gaps between elements
            float totalOptionHeight = Mathf.Min(options.Count, MAX_VISIBLE_OPTIONS) * OPTION_HEIGHT;
            float totalGapHeight = Mathf.Min(options.Count - 1, MAX_VISIBLE_OPTIONS - 1) * OPTION_GAP;
            float panelHeight = totalOptionHeight + totalGapHeight + OPTION_MARGIN + 2 * OPTION_GAP;

            // Match the main button's anchor, pivot, and width
            // The parent panels stretch across the full width, and buttons are left-aligned within them
            RectTransform panelRect = dropdownPanel.GetComponent<RectTransform>();
            panelRect.anchorMin = mainRect.anchorMin;
            panelRect.anchorMax = mainRect.anchorMax;
            panelRect.pivot = new Vector2(0.5f, 1f); // Top center
            panelRect.sizeDelta = new Vector2(mainRect.sizeDelta.x, panelHeight); // Same width as button
            panelRect.anchoredPosition = new Vector2(0, -mainRect.sizeDelta.y); // Directly below

            // Move the panel to the correct world position
            dropdownPanel.transform.position = mainRect.TransformPoint(new Vector3(0, -mainRect.sizeDelta.y, 0));

            UnityEngine.Debug.Log($"[FIXED] Panel positioning - anchorMin: {panelRect.anchorMin}, anchorMax: {panelRect.anchorMax}, pivot: {panelRect.pivot}");
            UnityEngine.Debug.Log($"[FIXED] Panel final - anchoredPosition: {panelRect.anchoredPosition}, sizeDelta: {panelRect.sizeDelta}");
            UnityEngine.Debug.Log($"[FIXED] Panel matches main button width: {mainRect.sizeDelta.x}, height for {Mathf.Min(options.Count, MAX_VISIBLE_OPTIONS)} options: {panelRect.sizeDelta.y}");
        }

        private void ShowDropdownPanel()
        {
            if (dropdownPanel == null) return;

            UnityEngine.Debug.Log("=== ShowDropdownPanel started ===");

            // Get panel rect transform
            RectTransform panelRect = dropdownPanel.GetComponent<RectTransform>();

            // Ensure the panel is active
            dropdownPanel.SetActive(true);

            UnityEngine.Debug.Log("About to call ConfigurePanelComponents...");
            // Configure panel components
            ConfigurePanelComponents(panelRect);

            UnityEngine.Debug.Log("About to call ConfigureScrollViewComponents...");
            // Configure scroll view components with proper positioning
            ConfigureScrollViewComponents(panelRect);

            UnityEngine.Debug.Log("About to call VerifyScrollRectConnections...");
            // Verify and fix scroll rect connections
            VerifyScrollRectConnections();

            UnityEngine.Debug.Log("About to call ConfigureContentSizing...");
            // Configure content sizing and positioning
            ConfigureContentSizing();

            UnityEngine.Debug.Log("About to call ConfigureOptionButtons...");

            // Ensure all option buttons are visible and properly positioned
            ConfigureOptionButtons();

            UnityEngine.Debug.Log("About to call ResetScrollPosition...");
            // Reset scroll position to top
            ResetScrollPosition();

            UnityEngine.Debug.Log("About to call EnsureProperZOrder...");
            // Ensure proper z-order
            EnsureProperZOrder();

            UnityEngine.Debug.Log("About to call LogDetailedPositions...");
            // Log detailed position analysis
            LogDetailedPositions();

            UnityEngine.Debug.Log($"[FIXED] Dropdown panel activated and positioned. Panel height: {panelRect.sizeDelta.y}, AnchoredPosition: {panelRect.anchoredPosition}, SizeDelta: {panelRect.sizeDelta}");
        }

        private void ConfigurePanelComponents(RectTransform panelRect)
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

        private void ConfigureScrollViewComponents(RectTransform panelRect)
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
            }

            // Configure viewport properly - fill the entire panel
            if (viewport != null)
            {
                RectTransform viewportRect = viewport.GetComponent<RectTransform>();
                if (viewportRect != null)
                {
                    // Set viewport to fill the panel but leave space for scrollbar
                    viewportRect.anchorMin = Vector2.zero;
                    viewportRect.anchorMax = Vector2.one;
                    viewportRect.sizeDelta = new Vector2(-SCROLLBAR_WIDTH, 0); // Leave space for scrollbar
                    viewportRect.anchoredPosition = Vector2.zero;

                    UnityEngine.Debug.Log($"Viewport configured - anchorMin: {viewportRect.anchorMin}, anchorMax: {viewportRect.anchorMax}, sizeDelta: {viewportRect.sizeDelta}, anchoredPosition: {viewportRect.anchoredPosition}");
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
                    scrollbarRect.sizeDelta = new Vector2(SCROLLBAR_WIDTH, 0); // Use constant for width
                    scrollbarRect.anchoredPosition = Vector2.zero;

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
                // Calculate content size based on number of options plus gaps
                // Add OPTION_GAP at the top and bottom to match gaps between elements
                float contentHeight = options.Count * OPTION_HEIGHT + (options.Count - 1) * OPTION_GAP + 2 * OPTION_GAP;

                // Configure content to stretch across viewport width and position at top
                contentRect.anchorMin = new Vector2(0, 1);
                contentRect.anchorMax = new Vector2(1, 1);
                contentRect.pivot = new Vector2(0.5f, 1f);
                contentRect.sizeDelta = new Vector2(0, contentHeight);

                // Position content at the top of the viewport
                contentRect.anchoredPosition = Vector2.zero;

                UnityEngine.Debug.Log($"Content sizing - sizeDelta: {contentRect.sizeDelta}, anchorMin: {contentRect.anchorMin}, anchorMax: {contentRect.anchorMax}");
                UnityEngine.Debug.Log($"Content height for {options.Count} options: {contentHeight}");
                UnityEngine.Debug.Log($"Content anchoredPosition: {contentRect.anchoredPosition}");

                // Force layout update to ensure proper positioning
                LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);
            }
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

        #endregion

        #region Debug and Logging Methods

        private void LogDetailedPositions()
        {
            UnityEngine.Debug.Log("=== DETAILED POSITION ANALYSIS ===");

            // Check parent canvas configuration
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
                ScrollRect scrollRectComponent = scrollView.GetComponent<ScrollRect>();
                RectTransform viewportRect = viewport.GetComponent<RectTransform>();
                Vector3 viewportWorldPos = viewportRect.position;
                Vector3 viewportScreenPos = RectTransformUtility.WorldToScreenPoint(null, viewportWorldPos);
                UnityEngine.Debug.Log($"Viewport - World: {viewportWorldPos}, Screen: {viewportScreenPos}, Local: {viewportRect.localPosition}");
                UnityEngine.Debug.Log($"Viewport - sizeDelta: {viewportRect.sizeDelta}, anchoredPosition: {viewportRect.anchoredPosition}");
            }

            // Content positions
            if (scrollView != null && content != null)
            {
                ScrollRect scrollRectComponent = scrollView.GetComponent<ScrollRect>();
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

        #endregion

        #region Compatibility Methods (TODO: Implement if needed)

        // These methods are stubs for external compatibility - implement if needed
        public void SetValue(int value) { /* TODO: implement if needed */ }
        public void SetSize(float width) { SetSize(width, mainButtonHeight); }
        public void SetSize(float width, float height)
        {
            mainButtonWidth = width;
            mainButtonHeight = height;

            // Update the main button layout
            ConfigureMainButtonLayout();

            // Update the main button text if it exists
            UpdateMainButtonText();
        }
        public void SetFontSize(int size) { /* TODO: implement if needed */ }
        public void SetColors(Color color) { /* TODO: implement if needed */ }
        public void SetColors(Color normal, Color hover, Color pressed) { /* TODO: implement if needed */ }

        #endregion
    }
}