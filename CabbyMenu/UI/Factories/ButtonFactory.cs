using CabbyMenu.UI.Modders;
using UnityEngine;
using UnityEngine.UI;

namespace CabbyMenu.UI.Factories
{
    /// <summary>
    /// Factory class for creating and configuring button UI elements.
    /// </summary>
    public class ButtonFactory
    {
        /// <summary>
        /// Builds a button GameObject with the specified text and default styling.
        /// </summary>
        /// <param name="text">The text to display on the button. Defaults to empty string.</param>
        /// <returns>A tuple containing the GameObject, GameObjectMod, and TextMod for the created button.</returns>
        public static (GameObject gameObject, GameObjectMod gameObjectMod, TextMod textMod) Build(string text = "")
        {
            GameObject buildInstance = DefaultControls.CreateButton(new DefaultControls.Resources());
            TextMod textMod = new(buildInstance.GetComponentInChildren<Text>());
            textMod.SetText(text).SetFontSize(Constants.DEFAULT_FONT_SIZE).SetColor(Constants.BLACK_COLOR);

            (GameObject gameObject, GameObjectMod gameObjectMod) = GameObjectFactory.Build(buildInstance);
            return (gameObject, gameObjectMod, textMod);
        }
    }
}