using CabbyCodes.Patches;
using CabbyCodes.UI.Modders;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace CabbyCodes.UI.CheatPanels
{
    public class TogglePanel : CheatPanel
    {
        public TogglePanel(BoxedReference IsOn, BasePatch togglePatch, string description) : base(description)
        {
            ToggleButton toggleButton = new(IsOn, togglePatch);
            Fit(toggleButton);
        }

        public TogglePanel(BoxedReference IsOn, Action toggleAction, string description) : base(description)
        {
            ToggleButton toggleButton = new(IsOn, toggleAction);
            Fit(toggleButton);
        }

        private void Fit(ToggleButton toggleButton)
        {
            int width = 120;

            GameObject buttonPanel = DefaultControls.CreatePanel(new DefaultControls.Resources());
            buttonPanel.name = "Toggle Button Panel";
            new ImageMod(buttonPanel.GetComponent<Image>()).SetColor(Color.clear);
            new Fitter(buttonPanel).Attach(cheatPanel);
            buttonPanel.transform.SetAsFirstSibling();

            LayoutElement buttonPanelLayout = buttonPanel.AddComponent<LayoutElement>();
            buttonPanelLayout.flexibleHeight = 1;
            buttonPanelLayout.minWidth = width;

            new Fitter(toggleButton.GetGameObject()).Attach(buttonPanel).Anchor(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f)).Size(new Vector2(width, 60));
        }
    }
}
