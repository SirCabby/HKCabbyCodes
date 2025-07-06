using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.UI.Modders;
using CabbyMenu.UI.Controls.InputField;
using CabbyMenu.UI.Controls.CustomDropdown;
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

        /// <summary>
        /// Manager for input field interactions and focus handling.
        /// </summary>
        private readonly InputFieldManager inputFieldManager;

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
        /// ScrollRect component for the cheat content area.
        /// </summary>
        private ScrollRect cheatScrollRect;

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
            inputFieldManager = new InputFieldManager();
        }

        /// <summary>
        /// Adds a cheat panel to the menu content area.
        /// </summary>
        /// <param name="cheatPanel">The cheat panel to add.</param>
        /// <returns>The added cheat panel.</returns>
        public CheatPanel AddCheatPanel(CheatPanel cheatPanel)
        {
            if (cheatContent == null)
            {
                // If cheatContent is null, the menu might be rebuilding or in an invalid state
                // Return the panel without adding it to prevent errors
                return cheatPanel;
            }
            
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
            inputFieldManager.RegisterInputFieldSync(inputFieldStatus);
        }

        /// <summary>
        /// Gets all registered input fields.
        /// </summary>
        /// <returns>A read-only list of registered input fields.</returns>
        public IReadOnlyList<InputFieldStatusBase> GetRegisteredInputs()
        {
            return inputFieldManager.GetRegisteredInputs();
        }

        /// <summary>
        /// Clears all registered input fields.
        /// </summary>
        private void ClearInputFields()
        {
            inputFieldManager.ClearInputFields();
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
        /// Removes a cheat panel from the menu content area.
        /// </summary>
        /// <param name="cheatPanel">The cheat panel to remove.</param>
        public void RemoveCheatPanel(CheatPanel cheatPanel)
        {
            if (contentCheatPanels.Contains(cheatPanel))
            {
                contentCheatPanels.Remove(cheatPanel);
            }
            GameObject panelGo = cheatPanel.GetGameObject();
            if (panelGo != null)
            {
                UnityEngine.Object.Destroy(panelGo);
            }
            // Force layout rebuild after removing
            if (cheatContent != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(cheatContent.GetComponent<RectTransform>());
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

                // Handle dropdown deselection - close all dropdowns if clicking outside
                if (Input.GetMouseButtonDown(0) && rootGameObject != null)
                {
                    Vector2 mousePosition = Input.mousePosition;
                    if (!CustomDropdown.IsMouseOverAnyDropdown(mousePosition))
                    {
                        CustomDropdown.CloseAllDropdowns();
                    }
                }

                // Update input field manager
                inputFieldManager.Update();
            }
            else if (rootGameObject != null && rootGoMod != null && rootGoMod.IsActive())
            {
                // Submit any pending changes before hiding the menu
                inputFieldManager.SubmitAndDeselect();
                
                rootGoMod.SetActive(false);
                if (categoryDropdown != null)
                {
                    categoryDropdown.SetValue(0);
                    OnCategorySelected(0);
                }

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

            // Force layout rebuild after clearing content
            LayoutRebuilder.ForceRebuildLayoutImmediate(cheatContent.GetComponent<RectTransform>());

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

            // Force layout rebuild after adding new panels
            LayoutRebuilder.ForceRebuildLayoutImmediate(cheatContent.GetComponent<RectTransform>());

            // Clear Unity's EventSystem selection when changing categories
            EventSystem.current?.SetSelectedGameObject(null);

            // Reset scroll position to top when changing categories
            if (cheatScrollRect != null)
            {
                cheatScrollRect.normalizedPosition = new Vector2(0, 1);
            }
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

            cheatScrollRect = cheatScrollable.GetComponent<ScrollRect>();
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
    }
}
