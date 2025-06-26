using UnityEngine.UI;
using UnityEngine;
using CabbyCodes.UI.Modders;

namespace CabbyCodes.UI.Factories
{
    /// <summary>
    /// Factory class for creating and configuring dropdown UI elements.
    /// </summary>
    public class DropdownFactory
    {
        /// <summary>
        /// Builds a dropdown GameObject with default styling and scrollbar configuration.
        /// </summary>
        /// <returns>A tuple containing the GameObject, GameObjectMod, and DropdownMod for the created dropdown.</returns>
        public static (GameObject gameObject, GameObjectMod gameObjectMod, DropdownMod dropdownMod) Build()
        {
            GameObject buildInstance = DefaultControls.CreateDropdown(new DefaultControls.Resources());
            new ScrollBarMod(buildInstance.transform.Find("Template").Find("Scrollbar").gameObject.GetComponent<Scrollbar>()).SetDefaults();

            (GameObject gameObject, GameObjectMod gameObjectMod) = GameObjectFactory.Build(buildInstance);
            return (gameObject, gameObjectMod, new DropdownMod(buildInstance));
        }
    }
}
