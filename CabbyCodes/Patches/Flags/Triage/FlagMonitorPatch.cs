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
using TMPro;
using CabbyCodes.Scenes;

namespace CabbyCodes.Patches.Flags.Triage
{
    public class FlagMonitorPatch
    {
        public static GameObject notificationPanel;
        public static TextMeshProUGUI notificationText;
        private static readonly Queue<string> notificationQueue = new Queue<string>();
        private const int MAX_NOTIFICATIONS = 200;
        private static readonly FlagMonitorReference monitorReference = FlagMonitorReference.Instance;
        private static readonly FlagFileLoggingReference fileLoggingReference = FlagFileLoggingReference.Instance;
        
        private static bool patchesApplied = false;

        // Static initialization flag to prevent multiple registrations
        private static bool sceneEventsRegistered = false;

        // Tracked fields from FlagInstances
        private static readonly HashSet<string> trackedPlayerDataFields = new HashSet<string>();
        private static readonly HashSet<string> trackedSceneDataFields = new HashSet<string>(); // Stores "flagId|||sceneName" combinations

        // Scene instance tracking
        private static readonly HashSet<string> knownSceneNames = new HashSet<string>();
        private static readonly HashSet<string> discoveredSceneNames = new HashSet<string>();
        private static readonly Dictionary<string, string> discoveredSceneAreas = new Dictionary<string, string>();
        private static string lastKnownSceneArea = null;

        // Fields to ignore during polling (noisy or not useful)
        private static readonly HashSet<string> ignoredFields = new HashSet<string>
        {
            // Additional hardcoded fields that are not in FlagInstances
            "currentArea",
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
            "playTime",
            "profileID"
        };

        // --- Performance optimizations ---
        private static readonly Dictionary<string, (FieldInfo fieldInfo, Type fieldType)> fieldInfoCache = new Dictionary<string, (FieldInfo, Type)>();
        private static PlayerData lastKnownInstance = null;

        // --- Polling state ---
        private static readonly Dictionary<string, object> previousPlayerDataValues = new Dictionary<string, object>();
        private static bool pollingStarted = false;
        private static bool hasLeftTitleScreen = false;
        private static bool isInMenuScene = false;

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
            string category = GetFlagCategory(flagType);
            if (category == "PlayerData")
            {
                string fieldName = flagKey;
                return $"public static readonly FlagDef {fieldName} = new FlagDef(\"{fieldName}\", null, false, \"{flagType}\");";
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
                    
                    return $"public static readonly FlagDef {sceneInstanceName}__{flagIdClean} = new FlagDef(\"{flagId}\", SceneInstances.{sceneInstanceName}, false, \"{flagType}\");";
                }
                else
                {
                    return $"// Error parsing flag key: {flagKey}";
                }
            }
        }

        /// <summary>
        /// Checks if a flag is in the UnusedFlags array
        /// </summary>
        private static bool IsFlagInUnusedFlags(string flagId, string sceneName)
        {
            foreach (var unusedFlag in FlagInstances.UnusedFlags)
            {
                if (unusedFlag != null && unusedFlag.Id == flagId)
                {
                    // For scene flags, check if the scene names match
                    if (unusedFlag.Type == "PersistentBoolData" || unusedFlag.Type == "PersistentIntData" || unusedFlag.Type == "GeoRockData")
                    {
                        if (unusedFlag.SceneName == sceneName)
                        {
                            return true;
                        }
                    }
                    // For PlayerData flags, just check the ID
                    else if (GetFlagCategory(unusedFlag.Type) == "PlayerData")
                    {
                        return true;
                    }
                }
            }
            return false;
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
        /// Clean up duplicate flags from the discovery collections
        /// </summary>
        private static void CleanupDuplicateFlags()
        {
            try
            {
                // Clean up duplicate scene flags
                var uniqueSceneFlags = new HashSet<string>();
                var cleanedSceneFlags = new HashSet<string>();
                
                foreach (var flagKey in discoveredSceneFlags)
                {
                    if (uniqueSceneFlags.Add(flagKey))
                    {
                        cleanedSceneFlags.Add(flagKey);
                    }
                    else
                    {
                        Debug.Log($"[Flag Discovery] Removed duplicate scene flag: {flagKey}");
                    }
                }
                
                discoveredSceneFlags.Clear();
                discoveredSceneFlags.UnionWith(cleanedSceneFlags);
                
                // Clean up duplicate player data flags
                var uniquePlayerDataFlags = new HashSet<string>();
                var cleanedPlayerDataFlags = new HashSet<string>();
                
                foreach (var flagName in discoveredPlayerDataFlags)
                {
                    if (uniquePlayerDataFlags.Add(flagName))
                    {
                        cleanedPlayerDataFlags.Add(flagName);
                    }
                    else
                    {
                        Debug.Log($"[Flag Discovery] Removed duplicate player data flag: {flagName}");
                    }
                }
                
                discoveredPlayerDataFlags.Clear();
                discoveredPlayerDataFlags.UnionWith(cleanedPlayerDataFlags);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[Flag Discovery] Failed to cleanup duplicate flags: {ex.Message}");
            }
        }

        /// <summary>
        /// Save discovered flags to a dedicated discovery file, preserving existing discoveries
        /// </summary>
        private static void SaveDiscoveredFlags()
        {
            try
            {
                // Clean up any duplicates before saving
                CleanupDuplicateFlags();
                
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
                var newScenes = new List<string>();

                foreach (var kvp in allDiscoveries)
                {
                    string flagKey = kvp.Key;
                    var (sceneName, flagType, value) = kvp.Value;
                    string codeLine = GenerateCodeLine(flagKey, sceneName, flagType);
                    
                    // Skip invalid entries
                    if (codeLine.Contains("Error parsing flag key:")) continue;
                    
                    string category = GetFlagCategory(flagType);
                    if (category == "PlayerData")
                    {
                        playerDataFlags.Add(codeLine);
                    }
                    else
                    {
                        sceneFlags.Add(codeLine);
                    }
                }
                
                // Add discovered scenes (only those not already defined in SceneInstances)
                foreach (var sceneName in discoveredSceneNames)
                {
                    // Skip scenes that are now defined in SceneInstances
                    if (knownSceneNames.Contains(sceneName)) continue;
                    
                    string areaName = discoveredSceneAreas.ContainsKey(sceneName) ? discoveredSceneAreas[sceneName] : null;
                    string sceneCodeLine;
                    
                    // If area is null, use null to indicate it's a system scene
                    if (areaName == null)
                    {
                        sceneCodeLine = $"public static readonly SceneMapData {sceneName} = new SceneMapData(\"{sceneName}\", null);";
                    }
                    else
                    {
                        sceneCodeLine = $"public static readonly SceneMapData {sceneName} = new SceneMapData(\"{sceneName}\", \"{areaName}\");";
                    }
                    
                    newScenes.Add(sceneCodeLine);
                }
                
                if (playerDataFlags.Count > 0 && FlagMonitorSettings.IncludePlayerDataFlags)
                {
                    lines.Add("// PlayerData Flags:");
                    playerDataFlags.Sort(); // Sort alphabetically
                    lines.AddRange(playerDataFlags);
                    lines.Add("");
                }

                if (sceneFlags.Count > 0 && FlagMonitorSettings.IncludeSceneFlags)
                {
                    lines.Add("// Scene Flags:");
                    sceneFlags.Sort(); // Sort alphabetically
                    lines.AddRange(sceneFlags);
                    lines.Add("");
                }

                if (newScenes.Count > 0)
                {
                    lines.Add("// New Scene Instances - Copy the following code into SceneInstances.cs:");
                    lines.Add("// Add these to the appropriate area section in SceneInstances.cs");
                    lines.Add("");
                    newScenes.Sort(); // Sort alphabetically
                    lines.AddRange(newScenes);
                    lines.Add("");
                }

                lines.Add($"// Total flags discovered: {allDiscoveries.Count}");
                lines.Add($"// Total new scenes discovered: {discoveredSceneNames.Count}");
                
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
                bool inNewScenesSection = false;
                
                foreach (string line in lines)
                {
                    string trimmedLine = line.Trim();
                    
                    if (trimmedLine.StartsWith("// PlayerData Flags:"))
                    {
                        inPlayerDataSection = true;
                        inSceneSection = false;
                        inNewScenesSection = false;
                        continue;
                    }
                    else if (trimmedLine.StartsWith("// Scene Flags:"))
                    {
                        inPlayerDataSection = false;
                        inSceneSection = true;
                        inNewScenesSection = false;
                        continue;
                    }
                    else if (trimmedLine.StartsWith("// New Scene Instances"))
                    {
                        inPlayerDataSection = false;
                        inSceneSection = false;
                        inNewScenesSection = true;
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
                                            // The first part is the scene name, keep it as underscore format to match game data
                                            // Don't convert underscores to hyphens - keep the original format
                                            sceneName = parts[0]; // Keep as Room_temple, not Room-temple
                                        }
                                    }
                                    
                                    // ALWAYS store in historical discoveries to preserve them across sessions
                                    // Use unique key combining flag ID and scene name to avoid duplicates
                                    // Use a special delimiter that won't appear in flag IDs or scene names
                                    string uniqueKey = $"{flagId}|||{sceneName}";
                                    

                                    
                                    // Check if this flag is already in historical discoveries to prevent duplicates
                                    if (!historicalDiscoveries.ContainsKey(uniqueKey))
                                    {
                                        historicalDiscoveries[uniqueKey] = (sceneName, "PersistentBoolData", null);
                                        discoveredSceneFlags.Add(uniqueKey);
                                        
                                        // Only add to current tracking if we want to monitor them
                                        if (!trackedSceneDataFields.Contains(uniqueKey))
                                        {
                                            trackedSceneDataFields.Add(uniqueKey);
                                        }
                                        loadedCount++;
                                    }
                                    else
                                    {
                                        //Debug.Log($"[Flag Discovery] Skipping duplicate flag: {uniqueKey} (already in historical discoveries)");
                                    }
                                }
                            }
                        }
                    }
                    else if (inNewScenesSection && trimmedLine.StartsWith("public static readonly SceneMapData"))
                    {
                        // Parse scene instance lines like: public static readonly SceneMapData SceneName = new SceneMapData("SceneName", "AreaName");
                        string sceneName = ExtractSceneNameFromCodeLine(trimmedLine);
                        string areaName = ExtractAreaNameFromCodeLine(trimmedLine);
                        
                        if (!string.IsNullOrEmpty(sceneName))
                        {
                            // Check if this scene is already discovered to prevent duplicates
                            if (!discoveredSceneNames.Contains(sceneName))
                            {
                                discoveredSceneNames.Add(sceneName);
                                discoveredSceneAreas[sceneName] = areaName;
                                
                                // Update last known scene area if we don't have one yet
                                if (lastKnownSceneArea == null)
                                {
                                    lastKnownSceneArea = areaName;
                                }
                                
                                loadedCount++;
                            }
                            else
                            {
                                Debug.Log($"[Flag Discovery] Skipping duplicate scene: {sceneName} (already discovered)");
                            }
                        }
                    }
                }
                
                Debug.Log($"[Flag Discovery] Loaded {loadedCount} flags/scenes from discovery file");
                
                // Clean up any duplicates that might exist in the loaded data
                CleanupDuplicateFlags();
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
        /// Extract all known scene names from SceneInstances
        /// </summary>
        private static void ExtractKnownSceneNames()
        {
            try
            {
                var sceneInstancesType = typeof(SceneInstances);
                var fields = sceneInstancesType.GetFields(BindingFlags.Public | BindingFlags.Static);
                
                foreach (var field in fields)
                {
                    if (field.FieldType == typeof(SceneMapData))
                    {
                        var sceneMapData = (SceneMapData)field.GetValue(null);
                        if (sceneMapData != null && !string.IsNullOrEmpty(sceneMapData.SceneName))
                        {
                            knownSceneNames.Add(sceneMapData.SceneName);
                            
                            // If this scene was previously discovered, remove it from discovered collections
                            if (discoveredSceneNames.Contains(sceneMapData.SceneName))
                            {
                                discoveredSceneNames.Remove(sceneMapData.SceneName);
                                discoveredSceneAreas.Remove(sceneMapData.SceneName);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[Flag Monitor] Failed to extract known scene names: {ex.Message}");
            }
        }

        /// <summary>
        /// Initialize flag monitoring. Should be called from the mod's Start method.
        /// </summary>
        public static void ApplyPatches()
        {
            if (patchesApplied) return;
            
            try
            {
                ExtractKnownSceneNames(); // Extract known scene names first
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
        /// Build the ignored fields set dynamically from FlagInstances.UnusedFlags
        /// </summary>
        private static void BuildIgnoredFieldsSet()
        {
            try
            {
                // Add all unused flags from FlagInstances
                foreach (var unusedFlag in FlagInstances.UnusedFlags)
                {
                    if (unusedFlag != null && !string.IsNullOrEmpty(unusedFlag.Id))
                    {
                        ignoredFields.Add(unusedFlag.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[Flag Monitor] Failed to build ignored fields set: {ex.Message}");
            }
        }

        /// <summary>
        /// Extract all field names from FlagInstances that need to be monitored
        /// </summary>
        private static void ExtractTrackedFields()
        {
            try
            {
                // First build the ignored fields set from FlagInstances.UnusedFlags
                BuildIgnoredFieldsSet();
                
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
                                trackedSceneDataFields.Add($"{flagDef.Id}|||{flagDef.SceneName}");
                            }
                        }
                    }
                }
                
                // Process UnusedFlags array to add scene flags to tracking
                foreach (var unusedFlag in FlagInstances.UnusedFlags)
                {
                    if (unusedFlag != null && !string.IsNullOrEmpty(unusedFlag.Id))
                    {
                        if (unusedFlag.Type == "PersistentBoolData" || unusedFlag.Type == "PersistentIntData" || unusedFlag.Type == "GeoRockData")
                        {
                            trackedSceneDataFields.Add($"{unusedFlag.Id}|||{unusedFlag.SceneName}");
                        }
                    }
                }
                
                // Handle ignored fields based on IncludeAllFlags setting
                if (FlagMonitorSettings.IncludeAllFlags)
                {
                    // When including all flags, add ignored fields back to tracking
                    foreach (var ignoredField in ignoredFields)
                    {
                        trackedPlayerDataFields.Add(ignoredField);
                    }
                }
                else
                {
                    // When not including all flags, remove ignored fields from tracking
                    foreach (var ignoredField in ignoredFields)
                    {
                        trackedPlayerDataFields.Remove(ignoredField);
                    }
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
            }
            
            // Track when we return to main menu to pause monitoring temporarily
            if (newScene.name == SceneInstances.Quit_To_Menu.SceneName || newScene.name == SceneInstances.Menu_Title.SceneName)
            {
                // We're entering a critical menu scene, clear previous values to prevent false change notifications
                // when we return to gameplay (flags will be re-initialized)
                previousPlayerDataValues.Clear();
                previousSceneDataValues.Clear();
                
                // Mark that we're in a menu scene
                isInMenuScene = true;
                
                // We're in a critical menu scene, pause monitoring to avoid processing flag resets
                // The polling will resume when we leave the menu again
                return;
            }
            
            // We're leaving a menu scene, resume monitoring
            if (isInMenuScene)
            {
                isInMenuScene = false;
                // Clear values again when leaving menu to ensure fresh baseline
                previousPlayerDataValues.Clear();
                previousSceneDataValues.Clear();
            }
            
            if (!monitorReference.IsEnabled) return;
            
            // Update the last known scene area before checking for new scenes
            UpdateLastKnownSceneArea(oldScene.name);
            
            // Only add scene change notification if enabled
            if (FlagMonitorSettings.ShowSceneTransitions)
            {
                string notification = $"[SceneChange]: {oldScene.name} -> {newScene.name}";
                AddNotification(notification);
            }
            
            // Check for new scene discovery
            CheckForNewScene(newScene.name);
        }

        /// <summary>
        /// Update the last known scene area based on the scene name
        /// </summary>
        private static void UpdateLastKnownSceneArea(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName)) return;
            
            // Skip system scenes and menu scenes
            if (sceneName.StartsWith("Menu_") || sceneName.StartsWith("UI_") || sceneName == "Init" || sceneName == "Preload") return;
            
            // Check if this scene is in our known scenes
            if (knownSceneNames.Contains(sceneName))
            {
                // Find the area for this known scene
                var sceneInstancesType = typeof(SceneInstances);
                var fields = sceneInstancesType.GetFields(BindingFlags.Public | BindingFlags.Static);
                
                foreach (var field in fields)
                {
                    if (field.FieldType == typeof(SceneMapData))
                    {
                        var sceneMapData = (SceneMapData)field.GetValue(null);
                        if (sceneMapData != null && sceneMapData.SceneName == sceneName)
                        {
                            // Only update area if it's not null (system scenes have null areas)
                            if (sceneMapData.AreaName != null)
                            {
                                lastKnownSceneArea = sceneMapData.AreaName;
                            }
                            return;
                        }
                    }
                }
            }
            else if (discoveredSceneNames.Contains(sceneName))
            {
                // Use the area from our discovered scenes
                if (discoveredSceneAreas.ContainsKey(sceneName))
                {
                    lastKnownSceneArea = discoveredSceneAreas[sceneName];
                }
            }
        }

        /// <summary>
        /// Check if a scene is new (not in SceneInstances) and add it to discoveries
        /// </summary>
        private static void CheckForNewScene(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName)) return;
            
            // Skip if we already know about this scene
            if (knownSceneNames.Contains(sceneName) || discoveredSceneNames.Contains(sceneName)) return;
            
            // Skip system scenes and menu scenes
            if (sceneName.StartsWith("Menu_") || sceneName.StartsWith("UI_") || sceneName == "Init" || sceneName == "Preload") return;
            
            // This is a new scene!
            discoveredSceneNames.Add(sceneName);
            
            // Use the last known scene area for new scenes, or null if we don't have one
            string areaName = lastKnownSceneArea;
            
            discoveredSceneAreas[sceneName] = areaName;
            
            string notification = $"[NEW SCENE DISCOVERED]: {sceneName} (Area: {areaName})";
            AddNotification(notification);
            
            // Save discoveries immediately
            SaveDiscoveredFlags();
        }

        private static void AddNotification(string message)
        {
            // Check if we should show this type of notification based on settings
            if (!ShouldShowNotificationType(message))
            {
                return;
            }

            // Log to file if enabled
            fileLoggingReference.LogMessage(message);
            
            // Only queue notifications if the monitor is enabled
            if (monitorReference.IsEnabled)
            {
                notificationQueue.Enqueue(message);
                
                // Truncate to last MAX_NOTIFICATIONS if queue gets too long
                while (notificationQueue.Count > MAX_NOTIFICATIONS)
                {
                    notificationQueue.Dequeue();
                }
                
                UpdateNotificationDisplay();
            }
        }

        /// <summary>
        /// Determines if a notification should be shown based on current settings
        /// </summary>
        private static bool ShouldShowNotificationType(string message)
        {
            // Scene transitions
            if (message.StartsWith("[SceneChange]"))
            {
                return FlagMonitorSettings.ShowSceneTransitions;
            }
            
            // New discoveries
            if (message.StartsWith("[NEW PLAYERDATA FLAG]") || 
                message.StartsWith("[NEW SCENE FLAG") ||
                message.StartsWith("[NEW SCENE DISCOVERED]") ||
                message.StartsWith("[NEW BOSSSTATUE FLAG]"))
            {
                return FlagMonitorSettings.ShowNewDiscoveries;
            }
            
            // Value changes
            if (message.StartsWith("[PlayerData]:") || message.StartsWith("[Scene:"))
            {
                return FlagMonitorSettings.ShowChangedValues;
            }
            
            // Default to showing other types
            return true;
        }

        /// <summary>
        /// Formats a notification message with appropriate color based on its content
        /// </summary>
        private static string FormatNotificationWithColor(string notification)
        {
            // Color coding for different types of notifications
            if (notification.StartsWith("[SceneChange]"))
            {
                return $"<color=#FFFFFF><b>{notification}</b></color>"; // White for scene changes
            }
            else if (notification.StartsWith("[PlayerData]"))
            {
                return $"<color=#00FFFF><b>{notification}</b></color>"; // Cyan for PlayerData changes
            }
            else if (notification.StartsWith("[Scene:"))
            {
                return $"<color=#00FFFF><b>{notification}</b></color>"; // Cyan for scene flags
            }
            else if (notification.StartsWith("[NEW PLAYERDATA FLAG]"))
            {
                return $"<color=#FF8000><b>{notification}</b></color>"; // Orange for new PlayerData flags
            }
            else if (notification.StartsWith("[NEW SCENE FLAG"))
            {
                return $"<color=#FF8000><b>{notification}</b></color>"; // Orange for new scene flags
            }
            else if (notification.StartsWith("[NEW SCENE DISCOVERED]"))
            {
                return $"<color=#FF00FF><b>{notification}</b></color>"; // Magenta for new scene discoveries
            }
            else if (notification.StartsWith("[DISCOVERY]"))
            {
                return $"<color=#FF8000><b>{notification}</b></color>"; // Orange for discoveries
            }
            else
            {
                return $"<color=#87CEEB><b>{notification}</b></color>"; // Light blue for other notifications
            }
        }

        public static void UpdateNotificationDisplay()
        {
            if (notificationPanel == null) return;
            
            int count = notificationQueue.Count;
            string displayText = $"<color=#00BFFF><b>Flag Monitor Active - Notifications: {count}/{MAX_NOTIFICATIONS}</b></color>\n\n";
            
            foreach (string notification in notificationQueue)
            {
                displayText += FormatNotificationWithColor(notification) + "\n";
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
                notificationText.text = $"<color=#00BFFF><b>Flag Monitor Active - Notifications: 0/{MAX_NOTIFICATIONS}</b></color>\n\n<color=#CCCCCC>Notifications cleared.</color>";
                
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
                    string codeLine = $"public static readonly FlagDef {fieldName} = new FlagDef(\"{fieldName}\", null, false, \"{flagType}\");";
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
                        
                        string codeLine = $"public static readonly FlagDef {sceneInstanceName}__{flagIdClean} = new FlagDef(\"{flagId}\", SceneInstances.{sceneInstanceName}, false, \"{flagType}\");";
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

        private static string ExtractSceneNameFromCodeLine(string codeLine)
        {
            try
            {
                // Extract the scene name from a code line like: public static readonly SceneMapData SceneName = new SceneMapData("SceneName", "AreaName");
                int startIndex = codeLine.IndexOf("SceneMapData ") + "SceneMapData ".Length;
                if (startIndex == -1) return null;
                
                int endIndex = codeLine.IndexOf(" =", startIndex);
                if (endIndex == -1) return null;
                
                return codeLine.Substring(startIndex, endIndex - startIndex).Trim();
            }
            catch (Exception ex)
            {
                Debug.LogError($"[Flag Discovery] Failed to extract scene name from code line: {ex.Message}");
                return null;
            }
        }

        private static string ExtractAreaNameFromCodeLine(string codeLine)
        {
            try
            {
                // Extract the area name from a code line like: public static readonly SceneMapData SceneName = new SceneMapData("SceneName", "AreaName");
                int startIndex = codeLine.IndexOf("SceneMapData(\"");
                if (startIndex == -1) return null;
                
                // Find the first closing quote after the scene name
                startIndex = codeLine.IndexOf("\"", startIndex + "SceneMapData(\"".Length);
                if (startIndex == -1) return null;
                
                // Find the second opening quote (area name)
                startIndex = codeLine.IndexOf("\"", startIndex + 1);
                if (startIndex == -1) return null;
                
                // Find the second closing quote
                int endIndex = codeLine.IndexOf("\"", startIndex + 1);
                if (endIndex == -1) return null;
                
                return codeLine.Substring(startIndex + 1, endIndex - startIndex - 1);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[Flag Discovery] Failed to extract area name from code line: {ex.Message}");
                return null;
            }
        }

        public static bool IsEnabled()
        {
            return monitorReference.IsEnabled;
        }

        public static void AddPanel()
        {
            foreach (var panel in CreatePanels())
            {
                CabbyCodesPlugin.cabbyMenu.AddCheatPanel(panel);
            }
        }

        /// <summary>
        /// Creates the panels for the Flag Monitor utility without directly adding them to the menu.
        /// This method mirrors the old AddPanel logic but allows dynamic use in dropdown sections.
        /// </summary>
        public static List<CheatPanel> CreatePanels()
        {
            var panels = new List<CheatPanel>
            {
                new InfoPanel("Flag Monitor").SetColor(CheatPanel.headerColor)
            };

            var monitorToggle = new TogglePanel(monitorReference, "Enable real-time flag change notifications on screen");
            panels.Add(monitorToggle);

            // Add settings section header
            panels.Add(new InfoPanel("Display Settings").SetColor(CheatPanel.subHeaderColor));

            // Create synced references for the new settings using DelegateReference to properly update FlagMonitorSettings
            var showNewDiscoveriesRef = new CabbyMenu.SyncedReferences.DelegateReference<bool>(
                () => FlagMonitorSettings.ShowNewDiscoveries,
                (value) => FlagMonitorSettings.SetShowNewDiscoveries(value));
            
            var showChangedValuesRef = new CabbyMenu.SyncedReferences.DelegateReference<bool>(
                () => FlagMonitorSettings.ShowChangedValues,
                (value) => FlagMonitorSettings.SetShowChangedValues(value));
            
            var showSceneTransitionsRef = new CabbyMenu.SyncedReferences.DelegateReference<bool>(
                () => FlagMonitorSettings.ShowSceneTransitions,
                (value) => FlagMonitorSettings.SetShowSceneTransitions(value));
            
            var includePlayerDataFlagsRef = new CabbyMenu.SyncedReferences.DelegateReference<bool>(
                () => FlagMonitorSettings.IncludePlayerDataFlags,
                (value) => FlagMonitorSettings.SetIncludePlayerDataFlags(value));
            
            var includeSceneFlagsRef = new CabbyMenu.SyncedReferences.DelegateReference<bool>(
                () => FlagMonitorSettings.IncludeSceneFlags,
                (value) => FlagMonitorSettings.SetIncludeSceneFlags(value));

            var includeAllFlagsRef = new CabbyMenu.SyncedReferences.DelegateReference<bool>(
                () => FlagMonitorSettings.IncludeAllFlags,
                (value) => FlagMonitorSettings.SetIncludeAllFlags(value));

            // Add setting toggles
            var showNewDiscoveriesToggle = new TogglePanel(showNewDiscoveriesRef, "Show new discoveries");
            panels.Add(showNewDiscoveriesToggle);

            var showChangedValuesToggle = new TogglePanel(showChangedValuesRef, "Show changed values");
            panels.Add(showChangedValuesToggle);

            var showSceneTransitionsToggle = new TogglePanel(showSceneTransitionsRef, "Show scene transitions");
            panels.Add(showSceneTransitionsToggle);

            var includePlayerDataFlagsToggle = new TogglePanel(includePlayerDataFlagsRef, "Include PlayerData flags");
            panels.Add(includePlayerDataFlagsToggle);

            var includeSceneFlagsToggle = new TogglePanel(includeSceneFlagsRef, "Include Scene flags");
            panels.Add(includeSceneFlagsToggle);

            var includeAllFlagsToggle = new TogglePanel(includeAllFlagsRef, "Include ALL flags (ignored + unused)");
            panels.Add(includeAllFlagsToggle);

            // Add utility section header
            panels.Add(new InfoPanel("Utilities").SetColor(CheatPanel.subHeaderColor));

            var clearButton = new ButtonPanel(() =>
            {
                ClearNotifications();
            }, "Clear Notifications", "Clear all current flag change notifications");
            panels.Add(clearButton);

            var fileLogToggle = new TogglePanel(fileLoggingReference, "Enable logging flag changes to a file in CabbySaves folder");
            panels.Add(fileLogToggle);

            var testButton = new ButtonPanel(() =>
            {
                TestFlagNotifications();
            }, "Test Flag Notifications", "Print example flag change messages to test the monitor display");
            panels.Add(testButton);

            return panels;
        }

        private static int testCounter = 0;
        
        public static void TestFlagNotifications()
        {
            if (!monitorReference.IsEnabled)
            {
                return;
            }
            
            testCounter++;
            
            // Test messages in the same format as the actual monitoring with color coding
            AddNotification($"[SceneChange]: TestScene1 -> TestScene2");
            AddNotification($"[PlayerData]: test_field_{testCounter} = true");
            AddNotification($"[PlayerData]: test_int_{testCounter} = {testCounter * 100}");
            AddNotification($"[Scene: TestScene] test_scene_flag_{testCounter} = true");
            AddNotification($"[Scene: TestScene] GeoRock: test_rock_{testCounter} = Broken");
            AddNotification($"[Scene: TestScene] GeoRock: test_rock_{testCounter + 1} = Intact (3 hits left)");
            AddNotification($"[NEW PLAYERDATA FLAG]: test_new_flag_{testCounter} (Boolean) = true");
            AddNotification($"[NEW SCENE FLAG: TestScene]: test_new_scene_flag_{testCounter} = false");
            
            // Test actual PlayerData field changes
            if (PlayerData.instance != null)
            {
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
                    // Check for critical menu scenes that should completely disable polling
                    string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
                    if (currentScene == SceneInstances.Menu_Title.SceneName || currentScene == SceneInstances.Quit_To_Menu.SceneName)
                    {
                        // We're in a critical menu scene, completely disable polling and clear caches
                        previousPlayerDataValues.Clear();
                        previousSceneDataValues.Clear();
                        isInMenuScene = true;
                        
                        // Skip polling to avoid processing flag resets
                        yield return new WaitForSeconds(0.25f);
                        continue;
                    }

                    // Quick check for time scale (game pause) before doing more expensive checks
                    if (Time.timeScale == 0f)
                    {
                        // Game is paused, skip polling to avoid processing flag resets
                        yield return new WaitForSeconds(0.25f);
                        continue;
                    }

                    // Comprehensive check for whether we should pause flag monitoring
                    if (ShouldPauseFlagMonitoring())
                    {
                        // We're in a state where monitoring should be paused, skip polling
                        yield return new WaitForSeconds(0.25f);
                        continue;
                    }

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
            
            // Check if PlayerData monitoring is enabled
            if (!FlagMonitorSettings.ShouldMonitorPlayerData())
            {
                return;
            }
            
            // Check for critical menu scenes that should completely disable polling
            string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            if (currentScene == SceneInstances.Menu_Title.SceneName || currentScene == SceneInstances.Quit_To_Menu.SceneName)
            {
                return;
            }
            
            // Comprehensive check for whether we should pause flag monitoring
            if (ShouldPauseFlagMonitoring())
            {
                return;
            }
            
            // Increment test counter to track polling calls
            testCounter++;

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
                if (!FlagMonitorSettings.IncludeAllFlags && ignoredFields.Contains(fieldName)) continue;

                try
                {
                    FlagMonitorDiagnostics.RecordFieldAccess(fieldName);
                    
                    object currentValue = field.GetValue(PlayerData.instance);
                    previousPlayerDataValues.TryGetValue(fieldName, out object previousValue);

                    // Check if this is a newly discovered flag (not in current tracking AND not previously discovered)
                    // When IncludeAllFlags is enabled, also check if it's not an ignored field
                    bool isNewFlag = !trackedPlayerDataFields.Contains(fieldName) && 
                                   !discoveredPlayerDataFlags.Contains(fieldName) &&
                                   (!FlagMonitorSettings.IncludeAllFlags || !ignoredFields.Contains(fieldName));
                    
                    // If IncludeAllFlags is enabled and this is an ignored field, add it to discovered set but don't treat as new
                    if (FlagMonitorSettings.IncludeAllFlags && ignoredFields.Contains(fieldName) && !discoveredPlayerDataFlags.Contains(fieldName))
                    {
                        discoveredPlayerDataFlags.Add(fieldName);
                    }
                    
                    if (isNewFlag)
                    {
                        Debug.Log($"[Flag Monitor] NEW FLAG DISCOVERED: {fieldName} (Type: {field.FieldType.Name})");
                        
                        // Check if this complex type should be treated as a special flag type BEFORE regular discovery
                        string flagType = field.FieldType.Name;
                        string fullTypeName = field.FieldType.FullName;
                        
                        // Special handling for BossStatue types
                        bool isBossStatueType = !IsSimpleType(field.FieldType) && 
                                              (flagType == "BossStatue" || 
                                               flagType.EndsWith("BossStatue") ||
                                               (flagType == "Completion" && fullTypeName != null && fullTypeName.Contains("BossStatue")));
                        
                        if (isBossStatueType)
                        {
                            // Handle as BossStatue flag
                            discoveredPlayerDataFlags.Add(fieldName);
                            string notification = $"[NEW BOSSSTATUE FLAG]: {fieldName} (BossStatue) = {currentValue}";
                            AddNotification(notification);
                            
                            // Log to console with copy-paste ready format for BossStatue
                            string codeFormat = $"public static readonly FlagDef {fieldName} = new FlagDef(\"{fieldName}\", null, false, \"PlayerData_BossStatue\");";
                            
                            // Log to file
                            fileLoggingReference.LogMessage($"[DISCOVERY] New BossStatue flag: {codeFormat}");
                            
                            // Store for later reference (both new and historical)
                            newFlagDiscoveries[fieldName] = ("Global", "PlayerData_BossStatue", currentValue);
                            historicalDiscoveries[fieldName] = ("Global", "PlayerData_BossStatue", currentValue);
                        }
                        else
                        {
                            // Handle as regular flag
                            discoveredPlayerDataFlags.Add(fieldName);
                            string notification = $"[NEW PLAYERDATA FLAG]: {fieldName} ({flagType}) = {currentValue}";
                            AddNotification(notification);
                            
                            // Log to console with copy-paste ready format
                            string codeFormat = $"public static readonly FlagDef {fieldName} = new FlagDef(\"{fieldName}\", null, false, \"PlayerData_{flagType}\");";
                            
                            // Log to file
                            fileLoggingReference.LogMessage($"[DISCOVERY] New PlayerData flag: {codeFormat}");
                            
                            // Store for later reference (both new and historical)
                            newFlagDiscoveries[fieldName] = ("Global", $"PlayerData_{flagType}", currentValue);
                            historicalDiscoveries[fieldName] = ("Global", $"PlayerData_{flagType}", currentValue);
                        }
                        
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
                else if (valueType.Name == "BossStatue" || valueType.Name.EndsWith("BossStatue") || 
                        (valueType.Name == "Completion" && valueType.FullName != null && valueType.FullName.Contains("BossStatue")))
                {
                    // Handle BossStatue types specifically for better display
                    try
                    {
                        var hasBeenSeenField = valueType.GetField("hasBeenSeen");
                        var isUnlockedField = valueType.GetField("isUnlocked");
                        var completedTier1Field = valueType.GetField("completedTier1");
                        var completedTier2Field = valueType.GetField("completedTier2");
                        var completedTier3Field = valueType.GetField("completedTier3");
                        
                        if (hasBeenSeenField != null) details.Add($"Seen: {hasBeenSeenField.GetValue(value)}");
                        if (isUnlockedField != null) details.Add($"Unlocked: {isUnlockedField.GetValue(value)}");
                        if (completedTier1Field != null) details.Add($"Tier1: {completedTier1Field.GetValue(value)}");
                        if (completedTier2Field != null) details.Add($"Tier2: {completedTier2Field.GetValue(value)}");
                        if (completedTier3Field != null) details.Add($"Tier3: {completedTier3Field.GetValue(value)}");
                    }
                    catch
                    {
                        // Fallback to generic reflection if specific fields fail
                        details.Add($"BossStatue: {value}");
                    }
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
                else if (valueType.Name == "BossStatue" || valueType.Name.EndsWith("BossStatue") || 
                        (valueType.Name == "Completion" && valueType.FullName != null && valueType.FullName.Contains("BossStatue")))
                {
                    // Handle BossStatue types specifically for better change detection
                    try
                    {
                        var hasBeenSeenField = valueType.GetField("hasBeenSeen");
                        var isUnlockedField = valueType.GetField("isUnlocked");
                        var completedTier1Field = valueType.GetField("completedTier1");
                        var completedTier2Field = valueType.GetField("completedTier2");
                        var completedTier3Field = valueType.GetField("completedTier3");
                        
                        if (hasBeenSeenField != null)
                        {
                            var prevSeen = hasBeenSeenField.GetValue(previousValue);
                            var currSeen = hasBeenSeenField.GetValue(currentValue);
                            if (!Equals(prevSeen, currSeen))
                            {
                                changes.Add($"[PlayerData]: {fieldName}.hasBeenSeen = {currSeen}");
                            }
                        }
                        
                        if (isUnlockedField != null)
                        {
                            var prevUnlocked = isUnlockedField.GetValue(previousValue);
                            var currUnlocked = isUnlockedField.GetValue(currentValue);
                            if (!Equals(prevUnlocked, currUnlocked))
                            {
                                changes.Add($"[PlayerData]: {fieldName}.isUnlocked = {currUnlocked}");
                            }
                        }
                        
                        if (completedTier1Field != null)
                        {
                            var prevTier1 = completedTier1Field.GetValue(previousValue);
                            var currTier1 = completedTier1Field.GetValue(currentValue);
                            if (!Equals(prevTier1, currTier1))
                            {
                                changes.Add($"[PlayerData]: {fieldName}.completedTier1 = {currTier1}");
                            }
                        }
                        
                        if (completedTier2Field != null)
                        {
                            var prevTier2 = completedTier2Field.GetValue(previousValue);
                            var currTier2 = completedTier2Field.GetValue(currentValue);
                            if (!Equals(prevTier2, currTier2))
                            {
                                changes.Add($"[PlayerData]: {fieldName}.completedTier2 = {currTier2}");
                            }
                        }
                        
                        if (completedTier3Field != null)
                        {
                            var prevTier3 = completedTier3Field.GetValue(previousValue);
                            var currTier3 = completedTier3Field.GetValue(currentValue);
                            if (!Equals(prevTier3, currTier3))
                            {
                                changes.Add($"[PlayerData]: {fieldName}.completedTier3 = {currTier3}");
                            }
                        }
                    }
                    catch
                    {
                        // Fallback to generic comparison if specific fields fail
                        if (!Equals(previousValue, currentValue))
                        {
                            changes.Add($"[PlayerData]: {fieldName} = {FormatComplexValue(currentValue, valueType)}");
                        }
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

        /// <summary>
        /// Checks if the game is currently in a state where flag monitoring should be paused
        /// </summary>
        private static bool ShouldPauseFlagMonitoring()
        {
            try
            {
                // Check only against the specific critical menu scenes from SceneInstances
                string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
                if (currentScene == SceneInstances.Quit_To_Menu.SceneName || currentScene == SceneInstances.Menu_Title.SceneName)
                {
                    return true;
                }

                // Check if the game is paused
                if (Time.timeScale == 0f)
                {
                    return true;
                }

                // Check if GameManager exists and is in a paused state
                if (GameManager._instance != null)
                {
                    // Check various pause-related fields in GameManager
                    var gameManagerType = typeof(GameManager);
                    
                    // Check if game is paused
                    var isPausedField = gameManagerType.GetField("isPaused");
                    if (isPausedField != null && isPausedField.FieldType == typeof(bool))
                    {
                        bool isPaused = (bool)isPausedField.GetValue(GameManager._instance);
                        if (isPaused) return true;
                    }

                    // Check if in menu
                    var inMenuField = gameManagerType.GetField("inMenu");
                    if (inMenuField != null && inMenuField.FieldType == typeof(bool))
                    {
                        bool inMenu = (bool)inMenuField.GetValue(GameManager._instance);
                        if (inMenu) return true;
                    }

                    // Check if game state is menu
                    var gameStateField = gameManagerType.GetField("gameState");
                    if (gameStateField != null)
                    {
                        var gameState = gameStateField.GetValue(GameManager._instance);
                        if (gameState != null && gameState.ToString().Contains("MENU"))
                        {
                            return true;
                        }
                    }
                }

                // Check if PlayerData is in a menu state
                if (PlayerData.instance != null)
                {
                    var playerDataType = typeof(PlayerData);
                    
                    // Check if player is in a menu
                    var inMenuField = playerDataType.GetField("inMenu");
                    if (inMenuField != null && inMenuField.FieldType == typeof(bool))
                    {
                        bool inMenu = (bool)inMenuField.GetValue(PlayerData.instance);
                        if (inMenu) return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                // If any reflection fails, assume we should pause monitoring to be safe
                Debug.LogWarning($"[Flag Monitor] Error checking pause state: {ex.Message}");
                return true;
            }
        }

        private static void PollSceneDataFields()
        {
            if (SceneData.instance == null) return;

            // Check if Scene monitoring is enabled
            if (!FlagMonitorSettings.ShouldMonitorScenes())
            {
                return;
            }

            // Check for critical menu scenes that should completely disable polling
            string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            if (currentScene == SceneInstances.Menu_Title.SceneName || currentScene == SceneInstances.Quit_To_Menu.SceneName)
            {
                return;
            }

            // Comprehensive check for whether we should pause flag monitoring
            if (ShouldPauseFlagMonitoring())
            {
                return;
            }

            // Check for instance change
            if (SceneData.instance != lastKnownSceneDataInstance)
            {
                HandleSceneDataInstanceChange();
            }

            // Poll all scene flags dynamically (including predefined flags)
            // This prevents duplicate discovery by having only one place where flags are discovered
            PollAllSceneFlags();
        }

        /// <summary>
        /// Polls ALL scene flags dynamically, including those not defined in FlagInstances
        /// </summary>
        private static void PollAllSceneFlags()
        {
            // Check for critical menu scenes that should completely disable polling
            string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            if (currentScene == SceneInstances.Menu_Title.SceneName || currentScene == SceneInstances.Quit_To_Menu.SceneName)
            {
                return;
            }
            
            // Comprehensive check for whether we should pause flag monitoring
            if (ShouldPauseFlagMonitoring())
            {
                return;
            }
            
            // Poll all PersistentBoolData entries
            if (SceneData.instance.persistentBoolItems != null)
            {
                foreach (var pbd in SceneData.instance.persistentBoolItems)
                {
                    string key = $"PersistentBool_{pbd.id}_{pbd.sceneName}";
                    bool currentValue = pbd.activated;
                    previousSceneDataValues.TryGetValue(key, out object previousValue);

                    // Check if this is a newly discovered flag (not in current tracking AND not previously discovered AND not in unused flags)
                    // Also check against historical discoveries to prevent duplicates from discovered_flags.txt
                    string uniqueKey = $"{pbd.id}|||{pbd.sceneName}";
                    
                    bool isNewFlag = !trackedSceneDataFields.Contains(uniqueKey) && 
                                   !discoveredSceneFlags.Contains(uniqueKey) &&
                                   !historicalDiscoveries.ContainsKey(uniqueKey) &&
                                   (FlagMonitorSettings.IncludeAllFlags || !IsFlagInUnusedFlags(pbd.id, pbd.sceneName));
                    

                    
                    if (isNewFlag)
                    {
                        discoveredSceneFlags.Add(uniqueKey);
                        string notification = $"[NEW SCENE FLAG: {pbd.sceneName}]: {pbd.id} = {currentValue}";
                        AddNotification(notification);
                        
                        // Log to console with copy-paste ready format
                        string codeFormat = $"public static readonly FlagDef {pbd.sceneName.Replace("-", "_")}__{pbd.id.Replace(" ", "_")} = new FlagDef(\"{pbd.id}\", SceneInstances.{pbd.sceneName.Replace("-", "_")}, false, \"PersistentBoolData\");";
                        
                        // Log to file
                        fileLoggingReference.LogMessage($"[DISCOVERY] New Scene flag: {codeFormat}");
                        
                        // Store for later reference (both new and historical)
                        // Use unique key combining flag ID and scene name to avoid duplicates
                        // Use a special delimiter that won't appear in flag IDs or scene names
                        newFlagDiscoveries[uniqueKey] = (pbd.sceneName, "PersistentBoolData", currentValue);
                        historicalDiscoveries[uniqueKey] = (pbd.sceneName, "PersistentBoolData", currentValue);
                        
                        // Auto-add to tracking sets for immediate monitoring
                        trackedSceneDataFields.Add(uniqueKey);
                        
                        // Auto-update discovery file on every discovery
                        SaveDiscoveredFlags(); // Save discovered flags immediately
                    }

                    // Handle value changes for all flags (both new and existing)
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
                        // Skip value change notifications for unused flags (unless IncludeAllFlags is enabled)
                        if (!FlagMonitorSettings.IncludeAllFlags && IsFlagInUnusedFlags(pbd.id, pbd.sceneName))
                        {
                            // Still update the previous value to avoid repeated checks
                            previousSceneDataValues[key] = currentValue;
                            continue;
                        }
                        
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

                    // Check if this is a newly discovered flag (not in current tracking AND not previously discovered AND not in unused flags)
                    // Also check against historical discoveries to prevent duplicates from discovered_flags.txt
                    string uniqueKey = $"{pid.id}|||{pid.sceneName}";
                    
                    bool isNewFlag = !trackedSceneDataFields.Contains(uniqueKey) && 
                                   !discoveredSceneFlags.Contains(uniqueKey) &&
                                   !historicalDiscoveries.ContainsKey(uniqueKey) &&
                                   (FlagMonitorSettings.IncludeAllFlags || !IsFlagInUnusedFlags(pid.id, pid.sceneName));
                    
                    if (isNewFlag)
                    {
                        discoveredSceneFlags.Add(uniqueKey);
                        string notification = $"[NEW SCENE FLAG: {pid.sceneName}]: {pid.id} = {currentValue}";
                        AddNotification(notification);
                        
                        // Log to console with copy-paste ready format
                        string codeFormat = $"public static readonly FlagDef {pid.sceneName.Replace("-", "_")}__{pid.id.Replace(" ", "_")} = new FlagDef(\"{pid.id}\", SceneInstances.{pid.sceneName.Replace("-", "_")}, false, \"PersistentIntData\");";
                        
                        // Log to file
                        fileLoggingReference.LogMessage($"[DISCOVERY] New Scene flag: {codeFormat}");
                        
                        // Store for later reference (both new and historical)
                        // Use unique key combining flag ID and scene name to avoid duplicates
                        // Use a special delimiter that won't appear in flag IDs or scene names
                        newFlagDiscoveries[uniqueKey] = (pid.sceneName, "PersistentIntData", currentValue);
                        historicalDiscoveries[uniqueKey] = (pid.sceneName, "PersistentIntData", currentValue);
                        
                        // Auto-add to tracking sets for immediate monitoring
                        trackedSceneDataFields.Add(uniqueKey);
                        
                        // Auto-update discovery file on every discovery
                        SaveDiscoveredFlags(); // Save discovered flags immediately
                    }

                    // Handle value changes for all flags (both new and existing)
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
                        // Skip value change notifications for unused flags (unless IncludeAllFlags is enabled)
                        if (!FlagMonitorSettings.IncludeAllFlags && IsFlagInUnusedFlags(pid.id, pid.sceneName))
                        {
                            // Still update the previous value to avoid repeated checks
                            previousSceneDataValues[key] = currentValue;
                            continue;
                        }
                        
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

                    // Check if this is a newly discovered flag (not in current tracking AND not previously discovered AND not in unused flags)
                    // Also check against historical discoveries to prevent duplicates from discovered_flags.txt
                    string uniqueKey = $"{grd.id}|||{grd.sceneName}";
                    
                    bool isNewFlag = !trackedSceneDataFields.Contains(uniqueKey) && 
                                   !discoveredSceneFlags.Contains(uniqueKey) &&
                                   !historicalDiscoveries.ContainsKey(uniqueKey) &&
                                   (FlagMonitorSettings.IncludeAllFlags || !IsFlagInUnusedFlags(grd.id, grd.sceneName));
                    
                    if (isNewFlag)
                    {
                        discoveredSceneFlags.Add(uniqueKey);
                        string status = currentValue ? "Broken" : $"Intact ({currentHitsLeft} hits left)";
                        string notification = $"[NEW SCENE FLAG: {grd.sceneName}]: GeoRock {grd.id} = {status}";
                        AddNotification(notification);
                        
                        // Log to console with copy-paste ready format
                        string codeFormat = $"public static readonly FlagDef {grd.sceneName.Replace("-", "_")}__Geo_Rock_{grd.id.Replace(" ", "_")} = new FlagDef(\"{grd.id}\", SceneInstances.{grd.sceneName.Replace("-", "_")}, false, \"GeoRockData\");";
                        
                        // Log to file
                        fileLoggingReference.LogMessage($"[DISCOVERY] New GeoRock flag: {codeFormat}");
                        
                        // Store for later reference (both new and historical)
                        // Use unique key combining flag ID and scene name to avoid duplicates
                        // Use a special delimiter that won't appear in flag IDs or scene names
                        newFlagDiscoveries[uniqueKey] = (grd.sceneName, "GeoRockData", currentValue);
                        historicalDiscoveries[uniqueKey] = (grd.sceneName, "GeoRockData", currentValue);
                        
                        // Auto-add to tracking sets for immediate monitoring
                        trackedSceneDataFields.Add(uniqueKey);
                        
                        // Auto-update discovery file on every discovery
                        SaveDiscoveredFlags(); // Save discovered flags immediately
                    }

                    // Handle value changes for all flags (both new and existing)
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
                        // Skip value change notifications for unused flags (unless IncludeAllFlags is enabled)
                        if (!FlagMonitorSettings.IncludeAllFlags && IsFlagInUnusedFlags(grd.id, grd.sceneName))
                        {
                            // Still update the previous value to avoid repeated checks
                            previousSceneDataValues[key] = currentValue;
                            continue;
                        }
                        
                        previousSceneDataValues[key] = currentValue;
                        string status = currentValue ? "Broken" : $"Intact ({currentHitsLeft} hits left)";
                        string notification = $"[Scene: {grd.sceneName}] GeoRock: {grd.id} = {status}";
                        AddNotification(notification);
                    }
                }
            }
        }






    }
} 