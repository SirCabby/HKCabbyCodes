using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.Controls;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace CabbyMenu.UI.CheatPanels
{
    /// <summary>
    /// A panel that combines a toggle with a teleport button aligned to the right.
    /// </summary>
    public class ToggleWithTeleportPanel : CheatPanel
    {
        private readonly ToggleButton toggleButton;
        private readonly GameObject teleportButton;

        /// <summary>
        /// Initializes a new instance of the ToggleWithTeleportPanel class.
        /// </summary>
        /// <param name="syncedReference">The synced reference for the toggle.</param>
        /// <param name="teleportAction">The action to perform when the teleport button is clicked.</param>
        /// <param name="description">The description text for the panel.</param>
        public ToggleWithTeleportPanel(ISyncedReference<bool> syncedReference, Action teleportAction, string description) : base(description)
        {
            // Add toggle button to the left side using PanelAdder
            GameObject togglePanel = PanelAdder.AddToggleButton(this, 0, syncedReference);
            toggleButton = togglePanel.GetComponentInChildren<ToggleButton>();

            // Create teleport button positioned at the right edge (similar to destroy button)
            (teleportButton, _, _) = ButtonBuilder.BuildDefault("Teleport");
            teleportButton.GetComponent<Button>().onClick.AddListener(() => teleportAction());

            // Attach the teleport button to the panel
            teleportButton.transform.SetParent(cheatPanel.transform, false);
            
            // Configure the RectTransform for positioning
            RectTransform buttonRect = teleportButton.GetComponent<RectTransform>();
            
            // Set the size
            buttonRect.sizeDelta = new Vector2(160f, Constants.DEFAULT_PANEL_HEIGHT);
            
            // Add LayoutElement to prevent the HorizontalLayoutGroup from controlling this button
            LayoutElement buttonLayout = teleportButton.AddComponent<LayoutElement>();
            buttonLayout.ignoreLayout = true;
            
            // Use right-center anchors to position the button at the right edge of the panel
            buttonRect.anchorMin = new Vector2(0.96f, 0.5f);
            buttonRect.anchorMax = new Vector2(0.96f, 0.5f);
            
            // Position the button at the right edge with a small offset to center it
            float buttonOffset = 80f / 2f;
            buttonRect.anchoredPosition = new Vector2(-buttonOffset, 0f);
        }

        /// <summary>
        /// Gets the toggle button component.
        /// </summary>
        /// <returns>The toggle button.</returns>
        public ToggleButton GetToggleButton()
        {
            return toggleButton;
        }

        /// <summary>
        /// Gets the toggle component.
        /// </summary>
        /// <returns>The toggle component.</returns>
        public Toggle GetToggle()
        {
            return toggleButton.GetGameObject().GetComponent<Toggle>();
        }

        /// <summary>
        /// Gets the teleport button GameObject.
        /// </summary>
        /// <returns>The teleport button GameObject.</returns>
        public GameObject GetTeleportButton()
        {
            return teleportButton;
        }
    }
} 