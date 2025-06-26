using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using CabbyMenu.UI.Factories;
using CabbyMenu.UI.Modders;
using CabbyMenu.UI.CheatPanels;
using System;
using CabbyMenu.Types;
using System.Timers;

namespace CabbyMenu.UI
{
    /// <summary>
    /// Main menu system for the CabbyCodes mod. Manages the UI, input handling, and cheat panel organization.
    /// </summary>
    public class CabbyMenu
    {
        /// <summary>
        /// Default size for cheat panels.
        /// </summary>
        private static readonly Vector2 cheatPanelSize = new(0, 50);

        /// <summary>
        /// The name of the mod.
        /// </summary>
        private readonly string name;

        /// <summary>
        /// The version of the mod.
        /// </summary>
        private readonly string version;

        /// <summary>
        /// Dictionary mapping category names to their panel creation actions.
        /// </summary>
        private readonly Dictionary<string, Action> registeredCategories = new();

        /// <summary>
        /// List of currently active cheat panels.
        /// </summary>
        private readonly List<CheatPanel> contentCheatPanels = new();

        // Manage InputFieldSync updates
        /// <summary>
        /// List of registered input fields for synchronization.
        /// </summary>
        private readonly List<InputFieldStatus> registeredInputs = new();

        /// <summary>
        /// The last selected input field.
        /// </summary>
        private InputFieldStatus lastSelected;

        /// <summary>
        /// Timestamp of the last selection.
        /// </summary>
        private float lastSelectedTime = 0;

        /// <summary>
        /// Timer for handling click events.
        /// </summary>
        private readonly Timer clickTimer;

        // Root Button
        /// <summary>
        /// The root GameObject for the menu system.
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
        Dropdown categoryDropdown;

        /// <summary>
        /// GameObject containing the cheat content area.
        /// </summary>
        private GameObject cheatContent;

        /// <summary>
        /// Initializes a new instance of the CabbyMenu class.
        /// </summary>
        /// <param name="name">The name of the mod.</param>
        /// <param name="version">The version of the mod.</param>
        public CabbyMenu(string name, string version)
        {
            this.name = name;
            this.version = version;

            clickTimer = new Timer(Constants.CLICK_TIMER_DELAY);
            clickTimer.Elapsed += OnElapsedClickTimer;
            clickTimer.AutoReset = false;
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
        public void RegisterInputFieldSync(InputFieldStatus inputFieldStatus)
        {
            registeredInputs.Add(inputFieldStatus);
        }

        /// <summary>
        /// Clears all registered input fields.
        /// </summary>
        private void ClearInputFields()
        {
            registeredInputs.Clear();
        }

        /// <summary>
        /// Handles click timer elapsed events to manage input field selection states.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The elapsed event arguments.</param>
        private void OnElapsedClickTimer(object source, ElapsedEventArgs e)
        {
            // Determine last selected
            bool updateSelected = false;
            foreach (InputFieldStatus input in registeredInputs)
            {
                float thisSelectedTime = input.GetSelectedTime();

                if (input.WasSelected() && (thisSelectedTime > lastSelectedTime))
                {
                    lastSelected?.Submit();

                    lastSelected = input;
                    lastSelectedTime = thisSelectedTime;
                    updateSelected = true;
                }
            }

            foreach (InputFieldStatus input in registeredInputs)
            {
                if (input == lastSelected && !updateSelected)
                {
                    lastSelected.Submit();
                    lastSelected = null;
                }

                input.OnSelected(input == lastSelected);
            }
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
            if (GameManager._instance != null && GameManager.instance.IsGamePaused())
            {
                if (rootGameObject == null)
                {
                    BuildCanvas();
                }

                if (!rootGoMod.IsActive())
                {
                    rootGoMod.SetActive(true);
                }

                if (Input.GetMouseButtonUp(0))
                {
                    clickTimer.Stop();
                    clickTimer.Start();
                }

                // keyboard inputs...
                if (Input.anyKeyDown && lastSelected != null)
                {
                    char? keyPressed = KeyCodeMap.GetChar(lastSelected.ValidChars);
                    if (keyPressed.HasValue)
                    {
                        InputField inputField = lastSelected.InputFieldGo.GetComponent<InputField>();
                        if (inputField.text == "0")
                        {
                            inputField.text = keyPressed.Value.ToString();
                        }
                        else if (inputField.text.Length == inputField.characterLimit)
                        {
                            inputField.text = inputField.text.Substring(0, inputField.characterLimit - 1) + keyPressed.Value.ToString();
                        }
                        else
                        {
                            inputField.text += keyPressed.Value;
                        }


                        lastSelectedTime = lastSelected.GetSelectedTime();
                    }

                    if (Input.GetKeyDown(KeyCode.Backspace))
                    {
                        InputField inputField = lastSelected.InputFieldGo.GetComponent<InputField>();
                        if (inputField.text.Length > 0)
                        {
                            inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);
                        }
                        lastSelectedTime = lastSelected.GetSelectedTime();
                    }

                    if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
                    {
                        lastSelected.Submit();
                        lastSelected = null;
                    }

                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        lastSelected.Cancel();
                        lastSelected = null;
                    }
                }
            }
            else if (rootGameObject != null && rootGoMod.IsActive())
            {
                rootGoMod.SetActive(false);
                categoryDropdown.value = 0;
                OnCategorySelected(0);
                lastSelected = null;

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

            // Build selected cheat panels
            if (arg0 < categoryDropdown.options.Count)
            {
                registeredCategories[categoryDropdown.options[arg0].text]();
            }

            lastSelected = null;
        }

        /// <summary>
        /// Builds the main canvas and UI elements for the menu system.
        /// </summary>
        private void BuildCanvas()
        {
            if (rootGameObject != null) return;

            // Base Canvas
            rootGameObject = new GameObject("CabbyCodes")
            {
                name = "CC Root Canvas"
            };
            rootGoMod = new GameObjectMod(rootGameObject);

            Canvas canvas = rootGameObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            rootGameObject.AddComponent<GraphicRaycaster>();

            CanvasScaler canvasScalar = rootGameObject.AddComponent<CanvasScaler>();
            canvasScalar.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScalar.referenceResolution = new Vector2(2560, 1440);

            // Menu Button
            (GameObject menuButton, GameObjectMod menuButtonGoMod, _) = ButtonFactory.Build("Code Menu");
            menuButtonGoMod.SetName("Open Menu Button");
            menuButton.GetComponent<Button>().onClick.AddListener(OnMenuButtonClicked);
            new Fitter(menuButton).Attach(canvas).Anchor(new Vector2(0.07f, 0.92f), new Vector2(0.12f, 0.95f));

            // Setup Menu Panel
            menuPanel = DefaultControls.CreatePanel(new DefaultControls.Resources());
            menuPanel.name = "Menu Panel";
            new Fitter(menuPanel).Attach(canvas);
            new ImageMod(menuPanel.GetComponent<Image>()).SetColor(new Color(0, 0, 0, 0.8f));
            menuPanelGoMod = new GameObjectMod(menuPanel).SetActive(false);

            // Category Select Text
            (GameObject categoryTextObj, GameObjectMod categoryTextGoMod, TextMod categoryTextMod) = TextFactory.Build("Select Category");
            categoryTextGoMod.SetName("Category Text").SetOutline(Color.black);
            categoryTextMod.SetColor(Color.white);
            new Fitter(categoryTextObj).Attach(menuPanel).Anchor(new Vector2(0.11f, 0.74f), new Vector2(0.11f, 0.74f)).Size(new Vector2(400, 100));

            // Category Dropdown
            Vector2 categorySize = new(280, 60);
            (GameObject categoryDropdownGameObject, GameObjectMod categoryDropdownGoMod, DropdownMod categoryDropdownMod) = DropdownFactory.Build();
            categoryDropdownGoMod.SetName("Category Dropdown");
            int showSize = Math.Min(9, registeredCategories.Count);
            categoryDropdownMod.SetSize(categorySize, showSize).SetFontSize(36);
            new Fitter(categoryDropdownGameObject).Attach(menuPanel).Anchor(new Vector2(0.08f, 0.72f), new Vector2(0.08f, 0.72f));

            categoryDropdown = categoryDropdownGameObject.GetComponent<Dropdown>();
            categoryDropdown.onValueChanged.AddListener(OnCategorySelected);

            // Populate Registered Categories
            List<Dropdown.OptionData> options = new();
            foreach (string category in registeredCategories.Keys)
            {
                options.Add(new Dropdown.OptionData(category));
            }
            categoryDropdown.options = options;

            // Cheat Scrollable
            GameObject cheatScrollable = DefaultControls.CreateScrollView(new DefaultControls.Resources());
            cheatScrollable.name = "Cheat Scrollable";
            cheatScrollable.GetComponent<Image>().color = Color.blue;
            cheatScrollable.GetComponent<ScrollRect>().scrollSensitivity = categorySize.y;
            new Fitter(cheatScrollable).Attach(menuPanel).Anchor(new Vector2(0.17f, 0.05f), new Vector2(0.8f, 0.9f)).Size(Vector2.zero);
            new ScrollBarMod(cheatScrollable.transform.Find("Scrollbar Vertical").gameObject.GetComponent<Scrollbar>()).SetDefaults();

            ScrollRect cheatScrollRect = cheatScrollable.GetComponent<ScrollRect>();
            cheatScrollRect.movementType = ScrollRect.MovementType.Clamped;

            cheatContent = cheatScrollable.transform.Find("Viewport").Find("Content").gameObject;
            VerticalLayoutGroup cheatLayoutGroup = cheatContent.AddComponent<VerticalLayoutGroup>();
            cheatLayoutGroup.padding = new RectOffset(10, 10, 10, 10);
            cheatLayoutGroup.spacing = 10;
            cheatLayoutGroup.childForceExpandHeight = false;
            cheatLayoutGroup.childControlHeight = false;

            ContentSizeFitter contentSizeFitter = cheatContent.AddComponent<ContentSizeFitter>();
            contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            // Title Text
            (GameObject titleText, GameObjectMod titleGoMod, TextMod titleTextMod) = TextFactory.Build(name);
            titleGoMod.SetName("Title Text").SetOutline(Color.black);
            titleTextMod.SetFontStyle(FontStyle.BoldAndItalic).SetFontSize(60).SetColor(Color.white);
            new Fitter(titleText).Attach(menuPanel).Anchor(new Vector2(0.5f, 0.93f), new Vector2(0.5f, 0.93f)).Size(new Vector2(400, 100));

            // Close Button
            (GameObject closeMenuButton, GameObjectMod closeMenuButtonGoMod, TextMod closeMenuButtonTextMod) = ButtonFactory.Build("Close");
            closeMenuButtonGoMod.SetName("Close Button");
            closeMenuButtonTextMod.SetFontSize(46);
            closeMenuButton.GetComponent<Button>().onClick.AddListener(OnMenuButtonClicked);
            new Fitter(closeMenuButton).Attach(menuPanel).Anchor(new Vector2(0.87f, 0.68f), new Vector2(0.92f, 0.7f));

            // Version Text
            (GameObject versionTextObj, GameObjectMod versionTextGoMod, TextMod versionTextMod) = TextFactory.Build("v" + version);
            versionTextGoMod.SetName("Version Text").SetOutline(Color.black);
            versionTextMod.SetFontStyle(FontStyle.BoldAndItalic).SetColor(Color.white);
            new Fitter(versionTextObj).Attach(menuPanel).Anchor(new Vector2(0.95f, 0.05f), new Vector2(0.95f, 0.1f)).Size(new Vector2(400, 100));

            OnCategorySelected(0);
        }
    }
}
