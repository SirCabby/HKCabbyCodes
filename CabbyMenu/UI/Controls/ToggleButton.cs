using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.Factories;
using CabbyMenu.UI.Modders;
using UnityEngine;
using UnityEngine.UI;

namespace CabbyMenu.UI.Controls
{
    public class ToggleButton
    {
        private static readonly Color onColor = Constants.ON_COLOR;
        private static readonly Color onHoverColor = Constants.ON_HOVER_COLOR;
        private static readonly Color onPressedColor = Constants.ON_PRESSED_COLOR;
        private static readonly Color offColor = Constants.OFF_COLOR;
        private static readonly Color offHoverColor = Constants.OFF_HOVER_COLOR;
        private static readonly Color offPressedColor = Constants.OFF_PRESSED_COLOR;

        private readonly GameObject toggleButton;
        private readonly GameObjectMod toggleButtonGoMod;
        private readonly TextMod textMod;
        private readonly ImageMod imageMod;
        private readonly Button buttonComponent;
        public ISyncedReference<bool> IsOn { get; private set; }

        public ToggleButton(ISyncedReference<bool> IsOn)
        {
            this.IsOn = IsOn;

            (toggleButton, toggleButtonGoMod, _) = ButtonFactory.BuildDefault();
            toggleButtonGoMod.SetName("Toggle Button");
            buttonComponent = toggleButton.GetComponent<Button>();
            buttonComponent.onClick.AddListener(Toggle);

            textMod = new TextMod(toggleButton.GetComponentInChildren<Text>());
            imageMod = new ImageMod(toggleButton.GetComponent<Image>());

            Update();
        }

        public GameObject GetGameObject()
        {
            return toggleButton;
        }

        public void Toggle()
        {
            IsOn.Set(!IsOn.Get());
            Update();
        }

        public void SetIsOn(ISyncedReference<bool> isOn)
        {
            IsOn = isOn;
            Update();
        }

        public void Update()
        {
            Color normalColor, hoverColor, pressedColor;

            if (IsOn != null && IsOn.Get())
            {
                textMod.SetText("ON");
                normalColor = onColor;
                hoverColor = onHoverColor;
                pressedColor = onPressedColor;
            }
            else
            {
                textMod.SetText("OFF");
                normalColor = offColor;
                hoverColor = offHoverColor;
                pressedColor = offPressedColor;
            }

            // Update the image color (normal state)
            imageMod.SetColor(normalColor);

            // Update all button states with appropriate colors
            ColorBlock colors = buttonComponent.colors;
            colors.normalColor = normalColor;
            colors.highlightedColor = hoverColor;
            colors.pressedColor = pressedColor;
            buttonComponent.colors = colors;
        }
    }
}