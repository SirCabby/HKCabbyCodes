using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.Controls.InputField;
using CabbyMenu.UI.Controls;
using CabbyMenu.Utilities;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace CabbyMenu.UI.CheatPanels
{
    /// <summary>
    /// A panel that combines an int input field with a teleport button aligned to the right.
    /// </summary>
    public class IntWithTeleportPanel : CheatPanel
    {
        private readonly BaseInputFieldSync<int> inputFieldSync;
        private readonly GameObject teleportButton;

        /// <summary>
        /// Initializes a new instance of the IntWithTeleportPanel class.
        /// </summary>
        /// <param name="syncedReference">The synced reference for the int value.</param>
        /// <param name="teleportAction">The action to perform when the teleport button is clicked.</param>
        /// <param name="description">The description text for the panel.</param>
        /// <param name="minValue">The minimum allowed value.</param>
        /// <param name="maxValue">The maximum allowed value.</param>
        public IntWithTeleportPanel(ISyncedReference<int> syncedReference, Action teleportAction, string description, int minValue = 0, int maxValue = 999) : base(description)
        {
            // Create input field on the left side with proper character limit calculation
            int characterLimit = Math.Max(3, maxValue.ToString().Length);
            inputFieldSync = InputFieldSync.Create(syncedReference, KeyCodeMap.ValidChars.Numeric, new Vector2(200, Constants.DEFAULT_PANEL_HEIGHT), characterLimit, minValue, maxValue);
            new Fitter(inputFieldSync.GetGameObject()).Attach(cheatPanel);
            inputFieldSync.GetGameObject().transform.SetAsFirstSibling();
            
            // Add LayoutElement to set the input field width
            LayoutElement inputFieldLayout = inputFieldSync.GetGameObject().AddComponent<LayoutElement>();
            inputFieldLayout.preferredWidth = 200;
            inputFieldLayout.minWidth = 200;

            // Create teleport button positioned at the right edge
            var (gameObject, gameObjectMod, textMod) = ButtonBuilder.BuildDefault("Teleport");
            teleportButton = gameObject;
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
            
            // Add update action for the input field
            updateActions.Add(inputFieldSync.Update);
        }

        /// <summary>
        /// Gets the input field sync component.
        /// </summary>
        /// <returns>The input field sync.</returns>
        public BaseInputFieldSync<int> GetInputFieldSync()
        {
            return inputFieldSync;
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