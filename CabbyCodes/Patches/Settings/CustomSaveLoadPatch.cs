using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using System.Collections.Generic;
using UnityEngine;
using CabbyMenu.Utilities;
using GlobalEnums;
using CabbyCodes.SavedGames;
using CabbyMenu.UI.Controls.InputField;
using CabbyMenu.UI;

namespace CabbyCodes.Patches.Settings
{
    /// <summary>
    /// Handles custom save/load functionality with user-named saves in a CabbySaves subfolder.
    /// </summary>
    public class CustomSaveLoadPatch
    {
        /// <summary>
        /// Flag to track if patches have been applied.
        /// </summary>
        private static bool patchesApplied = false;

        /// <summary>
        /// Flag to prevent double-saving when the Save button triggers multiple save attempts.
        /// </summary>
        private static bool isSaving = false;

        /// <summary>
        /// Flag to track if Enter key was recently pressed for legitimate input field submission.
        /// This gets set when Enter is detected and reset after a short delay.
        /// </summary>
        private static bool enterKeyRecentlyPressed = false;

        /// <summary>
        /// Time when Enter key was last pressed, used to reset the flag after a delay.
        /// </summary>
        private static float enterKeyPressTime = -1f;

        /// <summary>
        /// Duration to keep the Enter key flag active after pressing Enter.
        /// </summary>
        private static readonly float ENTER_KEY_FLAG_DURATION = 0.1f; // 100ms should be plenty

        /// <summary>
        /// Creates and returns the custom save name input field panel (for external use).
        /// </summary>
        /// <returns>The created InputFieldPanel for custom save names.</returns>
        public static InputFieldPanel<string> CreateCustomSaveNamePanel()
        {
            // Calculate width for 35 characters but allow 50 characters
            int widthFor35Chars = CalculatePanelWidth(35);
            
            // Create custom input field sync that triggers save on Enter, but with logic to prevent auto-saving when clicking away
            var customInputFieldSync = new StringInputFieldSync(
                customSaveNameRef,
                KeyCodeMap.ValidChars.AlphaNumericWithSpaces,
                new Vector2(widthFor35Chars, CabbyMenu.Constants.DEFAULT_PANEL_HEIGHT),
                60,
                () => AttemptSaveCustomGame());
            
            var panel = new InputFieldPanel<string>(customInputFieldSync, "(Optional) Save game name");
            customSaveNameInputPanel = panel;
            return panel;
        }

        /// <summary>
        /// Creates the Save button panel wired to the internal SaveCustomGame logic.
        /// </summary>
        public static ButtonPanel CreateSaveButtonPanel()
        {
            return new ButtonPanel(() => AttemptSaveCustomGame(true), "Save", "Save game at current location");
        }

        /// <summary>
        /// Builds panels for each existing custom save file without attaching them.
        /// SettingsPatch injects them into the menu at the correct spot.
        /// </summary>
        /// <param name="onListChanged">Callback invoked when a save is deleted so the caller can rebuild the UI.</param>
        public static List<CheatPanel> BuildSavePanels(System.Action onListChanged)
        {
            var panels = new List<CheatPanel>();

            foreach (string saveFileName in SavedGameManager.GetCustomSaveFiles())
            {
                string displayName = SavedGameManager.GetDisplayNameFromFileName(saveFileName);

                var buttonPanel = new ButtonPanel(() =>
                {
                    var menu = CabbyCodesPlugin.cabbyMenu;
                    if (menu == null)
                    {
                        LoadCustomSave(saveFileName);
                        return;
                    }

                    string msg = $"Load the save '{displayName}'?\n\nYou will lose any unsaved progress.";
                    var popup = new CabbyMenu.UI.Popups.ConfirmationPopup(
                        menu,
                        "Are you sure?",
                        msg,
                        "Load",
                        "Cancel",
                        () => { LoadCustomSave(saveFileName); },
                        null);
                    popup.Show();
                }, "Load", displayName);

                // Add destroy button with confirmation popup
                PanelAdder.AddDestroyButtonToPanel(buttonPanel, () =>
                {
                    // actual deletion logic after panel removal
                    if (SavedGameManager.DeleteCustomSave(saveFileName))
                    {
                        SaveGameAnalysisPatch.RefreshFileList();
                        onListChanged?.Invoke();
                    }
                }, "X", 60f, (confirm, cancel) =>
                {
                    var menu = CabbyCodesPlugin.cabbyMenu;
                    if (menu == null) { confirm(); return; }
                    string msg = $"Delete the save '{displayName}'?\n\n This action cannot be undone.";
                    var popup = new CabbyMenu.UI.Popups.ConfirmationPopup(
                        menu,
                        "Are you sure?",
                        msg,
                        "Delete",
                        "Keep",
                        confirm,
                        cancel);
                    popup.Show();
                });

                // Add inline SAVE button between Load button and description label
                PanelAdder.AddButton(buttonPanel, 1, () =>
                {
                    // Save (overwrite) logic for this slot
                    string customName = displayName; // use display name as save name
                    string fileName = SavedGameManager.GenerateSaveFileName(customName);
                    string filePath = System.IO.Path.Combine(SavedGameManager.GetCabbySavesDirectory(), fileName);

                    if (System.IO.File.Exists(filePath))
                    {
                        var menu = CabbyCodesPlugin.cabbyMenu;
                        if (menu != null)
                        {
                            string msg = $"Overwrite the save '{displayName}'?\n\nThis action cannot be undone.";
                            var popup = new CabbyMenu.UI.Popups.ConfirmationPopup(
                                menu,
                                "Overwrite Save File?",
                                msg,
                                "Overwrite",
                                "Cancel",
                                () => { SavedGameManager.SaveCustomGame(customName); },
                                null);
                            popup.Show();
                        }
                        else
                        {
                            SavedGameManager.SaveCustomGame(customName);
                        }
                    }
                    else
                    {
                        // No file exists - just save
                        SavedGameManager.SaveCustomGame(customName);
                    }
                }, "Save", new Vector2(CabbyMenu.Constants.MIN_PANEL_WIDTH, CabbyMenu.Constants.DEFAULT_PANEL_HEIGHT));

                panels.Add(buttonPanel);
            }

            return panels;
        }

        /// <summary>
        /// Calculates the panel width based on character limit, matching the logic from InputFieldPanel.
        /// </summary>
        /// <param name="characterLimit">The maximum number of characters allowed.</param>
        /// <returns>The calculated width in pixels.</returns>
        private static int CalculatePanelWidth(int characterLimit)
        {
            // Use the cursor character width for panel sizing to match visible character logic
            float estimatedCharWidth = CalculateCursorCharacterWidth(CabbyMenu.Constants.DEFAULT_FONT_SIZE);
            float uiBuffer = 10f;
            float calculatedWidth = (characterLimit * estimatedCharWidth) + uiBuffer;
            return Mathf.Max(CabbyMenu.Constants.MIN_PANEL_WIDTH, Mathf.RoundToInt(calculatedWidth));
        }

        /// <summary>
        /// Calculates the character width for cursor positioning calculations.
        /// </summary>
        /// <param name="fontSize">The font size in pixels.</param>
        /// <returns>The estimated character width for cursor positioning.</returns>
        private static float CalculateCursorCharacterWidth(int fontSize)
        {
            return fontSize * 0.65f;
        }

        /// <summary>
        /// Saves the current game state as a custom save.
        /// </summary>
        private static void SaveCustomGame(string customNameParam)
        {
            // flag already set by caller
            // customNameParam already captured, but guard nulls
            string customName = (customNameParam ?? string.Empty).Trim();
            
            SavedGameManager.SaveCustomGame(customName, (success) =>
            {
                if (success)
                {
                    CabbyCodesPlugin.BLogger.LogDebug(string.Format("Game saved successfully: {0}", customName));
                    
                    // Clear the custom name input field after saving
                    customSaveNameRef.Set("");
                    
                    // Force an update to ensure the UI reflects the cleared value
                    customSaveNameInputPanel?.ForceUpdate();
                    
                    SaveGameAnalysisPatch.RefreshFileList();
                    SettingsPatch.RefreshCustomSavePanels();
                }
                else
                {
                    CabbyCodesPlugin.BLogger.LogWarning(string.Format("Failed to save game: {0}", customName));
                }

                // Reset saving flag once callback completes
                isSaving = false;
            });
        }

        /// <summary>
        /// Loads a custom save file.
        /// </summary>
        /// <param name="saveFileName">The name of the save file to load.</param>
        private static void LoadCustomSave(string saveFileName)
        {
            // Capture menu state before custom save load
            MenuStateManager.CaptureCurrentState();
            
            // Cache the current slot before leaving (in case it changed)
            if (GameManager.instance != null)
            {
                LastLoadedSlot = GameManager.instance.profileID;
            }

            // Show loading popup immediately when user clicks load
            var loadingPopup = CreateLoadingPopup();
            if (loadingPopup != null)
            {
                loadingPopup.Show();
                
                // Store the popup reference so it can be accessed later
                currentLoadingPopup = loadingPopup;
            }
            else
            {
                CabbyCodesPlugin.BLogger.LogWarning("LoadCustomSave: Failed to create loading popup");
            }

            // Instead of loading immediately, set up QuickStartPatch to load after menu
            QuickStartPatch.CustomFileToLoad = saveFileName;

            // Start coroutine to return to main menu
            GameManager.instance?.StartCoroutine(GameManager.instance.ReturnToMainMenu(
                    GameManager.ReturnToMainMenuSaveModes.DontSave, null));
        }

        /// <summary>
        /// Creates a loading popup with "Loading Game" header and "Please Wait . . ." message.
        /// </summary>
        /// <returns>The created loading popup, or null if no menu is available.</returns>
        private static IPersistentPopup CreateLoadingPopup()
        {
            // Create a scene-independent popup that can persist across scene changes
            var popup = CreatePersistentLoadingPopup();
            return popup;
        }

        /// <summary>
        /// Creates a persistent loading popup that can survive scene changes.
        /// </summary>
        /// <returns>The created persistent popup.</returns>
        private static IPersistentPopup CreatePersistentLoadingPopup()
        {
            // Leverage PopupBase with no CabbyMainMenu to create a persistent stand-alone popup
            var popup = new CabbyMenu.UI.Popups.PopupBase(
                " Loading Game",
                "Please Wait . . .",
                500f,
                400f,
                showHeader: true,
                autoResize: false);

            // Ensure it starts hidden until we show it explicitly
            popup.Hide();

            return popup;
        }

        /// <summary>
        /// Gets the current loading popup instance.
        /// </summary>
        /// <returns>The current loading popup, or null if none exists.</returns>
        public static IPersistentPopup GetCurrentLoadingPopup()
        {
            return currentLoadingPopup;
        }

        /// <summary>
        /// Clears the current loading popup reference.
        /// </summary>
        public static void ClearCurrentLoadingPopup()
        {
            currentLoadingPopup = null;
        }

        /// <summary>
        /// Event handler to set the hero's position after entering the scene during custom save load.
        /// </summary>
        /// <param name="targetPosition">The target position to place the hero.</param>
        private static void OnFinishedEnteringSceneLoad(Vector2 targetPosition)
        {
            var gm = GameManager._instance;
            var hero = gm.hero_ctrl;
            
            if (hero != null)
            {
                Vector3 newPos = new Vector3(targetPosition.x, targetPosition.y, hero.transform.position.z);
                hero.transform.position = newPos;
                gm.cameraCtrl.SnapTo(newPos.x, newPos.y);
                
                // Restore the game state to fully playable
                gm.SetState(GameState.PLAYING);
                gm.inputHandler.StartAcceptingInput();
                gm.inputHandler.AllowPause();
                
                // Unpause the game
                gm.isPaused = false;
                Time.timeScale = 1f;
                TimeController.GenericTimeScale = 1f;
                
                // Transition audio snapshot
                gm.actorSnapshotUnpaused?.TransitionTo(0f);
                
                // Update UI state to match game state
                gm.ui?.SetState(UIState.PLAYING);
                
                CabbyCodesPlugin.BLogger.LogDebug(string.Format("Restored hero position to ({0}, {1}) after custom save load", targetPosition.x, targetPosition.y));
            }
            
            // Unsubscribe to avoid memory leaks
            gm.OnFinishedEnteringScene -= () => OnFinishedEnteringSceneLoad(targetPosition);
            
            // Restore menu state after the scene has finished loading
            GameReloadManager.OnGameLoadComplete();
        }

        /// <summary>
        /// Checks for Enter key presses and updates the flag accordingly.
        /// This should be called every frame to maintain accurate Enter key state.
        /// </summary>
        public static void UpdateEnterKeyState()
        {
            // Check if Enter key was just pressed
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                enterKeyRecentlyPressed = true;
                enterKeyPressTime = Time.realtimeSinceStartup;
            }
            
            // Reset the flag after the duration expires
            if (enterKeyRecentlyPressed && Time.realtimeSinceStartup - enterKeyPressTime > ENTER_KEY_FLAG_DURATION)
            {
                enterKeyRecentlyPressed = false;
            }
        }

        /// <summary>
        /// Attempts to save the game, prompting for overwrite if file exists.
        /// </summary>
        /// <param name="fromButtonClick">True if called from Save button click, false if from input field submission.</param>
        private static void AttemptSaveCustomGame(bool fromButtonClick = false)
        {
            // Only save if this is a legitimate submit action (Enter key) or from button click
            if (!fromButtonClick && !enterKeyRecentlyPressed)
            {
                return;
            }

            // Debounce duplicate calls that can occur when input field submits then button click fires
            if (Time.realtimeSinceStartup - lastSaveAttemptTime < 0.2f)
            {
                return;
            }
            lastSaveAttemptTime = Time.realtimeSinceStartup;

            if (isSaving) return; // Another save already in progress
            // Capture the latest text directly from the UI to avoid stale reference value
            string customName = GetCurrentInputFieldText().Trim();
            
            // Immediately mark saving in progress to block any other events until callback finishes
            isSaving = true;

            string fileName = SavedGameManager.GenerateSaveFileName(customName);
            string filePath = System.IO.Path.Combine(SavedGameManager.GetCabbySavesDirectory(), fileName);
 
            if (System.IO.File.Exists(filePath))
            {
                var menu = CabbyCodesPlugin.cabbyMenu;
                if (menu == null)
                {
                    SaveCustomGame(customName);
                    return;
                }
 
                string msg = $"A save named '{customName}' already exists.\n\nOverwrite the existing file?";
                var popup = new CabbyMenu.UI.Popups.ConfirmationPopup(
                    menu,
                    "Overwrite Save File?",
                    msg,
                    "Overwrite",
                    "Cancel",
                    () => { SaveCustomGame(customName); },
                    null);
                popup.Show();
            }
            else
            {
                SaveCustomGame(customName);
            }

            // Reset the Enter key flag after processing to prevent multiple saves from the same key press
            if (!fromButtonClick)
            {
                enterKeyRecentlyPressed = false;
            }
        }

        /// <summary>
        /// Gets the latest text from the input field, even if not submitted yet.
        /// </summary>
        private static string GetCurrentInputFieldText()
        {
            if (customSaveNameInputPanel != null)
            {
                var inputField = customSaveNameInputPanel.GetInputField();
                if (inputField != null)
                {
                    return inputField.text ?? string.Empty;
                }
            }
            return customSaveNameRef.Get() ?? string.Empty;
        }

        /// <summary>
        /// Reference to the custom save name input field panel for forcing updates.
        /// </summary>
        private static InputFieldPanel<string> customSaveNameInputPanel;

        /// <summary>
        /// Synced reference for the custom save name input field.
        /// </summary>
        private static readonly BoxedReference<string> customSaveNameRef = new BoxedReference<string>("");

        /// <summary>
        /// Tracks the last loaded vanilla slot (profileID) for restoring after custom save load.
        /// </summary>
        public static int LastLoadedSlot = -1;

        /// <summary>
        /// Reference to the current loading popup for external access.
        /// </summary>
        private static IPersistentPopup currentLoadingPopup;

        /// <summary>
        /// Applies Harmony patches for custom save/load functionality.
        /// </summary>
        public static void ApplyPatches()
        {
            if (patchesApplied)
            {
                return;
            }

            patchesApplied = true;
        }

        private static float lastSaveAttemptTime = -1f;
    }
} 