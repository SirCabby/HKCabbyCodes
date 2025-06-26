using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.Factories;
using CabbyMenu.UI.Modders;
using UnityEngine;
using UnityEngine.UI;

namespace CabbyMenu.UI.ReferenceControls
{
    public class DropDownSync
    {
        private readonly GameObject dropdownGo;

        public ISyncedReference<int> SelectedValue { get; private set; }

        public DropDownSync(ISyncedReference<int> selectedValue)
        {
            SelectedValue = selectedValue;

            (dropdownGo, GameObjectMod dropdownGoMod, DropdownMod dropdownMod) = DropdownFactory.Build();
            dropdownGoMod.SetName("DropDownSync");

            Dropdown dropdown = dropdownGo.GetComponent<Dropdown>();

            dropdown.onValueChanged.AddListener(DropdownSelect);
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
            dropdownGo.GetComponent<Dropdown>().value = SelectedValue.Get();
        }
    }
}