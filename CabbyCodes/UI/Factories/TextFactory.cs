using CabbyCodes.UI.Modders;
using UnityEngine;
using UnityEngine.UI;

namespace CabbyCodes.UI.Factories
{
    public class TextFactory
    {
        public static (GameObject gameObject, GameObjectMod gameObjectMod, TextMod textMod) Build(string text = "")
        {
            GameObject buildInstance = DefaultControls.CreateText(new DefaultControls.Resources());
            TextMod textMod = new(buildInstance.GetComponent<Text>());
            textMod.SetText(text).SetFontSize(36).SetColor(Color.black);

            (GameObject gameObject, GameObjectMod gameObjectMod) = GameObjectFactory.Build(buildInstance);
            return (gameObject, gameObjectMod, textMod);
        }
    }
}
