using CabbyMenu.UI.Modders;
using UnityEngine.UI;
using UnityEngine;
using CabbyMenu.UI.Factories;
using System;

namespace CabbyMenu.UI.CheatPanels
{
    public enum ButtonStyle
    {
        Default,
        Danger,
        Success
    }

    public class ButtonPanel : CheatPanel
    {
        private static readonly Vector2 middle = Constants.MIDDLE_ANCHOR_VECTOR;

        private readonly GameObject button;
        private readonly LayoutElement buttonPanelLayout;
        private readonly Action action;

        public ButtonPanel(Action action, string buttonText, string description)
            : this(action, buttonText, description, ButtonStyle.Default)
        {
        }

        public ButtonPanel(Action action, string buttonText, string description, ButtonStyle style) : base(description)
        {
            this.action = action;
            (button, buttonPanelLayout) = CreateButtonPanel(buttonText, style);
        }

        /// <summary>
        /// Creates the button panel with the specified configuration.
        /// </summary>
        private (GameObject button, LayoutElement layout) CreateButtonPanel(string buttonText, ButtonStyle style)
        {
            GameObject buttonPanel = DefaultControls.CreatePanel(new DefaultControls.Resources());
            buttonPanel.name = "Button Panel";
            new ImageMod(buttonPanel.GetComponent<Image>()).SetColor(Color.clear);
            new Fitter(buttonPanel).Attach(cheatPanel);
            buttonPanel.transform.SetAsFirstSibling();

            var layout = buttonPanel.AddComponent<LayoutElement>();
            layout.flexibleHeight = Constants.FLEXIBLE_LAYOUT_VALUE;
            layout.preferredWidth = CalculateButtonWidth(buttonText);
            layout.minWidth = CalculateButtonWidth(buttonText);

            // Debug logging to verify width calculation
            UnityEngine.Debug.Log($"ButtonPanel: buttonText='{buttonText}', calculatedWidth={layout.preferredWidth}");

            (GameObject button, _, _) = GetButtonFactory(style)(buttonText);
            button.GetComponent<Button>().onClick.AddListener(DoAction);
            new Fitter(button).Attach(buttonPanel).Anchor(middle, middle).Size(new Vector2(layout.preferredWidth, Constants.DEFAULT_PANEL_HEIGHT));

            return (button, layout);
        }

        /// <summary>
        /// Gets the appropriate button factory method based on the style.
        /// </summary>
        private static Func<string, (GameObject, GameObjectMod, TextMod)> GetButtonFactory(ButtonStyle style)
        {
            switch (style)
            {
                case ButtonStyle.Danger:
                    return ButtonFactory.BuildDanger;
                case ButtonStyle.Success:
                    return ButtonFactory.BuildSuccess;
                default:
                    return ButtonFactory.BuildDefault;
            }
        }

        public ButtonPanel SetButtonSize(int width)
        {
            buttonPanelLayout.minWidth = width;
            new Fitter(button).Size(new Vector2(width, Constants.DEFAULT_PANEL_HEIGHT));
            Update();
            return this;
        }

        private void DoAction()
        {
            action();
        }

        /// <summary>
        /// Calculates the button width based on text length, ensuring it's never less than the minimum width.
        /// </summary>
        /// <param name="text">The button text.</param>
        /// <returns>The calculated width in pixels.</returns>
        private static int CalculateButtonWidth(string text)
        {
            if (string.IsNullOrEmpty(text))
                return Constants.MIN_PANEL_WIDTH;
                
            float estimatedCharWidth = CalculateCharacterWidth(Constants.DEFAULT_FONT_SIZE);
            
            // Account for padding/margins (add about 20 pixels for button borders/padding)
            float calculatedWidth = (text.Length * estimatedCharWidth) + 20f;
            
            // Ensure the width is never less than the minimum
            return Mathf.Max(Constants.MIN_PANEL_WIDTH, Mathf.RoundToInt(calculatedWidth));
        }

        /// <summary>
        /// Calculates the estimated width of a single character based on font size.
        /// This is used for button sizing calculations.
        /// </summary>
        /// <param name="fontSize">The font size in pixels.</param>
        /// <returns>The estimated character width in pixels.</returns>
        private static float CalculateCharacterWidth(int fontSize)
        {
            // Estimate character width based on font size (more realistic for most fonts)
            // For most fonts, character width is roughly 0.4-0.5 times the font size
            return fontSize * 0.45f;
        }
    }
}