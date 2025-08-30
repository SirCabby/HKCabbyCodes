namespace CabbyMenu.UI
{
    /// <summary>
    /// A simple interface for managing a persistent popup.
    /// </summary>
    public interface IPersistentPopup
    {
        void Show();
        void Hide();
        void Destroy();
        void SetMessageText(string message);
    }
} 