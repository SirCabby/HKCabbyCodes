using CabbyMenu.Types;

namespace CabbyCodes
{
    /// <summary>
    /// Implementation of IGameStateProvider that provides game state information to CabbyMenu.
    /// This allows CabbyMenu to remain independent of game-specific assemblies.
    /// </summary>
    public class GameStateProvider : IGameStateProvider
    {
        /// <summary>
        /// Gets whether the menu should be shown.
        /// </summary>
        /// <returns>True if the menu should be shown, false otherwise.</returns>
        public bool ShouldShowMenu()
        {
            // For now, show the menu when the game is paused
            return GameManager._instance != null && GameManager.instance.IsGamePaused();
        }
    }
}