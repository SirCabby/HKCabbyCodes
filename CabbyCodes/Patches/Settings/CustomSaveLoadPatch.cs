using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;
using CabbyMenu.Utilities;
using GlobalEnums;

namespace CabbyCodes.Patches.Settings
{
    /// <summary>
    /// Handles custom save/load functionality with user-named saves in a CabbySaves subfolder.
    /// </summary>
    public class CustomSaveLoadPatch
    {
        /// <summary>
        /// File extension for custom save files.
        /// </summary>
        private const string SAVE_FILE_EXTENSION = ".dat";

        /// <summary>
        /// Flag to track if patches have been applied.
        /// </summary>
        private static bool patchesApplied = false;

        /// <summary>
        /// Gets the CabbySaves directory path.
        /// </summary>
        /// <returns>The full path to the CabbySaves directory.</returns>
        public static string GetCabbySavesDirectory()
        {
            // Get the base save directory from the game
            string baseSaveDir = Application.persistentDataPath;
            string cabbySavesDir = Path.Combine(baseSaveDir, "CabbySaves");
            
            // Ensure the directory exists
            if (!Directory.Exists(cabbySavesDir))
            {
                Directory.CreateDirectory(cabbySavesDir);
            }
            
            return cabbySavesDir;
        }

        /// <summary>
        /// Saves the current game state to a custom save file.
        /// </summary>
        /// <param name="saveName">The name for the save file (optional, will use timestamp if null/empty).</param>
        /// <param name="callback">Callback to execute when save completes.</param>
        public static void SaveCustomGame(string saveName, Action<bool> callback = null)
        {
            if (GameManager.instance == null)
            {
                callback?.Invoke(false);
                return;
            }

            // Generate filename
            string fileName = GenerateSaveFileName(saveName);
            string filePath = Path.Combine(GetCabbySavesDirectory(), fileName);

            // Use the vanilla save logic but redirect the output
            SaveCustomGameInternal(filePath, callback);
        }

        /// <summary>
        /// Loads a custom save file.
        /// </summary>
        /// <param name="fileName">The name of the save file to load.</param>
        /// <param name="callback">Callback to execute when load completes.</param>
        public static void LoadCustomGame(string fileName, Action<bool> callback = null)
        {
            if (GameManager.instance == null)
            {
                callback?.Invoke(false);
                return;
            }

            // Ensure UI is set up for continue (matches vanilla flow)
            GameManager.instance.ui?.ContinueGame();

            string filePath = Path.Combine(GetCabbySavesDirectory(), fileName);
            
            if (!File.Exists(filePath))
            {
                callback?.Invoke(false);
                return;
            }

            // Use the vanilla load logic but read from our custom file
            LoadCustomGameInternal(filePath, callback);
        }

        /// <summary>
        /// Gets a list of available custom save files.
        /// </summary>
        /// <returns>List of save file names.</returns>
        public static List<string> GetCustomSaveFiles()
        {
            string cabbySavesDir = GetCabbySavesDirectory();
            
            if (!Directory.Exists(cabbySavesDir))
            {
                return new List<string>();
            }

            return Directory.GetFiles(cabbySavesDir, "*" + SAVE_FILE_EXTENSION)
                           .Select(Path.GetFileName)
                           .Where(name => !string.IsNullOrEmpty(name))
                           .ToList();
        }

        /// <summary>
        /// Deletes a custom save file.
        /// </summary>
        /// <param name="fileName">The name of the save file to delete.</param>
        /// <returns>True if the file was deleted successfully, false otherwise.</returns>
        public static bool DeleteCustomSave(string fileName)
        {
            string filePath = Path.Combine(GetCabbySavesDirectory(), fileName);
            
            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                    return true;
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Failed to delete custom save file {fileName}: {ex.Message}");
                }
            }
            
            return false;
        }

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
            List<string> saveFiles = GetCustomSaveFiles();
            
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
            string displayName = GetDisplayNameFromFileName(saveFileName);
            
            ButtonPanel buttonPanel = new ButtonPanel(() => { LoadCustomSave(saveFileName); }, "Load", displayName);

            // Add the panel to the menu first so it has a parent
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(buttonPanel);
            
            // Track this panel for refresh purposes
            saveFilePanels.Add(buttonPanel);

            // Create the X button after the panel has been added to the menu
            GameObject destroyButton = PanelAdder.AddDestroyButtonToPanel(buttonPanel, () =>
            {
                // Remove the save file
                bool deleted = DeleteCustomSave(saveFileName);
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
            
            SaveCustomGame(customName, (success) =>
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

        /// <summary>
        /// Generates a save file name from the provided name or timestamp.
        /// </summary>
        /// <param name="saveName">The user-provided save name (can be null/empty).</param>
        /// <returns>The generated filename with SAVE_FILE_EXTENSION extension.</returns>
        private static string GenerateSaveFileName(string saveName)
        {
            if (string.IsNullOrWhiteSpace(saveName))
            {
                // Use timestamp format: YYYY-MM-DD_HH-MM-SS
                return DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + SAVE_FILE_EXTENSION;
            }
            else
            {
                // Sanitize the filename to remove invalid characters and convert spaces to underscores
                string sanitizedName = string.Join("_", saveName.Split(Path.GetInvalidFileNameChars()));
                // Convert spaces to underscores for the actual file name
                sanitizedName = sanitizedName.Replace(" ", "_");
                return sanitizedName + SAVE_FILE_EXTENSION;
            }
        }

        /// <summary>
        /// Converts a file name back to a display name by removing extension and converting underscores to spaces.
        /// </summary>
        /// <param name="fileName">The file name to convert.</param>
        /// <returns>The display name without extension and with spaces instead of underscores.</returns>
        private static string GetDisplayNameFromFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return fileName;
            }

            // Remove the file extension
            string nameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            
            // Convert underscores back to spaces for display
            return nameWithoutExtension.Replace("_", " ");
        }

        /// <summary>
        /// Internal method to save the game using vanilla logic but to a custom file.
        /// </summary>
        /// <param name="filePath">The full path where to save the file.</param>
        /// <param name="callback">Callback to execute when save completes.</param>
        private static void SaveCustomGameInternal(string filePath, Action<bool> callback)
        {
            try
            {
                // Get the current game state
                GameManager gameManager = GameManager.instance;
                if (gameManager.playerData == null || gameManager.sceneData == null)
                {
                    callback?.Invoke(false);
                    return;
                }

                // Save level state first (like vanilla does)
                gameManager.SaveLevelState();

                // Update player data like vanilla does
                // Note: sessionPlayTimer is private, so we'll use the current play time
                gameManager.playerData.playTime = gameManager.PlayTime;
                gameManager.playerData.version = "1.5.78.11833";
                gameManager.playerData.profileID = -1; // Use -1 to indicate custom save
                gameManager.playerData.CountGameCompletion();

                // Capture current scene and position
                string currentSceneName = GameManager.GetBaseSceneName(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
                Vector2 currentPosition = Vector2.zero;
                
                // Get current player position if hero is available
                if (gameManager.hero_ctrl != null && gameManager.hero_ctrl.transform != null)
                {
                    currentPosition = new Vector2(gameManager.hero_ctrl.transform.position.x, gameManager.hero_ctrl.transform.position.y);
                }

                // Create the save data structure (extends vanilla with scene/position data)
                var saveGameData = new SaveGameData(gameManager.playerData, gameManager.sceneData, currentSceneName, currentPosition);
                
                // Serialize to JSON (same as vanilla)
                string jsonData = JsonUtility.ToJson(saveGameData);
                
                // Apply encryption if the game uses it
                byte[] fileData;
                if (gameManager.gameConfig.useSaveEncryption && !Platform.Current.IsFileSystemProtected)
                {
                    string encryptedData = Encryption.Encrypt(jsonData);
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        binaryFormatter.Serialize(memoryStream, encryptedData);
                        fileData = memoryStream.ToArray();
                    }
                }
                else
                {
                    fileData = Encoding.UTF8.GetBytes(jsonData);
                }

                // Write to our custom file
                File.WriteAllBytes(filePath, fileData);
                
                callback?.Invoke(true);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to save custom game to {filePath}: {ex.Message}");
                callback?.Invoke(false);
            }
        }

        /// <summary>
        /// Internal method to load the game using vanilla logic but from a custom file.
        /// </summary>
        /// <param name="filePath">The full path of the file to load.</param>
        /// <param name="callback">Callback to execute when load completes.</param>
        private static void LoadCustomGameInternal(string filePath, Action<bool> callback)
        {
            try
            {
                // Read the file data
                byte[] fileData = File.ReadAllBytes(filePath);
                
                GameManager gameManager = GameManager.instance;
                
                // Deserialize using the same logic as vanilla
                string jsonData;
                if (gameManager.gameConfig.useSaveEncryption && !Platform.Current.IsFileSystemProtected)
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    using (MemoryStream memoryStream = new MemoryStream(fileData))
                    {
                        string encryptedData = (string)binaryFormatter.Deserialize(memoryStream);
                        jsonData = Encryption.Decrypt(encryptedData);
                    }
                }
                else
                {
                    jsonData = Encoding.UTF8.GetString(fileData);
                }

                SaveGameData saveGameData = JsonUtility.FromJson<SaveGameData>(jsonData);
                
                // Apply the loaded data like vanilla does
                PlayerData.instance = saveGameData.playerData;
                gameManager.playerData = saveGameData.playerData;
                SceneData.instance = saveGameData.sceneData;
                gameManager.sceneData = saveGameData.sceneData;
                
                // Update input handler
                gameManager.inputHandler.RefreshPlayerData();

                // Call ContinueGame to trigger the normal transition (matches vanilla flow)
                // Note: This will fade out, load the correct scene, and handle all state
                gameManager.ContinueGame();

                // Restore the hero position after the scene loads (if available)
                if (!string.IsNullOrEmpty(saveGameData.sceneName))
                {
                    Vector2 targetPosition = saveGameData.GetPlayerPosition();
                    // Use a local handler to ensure correct unsubscription
                    void handler()
                    {
                        OnFinishedEnteringSceneLoad(targetPosition);
                        gameManager.OnFinishedEnteringScene -= handler;
                    }

                    gameManager.OnFinishedEnteringScene += handler;
                }

                callback?.Invoke(true);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to load custom game from {filePath}: {ex.Message}");
                callback?.Invoke(false);
            }
        }
    }
} 