using UnityEngine;
using CabbyMenu.SyncedReferences;

namespace CabbyMenu.UI.Controls.CustomDropdown
{
    public class DropDownSync
    {
        private readonly GameObject dropdownGo;
        private readonly CustomDropdown customDropdown;

        public ISyncedReference<int> SelectedValue { get; private set; }

        public DropDownSync(ISyncedReference<int> selectedValue)
        {
            SelectedValue = selectedValue;

            // Create a new GameObject and add CustomDropdown
            dropdownGo = new GameObject("DropDownSync");
            customDropdown = dropdownGo.AddComponent<CustomDropdown>();

            // Set initial value
            customDropdown.SetValue(SelectedValue.Get());
            // Listen for value changes
            customDropdown.onValueChanged.AddListener(DropdownSelect);
        }

        public GameObject GetGameObject()
        {
            return dropdownGo;
        }

        public void DropdownSelect(int value)
        {
            SelectedValue.Set(value);
        }

        public void Update()
        {
            customDropdown.SetValue(SelectedValue.Get());
        }
    }
}