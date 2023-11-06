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
        private readonly Action action;
        public BoxedReference IsOn { get; private set; }

        public ToggleButton(BoxedReference IsOn, BasePatch patch) : this(IsOn, (Action)null)
        {
            this.IsOn = IsOn;
            action = () =>
            {
                if ((bool)this.IsOn.Value)
                {
                    patch.Patch();
                }
                else
                {
                    patch.UnPatch();
                }
            };
        }

        public ToggleButton(BoxedReference IsOn, Action action)
        {
            this.IsOn = IsOn;
            this.action = action;

            (toggleButton, GameObjectMod toggleButtonGoMod, _) = ButtonFactory.Build();
            toggleButtonGoMod.SetName("Toggle Button");
            toggleButton.GetComponent<Button>().onClick.AddListener(Toggle);

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
            IsOn.Value = !(bool)IsOn.Value;
            action();
            Update();
        }

        private void Update()
        {
            if ((bool)IsOn.Value)
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
