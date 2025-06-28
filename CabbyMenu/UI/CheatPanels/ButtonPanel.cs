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

        public ButtonPanel(Action action, string buttonText, string description, int width = Constants.DEFAULT_PANEL_WIDTH)
            : this(action, buttonText, description, ButtonStyle.Default, width)
        {
        }

        public ButtonPanel(Action action, string buttonText, string description, ButtonStyle style, int width = Constants.DEFAULT_PANEL_WIDTH) : base(description)
        {
            this.action = action;
            (button, buttonPanelLayout) = CreateButtonPanel(buttonText, style, width);
        }

        /// <summary>
        /// Creates the button panel with the specified configuration.
        /// </summary>
        private (GameObject button, LayoutElement layout) CreateButtonPanel(string buttonText, ButtonStyle style, int width)
        {
            GameObject buttonPanel = DefaultControls.CreatePanel(new DefaultControls.Resources());
            buttonPanel.name = "Button Panel";
            new ImageMod(buttonPanel.GetComponent<Image>()).SetColor(Color.clear);
            new Fitter(buttonPanel).Attach(cheatPanel);
            buttonPanel.transform.SetAsFirstSibling();

            var layout = buttonPanel.AddComponent<LayoutElement>();
            layout.flexibleHeight = Constants.FLEXIBLE_LAYOUT_VALUE;
            layout.minWidth = width;

            (GameObject button, _, _) = GetButtonFactory(style)(buttonText);
            button.GetComponent<Button>().onClick.AddListener(DoAction);
            new Fitter(button).Attach(buttonPanel).Anchor(middle, middle).Size(new Vector2(width, Constants.DEFAULT_PANEL_HEIGHT));

            return (button, layout);
        }

        /// <summary>
        /// Gets the appropriate button factory method based on the style.
        /// </summary>
        private static Func<string, (GameObject, GameObjectMod, TextMod)> GetButtonFactory(ButtonStyle style)
        {
            return style switch
            {
                ButtonStyle.Danger => ButtonFactory.BuildDanger,
                ButtonStyle.Success => ButtonFactory.BuildSuccess,
                _ => ButtonFactory.BuildDefault
            };
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
    }
}