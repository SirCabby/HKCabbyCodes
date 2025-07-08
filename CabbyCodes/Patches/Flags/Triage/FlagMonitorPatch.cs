using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using HarmonyLib;
using CabbyMenu.UI.CheatPanels;

namespace CabbyCodes.Patches.Flags.Triage
{
    public class FlagMonitorPatch
    {
        private static GameObject notificationPanel;
        private static Text notificationText;
        private static Queue<string> notificationQueue = new Queue<string>();
        private static float lastNotificationTime = 0f;
        private static bool isEnabled = false;
        private static bool logToFile = false;
        private static string logFilePath;

        [HarmonyPatch(typeof(SceneData), "SaveMyState", typeof(PersistentBoolData))]
        public static class PersistentBoolDataMonitor
        {
            static void Postfix(PersistentBoolData persistentBoolData)
            {
                if (!isEnabled) return;
                
                string notification = $"Flag Changed: {persistentBoolData.id} in {persistentBoolData.sceneName} = {persistentBoolData.activated}";
                AddNotification(notification);
            }
        }

        [HarmonyPatch(typeof(SceneData), "SaveMyState", typeof(PersistentIntData))]
        public static class PersistentIntDataMonitor
        {
            static void Postfix(PersistentIntData persistentIntData)
            {
                if (!isEnabled) return;
                
                string notification = $"Flag Changed: {persistentIntData.id} in {persistentIntData.sceneName} = {persistentIntData.value}";
                AddNotification(notification);
            }
        }

        [HarmonyPatch(typeof(SceneData), "SaveMyState", typeof(GeoRockData))]
        public static class GeoRockDataMonitor
        {
            static void Postfix(GeoRockData geoRockData)
            {
                if (!isEnabled) return;
                
                string notification = $"GeoRock Changed: {geoRockData.id} in {geoRockData.sceneName}";
                AddNotification(notification);
            }
        }

        [HarmonyPatch(typeof(PlayerData), "SetBool")]
        public static class PlayerDataBoolMonitor
        {
            static void Postfix(string boolName, bool value)
            {
                if (!isEnabled) return;
                
                string notification = $"PlayerData Bool: {boolName} = {value}";
                AddNotification(notification);
            }
        }

        [HarmonyPatch(typeof(PlayerData), "SetInt")]
        public static class PlayerDataIntMonitor
        {
            static void Postfix(string intName, int value)
            {
                if (!isEnabled) return;
                
                string notification = $"PlayerData Int: {intName} = {value}";
                AddNotification(notification);
            }
        }

        private static void AddNotification(string message)
        {
            if (Time.time - lastNotificationTime < 0.1f) return; // Throttle notifications
            
            // Log to console
            Debug.Log($"[Flag Monitor] {message}");
            
            // Log to file if enabled
            if (logToFile && !string.IsNullOrEmpty(logFilePath))
            {
                try
                {
                    string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    string logEntry = $"[{timestamp}] {message}";
                    System.IO.File.AppendAllText(logFilePath, logEntry + System.Environment.NewLine);
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"[Flag Monitor] Failed to write to log file: {ex.Message}");
                }
            }
            
            notificationQueue.Enqueue(message);
            lastNotificationTime = Time.time;
            
            // Keep only the last 10 notifications
            while (notificationQueue.Count > 10)
            {
                notificationQueue.Dequeue();
            }
            
            UpdateNotificationDisplay();
        }

        private static void UpdateNotificationDisplay()
        {
            if (notificationPanel == null) return;
            
            string displayText = "Flag Monitor Active\n\n";
            foreach (string notification in notificationQueue)
            {
                displayText += notification + "\n";
            }
            
            notificationText.text = displayText;
        }

        public static void CreateNotificationPanel()
        {
            if (notificationPanel != null) return;

            // Create the main panel
            notificationPanel = new GameObject("FlagMonitorPanel");
            Canvas canvas = notificationPanel.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 1000; // Ensure it's on top
            
            // Add CanvasScaler for proper scaling
            CanvasScaler scaler = notificationPanel.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            
            // Create background panel
            GameObject backgroundPanel = new GameObject("Background");
            backgroundPanel.transform.SetParent(notificationPanel.transform, false);
            
            Image backgroundImage = backgroundPanel.AddComponent<Image>();
            backgroundImage.color = new Color(0, 0, 0, 0.8f);
            
            RectTransform backgroundRect = backgroundPanel.GetComponent<RectTransform>();
            backgroundRect.anchorMin = new Vector2(0, 0.7f);
            backgroundRect.anchorMax = new Vector2(0.4f, 1f);
            backgroundRect.offsetMin = Vector2.zero;
            backgroundRect.offsetMax = Vector2.zero;
            
            // Create text component
            GameObject textObject = new GameObject("NotificationText");
            textObject.transform.SetParent(backgroundPanel.transform, false);
            
            notificationText = textObject.AddComponent<Text>();
            notificationText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            notificationText.fontSize = 14;
            notificationText.color = Color.white;
            notificationText.alignment = TextAnchor.UpperLeft;
            
            RectTransform textRect = textObject.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = new Vector2(10, 10);
            textRect.offsetMax = new Vector2(-10, -10);
            
            // Initially hide the panel
            notificationPanel.SetActive(false);
        }

        public static void ToggleMonitor()
        {
            if (notificationPanel == null)
            {
                CreateNotificationPanel();
            }
            
            isEnabled = !isEnabled;
            notificationPanel.SetActive(isEnabled);
            
            if (isEnabled)
            {
                notificationText.text = "Flag Monitor Active\n\nWaiting for flag changes...";
            }
        }

        public static void ClearNotifications()
        {
            notificationQueue.Clear();
            if (notificationText != null)
            {
                notificationText.text = "Flag Monitor Active\n\nNotifications cleared.";
            }
        }

        public static void ToggleFileLogging()
        {
            logToFile = !logToFile;
            
            if (logToFile)
            {
                // Create log file in CabbySaves folder
                string savesPath = System.IO.Path.Combine(Application.dataPath, "..", "CabbySaves");
                if (!System.IO.Directory.Exists(savesPath))
                {
                    System.IO.Directory.CreateDirectory(savesPath);
                }
                
                string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                logFilePath = System.IO.Path.Combine(savesPath, $"flag_monitor_{timestamp}.log");
                
                // Write header to log file
                string header = $"Flag Monitor Log - Started at {System.DateTime.Now}\n" +
                               "==========================================\n";
                System.IO.File.WriteAllText(logFilePath, header);
                
                Debug.Log($"[Flag Monitor] File logging enabled: {logFilePath}");
            }
            else
            {
                Debug.Log("[Flag Monitor] File logging disabled");
                logFilePath = null;
            }
        }

        public static bool IsFileLoggingEnabled()
        {
            return logToFile;
        }

        public static bool IsEnabled()
        {
            return isEnabled;
        }

        public static void AddNotificationDirect(string message)
        {
            AddNotification(message);
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Flag Monitor").SetColor(CheatPanel.headerColor));
            
            var toggleButton = new ButtonPanel(() =>
            {
                ToggleMonitor();
            }, "Toggle Flag Monitor", "Show/hide real-time flag change notifications on screen");
            
            var clearButton = new ButtonPanel(() =>
            {
                ClearNotifications();
            }, "Clear Notifications", "Clear all current flag change notifications");
            
            var fileLogButton = new ButtonPanel(() =>
            {
                ToggleFileLogging();
            }, "Toggle File Logging", "Enable/disable logging flag changes to a file in CabbySaves folder");
            
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(toggleButton);
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(clearButton);
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(fileLogButton);
        }

        // Helper methods for UI-triggered flag changes
        public static void NotifyFlagChange(string flagName, object value, string flagType = "UI")
        {
            if (!isEnabled) return;
            
            string message = $"[{flagType}] {flagName} = {value}";
            AddNotificationDirect(message);
        }
        
        public static void NotifyPlayerDataBool(string flagName, bool value)
        {
            NotifyFlagChange(flagName, value, "PlayerData_Bool");
        }
        
        public static void NotifyPlayerDataInt(string flagName, int value)
        {
            NotifyFlagChange(flagName, value, "PlayerData_Int");
        }
        
        public static void NotifySceneDataBool(string flagName, bool value, string sceneName)
        {
            NotifyFlagChange($"{flagName} ({sceneName})", value, "SceneData_Bool");
        }
        
        public static void NotifySceneDataInt(string flagName, int value, string sceneName)
        {
            NotifyFlagChange($"{flagName} ({sceneName})", value, "SceneData_Int");
        }
    }
} 