using UnityEngine;
using CabbyMenu.UI.Controls.CustomDropdown;

namespace CabbyMenu.UI.Factories
{
    /// <summary>
    /// Factory class for creating and configuring custom dropdown UI elements.
    /// </summary>
    public class DropdownFactory
    {
        /// <summary>
        /// Builds a custom dropdown GameObject with default styling.
        /// </summary>
        /// <returns>A tuple containing the GameObject and CustomDropdown for the created dropdown.</returns>
        public static (GameObject gameObject, CustomDropdown customDropdown) Build()
        {
            GameObject dropdownGO = new GameObject("CustomDropdown");
            CustomDropdown customDropdown = dropdownGO.AddComponent<CustomDropdown>();
            return (dropdownGO, customDropdown);
        }
    }
}