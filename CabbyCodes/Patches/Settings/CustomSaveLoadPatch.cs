using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using System.Collections.Generic;
using UnityEngine;
using CabbyMenu.Utilities;
using GlobalEnums;
using CabbyCodes.SavedGames;

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
        /// Creates and returns the custom save name input field panel (for external use).
        /// </summary>
        /// <returns>The created InputFieldPanel for custom save names.</returns>
        public static InputFieldPanel<string> CreateCustomSaveNamePanel()
        {
            // Calculate width for 35 characters but allow 50 characters
            int widthFor35Chars = CalculatePanelWidth(35);
            var panel = new InputFieldPanel<string>(customSaveNameRef, KeyCodeMap.ValidChars.AlphaNumericWithSpaces, 60, widthFor35Chars, "(Optional) Save game name");
            customSaveNameInputPanel = panel;
            return panel;
        }

        /// <summary>
        /// Creates the Save button panel wired to the internal SaveCustomGame logic.
        /// </summary>
        public static ButtonPanel CreateSaveButtonPanel()
        {
            return new ButtonPanel(AttemptSaveCustomGame, "Save", "Save game at current location");
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
        private static void SaveCustomGame()
        {
            // Get the custom name if provided
            string customName = ((customSaveNameRef?.Get()) ?? "").Trim();
            
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
            });
        }

        /// <summary>
        /// Loads a custom save file.
        /// </summary>
        /// <param name="saveFileName">The name of the save file to load.</param>
        private static void LoadCustomSave(string saveFileName)
        {
            // Cache the current slot before leaving (in case it changed)
            if (GameManager.instance != null)
            {
                LastLoadedSlot = GameManager.instance.profileID;
            }

            // Instead of loading immediately, set up QuickStartPatch to load after menu
            QuickStartPatch.CustomFileToLoad = saveFileName;

            // Start coroutine to return to main menu
            GameManager.instance?.StartCoroutine(GameManager.instance.ReturnToMainMenu(
                    GameManager.ReturnToMainMenuSaveModes.DontSave, null));
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
        }

        /// <summary>
        /// Attempts to save the game, prompting for overwrite if file exists.
        /// </summary>
        private static void AttemptSaveCustomGame()
        {
            string customName = ((customSaveNameRef?.Get()) ?? "").Trim();
            string fileName = SavedGameManager.GenerateSaveFileName(customName);
            string filePath = System.IO.Path.Combine(SavedGameManager.GetCabbySavesDirectory(), fileName);

            if (System.IO.File.Exists(filePath))
            {
                var menu = CabbyCodesPlugin.cabbyMenu;
                if (menu == null)
                {
                    SaveCustomGame();
                    return;
                }

                string msg = $"A save named '{customName}' already exists.\n\nOverwrite the existing file?";
                var popup = new CabbyMenu.UI.Popups.ConfirmationPopup(
                    menu,
                    "Overwrite Save File?",
                    msg,
                    "Overwrite",
                    "Cancel",
                    () => { SaveCustomGame(); },
                    null);
                popup.Show();
            }
            else
            {
                SaveCustomGame();
            }
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
    }
} 