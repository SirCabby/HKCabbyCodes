using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;
using CabbyCodes.Patches.Teleport;
using CabbyCodes.CheatState;
using CabbyCodes.Scenes;

namespace CabbyCodes.SavedGames
{
    public static class SavedGameManager
    {
        private const string SAVE_FILE_EXTENSION = ".dat";

        public static string GetCabbySavesDirectory()
        {
            string baseSaveDir = Application.persistentDataPath;
            string cabbySavesDir = Path.Combine(baseSaveDir, "CabbySaves");
            if (!Directory.Exists(cabbySavesDir))
            {
                Directory.CreateDirectory(cabbySavesDir);
            }
            return cabbySavesDir;
        }

        public static void SaveCustomGame(string saveName, Action<bool> callback = null)
        {
            if (GameManager.instance == null)
            {
                callback?.Invoke(false);
                return;
            }
            string fileName = GenerateSaveFileName(saveName);
            string filePath = Path.Combine(GetCabbySavesDirectory(), fileName);
            SaveCustomGameInternal(filePath, callback);
        }

        public static void LoadCustomGame(string fileName, Action<bool> callback = null)
        {
            if (GameManager.instance == null)
            {
                callback?.Invoke(false);
                return;
            }
            GameManager.instance.ui?.ContinueGame();
            string filePath = Path.Combine(GetCabbySavesDirectory(), fileName);
            if (!File.Exists(filePath))
            {
                callback?.Invoke(false);
                return;
            }
            LoadCustomGameInternal(filePath, callback);
        }

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

        public static string GenerateSaveFileName(string saveName)
        {
            if (string.IsNullOrWhiteSpace(saveName))
            {
                return DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + SAVE_FILE_EXTENSION;
            }
            else
            {
                string sanitizedName = string.Join("_", saveName.Split(Path.GetInvalidFileNameChars()));
                sanitizedName = sanitizedName.Replace(" ", "_");
                return sanitizedName + SAVE_FILE_EXTENSION;
            }
        }

        public static string GetDisplayNameFromFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return fileName;
            }
            string nameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            return nameWithoutExtension.Replace("_", " ");
        }

        private static void SaveCustomGameInternal(string filePath, Action<bool> callback)
        {
            try
            {
                GameManager gameManager = GameManager.instance;
                if (gameManager.playerData == null || gameManager.sceneData == null)
                {
                    callback?.Invoke(false);
                    return;
                }
                gameManager.SaveLevelState();
                gameManager.playerData.playTime = gameManager.PlayTime;
                gameManager.playerData.version = "1.5.78.11833";
                gameManager.playerData.profileID = -1;
                gameManager.playerData.CountGameCompletion();
                string currentSceneName = GameManager.GetBaseSceneName(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
                Vector2 currentPosition = Vector2.zero;
                if (gameManager.hero_ctrl != null && gameManager.hero_ctrl.transform != null)
                {
                    currentPosition = new Vector2(gameManager.hero_ctrl.transform.position.x, gameManager.hero_ctrl.transform.position.y);
                }
                var saveGameData = new Patches.Settings.SaveGameData(gameManager.playerData, gameManager.sceneData, currentSceneName, currentPosition);
                string jsonData = JsonUtility.ToJson(saveGameData);
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
                File.WriteAllBytes(filePath, fileData);
                callback?.Invoke(true);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to save custom game to {filePath}: {ex.Message}");
                callback?.Invoke(false);
            }
        }

        private static void LoadCustomGameInternal(string filePath, Action<bool> callback)
        {
            // Get the existing loading popup that was created when user clicked load
            var loadingPopup = Patches.Settings.CustomSaveLoadPatch.GetCurrentLoadingPopup();
            
            // Add a delay to ensure the popup has time to display before proceeding
            if (loadingPopup != null)
            {
                // Use a coroutine to delay the loading logic by at least 1 frame
                GameManager.instance?.StartCoroutine(DelayedLoadCustomGameInternal(filePath, callback, loadingPopup));
            }
            else
            {
                // No popup available, proceed immediately
                LoadCustomGameInternalCore(filePath, callback, null);
            }
        }

        /// <summary>
        /// Coroutine to delay loading logic to allow popup to display.
        /// </summary>
        private static System.Collections.IEnumerator DelayedLoadCustomGameInternal(string filePath, Action<bool> callback, CabbyMenu.UI.IPersistentPopup loadingPopup)
        {
            // Wait at least 2 frames to ensure popup is fully visible and rendered
            yield return null;
            yield return null;
            
            // Now proceed with the actual loading logic
            LoadCustomGameInternalCore(filePath, callback, loadingPopup);
        }

        /// <summary>
        /// Core loading logic that was previously in LoadCustomGameInternal.
        /// </summary>
        private static void LoadCustomGameInternalCore(string filePath, Action<bool> callback, CabbyMenu.UI.IPersistentPopup loadingPopup)
        {
            try
            {
                byte[] fileData = File.ReadAllBytes(filePath);
                GameManager gameManager = GameManager.instance;
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
                var saveGameData = JsonUtility.FromJson<Patches.Settings.SaveGameData>(jsonData);
                PlayerData.instance = saveGameData.playerData;
                gameManager.playerData = saveGameData.playerData;
                SceneData.instance = saveGameData.sceneData;
                gameManager.sceneData = saveGameData.sceneData;
                
                // Restore the last loaded slot after loading custom save
                if (Patches.Settings.CustomSaveLoadPatch.LastLoadedSlot >= 0)
                {
                    if (GameManager.instance != null)
                    {
                        GameManager.instance.profileID = Patches.Settings.CustomSaveLoadPatch.LastLoadedSlot;
                    }
                    if (PlayerData.instance != null && PlayerData.instance.profileID != GameManager.instance.profileID)
                    {
                        PlayerData.instance.profileID = GameManager.instance.profileID;
                    }
                }

                gameManager.inputHandler.RefreshPlayerData();
                gameManager.ContinueGame();
                
                // Always trigger cheat state restoration after the game loads, regardless of scene data
                void cheatRestoreHandler()
                {
                    // Unsubscribe first to avoid memory leaks
                    gameManager.OnFinishedEnteringScene -= cheatRestoreHandler;
                    
                    // Restore all active cheat states after the scene has loaded
                    CheatStateManager.RestoreAllCheatStates();
                }
                gameManager.OnFinishedEnteringScene += cheatRestoreHandler;
                
                // After the scene has finished loading, wait until the hero is sitting at a bench
                // (PlayerData.atBench == true). Once that is true, wait an additional 2 seconds
                // before teleporting to the saved scene and position.
                if (!string.IsNullOrEmpty(saveGameData.sceneName))
                {
                    Vector2 targetPosition = saveGameData.GetPlayerPosition();

                    // Create a teleport location for the saved scene and position
                    var sceneData = SceneManagement.GetSceneData(saveGameData.sceneName) ?? new SceneMapData(saveGameData.sceneName);
                    var teleportLocation = new TeleportLocation(sceneData, "Custom Save Location", targetPosition);

                    // Subscribe to the scene entry event to trigger teleport after the game loads
                    void handler()
                    {
                        // Unsubscribe first to avoid memory leaks
                        gameManager.OnFinishedEnteringScene -= handler;

                        // Start coroutine that waits for atBench flag and the extra delay
                        gameManager.StartCoroutine(TeleportAfterBench(teleportLocation, loadingPopup));
                    }
                    gameManager.OnFinishedEnteringScene += handler;
                }
                else
                {
                    // No scene teleport needed, hide popup immediately
                    if (loadingPopup != null)
                    {
                        loadingPopup.Hide();
                        loadingPopup.Destroy();
                    }
                    
                    // Clear the popup reference
                    Patches.Settings.CustomSaveLoadPatch.ClearCurrentLoadingPopup();
                    
                    // Also hide reload popup if this was a reload
                    HideReloadPopupIfPresent();
                }
                
                callback?.Invoke(true);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to load custom game from {filePath}: {ex.Message}");
                
                // Hide popup on error
                if (loadingPopup != null)
                {
                    loadingPopup.Hide();
                    loadingPopup.Destroy();
                }
                
                // Clear the popup reference
                Patches.Settings.CustomSaveLoadPatch.ClearCurrentLoadingPopup();
                
                // Also hide reload popup if this was a reload
                HideReloadPopupIfPresent();
                
                callback?.Invoke(false);
            }
        }

        /// <summary>
        /// Hides the reload popup if it exists. Used when custom save loads complete.
        /// </summary>
        private static void HideReloadPopupIfPresent()
        {
            // Access the reload popup through reflection since it's private in GameReloadManager
            try
            {
                var gameReloadManagerType = typeof(GameReloadManager);
                var currentReloadPopupField = gameReloadManagerType.GetField("currentReloadPopup", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                
                if (currentReloadPopupField != null)
                {
                    if (currentReloadPopupField.GetValue(null) is CabbyMenu.UI.IPersistentPopup reloadPopup)
                    {
                        reloadPopup.Hide();
                        reloadPopup.Destroy();
                        currentReloadPopupField.SetValue(null, null);

                        // Clear all reload requests
                        var clearAllReloadRequestsMethod = gameReloadManagerType.GetMethod("ClearAllReloadRequests",
                            System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                        clearAllReloadRequestsMethod?.Invoke(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                CabbyCodesPlugin.BLogger.LogWarning(string.Format("Failed to hide reload popup: {0}", ex.Message));
            }
        }


        /// <summary>
        /// Coroutine to teleport to the correct scene and position after the game has loaded.
        /// </summary>
        /// <param name="teleportLocation">The location to teleport to.</param>
        /// <param name="loadingPopup">The loading popup to hide after teleport is complete.</param>
        private static System.Collections.IEnumerator TeleportAfterBench(TeleportLocation teleportLocation, CabbyMenu.UI.IPersistentPopup loadingPopup)
        {
            // Wait until the player sits down at a bench (atBench becomes true)
            while (PlayerData.instance == null || !PlayerData.instance.atBench)
            {
                yield return null; // wait one frame
            }

            // Update popup message to instruct user to get off bench
            loadingPopup?.SetMessageText("Get off bench to restore position");
            
            // Also update reload popup message if it exists
            UpdateReloadPopupMessage("Get off bench to restore position");

            // Wait until the player STANDS UP again (atBench becomes false)
            while (PlayerData.instance != null && PlayerData.instance.atBench)
            {
                yield return null; // still sitting
            }

            // Restore original message
            loadingPopup?.SetMessageText("Please Wait . . .");
            
            // Also update reload popup message
            UpdateReloadPopupMessage("Please Wait . . .");

            // Extra delay to ensure everything is fully settled after standing up
            yield return new WaitForSeconds(0.5f);
            
            // Use the same logic as TeleportAfterSceneLoad for the actual teleport
            string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            string baseSceneName = GameManager.GetBaseSceneName(currentScene);

            if (baseSceneName == teleportLocation.Scene.SceneName)
            {
                // We're already in the correct scene, just move to the position
                var gm = GameManager._instance;
                var hero = gm?.hero_ctrl;
                if (hero != null)
                {
                    Vector3 newPos = new Vector3(teleportLocation.Location.x, teleportLocation.Location.y, hero.transform.position.z);
                    hero.transform.position = newPos;
                    gm.cameraCtrl?.SnapTo(newPos.x, newPos.y);

                    CabbyCodesPlugin.BLogger.LogDebug(string.Format("Moved to position ({0}, {1}) in scene '{2}'",
                        teleportLocation.Location.x, teleportLocation.Location.y, teleportLocation.Scene.SceneName));
                }
            }
            else
            {
                // We need to teleport to a different scene
                CabbyCodesPlugin.BLogger.LogDebug(string.Format("Teleporting to scene '{0}' at position ({1}, {2})",
                    teleportLocation.Scene.SceneName, teleportLocation.Location.x, teleportLocation.Location.y));

                TeleportService.DoTeleport(teleportLocation);
            }
            
            // Hide and destroy the loading popup after teleport is complete
            if (loadingPopup != null)
            {
                loadingPopup.Hide();
                loadingPopup.Destroy();
            }
            
            // Also hide reload popup if this was a reload
            HideReloadPopupIfPresent();
        }

        /// <summary>
        /// Updates the reload popup message if it exists. Used during bench sequence.
        /// </summary>
        /// <param name="message">The message to display.</param>
        private static void UpdateReloadPopupMessage(string message)
        {
            try
            {
                var gameReloadManagerType = typeof(GameReloadManager);
                var currentReloadPopupField = gameReloadManagerType.GetField("currentReloadPopup", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                
                if (currentReloadPopupField != null)
                {
                    if (currentReloadPopupField.GetValue(null) is CabbyMenu.UI.IPersistentPopup reloadPopup)
                    {
                        reloadPopup.SetMessageText(message);
                    }
                }
            }
            catch (Exception ex)
            {
                CabbyCodesPlugin.BLogger.LogWarning(string.Format("Failed to update reload popup message: {0}", ex.Message));
            }
        }
    }
} 