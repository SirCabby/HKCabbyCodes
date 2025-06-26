using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.ReferenceControls;
using UnityEngine.UI;

namespace CabbyMenu.UI.CheatPanels
{
    public class InputFieldPanel<T> : CheatPanel
    {
        private InputFieldSync<T> inputFieldSync;

        public InputFieldPanel(ISyncedReference<T> syncedReference, KeyCodeMap.ValidChars validChars, int characterLimit, int width, string description) : base(description)
        {
            inputFieldSync = new(syncedReference, validChars, new(width, Constants.DEFAULT_PANEL_HEIGHT), characterLimit);
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