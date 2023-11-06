using UnityEngine;

namespace CabbyCodes.UI.CheatPanels
{
    public class InfoPanel : CheatPanel
    {
        private static Color infoColor = new(1, 0.5f, 0);

        public InfoPanel(string description) : base(description)
        {
            SetColor(infoColor);
            ResetPattern();
        }
    }
}
