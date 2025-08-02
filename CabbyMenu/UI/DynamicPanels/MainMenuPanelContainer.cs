using CabbyMenu.UI.CheatPanels;
using System.Collections.Generic;

namespace CabbyMenu.UI.DynamicPanels
{
    /// <summary>
    /// IHierarchicalPanelContainer implementation that proxies to a CabbyMainMenu instance.
    /// </summary>
    public class MainMenuPanelContainer : IHierarchicalPanelContainer
    {
        private readonly CabbyMainMenu menu;
        public MainMenuPanelContainer(CabbyMainMenu menu) => this.menu = menu;
        public int GetPanelIndex(CheatPanel panel) => menu.GetPanelIndex(panel);
        public IReadOnlyList<CheatPanel> GetAllPanels() => menu.GetAllPanels();
        public CheatPanel AddPanel(CheatPanel panel) => menu.AddCheatPanel(panel);
        public CheatPanel InsertPanel(CheatPanel panel, int index) => menu.InsertCheatPanel(panel, index);
        public void RemovePanel(CheatPanel panel) => menu.RemoveCheatPanel(panel);
        public List<CheatPanel> DetachPanelsAtRange(int startIndex, int count) => menu.DetachPanelsAtRange(startIndex, count);
        public void ReattachPanelsAtRange(List<CheatPanel> panels, int index) => menu.ReattachPanelsAtRange(panels, index);
    }
} 