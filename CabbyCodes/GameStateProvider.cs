using CabbyMenu;
using UnityEngine;
using CabbyCodes.CheatState;

namespace CabbyCodes
{
    /// <summary>
    /// Provides game state information to the menu system.
    /// </summary>
    public class GameStateProvider : IGameStateProvider
    {
        public GameStateProvider()
        {
            // Initialize the CheatStateManager
            CheatStateManager.Initialize();
        }

        /// <summary>
        /// Gets whether the menu should be shown.
        /// </summary>
        /// <returns>True if the menu should be shown, false otherwise.</returns>
        public bool ShouldShowMenu()
        {
            // For now, show the menu when the game is paused
            return GameManager._instance != null && GameManager.instance.IsGamePaused();
        }

        /// <summary>
        /// Gets the current scene name.
        /// </summary>
        /// <returns>The current scene name.</returns>
        public static string GetCurrentSceneName()
        {
            return GameManager.GetBaseSceneName(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }
}