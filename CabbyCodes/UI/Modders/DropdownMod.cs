using UnityEngine;
using UnityEngine.UI;

namespace CabbyCodes.UI.Modders
{
    public class DropdownMod
    {
        private readonly GameObject dropdownGameObject;

        public DropdownMod(GameObject dropdownGameObject)
        {
            this.dropdownGameObject = dropdownGameObject;
        }

        public GameObject Get()
        {
            return dropdownGameObject;
        }

        public DropdownMod SetSize(Vector2 size, int showSize = 5)
        {
            new Fitter(dropdownGameObject).Size(size);
            
            GameObject template = dropdownGameObject.transform.Find("Template").gameObject;
            template.GetComponent<RectTransform>().sizeDelta = new Vector2(0, size.y * showSize);
            template.GetComponent<ScrollRect>().scrollSensitivity = size.y;

            GameObject viewport = template.transform.Find("Viewport").gameObject;
            viewport.GetComponent<RectTransform>().sizeDelta = new Vector2(0, size.y * showSize);

            GameObject content = viewport.transform.Find("Content").gameObject; // dropdown popup
            content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, size.y);

            GameObject item = content.transform.Find("Item").gameObject;
            item.GetComponent<RectTransform>().sizeDelta = new Vector2(0, size.y);

            return this;
        }

        public DropdownMod SetFontSize(int fontSize)
        {
            dropdownGameObject.transform.Find("Label").gameObject.GetComponent<Text>().fontSize = fontSize;

            GameObject item = dropdownGameObject.transform.Find("Template").Find("Viewport").Find("Content").Find("Item").gameObject;
            item.transform.Find("Item Label").gameObject.GetComponent<Text>().fontSize = fontSize;

            return this;
        }
    }
}
