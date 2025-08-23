using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.Modders;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
        private bool isInteractable = true;
        
        // Hover popup for disabled state messages
        private HoverPopup hoverPopup;
        private string disabledMessage = "";

        public ToggleButton(ISyncedReference<bool> IsOn)
        {
            this.IsOn = IsOn;

            (toggleButton, toggleButtonGoMod, _) = ButtonBuilder.BuildDefault();
            toggleButtonGoMod.SetName("Toggle Button");
            buttonComponent = toggleButton.GetComponent<Button>();
            buttonComponent.onClick.AddListener(Toggle);

            textMod = new TextMod(toggleButton.GetComponentInChildren<Text>());
            imageMod = new ImageMod(toggleButton.GetComponent<Image>());

            // Create hover popup for disabled state messages
            CreateHoverPopup();

            // Add EventTrigger for hover events
            AddHoverEventTriggers();

            Update();
        }

        public GameObject GetGameObject()
        {
            return toggleButton;
        }

        public void Toggle()
        {
            if (!isInteractable) return;
            IsOn.Set(!IsOn.Get());
            Update();
        }

        public void SetIsOn(ISyncedReference<bool> isOn)
        {
            IsOn = isOn;
            Update();
        }

        public void SetInteractable(bool interactable)
        {
            isInteractable = interactable;
            buttonComponent.interactable = interactable;
            Update(); // Update colors to reflect disabled state
        }

        /// <summary>
        /// Sets the message to display when hovering over a disabled toggle button.
        /// </summary>
        /// <param name="message">The message to display on hover</param>
        public void SetDisabledMessage(string message)
        {
            disabledMessage = message;
        }

        /// <summary>
        /// Creates the hover popup component for disabled state messages.
        /// </summary>
        private void CreateHoverPopup()
        {
            if (hoverPopup == null)
            {
                hoverPopup = toggleButton.AddComponent<HoverPopup>();
            }
        }

        /// <summary>
        /// Shows the hover popup when hovering over a disabled button.
        /// </summary>
        public void OnPointerEnter()
        {
            if (!isInteractable && !string.IsNullOrEmpty(disabledMessage))
            {
                hoverPopup?.ShowPopup(disabledMessage);
            }
        }

        /// <summary>
        /// Hides the hover popup when leaving a disabled button.
        /// </summary>
        public void OnPointerExit()
        {
            hoverPopup?.HidePopup();
        }

        /// <summary>
        /// Manually shows the hover popup with the current disabled message.
        /// Useful for testing or external triggering.
        /// </summary>
        public void ShowHoverPopup()
        {
            if (!isInteractable && !string.IsNullOrEmpty(disabledMessage))
            {
                hoverPopup?.ShowPopup(disabledMessage);
            }
        }

        /// <summary>
        /// Manually hides the hover popup.
        /// Useful for testing or external triggering.
        /// </summary>
        public void HideHoverPopup()
        {
            hoverPopup?.HidePopup();
        }

        /// <summary>
        /// Adds EventTrigger components to handle mouse enter/exit events for hover popup.
        /// </summary>
        private void AddHoverEventTriggers()
        {
            var eventTrigger = toggleButton.GetComponent<EventTrigger>();
            if (eventTrigger == null)
            {
                eventTrigger = toggleButton.AddComponent<EventTrigger>();
            }

            // Mouse Enter event
            EventTrigger.Entry enterEntry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerEnter
            };
            enterEntry.callback.AddListener((data) => OnPointerEnter());
            eventTrigger.triggers.Add(enterEntry);

            // Mouse Exit event
            EventTrigger.Entry exitEntry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerExit
            };
            exitEntry.callback.AddListener((data) => OnPointerExit());
            eventTrigger.triggers.Add(exitEntry);
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

            // If button is disabled, use disabled colors
            if (!isInteractable)
            {
                normalColor = offColor;
                hoverColor = offColor;
                pressedColor = offColor;
            }

            // Update the image color (normal state)
            imageMod.SetColor(normalColor);

            // Update all button states with appropriate colors
            ColorBlock colors = buttonComponent.colors;
            colors.normalColor = normalColor;
            colors.highlightedColor = hoverColor;
            colors.pressedColor = pressedColor;
            colors.disabledColor = offColor; // Set disabled color to off color
            buttonComponent.colors = colors;
        }
    }
}