using UnityEngine;
using UnityEngine.UI;

namespace CabbyCodes.UI.Modders
{
    public class TextMod
    {
        Text text;

        public TextMod(Text text)
        {
            this.text = text;
        }

        public Text Get()
        {
            return text;
        }

        public TextMod SetText(string text)
        {
            this.text.text = text;
            return this;
        }

        public TextMod SetFontSize(int size)
        {
            text.fontSize = size;
            return this;
        }

        public TextMod SetColor(Color color)
        {
            text.color = color;
            return this;
        }

        public TextMod SetAlignment(TextAnchor alignment)
        {
            text.alignment = alignment;
            return this;
        }

        public TextMod SetFontStyle(FontStyle fontStyle)
        {
            text.fontStyle = fontStyle;
            return this;
        }
    }
}
