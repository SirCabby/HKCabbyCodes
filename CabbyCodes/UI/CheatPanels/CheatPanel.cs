using UnityEngine.UI;
using UnityEngine;
using CabbyCodes.UI.Factories;
using CabbyCodes.UI.Modders;

namespace CabbyCodes.UI.CheatPanels
{
    public class CheatPanel
    {
        protected readonly GameObject cheatPanel;

        public CheatPanel(string description, float height = 80)
        {
            cheatPanel = DefaultControls.CreatePanel(new DefaultControls.Resources());
            cheatPanel.name = "Cheat Panel";

            LayoutElement imageLayout = cheatPanel.AddComponent<LayoutElement>();
            imageLayout.preferredHeight = height;
            imageLayout.flexibleWidth = 1;

            (GameObject cheat1TextObj, _, TextMod textMod) = TextFactory.Build(description);
            textMod.SetAlignment(TextAnchor.MiddleLeft).SetFontStyle(FontStyle.Bold);
            new Fitter(cheat1TextObj).Attach(cheatPanel).Anchor(new Vector2(0.2f, 0.5f), new Vector2(0.95f, 0.5f)).Size(new Vector2(0, 50));
        }

        public GameObject GetGameObject()
        {
            return cheatPanel;
        }
    }
}
