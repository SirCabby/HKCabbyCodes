using UnityEngine.UI;
using UnityEngine;

namespace CabbyCodes.UI.CheatPanels
{
    public class CheatPanel
    {
        private readonly GameObject cheatPanel;

        public CheatPanel(float height)
        {
            cheatPanel = DefaultControls.CreatePanel(new DefaultControls.Resources());
            cheatPanel.name = "Cheat Panel";
            cheatPanel.transform.localScale = Vector2.one;

            LayoutElement imageLayout = cheatPanel.AddComponent<LayoutElement>();
            imageLayout.preferredHeight = height;
            imageLayout.flexibleWidth = 1;
        }

        public GameObject GetGameObject()
        {
            return cheatPanel;
        }
    }
}
