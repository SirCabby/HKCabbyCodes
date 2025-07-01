namespace CabbyMenu
{
    /// <summary>
    /// Interface for providing game state information to the UI library.
    /// This allows the UI library to remain independent of game-specific assemblies.
    /// </summary>
    public interface IGameStateProvider
    {
        /// <summary>
        /// Gets whether the menu should be shown.
        /// </summary>
        /// <returns>True if the menu should be shown, false otherwise.</returns>
        bool ShouldShowMenu();
    }
}