using System.Collections.Generic;
using UnityEngine;

namespace CabbyMenu.Utilities
{
    /// <summary>
    /// Maps Unity KeyCode values to character representations and provides input validation.
    /// </summary>
    public static class KeyCodeMap
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
            AlphaNumeric,

            /// <summary>
            /// Numeric characters (0-9) plus decimal point.
            /// </summary>
            Decimal,

            /// <summary>
            /// Alphabetic, numeric characters, and spaces.
            /// </summary>
            AlphaNumericWithSpaces
        }

        /// <summary>
        /// Dictionary mapping all KeyCodes to their character representations.
        /// </summary>
        private static readonly Dictionary<KeyCode, char> _keyCodeMap = new Dictionary<KeyCode, char>();

        /// <summary>
        /// HashSet of numeric KeyCodes for fast lookup.
        /// </summary>
        private static readonly HashSet<KeyCode> _numericKeys = new HashSet<KeyCode>();

        /// <summary>
        /// HashSet of alphabetic KeyCodes for fast lookup.
        /// </summary>
        private static readonly HashSet<KeyCode> _alphaKeys = new HashSet<KeyCode>();

        /// <summary>
        /// HashSet of decimal KeyCodes (numeric + decimal point) for fast lookup.
        /// </summary>
        private static readonly HashSet<KeyCode> _decimalKeys = new HashSet<KeyCode>();

        /// <summary>
        /// HashSet of alphanumeric with spaces KeyCodes for fast lookup.
        /// </summary>
        private static readonly HashSet<KeyCode> _alphaNumericWithSpacesKeys = new HashSet<KeyCode>();

        static KeyCodeMap()
        {
            // Numeric keys
            _keyCodeMap.Add(KeyCode.Keypad0, '0');
            _keyCodeMap.Add(KeyCode.Keypad1, '1');
            _keyCodeMap.Add(KeyCode.Keypad2, '2');
            _keyCodeMap.Add(KeyCode.Keypad3, '3');
            _keyCodeMap.Add(KeyCode.Keypad4, '4');
            _keyCodeMap.Add(KeyCode.Keypad5, '5');
            _keyCodeMap.Add(KeyCode.Keypad6, '6');
            _keyCodeMap.Add(KeyCode.Keypad7, '7');
            _keyCodeMap.Add(KeyCode.Keypad8, '8');
            _keyCodeMap.Add(KeyCode.Keypad9, '9');
            _keyCodeMap.Add(KeyCode.Alpha0, '0');
            _keyCodeMap.Add(KeyCode.Alpha1, '1');
            _keyCodeMap.Add(KeyCode.Alpha2, '2');
            _keyCodeMap.Add(KeyCode.Alpha3, '3');
            _keyCodeMap.Add(KeyCode.Alpha4, '4');
            _keyCodeMap.Add(KeyCode.Alpha5, '5');
            _keyCodeMap.Add(KeyCode.Alpha6, '6');
            _keyCodeMap.Add(KeyCode.Alpha7, '7');
            _keyCodeMap.Add(KeyCode.Alpha8, '8');
            _keyCodeMap.Add(KeyCode.Alpha9, '9');

            // Decimal point keys
            _keyCodeMap.Add(KeyCode.Period, '.');
            _keyCodeMap.Add(KeyCode.KeypadPeriod, '.');

            // Space key
            _keyCodeMap.Add(KeyCode.Space, ' ');

            // Alphabetic keys
            _keyCodeMap.Add(KeyCode.A, 'A');
            _keyCodeMap.Add(KeyCode.B, 'B');
            _keyCodeMap.Add(KeyCode.C, 'C');
            _keyCodeMap.Add(KeyCode.D, 'D');
            _keyCodeMap.Add(KeyCode.E, 'E');
            _keyCodeMap.Add(KeyCode.F, 'F');
            _keyCodeMap.Add(KeyCode.G, 'G');
            _keyCodeMap.Add(KeyCode.H, 'H');
            _keyCodeMap.Add(KeyCode.I, 'I');
            _keyCodeMap.Add(KeyCode.J, 'J');
            _keyCodeMap.Add(KeyCode.K, 'K');
            _keyCodeMap.Add(KeyCode.L, 'L');
            _keyCodeMap.Add(KeyCode.M, 'M');
            _keyCodeMap.Add(KeyCode.N, 'N');
            _keyCodeMap.Add(KeyCode.O, 'O');
            _keyCodeMap.Add(KeyCode.P, 'P');
            _keyCodeMap.Add(KeyCode.Q, 'Q');
            _keyCodeMap.Add(KeyCode.R, 'R');
            _keyCodeMap.Add(KeyCode.S, 'S');
            _keyCodeMap.Add(KeyCode.T, 'T');
            _keyCodeMap.Add(KeyCode.U, 'U');
            _keyCodeMap.Add(KeyCode.V, 'V');
            _keyCodeMap.Add(KeyCode.W, 'W');
            _keyCodeMap.Add(KeyCode.X, 'X');
            _keyCodeMap.Add(KeyCode.Y, 'Y');
            _keyCodeMap.Add(KeyCode.Z, 'Z');

            // Numeric KeyCodes
            _numericKeys.Add(KeyCode.Keypad0);
            _numericKeys.Add(KeyCode.Keypad1);
            _numericKeys.Add(KeyCode.Keypad2);
            _numericKeys.Add(KeyCode.Keypad3);
            _numericKeys.Add(KeyCode.Keypad4);
            _numericKeys.Add(KeyCode.Keypad5);
            _numericKeys.Add(KeyCode.Keypad6);
            _numericKeys.Add(KeyCode.Keypad7);
            _numericKeys.Add(KeyCode.Keypad8);
            _numericKeys.Add(KeyCode.Keypad9);
            _numericKeys.Add(KeyCode.Alpha0);
            _numericKeys.Add(KeyCode.Alpha1);
            _numericKeys.Add(KeyCode.Alpha2);
            _numericKeys.Add(KeyCode.Alpha3);
            _numericKeys.Add(KeyCode.Alpha4);
            _numericKeys.Add(KeyCode.Alpha5);
            _numericKeys.Add(KeyCode.Alpha6);
            _numericKeys.Add(KeyCode.Alpha7);
            _numericKeys.Add(KeyCode.Alpha8);
            _numericKeys.Add(KeyCode.Alpha9);

            // Alphabetic KeyCodes
            _alphaKeys.Add(KeyCode.A);
            _alphaKeys.Add(KeyCode.B);
            _alphaKeys.Add(KeyCode.C);
            _alphaKeys.Add(KeyCode.D);
            _alphaKeys.Add(KeyCode.E);
            _alphaKeys.Add(KeyCode.F);
            _alphaKeys.Add(KeyCode.G);
            _alphaKeys.Add(KeyCode.H);
            _alphaKeys.Add(KeyCode.I);
            _alphaKeys.Add(KeyCode.J);
            _alphaKeys.Add(KeyCode.K);
            _alphaKeys.Add(KeyCode.L);
            _alphaKeys.Add(KeyCode.M);
            _alphaKeys.Add(KeyCode.N);
            _alphaKeys.Add(KeyCode.O);
            _alphaKeys.Add(KeyCode.P);
            _alphaKeys.Add(KeyCode.Q);
            _alphaKeys.Add(KeyCode.R);
            _alphaKeys.Add(KeyCode.S);
            _alphaKeys.Add(KeyCode.T);
            _alphaKeys.Add(KeyCode.U);
            _alphaKeys.Add(KeyCode.V);
            _alphaKeys.Add(KeyCode.W);
            _alphaKeys.Add(KeyCode.X);
            _alphaKeys.Add(KeyCode.Y);
            _alphaKeys.Add(KeyCode.Z);

            // Decimal KeyCodes (numeric + decimal point) - reuse existing numeric keys
            foreach (var numericKey in _numericKeys)
            {
                _decimalKeys.Add(numericKey);
            }
            _decimalKeys.Add(KeyCode.Period);
            _decimalKeys.Add(KeyCode.KeypadPeriod);

            // AlphaNumeric with spaces KeyCodes - combine alpha, numeric, and space
            foreach (var alphaKey in _alphaKeys)
            {
                _alphaNumericWithSpacesKeys.Add(alphaKey);
            }
            foreach (var numericKey in _numericKeys)
            {
                _alphaNumericWithSpacesKeys.Add(numericKey);
            }
            _alphaNumericWithSpacesKeys.Add(KeyCode.Space);
        }

        /// <summary>
        /// Gets the character representation of the currently pressed key based on the valid character type.
        /// </summary>
        /// <param name="validChars">The type of characters that are valid for input.</param>
        /// <returns>The character representation of the pressed key, or null if no valid key is pressed.</returns>
        public static char? GetChar(ValidChars validChars)
        {
            HashSet<KeyCode> keysToCheck;
            switch (validChars)
            {
                case ValidChars.Alpha:
                    keysToCheck = _alphaKeys;
                    break;
                case ValidChars.Numeric:
                    keysToCheck = _numericKeys;
                    break;
                case ValidChars.Decimal:
                    keysToCheck = _decimalKeys;
                    break;
                case ValidChars.AlphaNumericWithSpaces:
                    keysToCheck = _alphaNumericWithSpacesKeys;
                    break;
                case ValidChars.AlphaNumeric:
                default:
                    keysToCheck = null; // null means check all keys
                    break;
            }

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