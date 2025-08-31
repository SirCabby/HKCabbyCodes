using CabbyMenu.UI.CheatPanels;
using CabbyMenu.SyncedReferences;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using CabbyCodes.SavedGames;

namespace CabbyCodes.Patches.Settings
{
    public class SaveGameAnalysisPatch : ISyncedValueList
    {
        private static SaveGameAnalysisPatch currentInstance;
        private readonly BoxedReference<int> selectedFileIndexRef = new BoxedReference<int>(0);
        private DropdownPanel fileDropdownPanel;
        private readonly List<string> customSaveFiles = new List<string>();

        /// <summary>
        /// Adds the save analyzer panels to the mod menu
        /// </summary>
        public static void AddPanels()
        {
            // Clear any existing instance when adding panels
            currentInstance = null;
            
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Save Game Analyzer").SetColor(CheatPanel.headerColor));
            
            AddFileDropdownPanel();
            AddAnalyzeButtonPanel();
        }

        /// <summary>
        /// Adds the file selection dropdown panel
        /// </summary>
        private static void AddFileDropdownPanel()
        {
            currentInstance = new SaveGameAnalysisPatch();
            currentInstance.fileDropdownPanel = new DropdownPanel(currentInstance, "Custom Save File", CabbyMenu.Constants.DEFAULT_PANEL_HEIGHT);
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(currentInstance.fileDropdownPanel);
        }

        /// <summary>
        /// Adds the analyze button panel
        /// </summary>
        private static void AddAnalyzeButtonPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new ButtonPanel(AnalyzeSelectedFile, "Analyze Save", "Analyze the selected custom save file and save results to Output directory"));
        }

        /// <summary>
        /// Analyzes the currently selected custom save file
        /// </summary>
        private static void AnalyzeSelectedFile()
        {
            if (currentInstance == null)
            {
                return;
            }

            int selectedIndex = currentInstance.selectedFileIndexRef.Get();
            if (selectedIndex < 0 || selectedIndex >= currentInstance.customSaveFiles.Count)
            {
                return;
            }

            string fileName = currentInstance.customSaveFiles[selectedIndex];
            if (fileName == "No custom save files found")
            {
                return;
            }

            string analysis = AnalyzeCustomSaveFile(fileName);
            if (analysis.StartsWith("Error:"))
            {
                return;
            }

            SaveAnalysisToFile(fileName, analysis);
        }

        /// <summary>
        /// Analyzes a custom save file
        /// </summary>
        private static string AnalyzeCustomSaveFile(string fileName)
        {
            try
            {
                string filePath = Path.Combine(SavedGameManager.GetCabbySavesDirectory(), fileName);
                if (!File.Exists(filePath))
                {
                    return string.Format("Error: Custom save file not found: {0}", fileName);
                }

                // Read the file
                byte[] fileData = File.ReadAllBytes(filePath);
                if (fileData == null || fileData.Length == 0)
                {
                    return string.Format("Error: Custom save file is empty: {0}", fileName);
                }

                // Decrypt if needed - try both encrypted and unencrypted formats
                string json;
                bool isEncrypted = false;
                
                try
                {
                    // First try to deserialize as BinaryFormatter (encrypted format)
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    using (MemoryStream memoryStream = new MemoryStream(fileData))
                    {
                        string encryptedData = (string)binaryFormatter.Deserialize(memoryStream);
                        json = Encryption.Decrypt(encryptedData);
                        isEncrypted = true;
                    }
                }
                catch
                {
                    // If BinaryFormatter fails, treat as unencrypted JSON
                    json = Encoding.UTF8.GetString(fileData);
                    isEncrypted = false;
                }

                // Deserialize the save data
                var saveGameData = JsonUtility.FromJson<SaveGameData>(json);
                if (saveGameData == null)
                {
                    return "Error: Failed to deserialize custom save game data";
                }

                // Create human-readable output
                return CreateCustomSaveOutput(saveGameData, fileName, isEncrypted);
            }
            catch (Exception ex)
            {
                return string.Format("Error analyzing custom save file: {0}", ex.Message);
            }
        }

        /// <summary>
        /// Creates human-readable output for custom save data
        /// </summary>
        private static string CreateCustomSaveOutput(SaveGameData saveData, string fileName, bool isEncrypted = false)
        {
            var output = new Dictionary<string, object>
            {
                ["fileName"] = fileName,
                ["analysisTimestamp"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                ["fileFormat"] = new Dictionary<string, object>
                {
                    ["isEncrypted"] = isEncrypted,
                    ["format"] = isEncrypted ? "BinaryFormatter + Encryption" : "Plain JSON"
                },
                ["dataValidation"] = new Dictionary<string, object>
                {
                    ["playerDataValid"] = saveData.playerData != null,
                    ["sceneDataValid"] = saveData.sceneData != null,
                    ["sceneNameValid"] = !string.IsNullOrEmpty(saveData.sceneName),
                    ["positionValid"] = saveData.playerX != 0 || saveData.playerY != 0,
                    ["playerDataProfileID"] = saveData.playerData?.profileID ?? -1,
                    ["playerDataVersion"] = saveData.playerData?.version ?? "unknown",
                    ["sceneNameLength"] = saveData.sceneName?.Length ?? 0,
                    ["positionX"] = saveData.playerX,
                    ["positionY"] = saveData.playerY
                },
                ["potentialIssues"] = AnalyzePotentialIssues(saveData)
            };

            // Use reflection to get all fields from SaveGameData
            var fields = typeof(SaveGameData).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            foreach (var field in fields)
            {
                var value = field.GetValue(saveData);
                output[field.Name] = ConvertValueForOutput(value);
            }

            return CreateSimpleJson(output);
        }

        /// <summary>
        /// Analyzes potential issues that could cause loading problems
        /// </summary>
        private static Dictionary<string, object> AnalyzePotentialIssues(SaveGameData saveData)
        {
            var issues = new Dictionary<string, object>();
            var warnings = new List<string>();
            var errors = new List<string>();

            if (saveData.playerData == null)
            {
                errors.Add("PlayerData is null - this will cause loading to fail");
            }

            if (saveData.sceneData == null)
            {
                errors.Add("SceneData is null - this will cause loading to fail");
            }

            if (string.IsNullOrEmpty(saveData.sceneName))
            {
                warnings.Add("Scene name is empty - teleportation will be skipped");
            }
            else if (saveData.sceneName.Length > 50)
            {
                warnings.Add("Scene name is unusually long - may indicate corruption");
            }

            // Check position issues
            if (saveData.playerX == 0 && saveData.playerY == 0)
            {
                warnings.Add("Player position is at origin (0,0) - may be default/empty position");
            }

            // Check for extreme values that might indicate corruption
            if (Math.Abs(saveData.playerX) > 10000 || Math.Abs(saveData.playerY) > 10000)
            {
                warnings.Add("Player position has extreme values - may indicate corruption");
            }

            // Check PlayerData version
            if (saveData.playerData != null)
            {
                if (string.IsNullOrEmpty(saveData.playerData.version))
                {
                    warnings.Add("PlayerData version is empty - may cause compatibility issues");
                }
                if (saveData.playerData.profileID < -1)
                {
                    warnings.Add("PlayerData profileID is invalid - may cause slot management issues");
                }
            }

            // Check SceneData for potential issues
            if (saveData.sceneData != null)
            {
                if (saveData.sceneData.persistentBoolItems == null)
                {
                    warnings.Add("SceneData persistentBoolItems is null");
                }
                else if (saveData.sceneData.persistentBoolItems.Count > 1000)
                {
                    warnings.Add("SceneData has unusually many persistent bool items - may indicate corruption");
                }

                if (saveData.sceneData.persistentIntItems == null)
                {
                    warnings.Add("SceneData persistentIntItems is null");
                }
                else if (saveData.sceneData.persistentIntItems.Count > 1000)
                {
                    warnings.Add("SceneData has unusually many persistent int items - may indicate corruption");
                }

                if (saveData.sceneData.geoRocks == null)
                {
                    warnings.Add("SceneData geoRocks is null");
                }
                else if (saveData.sceneData.geoRocks.Count > 500)
                {
                    warnings.Add("SceneData has unusually many geo rocks - may indicate corruption");
                }
            }

            issues["warnings"] = warnings;
            issues["errors"] = errors;
            issues["hasWarnings"] = warnings.Count > 0;
            issues["hasErrors"] = errors.Count > 0;
            issues["totalIssues"] = warnings.Count + errors.Count;

            return issues;
        }

        /// <summary>
        /// Creates a simple JSON string from a dictionary
        /// </summary>
        private static string CreateSimpleJson(Dictionary<string, object> data)
        {
            var json = new StringBuilder();
            json.AppendLine("{");
            
            bool first = true;
            foreach (var kvp in data)
            {
                if (!first) json.AppendLine(",");
                first = false;
                
                json.AppendFormat("  \"{0}\": ", kvp.Key);
                
                if (kvp.Value is string str)
                {
                    json.AppendFormat("\"{0}\"", str.Replace("\"", "\\\""));
                }
                else if (kvp.Value is Dictionary<string, object> dict)
                {
                    json.Append(CreateSimpleJson(dict));
                }
                else if (kvp.Value is List<Dictionary<string, object>> list)
                {
                    json.Append("[");
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (i > 0) json.Append(",");
                        json.Append(CreateSimpleJson(list[i]));
                    }
                    json.Append("]");
                }
                else
                {
                    json.Append(kvp.Value?.ToString() ?? "null");
                }
            }
            
            json.AppendLine();
            json.Append("}");
            return json.ToString();
        }

        /// <summary>
        /// Saves the analysis to a file in the Output directory
        /// </summary>
        private static void SaveAnalysisToFile(string fileName, string analysis)
        {
            string outputDir = Path.Combine(Application.dataPath, "..", "Output");
            Directory.CreateDirectory(outputDir);
            
            string baseFileName = Path.GetFileNameWithoutExtension(fileName);
            string outputFileName = string.Format("custom_save_analysis_{0}_{1}.json", baseFileName, DateTime.Now.ToString("yyyyMMdd_HHmmss"));
            string filePath = Path.Combine(outputDir, outputFileName);
            
            File.WriteAllText(filePath, analysis);
        }

        /// <summary>
        /// Converts a value to a format suitable for JSON output
        /// </summary>
        private static object ConvertValueForOutput(object value)
        {
            if (value == null) return null;
            
            var valueType = value.GetType();
            
            if (valueType.IsPrimitive || valueType == typeof(string) || valueType == typeof(decimal))
            {
                return value;
            }
            
            if (valueType.IsEnum)
            {
                return value.ToString();
            }
            
            if (valueType.IsArray)
            {
                var array = (Array)value;
                var list = new List<object>();
                for (int i = 0; i < array.Length; i++)
                {
                    list.Add(ConvertValueForOutput(array.GetValue(i)));
                }
                return list;
            }
            
            if (valueType.IsGenericType && typeof(System.Collections.IEnumerable).IsAssignableFrom(valueType))
            {
                var list = new List<object>();
                var enumerable = (System.Collections.IEnumerable)value;
                foreach (var item in enumerable)
                {
                    list.Add(ConvertValueForOutput(item));
                }
                return list;
            }
            
            // Handle complex objects - use reflection to get all fields
            if (valueType.IsClass)
            {
                var objectOutput = new Dictionary<string, object>();
                var fields = valueType.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                
                foreach (var field in fields)
                {
                    try
                    {
                        var fieldValue = field.GetValue(value);
                        objectOutput[field.Name] = ConvertValueForOutput(fieldValue);
                    }
                    catch (Exception)
                    {
                        objectOutput[field.Name] = "ERROR_READING_FIELD";
                    }
                }
                
                return objectOutput;
            }
            
            return value.ToString();
        }

        /// <summary>
        /// Saves the analysis to a file in the Output directory
        /// </summary>
        /// <param name="saveSlot">The save slot analyzed</param>
        /// <param name="analysis">The analysis JSON string</param>
        public void SaveAnalysisToFile(int saveSlot, string analysis)
        {
            string outputDir = Path.Combine(Application.dataPath, "..", "Output");
            Directory.CreateDirectory(outputDir);
            
            string fileName = $"save_analysis_slot_{saveSlot}_{DateTime.Now:yyyyMMdd_HHmmss}.json";
            string filePath = Path.Combine(outputDir, fileName);
            
            File.WriteAllText(filePath, analysis);
        }

        /// <summary>
        /// Gets the current selected file index
        /// </summary>
        public int Get()
        {
            return selectedFileIndexRef.Get();
        }

        /// <summary>
        /// Sets the selected file index
        /// </summary>
        public void Set(int value)
        {
            selectedFileIndexRef.Set(value);
        }

        /// <summary>
        /// Gets the list of available custom save file options
        /// </summary>
        public List<string> GetValueList()
        {
            // Refresh the file list each time it's requested
            customSaveFiles.Clear();
            
            List<string> saveFiles = SavedGameManager.GetCustomSaveFiles();
            if (saveFiles.Count > 0)
            {
                customSaveFiles.AddRange(saveFiles);
            }
            else
            {
                customSaveFiles.Add("No custom save files found");
            }
            
            return customSaveFiles;
        }

        /// <summary>
        /// Refreshes the file list for the current instance. Called when saves are created or deleted.
        /// </summary>
        public static void RefreshFileList()
        {
            if (currentInstance != null)
            {
                // Clear the current list to force a refresh
                currentInstance.customSaveFiles.Clear();
                
                // Reset the selection to 0 since the list is changing
                currentInstance.selectedFileIndexRef.Set(0);
                
                // Get the updated file list
                List<string> updatedFiles = currentInstance.GetValueList();
                
                // Update the dropdown options if the dropdown panel exists
                if (currentInstance.fileDropdownPanel != null)
                {
                    var dropDownSync = currentInstance.fileDropdownPanel.GetDropDownSync();
                    if (dropDownSync != null)
                    {
                        var customDropdown = dropDownSync.GetCustomDropdown();
                        
                        // Check if the custom dropdown is still valid
                        if (customDropdown == null || customDropdown.transform == null)
                        {
                            return; // Skip refresh if dropdown is invalid
                        }
                        
                        try
                        {
                            // Force a complete recreation by using reflection to access the private ClearExistingOptionButtons method
                            var clearMethod = customDropdown.GetType().GetMethod("ClearExistingOptionButtons", 
                                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                            // Clear existing buttons
                            clearMethod?.Invoke(customDropdown, null);

                            // Set the new options
                            dropDownSync.SetOptions(updatedFiles);
                            
                            // Force the dropdown to recreate the option buttons by calling CreateOptionButtons
                            var createMethod = customDropdown.GetType().GetMethod("CreateOptionButtons", 
                                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                            
                            createMethod?.Invoke(customDropdown, null);
                            
                            // Reset the dropdown selection to 0
                            customDropdown.SetValue(0);
                            
                            // Update the dropdown to reflect the changes
                            dropDownSync.Update();
                        }
                        catch (System.Exception ex)
                        {
                            // Log the error but don't crash the mod
                            UnityEngine.Debug.LogWarning($"[CabbyCodes] Error refreshing file list: {ex.Message}");
                        }
                    }
                }
            }
        }
    }
} 