using UnityEngine;
using UnityEngine.UI;

namespace CabbyMenu.UI.Modders
{
    public class TextMod
    {
        private readonly Text text;

        public TextMod(Text text)
        {
            this.text = text;
        }

        /// <summary>
        /// Builds a text GameObject with the specified text and default styling.
        /// </summary>
        /// <param name="text">The text to display. Defaults to empty string.</param>
        /// <returns>A tuple containing the GameObject, GameObjectMod, and TextMod for the created text element.</returns>
        public static (GameObject gameObject, GameObjectMod gameObjectMod, TextMod textMod) Build(string text = "")
        {
            GameObject textGameObject = DefaultControls.CreateText(new DefaultControls.Resources());
            GameObjectMod gameObjectMod = new GameObjectMod(textGameObject);
            TextMod textMod = new TextMod(textGameObject.GetComponent<Text>());
            textMod.SetText(text).SetFontSize(Constants.DEFAULT_FONT_SIZE).SetColor(Color.black);

            return (textGameObject, gameObjectMod, textMod);
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