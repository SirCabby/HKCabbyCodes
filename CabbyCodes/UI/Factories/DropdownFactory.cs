using UnityEngine.UI;
using UnityEngine;
using CabbyCodes.UI.Modders;

namespace CabbyCodes.UI.Factories
{
    public class DropdownFactory
    {
        public static (GameObject gameObject, GameObjectMod gameObjectMod, DropdownMod dropdownMod) Build()
        {
            GameObject buildInstance = DefaultControls.CreateDropdown(new DefaultControls.Resources());
            new ScrollBarMod(buildInstance.transform.Find("Template").Find("Scrollbar").gameObject.GetComponent<Scrollbar>()).SetDefaults();

            (GameObject gameObject, GameObjectMod gameObjectMod) = GameObjectFactory.Build(buildInstance);
            return (gameObject, gameObjectMod, new DropdownMod(buildInstance));
        }
    }
}
