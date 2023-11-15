using UnityEngine.UI;
using UnityEngine;
using CabbyCodes.UI.Factories;
using CabbyCodes.UI.Modders;
using System.Collections.Generic;
using System;

namespace CabbyCodes.UI.CheatPanels
{
    public class CheatPanel
    {
        protected static bool isOdd = true;
        protected static Color color1 = new(0.8f, 0.8f, 0.8f);
        protected static Color color2 = new(0.6f, 0.6f, 0.6f);
        public static readonly Color warningColor = new(1, 0.5f, 0);
        public static readonly Color headerColor = new(0.2f, 0.8f, 0.2f);
        public static readonly Color subHeaderColor = new(0.5f, 0.5f, 0.8f);

        public readonly GameObject cheatPanel;
        protected readonly GameObject cheatTextObj;
        public readonly List<Action> updateActions = new();

        public CheatPanel(string description)
        {
            cheatPanel = DefaultControls.CreatePanel(new DefaultControls.Resources());
            cheatPanel.name = "Cheat Panel";

            Color thisColor = isOdd ? color1 : color2;
            new ImageMod(cheatPanel.GetComponent<Image>()).SetColor(thisColor);
            isOdd = !isOdd;

            HorizontalLayoutGroup cheatLayoutGroup = cheatPanel.AddComponent<HorizontalLayoutGroup>();
            cheatLayoutGroup.padding = new RectOffset(20, 20, 20, 20);
            cheatLayoutGroup.spacing = 50;
            cheatLayoutGroup.childForceExpandHeight = false;
            cheatLayoutGroup.childForceExpandWidth = false;
            cheatLayoutGroup.childControlHeight = true;
            cheatLayoutGroup.childControlWidth = true;

            ContentSizeFitter panelContentSizeFitter = cheatPanel.AddComponent<ContentSizeFitter>();
            panelContentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            // Setup Text
            (cheatTextObj, _, TextMod textMod) = TextFactory.Build(description);
            textMod.SetAlignment(TextAnchor.MiddleLeft).SetFontStyle(FontStyle.Bold);
            new Fitter(cheatTextObj).Attach(cheatPanel);

            LayoutElement textLayout = cheatTextObj.AddComponent<LayoutElement>();
            textLayout.flexibleWidth = 1;
            textLayout.flexibleHeight = 1;

            ContentSizeFitter textContentSizeFitter = cheatTextObj.AddComponent<ContentSizeFitter>();
            textContentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        }

        public CheatPanel SetColor(Color color)
        {
            new ImageMod(cheatPanel.GetComponent<Image>()).SetColor(color);
            return this;
        }

        public CheatPanel SetSize(Vector2 size)
        {
            new Fitter(cheatPanel).Size(size);
            return this;
        }

        public static void ResetPattern()
        {
            isOdd = true;
        }

        public GameObject GetGameObject()
        {
            return cheatPanel;
        }

        public void Update()
        {
            foreach (Action action in updateActions)
            {
                action();
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(cheatPanel.GetComponent<RectTransform>());
        }
    }
}
