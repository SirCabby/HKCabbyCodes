using System.Timers;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using CabbyCodes.UI.Factories;
using CabbyCodes.UI;
using CabbyCodes.UI.Modders;
using CabbyCodes.UI.CheatPanels;
using System;

namespace CabbyCodes
{
    public class CabbyMenu
    {
        private readonly string name;
        private readonly string version;
        private readonly Dictionary<string, Action> registeredCategories = new();

        private bool shouldUpdate = true;
        private readonly Timer updateTimer;

        private GameObject rootGameObject;
        private GameObjectMod rootGoMod;
        
        private bool isMenuOpen = false;
        private GameObject menuPanel;
        private GameObjectMod menuPanelGoMod;
        Dropdown categoryDropdown;
        private GameObject cheatContent;

        public CabbyMenu(string name, string version)
        {
            this.name = name;
            this.version = version;

            updateTimer = new Timer(100);
            updateTimer.Elapsed += OnElapsedUpdateTimer;
            updateTimer.AutoReset = true;
            updateTimer.Enabled = true;
        }

        public CheatPanel AddCheatPanel(CheatPanel cheatPanel)
        {
            new Fitter(cheatPanel.GetGameObject()).Attach(cheatContent).Size(new Vector2(0, 50));
            return cheatPanel;
        }

        public void RegisterCategory(string categoryName, Action cheatContent)
        {
            registeredCategories.Add(categoryName, cheatContent);
        }

        public void Update()
        {
            if (!shouldUpdate) return;
            //if (true)
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
            }
            else if (rootGameObject != null && rootGoMod.IsActive())
            {
                rootGoMod.SetActive(false);

                // Reset the menu on unpausing
                if (isMenuOpen)
                {
                    OnCategorySelected(0);
                    OnMenuButtonClicked();
                }
            }

            shouldUpdate = false;
        }

        private void OnElapsedUpdateTimer(object source, ElapsedEventArgs e)
        {
            shouldUpdate = true;
        }

        private void OnMenuButtonClicked()
        {
            isMenuOpen = !isMenuOpen;
            shouldUpdate = true;
            menuPanelGoMod.SetActive(isMenuOpen);
        }

        private void OnCategorySelected(int arg0)
        {
            // Clear current cheat panels
            foreach (Transform child in cheatContent.transform)
            {
                UnityEngine.Object.Destroy(child.gameObject);
            }

            // Build selected cheat panels
            if (arg0 < categoryDropdown.options.Count)
            {
                CheatPanel.ResetPattern();
                registeredCategories[categoryDropdown.options[arg0].text]();
            }
        }

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
            categoryDropdownMod.SetSize(categorySize).SetFontSize(36);
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
