using UnityEngine;
using UnityEngine.UI;

namespace CabbyCodes.UI.Modders
{
    public class ScrollBarMod
    {
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
            scrollBarColors.normalColor = new Color(0.3f, 0.3f, 1, 1);
            scrollBarColors.highlightedColor = new Color(0, 0, 1, 1);
            scrollbar.colors = scrollBarColors;

            return this;
        }
    }
}
