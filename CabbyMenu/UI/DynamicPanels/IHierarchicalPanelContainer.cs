using System.Collections.Generic;
using CabbyMenu.UI.CheatPanels;

namespace CabbyMenu.UI.DynamicPanels
{
    /// <summary>
    /// Abstraction for containers that hold cheat panels (main menu or sub-list).
    /// </summary>
    public interface IHierarchicalPanelContainer
    {
        int GetPanelIndex(CheatPanel panel);
        IReadOnlyList<CheatPanel> GetAllPanels();
        CheatPanel AddPanel(CheatPanel panel);
        CheatPanel InsertPanel(CheatPanel panel, int index);
        void RemovePanel(CheatPanel panel);
        List<CheatPanel> DetachPanelsAtRange(int startIndex, int count);
        void ReattachPanelsAtRange(List<CheatPanel> panels, int index);
    }
} 