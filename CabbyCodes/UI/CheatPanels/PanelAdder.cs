using CabbyCodes.UI.Factories;
using CabbyCodes.UI.Modders;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace CabbyCodes.UI.CheatPanels
{
    public class PanelAdder
    {
        private static readonly Vector2 middle = new(0.5f, 0.5f);

        public static GameObject AddButton(CheatPanel panel, int siblingIndex, UnityAction action, string buttonText, Vector2 size)
        {
            GameObject buttonPanel = DefaultControls.CreatePanel(new DefaultControls.Resources());
            buttonPanel.name = "Button Panel";
            new ImageMod(buttonPanel.GetComponent<Image>()).SetColor(Color.clear);
            new Fitter(buttonPanel).Attach(panel.cheatPanel);
            buttonPanel.transform.SetSiblingIndex(siblingIndex);

            var buttonPanelLayout = buttonPanel.AddComponent<LayoutElement>();
            buttonPanelLayout.flexibleHeight = 1;
            buttonPanelLayout.minWidth = size.x;

            (GameObject button, _, _) = ButtonFactory.Build(buttonText);
            button.GetComponent<Button>().onClick.AddListener(action);
            new Fitter(button).Attach(buttonPanel).Anchor(middle, middle).Size(size);

            return buttonPanel;
        }

        public static GameObject AddDestroyPanelButton(CheatPanel panel, int siblingIndex, Action additionalAction, string buttonText, Vector2 size)
        {
            return AddButton(panel, siblingIndex, delegate
            {
                UnityEngine.Object.Destroy(panel.cheatPanel);
                additionalAction();
            }, buttonText, size);
        }
    }
}
