namespace CabbyMenu.SyncedReferences
{
    /// <summary>
    /// Interface for synchronizing values between UI controls and game data.
    /// </summary>
    /// <typeparam name="T">The type of value to synchronize.</typeparam>
    public interface ISyncedReference<T>
    {
        /// <summary>
        /// Gets the current value from the game data.
        /// </summary>
        /// <returns>The current value.</returns>
        T Get();

        /// <summary>
        /// Sets the value in the game data.
        /// </summary>
        /// <param name="value">The value to set.</param>
        void Set(T value);
    }
}