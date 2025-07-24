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
        /// Adds all custom save/load panels to the mod menu.
        /// </summary>
        public static void AddPanels()
        {
            // Header panel
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Save / Load").SetColor(CheatPanel.headerColor));
            
            // Save panel
            AddSavePanel();
            
            // Custom save name input field
            AddCustomSaveNamePanel();
            
            // Sub-header for saved games
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Saved Games").SetColor(CheatPanel.subHeaderColor));
            
            // Load existing save files
            LoadExistingSaveFiles();
        }

        /// <summary>
        /// Adds the save game button panel to the menu.
        /// </summary>
        private static void AddSavePanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new ButtonPanel(SaveCustomGame, "Save", "Save game at current location"));
        }

        /// <summary>
        /// Adds the custom save name input field panel to the menu.
        /// </summary>
        private static void AddCustomSaveNamePanel()
        {
            // Calculate width for 35 characters but allow 50 characters
            int widthFor35Chars = CalculatePanelWidth(35);
            customSaveNameInputPanel = new InputFieldPanel<string>(customSaveNameRef, KeyCodeMap.ValidChars.AlphaNumericWithSpaces, 60, widthFor35Chars, "(Optional) Save game name");
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(customSaveNameInputPanel);
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
        /// Loads existing save files and creates panels for them.
        /// </summary>
        private static void LoadExistingSaveFiles()
        {
            List<string> saveFiles = SavedGameManager.GetCustomSaveFiles();
            
            foreach (string saveFileName in saveFiles)
            {
                AddCustomSavePanel(saveFileName);
            }
        }

        /// <summary>
        /// Refreshes the save file list by removing old panels and creating new ones.
        /// </summary>
        private static void RefreshSaveFileList()
        {
            // Remove all existing save file panels
            foreach (CheatPanel panel in saveFilePanels)
            {
                CabbyCodesPlugin.cabbyMenu.RemoveCheatPanel(panel);
            }
            saveFilePanels.Clear();
            
            // Reload and create new panels for current save files
            LoadExistingSaveFiles();
        }

        /// <summary>
        /// Adds a custom save game panel with load functionality and destroy button.
        /// </summary>
        /// <param name="saveFileName">The name of the save file to create a panel for.</param>
        private static void AddCustomSavePanel(string saveFileName)
        {
            // Convert file name to display name for the UI
            string displayName = SavedGameManager.GetDisplayNameFromFileName(saveFileName);
            
            ButtonPanel buttonPanel = new ButtonPanel(() => { LoadCustomSave(saveFileName); }, "Load", displayName);

            // Add the panel to the menu first so it has a parent
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(buttonPanel);
            
            // Track this panel for refresh purposes
            saveFilePanels.Add(buttonPanel);

            // Create the X button after the panel has been added to the menu
            GameObject destroyButton = PanelAdder.AddDestroyButtonToPanel(buttonPanel, () =>
            {
                // Remove the save file
                bool deleted = SavedGameManager.DeleteCustomSave(saveFileName);
                if (deleted)
                {
                    // Remove the panel from tracking and menu
                    saveFilePanels.Remove(buttonPanel);
                    CabbyCodesPlugin.cabbyMenu.RemoveCheatPanel(buttonPanel);
                    CabbyCodesPlugin.BLogger.LogDebug(string.Format("Removed save file: {0}", saveFileName));
                }
                else
                {
                    CabbyCodesPlugin.BLogger.LogWarning(string.Format("Failed to remove save file: {0}", saveFileName));
                }
            });
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
                    
                    // Refresh the save file list to show the new save
                    RefreshSaveFileList();
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
                
                // Actually unpause the game
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
        /// Reference to the custom save name input field panel for forcing updates.
        /// </summary>
        private static InputFieldPanel<string> customSaveNameInputPanel;

        /// <summary>
        /// Synced reference for the custom save name input field.
        /// </summary>
        private static readonly BoxedReference<string> customSaveNameRef = new BoxedReference<string>("");

        /// <summary>
        /// List of save file panels that need to be refreshed.
        /// </summary>
        private static readonly List<CheatPanel> saveFilePanels = new List<CheatPanel>();

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

            InitializeConfig();
            patchesApplied = true;
        }

        /// <summary>
        /// Initializes the configuration entries.
        /// </summary>
        private static void InitializeConfig()
        {
            // No configuration entries needed for custom save/load
        }
    }
} 