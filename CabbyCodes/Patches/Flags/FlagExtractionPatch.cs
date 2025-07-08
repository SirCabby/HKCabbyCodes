using CabbyMenu.UI.CheatPanels;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CabbyCodes.Patches.Flags
{
    public class FlagExtractionPatch
    {
        public static void AddPanel()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Flag Extraction").SetColor(CheatPanel.headerColor));
            
            var extractButton = new ButtonPanel(() =>
            {
                try
                {
                    ExtractAllFlags();
                    Debug.Log("Flag extraction completed! Check CabbySaves folder for all_flags_report.txt");
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"Flag extraction failed: {ex.Message}");
                }
            }, "Extract All Flags", "Extract all game flags to a comprehensive report file");
            
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(extractButton);
        }

        public static void ExtractAllFlags()
        {
            var allFlags = new List<FlagData>();
            
            // Extract PersistentBoolData flags
            ExtractPersistentBoolFlags(allFlags);
            
            // Extract PersistentIntData flags  
            ExtractPersistentIntFlags(allFlags);
            
            // Extract GeoRock flags
            ExtractGeoRockFlags(allFlags);
            
            // Extract PlayerData flags
            ExtractPlayerDataFlags(allFlags);
            
            // Write results to file
            WriteFlagReport(allFlags);
        }
        
        private static void ExtractPersistentBoolFlags(List<FlagData> allFlags)
        {
            if (SceneData.instance?.persistentBoolItems == null) return;
            
            foreach (var pbd in SceneData.instance.persistentBoolItems)
            {
                allFlags.Add(new FlagData(
                    pbd.id,
                    pbd.sceneName,
                    pbd.semiPersistent,
                    "PersistentBoolData"
                ));
            }
        }
        
        private static void ExtractPersistentIntFlags(List<FlagData> allFlags)
        {
            if (SceneData.instance?.persistentIntItems == null) return;
            
            foreach (var pid in SceneData.instance.persistentIntItems)
            {
                allFlags.Add(new FlagData(
                    pid.id,
                    pid.sceneName,
                    pid.semiPersistent,
                    "PersistentIntData"
                ));
            }
        }
        
        private static void ExtractGeoRockFlags(List<FlagData> allFlags)
        {
            if (SceneData.instance?.geoRocks == null) return;
            
            foreach (var grd in SceneData.instance.geoRocks)
            {
                allFlags.Add(new FlagData(
                    grd.id,
                    grd.sceneName,
                    false,
                    "GeoRockData"
                ));
            }
        }
        
        private static void ExtractPlayerDataFlags(List<FlagData> allFlags)
        {
            if (PlayerData.instance == null) return;
            
            var playerDataType = typeof(PlayerData);
            var fields = playerDataType.GetFields(BindingFlags.Public | BindingFlags.Instance);
            
            foreach (var field in fields)
            {
                if (field.FieldType == typeof(bool))
                {
                    allFlags.Add(new FlagData(
                        field.Name,
                        "Global",
                        false,
                        "PlayerData_Bool"
                    ));
                }
                else if (field.FieldType == typeof(int))
                {
                    allFlags.Add(new FlagData(
                        field.Name,
                        "Global",
                        false,
                        "PlayerData_Int"
                    ));
                }
            }
        }
        
        private static void WriteFlagReport(List<FlagData> allFlags)
        {
            string outputPath = Path.Combine(Application.persistentDataPath, "CabbySaves", "all_flags_report.txt");
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
            
            using (var writer = new StreamWriter(outputPath))
            {
                writer.WriteLine($"Hollow Knight Complete Flag Report");
                writer.WriteLine($"Generated: {DateTime.Now}");
                writer.WriteLine($"Total Flags Found: {allFlags.Count}");
                writer.WriteLine("=".PadLeft(80, '='));
                writer.WriteLine();
                
                // Group by type
                var groupedFlags = allFlags.GroupBy(f => f.Type).OrderBy(g => g.Key);
                
                foreach (var group in groupedFlags)
                {
                    writer.WriteLine($"## {group.Key} ({group.Count()} flags)");
                    writer.WriteLine();
                    
                    foreach (var flag in group.OrderBy(f => f.SceneName).ThenBy(f => f.Id))
                    {
                        writer.WriteLine($"ID: {flag.Id}");
                        writer.WriteLine($"Scene: {flag.SceneName}");
                        writer.WriteLine($"SemiPersistent: {flag.SemiPersistent}");
                        writer.WriteLine();
                    }
                    writer.WriteLine("-".PadLeft(40, '-'));
                    writer.WriteLine();
                }
            }
        }
    }
} 