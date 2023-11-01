using CabbyCodes.UI.Modders;
using UnityEngine;
using UnityEngine.UI;

namespace CabbyCodes.UI.Factories
{
    public class TextFactory : GameObjectFactory<TextFactory>
    {
        protected TextMod textMod;

        public TextFactory(string text = "") : base()
        {
            buildInstance = DefaultControls.CreateText(new DefaultControls.Resources());
            textMod = new TextMod(buildInstance.GetComponent<Text>());
            textMod.SetText(text).SetFontSize(36).SetColor(Color.black);
        }

        public TextMod GetTextMod()
        {
            return textMod;
        }

        public new GameObject Build()
        {
            textMod = null;
            return base.Build();
        }
    }
}
