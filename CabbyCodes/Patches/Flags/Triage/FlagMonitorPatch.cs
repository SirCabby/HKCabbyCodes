using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using CabbyMenu.UI.CheatPanels;
using System.Reflection;
using System.Collections;
using CabbyCodes.Flags;
using System;

namespace CabbyCodes.Patches.Flags.Triage
{
    public class FlagMonitorPatch
    {
        public static GameObject notificationPanel;
        public static Text notificationText;
        private static readonly Queue<string> notificationQueue = new Queue<string>();
        private static readonly FlagMonitorReference monitorReference = FlagMonitorReference.Instance;
        private static readonly FlagFileLoggingReference fileLoggingReference = FlagFileLoggingReference.Instance;
        
        private static bool patchesApplied = false;

        // Static initialization flag to prevent multiple registrations
        private static bool sceneEventsRegistered = false;

        // Tracked fields from FlagInstances
        private static readonly HashSet<string> trackedPlayerDataFields = new HashSet<string>();
        private static readonly HashSet<string> trackedSceneDataFields = new HashSet<string>();

        // Fields to ignore during polling (noisy or not useful) that are not already commented out
        private static readonly HashSet<string> ignoredFields = new HashSet<string>
        {
            "atBench",
            "currentArea",
            "damagedBlue",
            "disablePause",
            "environmentType",
            "hazardRespawnFacingRight",
            "health",
            "healthBlue",
            "isFirstGame",
            "isInvincible",
            "geo",
            #region Kills
                "killedAbyssCrawler",
                "killedAbyssTendril",
                "killedAcidFlyer",
                "killedAcidWalker",
                "killedAngryBuzzer",
                "killedBabyCentipede",
                "killedBeamMiner",
                "killedBeeHatchling",
                "killedBeeStinger",
                "killedBigBee",
                "killedBigBuzzer",
                "killedBigCentipede",
                "killedBigFly",
                "killedBindingSeal",
                "killedBlackKnight",
                "killedBlobFlyer",
                "killedBlobble",
                "killedBlocker",
                "killedBlowFly",
                "killedBouncer",
                "killedBurstingBouncer",
                "killedBurstingZombie",
                "killedBuzzer",
                "killedCeilingDropper",
                "killedCentipedeHatcher",
                "killedClimber",
                "killedColFlyingSentry",
                "killedColHopper",
                "killedColMiner",
                "killedColMosquito",
                "killedColRoller",
                "killedColShield",
                "killedColWorm",
                "killedCrawler",
                "killedCrystalCrawler",
                "killedCrystalFlyer",
                "killedDreamGuard",
                "killedDummy",
                "killedDungDefender",
                "killedEggSac",
                "killedElectricMage",
                "killedFalseKnight",
                "killedFatFluke",
                "killedFinalBoss",
                "killedFlameBearerLarge",
                "killedFlameBearerMed",
                "killedFlameBearerSmall",
                "killedFlipHopper",
                "killedFlukeMother",
                "killedFlukefly",
                "killedFlukeman",
                "killedFlyingSentryJavelin",
                "killedFlyingSentrySword",
                "killedFungCrawler",
                "killedFungifiedZombie",
                "killedFungoonBaby",
                "killedFungusFlyer",
                "killedGardenZombie",
                "killedGhostAladar",
                "killedGhostGalien",
                "killedGhostHu",
                "killedGhostMarkoth",
                "killedGhostMarmu",
                "killedGhostNoEyes",
                "killedGhostXero",
                "killedGiantHopper",
                "killedGodseekerMask",
                "killedGorgeousHusk",
                "killedGrassHopper",
                "killedGreatShieldZombie",
                "killedGreyPrince",
                "killedGrimm",
                "killedGrubMimic",
                "killedHatcher",
                "killedHatchling",
                "killedHealthScuttler",
                "killedHeavyMantis",
                "killedHiveKnight",
                "killedHollowKnight",
                "killedHollowKnightPrime",
                "killedHopper",
                "killedHornet",
                "killedHunterMark",
                "killedInfectedKnight",
                "killedInflater",
                "killedJarCollector",
                "killedJellyCrawler",
                "killedJellyfish",
                "killedLaserBug",
                "killedLazyFlyer",
                "killedLesserMawlek",
                "killedLobsterLancer",
                "killedMage",
                "killedMageBalloon",
                "killedMageBlob",
                "killedMageKnight",
                "killedMageLord",
                "killedMantis",
                "killedMantisFlyerChild",
                "killedMantisHeavyFlyer",
                "killedMantisLord",
                "killedMawlek",
                "killedMawlekTurret",
                "killedMegaBeamMiner",
                "killedMegaJellyfish",
                "killedMegaMossCharger",
                "killedMenderBug",
                "killedMimicSpider",
                "killedMinesCrawler",
                "killedMiniSpider",
                "killedMosquito",
                "killedMossCharger",
                "killedMossFlyer",
                "killedMossKnight",
                "killedMossKnightFat",
                "killedMossWalker",
                "killedMossmanRunner",
                "killedMossmanShaker",
                "killedMummy",
                "killedMushroomBaby",
                "killedMushroomBrawler",
                "killedMushroomRoller",
                "killedMushroomTurret",
                "killedNailBros",
                "killedNailsage",
                "killedNightmareGrimm",
                "killedOblobble",
                "killedOrangeBalloon",
                "killedOrangeScuttler",
                "killedPaintmaster",
                "killedPalaceFly",
                "killedPaleLurker",
                "killedPigeon",
                "killedPlantShooter",
                "killedPrayerSlug",
                "killedRoller",
                "killedRoyalCoward",
                "killedRoyalDandy",
                "killedRoyalGuard",
                "killedRoyalPlumper",
                "killedSentry",
                "killedSentryFat",
                "killedShootSpider",
                "killedSibling",
                "killedSlashSpider",
                "killedSnapperTrap",
                "killedSpiderCorpse",
                "killedSpiderFlyer",
                "killedSpitter",
                "killedSpittingZombie",
                "killedSuperSpitter",
                "killedTraitorLord",
                "killedVoidIdol_1",
                "killedVoidIdol_2",
                "killedVoidIdol_3",
                "killedWhiteDefender",
                "killedWhiteRoyal",
                "killedWorm",
                "killedZapBug",
                "killedZombieBarger",
                "killedZombieGuard",
                "killedZombieHive",
                "killedZombieHornhead",
                "killedZombieLeaper",
                "killedZombieMiner",
                "killedZombieRunner",
                "killedZombieShield",
                "killedZote",
                "killedZotelingBalloon",
                "killedZotelingBuzzer",
                "killedZotelingHopper",
                "killsAbyssCrawler",
                "killsAbyssTendril",
                "killsAcidFlyer",
                "killsAcidWalker",
                "killsAngryBuzzer",
                "killsBabyCentipede",
                "killsBeamMiner",
                "killsBeeHatchling",
                "killsBeeStinger",
                "killsBigBee",
                "killsBigBuzzer",
                "killsBigCentipede",
                "killsBigFly",
                "killsBindingSeal",
                "killsBlackKnight",
                "killsBlobFlyer",
                "killsBlobble",
                "killsBlocker",
                "killsBlowFly",
                "killsBouncer",
                "killsBurstingBouncer",
                "killsBurstingZombie",
                "killsBuzzer",
                "killsCeilingDropper",
                "killsCentipedeHatcher",
                "killsClimber",
                "killsColFlyingSentry",
                "killsColHopper",
                "killsColMiner",
                "killsColMosquito",
                "killsColRoller",
                "killsColShield",
                "killsColWorm",
                "killsCrawler",
                "killsCrystalCrawler",
                "killsCrystalFlyer",
                "killsDreamGuard",
                "killsDummy",
                "killsDungDefender",
                "killsEggSac",
                "killsElectricMage",
                "killsFalseKnight",
                "killsFatFluke",
                "killsFinalBoss",
                "killsFlameBearerLarge",
                "killsFlameBearerMed",
                "killsFlameBearerSmall",
                "killsFlipHopper",
                "killsFlukeMother",
                "killsFlukefly",
                "killsFlukeman",
                "killsFlyingSentryJavelin",
                "killsFlyingSentrySword",
                "killsFungCrawler",
                "killsFungifiedZombie",
                "killsFungoonBaby",
                "killsFungusFlyer",
                "killsGardenZombie",
                "killsGhostAladar",
                "killsGhostGalien",
                "killsGhostHu",
                "killsGhostMarkoth",
                "killsGhostMarmu",
                "killsGhostNoEyes",
                "killsGhostXero",
                "killsGiantHopper",
                "killsGodseekerMask",
                "killsGorgeousHusk",
                "killsGrassHopper",
                "killsGreatShieldZombie",
                "killsGreyPrince",
                "killsGrimm",
                "killsGrubMimic",
                "killsHatcher",
                "killsHatchling",
                "killsHealthScuttler",
                "killsHeavyMantis",
                "killsHiveKnight",
                "killsHollowKnight",
                "killsHollowKnightPrime",
                "killsHopper",
                "killsHornet",
                "killsHunterMark",
                "killsInfectedKnight",
                "killsInflater",
                "killsJarCollector",
                "killsJellyCrawler",
                "killsJellyfish",
                "killsLaserBug",
                "killsLazyFlyer",
                "killsLesserMawlek",
                "killsLobsterLancer",
                "killsMage",
                "killsMageBalloon",
                "killsMageBlob",
                "killsMageKnight",
                "killsMageLord",
                "killsMantis",
                "killsMantisFlyerChild",
                "killsMantisHeavyFlyer",
                "killsMantisLord",
                "killsMawlek",
                "killsMawlekTurret",
                "killsMegaBeamMiner",
                "killsMegaJellyfish",
                "killsMegaMossCharger",
                "killsMenderBug",
                "killsMimicSpider",
                "killsMinesCrawler",
                "killsMiniSpider",
                "killsMosquito",
                "killsMossCharger",
                "killsMossFlyer",
                "killsMossKnight",
                "killsMossKnightFat",
                "killsMossWalker",
                "killsMossmanRunner",
                "killsMossmanShaker",
                "killsMummy",
                "killsMushroomBaby",
                "killsMushroomBrawler",
                "killsMushroomRoller",
                "killsMushroomTurret",
                "killsNailBros",
                "killsNailsage",
                "killsNightmareGrimm",
                "killsOblobble",
                "killsOrangeBalloon",
                "killsOrangeScuttler",
                "killsPaintmaster",
                "killsPalaceFly",
                "killsPaleLurker",
                "killsPigeon",
                "killsPlantShooter",
                "killsPrayerSlug",
                "killsRoller",
                "killsRoyalCoward",
                "killsRoyalDandy",
                "killsRoyalGuard",
                "killsRoyalPlumper",
                "killsSentry",
                "killsSentryFat",
                "killsShootSpider",
                "killsSibling",
                "killsSlashSpider",
                "killsSnapperTrap",
                "killsSpiderCorpse",
                "killsSpiderFlyer",
                "killsSpitter",
                "killsSpittingZombie",
                "killsSuperSpitter",
                "killsTraitorLord",
                "killsVoidIdol_1",
                "killsVoidIdol_2",
                "killsVoidIdol_3",
                "killsWhiteDefender",
                "killsWhiteRoyal",
                "killsWorm",
                "killsZapBug",
                "killsZombieBarger",
                "killsZombieGuard",
                "killsZombieHive",
                "killsZombieHornhead",
                "killsZombieLeaper",
                "killsZombieMiner",
                "killsZombieRunner",
                "killsZombieShield",
                "killsZote",
                "killsZotelingBalloon",
                "killsZotelingBuzzer",
                "killsZotelingHopper",
            #endregion
            "MPCharge",
            "profileID"
        };

        // --- Performance optimizations ---
        private static readonly Dictionary<string, (FieldInfo fieldInfo, Type fieldType)> fieldInfoCache = new Dictionary<string, (FieldInfo, Type)>();
        private static PlayerData lastKnownInstance = null;

        // --- Polling state ---
        private static readonly Dictionary<string, object> previousPlayerDataValues = new Dictionary<string, object>();
        private static bool pollingStarted = false;
        private static bool hasLeftTitleScreen = false;

        // --- Scene data polling state ---
        private static readonly Dictionary<string, object> previousSceneDataValues = new Dictionary<string, object>();
        private static SceneData lastKnownSceneDataInstance = null;

        /// <summary>
        /// Initialize flag monitoring. Should be called from the mod's Start method.
        /// </summary>
        public static void ApplyPatches()
        {
            if (patchesApplied) return;
            
            try
            {
                ExtractTrackedFields();
                InitializeSceneMonitoring();
                
                patchesApplied = true;
                
                StartPolling();
            }
            catch (Exception ex)
            {
                Debug.LogError($"[Flag Monitor] Failed to initialize flag monitoring: {ex.Message}");
            }
        }

        /// <summary>
        /// Extract all field names from FlagInstances that need to be monitored
        /// </summary>
        private static void ExtractTrackedFields()
        {
            try
            {
                var flagInstancesType = typeof(FlagInstances);
                var fields = flagInstancesType.GetFields(BindingFlags.Public | BindingFlags.Static);
                
                foreach (var field in fields)
                {
                    if (field.FieldType == typeof(FlagDef))
                    {
                        var flagDef = (FlagDef)field.GetValue(null);
                        if (flagDef != null)
                        {
                            if (flagDef.Type == "PlayerData_Bool" || flagDef.Type == "PlayerData_Int")
                            {
                                trackedPlayerDataFields.Add(flagDef.Id);
                            }
                            else if (flagDef.Type == "PersistentBoolData" || flagDef.Type == "PersistentIntData" || flagDef.Type == "GeoRockData")
                            {
                                trackedSceneDataFields.Add(flagDef.Id);
                            }
                        }
                    }
                }
                
                // Remove ignored fields from tracking set
                foreach (var ignoredField in ignoredFields)
                {
                    trackedPlayerDataFields.Remove(ignoredField);
                }
                
                // Build FieldInfo cache for performance
                BuildFieldInfoCache();
            }
            catch (Exception ex)
            {
                Debug.LogError($"[Flag Monitor] Failed to extract tracked fields: {ex.Message}");
            }
        }

        /// <summary>
        /// Build FieldInfo cache for all tracked PlayerData fields
        /// </summary>
        private static void BuildFieldInfoCache()
        {
            fieldInfoCache.Clear();
            var playerDataType = typeof(PlayerData);
            
            foreach (var fieldName in trackedPlayerDataFields)
            {
                var fieldInfo = playerDataType.GetField(fieldName);
                if (fieldInfo != null)
                {
                    fieldInfoCache[fieldName] = (fieldInfo, fieldInfo.FieldType);
                }
                else
                {
                    Debug.LogWarning($"[Flag Monitor] Could not find field '{fieldName}' in PlayerData");
                }
            }
        }

        /// <summary>
        /// Handle PlayerData instance changes (new save file, etc.)
        /// </summary>
        private static void HandleInstanceChange()
        {
            lastKnownInstance = PlayerData.instance;
            previousPlayerDataValues.Clear(); // Reset previous values for new instance
        }

        /// <summary>
        /// Handle SceneData instance changes (new save file, etc.)
        /// </summary>
        private static void HandleSceneDataInstanceChange()
        {
            lastKnownSceneDataInstance = SceneData.instance;
            previousSceneDataValues.Clear(); // Reset previous values for new instance
        }

        /// <summary>
        /// Rebuild FieldInfo cache if needed
        /// </summary>
        private static void RebuildFieldInfoCache()
        {
            BuildFieldInfoCache();
        }

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
            // Track when we leave the title screen for the first time
            if (oldScene.name == "Menu_Title" && newScene.name != "Menu_Title")
            {
                hasLeftTitleScreen = true;
                Debug.Log("[Flag Monitor] Left title screen, flag monitoring will now be active");
            }
            
            if (!monitorReference.IsEnabled) return;
            
            string notification = $"[SceneChange]: {oldScene.name} -> {newScene.name}";
            AddNotification(notification);
        }

        private static void AddNotification(string message)
        {
            // Log to file if enabled
            fileLoggingReference.LogMessage(message);
            
            // Only queue notifications if the monitor is enabled
            if (monitorReference.IsEnabled)
            {
                notificationQueue.Enqueue(message);
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
            
            // Force layout rebuild
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
        
        private static IEnumerator ScrollToBottomAfterLayout(ScrollRect scrollRect)
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
            
            // Test messages in the same format as the actual monitoring
            AddNotification($"[SceneChange]: TestScene1 -> TestScene2");
            AddNotification($"[PlayerData]: test_field_{testCounter} = true");
            AddNotification($"[PlayerData]: test_int_{testCounter} = {testCounter * 100}");
            AddNotification($"[Scene: TestScene] test_scene_flag_{testCounter} = true");
            
            // Test actual PlayerData field changes
            if (PlayerData.instance != null)
            {
                Debug.Log("[Flag Monitor] Testing actual PlayerData field changes...");
                PlayerData.instance.geo = PlayerData.instance.geo + 1;
                PlayerData.instance.hasDash = !PlayerData.instance.hasDash;
            }
        }

        /// <summary>
        /// Start polling for PlayerData and SceneData field changes every 0.25s. Should be called from mod Start.
        /// </summary>
        public static void StartPolling()
        {
            if (pollingStarted) return;
            pollingStarted = true;
            // Find or create a MonoBehaviour to run the coroutine
            FlagMonitorMonoBehaviour.EnsureInstance();
            FlagMonitorMonoBehaviour.Instance.StartCoroutine(PollDataFields());
        }

        private static IEnumerator PollDataFields()
        {
            while (true)
            {
                // Only poll if we've left the title screen and we're not currently in Menu_Title
                if (monitorReference.IsEnabled && 
                    hasLeftTitleScreen && 
                    UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Menu_Title")
                {
                    // Save scene data before polling to ensure we see the latest updates immediately
                    GameManager._instance?.SaveLevelState();

                    // Poll PlayerData fields
                    PollPlayerDataFields();
                    
                    // Poll SceneData fields
                    PollSceneDataFields();
                }

                yield return new WaitForSeconds(0.25f);
            }
        }

        private static void PollPlayerDataFields()
        {
            if (PlayerData.instance == null) return;

            // Check for instance change
            if (PlayerData.instance != lastKnownInstance)
            {
                HandleInstanceChange();
            }

            foreach (var fieldName in trackedPlayerDataFields)
            {
                // Use cached FieldInfo
                if (!fieldInfoCache.TryGetValue(fieldName, out var fieldData))
                {
                    continue; // Field not in cache, skip
                }

                var (fieldInfo, fieldType) = fieldData;

                try
                {
                    object currentValue = fieldInfo.GetValue(PlayerData.instance);
                    previousPlayerDataValues.TryGetValue(fieldName, out object previousValue);

                    // Type-specific comparison for better performance
                    bool valueChanged = false;
                    if (previousValue == null)
                    {
                        // Don't print initial state - just store the current value as baseline
                        // This ensures we only show actual changes that happen during gameplay
                        valueChanged = false;
                        // Store the baseline value
                        previousPlayerDataValues[fieldName] = currentValue;
                    }
                    else if (fieldType == typeof(bool))
                    {
                        valueChanged = (bool)currentValue != (bool)previousValue;
                    }
                    else if (fieldType == typeof(int))
                    {
                        valueChanged = (int)currentValue != (int)previousValue;
                    }
                    else if (fieldType == typeof(float))
                    {
                        valueChanged = !Mathf.Approximately((float)currentValue, (float)previousValue);
                    }
                    else if (fieldType == typeof(string))
                    {
                        valueChanged = !string.Equals((string)currentValue, (string)previousValue);
                    }
                    else
                    {
                        // Fallback to object.Equals for other types
                        valueChanged = !Equals(currentValue, previousValue);
                    }

                    if (valueChanged)
                    {
                        previousPlayerDataValues[fieldName] = currentValue;
                        string notification = $"[PlayerData]: {fieldName} = {currentValue}";
                        AddNotification(notification);
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"[Flag Monitor] Field access failed for '{fieldName}', rebuilding cache: {ex.Message}");
                    RebuildFieldInfoCache();
                    break; // Skip rest of this poll cycle
                }
            }
        }

        private static void PollSceneDataFields()
        {
            if (SceneData.instance == null) return;

            // Check for instance change
            if (SceneData.instance != lastKnownSceneDataInstance)
            {
                HandleSceneDataInstanceChange();
            }

            PollPersistentBoolData();
            PollPersistentIntData();
            PollGeoRockData();
        }

        private static void PollPersistentBoolData()
        {
            if (SceneData.instance.persistentBoolItems == null) return;

            foreach (var pbd in SceneData.instance.persistentBoolItems)
            {
                if (!trackedSceneDataFields.Contains(pbd.id)) continue;

                string key = $"PersistentBool_{pbd.id}_{pbd.sceneName}";
                bool currentValue = pbd.activated;
                previousSceneDataValues.TryGetValue(key, out object previousValue);

                bool valueChanged;
                if (previousValue == null)
                {
                    // Don't print initial state - just store the current value as baseline
                    valueChanged = false;
                    previousSceneDataValues[key] = currentValue;
                }
                else
                {
                    valueChanged = (bool)previousValue != currentValue;
                }

                if (valueChanged)
                {
                    previousSceneDataValues[key] = currentValue;
                    string notification = $"[Scene: {pbd.sceneName}] {pbd.id} = {currentValue}";
                    AddNotification(notification);
                }
            }
        }

        private static void PollPersistentIntData()
        {
            if (SceneData.instance.persistentIntItems == null) return;

            foreach (var pid in SceneData.instance.persistentIntItems)
            {
                if (!trackedSceneDataFields.Contains(pid.id)) continue;

                string key = $"PersistentInt_{pid.id}_{pid.sceneName}";
                int currentValue = pid.value;
                previousSceneDataValues.TryGetValue(key, out object previousValue);

                bool valueChanged;
                if (previousValue == null)
                {
                    // Don't print initial state - just store the current value as baseline
                    valueChanged = false;
                    previousSceneDataValues[key] = currentValue;
                }
                else
                {
                    valueChanged = (int)previousValue != currentValue;
                }

                if (valueChanged)
                {
                    previousSceneDataValues[key] = currentValue;
                    string notification = $"[Scene: {pid.sceneName}] {pid.id} = {currentValue}";
                    AddNotification(notification);
                }
            }
        }

        /// <summary>
        /// Polls GeoRockData for changes and logs notifications when georocks are broken.
        /// GeoRockData uses 'hitsLeft' integer to track state:
        /// - hitsLeft > 0 = Georock is intact
        /// - hitsLeft <= 0 = Georock is broken
        /// </summary>
        private static void PollGeoRockData()
        {
            if (SceneData.instance?.geoRocks == null) return;

            foreach (var grd in SceneData.instance.geoRocks)
            {
                if (!trackedSceneDataFields.Contains(grd.id)) continue;

                string key = $"GeoRock_{grd.id}_{grd.sceneName}";
                int currentHitsLeft = grd.hitsLeft;
                bool currentValue = currentHitsLeft <= 0; // True if broken (no hits left)

                previousSceneDataValues.TryGetValue(key, out object previousValue);

                bool valueChanged;
                if (previousValue == null)
                {
                    // Don't print initial state - just store the current value as baseline
                    valueChanged = false;
                    previousSceneDataValues[key] = currentValue;
                }
                else
                {
                    valueChanged = (bool)previousValue != currentValue;
                }

                if (valueChanged)
                {
                    previousSceneDataValues[key] = currentValue;
                    string status = currentValue ? "Broken" : $"Intact ({currentHitsLeft} hits left)";
                    string notification = $"[Scene: {grd.sceneName}] GeoRock: {grd.id} = {status}";
                    AddNotification(notification);
                }
            }
        }
    }
} 