using CabbyMenu;
using UnityEngine;

namespace CabbyCodes
{
    /// <summary>
    /// Provides game state information to the menu system.
    /// </summary>
    public class GameStateProvider : IGameStateProvider
    {
        private static CheatStateManager cheatStateManager;

        public GameStateProvider()
        {
            // Create the cheat state manager if it doesn't exist
            if (cheatStateManager == null)
            {
                var go = new GameObject("CheatStateManager");
                Object.DontDestroyOnLoad(go);
                cheatStateManager = go.AddComponent<CheatStateManager>();
            }
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

        public static CheatStateManager GetCheatStateManager()
        {
            return cheatStateManager;
        }
    }
}