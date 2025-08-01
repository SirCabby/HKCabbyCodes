using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using CabbyMenu.UI.CheatPanels;
using System.Reflection;
using System.Collections;
using CabbyCodes.Flags;
using System;
using System.IO;

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

        // Fields to ignore during polling (noisy or not useful)
        private static readonly HashSet<string> ignoredFields = new HashSet<string>
        {
            "atBench",
            "currentArea",
            "currentInvPane",
            "damagedBlue",
            "disablePause",
            "environmentType",
            "hazardRespawnFacingRight",
            "health",
            "healthBlue",
            "isFirstGame",
            "isInvincible",
            "geo",
            "lastJournalItem",
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
            "profileID",
            "respawnFacingRight",
            "respawnType"
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

        // --- Dynamic flag discovery ---
        private static readonly HashSet<string> discoveredPlayerDataFlags = new HashSet<string>();
        private static readonly HashSet<string> discoveredSceneFlags = new HashSet<string>();
        private static readonly Dictionary<string, (string sceneName, string type, object value)> newFlagDiscoveries = new Dictionary<string, (string, string, object)>();
        
        // --- Historical discoveries (preserved across sessions) ---
        private static readonly Dictionary<string, (string sceneName, string type, object value)> historicalDiscoveries = new Dictionary<string, (string, string, object)>();

        /// <summary>
        /// Generate a code line for a flag definition
        /// </summary>
        private static string GenerateCodeLine(string flagKey, string sceneName, string flagType)
        {
            if (flagType.StartsWith("PlayerData_"))
            {
                string fieldName = flagKey;
                return $"public static readonly FlagDef {fieldName} = new FlagDef(\"{fieldName}\", \"Global\", false, \"{flagType}\");";
            }
            else
            {
                // Scene flag - flagKey is now in format "flagId|||sceneName"
                string[] parts = flagKey.Split(new string[] { "|||" }, 2, StringSplitOptions.None);
                if (parts.Length >= 2)
                {
                    string flagId = parts[0];
                    
                    // Use the actual sceneName parameter, not the sceneKey from the flagKey
                    string sceneInstanceName = sceneName.Replace("-", "_");
                    string flagIdClean = CleanFieldName(flagId);
                    
                    return $"public static readonly FlagDef {sceneInstanceName}__{flagIdClean} = new FlagDef(\"{flagId}\", SceneInstances.{sceneInstanceName}.SceneName, false, \"{flagType}\");";
                }
                else
                {
                    return $"// Error parsing flag key: {flagKey}";
                }
            }
        }

        /// <summary>
        /// Cleans a flag ID to create a valid C# field name
        /// </summary>
        private static string CleanFieldName(string flagId)
        {
            if (string.IsNullOrEmpty(flagId))
                return "UnknownFlag";

            // Replace invalid characters
            var cleaned = flagId
                .Replace(" ", "_")
                .Replace("(", "")
                .Replace(")", "")
                .Replace("-", "_")
                .Replace(".", "_")
                .Replace(",", "_")
                .Replace("'", "")
                .Replace("\"", "")
                .Replace("&", "And")
                .Replace("+", "Plus")
                .Replace("=", "Equals")
                .Replace("!", "Not")
                .Replace("?", "Question")
                .Replace("#", "Hash")
                .Replace("@", "At")
                .Replace("$", "Dollar")
                .Replace("%", "Percent")
                .Replace("^", "Caret")
                .Replace("*", "Star")
                .Replace("[", "")
                .Replace("]", "")
                .Replace("{", "")
                .Replace("}", "")
                .Replace("|", "Pipe")
                .Replace("\\", "Backslash")
                .Replace("/", "Slash")
                .Replace(":", "")
                .Replace(";", "")
                .Replace("<", "Less")
                .Replace(">", "Greater")
                .Replace("~", "Tilde")
                .Replace("`", "Backtick");

            // Ensure it starts with a letter or underscore
            if (cleaned.Length > 0 && !char.IsLetter(cleaned[0]) && cleaned[0] != '_')
            {
                cleaned = "_" + cleaned;
            }

            // Remove consecutive underscores
            while (cleaned.Contains("__"))
            {
                cleaned = cleaned.Replace("__", "_");
            }

            // Remove trailing underscores
            cleaned = cleaned.TrimEnd('_');

            // Ensure it's not empty after cleaning
            if (string.IsNullOrEmpty(cleaned))
                return "UnknownFlag";

            return cleaned;
        }

        /// <summary>
        /// Save discovered flags to a dedicated discovery file, preserving existing discoveries
        /// </summary>
        private static void SaveDiscoveredFlags()
        {
            try
            {
                string fileName = "discovered_flags.txt";
                string filePath = Path.Combine(Application.persistentDataPath, "CabbySaves", fileName);
                
                // Ensure directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                
                // Explicitly merge historical discoveries with new discoveries
                var allDiscoveries = new Dictionary<string, (string sceneName, string type, object value)>();
                
                // First, add all historical discoveries (preserves old flags)
                foreach (var kvp in historicalDiscoveries)
                {
                    allDiscoveries[kvp.Key] = kvp.Value;
                }
                
                // Then, add new discoveries (new discoveries take precedence)
                foreach (var kvp in newFlagDiscoveries)
                {
                    allDiscoveries[kvp.Key] = kvp.Value;
                }
                
                var lines = new List<string>
                {
                    "// Discovered Flags - Copy the following code into FlagInstances.cs",
                    "// This file is automatically updated when new flags are discovered",
                    $"// Last updated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}",
                    ""
                };
                
                var playerDataFlags = new List<string>();
                var sceneFlags = new List<string>();

                foreach (var kvp in allDiscoveries)
                {
                    string flagKey = kvp.Key;
                    var (sceneName, flagType, value) = kvp.Value;
                    string codeLine = GenerateCodeLine(flagKey, sceneName, flagType);
                    
                    // Skip invalid entries
                    if (codeLine.Contains("Error parsing flag key:")) continue;
                    
                    if (flagType.StartsWith("PlayerData_"))
                    {
                        playerDataFlags.Add(codeLine);
                    }
                    else
                    {
                        sceneFlags.Add(codeLine);
                    }
                }
                
                if (playerDataFlags.Count > 0)
                {
                    lines.Add("// PlayerData Flags:");
                    playerDataFlags.Sort(); // Sort alphabetically
                    lines.AddRange(playerDataFlags);
                    lines.Add("");
                }

                if (sceneFlags.Count > 0)
                {
                    lines.Add("// Scene Flags:");
                    sceneFlags.Sort(); // Sort alphabetically
                    lines.AddRange(sceneFlags);
                    lines.Add("");
                }

                lines.Add($"// Total flags discovered: {allDiscoveries.Count}");
                
                File.WriteAllLines(filePath, lines);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[Flag Discovery] Failed to save discovered flags: {ex.Message}");
            }
        }

        /// <summary>
        /// Load discovered flags from the discovery file and add them to tracking sets
        /// </summary>
        private static void LoadDiscoveredFlags()
        {
            try
            {
                string fileName = "discovered_flags.txt";
                string filePath = Path.Combine(Application.persistentDataPath, "CabbySaves", fileName);
                
                if (!File.Exists(filePath)) return;
                
                var lines = File.ReadAllLines(filePath);
                int loadedCount = 0;
                bool inPlayerDataSection = false;
                bool inSceneSection = false;
                
                foreach (string line in lines)
                {
                    string trimmedLine = line.Trim();
                    
                    if (trimmedLine.StartsWith("// PlayerData Flags:"))
                    {
                        inPlayerDataSection = true;
                        inSceneSection = false;
                        continue;
                    }
                    else if (trimmedLine.StartsWith("// Scene Flags:"))
                    {
                        inPlayerDataSection = false;
                        inSceneSection = true;
                        continue;
                    }
                    else if (trimmedLine.StartsWith("// Total flags discovered:"))
                    {
                        break; // Stop parsing at total count
                    }
                    
                    if (trimmedLine.StartsWith("public static readonly FlagDef"))
                    {
                        string flagName = ExtractFlagNameFromCodeLine(trimmedLine);
                        if (!string.IsNullOrEmpty(flagName))
                        {
                            if (inPlayerDataSection)
                            {
                                // ALWAYS store in historical discoveries to preserve them across sessions
                                historicalDiscoveries[flagName] = ("Global", "PlayerData_Bool", null);
                                discoveredPlayerDataFlags.Add(flagName);
                                
                                // Only add to current tracking if we want to monitor them
                                if (!trackedPlayerDataFields.Contains(flagName))
                                {
                                    trackedPlayerDataFields.Add(flagName);
                                }
                                loadedCount++;
                            }
                            else if (inSceneSection)
                            {
                                // For scene flags, we need to extract the original ID from the code line
                                string flagId = ExtractFlagIdFromCodeLine(trimmedLine);
                                if (!string.IsNullOrEmpty(flagId))
                                {
                                    // Extract scene name from the flag name (format: SceneName__FlagId)
                                    string sceneName = "Unknown";
                                    if (flagName.Contains("__"))
                                    {
                                        string[] parts = flagName.Split(new string[] { "__" }, 2, StringSplitOptions.None);
                                        if (parts.Length >= 2)
                                        {
                                            // The first part is the scene name, convert back to proper format
                                            sceneName = parts[0].Replace("_", "-");
                                        }
                                    }
                                    
                                    // ALWAYS store in historical discoveries to preserve them across sessions
                                    // Use unique key combining flag ID and scene name to avoid duplicates
                                    // Use a special delimiter that won't appear in flag IDs or scene names
                                    string uniqueKey = $"{flagId}|||{sceneName}";
                                    historicalDiscoveries[uniqueKey] = (sceneName, "PersistentBoolData", null);
                                    discoveredSceneFlags.Add(uniqueKey);
                                    
                                    // Only add to current tracking if we want to monitor them
                                    if (!trackedSceneDataFields.Contains(flagId))
                                    {
                                        trackedSceneDataFields.Add(flagId);
                                    }
                                    loadedCount++;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[Flag Discovery] Failed to load discovered flags: {ex.Message}");
            }
        }

        /// <summary>
        /// Get the category of a flag type for persistent storage
        /// </summary>
        private static string GetFlagCategory(string flagType)
        {
            if (flagType.StartsWith("PlayerData_"))
                return "PlayerData";
            else
                return "Scene";
        }

        /// <summary>
        /// Initialize flag monitoring. Should be called from the mod's Start method.
        /// </summary>
        public static void ApplyPatches()
        {
            if (patchesApplied) return;
            
            try
            {
                ExtractTrackedFields();
                LoadDiscoveredFlags(); // Load previously discovered flags
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
                            if (flagDef.Type.StartsWith("PlayerData_"))
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
                
                LayoutRebuilder.ForceRebuildLayoutImmediate(notificationText.GetComponent<RectTransform>());
                LayoutRebuilder.ForceRebuildLayoutImmediate(notificationText.transform.parent.GetComponent<RectTransform>());
            }
            
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

        /// <summary>
        /// Generate a comprehensive report of all discovered flags for easy copy-paste into FlagInstances.cs
        /// </summary>
        public static void GenerateDiscoveryReport()
        {
            if (newFlagDiscoveries.Count == 0)
            {
                return;
            }

            // Group by type for better organization
            var playerDataFlags = new List<string>();
            var sceneFlags = new List<string>();

            foreach (var kvp in newFlagDiscoveries)
            {
                string flagKey = kvp.Key;
                var (sceneName, flagType, _) = kvp.Value;

                if (flagType.StartsWith("PlayerData_"))
                {
                    string fieldName = flagKey;
                    string codeLine = $"public static readonly FlagDef {fieldName} = new FlagDef(\"{fieldName}\", \"Global\", false, \"{flagType}\");";
                    playerDataFlags.Add(codeLine);
                }
                else
                {
                    // Scene flag
                    string[] parts = flagKey.Split(new char[] { '_' }, 2);
                    if (parts.Length >= 2)
                    {
                        string flagId = parts[0];
                        
                        // Convert scene name to proper format
                        string sceneInstanceName = sceneName.Replace("-", "_");
                        string flagIdClean = flagId.Replace(" ", "_");
                        
                        string codeLine = $"public static readonly FlagDef {sceneInstanceName}__{flagIdClean} = new FlagDef(\"{flagId}\", SceneInstances.{sceneInstanceName}.SceneName, false, \"{flagType}\");";
                        sceneFlags.Add(codeLine);
                    }
                }
            }

            // Also log to file
            fileLoggingReference.LogMessage("=== FLAG DISCOVERY REPORT ===");
            fileLoggingReference.LogMessage($"Total flags discovered: {newFlagDiscoveries.Count}");
            fileLoggingReference.LogMessage($"PlayerData flags: {playerDataFlags.Count}");
            fileLoggingReference.LogMessage($"Scene flags: {sceneFlags.Count}");
            fileLoggingReference.LogMessage("=== END REPORT ===");
            
            // Update consolidated file
            SaveDiscoveredFlags();
        }

        /// <summary>
        /// Extract flag name from a code line
        /// </summary>
        private static string ExtractFlagNameFromCodeLine(string codeLine)
        {
            int startIndex = codeLine.IndexOf("FlagDef ") + 8;
            int endIndex = codeLine.IndexOf(" = new");
            if (startIndex > 7 && endIndex > startIndex)
            {
                return codeLine.Substring(startIndex, endIndex - startIndex).Trim();
            }
            return "";
        }

        /// <summary>
        /// Extract flag ID from a scene flag code line
        /// </summary>
        private static string ExtractFlagIdFromCodeLine(string codeLine)
        {
            int startIndex = codeLine.IndexOf("(\"") + 2;
            int endIndex = codeLine.IndexOf("\", SceneInstances");
            if (startIndex > 1 && endIndex > startIndex)
            {
                return codeLine.Substring(startIndex, endIndex - startIndex).Trim();
            }
            return "";
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
                    
                    // Log diagnostics if enabled and interval has passed
                    if (FlagMonitorDiagnostics.ShouldLogDiagnostics())
                    {
                        FlagMonitorDiagnostics.LogDiagnostics();
                    }
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

            // Get all fields from PlayerData using reflection
            var playerDataType = typeof(PlayerData);
            var allFields = playerDataType.GetFields(BindingFlags.Public | BindingFlags.Instance);

            foreach (var field in allFields)
            {
                var fieldName = field.Name;
                
                // Skip ignored fields
                if (ignoredFields.Contains(fieldName)) continue;

                try
                {
                    FlagMonitorDiagnostics.RecordFieldAccess(fieldName);
                    
                    object currentValue = field.GetValue(PlayerData.instance);
                    previousPlayerDataValues.TryGetValue(fieldName, out object previousValue);

                    // Check if this is a newly discovered flag (not in current tracking AND not previously discovered)
                    bool isNewFlag = !trackedPlayerDataFields.Contains(fieldName) && !discoveredPlayerDataFlags.Contains(fieldName);
                    
                    if (isNewFlag)
                    {
                        discoveredPlayerDataFlags.Add(fieldName);
                        string flagType = field.FieldType.Name;
                        string notification = $"[NEW PLAYERDATA FLAG]: {fieldName} ({flagType}) = {currentValue}";
                        AddNotification(notification);
                        
                        // Log to console with copy-paste ready format
                        string codeFormat = $"public static readonly FlagDef {fieldName} = new FlagDef(\"{fieldName}\", \"Global\", false, \"PlayerData_{flagType}\");";
                        
                        // Log to file
                        fileLoggingReference.LogMessage($"[DISCOVERY] New PlayerData flag: {codeFormat}");
                        
                        // Store for later reference (both new and historical)
                        newFlagDiscoveries[fieldName] = ("Global", $"PlayerData_{flagType}", currentValue);
                        historicalDiscoveries[fieldName] = ("Global", $"PlayerData_{flagType}", currentValue);
                        
                        // Auto-add to tracking sets for immediate monitoring
                        trackedPlayerDataFields.Add(fieldName);
                        fieldInfoCache[fieldName] = (field, field.FieldType);
                        
                        // Auto-update discovery file on every discovery
                        SaveDiscoveredFlags(); // Save discovered flags immediately
                    }

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
                    else if (field.FieldType == typeof(bool))
                    {
                        valueChanged = (bool)currentValue != (bool)previousValue;
                    }
                    else if (field.FieldType == typeof(int))
                    {
                        valueChanged = (int)currentValue != (int)previousValue;
                    }
                    else if (field.FieldType == typeof(float))
                    {
                        valueChanged = !Mathf.Approximately((float)currentValue, (float)previousValue);
                    }
                    else if (field.FieldType == typeof(string))
                    {
                        valueChanged = !string.Equals((string)currentValue, (string)previousValue);
                    }
                    else if (IsSimpleType(field.FieldType))
                    {
                        // Fallback to object.Equals for other simple types
                        valueChanged = !Equals(currentValue, previousValue);
                    }
                    else
                    {
                        // For complex types, check recursively for changes
                        var changes = DetectComplexTypeChanges(fieldName, previousValue, currentValue, field.FieldType);
                        if (changes.Count > 0)
                        {
                            valueChanged = true;
                            FlagMonitorDiagnostics.RecordFieldChange(fieldName);
                            previousPlayerDataValues[fieldName] = currentValue;
                            
                            foreach (var change in changes)
                            {
                                AddNotification(change);
                            }
                        }
                    }

                    if (valueChanged)
                    {
                        FlagMonitorDiagnostics.RecordFieldChange(fieldName);
                        previousPlayerDataValues[fieldName] = currentValue;
                        
                        // Create detailed notification for simple types
                        string notification = CreateDetailedNotification(fieldName, currentValue, field.FieldType);
                        AddNotification(notification);
                    }
                }
                catch (Exception ex)
                {
                    FlagMonitorDiagnostics.RecordFieldError(fieldName, ex.Message);
                    Debug.LogWarning($"[Flag Monitor] Field access failed for '{fieldName}', rebuilding cache: {ex.Message}");
                    RebuildFieldInfoCache();
                }
            }
        }

        /// <summary>
        /// Creates a detailed notification string for complex types using reflection
        /// </summary>
        private static string CreateDetailedNotification(string fieldName, object value, Type fieldType)
        {
            return $"[PlayerData]: {fieldName} = {FormatComplexValue(value, fieldType)}";
        }

        /// <summary>
        /// Recursively formats complex values with detailed breakdown
        /// </summary>
        private static string FormatComplexValue(object value, Type valueType, int depth = 0)
        {
            const int MAX_DEPTH = 3; // Prevent infinite recursion
            
            // For simple types, use the standard format
            if (IsSimpleType(valueType))
            {
                return value?.ToString() ?? "null";
            }

            // Prevent infinite recursion
            if (depth >= MAX_DEPTH)
            {
                return value?.GetType().Name ?? "null";
            }

            // For complex types, create a detailed breakdown
            try
            {
                if (value == null)
                {
                    return "null";
                }

                var details = new List<string>();
                
                // Handle different complex types
                if (valueType.IsGenericType && valueType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    // Handle List<T> types
                    if (value is IList list)
                    {
                        details.Add($"Count: {list.Count}");
                        if (list.Count > 0)
                        {
                            var items = new List<string>();
                            for (int i = 0; i < Math.Min(list.Count, 5); i++) // Show first 5 items
                            {
                                var item = list[i];
                                if (item == null)
                                {
                                    items.Add("null");
                                }
                                else if (IsSimpleType(item.GetType()))
                                {
                                    items.Add(item.ToString());
                                }
                                else
                                {
                                    // Recursively format complex list items
                                    items.Add(FormatComplexValue(item, item.GetType(), depth + 1));
                                }
                            }
                            if (list.Count > 5)
                            {
                                items.Add($"... and {list.Count - 5} more");
                            }
                            details.Add($"Items: [{string.Join(", ", items)}]");
                        }
                    }
                }
                else if (valueType == typeof(Vector3))
                {
                    // Handle Vector3 specifically
                    var vector = (Vector3)value;
                    details.Add($"X: {vector.x:F2}, Y: {vector.y:F2}, Z: {vector.z:F2}");
                }
                else
                {
                    // Use reflection to get all public fields and properties
                    var fields = valueType.GetFields(BindingFlags.Public | BindingFlags.Instance);
                    var properties = valueType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                    foreach (var field in fields)
                    {
                        try
                        {
                            var fieldValue = field.GetValue(value);
                            if (IsSimpleType(field.FieldType))
                            {
                                details.Add($"{field.Name}: {fieldValue}");
                            }
                            else if (fieldValue is IList list)
                            {
                                details.Add($"{field.Name}: List[{list.Count} items]");
                            }
                            else
                            {
                                // Recursively format complex fields
                                details.Add($"{field.Name}: {FormatComplexValue(fieldValue, field.FieldType, depth + 1)}");
                            }
                        }
                        catch
                        {
                            details.Add($"{field.Name}: <error>");
                        }
                    }

                    foreach (var prop in properties)
                    {
                        try
                        {
                            if (prop.CanRead && prop.GetIndexParameters().Length == 0)
                            {
                                var propValue = prop.GetValue(value);
                                if (IsSimpleType(prop.PropertyType))
                                {
                                    details.Add($"{prop.Name}: {propValue}");
                                }
                                else if (propValue is IList list)
                                {
                                    details.Add($"{prop.Name}: List[{list.Count} items]");
                                }
                                else
                                {
                                    // Recursively format complex properties
                                    details.Add($"{prop.Name}: {FormatComplexValue(propValue, prop.PropertyType, depth + 1)}");
                                }
                            }
                        }
                        catch
                        {
                            details.Add($"{prop.Name}: <error>");
                        }
                    }
                }

                if (details.Count == 0)
                {
                    return value.ToString();
                }

                return $"{{{string.Join(", ", details)}}}";
            }
            catch (Exception ex)
            {
                // Fallback to simple format if reflection fails
                return $"{value} (Error: {ex.Message})";
            }
        }

        /// <summary>
        /// Recursively detects changes in complex types and returns detailed change notifications
        /// </summary>
        private static List<string> DetectComplexTypeChanges(string fieldName, object previousValue, object currentValue, Type valueType, int depth = 0)
        {
            const int MAX_DEPTH = 3; // Prevent infinite recursion
            var changes = new List<string>();
            
            // Prevent infinite recursion
            if (depth >= MAX_DEPTH)
            {
                return changes;
            }

            // Handle null cases
            if (previousValue == null && currentValue == null)
            {
                return changes;
            }
            
            if (previousValue == null || currentValue == null)
            {
                changes.Add($"[PlayerData]: {fieldName} = {FormatComplexValue(currentValue, valueType)}");
                return changes;
            }

            try
            {
                // Handle different complex types
                if (valueType.IsGenericType && valueType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    if (previousValue is IList previousList && currentValue is IList currentList)
                    {
                        // Check if count changed
                        if (previousList.Count != currentList.Count)
                        {
                            changes.Add($"[PlayerData]: {fieldName} = {FormatComplexValue(currentValue, valueType)}");
                            return changes;
                        }

                        // Check individual items
                        for (int i = 0; i < currentList.Count; i++)
                        {
                            var previousItem = previousList[i];
                            var currentItem = currentList[i];

                            if (!Equals(previousItem, currentItem))
                            {
                                changes.Add($"[PlayerData]: {fieldName}[{i}] = {FormatComplexValue(currentItem, currentItem?.GetType() ?? typeof(object))}");
                            }
                        }
                    }
                }
                else if (valueType == typeof(Vector3))
                {
                    // Handle Vector3 specifically
                    var previousVector = (Vector3)previousValue;
                    var currentVector = (Vector3)currentValue;
                    
                    if (!Mathf.Approximately(previousVector.x, currentVector.x) ||
                        !Mathf.Approximately(previousVector.y, currentVector.y) ||
                        !Mathf.Approximately(previousVector.z, currentVector.z))
                    {
                        changes.Add($"[PlayerData]: {fieldName} = {FormatComplexValue(currentValue, valueType)}");
                    }
                }
                else
                {
                    // Use reflection to check all public fields and properties
                    var fields = valueType.GetFields(BindingFlags.Public | BindingFlags.Instance);
                    var properties = valueType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                    foreach (var field in fields)
                    {
                        try
                        {
                            var previousFieldValue = field.GetValue(previousValue);
                            var currentFieldValue = field.GetValue(currentValue);
                            
                            if (!Equals(previousFieldValue, currentFieldValue))
                            {
                                if (IsSimpleType(field.FieldType))
                                {
                                    changes.Add($"[PlayerData]: {fieldName}.{field.Name} = {currentFieldValue}");
                                }
                                else
                                {
                                    // Recursively check complex fields
                                    var nestedChanges = DetectComplexTypeChanges($"{fieldName}.{field.Name}", previousFieldValue, currentFieldValue, field.FieldType, depth + 1);
                                    changes.AddRange(nestedChanges);
                                }
                            }
                        }
                        catch
                        {
                            // Ignore reflection errors for individual fields
                        }
                    }

                    foreach (var prop in properties)
                    {
                        try
                        {
                            if (prop.CanRead && prop.GetIndexParameters().Length == 0)
                            {
                                var previousPropValue = prop.GetValue(previousValue);
                                var currentPropValue = prop.GetValue(currentValue);
                                
                                if (!Equals(previousPropValue, currentPropValue))
                                {
                                    if (IsSimpleType(prop.PropertyType))
                                    {
                                        changes.Add($"[PlayerData]: {fieldName}.{prop.Name} = {currentPropValue}");
                                    }
                                    else
                                    {
                                        // Recursively check complex properties
                                        var nestedChanges = DetectComplexTypeChanges($"{fieldName}.{prop.Name}", previousPropValue, currentPropValue, prop.PropertyType, depth + 1);
                                        changes.AddRange(nestedChanges);
                                    }
                                }
                            }
                        }
                        catch
                        {
                            // Ignore reflection errors for individual properties
                        }
                    }
                }
            }
            catch (Exception)
            {
                // If reflection fails, fall back to simple comparison
                if (!Equals(previousValue, currentValue))
                {
                    changes.Add($"[PlayerData]: {fieldName} = {FormatComplexValue(currentValue, valueType)}");
                }
            }

            return changes;
        }

        /// <summary>
        /// Determines if a type is simple enough to display directly
        /// </summary>
        private static bool IsSimpleType(Type type)
        {
            return type.IsPrimitive || 
                   type == typeof(string) || 
                   type == typeof(decimal) || 
                   type == typeof(DateTime) || 
                   type == typeof(TimeSpan) ||
                   type.IsEnum;
        }

        private static void PollSceneDataFields()
        {
            if (SceneData.instance == null) return;

            // Check for instance change
            if (SceneData.instance != lastKnownSceneDataInstance)
            {
                HandleSceneDataInstanceChange();
            }

            // Poll predefined flags (existing functionality)
            PollPersistentBoolData();
            PollPersistentIntData();
            PollGeoRockData();
            
            // Poll all scene flags dynamically (new functionality)
            PollAllSceneFlags();
        }

        /// <summary>
        /// Polls ALL scene flags dynamically, including those not defined in FlagInstances
        /// </summary>
        private static void PollAllSceneFlags()
        {
            // Poll all PersistentBoolData entries
            if (SceneData.instance.persistentBoolItems != null)
            {
                foreach (var pbd in SceneData.instance.persistentBoolItems)
                {
                    string key = $"PersistentBool_{pbd.id}_{pbd.sceneName}";
                    bool currentValue = pbd.activated;
                    previousSceneDataValues.TryGetValue(key, out object previousValue);

                    // Check if this is a newly discovered flag (not in current tracking AND not previously discovered)
                    bool isNewFlag = !trackedSceneDataFields.Contains(pbd.id) && !discoveredSceneFlags.Contains($"{pbd.id}|||{pbd.sceneName}");
                    
                    if (isNewFlag)
                    {
                        discoveredSceneFlags.Add($"{pbd.id}|||{pbd.sceneName}");
                        string notification = $"[NEW SCENE FLAG]: {pbd.id} ({pbd.sceneName}) = {currentValue}";
                        AddNotification(notification);
                        
                        // Log to console with copy-paste ready format
                        string codeFormat = $"public static readonly FlagDef {pbd.sceneName.Replace("-", "_")}__{pbd.id.Replace(" ", "_")} = new FlagDef(\"{pbd.id}\", SceneInstances.{pbd.sceneName.Replace("-", "_")}.SceneName, false, \"PersistentBoolData\");";
                        
                        // Log to file
                        fileLoggingReference.LogMessage($"[DISCOVERY] New Scene flag: {codeFormat}");
                        
                        // Store for later reference (both new and historical)
                        // Use unique key combining flag ID and scene name to avoid duplicates
                        // Use a special delimiter that won't appear in flag IDs or scene names
                        string uniqueKey = $"{pbd.id}|||{pbd.sceneName}";
                        newFlagDiscoveries[uniqueKey] = (pbd.sceneName, "PersistentBoolData", currentValue);
                        historicalDiscoveries[uniqueKey] = (pbd.sceneName, "PersistentBoolData", currentValue);
                        
                        // Auto-add to tracking sets for immediate monitoring
                        trackedSceneDataFields.Add(pbd.id);
                        
                        // Auto-update discovery file on every discovery
                        SaveDiscoveredFlags(); // Save discovered flags immediately
                    }

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

            // Poll all PersistentIntData entries
            if (SceneData.instance.persistentIntItems != null)
            {
                foreach (var pid in SceneData.instance.persistentIntItems)
                {
                    string key = $"PersistentInt_{pid.id}_{pid.sceneName}";
                    int currentValue = pid.value;
                    previousSceneDataValues.TryGetValue(key, out object previousValue);

                    // Check if this is a newly discovered flag (not in current tracking AND not previously discovered)
                    bool isNewFlag = !trackedSceneDataFields.Contains(pid.id) && !discoveredSceneFlags.Contains($"{pid.id}|||{pid.sceneName}");
                    
                    if (isNewFlag)
                    {
                        discoveredSceneFlags.Add($"{pid.id}|||{pid.sceneName}");
                        string notification = $"[NEW SCENE FLAG]: {pid.id} ({pid.sceneName}) = {currentValue}";
                        AddNotification(notification);
                        
                        // Log to console with copy-paste ready format
                        string codeFormat = $"public static readonly FlagDef {pid.sceneName.Replace("-", "_")}__{pid.id.Replace(" ", "_")} = new FlagDef(\"{pid.id}\", SceneInstances.{pid.sceneName.Replace("-", "_")}.SceneName, false, \"PersistentIntData\");";
                        
                        // Log to file
                        fileLoggingReference.LogMessage($"[DISCOVERY] New Scene flag: {codeFormat}");
                        
                        // Store for later reference (both new and historical)
                        // Use unique key combining flag ID and scene name to avoid duplicates
                        // Use a special delimiter that won't appear in flag IDs or scene names
                        string uniqueKey = $"{pid.id}|||{pid.sceneName}";
                        newFlagDiscoveries[uniqueKey] = (pid.sceneName, "PersistentIntData", currentValue);
                        historicalDiscoveries[uniqueKey] = (pid.sceneName, "PersistentIntData", currentValue);
                        
                        // Auto-add to tracking sets for immediate monitoring
                        trackedSceneDataFields.Add(pid.id);
                        
                        // Auto-update discovery file on every discovery
                        SaveDiscoveredFlags(); // Save discovered flags immediately
                    }

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

            // Poll all GeoRockData entries
            if (SceneData.instance.geoRocks != null)
            {
                foreach (var grd in SceneData.instance.geoRocks)
                {
                    string key = $"GeoRock_{grd.id}_{grd.sceneName}";
                    int currentHitsLeft = grd.hitsLeft;
                    bool currentValue = currentHitsLeft <= 0; // True if broken (no hits left)

                    previousSceneDataValues.TryGetValue(key, out object previousValue);

                    // Check if this is a newly discovered flag (not in current tracking AND not previously discovered)
                    bool isNewFlag = !trackedSceneDataFields.Contains(grd.id) && !discoveredSceneFlags.Contains($"{grd.id}|||{grd.sceneName}");
                    
                    if (isNewFlag)
                    {
                        discoveredSceneFlags.Add($"{grd.id}|||{grd.sceneName}");
                        string status = currentValue ? "Broken" : $"Intact ({currentHitsLeft} hits left)";
                        string notification = $"[NEW SCENE FLAG]: GeoRock {grd.id} ({grd.sceneName}) = {status}";
                        AddNotification(notification);
                        
                        // Log to console with copy-paste ready format
                        string codeFormat = $"public static readonly FlagDef {grd.sceneName.Replace("-", "_")}__Geo_Rock_{grd.id.Replace(" ", "_")} = new FlagDef(\"{grd.id}\", SceneInstances.{grd.sceneName.Replace("-", "_")}.SceneName, false, \"GeoRockData\");";
                        
                        // Log to file
                        fileLoggingReference.LogMessage($"[DISCOVERY] New GeoRock flag: {codeFormat}");
                        
                        // Store for later reference (both new and historical)
                        // Use unique key combining flag ID and scene name to avoid duplicates
                        // Use a special delimiter that won't appear in flag IDs or scene names
                        string uniqueKey = $"{grd.id}|||{grd.sceneName}";
                        newFlagDiscoveries[uniqueKey] = (grd.sceneName, "GeoRockData", currentValue);
                        historicalDiscoveries[uniqueKey] = (grd.sceneName, "GeoRockData", currentValue);
                        
                        // Auto-add to tracking sets for immediate monitoring
                        trackedSceneDataFields.Add(grd.id);
                        
                        // Auto-update discovery file on every discovery
                        SaveDiscoveredFlags(); // Save discovered flags immediately
                    }

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