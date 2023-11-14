using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CabbyCodes
{
    public class KeyCodeMap
    {
        public enum ValidChars
        {
            Numeric,
            Alpha,
            AlphaNumeric
        }

        private static readonly Dictionary<KeyCode, char> numbers = new()
        {
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
            { KeyCode.Alpha9, '9' }
        };

        private static readonly Dictionary<KeyCode, char> letters = new()
        {
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

        private static readonly Dictionary<KeyCode, char> allCharacters = new();

        static KeyCodeMap()
        {
            numbers.ToList().ForEach(kvp => allCharacters[kvp.Key] = kvp.Value);
            letters.ToList().ForEach(kvp => allCharacters[kvp.Key] = kvp.Value);
        }

        public static char? GetChar(ValidChars validChars)
        {
            Dictionary<KeyCode, char> validCharsDict = validChars switch
            {
                ValidChars.Alpha => letters,
                ValidChars.Numeric => numbers,
                ValidChars.AlphaNumeric => allCharacters,
                _ => allCharacters,
            };

            foreach (KeyValuePair<KeyCode, char> kvp in validCharsDict)
            {
                if (Input.GetKeyDown(kvp.Key))
                {
                    return kvp.Value;
                }
            }

            return null;
        }
    }
}
