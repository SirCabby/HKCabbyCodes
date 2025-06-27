using UnityEngine;
using UnityEngine.UI;

namespace CabbyMenu.UI.Modders
{
    public class ScrollBarMod
    {
        private static readonly Color normalColor = Constants.NORMAL_SCROLLBAR_COLOR;
        private static readonly Color highlightColor = Constants.HIGHLIGHT_SCROLLBAR_COLOR;

        private readonly Scrollbar scrollbar;

        public ScrollBarMod(Scrollbar scrollbar)
        {
            this.scrollbar = scrollbar;
        }

        public Scrollbar Get()
        {
            return scrollbar;
        }

        public ScrollBarMod SetDefaults()
        {
            ColorBlock scrollBarColors = scrollbar.colors;
            scrollBarColors.normalColor = normalColor;
            scrollBarColors.highlightedColor = highlightColor;
            scrollbar.colors = scrollBarColors;

            return this;
        }
    }
}