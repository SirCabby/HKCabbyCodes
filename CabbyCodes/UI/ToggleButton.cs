using CabbyCodes.Patches;
using CabbyCodes.UI.Factories;
using CabbyCodes.UI.Modders;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace CabbyCodes.UI
{
    public class ToggleButton
    {
        private readonly GameObject toggleButton;
        private readonly TextMod textMod;
        private readonly ImageMod imageMod;
        private readonly Color onColor = new(0, 0.8f, 1, 1);
        private readonly Color offColor = Color.white;
        private readonly Action<object> action;
        public object IsOn { get; private set; }

        public ToggleButton(BasePatch patch) : this((Action<object>)null)
        {
            action = (isOn) =>
            {
                if ((bool)isOn)
                {
                    patch.Patch();
                }
                else
                {
                    patch.UnPatch();
                }
            };
        }

        public ToggleButton(Action<object> action)
        {
            this.action = action;

            (toggleButton, GameObjectMod toggleButtonGoMod, _) = ButtonFactory.Build();
            toggleButtonGoMod.SetName("Toggle Button");
            toggleButton.GetComponent<Button>().onClick.AddListener(Toggle);

            textMod = new TextMod(toggleButton.GetComponentInChildren<Text>());
            imageMod = new ImageMod(toggleButton.GetComponent<Image>());

            IsOn = false;

            Update();
        }

        public GameObject GetGameObject()
        {
            return toggleButton;
        }

        public void Toggle()
        {
            IsOn = !(bool)IsOn;
            action(IsOn);
            Update();
        }

        private void Update()
        {
            if ((bool)IsOn)
            {
                textMod.SetText("ON");
                imageMod.SetColor(onColor);
            }
            else
            {
                textMod.SetText("OFF");
                imageMod.SetColor(offColor);
            }
        }
    }
}
