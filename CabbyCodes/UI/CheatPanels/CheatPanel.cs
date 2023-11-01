using UnityEngine.UI;
using UnityEngine;
using CabbyCodes.UI.Modders;

namespace CabbyCodes.UI.CheatPanels
{
    public class CheatPanel
    {
        private readonly GameObject cheatPanel;
        private readonly ImageMod imageMod;

        public CheatPanel()
        {
            cheatPanel = DefaultControls.CreatePanel(new DefaultControls.Resources());
            cheatPanel.name = "Cheat Panel";
            //cheatPanel.GetComponent<Image>().color = thisColor;
            //cheatPanel.transform.SetParent(cheatContent.transform, false);
            cheatPanel.transform.localScale = Vector2.one;

            LayoutElement imageLayout = cheatPanel.AddComponent<LayoutElement>();
            imageLayout.preferredHeight = 80;
            imageLayout.flexibleWidth = 1;
        }

        public GameObject GetGameObject()
        {
            return cheatPanel;
        }

        //public GameObject AddCheatPanel()
        //{
        //    Color thisColor = cheatContent.transform.childCount % 2 == 1 ? Color.red : Color.green;

        //    GameObject cheatPanel = DefaultControls.CreatePanel(new DefaultControls.Resources());
        //    cheatPanel.name = "Cheat Panel";
        //    cheatPanel.GetComponent<Image>().color = thisColor;
        //    cheatPanel.transform.SetParent(cheatContent.transform, false);
        //    cheatPanel.transform.localScale = Vector2.one;

        //    LayoutElement imageLayout = cheatPanel.AddComponent<LayoutElement>();
        //    imageLayout.preferredHeight = 80;
        //    imageLayout.flexibleWidth = 1;

        //    return cheatPanel;
        //}
    }
}
