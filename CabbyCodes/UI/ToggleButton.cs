using CabbyCodes.SyncedReferences;
using CabbyCodes.UI.Factories;
using CabbyCodes.UI.Modders;
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
        public SyncedReference<bool> IsOn { get; private set; }

        public ToggleButton(SyncedReference<bool> IsOn)
        {
            this.IsOn = IsOn;

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
            IsOn.Set(!IsOn.Get());
            Update();
        }

        public void SetIsOn(SyncedReference<bool> isOn)
        {
            IsOn = isOn;
            Update();
        }

        private void Update()
        {
            if (IsOn != null && IsOn.Get())
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
