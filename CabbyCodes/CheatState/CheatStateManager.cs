using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

namespace CabbyCodes.CheatState
{
    /// <summary>
    /// Manages cheat states across GameManager lifecycle changes and ensures proper state restoration.
    /// </summary>
    public static class CheatStateManager
    {
        private static readonly Dictionary<string, bool> activeCheatStates = new Dictionary<string, bool>();
        private static readonly HashSet<ICheatStateRestorable> restorableCheats = new HashSet<ICheatStateRestorable>();
        
        private static bool gameManagerEventHandlerRegistered = false;
        private static GameManager lastGameManagerInstance = null;
        private static bool sceneEventHandlerRegistered = false;
        private static bool isInitialized = false;

        /// <summary>
        /// Initializes the CheatStateManager if not already initialized.
        /// </summary>
        public static void Initialize()
        {
            if (!isInitialized)
            {
                isInitialized = true;
                // Subscribe to scene change events to monitor GameManager changes
                RegisterSceneEventHandler();
                // Also check current GameManager state immediately
                CheckGameManagerInstance();
            }
        }

        public static void RegisterCheatState(string cheatKey, bool isActive)
        {
            activeCheatStates[cheatKey] = isActive;
        }

        public static bool GetCheatState(string cheatKey)
        {
            return activeCheatStates.TryGetValue(cheatKey, out bool state) && state;
        }

        public static void RegisterRestorableCheat(ICheatStateRestorable cheat)
        {
            if (cheat != null)
            {
                Initialize(); // Ensure we're monitoring GameManager changes
                restorableCheats.Add(cheat);
            }
        }

        public static void UnregisterRestorableCheat(ICheatStateRestorable cheat)
        {
            if (cheat != null)
            {
                restorableCheats.Remove(cheat);
            }
        }

        private static void RegisterSceneEventHandler()
        {
            if (!sceneEventHandlerRegistered)
            {
                UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnSceneChanged;
                sceneEventHandlerRegistered = true;
            }
        }

        private static void OnSceneChanged(Scene oldScene, Scene newScene)
        {
            // Scene changes can cause GameManager instance changes, so check
            CheckGameManagerInstance();
        }

        private static void CheckGameManagerInstance()
        {
            // Check if GameManager instance has changed (can happen during game restarts/loads)
            var currentInstance = GameManager._instance;
            if (currentInstance != lastGameManagerInstance)
            {
                // Instance changed - unregister from old instance if needed
                if (lastGameManagerInstance != null && gameManagerEventHandlerRegistered)
                {
                    lastGameManagerInstance.OnFinishedEnteringScene -= OnFinishedEnteringScene;
                    gameManagerEventHandlerRegistered = false;
                }
                
                lastGameManagerInstance = currentInstance;
                
                // Register with new instance if it exists
                if (currentInstance != null)
                {
                    RegisterGameManagerEventHandler();
                }
            }
        }

        private static void RegisterGameManagerEventHandler()
        {
            if (GameManager._instance != null && !gameManagerEventHandlerRegistered)
            {
                GameManager._instance.OnFinishedEnteringScene += OnFinishedEnteringScene;
                gameManagerEventHandlerRegistered = true;
            }
        }

        private static void OnFinishedEnteringScene()
        {
            // Restore all active cheat states after scene transition
            RestoreAllCheatStates();
        }

        public static void RestoreAllCheatStates()
        {
            // Create a copy of the HashSet to avoid modification during iteration
            var cheatsToRestore = new HashSet<ICheatStateRestorable>(restorableCheats);
            
            foreach (var cheat in cheatsToRestore)
            {
                try
                {
                    // Check if the cheat object is still valid (not destroyed)
                    if (cheat == null)
                    {
                        // Remove null references from the collection
                        restorableCheats.Remove(cheat);
                        continue;
                    }
                    
                    string cheatKey = cheat.GetCheatKey();
                    if (GetCheatState(cheatKey))
                    {
                        cheat.RestoreState();
                    }
                }
                catch (Exception ex)
                {
                    CabbyCodesPlugin.BLogger.LogError(string.Format("Failed to restore cheat state for {0}: {1}", cheat?.GetType().Name ?? "null", ex.Message));
                    
                    // Remove the problematic cheat to prevent future errors
                    if (cheat != null)
                    {
                        restorableCheats.Remove(cheat);
                    }
                }
            }
        }

        public static void ClearAllCheatStates()
        {
            activeCheatStates.Clear();
            restorableCheats.Clear();
        }
    }
} 