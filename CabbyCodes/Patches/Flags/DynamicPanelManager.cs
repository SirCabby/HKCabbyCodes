using System;
using System.Collections.Generic;
using CabbyMenu.UI.CheatPanels;

namespace CabbyCodes.Patches.Flags
{
    /// <summary>
    /// Generic manager for handling dynamic panel recreation based on dropdown selections.
    /// This destroys old category panels and creates new ones when the dropdown selection changes,
    /// while preserving the order of other panels in the menu.
    /// </summary>
    public class DynamicPanelManager
    {
        private readonly DropdownPanel dropdownPanel;
        private readonly List<CheatPanel> dynamicPanels = new List<CheatPanel>();
        private readonly List<CheatPanel> detachedPanels = new List<CheatPanel>();
        private readonly Func<int, List<CheatPanel>> panelFactory;
        private readonly int insertionIndex;
        private int lastSelectedIndex = -1;
        private bool panelsDetached = false;

        /// <summary>
        /// Creates a new DynamicPanelManager.
        /// </summary>
        /// <param name="dropdownPanel">The dropdown panel that controls the selection</param>
        /// <param name="panelFactory">Function that creates panels for a given selection index</param>
        /// <param name="insertionIndex">The index where dynamic panels should be inserted (relative to dropdown position)</param>
        public DynamicPanelManager(DropdownPanel dropdownPanel, Func<int, List<CheatPanel>> panelFactory, int insertionIndex = 1)
        {
            this.dropdownPanel = dropdownPanel;
            this.panelFactory = panelFactory;
            this.insertionIndex = insertionIndex;
        }

        /// <summary>
        /// Recreates the dynamic panels based on the current dropdown selection.
        /// This destroys the old category panels and creates new ones, while preserving
        /// the order of other panels in the menu.
        /// </summary>
        public void RecreateDynamicPanels()
        {
            int currentIndex = GetCurrentSelection();
            
            // Only update if the selection has changed
            if (currentIndex == lastSelectedIndex)
                return;

            // Get the dropdown panel index
            int dropdownIndex = CabbyCodesPlugin.cabbyMenu.GetPanelIndex(dropdownPanel);
            if (dropdownIndex == -1)
            {
                // Dropdown panel not found, fall back to simple add/remove
                RemoveDynamicPanels();
                AddNewDynamicPanels(currentIndex);
                lastSelectedIndex = currentIndex;
                return;
            }

            // Calculate the insertion point (relative to dropdown)
            int insertionPoint = dropdownIndex + insertionIndex;

            // If we have existing dynamic panels, remove them and detach panels that come after
            if (dynamicPanels.Count > 0)
            {
                // Remove existing dynamic panels
                RemoveDynamicPanels();
                
                // Detach panels that come after the dynamic section
                DetachPanelsAfterDynamic(insertionPoint);
            }
            else
            {
                // First time - detach panels that come after the dropdown
                DetachPanelsAfterDynamic(insertionPoint);
            }

            // Add new dynamic panels at the correct position
            AddNewDynamicPanelsAtPosition(currentIndex, insertionPoint);

            // Re-attach detached panels if we had any
            if (panelsDetached)
            {
                ReattachDetachedPanels();
            }

            lastSelectedIndex = currentIndex;
        }

        /// <summary>
        /// Gets the current selection from the dropdown.
        /// </summary>
        /// <returns>The current selection index</returns>
        protected virtual int GetCurrentSelection()
        {
            // This can be overridden by subclasses if needed
            return dropdownPanel.GetDropDownSync().SelectedValue.Get();
        }

        private void RemoveDynamicPanels()
        {
            foreach (var panel in dynamicPanels)
            {
                CabbyCodesPlugin.cabbyMenu.RemoveCheatPanel(panel);
            }
            dynamicPanels.Clear();
        }

        private void DetachPanelsAfterDynamic(int insertionPoint)
        {
            // Get all panels that come after the insertion point
            var allPanels = CabbyCodesPlugin.cabbyMenu.GetAllPanels();
            if (allPanels == null || insertionPoint >= allPanels.Count)
            {
                return;
            }

            // Calculate how many panels to detach
            int panelsToDetach = allPanels.Count - insertionPoint;
            
            // Detach and store the panels that come after our insertion point
            detachedPanels.Clear();
            detachedPanels.AddRange(CabbyCodesPlugin.cabbyMenu.DetachPanelsAtRange(insertionPoint, panelsToDetach));
            panelsDetached = true;
        }

        private void AddNewDynamicPanelsAtPosition(int currentIndex, int insertionPoint)
        {
            // Create new panels using the factory
            List<CheatPanel> newPanels = panelFactory(currentIndex);
            
            // Insert panels at the correct position
            for (int i = 0; i < newPanels.Count; i++)
            {
                CheatPanel panel = newPanels[i];
                CheatPanel addedPanel = CabbyCodesPlugin.cabbyMenu.InsertCheatPanel(panel, insertionPoint + i);
                dynamicPanels.Add(addedPanel);
            }
        }

        private void AddNewDynamicPanels(int currentIndex)
        {
            // Create new panels using the factory
            List<CheatPanel> newPanels = panelFactory(currentIndex);
            
            // Add panels to the end
            foreach (CheatPanel panel in newPanels)
            {
                CheatPanel addedPanel = CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
                dynamicPanels.Add(addedPanel);
            }
        }

        private void ReattachDetachedPanels()
        {
            if (detachedPanels.Count > 0)
            {
                // Calculate where to reattach (after the dynamic panels)
                int dropdownIndex = CabbyCodesPlugin.cabbyMenu.GetPanelIndex(dropdownPanel);
                int reattachPoint = dropdownIndex + insertionIndex + dynamicPanels.Count;
                
                CabbyCodesPlugin.cabbyMenu.ReattachPanelsAtRange(detachedPanels, reattachPoint);
                detachedPanels.Clear();
                panelsDetached = false;
            }
        }

        /// <summary>
        /// Gets the dropdown panel associated with this manager.
        /// </summary>
        public DropdownPanel GetDropdownPanel()
        {
            return dropdownPanel;
        }

        /// <summary>
        /// Gets the currently active dynamic panels.
        /// </summary>
        public IReadOnlyList<CheatPanel> GetDynamicPanels()
        {
            return dynamicPanels.AsReadOnly();
        }
    }
} 