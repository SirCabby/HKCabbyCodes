using CabbyCodes.SyncedReferences;
using CabbyCodes.UI.Modders;
using UnityEngine;
using UnityEngine.UI;

namespace CabbyCodes.UI.CheatPanels
{
    public class TogglePanel : CheatPanel
    {
        private readonly ToggleButton toggleButton;

        public TogglePanel(SyncedReference<bool> syncedReference, string description) : base(description)
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

            toggleButton = new(syncedReference);
            new Fitter(toggleButton.GetGameObject()).Attach(buttonPanel).Anchor(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f)).Size(new Vector2(width, 60));
        }

        public ToggleButton GetToggleButton()
        {
            return toggleButton;
        }
    }
}
