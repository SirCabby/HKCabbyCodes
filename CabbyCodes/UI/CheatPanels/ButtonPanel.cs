using CabbyCodes.SyncedReferences;
using CabbyCodes.UI.Modders;
using UnityEngine.UI;
using UnityEngine;
using CabbyCodes.UI.Factories;
using System;

namespace CabbyCodes.UI.CheatPanels
{
    public class ButtonPanel : CheatPanel
    {
        private readonly GameObject button;
        private readonly Action action;

        public ButtonPanel(Action action, string buttonText, string description) : base(description)
        {
            this.action = action;
            int width = 120;

            GameObject buttonPanel = DefaultControls.CreatePanel(new DefaultControls.Resources());
            buttonPanel.name = "Button Panel";
            new ImageMod(buttonPanel.GetComponent<Image>()).SetColor(Color.clear);
            new Fitter(buttonPanel).Attach(cheatPanel);
            buttonPanel.transform.SetAsFirstSibling();

            LayoutElement buttonPanelLayout = buttonPanel.AddComponent<LayoutElement>();
            buttonPanelLayout.flexibleHeight = 1;
            buttonPanelLayout.minWidth = width;

            (button, _, _) = ButtonFactory.Build(buttonText);
            button.GetComponent<Button>().onClick.AddListener(DoAction);
            new Fitter(button).Attach(buttonPanel).Anchor(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f)).Size(new Vector2(width, 60));
        }

        private void DoAction()
        {
            action();
        }
    }
}
