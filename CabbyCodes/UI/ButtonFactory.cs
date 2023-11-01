using UnityEngine;
using UnityEngine.UI;

namespace CabbyCodes.UI
{
    public class ButtonFactory : TextFactory
    {
        public ButtonFactory(string text) : base(DefaultControls.CreateButton(new DefaultControls.Resources()), text) { }

        public new GameObject Build()
        {
            return base.Build();
        }

    }
}
