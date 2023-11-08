using CabbyCodes.SyncedReferences;
using CabbyCodes.UI.Factories;
using CabbyCodes.UI.Modders;
using UnityEngine;
using UnityEngine.UI;

namespace CabbyCodes.UI.ReferenceControls
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
            dropdownGo.GetComponent<Dropdown>().onValueChanged.AddListener(DropdownSelect);
        }

        public GameObject GetGameObject()
        {
            return dropdownGo;
        }

        public void DropdownSelect(int value)
        {
            SelectedValue.Set(value);
        }
    }
}
