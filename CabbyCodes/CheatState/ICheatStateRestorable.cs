namespace CabbyCodes
{
    /// <summary>
    /// Interface for cheats that can have their state restored after scene transitions.
    /// </summary>
    public interface ICheatStateRestorable
    {
        /// <summary>
        /// Gets the unique key for this cheat.
        /// </summary>
        string GetCheatKey();

        /// <summary>
        /// Restores the cheat's active state after a scene transition.
        /// </summary>
        void RestoreState();
    }
} 