using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CabbyMenu.UI.Modders;

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
        protected static Color color1 = Constants.PANEL_COLOR_1;

        /// <summary>
        /// Second color in the alternating panel color scheme.
        /// </summary>
        protected static Color color2 = Constants.PANEL_COLOR_2;

        /// <summary>
        /// Color used for warning messages.
        /// </summary>
        public static readonly Color warningColor = Constants.WARNING_COLOR;

        /// <summary>
        /// Color used for header panels.
        /// </summary>
        public static readonly Color headerColor = Constants.HEADER_COLOR;

        /// <summary>
        /// Color used for sub-header panels.
        /// </summary>
        public static readonly Color subHeaderColor = Constants.SUB_HEADER_COLOR;

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
        public readonly List<Action> updateActions = new List<Action>();

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
            cheatLayoutGroup.padding = new RectOffset(Constants.CHEAT_PANEL_PADDING, Constants.CHEAT_PANEL_PADDING, Constants.CHEAT_PANEL_PADDING, Constants.CHEAT_PANEL_PADDING);
            cheatLayoutGroup.spacing = Constants.CHEAT_PANEL_SPACING;
            cheatLayoutGroup.childForceExpandHeight = false;
            cheatLayoutGroup.childForceExpandWidth = false;
            cheatLayoutGroup.childControlHeight = true;
            cheatLayoutGroup.childControlWidth = true;

            ContentSizeFitter panelContentSizeFitter = cheatPanel.AddComponent<ContentSizeFitter>();
            panelContentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            // Setup Text
            var textFactoryResult = TextMod.Build(description);
            cheatTextObj = textFactoryResult.Item1;
            TextMod textMod = textFactoryResult.Item3;
            textMod.SetAlignment(TextAnchor.MiddleLeft).SetFontStyle(FontStyle.Bold);
            new Fitter(cheatTextObj).Attach(cheatPanel);

            LayoutElement textLayout = cheatTextObj.AddComponent<LayoutElement>();
            textLayout.flexibleWidth = Constants.FLEXIBLE_LAYOUT_VALUE;
            textLayout.flexibleHeight = Constants.FLEXIBLE_LAYOUT_VALUE;

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