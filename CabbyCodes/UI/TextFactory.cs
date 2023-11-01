using UnityEngine;
using UnityEngine.UI;

namespace CabbyCodes.UI
{
    public class TextFactory
    {
        protected GameObject buildInstance;

        protected TextFactory(GameObject buildInstance, string text) 
        {
            this.buildInstance = buildInstance;
            SetText(buildInstance.GetComponentInChildren<Text>(), text);
        }

        public TextFactory(string text)
        {
            buildInstance = DefaultControls.CreateText(new DefaultControls.Resources());
            SetText(buildInstance.GetComponent<Text>(), text);
        }

        private void SetText(Text text, string str)
        {
            text.text = str;
            text.fontSize = 36;
            text.color = Color.black;
        }

        public TextFactory SetName(string name)
        {
            buildInstance.name = name;
            return this;
        }

        public TextFactory SetFontSize(int size)
        {
            Text categoryText = buildInstance.GetComponent<Text>();
            categoryText.fontSize = size;
            return this;
        }

        public TextFactory SetColor(Color color)
        {
            Text categoryText = buildInstance.GetComponent<Text>();
            categoryText.color = color;
            return this;
        }

        public TextFactory SetAlignment(TextAnchor alignment)
        {
            Text categoryText = buildInstance.GetComponent<Text>();
            categoryText.alignment = alignment;
            return this;
        }

        public TextFactory SetFontStyle(FontStyle fontStyle)
        {
            Text categoryText = buildInstance.GetComponent<Text>();
            categoryText.fontStyle = fontStyle;
            return this;
        }

        public GameObject Build()
        {
            GameObject result = buildInstance;
            buildInstance = null;
            return result;
        }

    }
}
