using UnityEngine;
using UnityEngine.UI;

namespace CabbyCodes.UI
{
    public class ToggleButton
    {
        private readonly GameObject toggleButton;
        private readonly Color onColor = new Color(0, 0.8f, 1, 1);
        private readonly Color offColor = Color.white;
        public bool isOn { get; set; }

        public ToggleButton()
        {
            toggleButton = new ButtonFactory("").SetName("Toggle Button").Build();
            toggleButton.GetComponent<Button>().onClick.AddListener(Toggle);
            Update();
        }

        public GameObject GetGameObject()
        {
            return toggleButton;
        }

        public void Toggle()
        {
            isOn = !isOn;
            Update();
        }

        private void Update()
        {
            Text toggleText = toggleButton.GetComponentInChildren<Text>();
            Image toggleImage = toggleButton.GetComponent<Image>();
            if (isOn)
            {
                toggleText.text = "ON";
                toggleImage.color = onColor;
            }
            else
            {
                toggleText.text = "OFF";
                toggleImage.color = offColor;
            }
        }
    }
}
