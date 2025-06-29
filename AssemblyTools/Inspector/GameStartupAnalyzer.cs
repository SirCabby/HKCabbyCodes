using System;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace AssemblyInspector
{
    public class GameStartupAnalyzer
    {
        public static void Main(string[] args)
        {
            string assemblyPath = args.Length > 0 ? args[0] : "../../CabbyCodes/lib/Assembly-CSharp.dll";
            string outputFile = "../../Input/game_startup_analysis.txt";
            
            if (!File.Exists(assemblyPath))
            {
                Console.WriteLine($"Assembly file not found: {assemblyPath}");
                return;
            }

            try
            {
                Console.WriteLine($"Analyzing game startup in assembly: {assemblyPath}");
                
                var analysis = new StringBuilder();
                analysis.AppendLine("=== HOLLOW KNIGHT GAME STARTUP ANALYSIS ===");
                analysis.AppendLine($"Generated: {DateTime.Now}");
                analysis.AppendLine();

                // Load the assembly with reflection-only loading to avoid dependency issues
                Assembly assembly = Assembly.ReflectionOnlyLoadFrom(Path.GetFullPath(assemblyPath));
                
                // Analyze key startup-related classes
                AnalyzeGameManager(assembly, analysis);
                AnalyzeSceneManagement(assembly, analysis);
                AnalyzeLoadingScreens(assembly, analysis);
                AnalyzeSaveSystem(assembly, analysis);
                AnalyzeIntroSequences(assembly, analysis);

                // Write to file
                File.WriteAllText(outputFile, analysis.ToString());
                Console.WriteLine($"Analysis complete. Results saved to: {outputFile}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error analyzing assembly: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }

        private static void AnalyzeGameManager(Assembly assembly, StringBuilder analysis)
        {
            analysis.AppendLine("=== GAME MANAGER ANALYSIS ===");
            
            try
            {
                Type gameManagerType = assembly.GetType("GameManager");
                if (gameManagerType != null)
                {
                    analysis.AppendLine($"Found GameManager class: {gameManagerType.FullName}");
                    analysis.AppendLine();

                    // Analyze methods
                    analysis.AppendLine("Key Methods:");
                    var methods = gameManagerType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                    foreach (var method in methods.OrderBy(m => m.Name))
                    {
                        if (IsStartupRelated(method.Name))
                        {
                            analysis.AppendLine($"  {method.Name}({string.Join(", ", method.GetParameters().Select(p => p.ParameterType.Name))}) -> {method.ReturnType.Name}");
                        }
                    }
                    analysis.AppendLine();

                    // Analyze properties
                    analysis.AppendLine("Key Properties:");
                    var properties = gameManagerType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                    foreach (var prop in properties.OrderBy(p => p.Name))
                    {
                        if (IsStartupRelated(prop.Name))
                        {
                            analysis.AppendLine($"  {prop.Name} ({prop.PropertyType.Name})");
                        }
                    }
                    analysis.AppendLine();
                }
                else
                {
                    analysis.AppendLine("GameManager class not found!");
                }
            }
            catch (Exception ex)
            {
                analysis.AppendLine($"Error analyzing GameManager: {ex.Message}");
            }
            analysis.AppendLine();
        }

        private static void AnalyzeSceneManagement(Assembly assembly, StringBuilder analysis)
        {
            analysis.AppendLine("=== SCENE MANAGEMENT ANALYSIS ===");
            
            try
            {
                // Look for scene-related classes
                var sceneTypes = assembly.GetTypes().Where(t => 
                    t.Name.Contains("Scene") || 
                    t.Name.Contains("Load") || 
                    t.Name.Contains("Transition") ||
                    t.Name.Contains("Manager") && (t.Name.Contains("Scene") || t.Name.Contains("Load")))
                    .OrderBy(t => t.Name);

                foreach (var type in sceneTypes)
                {
                    analysis.AppendLine($"Scene-related class: {type.FullName}");
                    
                    // Show key methods
                    var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                    var keyMethods = methods.Where(m => 
                        m.Name.Contains("Load") || 
                        m.Name.Contains("Scene") || 
                        m.Name.Contains("Transition") ||
                        m.Name.Contains("Start") ||
                        m.Name.Contains("Begin"))
                        .OrderBy(m => m.Name);

                    foreach (var method in keyMethods.Take(10)) // Limit to avoid overwhelming output
                    {
                        analysis.AppendLine($"  {method.Name}({string.Join(", ", method.GetParameters().Select(p => p.ParameterType.Name))}) -> {method.ReturnType.Name}");
                    }
                    analysis.AppendLine();
                }
            }
            catch (Exception ex)
            {
                analysis.AppendLine($"Error analyzing scene management: {ex.Message}");
            }
            analysis.AppendLine();
        }

        private static void AnalyzeLoadingScreens(Assembly assembly, StringBuilder analysis)
        {
            analysis.AppendLine("=== LOADING SCREEN ANALYSIS ===");
            
            try
            {
                // Look for loading screen related classes
                var loadingTypes = assembly.GetTypes().Where(t => 
                    t.Name.Contains("Loading") || 
                    t.Name.Contains("Screen") ||
                    t.Name.Contains("Splash") ||
                    t.Name.Contains("Intro"))
                    .OrderBy(t => t.Name);

                foreach (var type in loadingTypes)
                {
                    analysis.AppendLine($"Loading-related class: {type.FullName}");
                    
                    // Show key methods
                    var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                    var keyMethods = methods.Where(m => 
                        m.Name.Contains("Start") || 
                        m.Name.Contains("Load") || 
                        m.Name.Contains("Show") ||
                        m.Name.Contains("Hide") ||
                        m.Name.Contains("Update"))
                        .OrderBy(m => m.Name);

                    foreach (var method in keyMethods.Take(10))
                    {
                        analysis.AppendLine($"  {method.Name}({string.Join(", ", method.GetParameters().Select(p => p.ParameterType.Name))}) -> {method.ReturnType.Name}");
                    }
                    analysis.AppendLine();
                }
            }
            catch (Exception ex)
            {
                analysis.AppendLine($"Error analyzing loading screens: {ex.Message}");
            }
            analysis.AppendLine();
        }

        private static void AnalyzeSaveSystem(Assembly assembly, StringBuilder analysis)
        {
            analysis.AppendLine("=== SAVE SYSTEM ANALYSIS ===");
            
            try
            {
                // Look for save-related classes
                var saveTypes = assembly.GetTypes().Where(t => 
                    t.Name.Contains("Save") || 
                    t.Name.Contains("Game") && t.Name.Contains("Data") ||
                    t.Name.Contains("Profile"))
                    .OrderBy(t => t.Name);

                foreach (var type in saveTypes)
                {
                    analysis.AppendLine($"Save-related class: {type.FullName}");
                    
                    // Show key methods
                    var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                    var keyMethods = methods.Where(m => 
                        m.Name.Contains("Load") || 
                        m.Name.Contains("Save") || 
                        m.Name.Contains("Game") ||
                        m.Name.Contains("Profile"))
                        .OrderBy(m => m.Name);

                    foreach (var method in keyMethods.Take(10))
                    {
                        analysis.AppendLine($"  {method.Name}({string.Join(", ", method.GetParameters().Select(p => p.ParameterType.Name))}) -> {method.ReturnType.Name}");
                    }
                    analysis.AppendLine();
                }
            }
            catch (Exception ex)
            {
                analysis.AppendLine($"Error analyzing save system: {ex.Message}");
            }
            analysis.AppendLine();
        }

        private static void AnalyzeIntroSequences(Assembly assembly, StringBuilder analysis)
        {
            analysis.AppendLine("=== INTRO SEQUENCE ANALYSIS ===");
            
            try
            {
                // Look for intro-related classes
                var introTypes = assembly.GetTypes().Where(t => 
                    t.Name.Contains("Intro") || 
                    t.Name.Contains("Opening") ||
                    t.Name.Contains("Tutorial") ||
                    t.Name.Contains("Menu"))
                    .OrderBy(t => t.Name);

                foreach (var type in introTypes)
                {
                    analysis.AppendLine($"Intro-related class: {type.FullName}");
                    
                    // Show key methods
                    var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                    var keyMethods = methods.Where(m => 
                        m.Name.Contains("Start") || 
                        m.Name.Contains("Begin") || 
                        m.Name.Contains("Show") ||
                        m.Name.Contains("Skip") ||
                        m.Name.Contains("Update"))
                        .OrderBy(m => m.Name);

                    foreach (var method in keyMethods.Take(10))
                    {
                        analysis.AppendLine($"  {method.Name}({string.Join(", ", method.GetParameters().Select(p => p.ParameterType.Name))}) -> {method.ReturnType.Name}");
                    }
                    analysis.AppendLine();
                }
            }
            catch (Exception ex)
            {
                analysis.AppendLine($"Error analyzing intro sequences: {ex.Message}");
            }
            analysis.AppendLine();
        }

        private static bool IsStartupRelated(string name)
        {
            string lowerName = name.ToLower();
            return lowerName.Contains("start") || 
                   lowerName.Contains("load") || 
                   lowerName.Contains("scene") || 
                   lowerName.Contains("game") ||
                   lowerName.Contains("save") ||
                   lowerName.Contains("intro") ||
                   lowerName.Contains("menu") ||
                   lowerName.Contains("transition");
        }
    }
} 