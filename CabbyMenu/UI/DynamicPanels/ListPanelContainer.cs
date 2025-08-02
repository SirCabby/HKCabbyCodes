using CabbyMenu.UI.CheatPanels;
using System.Collections.Generic;

namespace CabbyMenu.UI.DynamicPanels
{
    /// <summary>
    /// IHierarchicalPanelContainer implementation for a simple in-memory list of panels.
    /// </summary>
    public class ListPanelContainer : IHierarchicalPanelContainer
    {
        private readonly List<CheatPanel> panels;
        public ListPanelContainer(List<CheatPanel> panels) => this.panels = panels;
        public int GetPanelIndex(CheatPanel panel) => panels.IndexOf(panel);
        public IReadOnlyList<CheatPanel> GetAllPanels() => panels.AsReadOnly();
        public CheatPanel AddPanel(CheatPanel panel) { panels.Add(panel); return panel; }
        public CheatPanel InsertPanel(CheatPanel panel, int index) { panels.Insert(index, panel); return panel; }
        public void RemovePanel(CheatPanel panel) => panels.Remove(panel);
        
        public List<CheatPanel> DetachPanelsAtRange(int startIndex, int count)
        {
            var detached = new List<CheatPanel>();
            for (int i = 0; i < count && startIndex < panels.Count; i++)
            {
                detached.Add(panels[startIndex]);
                panels.RemoveAt(startIndex);
            }
            return detached;
        }

        public void ReattachPanelsAtRange(List<CheatPanel> attach, int index)
        {
            for (int i = 0; i < attach.Count; i++) panels.Insert(index + i, attach[i]);
        }
    }
} 