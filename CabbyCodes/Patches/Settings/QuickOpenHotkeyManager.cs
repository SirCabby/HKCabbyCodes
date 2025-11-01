using System;
using System.Collections;
using BepInEx.Configuration;
using CabbyMenu.UI.Popups;
using CabbyMenu.Utilities;
using UnityEngine;

namespace CabbyCodes.Patches.Settings
{
    /// <summary>
    /// Manages the configurable hotkey used to quick-open the Cabby Codes menu.
    /// Handles binding capture, persistence, and runtime hotkey execution.
    /// </summary>
    public static class QuickOpenHotkeyManager
    {
        private const string ConfigSection = "Settings";
        private const string EnabledKey = "EnableQuickOpenHotkey";
        private const string BindingKey = "QuickOpenHotkeyBinding";
        private const string NoneBindingValue = "None";
        private const float ActivationCooldownSeconds = 0.35f;
        private const string PopupHeader = "Set Quick Open Hotkey";
        private const string PopupMessage = "Press the key, mouse button, or controller button you want to use.\nPress ESC to clear the binding.";

        private static readonly KeyCode[] keyCodes = (KeyCode[])Enum.GetValues(typeof(KeyCode));
        private static readonly System.Collections.Generic.HashSet<KeyCode> disallowedKeys = new System.Collections.Generic.HashSet<KeyCode>
        {
            KeyCode.Mouse0,
            KeyCode.Mouse1
        };

        private static ConfigEntry<bool> hotkeyEnabled;
        private static ConfigEntry<string> hotkeyBinding;

        private static bool initialized;
        private static bool isListening;
        private static bool executeInProgress;
        private static float hotkeyActivationAllowedAt;
        private static PopupBase bindingPopup;

        public static event System.Action BindingChanged;

        /// <summary>
        /// Ensures the configuration entries are created and ready for use.
        /// </summary>
        public static void Initialize(ConfigFile configFile)
        {
            if (initialized || configFile == null)
            {
                return;
            }

            hotkeyEnabled = configFile.Bind(
                ConfigSection,
                EnabledKey,
                false,
                "Enable a configurable hotkey that pauses the game and opens the Cabby Codes menu.");

            hotkeyBinding = configFile.Bind(
                ConfigSection,
                BindingKey,
                NoneBindingValue,
                "Binding used for the quick open hotkey. Stores the KeyCode name or 'None'.");

            hotkeyActivationAllowedAt = Time.unscaledTime;
            initialized = true;
        }

        /// <summary>
        /// Gets the current enabled state.
        /// </summary>
        public static bool GetEnabled()
        {
            return hotkeyEnabled != null && hotkeyEnabled.Value;
        }

        /// <summary>
        /// Sets the enabled state.
        /// </summary>
        public static void SetEnabled(bool value)
        {
            if (hotkeyEnabled != null)
            {
                hotkeyEnabled.Value = value;
            }
        }

        /// <summary>
        /// Begins the binding capture mode.
        /// </summary>
        public static void BeginListeningForBinding()
        {
            if (!initialized)
            {
                return;
            }

            isListening = true;
            // Delay activation slightly so the same key used to start binding does not immediately trigger.
            hotkeyActivationAllowedAt = Time.unscaledTime + ActivationCooldownSeconds;
            ShowBindingPopup();
        }

        /// <summary>
        /// Cancels the binding capture mode without changing the existing binding.
        /// </summary>
        public static void CancelListening()
        {
            isListening = false;
            CloseBindingPopup();
        }

        /// <summary>
        /// Clears the current binding, effectively unbinding the hotkey.
        /// </summary>
        public static void ClearBinding()
        {
            SetBinding(null);
            CloseBindingPopup();
        }

        /// <summary>
        /// Returns a user-friendly display value for the binding button.
        /// </summary>
        public static string GetBindingDisplay()
        {
            if (hotkeyBinding == null || string.IsNullOrEmpty(hotkeyBinding.Value) || hotkeyBinding.Value == NoneBindingValue)
            {
                return "Not Bound";
            }

            return FormatKeyCodeDisplay(hotkeyBinding.Value);
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

            if (isListening)
            {
                ListenForBindingInput();
                return;
            }

            if (!GetEnabled() || executeInProgress || Time.unscaledTime < hotkeyActivationAllowedAt)
            {
                return;
            }

            if (!TryGetBinding(out KeyCode bindingKey))
            {
                return;
            }

            if (Input.GetKeyDown(bindingKey))
            {
                TriggerQuickOpen();
            }
        }

        /// <summary>
        /// Gets whether the manager is currently listening for a new binding.
        /// </summary>
        public static bool IsListening()
        {
            return isListening;
        }

        /// <summary>
        /// Attempts to parse the stored binding into a KeyCode.
        /// </summary>
        private static bool TryGetBinding(out KeyCode bindingKey)
        {
            bindingKey = KeyCode.None;

            if (hotkeyBinding == null)
            {
                return false;
            }

            string stored = hotkeyBinding.Value;
            if (string.IsNullOrEmpty(stored) || stored == NoneBindingValue)
            {
                return false;
            }

            try
            {
                bindingKey = (KeyCode)Enum.Parse(typeof(KeyCode), stored);
                if (!IsBindingAllowed(bindingKey))
                {
                    ClearStoredBinding();
                    BindingChanged?.Invoke();
                    return false;
                }

                return bindingKey != KeyCode.None;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Saves the given key as the new binding or clears when null.
        /// </summary>
        private static void SetBinding(KeyCode? key)
        {
            if (hotkeyBinding == null)
            {
                return;
            }

            if (key.HasValue)
            {
                if (!IsBindingAllowed(key.Value))
                {
                    ShowDisallowedMessage();
                    return;
                }

                hotkeyBinding.Value = key.Value.ToString();
            }
            else
            {
                hotkeyBinding.Value = NoneBindingValue;
            }

            hotkeyActivationAllowedAt = Time.unscaledTime + ActivationCooldownSeconds;
            CloseBindingPopup();
            BindingChanged?.Invoke();
        }

        /// <summary>
        /// Listens for any key/button input to record a new binding.
        /// </summary>
        private static void ListenForBindingInput()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SetBinding(null);
                isListening = false;
                CloseBindingPopup();
                return;
            }

            foreach (KeyCode keyCode in keyCodes)
            {
                if (keyCode == KeyCode.None)
                {
                    continue;
                }

                if (Input.GetKeyDown(keyCode))
                {
                    if (IsBindingAllowed(keyCode))
                    {
                        SetBinding(keyCode);
                        isListening = false;
                        CloseBindingPopup();
                    }
                    else
                    {
                        ShowDisallowedMessage();
                    }
                    break;
                }
            }
        }

        /// <summary>
        /// Initiates the quick open behaviour for the menu.
        /// </summary>
        private static void TriggerQuickOpen()
        {
            if (executeInProgress)
            {
                return;
            }

            executeInProgress = true;
            CoroutineRunner.Instance.StartCoroutine(ExecuteQuickOpenRoutine());
        }

        /// <summary>
        /// Coroutine that pauses the game if needed and toggles the Cabby Codes menu open state.
        /// </summary>
        private static IEnumerator ExecuteQuickOpenRoutine()
        {
            try
            {
                GameManager gameManager = GameManager.instance ?? GameManager._instance;
                if (gameManager == null)
                {
                    yield break;
                }

                // Wait for the Cabby Codes menu to exist
                while (CabbyCodesPlugin.cabbyMenu == null)
                {
                    yield return null;
                }

                bool isMenuOpen = CabbyCodesPlugin.cabbyMenu.IsMenuOpen();
                if (!isMenuOpen)
                {
                    if (!gameManager.IsGamePaused())
                    {
                        gameManager.StartCoroutine(gameManager.PauseGameToggle());

                        float timeout = Time.realtimeSinceStartup + 3f;
                        while (!gameManager.IsGamePaused() && Time.realtimeSinceStartup < timeout)
                        {
                            yield return null;
                        }
                    }

                    // Give Unity a frame to process pause state/UI changes.
                    yield return null;
                    CabbyCodesPlugin.cabbyMenu.SetMenuOpen(true);
                }
                else
                {
                    CabbyCodesPlugin.cabbyMenu.SetMenuOpen(false);

                    if (gameManager.IsGamePaused())
                    {
                        gameManager.StartCoroutine(gameManager.PauseGameToggle());

                        float timeout = Time.realtimeSinceStartup + 3f;
                        while (gameManager.IsGamePaused() && Time.realtimeSinceStartup < timeout)
                        {
                            yield return null;
                        }
                    }
                }
            }
            finally
            {
                executeInProgress = false;
                hotkeyActivationAllowedAt = Time.unscaledTime + ActivationCooldownSeconds;
            }
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

        private static bool IsBindingAllowed(KeyCode keyCode)
        {
            return keyCode != KeyCode.None && !disallowedKeys.Contains(keyCode);
        }

        private static void ShowDisallowedMessage()
        {
            // Intentionally left blank - disallowed inputs simply do nothing.
        }

        private static void ClearStoredBinding()
        {
            if (hotkeyBinding != null)
            {
                hotkeyBinding.Value = NoneBindingValue;
            }
        }

        /// <summary>
        /// Formats the stored KeyCode string to a more readable form.
        /// </summary>
        private static string FormatKeyCodeDisplay(string rawValue)
        {
            if (string.IsNullOrEmpty(rawValue))
            {
                return "Not Bound";
            }

            // Special case: strip the "KeyCode." prefix if present.
            if (rawValue.StartsWith("KeyCode."))
            {
                rawValue = rawValue.Substring("KeyCode.".Length);
            }

            // Insert spaces before capital letters (e.g., JoystickButton0 -> Joystick Button 0).
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            for (int i = 0; i < rawValue.Length; i++)
            {
                char current = rawValue[i];
                if (i > 0 && char.IsUpper(current) && !char.IsUpper(rawValue[i - 1]))
                {
                    builder.Append(' ');
                }

                if (i > 0 && char.IsDigit(current) && !char.IsDigit(rawValue[i - 1]))
                {
                    builder.Append(' ');
                }

                builder.Append(current);
            }

            return builder.ToString();
        }
    }
}


