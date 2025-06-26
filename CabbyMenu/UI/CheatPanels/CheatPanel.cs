using UnityEngine.UI;
using UnityEngine;
using CabbyMenu.UI.Factories;
using CabbyMenu.UI.Modders;
using System.Collections.Generic;
using System;

namespace CabbyMenu.UI.CheatPanels
{
    /// <summary>
    /// Base class for all cheat panels in the mod menu. Provides common functionality and styling.
    /// </summary>
    public class CheatPanel
    {
        /// <summary>
        /// Tracks whether the current panel should use the odd or even color pattern.
        /// </summary>
        protected static bool isOdd = true;

        /// <summary>
        /// First color in the alternating panel color scheme.
        /// </summary>
        protected static Color color1 = new(0.8f, 0.8f, 0.8f);

        /// <summary>
        /// Second color in the alternating panel color scheme.
        /// </summary>
        protected static Color color2 = new(0.6f, 0.6f, 0.6f);

        /// <summary>
        /// Color used for warning messages.
        /// </summary>
        public static readonly Color warningColor = new(1, 0.5f, 0);

        /// <summary>
        /// Color used for header panels.
        /// </summary>
        public static readonly Color headerColor = new(0.2f, 0.8f, 0.2f);

        /// <summary>
        /// Color used for sub-header panels.
        /// </summary>
        public static readonly Color subHeaderColor = new(0.5f, 0.5f, 0.8f);

        /// <summary>
        /// The main GameObject for this cheat panel.
        /// </summary>
        public readonly GameObject cheatPanel;

        /// <summary>
        /// The GameObject containing the description text.
        /// </summary>
        protected readonly GameObject cheatTextObj;

        /// <summary>
        /// List of actions to execute during panel updates.
        /// </summary>
        public readonly List<Action> updateActions = new();

        /// <summary>
        /// Initializes a new instance of the CheatPanel class.
        /// </summary>
        /// <param name="description">The description text to display on the panel.</param>
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

        /// <summary>
        /// Sets the background color of the panel.
        /// </summary>
        /// <param name="color">The color to set for the panel background.</param>
        /// <returns>This CheatPanel instance for method chaining.</returns>
        public CheatPanel SetColor(Color color)
        {
            new ImageMod(cheatPanel.GetComponent<Image>()).SetColor(color);
            return this;
        }

        /// <summary>
        /// Sets the size of the panel.
        /// </summary>
        /// <param name="size">The size to set for the panel.</param>
        /// <returns>This CheatPanel instance for method chaining.</returns>
        public CheatPanel SetSize(Vector2 size)
        {
            new Fitter(cheatPanel).Size(size);
            return this;
        }

        /// <summary>
        /// Resets the alternating color pattern for panels.
        /// </summary>
        public static void ResetPattern()
        {
            isOdd = true;
        }

        /// <summary>
        /// Gets the main GameObject for this panel.
        /// </summary>
        /// <returns>The GameObject representing this cheat panel.</returns>
        public GameObject GetGameObject()
        {
            return cheatPanel;
        }

        /// <summary>
        /// Updates the panel by executing all registered update actions and rebuilding the layout.
        /// </summary>
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