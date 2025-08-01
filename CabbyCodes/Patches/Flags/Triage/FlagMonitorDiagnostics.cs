using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using CabbyCodes.Flags;

namespace CabbyCodes.Patches.Flags.Triage
{
    /// <summary>
    /// Diagnostic tools for flag monitoring to help identify issues with flag tracking.
    /// </summary>
    public class FlagMonitorDiagnostics
    {
        private static readonly Dictionary<string, int> fieldAccessCounts = new Dictionary<string, int>();
        private static readonly Dictionary<string, int> fieldChangeCounts = new Dictionary<string, int>();
        private static readonly List<string> missedFields = new List<string>();
        private static readonly List<string> errorFields = new List<string>();
        
        private static bool diagnosticsEnabled = false;
        private static float lastDiagnosticTime = 0f;
        private static readonly float diagnosticInterval = 30f; // Log diagnostics every 30 seconds

        /// <summary>
        /// Enable or disable diagnostic logging
        /// </summary>
        public static bool DiagnosticsEnabled
        {
            get => diagnosticsEnabled;
            set
            {
                diagnosticsEnabled = value;
                if (value)
                {
                    Debug.Log("[Flag Monitor Diagnostics] Diagnostics enabled");
                    ResetDiagnostics();
                }
                else
                {
                    Debug.Log("[Flag Monitor Diagnostics] Diagnostics disabled");
                }
            }
        }

        /// <summary>
        /// Reset all diagnostic counters
        /// </summary>
        public static void ResetDiagnostics()
        {
            fieldAccessCounts.Clear();
            fieldChangeCounts.Clear();
            missedFields.Clear();
            errorFields.Clear();
            lastDiagnosticTime = 0f;
            Debug.Log("[Flag Monitor Diagnostics] Diagnostic counters reset");
        }

        /// <summary>
        /// Record a field access for diagnostics
        /// </summary>
        public static void RecordFieldAccess(string fieldName)
        {
            if (!diagnosticsEnabled) return;
            
            if (!fieldAccessCounts.ContainsKey(fieldName))
            {
                fieldAccessCounts[fieldName] = 0;
            }
            fieldAccessCounts[fieldName]++;
        }

        /// <summary>
        /// Record a field change for diagnostics
        /// </summary>
        public static void RecordFieldChange(string fieldName)
        {
            if (!diagnosticsEnabled) return;
            
            if (!fieldChangeCounts.ContainsKey(fieldName))
            {
                fieldChangeCounts[fieldName] = 0;
            }
            fieldChangeCounts[fieldName]++;
        }

        /// <summary>
        /// Record a missed field (field that should be tracked but isn't)
        /// </summary>
        public static void RecordMissedField(string fieldName)
        {
            if (!diagnosticsEnabled) return;
            
            if (!missedFields.Contains(fieldName))
            {
                missedFields.Add(fieldName);
                Debug.LogWarning($"[Flag Monitor Diagnostics] Missed field detected: {fieldName}");
            }
        }

        /// <summary>
        /// Record a field access error
        /// </summary>
        public static void RecordFieldError(string fieldName, string error)
        {
            if (!diagnosticsEnabled) return;
            
            string errorKey = $"{fieldName}: {error}";
            if (!errorFields.Contains(errorKey))
            {
                errorFields.Add(errorKey);
                Debug.LogError($"[Flag Monitor Diagnostics] Field access error: {fieldName} - {error}");
            }
        }

        /// <summary>
        /// Check if diagnostics should be logged based on time interval
        /// </summary>
        public static bool ShouldLogDiagnostics()
        {
            if (!diagnosticsEnabled) return false;
            
            float currentTime = Time.time;
            if (currentTime - lastDiagnosticTime >= diagnosticInterval)
            {
                lastDiagnosticTime = currentTime;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Log comprehensive diagnostic information
        /// </summary>
        public static void LogDiagnostics()
        {
            if (!diagnosticsEnabled) return;

            Debug.Log("=== Flag Monitor Diagnostics ===");
            
            // Log field access statistics
            Debug.Log($"Total fields accessed: {fieldAccessCounts.Count}");
            foreach (var kvp in fieldAccessCounts)
            {
                fieldChangeCounts.TryGetValue(kvp.Key, out int changeCount);
                Debug.Log($"  {kvp.Key}: {kvp.Value} accesses, {changeCount} changes");
            }
            
            // Log missed fields
            if (missedFields.Count > 0)
            {
                Debug.Log($"Missed fields ({missedFields.Count}):");
                foreach (string field in missedFields)
                {
                    Debug.Log($"  {field}");
                }
            }
            
            // Log error fields
            if (errorFields.Count > 0)
            {
                Debug.Log($"Field errors ({errorFields.Count}):");
                foreach (string error in errorFields)
                {
                    Debug.Log($"  {error}");
                }
            }
            
            // Log current state
            Debug.Log($"Current PlayerData instance: {(PlayerData.instance != null ? "Valid" : "Null")}");
            Debug.Log($"Current SceneData instance: {(SceneData.instance != null ? "Valid" : "Null")}");
            Debug.Log($"Current scene: {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
            
            Debug.Log("=== End Diagnostics ===");
        }

        /// <summary>
        /// Analyze FlagInstances to find potential tracking gaps
        /// </summary>
        public static void AnalyzeFlagInstances()
        {
            if (!diagnosticsEnabled) return;

            Debug.Log("=== FlagInstances Analysis ===");
            
            var flagInstancesType = typeof(FlagInstances);
            var fields = flagInstancesType.GetFields(BindingFlags.Public | BindingFlags.Static);
            
            int totalFlags = 0;
            int playerDataFlags = 0;
            int sceneDataFlags = 0;
            int unknownFlags = 0;
            
            foreach (var field in fields)
            {
                if (field.FieldType == typeof(FlagDef))
                {
                    totalFlags++;
                    var flagDef = (FlagDef)field.GetValue(null);
                    if (flagDef != null)
                    {
                        switch (flagDef.Type)
                        {
                            case "PlayerData_Bool":
                            case "PlayerData_Int":
                                playerDataFlags++;
                                break;
                            case "PersistentBoolData":
                            case "PersistentIntData":
                            case "GeoRockData":
                                sceneDataFlags++;
                                break;
                            default:
                                unknownFlags++;
                                Debug.LogWarning($"Unknown flag type: {flagDef.Type} for {flagDef.Id}");
                                break;
                        }
                    }
                }
            }
            
            Debug.Log($"Total flags: {totalFlags}");
            Debug.Log($"PlayerData flags: {playerDataFlags}");
            Debug.Log($"SceneData flags: {sceneDataFlags}");
            Debug.Log($"Unknown type flags: {unknownFlags}");
            Debug.Log("=== End Analysis ===");
        }

        /// <summary>
        /// Check for fields in PlayerData that might be missing from tracking
        /// </summary>
        public static void CheckMissingPlayerDataFields()
        {
            if (!diagnosticsEnabled) return;

            Debug.Log("=== Missing PlayerData Fields Check ===");
            
            var playerDataType = typeof(PlayerData);
            var fields = playerDataType.GetFields(BindingFlags.Public | BindingFlags.Instance);
            
            var trackedFields = new HashSet<string>();
            var flagInstancesType = typeof(FlagInstances);
            var flagFields = flagInstancesType.GetFields(BindingFlags.Public | BindingFlags.Static);
            
            // Build list of tracked fields
            foreach (var field in flagFields)
            {
                if (field.FieldType == typeof(FlagDef))
                {
                    var flagDef = (FlagDef)field.GetValue(null);
                    if (flagDef != null && (flagDef.Type == "PlayerData_Bool" || flagDef.Type == "PlayerData_Int"))
                    {
                        trackedFields.Add(flagDef.Id);
                    }
                }
            }
            
            // Check for potentially missing fields
            foreach (var field in fields)
            {
                if (field.FieldType == typeof(bool) || field.FieldType == typeof(int))
                {
                    if (!trackedFields.Contains(field.Name))
                    {
                        Debug.LogWarning($"Potentially untracked PlayerData field: {field.Name} ({field.FieldType.Name})");
                    }
                }
            }
            
            Debug.Log("=== End Missing Fields Check ===");
        }

        /// <summary>
        /// Generate a comprehensive diagnostic report
        /// </summary>
        public static void GenerateDiagnosticReport()
        {
            if (!diagnosticsEnabled) return;

            LogDiagnostics();
            AnalyzeFlagInstances();
            CheckMissingPlayerDataFields();
        }
    }
} 