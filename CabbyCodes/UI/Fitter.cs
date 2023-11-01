using UnityEngine;

namespace CabbyCodes.UI
{
    public class Fitter
    {
        GameObject go;

        public Fitter(GameObject go)
        {
            this.go = go;
        }

        public Fitter Attach(Canvas parent)
        {
            AttachToTransform(parent.transform);
            return this;
        }

        public Fitter Attach(GameObject parent)
        {
            AttachToTransform(parent.transform);
            return this;
        }

        public Fitter Anchor(Vector2 minAnchor, Vector2 maxAnchor)
        {
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.anchorMax = maxAnchor;
            rect.anchorMin = minAnchor;
            return this;
        }

        public Fitter Size(Vector2 sizeDelta)
        {
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.sizeDelta = sizeDelta;
            return this;
        }

        private void AttachToTransform(Transform parent)
        {
            go.transform.SetParent(parent, false);
        }
    }
}
