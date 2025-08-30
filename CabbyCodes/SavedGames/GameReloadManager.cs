using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CabbyCodes.Patches.Settings;
using CabbyMenu.UI;

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
        
        // Reference to the current reload popup
        private static IPersistentPopup currentReloadPopup;

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
            // Don't show popup yet - it will be shown when the actual reload begins
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
            
            // If no sources remain, cancel the pending reload and hide popup
            if (reloadSources.Count == 0 && pendingReload && !reloadRequested)
            {
                pendingReload = false;
                HideReloadPopup();
            }
        }

        /// <summary>
        /// Clears all reload requests. Call this after a reload completes to reset the system.
        /// </summary>
        public static void ClearAllReloadRequests()
        {
            reloadSources.Clear();
            HideReloadPopup();
        }

        /// <summary>
        /// Shows the reload loading popup.
        /// </summary>
        private static void ShowReloadPopup()
        {
            if (currentReloadPopup == null)
            {
                currentReloadPopup = CreateReloadPopup();
                if (currentReloadPopup != null)
                {
                    currentReloadPopup.Show();
                    CabbyCodesPlugin.BLogger.LogDebug("GameReloadManager: Reload popup shown");
                }
            }
        }

        /// <summary>
        /// Hides and destroys the reload loading popup.
        /// </summary>
        private static void HideReloadPopup()
        {
            if (currentReloadPopup != null)
            {
                currentReloadPopup.Hide();
                currentReloadPopup.Destroy();
                currentReloadPopup = null;
                CabbyCodesPlugin.BLogger.LogDebug("GameReloadManager: Reload popup hidden");
            }
        }

        /// <summary>
        /// Creates a loading popup for reloads.
        /// </summary>
        /// <returns>The created loading popup, or null if creation fails.</returns>
        private static IPersistentPopup CreateReloadPopup()
        {
            try
            {
                // Create a scene-independent popup that can persist across scene changes
                var persistentGo = new GameObject("ReloadLoadingPopup");
                UnityEngine.Object.DontDestroyOnLoad(persistentGo);
                
                // Add Canvas components for UI rendering
                Canvas canvas = persistentGo.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay; // This is crucial for visibility
                canvas.overrideSorting = true;
                canvas.sortingOrder = 1000; // Ensure popup appears on top of all UI
                persistentGo.AddComponent<GraphicRaycaster>();
                
                CanvasScaler scaler = persistentGo.AddComponent<CanvasScaler>();
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.referenceResolution = new Vector2(1920, 1080); // Standard reference resolution
                
                // Create the popup content
                CreateReloadPopupContent(persistentGo);
                
                // Return a simple popup wrapper that manages the persistent GameObject
                return new PersistentPopupWrapper(persistentGo);
            }
            catch (System.Exception ex)
            {
                CabbyCodesPlugin.BLogger.LogError(string.Format("Failed to create reload popup: {0}", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Creates the visual content for the reload popup.
        /// </summary>
        /// <param name="parent">The parent GameObject to attach content to.</param>
        private static void CreateReloadPopupContent(GameObject parent)
        {
            // Create dimmed background
            GameObject background = DefaultControls.CreatePanel(new DefaultControls.Resources());
            background.name = "Popup Background";
            var backgroundImage = background.GetComponent<UnityEngine.UI.Image>();
            backgroundImage.color = new Color(0f, 0f, 0f, 0.6f);
            background.transform.SetParent(parent.transform, false);
            
            // Set background to fill the screen
            var backgroundRect = background.GetComponent<RectTransform>();
            backgroundRect.anchorMin = Vector2.zero;
            backgroundRect.anchorMax = Vector2.one;
            backgroundRect.sizeDelta = Vector2.zero;

            // Create main popup panel
            GameObject popupPanel = DefaultControls.CreatePanel(new DefaultControls.Resources());
            popupPanel.name = "Popup Panel";
            var panelImage = popupPanel.GetComponent<UnityEngine.UI.Image>();
            panelImage.color = new Color(0f, 0f, 0f, 1f);
            popupPanel.transform.SetParent(parent.transform, false);
            
            // Center the panel
            var panelRect = popupPanel.GetComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0.5f, 0.5f);
            panelRect.anchorMax = new Vector2(0.5f, 0.5f);
            panelRect.sizeDelta = new Vector2(400, 200);
            panelRect.anchoredPosition = Vector2.zero;

            // Add layout components
            VerticalLayoutGroup verticalLayout = popupPanel.AddComponent<VerticalLayoutGroup>();
            verticalLayout.padding = new RectOffset(20, 20, 20, 20);
            verticalLayout.spacing = 10;
            verticalLayout.childAlignment = TextAnchor.UpperCenter;
            verticalLayout.childControlWidth = true;
            verticalLayout.childControlHeight = true;
            verticalLayout.childForceExpandWidth = true;
            verticalLayout.childForceExpandHeight = false;

            // Create header
            GameObject headerContainer = DefaultControls.CreatePanel(new DefaultControls.Resources());
            headerContainer.name = "Header Container";
            var headerImage = headerContainer.GetComponent<UnityEngine.UI.Image>();
            headerImage.color = new Color(0.2f, 0.2f, 0.2f, 1f);
            headerContainer.transform.SetParent(popupPanel.transform, false);
            
            var headerLayout = headerContainer.AddComponent<LayoutElement>();
            headerLayout.preferredHeight = 60;
            headerLayout.minHeight = 50;

            // Header text
            GameObject headerText = new GameObject("Header Text");
            headerText.transform.SetParent(headerContainer.transform, false);
            var headerTextComponent = headerText.AddComponent<UnityEngine.UI.Text>();
            headerTextComponent.text = "Reloading Game";
            headerTextComponent.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            headerTextComponent.fontSize = 24;
            headerTextComponent.fontStyle = FontStyle.Bold;
            headerTextComponent.color = Color.white;
            headerTextComponent.alignment = TextAnchor.MiddleCenter;
            
            var headerTextRect = headerText.GetComponent<RectTransform>();
            headerTextRect.anchorMin = Vector2.zero;
            headerTextRect.anchorMax = Vector2.one;
            headerTextRect.sizeDelta = Vector2.zero;

            // Create message text
            GameObject messageText = new GameObject("Message Text");
            messageText.transform.SetParent(popupPanel.transform, false);
            var messageTextComponent = messageText.AddComponent<UnityEngine.UI.Text>();
            messageTextComponent.text = "Please Wait . . .";
            messageTextComponent.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            messageTextComponent.fontSize = 18;
            messageTextComponent.color = Color.white;
            messageTextComponent.alignment = TextAnchor.MiddleCenter;
            
            var messageLayout = messageText.AddComponent<LayoutElement>();
            messageLayout.preferredHeight = 80;
            messageLayout.minHeight = 60;
            
            var messageTextRect = messageText.GetComponent<RectTransform>();
            messageTextRect.anchorMin = Vector2.zero;
            messageTextRect.anchorMax = Vector2.one;
            messageTextRect.sizeDelta = Vector2.zero;
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
                // Show popup when the actual reload begins
                ShowReloadPopup();
                
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
                        HideReloadPopup(); // Hide popup on error
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
            
            // Don't clear reload requests or hide popup yet - wait until player is back in game
            // The popup will be hidden when the custom file load completes
        }

        private System.Collections.IEnumerator DeleteTempSaveAfterDelay(string fileName)
        {
            // Wait a few seconds to ensure the load is complete
            yield return new WaitForSeconds(5f);
            SavedGameManager.DeleteCustomSave(fileName);
        }
    }
} 