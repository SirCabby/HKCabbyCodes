using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using HarmonyLib;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.SyncedReferences;
using CabbyCodes.Patches.Flags.Triage;

namespace CabbyCodes.Patches.Flags.Triage
{
    public class FlagMonitorPatch
    {
        public static GameObject notificationPanel;
        public static Text notificationText;
        private static Queue<string> notificationQueue = new Queue<string>();
        private static FlagMonitorReference monitorReference = FlagMonitorReference.Instance;
        private static FlagFileLoggingReference fileLoggingReference = FlagFileLoggingReference.Instance;

        [HarmonyPatch(typeof(SceneData), "SaveMyState", typeof(PersistentBoolData))]
        public static class PersistentBoolDataMonitor
        {
            static void Postfix(PersistentBoolData persistentBoolData)
            {
                if (!FlagMonitorPatch.monitorReference.IsEnabled) return;
                
                string notification = $"[{persistentBoolData.sceneName}]: {persistentBoolData.id} = {persistentBoolData.activated}";
                AddNotification(notification);
            }
        }

        [HarmonyPatch(typeof(SceneData), "SaveMyState", typeof(PersistentIntData))]
        public static class PersistentIntDataMonitor
        {
            static void Postfix(PersistentIntData persistentIntData)
            {
                if (!FlagMonitorPatch.monitorReference.IsEnabled) return;
                
                string notification = $"[{persistentIntData.sceneName}]: {persistentIntData.id} = {persistentIntData.value}";
                AddNotification(notification);
            }
        }

        [HarmonyPatch(typeof(SceneData), "SaveMyState", typeof(GeoRockData))]
        public static class GeoRockDataMonitor
        {
            static void Postfix(GeoRockData geoRockData)
            {
                if (!FlagMonitorPatch.monitorReference.IsEnabled) return;
                
                string notification = $"[{geoRockData.sceneName}]: {geoRockData.id} = GeoRock";
                AddNotification(notification);
            }
        }

        [HarmonyPatch(typeof(PlayerData), "SetBool")]
        public static class PlayerDataBoolMonitor
        {
            static void Postfix(string boolName, bool value)
            {
                if (!FlagMonitorPatch.monitorReference.IsEnabled) return;
                
                string notification = $"[PlayerData]: {boolName} = {value}";
                AddNotification(notification);
            }
        }

        [HarmonyPatch(typeof(PlayerData), "SetInt")]
        public static class PlayerDataIntMonitor
        {
            static void Postfix(string intName, int value)
            {
                if (!FlagMonitorPatch.monitorReference.IsEnabled) return;
                
                string notification = $"[PlayerData]: {intName} = {value}";
                AddNotification(notification);
            }
        }

        private static void AddNotification(string message)
        {
            // Log to console
            Debug.Log($"[Flag Monitor] {message}");
            
            // Log to file if enabled
            fileLoggingReference.LogMessage(message);
            
            notificationQueue.Enqueue(message);
            
            UpdateNotificationDisplay();
        }

        private static void UpdateNotificationDisplay()
        {
            if (notificationPanel == null) return;
            
            int count = notificationQueue.Count;
            string displayText = $"Flag Monitor Active - Total Notifications: {count}\n\n";
            
            foreach (string notification in notificationQueue)
            {
                displayText += notification + "\n";
            }
            
            notificationText.text = displayText;
            
            // Force layout update for scroll view
            Canvas.ForceUpdateCanvases();
            
            // Auto-scroll to bottom to show latest notifications
            ScrollRect scrollRect = notificationPanel.GetComponentInChildren<ScrollRect>();
            if (scrollRect != null)
            {
                scrollRect.verticalNormalizedPosition = 0f;
            }
        }



        public static void ClearNotifications()
        {
            notificationQueue.Clear();
            if (notificationText != null)
            {
                notificationText.text = "Flag Monitor Active - Total Notifications: 0\n\nNotifications cleared.";
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

        // Helper methods for UI-triggered flag changes
        public static void NotifyFlagChange(string flagName, object value, string flagType = "UI")
        {
            if (!monitorReference.IsEnabled) return;
            
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

        private static int testCounter = 0;
        
        public static void TestFlagNotifications()
        {
            testCounter++;
            
            // Test messages in the same format as the actual patches
            AddNotification($"[TestScene]: test_flag_{testCounter} = true");
            AddNotification($"[TestScene]: test_int_{testCounter} = {testCounter}");
            AddNotification($"[TestScene]: test_georock_{testCounter} = GeoRock");
            AddNotification($"[PlayerData]: test_player_bool_{testCounter} = true");
            AddNotification($"[PlayerData]: test_player_int_{testCounter} = {testCounter * 100}");
            
            Debug.Log($"[Flag Monitor] Test notifications added (counter: {testCounter})");
        }
    }
} 