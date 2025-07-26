using UnityEngine;
using System.Collections.Generic;
using System;

namespace CabbyCodes
{
    /// <summary>
    /// Manages cheat states across GameManager lifecycle changes and ensures proper state restoration.
    /// </summary>
    public class CheatStateManager : MonoBehaviour
    {
        private static readonly Dictionary<string, bool> activeCheatStates = new Dictionary<string, bool>();
        private static readonly List<ICheatStateRestorable> restorableCheats = new List<ICheatStateRestorable>();
        private bool gameManagerEventHandlerRegistered = false;

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
            if (!restorableCheats.Contains(cheat))
            {
                restorableCheats.Add(cheat);
            }
        }

        public static void UnregisterRestorableCheat(ICheatStateRestorable cheat)
        {
            restorableCheats.Remove(cheat);
        }

        private void Update()
        {
            // Monitor for GameManager instance changes and register event handlers
            if (!gameManagerEventHandlerRegistered && GameManager._instance != null)
            {
                RegisterGameManagerEventHandler();
            }
        }

        private void RegisterGameManagerEventHandler()
        {
            if (GameManager._instance != null)
            {
                GameManager._instance.OnFinishedEnteringScene += OnFinishedEnteringScene;
                gameManagerEventHandlerRegistered = true;
            }
        }

        private void OnFinishedEnteringScene()
        {
            // Restore all active cheat states after scene transition
            RestoreAllCheatStates();
        }

        public static void RestoreAllCheatStates()
        {
            foreach (var cheat in restorableCheats)
            {
                try
                {
                    string cheatKey = cheat.GetCheatKey();
                    if (GetCheatState(cheatKey))
                    {
                        cheat.RestoreState();
                    }
                }
                catch (Exception ex)
                {
                    CabbyCodesPlugin.BLogger.LogError(string.Format("Failed to restore cheat state for {0}: {1}", cheat.GetType().Name, ex.Message));
                }
            }
        }

        public static void ClearAllCheatStates()
        {
            activeCheatStates.Clear();
        }
    }
} 