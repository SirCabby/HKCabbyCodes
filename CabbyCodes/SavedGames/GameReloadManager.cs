using System;
using UnityEngine;
using CabbyCodes.Patches.Settings;

namespace CabbyCodes.SavedGames
{
    /// <summary>
    /// Provides a reusable way to save, quit to main menu, and reload a save file.
    /// </summary>
    public class GameReloadManager : MonoBehaviour
    {
        private static GameReloadManager _instance;
        private static string pendingSaveFileName;
        private static bool deleteAfterLoad;
        private static Action<bool> onComplete;
        private static bool reloadRequested;
        private static bool waitingForUnpause;
        private static bool pendingReload;
        private static bool pendingDeleteAfterLoad;
        private static string pendingSaveName;
        private static Action<bool> pendingOnComplete;

        /// <summary>
        /// Call this to save, quit to main menu, and reload the save. Optionally delete the save after loading.
        /// </summary>
        public static void SaveAndReload(bool deleteAfterLoad = true, string saveName = null, Action<bool> onComplete = null)
        {
            EnsureInstance();
            if (reloadRequested || pendingReload) return; // Prevent duplicate requests
            pendingReload = true;
            pendingDeleteAfterLoad = deleteAfterLoad;
            pendingSaveName = saveName;
            pendingOnComplete = onComplete;
        }

        private static void EnsureInstance()
        {
            if (_instance == null)
            {
                var go = new GameObject("GameReloadManager");
                DontDestroyOnLoad(go);
                _instance = go.AddComponent<GameReloadManager>();
            }
        }

        private void Update()
        {
            // If a reload is pending and the game is unpaused, trigger the save/quit/reload
            if (pendingReload && GameManager.instance != null && !GameManager.instance.IsGamePaused())
            {
                reloadRequested = true;
                pendingReload = false;
                string tempName = pendingSaveName ?? $"temp_room_reload_{Guid.NewGuid()}";
                deleteAfterLoad = pendingDeleteAfterLoad;
                onComplete = pendingOnComplete;
                SavedGameManager.SaveCustomGame(tempName, success =>
                {
                    if (success)
                    {
                        pendingSaveFileName = SavedGameManager.GenerateSaveFileName(tempName);
                        waitingForUnpause = false;
                        // Now quit to main menu
                        GameManager.instance.StartCoroutine(GameManager.instance.ReturnToMainMenu(GameManager.ReturnToMainMenuSaveModes.DontSave, OnReturnedToMainMenu));
                    }
                    else
                    {
                        reloadRequested = false;
                        onComplete?.Invoke(false);
                    }
                });
            }

            if (!reloadRequested) return;

            if (waitingForUnpause)
            {
                // Wait for the user to unpause
                if (GameManager.instance != null && !GameManager.instance.IsGamePaused())
                {
                    waitingForUnpause = false;
                    // Now quit to main menu
                    GameManager.instance.StartCoroutine(GameManager.instance.ReturnToMainMenu(GameManager.ReturnToMainMenuSaveModes.DontSave, OnReturnedToMainMenu));
                }
            }
        }

        private void OnReturnedToMainMenu(bool _)
        {
            QuickStartPatch.CustomFileToLoad = pendingSaveFileName;
            if (deleteAfterLoad)
            {
                _instance.StartCoroutine(DeleteTempSaveAfterDelay(pendingSaveFileName));
            }
            reloadRequested = false;
            onComplete?.Invoke(true);
        }

        private System.Collections.IEnumerator DeleteTempSaveAfterDelay(string fileName)
        {
            // Wait a few seconds to ensure the load is complete
            yield return new WaitForSeconds(5f);
            SavedGameManager.DeleteCustomSave(fileName);
        }
    }
} 