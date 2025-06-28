using CabbyMenu.UI.Factories;
using CabbyMenu.UI.Modders;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;
using System;
using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.ReferenceControls;

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
                (text) => ButtonFactory.BuildDefault(buttonText), action);
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

            ToggleButton toggleButton = new(syncedReference);
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

        public static GameObject AddDestroyPanelButton(CheatPanel panel, int siblingIndex, Action additionalAction, string buttonText, Vector2 size)
        {
            return CreateButtonPanel(panel, siblingIndex, "Destroy Button Panel", size,
                (text) => ButtonFactory.BuildDanger(buttonText), delegate
                {
                    UnityEngine.Object.Destroy(panel.cheatPanel);
                    additionalAction();
                });
        }
    }
}