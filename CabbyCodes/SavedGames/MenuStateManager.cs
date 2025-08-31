using System;
using UnityEngine;

namespace CabbyCodes.SavedGames
{
    /// <summary>
    /// Manages persistence and restoration of menu UI state across game reloads.
    /// Captures and restores key dropdown selections and category states.
    /// </summary>
    public class MenuStateManager : MonoBehaviour
    {
        private static MenuStateManager _instance;
        private static MenuState currentState;
        private static bool stateRestored = false;
        
        // Callback for when state restoration is complete
        private static Action onStateRestored;
        
        /// <summary>
        /// Captures the current menu state before a reload operation.
        /// </summary>
        public static void CaptureCurrentState()
        {
            EnsureInstance();
            
            if (CabbyCodesPlugin.cabbyMenu == null) return;
            
            currentState = new MenuState();
            
            // Capture main category selection
            CaptureMainCategoryState();
            
            // Capture Flags category state (most commonly used)
            CaptureFlagsState();
        }
        
        /// <summary>
        /// Restores the previously captured menu state after a reload operation.
        /// </summary>
        public static void RestoreCapturedState(Action onComplete = null)
        {
            if (currentState == null || stateRestored)
            {
                onComplete?.Invoke();
                return;
            }
            
            onStateRestored = onComplete;
            EnsureInstance();
            
            // Start restoration process
            _instance.StartCoroutine(RestoreStateCoroutine());
        }
        
        /// <summary>
        /// Clears the captured state after successful restoration.
        /// </summary>
        public static void ClearCapturedState()
        {
            currentState = null;
            stateRestored = false;
            onStateRestored = null;
        }
        
        /// <summary>
        /// Coroutine to restore state with proper timing for UI initialization.
        /// </summary>
        private static System.Collections.IEnumerator RestoreStateCoroutine()
        {
            // Wait for menu to be fully initialized
            yield return new WaitForSeconds(0.1f);
            
            // Wait for all categories to be registered
            while (CabbyCodesPlugin.cabbyMenu == null || 
                   CabbyCodesPlugin.cabbyMenu.GetRegisteredCategories() == 0)
            {
                yield return new WaitForSeconds(0.05f);
            }
            
            // Wait a bit more for the UI to be fully rendered
            yield return new WaitForSeconds(0.2f);
            
            // Restore main category first
            RestoreMainCategoryState();
            
            // Wait for main category restoration to complete
            yield return new WaitForSeconds(0.1f);
            
            // Restore sub-category states
            RestoreFlagsState();
            
            // Mark restoration as complete
            stateRestored = true;
            onStateRestored?.Invoke();
        }
        
        private static void CaptureMainCategoryState()
        {
            try
            {
                // Get the current main category selection from the menu
                if (CabbyCodesPlugin.cabbyMenu != null)
                {
                    int currentCategory = CabbyCodesPlugin.cabbyMenu.GetCurrentCategoryIndex();
                    if (currentCategory >= 0)
                    {
                        currentState.MainCategoryIndex = currentCategory;
                        
                        // Immediately update the config so it's available for the next menu build
                        var lastSelectedCategoryConfig = CabbyCodesPlugin.configFile.Bind("MainMenu", "LastSelectedCategory", 0, "");
                        lastSelectedCategoryConfig.Value = currentCategory;
                    }
                }
            }
            catch (Exception ex)
            {
                CabbyCodesPlugin.BLogger.LogWarning(string.Format("Failed to capture main category state: {0}", ex.Message));
            }
        }
        
        private static void CaptureFlagsState()
        {
            try
            {
                // Capture the current flags category selection from config
                var flagsCategoryConfig = CabbyCodesPlugin.configFile.Bind("Flags", "LastSelectedFlagType", 0, "");
                currentState.FlagsCategoryIndex = flagsCategoryConfig.Value;
                
                // Capture the current player flag page
                CapturePlayerFlagPageState();
            }
            catch (Exception ex)
            {
                CabbyCodesPlugin.BLogger.LogWarning(string.Format("Failed to capture flags state: {0}", ex.Message));
            }
        }
        
        private static void CapturePlayerFlagPageState()
        {
            try
            {
                // Get the current page from PlayerFlagPatch config
                var currentPageConfig = CabbyCodesPlugin.configFile.Bind("PlayerFlags", "CurrentPage", 0, "");
                currentState.PlayerFlagPage = currentPageConfig.Value;
            }
            catch (Exception ex)
            {
                CabbyCodesPlugin.BLogger.LogWarning(string.Format("Failed to capture player flag page state: {0}", ex.Message));
            }
        }
        
        private static void RestoreMainCategoryState()
        {
            try
            {
                if (currentState?.MainCategoryIndex.HasValue == true)
                {
                    var categoryIndex = currentState.MainCategoryIndex.Value;
                    
                    // Set the deferred restoration and reset the state so it will be applied
                    if (CabbyCodesPlugin.cabbyMenu != null)
                    {
                        if (categoryIndex >= 0 && categoryIndex < CabbyCodesPlugin.cabbyMenu.GetRegisteredCategories())
                        {
                            // Set the deferred restoration from the captured state
                            CabbyCodesPlugin.cabbyMenu.SetDeferredRestorationFromExternal(categoryIndex);
                        }
                        else
                        {
                            CabbyCodesPlugin.BLogger.LogWarning(string.Format("MenuStateManager: Invalid category index {0}, max is {1}", categoryIndex, CabbyCodesPlugin.cabbyMenu.GetRegisteredCategories() - 1));
                        }
                    }
                    else
                    {
                        CabbyCodesPlugin.BLogger.LogWarning("MenuStateManager: Cannot restore main category - menu is null");
                    }
                }
            }
            catch (Exception ex)
            {
                CabbyCodesPlugin.BLogger.LogWarning(string.Format("Failed to restore main category state: {0}", ex.Message));
            }
        }
        
        private static void RestoreFlagsState()
        {
            try
            {
                if (currentState?.FlagsCategoryIndex.HasValue == true)
                {
                    var flagsIndex = currentState.FlagsCategoryIndex.Value;
                    
                    // Restore the flags category selection
                    var flagsCategoryConfig = CabbyCodesPlugin.configFile.Bind("Flags", "LastSelectedFlagType", 0, "");
                    flagsCategoryConfig.Value = flagsIndex;
                }
                
                // Restore the player flag page
                RestorePlayerFlagPageState();
            }
            catch (Exception ex)
            {
                CabbyCodesPlugin.BLogger.LogWarning(string.Format("Failed to restore flags state: {0}", ex.Message));
            }
        }
        
        private static void RestorePlayerFlagPageState()
        {
            try
            {
                if (currentState?.PlayerFlagPage.HasValue == true)
                {
                    var pageIndex = currentState.PlayerFlagPage.Value;
                    
                    // Restore the player flag page
                    var currentPageConfig = CabbyCodesPlugin.configFile.Bind("PlayerFlags", "CurrentPage", 0, "");
                    currentPageConfig.Value = pageIndex;
                }
            }
            catch (Exception ex)
            {
                CabbyCodesPlugin.BLogger.LogWarning(string.Format("Failed to restore player flag page state: {0}", ex.Message));
            }
        }
        
        private static void EnsureInstance()
        {
            if (_instance == null)
            {
                var go = new GameObject("MenuStateManager");
                DontDestroyOnLoad(go);
                _instance = go.AddComponent<MenuStateManager>();
            }
        }
    }
}
