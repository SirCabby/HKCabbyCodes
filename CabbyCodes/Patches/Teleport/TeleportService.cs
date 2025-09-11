using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using GlobalEnums;
using CabbyMenu.UI;
using CabbyMenu.UI.Popups;

namespace CabbyCodes.Patches.Teleport
{
    /// <summary>
    /// Service class that handles shared teleportation functionality.
    /// </summary>
    public static class TeleportService
    {
        /// <summary>
        /// Field info for accessing the hero field in GameMap.
        /// </summary>
        private static readonly FieldInfo heroFieldInfo = typeof(GameMap).GetField("hero", BindingFlags.NonPublic | BindingFlags.Instance);

        /// <summary>
        /// Flag to prevent multiple teleport attempts while one is in progress.
        /// </summary>
        private static bool teleportInProgress = false;

        /// <summary>
        /// Performs teleportation to the specified location.
        /// </summary>
        /// <param name="teleportLocation">The location to teleport to.</param>
        public static void DoTeleport(TeleportLocation teleportLocation)
        {
            if (teleportInProgress)
            {
                CabbyCodesPlugin.BLogger.LogWarning("Teleport already in progress, ignoring request");
                return;
            }

            CabbyCodesPlugin.BLogger.LogInfo(string.Format("Teleporting to [{0}] ({1}, {2})", teleportLocation.Scene.SceneName, teleportLocation.Location.x, teleportLocation.Location.y));

            GameManager gm = GameManager._instance;
            if (gm == null)
            {
                CabbyCodesPlugin.BLogger.LogError("GameManager is null, cannot teleport");
                return;
            }

            // Unpause the game by setting all pause state fields before unpausing the hero
            var hero = gm.hero_ctrl;
            if (hero != null)
            {
                // Set all pause state fields to unpaused state
                gm.isPaused = false;
                Time.timeScale = 1f;
                TimeController.GenericTimeScale = 1f;
                
                // Restore input handling
                gm.inputHandler.StartAcceptingInput();
                gm.inputHandler.AllowPause();
                
                // Set game state to playing
                gm.SetState(GameState.PLAYING);
                
                // Update UI state to match game state
                gm.ui?.SetState(UIState.PLAYING);
                
                // Now unpause the hero
                hero.UnPause();
            }

            // Store the target location for use after the scene loads
            _pendingTeleportLocation = teleportLocation;
            teleportInProgress = true;

            // Start the coroutine that waits for the hero to be ready before teleporting
            gm.StartCoroutine(WaitForHeroReadyAndTeleport(teleportLocation));
        }

        /// <summary>
        /// Coroutine that waits for the hero to be fully initialized before starting the teleport.
        /// </summary>
        /// <param name="teleportLocation">The location to teleport to.</param>
        private static IEnumerator WaitForHeroReadyAndTeleport(TeleportLocation teleportLocation)
        {
            // Show a persistent loading popup while we prepare the teleport
            if (loadingPopup == null)
            {
                loadingPopup = new PopupBase("Teleport", "Preparing teleport . . .", 600f, 300f, showHeader: true, autoResize: true);
                loadingPopup.Show();
                // Remove from PopupBase tracking so it isn't hidden when menu closes
                try
                {
                    var listField = typeof(PopupBase).GetField("openPopups", BindingFlags.NonPublic | BindingFlags.Static);
                    var list = listField?.GetValue(null) as IList;
                    list?.Remove(loadingPopup);
                }
                catch {}

                // If hero is currently seated at a bench, update popup message to match custom save load style
                try
                {
                    if (PlayerData.instance != null && PlayerData.instance.atBench && loadingPopup is PopupBase benchPopup)
                    {
                        benchPopup.SetMessageText("Get off bench to restore position");
                    }
                }
                catch {}

                RaisePopupSortingOrder(loadingPopup, 5000);
                popupStartTime = Time.realtimeSinceStartup;
                try
                {
                    var rootField = typeof(PopupBase).GetField("popupRoot", BindingFlags.NonPublic | BindingFlags.Instance);
                    if (rootField != null)
                    {
                        var rootObj = rootField.GetValue(loadingPopup) as GameObject;
                        var canvas = rootObj?.GetComponent<Canvas>();
                        if (canvas != null)
                        {
                            canvas.sortingOrder = 5000;
                        }
                    }
                }
                catch { }
            }
            
            GameManager gm = GameManager._instance;
            if (gm == null)
            {
                // Clean up the loading popup if we cannot proceed
                loadingPopup?.Destroy();
                if (loadingPopup != null && popupStartTime >= 0f)
                {
                    float openTime = Time.realtimeSinceStartup - popupStartTime;
                    CabbyCodesPlugin.BLogger.LogInfo(string.Format("Teleport loading popup destroyed after {0:F2} seconds", openTime));
                }
                loadingPopup = null;
                teleportInProgress = false;
                yield break;
            }

            // Wait for the hero to be available
            HeroController hero = null;
            while (hero == null)
            {
                hero = gm.hero_ctrl;
                if (hero == null)
                {
                    yield return null;
                }
            }

            // Wait for the hero to be fully initialized and ready
            while (true)
            {
                // Check if the hero is ready for teleportation
                bool heroReady = IsHeroReadyForTeleport(hero, gm);
                if (heroReady)
                {
                    break;
                }
                yield return null;
            }

            // Subscribe to the event that fires after the hero is spawned in the new scene
            gm.OnFinishedEnteringScene += OnFinishedEnteringSceneTeleport;

            // Start the scene transition
            gm.BeginSceneTransition(new GameManager.SceneLoadInfo
            {
                AlwaysUnloadUnusedAssets = true,
                EntryGateName = "dreamGate", // Use a valid entry gate or empty if not needed
                PreventCameraFadeOut = true,
                SceneName = teleportLocation.Scene.SceneName,
                Visualization = GameManager.SceneLoadVisualizations.Dream
            });

            // Hero is ready; remove the loading popup before starting the scene transition
            // Keep the loading popup visible until the scene has finished loading (destroy later)
        }

        /// <summary>
        /// Checks if the hero is ready for teleportation by examining various state conditions.
        /// </summary>
        /// <param name="hero">The hero controller to check.</param>
        /// <param name="gm">The game manager instance.</param>
        /// <returns>True if the hero is ready for teleportation, false otherwise.</returns>
        private static bool IsHeroReadyForTeleport(HeroController hero, GameManager gm)
        {
            if (hero == null || gm == null)
            {
                return false;
            }

            bool isGamePlaying = gm.gameState == GameState.PLAYING;
            bool acceptingInput = gm.inputHandler.acceptingInput;
            var heroState = hero.hero_state;
            bool onGround = false;
            bool atBench = false;
            try { onGround = hero.cState.onGround; } catch { }
            try { atBench = PlayerData.instance != null && PlayerData.instance.atBench; } catch { }

            if (!isGamePlaying) return false;
            if (!acceptingInput) return false;
            if (heroState == ActorStates.no_input || heroState == ActorStates.airborne) return false;
            if (atBench) return false;
            if (!onGround) return false;
            if (hero.transform == null) return false;

            return true;
        }

        // Store the pending teleport location
        private static TeleportLocation _pendingTeleportLocation;
        
        // Reference to the loading popup shown while waiting for the hero to be ready
        private static IPersistentPopup loadingPopup;
        
        // Track popup open time
        private static float popupStartTime = -1f;
        
        // Event handler to set the hero's position after entering the scene
        private static void OnFinishedEnteringSceneTeleport()
        {
            var gm = GameManager._instance;
            var hero = gm.hero_ctrl;
            
            if (_pendingTeleportLocation != null && hero != null)
            {
                Vector3 newPos = new Vector3(_pendingTeleportLocation.Location.x, _pendingTeleportLocation.Location.y, hero.transform.position.z);
                hero.transform.position = newPos;
                gm.cameraCtrl.SnapTo(newPos.x, newPos.y);
                
                // Restore the game state to fully playable
                gm.SetState(GameState.PLAYING);
                gm.inputHandler.StartAcceptingInput();
                gm.inputHandler.AllowPause();
                
                // Actually unpause the game
                gm.isPaused = false;
                Time.timeScale = 1f;
                TimeController.GenericTimeScale = 1f;
                
                gm.actorSnapshotUnpaused?.TransitionTo(0f);
                gm.ui?.SetState(UIState.PLAYING);

                // Destroy the loading popup now that teleport is complete
                loadingPopup?.Destroy();
                if (loadingPopup != null && popupStartTime >= 0f)
                {
                    float openTime = Time.realtimeSinceStartup - popupStartTime;
                }
                loadingPopup = null;
            }
            // Unsubscribe to avoid memory leaks
            gm.OnFinishedEnteringScene -= OnFinishedEnteringSceneTeleport;
            _pendingTeleportLocation = null;
            teleportInProgress = false;
        }

        /// <summary>
        /// Gets the current player position for saving teleport locations.
        /// </summary>
        /// <returns>A tuple containing the scene name and position.</returns>
        public static (string sceneName, Vector2 position) GetCurrentPlayerPosition()
        {
            GameMap gm = GameManager._instance.gameMap.GetComponent<GameMap>();
            Vector3 heroPos = ((GameObject)heroFieldInfo.GetValue(gm)).transform.position;
            string sceneName = GameStateProvider.GetCurrentSceneName();
            Vector2 teleportLocation = new Vector2((int)Math.Round(heroPos.x), (int)Math.Ceiling(heroPos.y));
            
            return (sceneName, teleportLocation);
        }

        /// <summary>
        /// Shows a confirmation dialog before teleporting to the specified location.
        /// </summary>
        /// <param name="teleportLocation">The location to teleport to.</param>
        /// <param name="destinationName">A human-readable name for the destination.</param>
        /// <param name="onConfirmed">Optional callback to execute after confirmation.</param>
        public static void DoTeleportWithConfirmation(TeleportLocation teleportLocation, string destinationName, Action onConfirmed = null)
        {
            var menu = CabbyCodesPlugin.cabbyMenu;
            if (menu == null) 
            { 
                DoTeleport(teleportLocation);
                onConfirmed?.Invoke();
                return; 
            }
            
            string msg = string.Format("Teleport to '{0}'?", destinationName);
            var popup = new ConfirmationPopup(
                menu,
                "Are you sure?",
                msg,
                "Teleport",
                "Cancel",
                () => { 
                    DoTeleport(teleportLocation);
                    onConfirmed?.Invoke();
                },
                null);
            popup.Show();
        }

        /// <summary>
        /// Shows a generic confirmation dialog for any action.
        /// </summary>
        /// <param name="title">The title of the confirmation dialog.</param>
        /// <param name="message">The message to display.</param>
        /// <param name="confirmText">Text for the confirm button.</param>
        /// <param name="cancelText">Text for the cancel button.</param>
        /// <param name="onConfirmed">Action to execute when confirmed.</param>
        /// <param name="onCancelled">Optional action to execute when cancelled.</param>
        public static void ShowConfirmationDialog(string title, string message, string confirmText, string cancelText, Action onConfirmed, Action onCancelled = null)
        {
            var menu = CabbyCodesPlugin.cabbyMenu;
            if (menu == null) 
            { 
                onConfirmed?.Invoke();
                return; 
            }
            
            var popup = new ConfirmationPopup(
                menu,
                title,
                message,
                confirmText,
                cancelText,
                onConfirmed,
                onCancelled);
            popup.Show();
        }

        /// <summary>
        /// Creates a loading popup with consistent styling
        /// </summary>
        /// <param name="menu">The menu to attach the popup to</param>
        /// <returns>A configured loading popup</returns>
        public static PopupBase CreateLoadingPopup(CabbyMainMenu menu)
        {
            if (menu == null) return null;
            
            var popup = new PopupBase(menu, "Loading", "Loading . . .", 800f, 100f, false);
            popup.SetPanelBackgroundColor(new Color(0.2f, 0.4f, 0.8f, 1f)); // Blue background
            popup.SetMessageBold(); // Make message text bold
            return popup;
        }

        /// <summary>
        /// Shows a loading popup and returns it for later hiding/destroying
        /// </summary>
        /// <param name="menu">The menu to attach the popup to</param>
        /// <returns>The created and shown loading popup</returns>
        public static PopupBase ShowLoadingPopup(CabbyMainMenu menu)
        {
            var popup = CreateLoadingPopup(menu);
            if (popup != null)
            {
                popup.Show();
                Canvas.ForceUpdateCanvases();
            }
            return popup;
        }

        /// <summary>
        /// Raises the canvas sorting order of a PopupBase to ensure visibility above other UI.
        /// </summary>
        /// <param name="popup">PopupBase instance.</param>
        /// <param name="order">Desired sorting order.</param>
        private static void RaisePopupSortingOrder(IPersistentPopup popup, int order)
        {
            if (popup == null) return;
            try
            {
                var rootField = typeof(PopupBase).GetField("popupRoot", BindingFlags.NonPublic | BindingFlags.Instance);
                GameObject rootObj = rootField?.GetValue(popup) as GameObject;
                Canvas canvas = rootObj != null ? rootObj.GetComponent<Canvas>() : null;
                if (canvas != null)
                {
                    canvas.sortingOrder = order;
                }
            }
            catch { }
        }
    }
} 