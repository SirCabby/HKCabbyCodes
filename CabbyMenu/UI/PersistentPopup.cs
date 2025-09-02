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
            try
            {
                if (persistentRoot != null && persistentRoot)
                {
                    persistentRoot.SetActive(true);
                }
            }
            catch (System.Exception ex)
            {
                // Log the error but don't rethrow to prevent crashes
                UnityEngine.Debug.LogWarning($"PersistentPopupWrapper.Show() failed: {ex.Message}");
            }
        }

        public void Hide()
        {
            try
            {
                if (persistentRoot != null && persistentRoot)
                {
                    persistentRoot.SetActive(false);
                }
            }
            catch (System.Exception ex)
            {
                // Log the error but don't rethrow to prevent crashes
                UnityEngine.Debug.LogWarning($"PersistentPopupWrapper.Hide() failed: {ex.Message}");
            }
        }

        public void Destroy()
        {
            try
            {
                if (persistentRoot != null && persistentRoot)
                {
                    Object.Destroy(persistentRoot);
                }
            }
            catch (System.Exception ex)
            {
                // Log the error but don't rethrow to prevent crashes
                UnityEngine.Debug.LogWarning($"PersistentPopupWrapper.Destroy() failed: {ex.Message}");
            }
        }

        public void SetMessageText(string message)
        {
            try
            {
                if (persistentRoot == null || !persistentRoot)
                {
                    return;
                }
                
                var messageTextTransform = persistentRoot.transform.Find("Popup Panel/Message Text");
                if (messageTextTransform == null)
                {
                    return;
                }
                
                var messageText = messageTextTransform.GetComponent<Text>();
                if (messageText != null)
                {
                    messageText.text = message;
                }
            }
            catch (System.Exception ex)
            {
                // Log the error but don't rethrow to prevent crashes
                UnityEngine.Debug.LogWarning($"PersistentPopupWrapper.SetMessageText() failed: {ex.Message}");
            }
        }
    }
} 