using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace CabbyMenu.Utilities
{
    /// <summary>
    /// Represents a single keyboard binding consisting of a main key plus optional
    /// Ctrl / Shift / Alt modifiers. Supports serialization to/from a compact string for
    /// configuration storage and runtime trigger detection.
    /// </summary>
    public class HotkeyBinding
    {
        /// <summary>
        /// String stored in configuration when no key is bound.
        /// </summary>
        public const string NoneValue = "None";

        /// <summary>
        /// Keys that are themselves modifiers and therefore cannot be used as the main key.
        /// </summary>
        public static readonly HashSet<KeyCode> ModifierKeys = new HashSet<KeyCode>
        {
            KeyCode.LeftControl, KeyCode.RightControl,
            KeyCode.LeftShift, KeyCode.RightShift,
            KeyCode.LeftAlt, KeyCode.RightAlt,
            KeyCode.AltGr
        };

        /// <summary>
        /// Keys that are not allowed to be bound as the main key (matches the quick open hotkey rules).
        /// </summary>
        public static readonly HashSet<KeyCode> DisallowedKeys = new HashSet<KeyCode>
        {
            KeyCode.Mouse0,
            KeyCode.Mouse1
        };

        /// <summary>
        /// The main (non-modifier) key. KeyCode.None when unbound.
        /// </summary>
        public KeyCode Key { get; set; } = KeyCode.None;

        /// <summary>
        /// Whether a Ctrl modifier is required.
        /// </summary>
        public bool Ctrl { get; set; }

        /// <summary>
        /// Whether a Shift modifier is required.
        /// </summary>
        public bool Shift { get; set; }

        /// <summary>
        /// Whether an Alt modifier is required.
        /// </summary>
        public bool Alt { get; set; }

        /// <summary>
        /// Gets whether this binding has a usable main key.
        /// </summary>
        public bool IsBound => Key != KeyCode.None;

        /// <summary>
        /// Gets whether a Ctrl key is currently held.
        /// </summary>
        public static bool CtrlHeld => Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);

        /// <summary>
        /// Gets whether a Shift key is currently held.
        /// </summary>
        public static bool ShiftHeld => Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        /// <summary>
        /// Gets whether an Alt key is currently held.
        /// </summary>
        public static bool AltHeld => Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt) || Input.GetKey(KeyCode.AltGr);

        /// <summary>
        /// Determines whether a key code is one of the modifier keys.
        /// </summary>
        public static bool IsModifierKey(KeyCode keyCode)
        {
            return ModifierKeys.Contains(keyCode);
        }

        /// <summary>
        /// Determines whether a key code is allowed to be used as a main binding key.
        /// </summary>
        public static bool IsAllowedMainKey(KeyCode keyCode)
        {
            return keyCode != KeyCode.None && !DisallowedKeys.Contains(keyCode) && !ModifierKeys.Contains(keyCode);
        }

        /// <summary>
        /// Builds a binding from the given main key plus whichever modifiers are currently held.
        /// </summary>
        public static HotkeyBinding CaptureCurrentModifiers(KeyCode mainKey)
        {
            return new HotkeyBinding
            {
                Key = mainKey,
                Ctrl = CtrlHeld,
                Shift = ShiftHeld,
                Alt = AltHeld
            };
        }

        /// <summary>
        /// Determines whether the bound modifiers match the modifiers currently held.
        /// </summary>
        public bool ModifiersMatch()
        {
            return Ctrl == CtrlHeld && Shift == ShiftHeld && Alt == AltHeld;
        }

        /// <summary>
        /// Determines whether this binding was triggered this frame (key pressed with matching modifiers).
        /// </summary>
        public bool IsTriggered()
        {
            return IsBound && Input.GetKeyDown(Key) && ModifiersMatch();
        }

        /// <summary>
        /// Serializes the binding to a compact configuration string, e.g. "Ctrl+Shift+F5" or "None".
        /// </summary>
        public string Serialize()
        {
            if (!IsBound)
            {
                return NoneValue;
            }

            StringBuilder builder = new StringBuilder();
            if (Ctrl)
            {
                builder.Append("Ctrl+");
            }
            if (Shift)
            {
                builder.Append("Shift+");
            }
            if (Alt)
            {
                builder.Append("Alt+");
            }
            builder.Append(Key.ToString());
            return builder.ToString();
        }

        /// <summary>
        /// Parses a serialized binding string. Returns an unbound binding when the value is empty,
        /// "None", or cannot be parsed.
        /// </summary>
        public static HotkeyBinding Parse(string value)
        {
            HotkeyBinding binding = new HotkeyBinding();

            if (string.IsNullOrEmpty(value) || value == NoneValue)
            {
                return binding;
            }

            string[] parts = value.Split('+');
            foreach (string rawPart in parts)
            {
                string part = rawPart.Trim();
                if (part.Length == 0)
                {
                    continue;
                }

                switch (part.ToLowerInvariant())
                {
                    case "ctrl":
                    case "control":
                        binding.Ctrl = true;
                        break;
                    case "shift":
                        binding.Shift = true;
                        break;
                    case "alt":
                        binding.Alt = true;
                        break;
                    default:
                        try
                        {
                            binding.Key = (KeyCode)Enum.Parse(typeof(KeyCode), part, true);
                        }
                        catch
                        {
                            binding.Key = KeyCode.None;
                        }
                        break;
                }
            }

            // If only modifiers parsed (no valid main key), treat as unbound.
            if (!binding.IsBound)
            {
                return new HotkeyBinding();
            }

            return binding;
        }

        /// <summary>
        /// Returns a compact, user-friendly display string for the binding, e.g. "Ctrl+Shift+F5".
        /// </summary>
        public string ToDisplay()
        {
            if (!IsBound)
            {
                return "Not Bound";
            }

            StringBuilder builder = new StringBuilder();
            if (Ctrl)
            {
                builder.Append("Ctrl+");
            }
            if (Shift)
            {
                builder.Append("Shift+");
            }
            if (Alt)
            {
                builder.Append("Alt+");
            }
            builder.Append(FriendlyKeyName(Key));
            return builder.ToString();
        }

        /// <summary>
        /// Produces a friendly name for a key. Top-row number keys (Alpha0-9) display as the bare
        /// digit, while numpad keys (Keypad0-9) keep their distinction (e.g. "Keypad 4").
        /// </summary>
        private static string FriendlyKeyName(KeyCode key)
        {
            string raw = key.ToString();

            // "Alpha4" -> "4" for the main keyboard number row (numpad keys keep their distinction).
            if (raw.Length == 6 && raw.StartsWith("Alpha") && char.IsDigit(raw[5]))
            {
                return raw.Substring(5);
            }

            // Controller buttons -> identifiable names (Xbox-style layout, the common PC default).
            int joystickButton = ExtractJoystickButtonIndex(raw);
            if (joystickButton >= 0)
            {
                return "Pad " + GamepadButtonName(joystickButton);
            }

            return FormatKeyName(raw);
        }

        /// <summary>
        /// Returns the trailing button index of a joystick key name (e.g. "JoystickButton3" or
        /// "Joystick1Button3" -> 3), or -1 if the key is not a joystick button.
        /// </summary>
        private static int ExtractJoystickButtonIndex(string raw)
        {
            if (!raw.StartsWith("Joystick"))
            {
                return -1;
            }

            int buttonIdx = raw.IndexOf("Button", StringComparison.Ordinal);
            if (buttonIdx < 0)
            {
                return -1;
            }

            string numberPart = raw.Substring(buttonIdx + "Button".Length);
            return int.TryParse(numberPart, out int index) ? index : -1;
        }

        /// <summary>
        /// Maps a controller button index to a friendly name using the standard Xbox-on-Windows layout.
        /// </summary>
        private static string GamepadButtonName(int index)
        {
            switch (index)
            {
                case 0: return "A";
                case 1: return "B";
                case 2: return "X";
                case 3: return "Y";
                case 4: return "LB";
                case 5: return "RB";
                case 6: return "Back";
                case 7: return "Start";
                case 8: return "LS";
                case 9: return "RS";
                default: return "Btn " + index;
            }
        }

        /// <summary>
        /// Inserts spaces before capital letters and digit groups for readability
        /// (e.g. "JoystickButton0" -> "Joystick Button 0").
        /// </summary>
        private static string FormatKeyName(string rawValue)
        {
            if (string.IsNullOrEmpty(rawValue))
            {
                return rawValue;
            }

            StringBuilder builder = new StringBuilder();
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
