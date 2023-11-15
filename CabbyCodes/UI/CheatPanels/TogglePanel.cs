using CabbyCodes.SyncedReferences;
using CabbyCodes.UI.Modders;
using CabbyCodes.UI.ReferenceControls;
using UnityEngine;
using UnityEngine.UI;

namespace CabbyCodes.UI.CheatPanels
{
    public class TogglePanel : CheatPanel
    {
        private static readonly Vector2 defaultSize = new(120, 60);
        private static readonly Vector2 middle = new(0.5f, 0.5f);

        private readonly ToggleButton toggleButton;

        public TogglePanel(ISyncedReference<bool> syncedReference, string description) : base(description)
        {
            GameObject buttonPanel = DefaultControls.CreatePanel(new DefaultControls.Resources());
            buttonPanel.name = "Toggle Button Panel";
            new ImageMod(buttonPanel.GetComponent<Image>()).SetColor(Color.clear);
            new Fitter(buttonPanel).Attach(cheatPanel);
            buttonPanel.transform.SetAsFirstSibling();

            LayoutElement buttonPanelLayout = buttonPanel.AddComponent<LayoutElement>();
            buttonPanelLayout.flexibleHeight = 1;
            buttonPanelLayout.minWidth = defaultSize.x;

            toggleButton = new(syncedReference);
            new Fitter(toggleButton.GetGameObject()).Attach(buttonPanel).Anchor(middle, middle).Size(defaultSize);
            updateActions.Add(toggleButton.Update);
        }

        public ToggleButton GetToggleButton()
        {
            return toggleButton;
        }
    }
}
