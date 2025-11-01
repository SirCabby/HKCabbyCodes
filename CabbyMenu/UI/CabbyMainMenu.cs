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
using CabbyMenu.UI.Popups;

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
        /// Gets the index of a cheat panel in the menu.
        /// </summary>
        /// <param name="cheatPanel">The cheat panel to find.</param>
        /// <returns>The index of the panel, or -1 if not found.</returns>
        public int GetPanelIndex(CheatPanel cheatPanel)
        {
            return contentCheatPanels.IndexOf(cheatPanel);
        }

        /// <summary>
        /// Gets all cheat panels in the menu.
        /// </summary>
        /// <returns>A read-only list of all cheat panels.</returns>
        public IReadOnlyList<CheatPanel> GetAllPanels()
        {
            return contentCheatPanels.AsReadOnly();
        }

        /// <summary>
        /// Filters the category dropdown to show/hide Sprite Viewer based on dev options state.
        /// This method can be called directly from CabbyCodes.
        /// </summary>
        /// <param name="showSpriteViewer">True to show Sprite Viewer, false to hide it</param>
        public void FilterSpriteViewerCategory(bool showSpriteViewer)
        {
            if (categoryDropdown != null)
            {
                // Rebuild the category options list
                List<string> options = new List<string>();
                foreach (string category in registeredCategories.Keys)
                {
                    // Filter out Sprite Viewer when showSpriteViewer is false
                    if (category == "Sprite Viewer" && !showSpriteViewer)
                    {
                        continue; // Skip this category
                    }
                    
                    options.Add(category);
                }
                
                // Update the dropdown options
                categoryDropdown.SetOptions(options);
                
                // The issue is that SetOptions doesn't rebuild the visual options.
                // Instead of trying to fix the existing dropdown, let's recreate it entirely.
                
                // Store the current selection
                int currentSelection = categoryDropdown.Value;
                
                // Get the dropdown's parent and position
                var dropdownGO = categoryDropdown.gameObject;
                var parent = dropdownGO.transform.parent;
                var rectTransform = dropdownGO.GetComponent<UnityEngine.RectTransform>();
                var originalAnchoredPosition = rectTransform.anchoredPosition;
                var originalSizeDelta = rectTransform.sizeDelta;
                var originalAnchorMin = rectTransform.anchorMin;
                var originalAnchorMax = rectTransform.anchorMax;
                
                // Remove the old dropdown
                UnityEngine.Object.DestroyImmediate(dropdownGO);
                
                // Create a new dropdown
                var (newDropdownGO, newCustomDropdown) = CustomDropdown.Build();
                newDropdownGO.name = "Category Dropdown";
                
                // Apply the same configuration as the original dropdown
                Vector2 categorySize = new Vector2(Constants.CATEGORY_DROPDOWN_WIDTH, Constants.CATEGORY_DROPDOWN_HEIGHT);
                newCustomDropdown.SetSize(categorySize.x, categorySize.y);
                newCustomDropdown.SetFontSize(Constants.DEFAULT_FONT_SIZE);
                
                // Position the new dropdown exactly like the original
                newDropdownGO.transform.SetParent(parent, false);
                var newRectTransform = newDropdownGO.GetComponent<UnityEngine.RectTransform>();
                newRectTransform.anchorMin = originalAnchorMin;
                newRectTransform.anchorMax = originalAnchorMax;
                newRectTransform.anchoredPosition = originalAnchoredPosition;
                newRectTransform.sizeDelta = originalSizeDelta;
                
                // Set the new options
                newCustomDropdown.SetOptions(options);
                newCustomDropdown.SetValue(currentSelection);
                newCustomDropdown.onValueChanged.AddListener(OnCategorySelected);
                
                // Replace the reference
                categoryDropdown = newCustomDropdown;
                
                // If the current selection is no longer valid, reset to first category
                if (categoryDropdown.Value >= options.Count)
                {
                    categoryDropdown.SetValue(0);
                    OnCategorySelected(0);
                }
            }
        }

        /// <summary>
        /// Dynamically registers or unregisters a category and refreshes the UI.
        /// </summary>
        /// <param name="categoryName">Name of the category to register/unregister</param>
        /// <param name="addCategory">True to add, false to remove</param>
        /// <param name="categoryAction">The action to execute when the category is selected (only needed when adding)</param>
        public void UpdateCategoryRegistration(string categoryName, bool addCategory, System.Action categoryAction = null)
        {
            if (addCategory)
            {
                // Add category if it doesn't exist
                if (!registeredCategories.ContainsKey(categoryName))
                {
                    registeredCategories.Add(categoryName, categoryAction);
                }
                else
                {
                    // Category already exists, skipping add
                }
            }
            else
            {
                // Remove category if it exists
                if (registeredCategories.ContainsKey(categoryName))
                {
                    registeredCategories.Remove(categoryName);
                }
                else
                {
                    // Category doesn't exist, skipping remove
                }
            }
            
            // Refresh the category dropdown
            if (categoryDropdown != null)
            {
                List<string> options = new List<string>();
                foreach (string category in registeredCategories.Keys)
                {
                    options.Add(category);
                }
                
                // The issue is that SetOptions doesn't rebuild the visual options.
                // Instead of trying to fix the existing dropdown, let's recreate it entirely.
                
                // Store the current selection
                int currentSelection = categoryDropdown.Value;
                
                // Get the dropdown's parent and position
                var dropdownGO = categoryDropdown.gameObject;
                var parent = dropdownGO.transform.parent;
                var rectTransform = dropdownGO.GetComponent<UnityEngine.RectTransform>();
                var originalAnchoredPosition = rectTransform.anchoredPosition;
                var originalSizeDelta = rectTransform.sizeDelta;
                var originalAnchorMin = rectTransform.anchorMin;
                var originalAnchorMax = rectTransform.anchorMax;
                
                // Remove the old dropdown
                UnityEngine.Object.DestroyImmediate(dropdownGO);
                
                // Create a new dropdown
                var (newDropdownGO, newCustomDropdown) = CustomDropdown.Build();
                newDropdownGO.name = "Category Dropdown";
                
                // Apply the same configuration as the original dropdown
                Vector2 categorySize = new Vector2(Constants.CATEGORY_DROPDOWN_WIDTH, Constants.CATEGORY_DROPDOWN_HEIGHT);
                newCustomDropdown.SetSize(categorySize.x, categorySize.y);
                newCustomDropdown.SetFontSize(Constants.DEFAULT_FONT_SIZE);
                
                // Position the new dropdown exactly like the original
                newDropdownGO.transform.SetParent(parent, false);
                var newRectTransform = newDropdownGO.GetComponent<UnityEngine.RectTransform>();
                newRectTransform.anchorMin = originalAnchorMin;
                newRectTransform.anchorMax = originalAnchorMax;
                newRectTransform.anchoredPosition = originalAnchoredPosition;
                newRectTransform.sizeDelta = originalSizeDelta;
                
                // Set the new options
                newCustomDropdown.SetOptions(options);
                newCustomDropdown.SetValue(currentSelection);
                newCustomDropdown.onValueChanged.AddListener(OnCategorySelected);
                
                // Replace the reference
                categoryDropdown = newCustomDropdown;
                
                // If the current selection is no longer valid, reset to first category
                if (categoryDropdown.Value >= options.Count)
                {
                    categoryDropdown.SetValue(0);
                    OnCategorySelected(0);
                }
            }
            else
            {
                // Category dropdown is null, cannot refresh
            }
        }

        /// <summary>
        /// Inserts a cheat panel at a specific index in the menu.
        /// </summary>
        /// <param name="cheatPanel">The cheat panel to insert.</param>
        /// <param name="index">The index where to insert the panel.</param>
        /// <returns>The inserted cheat panel.</returns>
        public CheatPanel InsertCheatPanel(CheatPanel cheatPanel, int index)
        {
            if (cheatContent == null)
            {
                // If cheatContent is null, the menu might be rebuilding or in an invalid state
                // Return the panel without adding it to prevent errors
                return cheatPanel;
            }
            
            // Clamp index to valid range
            index = Math.Max(0, Math.Min(index, contentCheatPanels.Count));
            
            new Fitter(cheatPanel.GetGameObject()).Attach(cheatContent).Size(cheatPanelSize);
            contentCheatPanels.Insert(index, cheatPanel);
            return cheatPanel;
        }

        /// <summary>
        /// Removes panels from a specific index range and returns them.
        /// </summary>
        /// <param name="startIndex">The starting index (inclusive).</param>
        /// <param name="count">The number of panels to remove.</param>
        /// <returns>A list of removed panels.</returns>
        public List<CheatPanel> RemovePanelsAtRange(int startIndex, int count)
        {
            List<CheatPanel> removedPanels = new List<CheatPanel>();
            
            if (startIndex < 0 || startIndex >= contentCheatPanels.Count || count <= 0)
            {
                return removedPanels;
            }
            
            int endIndex = Math.Min(startIndex + count, contentCheatPanels.Count);
            int actualCount = endIndex - startIndex;
            
            for (int i = 0; i < actualCount; i++)
            {
                CheatPanel panel = contentCheatPanels[startIndex];
                removedPanels.Add(panel);
                contentCheatPanels.RemoveAt(startIndex);
                
                // Destroy the GameObject but keep the panel reference
                GameObject panelGo = panel.GetGameObject();
                if (panelGo != null)
                {
                    UnityEngine.Object.Destroy(panelGo);
                }
            }
            
            // Force layout rebuild after removing
            if (cheatContent != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(cheatContent.GetComponent<RectTransform>());
            }
            
            return removedPanels;
        }

        /// <summary>
        /// Detaches panels from a specific index range without destroying them and returns them.
        /// </summary>
        /// <param name="startIndex">The starting index (inclusive).</param>
        /// <param name="count">The number of panels to detach.</param>
        /// <returns>A list of detached panels.</returns>
        public List<CheatPanel> DetachPanelsAtRange(int startIndex, int count)
        {
            List<CheatPanel> detachedPanels = new List<CheatPanel>();
            
            if (startIndex < 0 || startIndex >= contentCheatPanels.Count || count <= 0)
            {
                return detachedPanels;
            }
            
            int endIndex = Math.Min(startIndex + count, contentCheatPanels.Count);
            int actualCount = endIndex - startIndex;
            
            for (int i = 0; i < actualCount; i++)
            {
                CheatPanel panel = contentCheatPanels[startIndex];
                detachedPanels.Add(panel);
                contentCheatPanels.RemoveAt(startIndex);
                
                // Detach the GameObject from parent without destroying it
                GameObject panelGo = panel.GetGameObject();
                if (panelGo != null)
                {
                    panelGo.transform.SetParent(null);
                    panelGo.SetActive(false); // Hide it temporarily
                }
            }
            
            // Force layout rebuild after detaching
            if (cheatContent != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(cheatContent.GetComponent<RectTransform>());
            }
            
            return detachedPanels;
        }

        /// <summary>
        /// Re-adds panels at a specific index, recreating their GameObjects.
        /// </summary>
        /// <param name="panels">The panels to re-add.</param>
        /// <param name="startIndex">The index where to start inserting.</param>
        public void ReAddPanelsAtRange(List<CheatPanel> panels, int startIndex)
        {
            if (panels == null || panels.Count == 0)
            {
                return;
            }
            
            // Clamp index to valid range
            startIndex = Math.Max(0, Math.Min(startIndex, contentCheatPanels.Count));
            
            for (int i = 0; i < panels.Count; i++)
            {
                CheatPanel panel = panels[i];
                new Fitter(panel.GetGameObject()).Attach(cheatContent).Size(cheatPanelSize);
                contentCheatPanels.Insert(startIndex + i, panel);
            }
            
            // Force layout rebuild after adding
            if (cheatContent != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(cheatContent.GetComponent<RectTransform>());
            }
        }

        /// <summary>
        /// Re-attaches detached panels at a specific index.
        /// </summary>
        /// <param name="panels">The panels to re-attach.</param>
        /// <param name="startIndex">The index where to start inserting.</param>
        public void ReattachPanelsAtRange(List<CheatPanel> panels, int startIndex)
        {
            if (panels == null || panels.Count == 0)
            {
                return;
            }
            
            // Clamp index to valid range
            startIndex = Math.Max(0, Math.Min(startIndex, contentCheatPanels.Count));
            
            for (int i = 0; i < panels.Count; i++)
            {
                CheatPanel panel = panels[i];
                GameObject panelGo = panel.GetGameObject();
                if (panelGo != null)
                {
                    // Completely reset the RectTransform to default state
                    RectTransform rect = panelGo.GetComponent<RectTransform>();
                    if (rect != null)
                    {
                        rect.anchorMin = Vector2.zero;
                        rect.anchorMax = Vector2.zero;
                        rect.pivot = new Vector2(0.5f, 0.5f);
                        rect.sizeDelta = Vector2.zero;
                        rect.anchoredPosition = Vector2.zero;
                        rect.localScale = Vector3.one;
                        rect.localRotation = Quaternion.identity;
                    }
                    
                    // Now apply the Fitter as if it's a fresh panel
                    new Fitter(panelGo).Attach(cheatContent).Size(cheatPanelSize);
                    panelGo.SetActive(true); // Show it again
                }
                contentCheatPanels.Insert(startIndex + i, panel);
            }
            
            // Force layout rebuild after re-attaching (same as RemoveCheatPanel)
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
                    
                    // Handle popup deselection - close all popups if clicking outside
                    if (!PopupBase.IsMouseOverAnyPopupContent(mousePosition))
                    {
                        PopupBase.CloseAllPopups();
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
                
                // Only reset category if we have a deferred restoration that needs to be applied
                // This prevents losing the current category selection when just closing/reopening the menu
                if (categoryDropdown != null && deferredCategoryRestoration.HasValue && !deferredRestorationApplied)
                {
                    categoryDropdown.SetValue(0);
                    OnCategorySelected(0);
                }

                // Close and clear all open dropdowns when menu is hidden
                CustomDropdown.CloseAllDropdowns();
                CustomDropdown.ClearAllDropdowns();

                // Close and clear all open popups when menu is hidden
                PopupBase.CloseAllPopups();
                PopupBase.ClearAllPopups();

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
            SetMenuOpen(!isMenuOpen);
        }

        /// <summary>
        /// Handles category selection changes in the dropdown.
        /// </summary>
        /// <param name="arg0">The index of the selected category.</param>
        private void OnCategorySelected(int arg0)
        {
            // Save the selected category if a save callback is provided
            if (onCategorySelectedCallback != null)
            {
                onCategorySelectedCallback(arg0);
            }
            
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

            // Close all open popups when changing categories
            PopupBase.CloseAllPopups();

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
        /// Callback for when a category is selected. Used to save the selection.
        /// </summary>
        private System.Action<int> onCategorySelectedCallback;
        
        /// <summary>
        /// Deferred category restoration index to apply when dropdown is ready.
        /// </summary>
        private int? deferredCategoryRestoration = null;
        
        /// <summary>
        /// Flag to track if deferred restoration has been applied to prevent automatic resets.
        /// </summary>
        private bool deferredRestorationApplied = false;
        
        /// <summary>
        /// Sets the callback for when a category is selected.
        /// </summary>
        /// <param name="callback">The callback to invoke when a category is selected</param>
        public void SetCategorySelectedCallback(System.Action<int> callback)
        {
            onCategorySelectedCallback = callback;
        }
        
        /// <summary>
        /// Gets the count of registered categories.
        /// </summary>
        /// <returns>The number of registered categories</returns>
        public int GetRegisteredCategories()
        {
            return registeredCategories.Count;
        }
        
        /// <summary>
        /// Gets the currently selected category index.
        /// </summary>
        /// <returns>The index of the currently selected category, or -1 if none selected</returns>
        public int GetCurrentCategoryIndex()
        {
            if (categoryDropdown != null)
            {
                return categoryDropdown.Value;
            }
            return -1;
        }
        
        /// <summary>
        /// Sets the category dropdown to a specific value and triggers the category selection.
        /// </summary>
        /// <param name="categoryIndex">The index of the category to select</param>
        public void SetCategoryDropdownValue(int categoryIndex)
        {
            if (categoryDropdown != null && categoryIndex >= 0 && categoryIndex < categoryDropdown.Options.Count)
            {
                categoryDropdown.SetValue(categoryIndex);
                OnCategorySelected(categoryIndex);
            }
        }
        
        /// <summary>
        /// Sets a deferred category restoration that will be applied when the dropdown is ready.
        /// </summary>
        /// <param name="categoryIndex">The index of the category to restore to</param>
        public void SetDeferredCategoryRestoration(int categoryIndex)
        {
            deferredCategoryRestoration = categoryIndex;
        }
        
        /// <summary>
        /// Sets a deferred category restoration and resets the restoration state to force it to be applied.
        /// This is used when the menu needs to be rebuilt with a different category.
        /// </summary>
        /// <param name="categoryIndex">The index of the category to restore to</param>
        public void SetDeferredCategoryRestorationAndReset(int categoryIndex)
        {
            deferredCategoryRestoration = categoryIndex;
            deferredRestorationApplied = false; // Reset so it will be applied again
        }
        
        /// <summary>
        /// Refreshes the deferred restoration from the config callback.
        /// This should be called when the menu needs to be rebuilt to ensure the correct category is restored.
        /// </summary>
        public void RefreshDeferredRestorationFromConfig()
        {
            // This method will be called from outside to set the deferred restoration
            // The actual config reading will happen in the calling code
        }
        
        /// <summary>
        /// Sets the deferred restoration from an external source (like config).
        /// This is used when the menu needs to be rebuilt with a specific category.
        /// </summary>
        /// <param name="categoryIndex">The index of the category to restore to</param>
        public void SetDeferredRestorationFromExternal(int categoryIndex)
        {
            deferredCategoryRestoration = categoryIndex;
            deferredRestorationApplied = false; // Reset so it will be applied again
        }
        
        /// <summary>
        /// Forces a rebuild of the menu with a new category selection.
        /// This is used when the deferred restoration has already been applied but we need to change it.
        /// </summary>
        /// <param name="categoryIndex">The index of the category to switch to</param>
        public void ForceCategoryChange(int categoryIndex)
        {
            if (categoryIndex >= 0 && categoryIndex < GetRegisteredCategories())
            {
                // Reset the deferred restoration state
                deferredRestorationApplied = false;
                deferredCategoryRestoration = categoryIndex;
                
                // Force a rebuild of the canvas
                if (rootGameObject != null)
                {
                    UnityEngine.Object.Destroy(rootGameObject);
                    rootGameObject = null;
                }
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

            // Clear any existing popup tracking when rebuilding the menu
            PopupBase.ClearAllPopups();

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

            // Check for deferred category restoration now that dropdown is ready
            if (deferredCategoryRestoration.HasValue)
            {
                int restoreIndex = deferredCategoryRestoration.Value;
                deferredCategoryRestoration = null; // Clear the deferred value
                
                if (restoreIndex >= 0 && restoreIndex < categoryDropdown.Options.Count)
                {
                    categoryDropdown.SetValue(restoreIndex);
                    OnCategorySelected(restoreIndex);
                    deferredRestorationApplied = true; // Mark that restoration was applied
                }
                else
                {
                    OnCategorySelected(0);
                }
            }
            else
            {
                // If no deferred restoration, default to first category
                OnCategorySelected(0);
            }
        }

        /// <summary>
        /// Gets the root GameObject (canvas) for the menu. Creates it if it does not yet exist.
        /// </summary>
        public GameObject GetRootGameObject()
        {
            if (rootGameObject == null)
            {
                BuildCanvas();
            }
            return rootGameObject;
        }

        /// <summary>
        /// Gets whether the Cabby Codes menu panel is currently open.
        /// </summary>
        public bool IsMenuOpen()
        {
            return isMenuOpen;
        }

        /// <summary>
        /// Sets the open state of the Cabby Codes menu panel.
        /// </summary>
        /// <param name="open">True to show the panel, false to hide it.</param>
        public void SetMenuOpen(bool open)
        {
            if (isMenuOpen == open)
            {
                if (menuPanelGoMod != null)
                {
                    menuPanelGoMod.SetActive(open);
                }
                return;
            }

            EnsureCanvasReady();

            isMenuOpen = open;

            if (menuPanelGoMod != null)
            {
                menuPanelGoMod.SetActive(open);
            }
        }

        /// <summary>
        /// Ensures the canvas and menu panel exist so toggling can occur safely.
        /// </summary>
        private void EnsureCanvasReady()
        {
            if (rootGameObject == null)
            {
                BuildCanvas();
            }

            if (rootGoMod != null && !rootGoMod.IsActive() && gameStateProvider.ShouldShowMenu())
            {
                rootGoMod.SetActive(true);
            }
        }
    }
}
