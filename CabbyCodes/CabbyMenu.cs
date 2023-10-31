using System.Timers;
using UnityEngine.UI;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace CabbyCodes
{
    public class CabbyMenu
    {
        private string name;
        private string version;
        private Dictionary<string, Action> registeredCategories = new Dictionary<string, Action>();

        private bool shouldUpdate = true;
        private Timer updateTimer;

        private GameObject rootGameObject;
        
        private bool isMenuOpen = false;
        private GameObject menuPanel;
        private Dropdown categoryDropdown;
        private GameObject cheatContent;

        public static void AttachAndAnchor(GameObject thisObj, Transform parentTransform, Vector2 minAnchor, Vector2 maxAnchor, Vector2? sizeDelta = null)
        {
            if (parentTransform != null)
            {
                thisObj.transform.SetParent(parentTransform.transform, false);
            }

            RectTransform rect = thisObj.GetComponent<RectTransform>();
            rect.anchorMax = maxAnchor;
            rect.anchorMin = minAnchor;

            if (sizeDelta.HasValue)
            {
                rect.sizeDelta = sizeDelta.Value;
            }
        }

        public CabbyMenu(string name, string version)
        {
            this.name = name;
            this.version = version;

            updateTimer = new Timer(100);
            updateTimer.Elapsed += OnElapsedUpdateTimer;
            updateTimer.AutoReset = true;
            updateTimer.Enabled = true;

            Update();
        }

        public void RegisterCategory(string categoryName, Action buildMethod)
        {
            registeredCategories.Add(categoryName, buildMethod);
        }

        public GameObject AddCheatPanel()
        {
            Color thisColor = cheatContent.transform.childCount % 2 == 1 ? Color.red : Color.green;

            GameObject cheatPanel = DefaultControls.CreatePanel(new DefaultControls.Resources());
            cheatPanel.name = "Cheat Panel";
            cheatPanel.GetComponent<Image>().color = thisColor;
            cheatPanel.transform.SetParent(cheatContent.transform, false);
            cheatPanel.transform.localScale = Vector2.one;

            LayoutElement imageLayout = cheatPanel.AddComponent<LayoutElement>();
            imageLayout.preferredHeight = 80;
            imageLayout.flexibleWidth = 1;

            return cheatPanel;
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
                    GameObject.Destroy(menuPanel);
                    menuPanel = null;
                }
            }
            else if (rootGameObject != null)
            {
                GameObject.Destroy(rootGameObject);
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
            rootGameObject = new GameObject("CabbyCodes");
            rootGameObject.name = "CC Root Canvas";

            Canvas canvas = rootGameObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            rootGameObject.AddComponent<GraphicRaycaster>();

            CanvasScaler canvasScalar = rootGameObject.AddComponent<CanvasScaler>();
            canvasScalar.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScalar.referenceResolution = new Vector2(2560, 1440);

            // Menu Button
            GameObject menuButton = DefaultControls.CreateButton(new DefaultControls.Resources());
            menuButton.name = "Open Menu Button";
            menuButton.transform.SetParent(canvas.transform, false);
            menuButton.GetComponent<Button>().onClick.AddListener(OnMenuButtonClicked);
            AttachAndAnchor(menuButton, canvas.transform, new Vector2(0.07f, 0.92f), new Vector2(0.12f, 0.95f), null);

            Text menuText = menuButton.GetComponentInChildren<Text>();
            menuText.text = "Code Menu";
            menuText.fontSize = 36;
        }

        private void ClearCheatContent()
        {
            foreach (Transform child in cheatContent.transform)
            {
                GameObject.Destroy(child.gameObject);
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
            menuPanel.transform.SetParent(canvas.transform, false);
            menuPanel.GetComponent<Image>().color = Color.grey;

            // Category Select Text
            GameObject categoryTextObj = DefaultControls.CreateText(new DefaultControls.Resources());
            categoryTextObj.name = "Category Text";
            AttachAndAnchor(categoryTextObj, menuPanel.transform, new Vector2(0.11f, 0.74f), new Vector2(0.11f, 0.74f), new Vector2(400, 100));

            Text categoryText = categoryTextObj.GetComponent<Text>();
            categoryText.text = "Select Category";
            categoryText.fontSize = 36;
            categoryText.color = Color.black;

            // Category Dropdown
            Vector2 categorySize = new Vector2(250, 60);
            int showSize = 5;
            GameObject categoryDropdownGameObject = DefaultControls.CreateDropdown(new DefaultControls.Resources());
            categoryDropdownGameObject.name = "Category Dropdown";
            AttachAndAnchor(categoryDropdownGameObject, menuPanel.transform, new Vector2(0.08f, 0.72f), new Vector2(0.08f, 0.72f), categorySize);
            categoryDropdown = categoryDropdownGameObject.GetComponent<Dropdown>();
            categoryDropdownGameObject.transform.Find("Label").gameObject.GetComponent<Text>().fontSize = 36;
            categoryDropdown.onValueChanged.AddListener(OnCategorySelected);

            // Populate Registered Categories
            List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
            foreach (string category in registeredCategories.Keys)
            {
                options.Add(new Dropdown.OptionData(category));
            }
            categoryDropdown.options = options;

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

            // Cheat Scrollable
            GameObject cheatScrollable = DefaultControls.CreateScrollView(new DefaultControls.Resources());
            cheatScrollable.name = "Cheat Scrollable";
            cheatScrollable.GetComponent<Image>().color = Color.blue;
            cheatScrollable.GetComponent<ScrollRect>().scrollSensitivity = categorySize.y;
            AttachAndAnchor(cheatScrollable, menuPanel.transform, new Vector2(0.17f, 0.05f), new Vector2(0.8f, 0.9f), Vector2.zero);

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
            GameObject titleTextObj = DefaultControls.CreateText(new DefaultControls.Resources());
            titleTextObj.name = "Title Text";
            AttachAndAnchor(titleTextObj, menuPanel.transform, new Vector2(0.5f, 0.93f), new Vector2(0.5f, 0.93f), new Vector2(400, 100));

            Text titleText = titleTextObj.GetComponent<Text>();
            titleText.text = name;
            titleText.fontStyle = FontStyle.BoldAndItalic;
            titleText.fontSize = 60;
            titleText.color = Color.black;

            // Close Button
            GameObject menuButton = DefaultControls.CreateButton(new DefaultControls.Resources());
            menuButton.name = "Close Button";
            menuButton.GetComponent<Button>().onClick.AddListener(OnMenuButtonClicked);
            AttachAndAnchor(menuButton, menuPanel.transform, new Vector2(0.87f, 0.68f), new Vector2(0.92f, 0.7f), null);

            Text menuText = menuButton.GetComponentInChildren<Text>();
            menuText.text = "Close";
            menuText.fontSize = 46;

            // Version Text
            GameObject versionTextObj = DefaultControls.CreateText(new DefaultControls.Resources());
            versionTextObj.name = "Version Text";
            AttachAndAnchor(versionTextObj, menuPanel.transform, new Vector2(0.95f, 0.05f), new Vector2(0.95f, 0.1f), new Vector2(400, 100));

            Text versionText = versionTextObj.GetComponent<Text>();
            versionText.text = "v" + version;
            versionText.fontStyle = FontStyle.BoldAndItalic;
            versionText.fontSize = 36;
            versionText.color = Color.black;

            OnCategorySelected(0);
        }
    }
}
