using UnityEngine.UI;
using UnityEngine;
using CabbyCodes.UI.Factories;
using CabbyCodes.UI.Modders;

namespace CabbyCodes.UI.CheatPanels
{
    public class CheatPanel
    {
        protected static bool isOdd = true;
        protected static Color color1 = new Color(0.8f, 0.8f, 0.8f);
        protected static Color color2 = new Color(0.7f, 0.7f, 0.7f);

        protected readonly GameObject cheatPanel;
        
        public CheatPanel(string description, float height = 80)
        {
            cheatPanel = DefaultControls.CreatePanel(new DefaultControls.Resources());
            cheatPanel.name = "Cheat Panel";

            LayoutElement imageLayout = cheatPanel.AddComponent<LayoutElement>();
            imageLayout.preferredHeight = height;
            imageLayout.flexibleWidth = 1;

            (GameObject cheatTextObj, _, TextMod textMod) = TextFactory.Build(description);
            textMod.SetAlignment(TextAnchor.MiddleLeft).SetFontStyle(FontStyle.Bold);
            new Fitter(cheatTextObj).Attach(cheatPanel).Anchor(new Vector2(0.2f, 0.5f), new Vector2(0.95f, 0.5f)).Size(new Vector2(0, 50));

            Color thisColor = isOdd ? color1 : color2;
            new ImageMod(cheatPanel.GetComponent<Image>()).SetColor(thisColor);
            isOdd = !isOdd;
        }

        public CheatPanel SetColor(Color color)
        {
            new ImageMod(cheatPanel.GetComponent<Image>()).SetColor(color);
            return this;
        }

        protected static void ResetPattern()
        {
            isOdd = true;
        }

        public GameObject GetGameObject()
        {
            return cheatPanel;
        }
    }
}
