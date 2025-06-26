using System.Collections.Generic;
using UnityEngine;

namespace CabbyCodes
{
    /// <summary>
    /// Maps Unity KeyCode values to character representations and provides input validation.
    /// </summary>
    public class KeyCodeMap
    {
        /// <summary>
        /// Defines the types of valid characters that can be input.
        /// </summary>
        public enum ValidChars
        {
            /// <summary>
            /// Only numeric characters (0-9).
            /// </summary>
            Numeric,

            /// <summary>
            /// Only alphabetic characters (A-Z).
            /// </summary>
            Alpha,

            /// <summary>
            /// Both alphabetic and numeric characters.
            /// </summary>
            AlphaNumeric
        }

        /// <summary>
        /// Dictionary mapping all KeyCodes to their character representations.
        /// </summary>
        private static readonly Dictionary<KeyCode, char> _keyCodeMap = new()
        {
            // Numeric keys
            { KeyCode.Keypad0, '0' },
            { KeyCode.Keypad1, '1' },
            { KeyCode.Keypad2, '2' },
            { KeyCode.Keypad3, '3' },
            { KeyCode.Keypad4, '4' },
            { KeyCode.Keypad5, '5' },
            { KeyCode.Keypad6, '6' },
            { KeyCode.Keypad7, '7' },
            { KeyCode.Keypad8, '8' },
            { KeyCode.Keypad9, '9' },
            { KeyCode.Alpha0, '0' },
            { KeyCode.Alpha1, '1' },
            { KeyCode.Alpha2, '2' },
            { KeyCode.Alpha3, '3' },
            { KeyCode.Alpha4, '4' },
            { KeyCode.Alpha5, '5' },
            { KeyCode.Alpha6, '6' },
            { KeyCode.Alpha7, '7' },
            { KeyCode.Alpha8, '8' },
            { KeyCode.Alpha9, '9' },
            
            // Alphabetic keys
            { KeyCode.A, 'A' },
            { KeyCode.B, 'B' },
            { KeyCode.C, 'C' },
            { KeyCode.D, 'D' },
            { KeyCode.E, 'E' },
            { KeyCode.F, 'F' },
            { KeyCode.G, 'G' },
            { KeyCode.H, 'H' },
            { KeyCode.I, 'I' },
            { KeyCode.J, 'J' },
            { KeyCode.K, 'K' },
            { KeyCode.L, 'L' },
            { KeyCode.M, 'M' },
            { KeyCode.N, 'N' },
            { KeyCode.O, 'O' },
            { KeyCode.P, 'P' },
            { KeyCode.Q, 'Q' },
            { KeyCode.R, 'R' },
            { KeyCode.S, 'S' },
            { KeyCode.T, 'T' },
            { KeyCode.U, 'U' },
            { KeyCode.V, 'V' },
            { KeyCode.W, 'W' },
            { KeyCode.X, 'X' },
            { KeyCode.Y, 'Y' },
            { KeyCode.Z, 'Z' }
        };

        /// <summary>
        /// HashSet of numeric KeyCodes for fast lookup.
        /// </summary>
        private static readonly HashSet<KeyCode> _numericKeys = new()
        {
            KeyCode.Keypad0, KeyCode.Keypad1, KeyCode.Keypad2, KeyCode.Keypad3, KeyCode.Keypad4,
            KeyCode.Keypad5, KeyCode.Keypad6, KeyCode.Keypad7, KeyCode.Keypad8, KeyCode.Keypad9,
            KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4,
            KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9
        };

        /// <summary>
        /// HashSet of alphabetic KeyCodes for fast lookup.
        /// </summary>
        private static readonly HashSet<KeyCode> _alphaKeys = new()
        {
            KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F, KeyCode.G, KeyCode.H,
            KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N, KeyCode.O, KeyCode.P,
            KeyCode.Q, KeyCode.R, KeyCode.S, KeyCode.T, KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X,
            KeyCode.Y, KeyCode.Z
        };

        /// <summary>
        /// Gets the character representation of the currently pressed key based on the valid character type.
        /// </summary>
        /// <param name="validChars">The type of characters that are valid for input.</param>
        /// <returns>The character representation of the pressed key, or null if no valid key is pressed.</returns>
        public static char? GetChar(ValidChars validChars)
        {
            // Determine which keys to check based on validChars
            HashSet<KeyCode> keysToCheck = validChars switch
            {
                ValidChars.Alpha => _alphaKeys,
                ValidChars.Numeric => _numericKeys,
                ValidChars.AlphaNumeric => null, // null means check all keys
                _ => null
            };

            // If AlphaNumeric or default, check all keys in the map
            if (keysToCheck == null)
            {
                foreach (var kvp in _keyCodeMap)
                {
                    if (Input.GetKeyDown(kvp.Key))
                    {
                        return kvp.Value;
                    }
                }
            }
            else
            {
                // Check only the specified key types
                foreach (var keyCode in keysToCheck)
                {
                    if (Input.GetKeyDown(keyCode))
                    {
                        return _keyCodeMap[keyCode];
                    }
                }
            }

            return null;
        }
    }
}
