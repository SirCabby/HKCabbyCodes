using UnityEngine;
using CabbyMenu.SyncedReferences;
using CabbyMenu.Utilities;
using System;

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

        public StringInputFieldSync(ISyncedReference<string> inputValue, KeyCodeMap.ValidChars validChars, Vector2 size, int characterLimit, Action onSubmitCallback, Action onCancelCallback = null) 
            : base(inputValue, validChars, size, characterLimit, onSubmitCallback, onCancelCallback)
        {
        }

        protected override void HandleSubmit(string text)
        {
            // For strings, just set the value directly without range validation
            InputValue.Set(text);
        }

        protected override bool CanSubmitText(string text)
        {
            // Strings can always be submitted, including empty strings
            return true;
        }
    }
} 