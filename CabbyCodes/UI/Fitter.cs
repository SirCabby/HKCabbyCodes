using UnityEngine;

namespace CabbyCodes.UI
{
    /// <summary>
    /// Utility class for configuring and positioning UI GameObjects in Unity.
    /// </summary>
    public class Fitter
    {
        /// <summary>
        /// The GameObject being configured.
        /// </summary>
        private readonly GameObject go;

        /// <summary>
        /// Initializes a new instance of the Fitter class.
        /// </summary>
        /// <param name="go">The GameObject to configure.</param>
        public Fitter(GameObject go)
        {
            this.go = go;
        }

        /// <summary>
        /// Attaches the GameObject to a Canvas parent.
        /// </summary>
        /// <param name="parent">The Canvas parent to attach to.</param>
        /// <returns>This Fitter instance for method chaining.</returns>
        public Fitter Attach(Canvas parent)
        {
            AttachToTransform(parent.transform);
            return this;
        }

        /// <summary>
        /// Attaches the GameObject to another GameObject parent.
        /// </summary>
        /// <param name="parent">The GameObject parent to attach to.</param>
        /// <returns>This Fitter instance for method chaining.</returns>
        public Fitter Attach(GameObject parent)
        {
            AttachToTransform(parent.transform);
            return this;
        }

        /// <summary>
        /// Sets the anchor points for the GameObject's RectTransform.
        /// </summary>
        /// <param name="minAnchor">The minimum anchor point (bottom-left).</param>
        /// <param name="maxAnchor">The maximum anchor point (top-right).</param>
        /// <returns>This Fitter instance for method chaining.</returns>
        public Fitter Anchor(Vector2 minAnchor, Vector2 maxAnchor)
        {
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.anchorMax = maxAnchor;
            rect.anchorMin = minAnchor;
            return this;
        }

        /// <summary>
        /// Sets the size delta for the GameObject's RectTransform.
        /// </summary>
        /// <param name="sizeDelta">The size delta to set.</param>
        /// <returns>This Fitter instance for method chaining.</returns>
        public Fitter Size(Vector2 sizeDelta)
        {
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.sizeDelta = sizeDelta;
            return this;
        }

        /// <summary>
        /// Attaches the GameObject to a parent Transform.
        /// </summary>
        /// <param name="parent">The parent Transform to attach to.</param>
        private void AttachToTransform(Transform parent)
        {
            go.transform.SetParent(parent, false);
        }
    }
}
