using CabbyCodes.Patches;
using System;

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
            new Fitter(toggleButton.GetGameObject()).Attach(cheatPanel);
            toggleButton.GetGameObject().transform.SetAsFirstSibling();
        }
    }
}
