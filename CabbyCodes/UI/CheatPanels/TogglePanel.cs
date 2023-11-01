using UnityEngine;

namespace CabbyCodes.UI.CheatPanels
{
    public class TogglePanel : CheatPanel
    {
        public TogglePanel(string description, float height = 80) : base(description, height)
        {
            ToggleButton cheat1ToggleButton = new();
            //cheat1ToggleButton.GetComponent<Button>().onClick.AddListener(OnMenuButtonClicked);
            new Fitter(cheat1ToggleButton.GetGameObject()).Attach(cheatPanel).Anchor(new Vector2(0.07f, 0.5f), new Vector2(0.07f, 0.5f)).Size(new Vector2(120, 50));
        }
    }
}
