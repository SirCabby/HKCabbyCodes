using System;
using System.Collections.Generic;
using System.Linq;
using CabbyMenu.UI.CheatPanels;

namespace CabbyMenu.UI.DynamicPanels
{
    /// <summary>
    /// Hierarchical manager for handling dynamic panel recreation based on dropdown selections.
    /// Supports unlimited nesting levels where each manager can have child managers.
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
        private bool isSuspended = false;

        // Hierarchical support
        private readonly DynamicPanelManager parentManager;
        private readonly List<DynamicPanelManager> childManagers = new List<DynamicPanelManager>();
        private readonly IHierarchicalPanelContainer container;
        private readonly Action onPanelsChanged;

        /// <summary>
        /// Creates a new DynamicPanelManager for hierarchical operation.
        /// </summary>
        public DynamicPanelManager(
            DropdownPanel dropdownPanel,
            Func<int, List<CheatPanel>> panelFactory,
            IHierarchicalPanelContainer container,
            int insertionIndex = 1,
            DynamicPanelManager parentManager = null,
            Action onPanelsChanged = null)
        {
            this.dropdownPanel = dropdownPanel;
            this.panelFactory = panelFactory;
            this.container = container;
            this.insertionIndex = insertionIndex;
            this.parentManager = parentManager;
            this.onPanelsChanged = onPanelsChanged;

            // Register with parent if we have one
            parentManager?.AddChildManager(this);
            DynamicPanelCoordinator.RegisterManager(this);
        }

        public void CreateInitialPanels(int initialIndex)
        {
            if (isSuspended) return;
            DynamicPanelCoordinator.SuspendOtherManagers(this);

            int dropdownIndex = container.GetPanelIndex(dropdownPanel);
            if (dropdownIndex == -1)
            {
                AddNewDynamicPanels(initialIndex);
                DynamicPanelCoordinator.ResumeAllManagers();
                lastSelectedIndex = initialIndex;
                return;
            }

            int insertionPoint = dropdownIndex + insertionIndex;
            DetachPanelsAfterDynamic(insertionPoint);
            AddNewDynamicPanelsAtPosition(initialIndex, insertionPoint);
            if (panelsDetached) ReattachDetachedPanels();
            DynamicPanelCoordinator.ResumeAllManagers();
            lastSelectedIndex = initialIndex;
        }

        public void RecreateDynamicPanels()
        {
            if (isSuspended) return;
            int currentIndex = GetCurrentSelection();
            if (currentIndex == lastSelectedIndex) return;
            DynamicPanelCoordinator.SuspendOtherManagers(this);

            int dropdownIndex = container.GetPanelIndex(dropdownPanel);
            if (dropdownIndex == -1)
            {
                RemoveDynamicPanels();
                AddNewDynamicPanels(currentIndex);
                DynamicPanelCoordinator.ResumeAllManagers();
                lastSelectedIndex = currentIndex;
                return;
            }

            int insertionPoint = dropdownIndex + insertionIndex;
            if (dynamicPanels.Count > 0)
            {
                RemoveDynamicPanels();
                DetachPanelsAfterDynamic(insertionPoint);
            }
            else
            {
                DetachPanelsAfterDynamic(insertionPoint);
            }
            AddNewDynamicPanelsAtPosition(currentIndex, insertionPoint);
            if (panelsDetached) ReattachDetachedPanels();
            DynamicPanelCoordinator.ResumeAllManagers();
            lastSelectedIndex = currentIndex;
        }

        protected virtual int GetCurrentSelection() => dropdownPanel.GetDropDownSync().SelectedValue.Get();

        public void AddChildManager(DynamicPanelManager childManager)
        {
            if (!childManagers.Contains(childManager)) childManagers.Add(childManager);
        }
        public void RemoveChildManager(DynamicPanelManager childManager) => childManagers.Remove(childManager);
        public IReadOnlyList<DynamicPanelManager> GetChildManagers() => childManagers.AsReadOnly();

        private void RemoveDynamicPanels()
        {
            foreach (var child in childManagers.ToList()) child.Cleanup();
            childManagers.Clear();
            foreach (var panel in dynamicPanels) container.RemovePanel(panel);
            dynamicPanels.Clear();
        }

        private void DetachPanelsAfterDynamic(int insertionPoint)
        {
            var allPanels = container.GetAllPanels();
            if (allPanels == null || insertionPoint >= allPanels.Count) return;
            int count = allPanels.Count - insertionPoint;
            detachedPanels.Clear();
            detachedPanels.AddRange(container.DetachPanelsAtRange(insertionPoint, count));
            panelsDetached = true;
        }

        private void AddNewDynamicPanelsAtPosition(int index, int insertionPoint)
        {
            var newPanels = panelFactory(index);
            for (int i = 0; i < newPanels.Count; i++)
            {
                var added = container.InsertPanel(newPanels[i], insertionPoint + i);
                dynamicPanels.Add(added);
            }
            onPanelsChanged?.Invoke();
        }

        private void AddNewDynamicPanels(int index)
        {
            var newPanels = panelFactory(index);
            foreach (var p in newPanels)
            {
                var added = container.AddPanel(p);
                dynamicPanels.Add(added);
            }
            onPanelsChanged?.Invoke();
        }

        private void ReattachDetachedPanels()
        {
            if (detachedPanels.Count == 0) return;
            int dropdownIndex = container.GetPanelIndex(dropdownPanel);
            int reattachPoint = dropdownIndex + insertionIndex + dynamicPanels.Count;
            container.ReattachPanelsAtRange(detachedPanels, reattachPoint);
            detachedPanels.Clear();
            panelsDetached = false;
        }

        public DropdownPanel GetDropdownPanel() => dropdownPanel;
        public IReadOnlyList<CheatPanel> GetDynamicPanels() => dynamicPanels.AsReadOnly();

        public void Cleanup()
        {
            foreach (var child in childManagers.ToList()) child.Cleanup();
            childManagers.Clear();
            RemoveDynamicPanels();
            if (panelsDetached) ReattachDetachedPanels();
            parentManager?.RemoveChildManager(this);
            DynamicPanelCoordinator.UnregisterManager(this);
        }

        public void Suspend()
        {
            isSuspended = true;
            foreach (var child in childManagers) child.Suspend();
        }

        public void Resume()
        {
            isSuspended = false;
            foreach (var child in childManagers) child.Resume();
        }
    }
} 