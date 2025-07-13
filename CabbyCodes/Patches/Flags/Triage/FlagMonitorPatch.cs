using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using HarmonyLib;
using CabbyMenu.UI.CheatPanels;

namespace CabbyCodes.Patches.Flags.Triage
{
    public class FlagMonitorPatch
    {
        public static GameObject notificationPanel;
        public static Text notificationText;
        private static Queue<string> notificationQueue = new Queue<string>();
        private static FlagMonitorReference monitorReference = FlagMonitorReference.Instance;
        private static FlagFileLoggingReference fileLoggingReference = FlagFileLoggingReference.Instance;
        private const int MAX_NOTIFICATIONS = 100; // Limit to prevent memory issues

        [HarmonyPatch(typeof(SceneData), "SaveMyState", typeof(PersistentBoolData))]
        public static class PersistentBoolDataMonitor
        {
            static void Postfix(PersistentBoolData persistentBoolData)
            {
                if (!monitorReference.IsEnabled) return;
                
                string notification = $"[{persistentBoolData.sceneName}]: {persistentBoolData.id} = {persistentBoolData.activated}";
                AddNotification(notification);
            }
        }

        [HarmonyPatch(typeof(SceneData), "SaveMyState", typeof(PersistentIntData))]
        public static class PersistentIntDataMonitor
        {
            static void Postfix(PersistentIntData persistentIntData)
            {
                if (!monitorReference.IsEnabled) return;
                
                string notification = $"[{persistentIntData.sceneName}]: {persistentIntData.id} = {persistentIntData.value}";
                AddNotification(notification);
            }
        }

        [HarmonyPatch(typeof(SceneData), "SaveMyState", typeof(GeoRockData))]
        public static class GeoRockDataMonitor
        {
            static void Postfix(GeoRockData geoRockData)
            {
                if (!monitorReference.IsEnabled) return;
                
                string notification = $"[{geoRockData.sceneName}]: {geoRockData.id} = GeoRock";
                AddNotification(notification);
            }
        }

        [HarmonyPatch(typeof(PlayerData), "SetBool")]
        public static class PlayerDataBoolMonitor
        {
            static void Postfix(string boolName, bool value)
            {
                if (!monitorReference.IsEnabled) return;
                
                string notification = $"[PlayerData]: {boolName} = {value}";
                AddNotification(notification);
            }
        }

        [HarmonyPatch(typeof(PlayerData), "SetInt")]
        public static class PlayerDataIntMonitor
        {
            static void Postfix(string intName, int value)
            {
                if (!monitorReference.IsEnabled) return;
                
                string notification = $"[PlayerData]: {intName} = {value}";
                AddNotification(notification);
            }
        }

        // Static initialization flag to prevent multiple registrations
        private static bool sceneEventsRegistered = false;

        /// <summary>
        /// Initialize scene change monitoring. Should be called from the mod's Start method.
        /// </summary>
        public static void InitializeSceneMonitoring()
        {
            if (sceneEventsRegistered) return;
            
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnActiveSceneChanged;
            
            sceneEventsRegistered = true;
        }

        private static void OnActiveSceneChanged(Scene oldScene, Scene newScene)
        {
            if (!monitorReference.IsEnabled) return;
            
            string notification = $"[SceneChange]: {oldScene.name} -> {newScene.name}";
            AddNotification(notification);
        }

        private static void AddNotification(string message)
        {
            // Log to console
            Debug.Log($"[Flag Monitor] {message}");
            
            // Log to file if enabled
            fileLoggingReference.LogMessage(message);
            
            // Only queue notifications if the monitor is enabled
            if (monitorReference.IsEnabled)
            {
                notificationQueue.Enqueue(message);
                
                // Limit queue size to prevent memory issues
                while (notificationQueue.Count > MAX_NOTIFICATIONS)
                {
                    notificationQueue.Dequeue();
                }
                
                UpdateNotificationDisplay();
            }
        }

        public static void UpdateNotificationDisplay()
        {
            if (notificationPanel == null) return;
            
            int count = notificationQueue.Count;
            string displayText = $"Flag Monitor Active - Total Notifications: {count}\n\n";
            
            foreach (string notification in notificationQueue)
            {
                displayText += notification + "\n";
            }
            
            notificationText.text = displayText;
            
            // Force layout rebuild like CabbyMainMenu does
            if (notificationText != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(notificationText.GetComponent<RectTransform>());
                LayoutRebuilder.ForceRebuildLayoutImmediate(notificationText.transform.parent.GetComponent<RectTransform>());
            }
            
            // Force canvas update
            Canvas.ForceUpdateCanvases();
            
            // Ensure background transparency is maintained based on hover state
            FlagMonitorReference.EnsureBackgroundTransparency();
            
            // Get scroll rect for auto-scrolling
            ScrollRect scrollRect = notificationPanel.GetComponentInChildren<ScrollRect>();
            
            // Auto-scroll to bottom to show latest notifications
            if (scrollRect != null && notificationPanel.activeSelf)
            {
                // Use a coroutine to ensure the layout is fully updated before scrolling
                FlagMonitorMonoBehaviour monoBehaviour = notificationPanel.GetComponent<FlagMonitorMonoBehaviour>();
                monoBehaviour?.StartCoroutine(ScrollToBottomAfterLayout(scrollRect));
            }
        }
        
        private static System.Collections.IEnumerator ScrollToBottomAfterLayout(ScrollRect scrollRect)
        {
            // Wait for the end of frame to ensure layout is complete
            yield return new WaitForEndOfFrame();
            
            // Force another canvas update to ensure content size is calculated
            Canvas.ForceUpdateCanvases();
            
            // Scroll to bottom (0 = bottom, 1 = top)
            scrollRect.verticalNormalizedPosition = 0f;
        }

        public static void ClearNotifications()
        {
            notificationQueue.Clear();
            if (notificationText != null)
            {
                notificationText.text = "Flag Monitor Active - Total Notifications: 0\n\nNotifications cleared.";
                
                // Force layout rebuild like CabbyMainMenu does
                LayoutRebuilder.ForceRebuildLayoutImmediate(notificationText.GetComponent<RectTransform>());
                LayoutRebuilder.ForceRebuildLayoutImmediate(notificationText.transform.parent.GetComponent<RectTransform>());
            }
            
            // Force canvas update
            Canvas.ForceUpdateCanvases();
            
            // Ensure background transparency is maintained based on hover state
            FlagMonitorReference.EnsureBackgroundTransparency();
            
            // Get scroll rect and reset to bottom
            ScrollRect scrollRect = notificationPanel?.GetComponentInChildren<ScrollRect>();
            if (scrollRect != null && notificationPanel.activeSelf)
            {
                // Use a coroutine to ensure the layout is fully updated before scrolling
                FlagMonitorMonoBehaviour monoBehaviour = notificationPanel.GetComponent<FlagMonitorMonoBehaviour>();
                monoBehaviour?.StartCoroutine(ScrollToBottomAfterLayout(scrollRect));
            }
        }

        public static bool IsEnabled()
        {
            return monitorReference.IsEnabled;
        }

        public static void AddNotificationDirect(string message)
        {
            AddNotification(message);
        }

        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Flag Monitor").SetColor(CheatPanel.headerColor));
            
            var monitorToggle = new TogglePanel(monitorReference, "Enable real-time flag change notifications on screen");
            
            var clearButton = new ButtonPanel(() =>
            {
                ClearNotifications();
            }, "Clear Notifications", "Clear all current flag change notifications");
            
            var fileLogToggle = new TogglePanel(fileLoggingReference, "Enable logging flag changes to a file in CabbySaves folder");
            
            var testButton = new ButtonPanel(() =>
            {
                TestFlagNotifications();
            }, "Test Flag Notifications", "Print example flag change messages to test the monitor display");
            
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(monitorToggle);
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(clearButton);
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(fileLogToggle);
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(testButton);
        }

        private static int testCounter = 0;
        
        public static void TestFlagNotifications()
        {
            if (!monitorReference.IsEnabled)
            {
                Debug.Log("[Flag Monitor] Test notifications skipped - monitor is not enabled");
                return;
            }
            
            testCounter++;
            
            // Test messages in the same format as the actual patches
            AddNotification($"[TestScene]: test_flag_{testCounter} = true");
            AddNotification($"[TestScene]: test_int_{testCounter} = {testCounter}");
            AddNotification($"[TestScene]: test_georock_{testCounter} = GeoRock");
            AddNotification($"[PlayerData]: test_player_bool_{testCounter} = true");
            AddNotification($"[PlayerData]: test_player_int_{testCounter} = {testCounter * 100}");
            
            // Test scene change notification
            AddNotification("[SceneChange]: oldscene -> newscene");
        }
    }
} 