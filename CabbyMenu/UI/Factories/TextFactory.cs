using CabbyMenu.UI.Modders;
using UnityEngine;
using UnityEngine.UI;

namespace CabbyMenu.UI.Factories
{
    /// <summary>
    /// Factory class for creating and configuring text UI elements.
    /// </summary>
    public class TextFactory
    {
        /// <summary>
        /// Builds a text GameObject with the specified text and default styling.
        /// </summary>
        /// <param name="text">The text to display. Defaults to empty string.</param>
        /// <returns>A tuple containing the GameObject, GameObjectMod, and TextMod for the created text element.</returns>
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