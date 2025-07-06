using CabbyMenu.UI.Modders;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;
using System;
using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.Controls;

namespace CabbyMenu.UI.CheatPanels
{
    public class PanelAdder
    {
        private static readonly Vector2 defaultIconSize = Constants.DEFAULT_ICON_SIZE_VECTOR;
        private static readonly Vector2 defaultToggleButtonSize = Constants.DEFAULT_PANEL_SIZE;
        private static readonly Vector2 middle = Constants.MIDDLE_ANCHOR_VECTOR;

        /// <summary>
        /// Creates a button panel with the specified configuration.
        /// </summary>
        private static GameObject CreateButtonPanel(CheatPanel parentPanel, int siblingIndex, string panelName, Vector2 size,
            Func<string, (GameObject, GameObjectMod, TextMod)> buttonFactory, UnityAction buttonAction)
        {
            GameObject buttonPanel = DefaultControls.CreatePanel(new DefaultControls.Resources());
            buttonPanel.name = panelName;
            new ImageMod(buttonPanel.GetComponent<Image>()).SetColor(Color.clear);
            new Fitter(buttonPanel).Attach(parentPanel.cheatPanel);
            buttonPanel.transform.SetSiblingIndex(siblingIndex);

            var buttonPanelLayout = buttonPanel.AddComponent<LayoutElement>();
            buttonPanelLayout.flexibleHeight = Constants.FLEXIBLE_LAYOUT_VALUE;
            buttonPanelLayout.minWidth = size.x;

            (GameObject button, _, _) = buttonFactory("");
            button.GetComponent<Button>().onClick.AddListener(buttonAction);
            new Fitter(button).Attach(buttonPanel).Anchor(middle, middle).Size(size);

            return buttonPanel;
        }

        public static GameObject AddButton(CheatPanel panel, int siblingIndex, UnityAction action, string buttonText, Vector2 size)
        {
            return CreateButtonPanel(panel, siblingIndex, "Button Panel", size,
                (text) => ButtonBuilder.BuildDefault(buttonText), action);
        }

        public static GameObject AddToggleButton(CheatPanel panel, int siblingIndex, ISyncedReference<bool> syncedReference)
        {
            GameObject buttonPanel = DefaultControls.CreatePanel(new DefaultControls.Resources());
            buttonPanel.name = "Toggle Button Panel";
            new ImageMod(buttonPanel.GetComponent<Image>()).SetColor(Color.clear);
            new Fitter(buttonPanel).Attach(panel.cheatPanel);
            buttonPanel.transform.SetSiblingIndex(siblingIndex);

            LayoutElement buttonPanelLayout = buttonPanel.AddComponent<LayoutElement>();
            buttonPanelLayout.flexibleHeight = Constants.FLEXIBLE_LAYOUT_VALUE;
            buttonPanelLayout.minWidth = defaultToggleButtonSize.x;

            ToggleButton toggleButton = new ToggleButton(syncedReference);
            new Fitter(toggleButton.GetGameObject()).Attach(buttonPanel).Anchor(middle, middle).Size(defaultToggleButtonSize);
            panel.updateActions.Add(toggleButton.Update);

            return buttonPanel;
        }

        public static (GameObject, ImageMod) AddSprite(CheatPanel panel, Sprite sprite, int siblingIndex)
        {
            GameObject imagePanel = DefaultControls.CreatePanel(new DefaultControls.Resources());
            imagePanel.name = "Sprite panel";
            new ImageMod(imagePanel.GetComponent<Image>()).SetColor(Color.clear);
            new Fitter(imagePanel).Attach(panel.cheatPanel);
            imagePanel.transform.SetSiblingIndex(siblingIndex);

            LayoutElement imagePanelLayout = imagePanel.AddComponent<LayoutElement>();
            imagePanelLayout.flexibleHeight = Constants.FLEXIBLE_LAYOUT_VALUE;
            imagePanelLayout.minWidth = defaultIconSize.x;

            GameObject icon = DefaultControls.CreateImage(new DefaultControls.Resources());
            ImageMod spriteImageMod = new ImageMod(icon.GetComponent<Image>()).SetSprite(sprite).SetColor(Color.white);
            new Fitter(icon).Attach(imagePanel).Anchor(middle, middle).Size(defaultIconSize);

            return (imagePanel, spriteImageMod);
        }

        /// <summary>
        /// Adds a destroy button directly to an existing panel (like ButtonPanel) positioned at the right edge.
        /// </summary>
        /// <param name="targetPanel">The panel to add the destroy button to.</param>
        /// <param name="additionalAction">Additional action to perform when the button is clicked.</param>
        /// <param name="buttonText">Text to display on the button (default: "X").</param>
        /// <param name="buttonSize">Size of the button (default: 60x60).</param>
        /// <returns>The created destroy button GameObject.</returns>
        public static GameObject AddDestroyButtonToPanel(CheatPanel targetPanel, Action additionalAction, string buttonText = "X", float buttonSize = 60f)
        {
            // Create the destroy button using the same factory as other buttons
            (GameObject button, _, _) = ButtonBuilder.BuildDanger(buttonText);
            
            button.GetComponent<Button>().onClick.AddListener(delegate
            {
                UnityEngine.Object.Destroy(targetPanel.cheatPanel);
                additionalAction();
            });

            // Check if the target panel exists
            if (targetPanel.cheatPanel == null)
            {
                return button;
            }
            
            // Attach the button to the target panel
            button.transform.SetParent(targetPanel.cheatPanel.transform, false);
            
            // Configure the RectTransform for positioning
            RectTransform buttonRect = button.GetComponent<RectTransform>();
            
            // Set the size
            buttonRect.sizeDelta = new Vector2(buttonSize, buttonSize);
            
            // Add LayoutElement to prevent the HorizontalLayoutGroup from controlling this button
            LayoutElement buttonLayout = button.AddComponent<LayoutElement>();
            buttonLayout.ignoreLayout = true;
            
            // Use right-center anchors to position the button at the right edge of the panel
            buttonRect.anchorMin = new Vector2(0.99f, 0.5f);
            buttonRect.anchorMax = new Vector2(0.99f, 0.5f);
            
            // Position the button at the right edge with a small offset to center it
            float buttonOffset = buttonSize / 2f;
            buttonRect.anchoredPosition = new Vector2(-buttonOffset, 0f);

            return button;
        }
    }
}