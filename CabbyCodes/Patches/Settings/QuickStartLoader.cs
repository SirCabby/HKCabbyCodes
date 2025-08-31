using UnityEngine;
using CabbyCodes.SavedGames;

namespace CabbyCodes.Patches.Settings
{
    /// <summary>
    /// Persistent loader for custom/quick start loads.
    /// </summary>
    public class QuickStartLoader : MonoBehaviour
    {
        private bool customLoadTriggered = false;

        void Update()
        {
            // Only run if not already triggered
            if (!customLoadTriggered && !string.IsNullOrEmpty(QuickStartPatch.CustomFileToLoad))
            {
                var gm = GameManager._instance;
                if (gm != null &&
                    gm.gameState == GlobalEnums.GameState.MAIN_MENU &&
                    gm.ui != null &&
                    gm.ui.menuState == GlobalEnums.MainMenuState.MAIN_MENU &&
                    !gm.ui.IsAnimatingMenus && !gm.ui.IsFadingMenu)
                {
                    customLoadTriggered = true;
                    string fileToLoad = QuickStartPatch.CustomFileToLoad;
                    QuickStartPatch.CustomFileToLoad = null;
                    CabbyCodesPlugin.BLogger.LogInfo(string.Format("QuickStartLoader: Loading custom file '{0}' after main menu.", fileToLoad));
                    SavedGameManager.LoadCustomGame(fileToLoad, (success) => {
                        if (success)
                        {
                            // Call OnGameLoadComplete after custom file load to restore menu state
                            GameReloadManager.OnGameLoadComplete();
                        }
                        else
                        {
                            CabbyCodesPlugin.BLogger.LogWarning(string.Format("QuickStartLoader: Failed to load custom file '{0}'.", fileToLoad));
                        }
                    });
                }
            }
            
            // Reset trigger if we leave the main menu
            if (customLoadTriggered)
            {
                var gm = GameManager._instance;
                if (gm == null || gm.gameState != GlobalEnums.GameState.MAIN_MENU)
                {
                    customLoadTriggered = false;
                }
            }
        }
    }
} 