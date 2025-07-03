using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.UI.Modders;
using CabbyMenu.UI.Controls.InputField;
using CabbyMenu.UI.Controls.CustomDropdown;
using CabbyMenu.Utilities;
using CabbyMenu.UI.Controls;

namespace CabbyMenu.UI
{
    /// <summary>
    /// Main menu system for mod UI. Manages the UI, input handling, and cheat panel organization.
    /// </summary>
    public class CabbyMainMenu
    {
        /// <summary>
        /// Default size for cheat panels.
        /// </summary>
        private static readonly Vector2 cheatPanelSize = new Vector2(0, Constants.CHEAT_PANEL_HEIGHT);

        /// <summary>
        /// The name of the mod.
        /// </summary>
        private readonly string name;

        /// <summary>
        /// The version of the mod.
        /// </summary>
        private readonly string version;

        /// <summary>
        /// Provider for game state information.
        /// </summary>
        private readonly IGameStateProvider gameStateProvider;

        /// <summary>
        /// Dictionary mapping category names to their panel creation actions.
        /// </summary>
        private readonly Dictionary<string, Action> registeredCategories = new Dictionary<string, Action>();

        /// <summary>
        /// List of currently active cheat panels.
        /// </summary>
        private readonly List<CheatPanel> contentCheatPanels = new List<CheatPanel>();

        // Manage InputFieldSync updates
        /// <summary>
        /// List of registered input fields for synchronization.
        /// </summary>
        private readonly List<InputFieldStatusBase> registeredInputs = new List<InputFieldStatusBase>();

        /// <summary>
        /// The last selected input field.
        /// </summary>
        private InputFieldStatusBase lastSelected;

        /// <summary>
        /// The root GameObject for the menu canvas.
        /// </summary>
        private GameObject rootGameObject;

        /// <summary>
        /// Modifier for the root GameObject.
        /// </summary>
        private GameObjectMod rootGoMod;

        // Opened Menu
        /// <summary>
        /// Whether the menu is currently open.
        /// </summary>
        private bool isMenuOpen = false;

        /// <summary>
        /// The main menu panel GameObject.
        /// </summary>
        private GameObject menuPanel;

        /// <summary>
        /// Modifier for the menu panel GameObject.
        /// </summary>
        private GameObjectMod menuPanelGoMod;

        /// <summary>
        /// Dropdown for category selection.
        /// </summary>
        CustomDropdown categoryDropdown;

        /// <summary>
        /// GameObject containing the cheat content area.
        /// </summary>
        private GameObject cheatContent;

        /// <summary>
        /// Initializes a new instance of the CabbyMainMenu class.
        /// </summary>
        /// <param name="name">The name of the mod.</param>
        /// <param name="version">The version of the mod.</param>
        /// <param name="gameStateProvider">Provider for game state information.</param>
        public CabbyMainMenu(string name, string version, IGameStateProvider gameStateProvider)
        {
            this.name = name;
            this.version = version;
            this.gameStateProvider = gameStateProvider ?? throw new ArgumentNullException(nameof(gameStateProvider));
        }

        /// <summary>
        /// Adds a cheat panel to the menu content area.
        /// </summary>
        /// <param name="cheatPanel">The cheat panel to add.</param>
        /// <returns>The added cheat panel.</returns>
        public CheatPanel AddCheatPanel(CheatPanel cheatPanel)
        {
            new Fitter(cheatPanel.GetGameObject()).Attach(cheatContent).Size(cheatPanelSize);
            contentCheatPanels.Add(cheatPanel);
            return cheatPanel;
        }

        /// <summary>
        /// Registers a category with its panel creation action.
        /// </summary>
        /// <param name="categoryName">The name of the category.</param>
        /// <param name="cheatContent">The action that creates panels for this category.</param>
        public void RegisterCategory(string categoryName, Action cheatContent)
        {
            registeredCategories.Add(categoryName, cheatContent);
        }

        /// <summary>
        /// Registers an input field for synchronization.
        /// </summary>
        /// <param name="inputFieldStatus">The input field status to register.</param>
        public void RegisterInputFieldSync(InputFieldStatusBase inputFieldStatus)
        {
            registeredInputs.Add(inputFieldStatus);
        }

        /// <summary>
        /// Gets all registered input fields.
        /// </summary>
        /// <returns>A read-only list of registered input fields.</returns>
        public IReadOnlyList<InputFieldStatusBase> GetRegisteredInputs()
        {
            return registeredInputs.AsReadOnly();
        }

        /// <summary>
        /// Clears all registered input fields.
        /// </summary>
        private void ClearInputFields()
        {
            // Clear Unity's internal focus on all input fields before clearing the list
            foreach (InputFieldStatusBase input in registeredInputs)
            {
                if (input?.InputFieldGo != null)
                {
                    InputField inputField = input.InputFieldGo.GetComponent<InputField>();
                    inputField?.DeactivateInputField();
                }
            }
            registeredInputs.Clear();
        }

        /// <summary>
        /// Updates all cheat panels.
        /// </summary>
        public void UpdateCheatPanels()
        {
            foreach (CheatPanel panel in contentCheatPanels)
            {
                panel.Update();
            }
        }

        /// <summary>
        /// Main update method called every frame. Handles menu visibility and input processing.
        /// </summary>
        public void Update()
        {
            if (gameStateProvider.ShouldShowMenu())
            {
                if (rootGameObject == null)
                {
                    BuildCanvas();
                }

                if (rootGoMod != null && !rootGoMod.IsActive())
                {
                    rootGoMod.SetActive(true);
                }

                // Handle mouse clicks for input field selection
                if (Input.GetMouseButtonDown(0))
                {
                    Vector2 mousePosition = Input.mousePosition;
                    InputFieldStatusBase clickedInput = null;

                    // Check if registeredInputs is valid
                    if (registeredInputs != null)
                    {
                        foreach (InputFieldStatusBase input in registeredInputs)
                        {
                            if (input != null && IsMouseOverInputField(input, mousePosition))
                            {
                                clickedInput = input;
                                break;
                            }
                        }
                    }

                    // Handle dropdown deselection - close all dropdowns if clicking outside
                    if (rootGameObject != null)
                    {
                        if (!CustomDropdown.IsMouseOverAnyDropdown(mousePosition))
                        {
                            CustomDropdown.CloseAllDropdowns();
                        }
                    }

                    // Update selection
                    if (clickedInput != null && clickedInput != lastSelected)
                    {
                        lastSelected?.Submit();
                        lastSelected = clickedInput;
                        // Set Unity's selected GameObject for keyboard input
                        if (clickedInput.InputFieldGo != null)
                        {
                            EventSystem.current?.SetSelectedGameObject(clickedInput.InputFieldGo);
                            // Calculate and set cursor position from mouse click
                            clickedInput.SetCursorPositionFromMouse(mousePosition);
                            UpdateInputFieldDisplay(clickedInput);
                        }
                    }
                    else if (clickedInput == null && lastSelected != null)
                    {
                        // Clicked outside any input field, deselect current
                        lastSelected.Submit();
                        lastSelected = null;
                        // Clear Unity's EventSystem selection
                        EventSystem.current?.SetSelectedGameObject(null);
                    }
                    else if (clickedInput == lastSelected && clickedInput != null)
                    {
                        // Clicked on the same input field - calculate and set cursor position from mouse click
                        if (clickedInput.InputFieldGo != null)
                        {
                            clickedInput.SetCursorPositionFromMouse(mousePosition);
                            UpdateInputFieldDisplay(clickedInput);
                        }
                    }

                    // Update selection states
                    SetInputFieldSelection(lastSelected);
                }

                // Handle keyboard input for selected input field
                if (Input.anyKeyDown && lastSelected != null)
                {
                    // Sync selection state from Unity before processing input
                    lastSelected.SyncSelectionFromUnity();
                    
                    // Handle arrow keys for cursor movement
                    if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        lastSelected.MoveCursor(-1);
                    }
                    else if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        lastSelected.MoveCursor(1);
                    }
                    // Handle Home key to move cursor to beginning
                    else if (Input.GetKeyDown(KeyCode.Home))
                    {
                        lastSelected.SetCursorPosition(0);
                    }
                    // Handle End key to move cursor to end
                    else if (Input.GetKeyDown(KeyCode.End))
                    {
                        lastSelected.SetCursorPosition(lastSelected.GetFullText().Length);
                    }
                    else
                    {
                        // Handle character input
                        char? keyPressed = KeyCodeMap.GetChar(lastSelected.ValidChars);
                        if (keyPressed.HasValue && lastSelected.InputFieldGo != null)
                        {
                            InputField inputField = lastSelected.InputFieldGo.GetComponent<InputField>();
                            if (inputField != null)
                            {
                                lastSelected.InsertCharacter(keyPressed.Value, inputField.characterLimit);
                            }
                        }

                        // Handle backspace
                        if (Input.GetKeyDown(KeyCode.Backspace))
                        {
                            lastSelected.DeleteCharacter();
                        }

                        // Handle delete key
                        if (Input.GetKeyDown(KeyCode.Delete))
                        {
                            lastSelected.DeleteForwardCharacter();
                        }
                    }

                    // Always update the display after any input handling
                    UpdateInputFieldDisplay(lastSelected);

                    if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
                    {
                        lastSelected.Submit();
                        lastSelected = null;
                        
                        // Clear Unity's EventSystem selection
                        EventSystem.current?.SetSelectedGameObject(null);
                        SetInputFieldSelection(lastSelected);
                    }

                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        lastSelected.Cancel();
                        lastSelected = null;
                        
                        // Clear Unity's EventSystem selection
                        EventSystem.current?.SetSelectedGameObject(null);
                        SetInputFieldSelection(lastSelected);
                    }
                }
                else if (Input.anyKeyDown && lastSelected == null)
                {
                }
            }
            else if (rootGameObject != null && rootGoMod != null && rootGoMod.IsActive())
            {
                // Submit any pending changes before hiding the menu
                lastSelected?.Submit();
                
                rootGoMod.SetActive(false);
                if (categoryDropdown != null)
                {
                    categoryDropdown.SetValue(0);
                    OnCategorySelected(0);
                }
                lastSelected = null;
                
                // Clear Unity's EventSystem selection when menu is hidden
                EventSystem.current?.SetSelectedGameObject(null);
                SetInputFieldSelection(lastSelected);

                // Close and clear all open dropdowns when menu is hidden
                CustomDropdown.CloseAllDropdowns();
                CustomDropdown.ClearAllDropdowns();

                // Reset the menu on unpausing
                if (isMenuOpen)
                {
                    OnMenuButtonClicked();
                }
            }
        }

        /// <summary>
        /// Handles menu button click events to toggle menu visibility.
        /// </summary>
        private void OnMenuButtonClicked()
        {
            isMenuOpen = !isMenuOpen;
            menuPanelGoMod.SetActive(isMenuOpen);
        }

        /// <summary>
        /// Handles category selection changes in the dropdown.
        /// </summary>
        /// <param name="arg0">The index of the selected category.</param>
        private void OnCategorySelected(int arg0)
        {
            // Clear current cheat panels
            foreach (Transform child in cheatContent.transform)
            {
                UnityEngine.Object.Destroy(child.gameObject);
            }

            ClearInputFields();
            contentCheatPanels.Clear();
            CheatPanel.ResetPattern();

            // Close all open dropdowns when changing categories
            CustomDropdown.CloseAllDropdowns();

            // Build selected cheat panels
            if (categoryDropdown != null && arg0 < categoryDropdown.Options.Count)
            {
                registeredCategories[categoryDropdown.Options[arg0]]();
            }

            lastSelected = null;
            
            // Clear Unity's EventSystem selection when changing categories
            EventSystem.current?.SetSelectedGameObject(null);
            SetInputFieldSelection(lastSelected);
        }

        /// <summary>
        /// Builds the main canvas and UI elements for the menu system.
        /// </summary>
        private void BuildCanvas()
        {
            if (rootGameObject != null) return;

            // Clear any existing dropdown tracking when rebuilding the menu
            CustomDropdown.ClearAllDropdowns();

            // Ensure EventSystem exists
            if (UnityEngine.Object.FindObjectOfType<EventSystem>() == null)
            {
                GameObject eventSystem = new GameObject("EventSystem");
                eventSystem.AddComponent<EventSystem>();
                eventSystem.AddComponent<StandaloneInputModule>();
            }

            // Base Canvas
            rootGameObject = new GameObject("ModMenu")
            {
                name = "Mod Menu Root Canvas"
            };
            rootGoMod = new GameObjectMod(rootGameObject);

            Canvas canvas = rootGameObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            rootGameObject.AddComponent<GraphicRaycaster>();

            CanvasScaler canvasScalar = rootGameObject.AddComponent<CanvasScaler>();
            canvasScalar.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScalar.referenceResolution = new Vector2(Constants.REFERENCE_RESOLUTION_WIDTH, Constants.REFERENCE_RESOLUTION_HEIGHT);

            // Menu Button
            (GameObject menuButton, GameObjectMod menuButtonGoMod, _) = ButtonBuilder.BuildDefault("Code Menu");
            menuButtonGoMod.SetName("Open Menu Button");
            menuButton.GetComponent<Button>().onClick.AddListener(OnMenuButtonClicked);
            new Fitter(menuButton).Attach(canvas).Anchor(new Vector2(Constants.MENU_BUTTON_MIN_X, Constants.MENU_BUTTON_MIN_Y), new Vector2(Constants.MENU_BUTTON_MAX_X, Constants.MENU_BUTTON_MAX_Y));

            // Setup Menu Panel
            menuPanel = DefaultControls.CreatePanel(new DefaultControls.Resources());
            menuPanel.name = "Menu Panel";
            new Fitter(menuPanel).Attach(canvas);
            new ImageMod(menuPanel.GetComponent<Image>()).SetColor(Constants.MENU_PANEL_COLOR);
            menuPanelGoMod = new GameObjectMod(menuPanel).SetActive(false);

            // Category Select Text
            (GameObject categoryTextObj, GameObjectMod categoryTextGoMod, TextMod categoryTextMod) = TextMod.Build("Select Category");
            categoryTextGoMod.SetName("Category Text").SetOutline(Color.black);
            categoryTextMod.SetColor(Color.white);
            new Fitter(categoryTextObj).Attach(menuPanel).Anchor(new Vector2(Constants.CATEGORY_TEXT_X, Constants.CATEGORY_TEXT_Y), new Vector2(Constants.CATEGORY_TEXT_X, Constants.CATEGORY_TEXT_Y)).Size(new Vector2(Constants.CATEGORY_TEXT_WIDTH, Constants.CATEGORY_TEXT_HEIGHT));

            // Category Dropdown
            Vector2 categorySize = new Vector2(Constants.CATEGORY_DROPDOWN_WIDTH, Constants.CATEGORY_DROPDOWN_HEIGHT);
            (GameObject categoryDropdownGameObject, CustomDropdown customDropdown) = CustomDropdown.Build();
            categoryDropdownGameObject.name = "Category Dropdown";
            int showSize = Math.Min(Constants.MAX_CATEGORY_SHOW_SIZE, registeredCategories.Count);
            customDropdown.SetSize(categorySize.x, categorySize.y);
            customDropdown.SetFontSize(Constants.DEFAULT_FONT_SIZE);
            new Fitter(categoryDropdownGameObject).Attach(menuPanel).Anchor(new Vector2(Constants.CATEGORY_DROPDOWN_X, Constants.CATEGORY_DROPDOWN_Y), new Vector2(Constants.CATEGORY_DROPDOWN_X, Constants.CATEGORY_DROPDOWN_Y));

            // Populate Registered Categories
            List<string> options = new List<string>();
            foreach (string category in registeredCategories.Keys)
            {
                options.Add(category);
            }
            customDropdown.SetOptions(options);
            customDropdown.onValueChanged.AddListener(OnCategorySelected);

            categoryDropdown = customDropdown;

            // Cheat Scrollable
            GameObject cheatScrollable = DefaultControls.CreateScrollView(new DefaultControls.Resources());
            cheatScrollable.name = "Cheat Scrollable";
            cheatScrollable.GetComponent<Image>().color = Constants.CHEAT_SCROLLABLE_BACKGROUND;
            cheatScrollable.GetComponent<ScrollRect>().scrollSensitivity = categorySize.y;
            new Fitter(cheatScrollable).Attach(menuPanel).Anchor(new Vector2(Constants.CHEAT_SCROLLABLE_MIN_X, Constants.CHEAT_SCROLLABLE_MIN_Y), new Vector2(Constants.CHEAT_SCROLLABLE_MAX_X, Constants.CHEAT_SCROLLABLE_MAX_Y)).Size(Vector2.zero);
            new ScrollBarMod(cheatScrollable.transform.Find("Scrollbar Vertical").gameObject.GetComponent<Scrollbar>()).SetDefaults();

            ScrollRect cheatScrollRect = cheatScrollable.GetComponent<ScrollRect>();
            cheatScrollRect.movementType = ScrollRect.MovementType.Clamped;

            cheatContent = cheatScrollable.transform.Find("Viewport").Find("Content").gameObject;
            VerticalLayoutGroup cheatLayoutGroup = cheatContent.AddComponent<VerticalLayoutGroup>();
            cheatLayoutGroup.padding = new RectOffset(Constants.CHEAT_CONTENT_PADDING, Constants.CHEAT_CONTENT_PADDING, Constants.CHEAT_CONTENT_PADDING, Constants.CHEAT_CONTENT_PADDING);
            cheatLayoutGroup.spacing = Constants.CHEAT_CONTENT_SPACING;
            cheatLayoutGroup.childForceExpandHeight = false;
            cheatLayoutGroup.childControlHeight = false;

            ContentSizeFitter contentSizeFitter = cheatContent.AddComponent<ContentSizeFitter>();
            contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            // Title Text
            (GameObject titleText, GameObjectMod titleGoMod, TextMod titleTextMod) = TextMod.Build(name);
            titleGoMod.SetName("Title Text").SetOutline(Color.black);
            titleTextMod.SetFontStyle(FontStyle.BoldAndItalic).SetFontSize(Constants.TITLE_FONT_SIZE).SetColor(Color.white);
            new Fitter(titleText).Attach(menuPanel).Anchor(new Vector2(Constants.TITLE_TEXT_X, Constants.TITLE_TEXT_Y), new Vector2(Constants.TITLE_TEXT_X, Constants.TITLE_TEXT_Y)).Size(new Vector2(Constants.TITLE_TEXT_WIDTH, Constants.TITLE_TEXT_HEIGHT));

            // Close Button
            (GameObject closeMenuButton, GameObjectMod closeMenuButtonGoMod, TextMod closeMenuButtonTextMod) = ButtonBuilder.BuildDefault("Close");
            closeMenuButtonGoMod.SetName("Close Button");
            closeMenuButtonTextMod.SetFontSize(Constants.CLOSE_BUTTON_FONT_SIZE);
            closeMenuButton.GetComponent<Button>().onClick.AddListener(OnMenuButtonClicked);
            new Fitter(closeMenuButton).Attach(menuPanel).Anchor(new Vector2(Constants.CLOSE_BUTTON_MIN_X, Constants.CLOSE_BUTTON_MIN_Y), new Vector2(Constants.CLOSE_BUTTON_MAX_X, Constants.CLOSE_BUTTON_MAX_Y));

            // Version Text
            (GameObject versionTextObj, GameObjectMod versionTextGoMod, TextMod versionTextMod) = TextMod.Build("v" + version);
            versionTextGoMod.SetName("Version Text").SetOutline(Color.black);
            versionTextMod.SetFontStyle(FontStyle.BoldAndItalic).SetColor(Color.white);
            new Fitter(versionTextObj).Attach(menuPanel).Anchor(new Vector2(Constants.VERSION_TEXT_X, Constants.VERSION_TEXT_MIN_Y), new Vector2(Constants.VERSION_TEXT_X, Constants.VERSION_TEXT_MAX_Y)).Size(new Vector2(Constants.VERSION_TEXT_WIDTH, Constants.VERSION_TEXT_HEIGHT));

            OnCategorySelected(0);
        }

        /// <summary>
        /// Checks if the mouse is over a specific input field.
        /// </summary>
        /// <param name="inputStatus">The input field status to check.</param>
        /// <param name="mousePosition">The current mouse position.</param>
        /// <returns>True if the mouse is over the input field, false otherwise.</returns>
        private bool IsMouseOverInputField(InputFieldStatusBase inputStatus, Vector2 mousePosition)
        {
            if (inputStatus?.InputFieldGo == null) return false;
            RectTransform rectTransform = inputStatus.InputFieldGo.GetComponent<RectTransform>();
            if (rectTransform == null) return false;
            return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, mousePosition);
        }

        /// <summary>
        /// Sets selection state for all input fields, ensuring only one is selected.
        /// </summary>
        /// <param name="selected">The input field to select, or null to deselect all.</param>
        private void SetInputFieldSelection(InputFieldStatusBase selected)
        {
            if (registeredInputs == null) return;
            
            foreach (var input in registeredInputs)
            {
                if (input != null)
                {
                    bool shouldBeSelected = input == selected;
                    UnityEngine.Debug.Log($"Setting {input.InputFieldGo?.name} selected={shouldBeSelected}");
                    input.SetSelected(shouldBeSelected);
                }
            }
        }

        /// <summary>
        /// Updates the input field display.
        /// </summary>
        /// <param name="inputStatus">The input field status to update.</param>
        private void UpdateInputFieldDisplay(InputFieldStatusBase inputStatus)
        {
            if (inputStatus?.InputFieldGo == null) return;

            InputField inputField = inputStatus.InputFieldGo.GetComponent<InputField>();
            if (inputField != null)
            {
                // Update the display text to show the visible portion
                string visibleText = inputStatus.GetVisibleText();
                if (inputField.text != visibleText)
                {
                    inputField.text = visibleText;
                }
                
                // Update Unity's cursor position to match the visible cursor position
                int visibleCursorPos = inputStatus.GetVisibleCursorPosition();
                inputField.caretPosition = visibleCursorPos;
                
                // Only update selection if there's no active selection (to preserve user's selection)
                var selection = inputStatus.GetTextSelection();
                if (!selection.HasValue)
                {
                    inputField.selectionAnchorPosition = visibleCursorPos;
                    inputField.selectionFocusPosition = visibleCursorPos;
                }
            }
        }
    }
}
