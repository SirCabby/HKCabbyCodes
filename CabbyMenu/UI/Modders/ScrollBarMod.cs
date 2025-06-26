using UnityEngine;
using UnityEngine.UI;

namespace CabbyMenu.UI.Modders
{
    public class ScrollBarMod
    {
        private static readonly Color normalColor = new(0.3f, 0.3f, 1, 1);
        private static readonly Color highlightColor = new(0, 0, 1, 1);

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