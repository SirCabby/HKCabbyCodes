using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

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
                if (!string.IsNullOrEmpty(saveGameData.sceneName))
                {
                    Vector2 targetPosition = saveGameData.GetPlayerPosition();
                    void handler()
                    {
                        var gm = GameManager._instance;
                        var hero = gm?.hero_ctrl;
                        if (hero != null)
                        {
                            Vector3 newPos = new Vector3(targetPosition.x, targetPosition.y, hero.transform.position.z);
                            hero.transform.position = newPos;
                            gm.cameraCtrl?.SnapTo(newPos.x, newPos.y);
                        }
                        // Unsubscribe to avoid memory leaks
                        if (gm != null)
                            gm.OnFinishedEnteringScene -= handler;
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