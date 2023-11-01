using CabbyCodes.UI.Modders;
using UnityEngine;
using UnityEngine.UI;

namespace CabbyCodes.UI.Factories
{
    public class ButtonFactory : GameObjectFactory<ButtonFactory>
    {
        protected TextMod textMod;

        public ButtonFactory(string text = "") : base()
        {
            buildInstance = DefaultControls.CreateButton(new DefaultControls.Resources());
            buildInstance.GetComponentInChildren<Text>();
            textMod = new TextMod(buildInstance.GetComponentInChildren<Text>());
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
