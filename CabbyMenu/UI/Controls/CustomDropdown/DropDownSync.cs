using UnityEngine;
using CabbyMenu.SyncedReferences;

namespace CabbyMenu.UI.Controls.CustomDropdown
{
    /// <summary>
    /// Provides data synchronization between ISyncedReference<int> and CustomDropdown UI component.
    /// This class focuses solely on data binding and does not handle UI creation.
    /// </summary>
    public class DropDownSync
    {
        private readonly CustomDropdown customDropdown;

        public ISyncedReference<int> SelectedValue { get; private set; }

        /// <summary>
        /// Factory method to create a DropDownSync with a new CustomDropdown GameObject.
        /// This maintains backward compatibility with the old API.
        /// </summary>
        /// <param name="selectedValue">The data source to synchronize with</param>
        /// <returns>A tuple containing the DropDownSync and the GameObject</returns>
        public static (DropDownSync dropDownSync, GameObject gameObject) Create(ISyncedReference<int> selectedValue)
        {
            var (gameObject, customDropdown) = CustomDropdown.Build();
            var dropDownSync = new DropDownSync(selectedValue, customDropdown);
            return (dropDownSync, gameObject);
        }

        /// <summary>
        /// Creates a new DropDownSync that synchronizes data with an existing CustomDropdown.
        /// </summary>
        /// <param name="selectedValue">The data source to synchronize with</param>
        /// <param name="customDropdown">The UI component to bind to</param>
        public DropDownSync(ISyncedReference<int> selectedValue, CustomDropdown customDropdown)
        {
            SelectedValue = selectedValue;
            this.customDropdown = customDropdown;

            // Set initial value
            customDropdown.SetValue(SelectedValue.Get());
            
            // Listen for value changes from UI
            customDropdown.onValueChanged.AddListener(DropdownSelect);
        }

        /// <summary>
        /// Handles UI selection changes and updates the data source.
        /// </summary>
        /// <param name="value">The selected index from the UI</param>
        public void DropdownSelect(int value)
        {
            SelectedValue.Set(value);
        }

        /// <summary>
        /// Synchronizes the UI with the current data value.
        /// Call this when the data source changes externally.
        /// </summary>
        public void Update()
        {
            customDropdown.SetValue(SelectedValue.Get());
        }

        /// <summary>
        /// Gets the underlying CustomDropdown component for direct UI manipulation.
        /// </summary>
        /// <returns>The CustomDropdown component</returns>
        public CustomDropdown GetCustomDropdown()
        {
            return customDropdown;
        }

        /// <summary>
        /// Enables or disables dynamic sizing based on the largest dropdown item.
        /// </summary>
        /// <param name="enabled">True to enable dynamic sizing, false to use fixed width</param>
        public void SetDynamicSizing(bool enabled)
        {
            customDropdown.SetDynamicSizing(enabled);
        }

        /// <summary>
        /// Sets the dropdown options and triggers dynamic sizing if enabled.
        /// </summary>
        /// <param name="options">The list of options to display</param>
        public void SetOptions(System.Collections.Generic.List<string> options)
        {
            customDropdown.SetOptions(options);
        }

        /// <summary>
        /// Sets the size of the dropdown. If dynamic sizing is enabled, only height changes are applied.
        /// </summary>
        /// <param name="width">The width in pixels (ignored if dynamic sizing is enabled)</param>
        /// <param name="height">The height in pixels</param>
        public void SetSize(float width, float height)
        {
            customDropdown.SetSize(width, height);
        }

        /// <summary>
        /// Sets the width of the dropdown. If dynamic sizing is enabled, this call is ignored.
        /// </summary>
        /// <param name="width">The width in pixels</param>
        public void SetSize(float width)
        {
            customDropdown.SetSize(width);
        }
    }
}