using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace CabbyMenu.UI.Controls.CustomDropdown
{
    /// <summary>
    /// When attached to a UI element, forwards mouse-wheel scroll events to a designated ScrollRect.
    /// This allows scrolling even when the pointer is hovering over child buttons inside the dropdown panel.
    /// </summary>
    public class OptionScrollProxy : MonoBehaviour, IScrollHandler
    {
        private ScrollRect targetScrollRect;

        /// <summary>
        /// Assign the ScrollRect that should receive forwarded scroll events.
        /// </summary>
        public void Initialize(ScrollRect scrollRect)
        {
            targetScrollRect = scrollRect;
        }

        public void OnScroll(PointerEventData eventData)
        {
            if (targetScrollRect != null && targetScrollRect.enabled)
            {
                targetScrollRect.OnScroll(eventData);
            }
        }
    }
} 