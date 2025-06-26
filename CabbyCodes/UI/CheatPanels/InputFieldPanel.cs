using CabbyCodes.SyncedReferences;
using CabbyCodes.UI.ReferenceControls;
using UnityEngine.UI;

namespace CabbyCodes.UI.CheatPanels
{
    public class InputFieldPanel<T> : CheatPanel
    {
        private InputFieldSync<T> inputFieldSync;

        public InputFieldPanel(ISyncedReference<T> syncedReference, KeyCodeMap.ValidChars validChars, int characterLimit, int width, string description) : base(description)
        {
            inputFieldSync = new(syncedReference, validChars, new(width, 60), characterLimit);
            new Fitter(inputFieldSync.GetGameObject()).Attach(cheatPanel);
            inputFieldSync.GetGameObject().transform.SetAsFirstSibling();
            updateActions.Add(inputFieldSync.Update);
        }

        public InputField GetInputField()
        {
            return inputFieldSync.GetGameObject().GetComponent<InputField>();
        }
    }
}
