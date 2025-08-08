using System;
using System.Collections.Generic;
using UnityEngine;
using CabbyCodes.Patches.Settings;

namespace CabbyCodes.SavedGames
{
    /// <summary>
    /// Provides a reusable way to queue save/reload operations for when the menu closes.
    /// Supports multiple sources requesting reloads independently.
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
        
        // Track reload requests from different sources
        private static readonly HashSet<string> reloadSources = new HashSet<string>();

        /// <summary>
        /// Queues a save/reload operation for when the menu closes. Optionally delete the save after loading.
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

        /// <summary>
        /// Requests a reload from a specific source. The reload will be queued until the menu closes.
        /// </summary>
        /// <param name="sourceName">Unique identifier for the source requesting the reload.</param>
        public static void RequestReload(string sourceName)
        {
            EnsureInstance();
            reloadSources.Add(sourceName);
            
            // If this is the first request, queue the reload operation
            if (reloadSources.Count == 1 && !reloadRequested && !pendingReload)
            {
                pendingReload = true;
                pendingDeleteAfterLoad = true;
                pendingSaveName = null;
                pendingOnComplete = null;
            }
        }

        /// <summary>
        /// Cancels a reload request from a specific source. If no sources remain, the reload is cancelled.
        /// </summary>
        /// <param name="sourceName">Unique identifier for the source cancelling the reload.</param>
        public static void CancelReload(string sourceName)
        {
            reloadSources.Remove(sourceName);
            
            // If no sources remain, cancel the pending reload
            if (reloadSources.Count == 0 && pendingReload && !reloadRequested)
            {
                pendingReload = false;
            }
        }

        /// <summary>
        /// Clears all reload requests. Call this after a reload completes to reset the system.
        /// </summary>
        public static void ClearAllReloadRequests()
        {
            reloadSources.Clear();
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
            
            // Clear all reload requests after the reload completes
            ClearAllReloadRequests();
        }

        private System.Collections.IEnumerator DeleteTempSaveAfterDelay(string fileName)
        {
            // Wait a few seconds to ensure the load is complete
            yield return new WaitForSeconds(5f);
            SavedGameManager.DeleteCustomSave(fileName);
        }
    }
} 