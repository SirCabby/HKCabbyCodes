using System;
using System.Collections.Generic;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.SyncedReferences;

namespace CabbyMenu.UI.DynamicPanels
{
    /// <summary>
    /// Class for creating categorized panel sections with dropdown selectors.
    /// </summary>
    public class CategorizedPanelSection : ISyncedReference<int>, ISyncedValueList
    {
        private readonly string dropdownLabel;
        private readonly List<string> categoryNames;
        private readonly Func<int, List<CheatPanel>> panelFactory;
        private readonly int insertionIndex;
        private readonly int defaultSelection;
        private int currentIndex;

        public CategorizedPanelSection(
            string dropdownLabel,
            List<string> categoryNames,
            Func<int, List<CheatPanel>> panelFactory,
            int insertionIndex = 1,
            int defaultSelection = 0)
        {
            this.dropdownLabel = dropdownLabel;
            this.categoryNames = categoryNames;
            this.panelFactory = panelFactory;
            this.insertionIndex = insertionIndex;
            this.defaultSelection = defaultSelection;
            currentIndex = defaultSelection;
        }

        /// <summary>
        /// Adds this categorized section directly to the provided main menu.
        /// </summary>
        public DynamicPanelManager AddToMenu(CabbyMainMenu menu)
        {
            var dropdownPanel = new DropdownPanel(this, dropdownLabel, Constants.DEFAULT_PANEL_HEIGHT);
            menu.AddCheatPanel(dropdownPanel);

            // set default selection
            dropdownPanel.GetDropDownSync().SelectedValue.Set(defaultSelection);

            var container = new MainMenuPanelContainer(menu);
            var panelManager = new DynamicPanelManager(dropdownPanel, panelFactory, container, insertionIndex);
            panelManager.RecreateDynamicPanels();

            // update on change
            dropdownPanel.GetDropDownSync().GetCustomDropdown().onValueChanged.AddListener(_ => panelManager.RecreateDynamicPanels());
            return panelManager;
        }

        /// <summary>
        /// Creates panels without auto-adding to the menu (for nested use).
        /// </summary>
        public List<CheatPanel> CreatePanels()
        {
            var panels = new List<CheatPanel>();
            var dropdownPanel = new DropdownPanel(this, dropdownLabel, Constants.DEFAULT_PANEL_HEIGHT);
            panels.Add(dropdownPanel);
            dropdownPanel.GetDropDownSync().SelectedValue.Set(defaultSelection);

            var container = new ListPanelContainer(panels);
            var panelManager = new DynamicPanelManager(dropdownPanel, panelFactory, container);
            dropdownPanel.GetDropDownSync().GetCustomDropdown().onValueChanged.AddListener(_ => panelManager.RecreateDynamicPanels());
            panelManager.RecreateDynamicPanels();
            return panels;
        }

        public List<CheatPanel> CreatePanels(DynamicPanelManager parentManager)
        {
            var panels = new List<CheatPanel>();
            var dropdownPanel = new DropdownPanel(this, dropdownLabel, Constants.DEFAULT_PANEL_HEIGHT);
            panels.Add(dropdownPanel);
            dropdownPanel.GetDropDownSync().SelectedValue.Set(defaultSelection);
            var container = new ListPanelContainer(panels);
            var panelManager = new DynamicPanelManager(dropdownPanel, panelFactory, container, 1, parentManager);
            dropdownPanel.GetDropDownSync().GetCustomDropdown().onValueChanged.AddListener(_ => panelManager.RecreateDynamicPanels());
            panelManager.RecreateDynamicPanels();
            return panels;
        }

        // ISyncedReference / ISyncedValueList implementation
        public int Get() => currentIndex;
        public void Set(int value) => currentIndex = Math.Max(0, Math.Min(value, categoryNames.Count - 1));
        public List<string> GetValueList() => new List<string>(categoryNames);
    }
} 