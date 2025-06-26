using CabbyMenu.UI.Modders;
using UnityEngine.UI;
using UnityEngine;
using CabbyMenu.UI.Factories;
using System;

namespace CabbyMenu.UI.CheatPanels
{
    public class ButtonPanel : CheatPanel
    {
        private static readonly Vector2 middle = new(0.5f, 0.5f);

        private readonly GameObject button;
        private readonly LayoutElement buttonPanelLayout;
        private readonly Action action;

        public ButtonPanel(Action action, string buttonText, string description, int width = 120) : base(description)
        {
            this.action = action;

            GameObject buttonPanel = DefaultControls.CreatePanel(new DefaultControls.Resources());
            buttonPanel.name = "Button Panel";
            new ImageMod(buttonPanel.GetComponent<Image>()).SetColor(Color.clear);
            new Fitter(buttonPanel).Attach(cheatPanel);
            buttonPanel.transform.SetAsFirstSibling();

            buttonPanelLayout = buttonPanel.AddComponent<LayoutElement>();
            buttonPanelLayout.flexibleHeight = 1;
            buttonPanelLayout.minWidth = width;

            (button, _, _) = ButtonFactory.Build(buttonText);
            button.GetComponent<Button>().onClick.AddListener(DoAction);
            new Fitter(button).Attach(buttonPanel).Anchor(middle, middle).Size(new Vector2(width, Constants.DEFAULT_PANEL_HEIGHT));
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