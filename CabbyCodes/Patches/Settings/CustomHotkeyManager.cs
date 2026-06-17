using System;
using System.Collections.Generic;
using BepInEx.Configuration;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.UI.Popups;
using CabbyMenu.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace CabbyCodes.Patches.Settings
{
    /// <summary>
    /// Central registry for per-row quick-action hotkeys (quick-load a save, quick-run a teleport).
    /// Each action is identified by a stable id, persisted to the "Hotkeys" config section, and
    /// polled every frame. Bindings support Ctrl / Shift / Alt modifiers and only fire during
    /// gameplay (never while the Cabby menu is open).
    /// </summary>
    public static class CustomHotkeyManager
    {
        private const string ConfigSection = "Hotkeys";
        private const float ActivationCooldownSeconds = 0.35f;
        // Short delay before HK's pause input is restored after a capture ends, so the ESC that ended
        // the capture has cleared (WasPressed only lasts its down-frame) before pause input goes live.
        private const float InputRestoreDelaySeconds = 0.1f;
        private const string PopupHeader = "Set Hotkey";
        private const string PopupMessage = "Press the key to bind. Hold Ctrl, Shift, and/or Alt for modifiers.\nPress ESC to clear the binding.";
        private const string UnboundDisplay = "Set Hotkey";

        // Content-fit sizing for the row hotkey button (estimated, matching ButtonPanel's approach).
        private const float ButtonCharWidthFactor = 0.5f;
        private const float ButtonHorizontalPadding = 30f;
        private const float MinButtonWidth = 150f;

        private static readonly KeyCode[] keyCodes = (KeyCode[])Enum.GetValues(typeof(KeyCode));

        /// <summary>
        /// A registered hotkey action.
        /// </summary>
        private class Entry
        {
            public string actionId;
            public ConfigDefinition def;
            public ConfigEntry<string> entry;
            public HotkeyBinding binding;
            public Action callback;
        }

        private static ConfigFile configFile;
        private static readonly Dictionary<string, Entry> entries = new Dictionary<string, Entry>();

        private static bool initialized;
        private static string listeningActionId;
        private static bool executeInProgress;
        private static float activationAllowedAt;
        private static PopupBase bindingPopup;
        private static bool restoreInputPending;
        private static float restoreInputAt;

        /// <summary>
        /// Raised with the action id whenever a binding is set or cleared, so UI can refresh immediately.
        /// </summary>
        public static event Action<string> BindingChanged;

        /// <summary>
        /// Builds the action id for a custom save hotkey.
        /// </summary>
        public static string SaveActionId(string saveFileName)
        {
            return "Save:" + saveFileName;
        }

        /// <summary>
        /// Builds the action id for a custom teleport hotkey.
        /// </summary>
        public static string TeleportActionId(string displayName)
        {
            return "Teleport:" + displayName;
        }

        /// <summary>
        /// Stores the configuration reference. Must be called once at plugin startup.
        /// </summary>
        public static void Initialize(ConfigFile config)
        {
            if (initialized || config == null)
            {
                return;
            }

            configFile = config;
            activationAllowedAt = Time.unscaledTime;
            initialized = true;
        }

        /// <summary>
        /// Registers (or refreshes) a hotkey action and returns its current binding.
        /// Re-registering an existing action id keeps the persisted binding and refreshes the callback.
        /// </summary>
        /// <param name="actionId">Stable, unique identifier for the action.</param>
        /// <param name="callback">Action invoked when the bound hotkey fires.</param>
        /// <param name="description">Human-friendly description used in the config file comment.</param>
        public static HotkeyBinding Register(string actionId, Action callback, string description)
        {
            if (!initialized || string.IsNullOrEmpty(actionId))
            {
                return new HotkeyBinding();
            }

            if (entries.TryGetValue(actionId, out Entry existing))
            {
                existing.callback = callback;
                existing.binding = HotkeyBinding.Parse(existing.entry.Value);
                return existing.binding;
            }

            string comment = string.IsNullOrEmpty(description)
                ? string.Format("Hotkey binding for {0}", actionId)
                : string.Format("Hotkey binding for {0}", description);

            ConfigEntry<string> configEntry = configFile.Bind(ConfigSection, actionId, HotkeyBinding.NoneValue, new ConfigDescription(comment));

            Entry entry = new Entry
            {
                actionId = actionId,
                def = new ConfigDefinition(ConfigSection, actionId),
                entry = configEntry,
                binding = HotkeyBinding.Parse(configEntry.Value),
                callback = callback
            };

            entries[actionId] = entry;
            return entry.binding;
        }

        /// <summary>
        /// Removes a registered hotkey action, optionally deleting its config entry.
        /// </summary>
        public static void Unregister(string actionId, bool removeFromConfig)
        {
            if (string.IsNullOrEmpty(actionId))
            {
                return;
            }

            entries.TryGetValue(actionId, out Entry entry);
            entries.Remove(actionId);

            if (listeningActionId == actionId)
            {
                CancelCapture();
            }

            if (removeFromConfig && configFile != null)
            {
                try
                {
                    ConfigDefinition def = entry != null ? entry.def : new ConfigDefinition(ConfigSection, actionId);
                    configFile.Remove(def);
                    configFile.Save();
                }
                catch (Exception ex)
                {
                    CabbyCodesPlugin.BLogger.LogWarning(string.Format("Failed to remove hotkey binding for '{0}': {1}", actionId, ex.Message));
                }
            }
        }

        /// <summary>
        /// Gets the current binding for an action, or an unbound binding if not registered.
        /// </summary>
        public static HotkeyBinding GetBinding(string actionId)
        {
            return entries.TryGetValue(actionId, out Entry entry) ? entry.binding : new HotkeyBinding();
        }

        /// <summary>
        /// Gets the label to display on the hotkey button for an action.
        /// </summary>
        public static string GetDisplay(string actionId)
        {
            HotkeyBinding binding = GetBinding(actionId);
            return binding.IsBound ? binding.ToDisplay() : UnboundDisplay;
        }

        /// <summary>
        /// Gets whether the manager is currently capturing a binding.
        /// </summary>
        public static bool IsListening()
        {
            return listeningActionId != null;
        }

        /// <summary>
        /// Begins capturing a new binding for the given action.
        /// </summary>
        public static void BeginCapture(string actionId)
        {
            if (!initialized || !entries.ContainsKey(actionId))
            {
                return;
            }

            listeningActionId = actionId;
            // Delay activation slightly so the click/key used to start capture does not immediately trigger.
            activationAllowedAt = Time.unscaledTime + ActivationCooldownSeconds;
            // A new capture is starting; cancel any pending restore from a previous capture.
            restoreInputPending = false;
            ShowBindingPopup();
            // Suppress the pause-menu input while capturing so ESC clears the binding and stays on
            // the page instead of resuming the game (which would close the whole Cabby menu).
            SetCaptureInputSuppressed(true);
        }

        /// <summary>
        /// Cancels binding capture without changing the existing binding.
        /// </summary>
        public static void CancelCapture()
        {
            StopListening();
        }

        /// <summary>
        /// Ends capture: closes the popup and restores normal pause handling.
        /// </summary>
        private static void StopListening()
        {
            listeningActionId = null;
            CloseBindingPopup();
            // Defer restoring HK's pause input by a moment instead of restoring it now. The ESC that
            // ends a capture is still WasPressed this frame, and HK's InputHandler.Update() runs after
            // ours - so re-enabling acceptingInput/pauseAllowed immediately lets HK read that same ESC
            // as a pause-resume, unpausing the game and closing the whole Cabby menu. Holding the
            // suppression until the keypress clears (see Update) keeps the menu open.
            restoreInputPending = true;
            restoreInputAt = Time.unscaledTime + InputRestoreDelaySeconds;
        }

        /// <summary>
        /// Adds a hotkey button to a save/teleport row and registers its action. The button is
        /// anchored to the right of the row, just left of the destroy (X) button. Its label tracks
        /// the current binding (updated immediately when the binding changes) and clicking it begins capture.
        /// </summary>
        /// <param name="panel">The row panel to add the button to.</param>
        /// <param name="actionId">Stable, unique identifier for the action.</param>
        /// <param name="onTrigger">Action invoked when the bound hotkey fires.</param>
        /// <param name="description">Human-friendly description used in the config file comment.</param>
        public static void AttachRowButton(CheatPanel panel, string actionId, Action onTrigger, string description)
        {
            Register(actionId, onTrigger, description);

            // Sit just left of the 60px-wide destroy button (with a small gap).
            const float destroyButtonClearance = 70f;
            string initialDisplay = GetDisplay(actionId);
            float width = MeasureButtonWidth(initialDisplay);
            GameObject button = PanelAdder.AddButtonAtRightEdge(
                panel,
                () => BeginCapture(actionId),
                initialDisplay,
                new Vector2(width, CabbyMenu.Constants.DEFAULT_PANEL_HEIGHT),
                destroyButtonClearance);

            RectTransform buttonRect = button.GetComponent<RectTransform>();
            Text label = button.GetComponentInChildren<Text>();
            if (label != null)
            {
                Action<string> handler = null;
                handler = changedActionId =>
                {
                    // Unsubscribe once the row has been destroyed (Unity-overloaded null check).
                    if (label == null)
                    {
                        BindingChanged -= handler;
                        return;
                    }

                    if (changedActionId == actionId)
                    {
                        string display = GetDisplay(actionId);
                        label.text = display;
                        // Re-fit the button to the new binding text, keeping its right edge anchored.
                        ResizeRightEdgeButton(buttonRect, MeasureButtonWidth(display), destroyButtonClearance);
                    }
                };
                BindingChanged += handler;
            }
        }

        /// <summary>
        /// Estimates a button width that fits the given label at the default font size, mirroring
        /// the content-fit sizing used elsewhere for menu buttons.
        /// </summary>
        private static float MeasureButtonWidth(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return MinButtonWidth;
            }

            float charWidth = CabbyMenu.Constants.DEFAULT_FONT_SIZE * ButtonCharWidthFactor;
            float width = (text.Length * charWidth) + ButtonHorizontalPadding;
            return Mathf.Max(MinButtonWidth, width);
        }

        /// <summary>
        /// Resizes a right-edge-anchored button to the given width while keeping its right edge fixed
        /// (just left of the destroy button), matching how <see cref="PanelAdder.AddButtonAtRightEdge"/>
        /// positions it.
        /// </summary>
        private static void ResizeRightEdgeButton(RectTransform buttonRect, float width, float rightOffset)
        {
            if (buttonRect == null)
            {
                return;
            }

            buttonRect.sizeDelta = new Vector2(width, buttonRect.sizeDelta.y);
            buttonRect.anchoredPosition = new Vector2(-(rightOffset + (width / 2f)), 0f);
        }

        /// <summary>
        /// Updates the manager. Must be called every frame from the plugin.
        /// </summary>
        public static void Update()
        {
            if (!initialized)
            {
                return;
            }

            // Restore HK's pause input a frame or two after a capture ended (see StopListening),
            // once the ESC that ended it has cleared so it cannot be re-read as a pause-resume.
            if (restoreInputPending && listeningActionId == null && Time.unscaledTime >= restoreInputAt)
            {
                restoreInputPending = false;
                SetCaptureInputSuppressed(false);
            }

            if (listeningActionId != null)
            {
                // If the capture popup was dismissed externally (e.g. clicking outside it), stop
                // listening and restore pause handling.
                if (!IsBindingPopupOpen())
                {
                    StopListening();
                    return;
                }

                ListenForBindingInput();
                return;
            }

            if (executeInProgress || Time.unscaledTime < activationAllowedAt)
            {
                return;
            }

            // Gameplay-only: never fire while the Cabby menu is open or before the game is running.
            if (CabbyCodesPlugin.cabbyMenu != null && CabbyCodesPlugin.cabbyMenu.IsMenuOpen())
            {
                return;
            }

            if (GameManager.instance == null && GameManager._instance == null)
            {
                return;
            }

            // Snapshot so a callback that rebuilds panels (and mutates the registry) is safe.
            List<Entry> snapshot = new List<Entry>(entries.Values);
            foreach (Entry entry in snapshot)
            {
                if (entry.binding.IsTriggered())
                {
                    Trigger(entry);
                    break;
                }
            }
        }

        private static void Trigger(Entry entry)
        {
            if (executeInProgress)
            {
                return;
            }

            executeInProgress = true;
            activationAllowedAt = Time.unscaledTime + ActivationCooldownSeconds;

            try
            {
                entry.callback?.Invoke();
            }
            catch (Exception ex)
            {
                CabbyCodesPlugin.BLogger.LogWarning(string.Format("Hotkey action '{0}' failed: {1}", entry.actionId, ex.Message));
            }
            finally
            {
                executeInProgress = false;
            }
        }

        private static void ListenForBindingInput()
        {
            if (!entries.TryGetValue(listeningActionId, out Entry entry))
            {
                CancelCapture();
                return;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SetBinding(entry, new HotkeyBinding());
                StopListening();
                return;
            }

            foreach (KeyCode keyCode in keyCodes)
            {
                if (!Input.GetKeyDown(keyCode))
                {
                    continue;
                }

                // Ignore modifier keys themselves - wait for the main key while they are held.
                if (HotkeyBinding.IsModifierKey(keyCode))
                {
                    continue;
                }

                // Ignore disallowed keys (e.g. mouse buttons).
                if (!HotkeyBinding.IsAllowedMainKey(keyCode))
                {
                    continue;
                }

                SetBinding(entry, HotkeyBinding.CaptureCurrentModifiers(keyCode));
                StopListening();
                break;
            }
        }

        /// <summary>
        /// Suppresses or restores Hollow Knight's pause-menu input while capturing a binding.
        /// Pressing ESC while the Cabby menu is open can resume the game two ways: the in-gameplay
        /// pause toggle (InputHandler, gated by pauseAllowed) and the pause-menu cancel action
        /// (UIManager.TogglePauseGame, gated by the UI input/navigation state). PreventPause only
        /// blocks the former, so StopUIInput is also used to block the latter - the same call HK
        /// itself uses to gate the cancel action during pause-menu transitions.
        /// </summary>
        private static void SetCaptureInputSuppressed(bool suppressed)
        {
            try
            {
                GameManager gm = GameManager.instance ?? GameManager._instance;
                if (gm == null || gm.inputHandler == null)
                {
                    return;
                }

                if (suppressed)
                {
                    gm.inputHandler.PreventPause();
                    gm.inputHandler.StopUIInput();
                }
                else
                {
                    gm.inputHandler.StartUIInput();
                    gm.inputHandler.AllowPause();
                }
            }
            catch (Exception ex)
            {
                CabbyCodesPlugin.BLogger.LogWarning(string.Format("Failed to set capture input state: {0}", ex.Message));
            }
        }

        private static void SetBinding(Entry entry, HotkeyBinding binding)
        {
            entry.binding = binding;
            entry.entry.Value = binding.Serialize();
            activationAllowedAt = Time.unscaledTime + ActivationCooldownSeconds;

            try
            {
                configFile.Save();
            }
            catch (Exception ex)
            {
                CabbyCodesPlugin.BLogger.LogWarning(string.Format("Failed to save hotkey binding for '{0}': {1}", entry.actionId, ex.Message));
            }

            BindingChanged?.Invoke(entry.actionId);
        }

        /// <summary>
        /// Returns whether the capture popup is still in the set of open popups.
        /// </summary>
        private static bool IsBindingPopupOpen()
        {
            if (bindingPopup == null)
            {
                return false;
            }

            System.Collections.Generic.IReadOnlyList<PopupBase> open = PopupBase.OpenPopups;
            for (int i = 0; i < open.Count; i++)
            {
                if (ReferenceEquals(open[i], bindingPopup))
                {
                    return true;
                }
            }

            return false;
        }

        private static void ShowBindingPopup()
        {
            if (bindingPopup != null)
            {
                bindingPopup.Show();
                return;
            }

            float popupWidth = 680f;
            float popupHeight = 260f;

            if (CabbyCodesPlugin.cabbyMenu != null)
            {
                bindingPopup = new PopupBase(CabbyCodesPlugin.cabbyMenu, PopupHeader, PopupMessage, popupWidth, popupHeight);
            }
            else
            {
                bindingPopup = new PopupBase(PopupHeader, PopupMessage, popupWidth, popupHeight);
            }

            bindingPopup.Show();
        }

        private static void CloseBindingPopup()
        {
            if (bindingPopup != null)
            {
                bindingPopup.Hide();
                bindingPopup.Destroy();
                bindingPopup = null;
            }
        }
    }
}
