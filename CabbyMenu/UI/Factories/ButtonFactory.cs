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
        /// Builds a button with custom colors for different use cases.
        /// </summary>
        /// <param name="text">The text to display on the button.</param>
        /// <param name="normalColor">The normal state color.</param>
        /// <param name="highlightedColor">The hover state color.</param>
        /// <param name="pressedColor">The pressed state color.</param>
        /// <returns>A tuple containing the GameObject, GameObjectMod, and TextMod for the created button.</returns>
        public static (GameObject gameObject, GameObjectMod gameObjectMod, TextMod textMod) BuildWithColors(
            string text, Color normalColor, Color highlightedColor, Color pressedColor)
        {
            GameObject buildInstance = DefaultControls.CreateButton(new DefaultControls.Resources());

            Button buttonComponent = buildInstance.GetComponent<Button>();
            Image buttonImage = buildInstance.GetComponent<Image>();

            // Apply rounded sprite to the button
            buttonImage.sprite = GetRoundedSprite();
            buttonImage.type = Image.Type.Sliced; // Use sliced type for 9-slice scaling

            // Apply custom colors
            buttonImage.color = normalColor;

            ColorBlock colors = buttonComponent.colors;
            colors.normalColor = normalColor;
            colors.highlightedColor = highlightedColor;
            colors.pressedColor = pressedColor;
            colors.fadeDuration = 0.1f;
            buttonComponent.colors = colors;

            // Add outline for better visibility
            Outline outline = buildInstance.AddComponent<Outline>();
            outline.effectColor = new Color(0f, 0f, 0f, 0.3f);
            outline.effectDistance = new Vector2(2f, 2f);

            TextMod textMod = new(buildInstance.GetComponentInChildren<Text>());
            textMod.SetText(text)
                   .SetFontSize(Constants.DEFAULT_FONT_SIZE)
                   .SetColor(Color.white);

            (GameObject gameObject, GameObjectMod gameObjectMod) = GameObjectFactory.Build(buildInstance);
            return (gameObject, gameObjectMod, textMod);
        }

        /// <summary>
        /// Builds a button GameObject with the specified text and default styling.
        /// </summary>
        /// <param name="text">The text to display on the button. Defaults to empty string.</param>
        /// <returns>A tuple containing the GameObject, GameObjectMod, and TextMod for the created button.</returns>
        public static (GameObject gameObject, GameObjectMod gameObjectMod, TextMod textMod) BuildDefault(string text = "")
        {
            return BuildWithColors(text, Constants.BUTTON_PRIMARY_NORMAL, Constants.BUTTON_PRIMARY_HOVER, Constants.BUTTON_PRIMARY_PRESSED);
        }

        /// <summary>
        /// Builds a success-themed button (green colors) for positive actions.
        /// </summary>
        /// <param name="text">The text to display on the button.</param>
        /// <returns>A tuple containing the GameObject, GameObjectMod, and TextMod for the created button.</returns>
        public static (GameObject gameObject, GameObjectMod gameObjectMod, TextMod textMod) BuildSuccess(string text = "")
        {
            return BuildWithColors(text, Constants.BUTTON_SUCCESS_NORMAL, Constants.BUTTON_SUCCESS_HOVER, Constants.BUTTON_SUCCESS_PRESSED);
        }

        /// <summary>
        /// Builds a danger-themed button (red colors) for destructive actions.
        /// </summary>
        /// <param name="text">The text to display on the button.</param>
        /// <returns>A tuple containing the GameObject, GameObjectMod, and TextMod for the created button.</returns>
        public static (GameObject gameObject, GameObjectMod gameObjectMod, TextMod textMod) BuildDanger(string text = "")
        {
            return BuildWithColors(text, Constants.BUTTON_DANGER_NORMAL, Constants.BUTTON_DANGER_HOVER, Constants.BUTTON_DANGER_PRESSED);
        }

        /// <summary>
        /// Creates a rounded sprite for button backgrounds.
        /// </summary>
        /// <returns>A rounded sprite texture.</returns>
        private static Sprite GetRoundedSprite()
        {
            int size = 128; // Increased texture size for better quality
            Texture2D texture = new Texture2D(size, size);

            // Use a smaller radius percentage for better scaling across different button sizes
            float radius = size * 0.06f; // 6% of size for corner radius
            Vector2 center = new Vector2(size / 2f, size / 2f);

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    Vector2 pixelPos = new Vector2(x, y);
                    Color pixelColor = Color.white;

                    // Check if pixel is in a corner that needs to be rounded
                    bool isInCorner = false;
                    Vector2 cornerCenter = Vector2.zero;

                    // Top-left corner
                    if (x < radius && y > size - radius)
                    {
                        isInCorner = true;
                        cornerCenter = new Vector2(radius, size - radius);
                    }
                    // Top-right corner
                    else if (x > size - radius && y > size - radius)
                    {
                        isInCorner = true;
                        cornerCenter = new Vector2(size - radius, size - radius);
                    }
                    // Bottom-left corner
                    else if (x < radius && y < radius)
                    {
                        isInCorner = true;
                        cornerCenter = new Vector2(radius, radius);
                    }
                    // Bottom-right corner
                    else if (x > size - radius && y < radius)
                    {
                        isInCorner = true;
                        cornerCenter = new Vector2(size - radius, radius);
                    }

                    if (isInCorner)
                    {
                        float distance = Vector2.Distance(pixelPos, cornerCenter);
                        if (distance > radius)
                        {
                            pixelColor.a = 0f; // Transparent outside the rounded corner
                        }
                    }

                    texture.SetPixel(x, y, pixelColor);
                }
            }

            texture.Apply();
            // Use 9-slice sprite for better scaling across different button sizes
            return Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f, 0, SpriteMeshType.FullRect, new Vector4(radius, radius, radius, radius));
        }
    }
}