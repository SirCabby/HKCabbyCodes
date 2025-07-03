using System;
using UnityEngine;
using CabbyMenu.SyncedReferences;
using CabbyMenu.TextProcessors;
using CabbyMenu.Utilities;

namespace CabbyMenu.UI.Controls.InputField
{
    /// <summary>
    /// Input field synchronization for string types without range validation.
    /// </summary>
    public class StringInputFieldSync : BaseInputFieldSync<string>
    {
        public StringInputFieldSync(ISyncedReference<string> inputValue, KeyCodeMap.ValidChars validChars, Vector2 size, int characterLimit) 
            : base(inputValue, validChars, size, characterLimit)
        {
        }

        protected override InputFieldStatus<string> CreateInputFieldStatus(KeyCodeMap.ValidChars validChars, int maxVisibleCharacters)
        {
            // For strings, we don't have a logical max value, so pass empty string
            return new InputFieldStatus<string>(inputFieldGo, SetSelected, Submit, Cancel, validChars, maxVisibleCharacters, textProcessor, string.Empty);
        }

        protected override void HandleSubmit(string text)
        {
            // For strings, just set the value directly without range validation
            InputValue.Set(text);
        }
    }
} 