using CabbyCodes.Patches;
using System;
using UnityEngine;

namespace CabbyCodes.UI.CheatPanels
{
    public class TogglePanel : CheatPanel
    {
        public TogglePanel(BoxedReference IsOn, BasePatch togglePatch, string description, float height = 80) : base(description, height)
        {
            ToggleButton toggleButton = new(IsOn, togglePatch);
            Fit(toggleButton);
        }

        public TogglePanel(BoxedReference IsOn, Action toggleAction, string description, float height = 80) : base(description, height)
        {
            ToggleButton toggleButton = new(IsOn, toggleAction);
            Fit(toggleButton);
        }

        private void Fit(ToggleButton toggleButton)
        {
            new Fitter(toggleButton.GetGameObject()).Attach(cheatPanel).Anchor(new Vector2(0.07f, 0.5f), new Vector2(0.07f, 0.5f)).Size(new Vector2(120, 50));
        }
    }
}
