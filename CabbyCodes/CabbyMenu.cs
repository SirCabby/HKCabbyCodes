using System.Timers;
using UnityEngine.UI;
using UnityEngine;
using System;
using System.Collections.Generic;
using CabbyCodes.UI.Factories;
using CabbyCodes.UI;
using CabbyCodes.UI.Modders;
using CabbyCodes.UI.CheatPanels;

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
        
        private bool isMenuOpen = false;
        private GameObject menuPanel;
        private Dropdown categoryDropdown;
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

        public void RegisterCategory(string categoryName, Action buildMethod)
        {
            registeredCategories.Add(categoryName, buildMethod);
        }

        public void AddCheatPanel(CheatPanel cheatPanel)
        {
            Color thisColor = cheatContent.transform.childCount % 2 == 1 ? Color.red : Color.green;

            new ImageMod(cheatPanel.GetGameObject().GetComponent<Image>()).SetColor(thisColor);
            new Fitter(cheatPanel.GetGameObject()).Attach(cheatContent);
        }

        public void Update()
        {
            if (!shouldUpdate) return;
            //if (GameManager._instance != null && GameManager.instance.IsGamePaused())
            if (true)
            {
                if (rootGameObject == null)
                {
                    BuildCanvas();
                }

                // Menu Panel
                if (isMenuOpen)
                {
                    if (menuPanel == null)
                    {
                        BuildMenuPanel();
                    }
                }
                else if (menuPanel != null)
                {
                    UnityEngine.Object.Destroy(menuPanel);
                    menuPanel = null;
                }
            }
            else if (rootGameObject != null)
            {
                UnityEngine.Object.Destroy(rootGameObject);
                rootGameObject = null;
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
        }

        private void BuildCanvas()
        {
            // Base Canvas
            rootGameObject = new GameObject("CabbyCodes")
            {
                name = "CC Root Canvas"
            };

            Canvas canvas = rootGameObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            rootGameObject.AddComponent<GraphicRaycaster>();

            CanvasScaler canvasScalar = rootGameObject.AddComponent<CanvasScaler>();
            canvasScalar.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScalar.referenceResolution = new Vector2(2560, 1440);

            // Menu Button
            (GameObject menuButton, GameObjectMod gameObjectMod, _) = ButtonFactory.Build("Code Menu");
            gameObjectMod.SetName("Open Menu Button");
            menuButton.GetComponent<Button>().onClick.AddListener(OnMenuButtonClicked);
            new Fitter(menuButton).Attach(canvas).Anchor(new Vector2(0.07f, 0.92f), new Vector2(0.12f, 0.95f));
        }

        private void ClearCheatContent()
        {
            foreach (Transform child in cheatContent.transform)
            {
                UnityEngine.Object.Destroy(child.gameObject);
            }
        }

        private void OnCategorySelected(int arg0)
        {
            ClearCheatContent();

            if (arg0 < categoryDropdown.options.Count)
            {
                registeredCategories[categoryDropdown.options[arg0].text]();
            }
        }

        private void BuildMenuPanel()
        {
            // Setup Menu Panel
            Canvas canvas = rootGameObject.GetComponent<Canvas>();

            menuPanel = DefaultControls.CreatePanel(new DefaultControls.Resources());
            menuPanel.name = "Menu Panel";
            new Fitter(menuPanel).Attach(canvas);
            menuPanel.GetComponent<Image>().color = new Color(0, 0, 0, 0.8f);

            // Category Select Text
            (GameObject categoryTextObj, GameObjectMod gameObjectMod, _) = TextFactory.Build("Select Category");
            gameObjectMod.SetName("Category Text");
            new Fitter(categoryTextObj).Attach(menuPanel).Anchor(new Vector2(0.11f, 0.74f), new Vector2(0.11f, 0.74f)).Size(new Vector2(400, 100));

            // Category Dropdown
            Vector2 categorySize = new(250, 60);
            int showSize = 5;
            GameObject categoryDropdownGameObject = DefaultControls.CreateDropdown(new DefaultControls.Resources());
            categoryDropdownGameObject.name = "Category Dropdown";
            new Fitter(categoryDropdownGameObject).Attach(menuPanel).Anchor(new Vector2(0.08f, 0.72f), new Vector2(0.08f, 0.72f)).Size(categorySize);
            categoryDropdown = categoryDropdownGameObject.GetComponent<Dropdown>();
            categoryDropdownGameObject.transform.Find("Label").gameObject.GetComponent<Text>().fontSize = 36;
            categoryDropdown.onValueChanged.AddListener(OnCategorySelected);

            GameObject template = categoryDropdownGameObject.transform.Find("Template").gameObject;
            template.GetComponent<RectTransform>().sizeDelta = new Vector2(0, categorySize.y * showSize);
            template.GetComponent<ScrollRect>().scrollSensitivity = categorySize.y;

            Scrollbar scrollBar = template.transform.Find("Scrollbar").gameObject.GetComponent<Scrollbar>();
            ColorBlock scrollBarColors = scrollBar.colors;
            scrollBarColors.normalColor = new Color(0.3f, 0.3f, 1, 1);
            scrollBarColors.highlightedColor = new Color(0, 0, 1, 1);
            scrollBar.colors = scrollBarColors;

            GameObject viewport = template.transform.Find("Viewport").gameObject;
            viewport.GetComponent<RectTransform>().sizeDelta = new Vector2(0, categorySize.y * showSize);

            GameObject content = viewport.transform.Find("Content").gameObject; // dropdown popup
            content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, categorySize.y);

            GameObject item = content.transform.Find("Item").gameObject;
            item.GetComponent<RectTransform>().sizeDelta = new Vector2(0, categorySize.y);
            item.transform.Find("Item Label").gameObject.GetComponent<Text>().fontSize = 36;

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

            Scrollbar cheatScrollBar = cheatScrollable.transform.Find("Scrollbar Vertical").gameObject.GetComponent<Scrollbar>();
            ColorBlock cheatScrollBarColors = cheatScrollBar.colors;
            cheatScrollBarColors.normalColor = new Color(0.3f, 0.3f, 1, 1);
            cheatScrollBarColors.highlightedColor = new Color(0, 0, 1, 1);
            cheatScrollBar.colors = cheatScrollBarColors;

            ScrollRect cheatScrollRect = cheatScrollable.GetComponent<ScrollRect>();
            cheatScrollRect.movementType = ScrollRect.MovementType.Clamped;

            cheatContent = cheatScrollable.transform.Find("Viewport").Find("Content").gameObject;

            VerticalLayoutGroup cheatLayoutGroup = cheatContent.AddComponent<VerticalLayoutGroup>();
            cheatLayoutGroup.padding = new RectOffset(10, 10, 10, 10);
            cheatLayoutGroup.spacing = 10;
            cheatLayoutGroup.childForceExpandHeight = false;

            ContentSizeFitter contentSizeFitter = cheatContent.AddComponent<ContentSizeFitter>();
            contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            // Title Text
            (GameObject titleText, GameObjectMod titleGoMod, TextMod titleTextMod) = TextFactory.Build(name);
            titleGoMod.SetName("Title Text").SetOutline(Color.black);
            titleTextMod.SetFontStyle(FontStyle.BoldAndItalic).SetFontSize(60).SetColor(Color.white);
            new Fitter(titleText).Attach(menuPanel).Anchor(new Vector2(0.5f, 0.93f), new Vector2(0.5f, 0.93f)).Size(new Vector2(400, 100));

            // Close Button
            (GameObject menuButton, GameObjectMod menuButtonGoMod, TextMod menuButtonTextMod) = ButtonFactory.Build("Close");
            menuButtonGoMod.SetName("Close Button");
            menuButtonTextMod.SetFontSize(46);
            menuButton.GetComponent<Button>().onClick.AddListener(OnMenuButtonClicked);
            new Fitter(menuButton).Attach(menuPanel).Anchor(new Vector2(0.87f, 0.68f), new Vector2(0.92f, 0.7f));

            // Version Text
            (GameObject versionTextObj, GameObjectMod versionTextGoMod, TextMod versionTextMod) = TextFactory.Build("v" + version);
            versionTextGoMod.SetName("Version Text");
            versionTextMod.SetFontStyle(FontStyle.BoldAndItalic);
            new Fitter(versionTextObj).Attach(menuPanel).Anchor(new Vector2(0.95f, 0.05f), new Vector2(0.95f, 0.1f)).Size(new Vector2(400, 100));

            OnCategorySelected(0);
        }
    }
}
