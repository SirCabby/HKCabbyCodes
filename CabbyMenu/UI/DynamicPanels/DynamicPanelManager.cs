using System;
using System.Collections.Generic;
using System.Linq;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.UI.Popups;
using CabbyMenu.Utilities;

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
        
        // Menu reference for popup creation
        private readonly CabbyMainMenu menu;

        /// <summary>
        /// Creates a new DynamicPanelManager for hierarchical operation.
        /// </summary>
        public DynamicPanelManager(
            DropdownPanel dropdownPanel,
            Func<int, List<CheatPanel>> panelFactory,
            IHierarchicalPanelContainer container,
            int insertionIndex = 1,
            DynamicPanelManager parentManager = null,
            Action onPanelsChanged = null,
            CabbyMainMenu menu = null)
        {
            this.dropdownPanel = dropdownPanel;
            this.panelFactory = panelFactory;
            this.container = container;
            this.insertionIndex = insertionIndex;
            this.parentManager = parentManager;
            this.onPanelsChanged = onPanelsChanged;
            this.menu = menu;

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
            
            // Show loading popup for any dynamic panel manager if we have menu access
            PopupBase loadingPopup = null;
            if (menu != null)
            {
                loadingPopup = CreateLoadingPopup();
                if (loadingPopup != null)
                {
                    loadingPopup.Show();
                    UnityEngine.Canvas.ForceUpdateCanvases();
                    
                    // Delay panel destruction by one frame to give UI time to render popup
                    CoroutineRunner.RunNextFrame(() => {
                        RecreateDynamicPanelsInternal(currentIndex, loadingPopup);
                    });
                    return;
                }
            }
            
            // No popup needed, proceed immediately
            RecreateDynamicPanelsInternal(currentIndex, null);
        }
        
        private void RecreateDynamicPanelsInternal(int currentIndex, PopupBase loadingPopup)
        {
            DynamicPanelCoordinator.SuspendOtherManagers(this);

            try
            {
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
            finally
            {
                // Hide loading popup if it was created
                if (loadingPopup != null)
                {
                    CoroutineRunner.RunNextFrame(() => {
                        loadingPopup.Hide();
                        loadingPopup.Destroy();
                    });
                }
            }
        }

        /// <summary>
        /// Creates a loading popup for dynamic panel changes
        /// </summary>
        private PopupBase CreateLoadingPopup()
        {
            if (menu != null)
            {
                var popup = new PopupBase(menu, "Loading", "Loading . . .", 400f, 200f);
                
                // Customize the popup appearance using the new styling methods
                popup.SetPanelBackgroundColor(new UnityEngine.Color(0.2f, 0.4f, 0.8f, 1f)); // Blue background
                popup.SetMessageBold(); // Make message text bold
                
                return popup;
            }
            return null;
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