using System.Collections.Generic;
using System.Linq;

namespace CabbyMenu.UI.DynamicPanels
{
    /// <summary>
    /// Static coordinator for managing multiple DynamicPanelManager instances.
    /// Ensures proper coordination when multiple managers are active.
    /// </summary>
    public static class DynamicPanelCoordinator
    {
        private static readonly List<DynamicPanelManager> activeManagers = new List<DynamicPanelManager>();
        private static readonly object lockObject = new object();

        public static void RegisterManager(DynamicPanelManager manager)
        {
            lock (lockObject)
            {
                activeManagers.Add(manager);
            }
        }

        public static void UnregisterManager(DynamicPanelManager manager)
        {
            lock (lockObject)
            {
                activeManagers.Remove(manager);
            }
        }

        public static void CleanupAllManagers()
        {
            lock (lockObject)
            {
                foreach (var manager in activeManagers.ToList())
                {
                    manager.Cleanup();
                }
                activeManagers.Clear();
            }
        }

        public static void SuspendOtherManagers(DynamicPanelManager currentManager)
        {
            lock (lockObject)
            {
                foreach (var manager in activeManagers)
                {
                    if (manager != currentManager)
                    {
                        manager.Suspend();
                    }
                }
            }
        }

        public static void ResumeAllManagers()
        {
            lock (lockObject)
            {
                foreach (var manager in activeManagers)
                {
                    manager.Resume();
                }
            }
        }

        public static IReadOnlyList<DynamicPanelManager> GetAllManagers()
        {
            lock (lockObject)
            {
                return activeManagers.AsReadOnly();
            }
        }
    }
} 