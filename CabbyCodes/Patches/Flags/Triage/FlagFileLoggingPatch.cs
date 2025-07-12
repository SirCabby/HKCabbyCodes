using CabbyMenu.SyncedReferences;
using UnityEngine;
using System.IO;

namespace CabbyCodes.Patches.Flags.Triage
{
    public class FlagFileLoggingReference : ISyncedReference<bool>
    {
        private static FlagFileLoggingReference instance;
        public static FlagFileLoggingReference Instance => instance ?? (instance = new FlagFileLoggingReference());

        private bool isEnabled = false;
        private string logFilePath;

        public bool IsEnabled => isEnabled;
        public string LogFilePath => logFilePath;

        public bool Get()
        {
            return isEnabled;
        }

        public void Set(bool newValue)
        {
            isEnabled = newValue;
            if (isEnabled)
            {
                // Create log file in CabbySaves folder
                string savesPath = Path.Combine(Application.dataPath, "..", "CabbySaves");
                if (!Directory.Exists(savesPath))
                {
                    Directory.CreateDirectory(savesPath);
                }
                string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                logFilePath = Path.Combine(savesPath, $"flag_monitor_{timestamp}.log");
                // Write header to log file
                string header = $"Flag Monitor Log - Started at {System.DateTime.Now}\n" +
                               "==========================================\n";
                File.WriteAllText(logFilePath, header);
                Debug.Log($"[Flag Monitor] File logging enabled: {logFilePath}");
            }
            else
            {
                Debug.Log("[Flag Monitor] File logging disabled");
                logFilePath = null;
            }
        }

        public void LogMessage(string message)
        {
            if (!isEnabled || string.IsNullOrEmpty(logFilePath)) return;
            try
            {
                string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                string logEntry = $"[{timestamp}] {message}";
                File.AppendAllText(logFilePath, logEntry + System.Environment.NewLine);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[Flag Monitor] Failed to write to log file: {ex.Message}");
            }
        }
    }
} 