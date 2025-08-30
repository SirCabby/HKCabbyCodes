using UnityEngine;
using UnityEngine.UI;

namespace CabbyMenu.UI
{
    /// <summary>
    /// A wrapper for a persistent popup that can survive scene changes.
    /// </summary>
    public class PersistentPopupWrapper : IPersistentPopup
    {
        private readonly GameObject persistentRoot;

        public PersistentPopupWrapper(GameObject persistentRoot)
        {
            this.persistentRoot = persistentRoot;
        }

        public void Show()
        {
            persistentRoot?.SetActive(true);
        }

        public void Hide()
        {
            persistentRoot?.SetActive(false);
        }

        public void Destroy()
        {
            if (persistentRoot != null)
            {
                Object.Destroy(persistentRoot);
            }
        }

        public void SetMessageText(string message)
        {
            var messageText = persistentRoot.transform.Find("Popup Panel/Message Text").GetComponent<Text>();
            if (messageText != null)
            {
                messageText.text = message;
            }
        }
    }
} 