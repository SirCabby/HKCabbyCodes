using System;
using System.Collections.Generic;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.SyncedReferences;

namespace CabbyCodes.Patches.Flags
{
    /// <summary>
    /// Helper class for creating categorized panel sections with dropdown selectors.
    /// This provides a clean way to define sections of panels that can be dynamically recreated
    /// when the dropdown selection changes.
    /// </summary>
    public class CategorizedPanelSection : ISyncedReference<int>, ISyncedValueList
    {
        private readonly string sectionName;
        private readonly string dropdownLabel;
        private readonly List<string> categoryNames;
        private readonly Func<int, List<CheatPanel>> panelFactory;
        private readonly int insertionIndex;
        private readonly int defaultSelection;
        private int currentIndex = 0;

        /// <summary>
        /// Creates a new categorized panel section.
        /// </summary>
        /// <param name="sectionName">The name of the section (for headers)</param>
        /// <param name="dropdownLabel">The label for the dropdown</param>
        /// <param name="categoryNames">The list of category names for the dropdown</param>
        /// <param name="panelFactory">Function that creates panels for a given category index</param>
        /// <param name="insertionIndex">The index where dynamic panels should be inserted (relative to dropdown position)</param>
        /// <param name="defaultSelection">The default selection index for the dropdown</param>
        public CategorizedPanelSection(string sectionName, string dropdownLabel, List<string> categoryNames, 
            Func<int, List<CheatPanel>> panelFactory, int insertionIndex = 1, int defaultSelection = 0)
        {
            this.sectionName = sectionName;
            this.dropdownLabel = dropdownLabel;
            this.categoryNames = categoryNames;
            this.panelFactory = panelFactory;
            this.insertionIndex = insertionIndex;
            this.defaultSelection = defaultSelection;
            this.currentIndex = defaultSelection;
        }

        /// <summary>
        /// Adds this categorized section to the menu.
        /// </summary>
        /// <returns>The created DynamicPanelManager for this section</returns>
        public DynamicPanelManager AddToMenu()
        {
            // Add section header
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel(sectionName).SetColor(CheatPanel.headerColor));

            // Create dropdown panel using this instance as the selector
            var dropdownPanel = new DropdownPanel(this, dropdownLabel, Constants.DEFAULT_PANEL_HEIGHT);
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(dropdownPanel);

            // Set the dropdown to the default selection
            dropdownPanel.GetDropDownSync().SelectedValue.Set(defaultSelection);

            // Create dynamic panel manager
            var panelManager = new DynamicPanelManager(dropdownPanel, panelFactory, insertionIndex);

            // Trigger initial panel creation
            panelManager.RecreateDynamicPanels();

            // Add update actions
            dropdownPanel.updateActions.Add(panelManager.RecreateDynamicPanels);
            dropdownPanel.GetDropDownSync().GetCustomDropdown().onValueChanged.AddListener(_ => panelManager.RecreateDynamicPanels());

            return panelManager;
        }

        /// <summary>
        /// Gets the category names for this section.
        /// </summary>
        public IReadOnlyList<string> GetCategoryNames()
        {
            return categoryNames.AsReadOnly();
        }

        public int Get()
        {
            return currentIndex;
        }

        public void Set(int value)
        {
            currentIndex = Math.Max(0, Math.Min(value, categoryNames.Count - 1));
        }

        public List<string> GetValueList()
        {
            return new List<string>(categoryNames);
        }
    }
} 