using CabbyCodes.SyncedReferences;
using CabbyCodes.UI.ReferenceControls;

namespace CabbyCodes.UI.CheatPanels
{
    public class InputFieldPanel<T> : CheatPanel
    {
        public InputFieldPanel(ISyncedReference<T> syncedReference, KeyCodeMap.ValidChars validChars, int characterLimit, int width, string description) : base(description)
        {
            InputFieldSync<T> inputField = new(syncedReference, validChars, new(width, 60), characterLimit);
            new Fitter(inputField.GetGameObject()).Attach(cheatPanel);
            inputField.GetGameObject().transform.SetAsFirstSibling();
            updateActions.Add(inputField.Update);
        }
    }
}
