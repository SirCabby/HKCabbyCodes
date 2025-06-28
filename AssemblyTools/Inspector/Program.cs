using System;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;

namespace AssemblyInspector
{
    public class TypeInfo
    {
        public string FullName { get; set; }
        public string Namespace { get; set; }
        public string Name { get; set; }
        public bool IsPublic { get; set; }
        public bool IsClass { get; set; }
        public bool IsEnum { get; set; }
        public bool IsInterface { get; set; }
        public bool IsAbstract { get; set; }
        public bool IsSealed { get; set; }
        public string BaseType { get; set; }
        public List<string> Interfaces { get; set; } = new List<string>();
        public List<MethodInfo> Methods { get; set; } = new List<MethodInfo>();
        public List<PropertyInfo> Properties { get; set; } = new List<PropertyInfo>();
        public List<FieldInfo> Fields { get; set; } = new List<FieldInfo>();
    }

    public class MethodInfo
    {
        public string Name { get; set; }
        public bool IsPublic { get; set; }
        public bool IsStatic { get; set; }
        public string ReturnType { get; set; }
        public List<string> Parameters { get; set; } = new List<string>();
    }

    public class PropertyInfo
    {
        public string Name { get; set; }
        public bool IsPublic { get; set; }
        public bool IsStatic { get; set; }
        public string PropertyType { get; set; }
        public bool HasGetter { get; set; }
        public bool HasSetter { get; set; }
    }

    public class FieldInfo
    {
        public string Name { get; set; }
        public bool IsPublic { get; set; }
        public bool IsStatic { get; set; }
        public string FieldType { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string assemblyPath = args.Length > 0 ? args[0] : "Assembly-CSharp.dll";
            string outputFile = "assembly_analysis.json";
            
            if (!File.Exists(assemblyPath))
            {
                Console.WriteLine($"Assembly file not found: {assemblyPath}");
                Console.WriteLine("Usage: AssemblyInspector.exe [path-to-assembly]");
                return;
            }

            try
            {
                Console.WriteLine($"Analyzing assembly: {assemblyPath}");
                
                // Load the assembly
                Assembly assembly = Assembly.LoadFile(Path.GetFullPath(assemblyPath));
                
                var analysis = new
                {
                    AssemblyInfo = new
                    {
                        FullName = assembly.FullName,
                        Location = assembly.Location,
                        Version = assembly.GetName().Version?.ToString(),
                        LoadedAt = DateTime.Now
                    },
                    Types = ExtractTypes(assembly)
                };

                // Write to JSON file
                string json = JsonSerializer.Serialize(analysis, new JsonSerializerOptions 
                { 
                    WriteIndented = true,
                    MaxDepth = 10
                });
                
                File.WriteAllText(outputFile, json);
                Console.WriteLine($"Analysis complete. Results saved to: {outputFile}");
                Console.WriteLine($"Total types analyzed: {analysis.Types.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error analyzing assembly: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }

        static List<TypeInfo> ExtractTypes(Assembly assembly)
        {
            var types = new List<TypeInfo>();
            
            try
            {
                Type[] assemblyTypes = assembly.GetTypes();
                
                foreach (Type type in assemblyTypes.OrderBy(t => t.FullName))
                {
                    try
                    {
                        var typeInfo = new TypeInfo
                        {
                            FullName = type.FullName ?? type.Name,
                            Namespace = type.Namespace ?? "",
                            Name = type.Name,
                            IsPublic = type.IsPublic,
                            IsClass = type.IsClass,
                            IsEnum = type.IsEnum,
                            IsInterface = type.IsInterface,
                            IsAbstract = type.IsAbstract,
                            IsSealed = type.IsSealed,
                            BaseType = type.BaseType?.FullName ?? ""
                        };

                        // Extract interfaces
                        foreach (var interfaceType in type.GetInterfaces())
                        {
                            typeInfo.Interfaces.Add(interfaceType.FullName ?? interfaceType.Name);
                        }

                        // Extract methods
                        foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
                        {
                            if (method.DeclaringType == type) // Only include methods declared in this type
                            {
                                var methodInfo = new MethodInfo
                                {
                                    Name = method.Name,
                                    IsPublic = method.IsPublic,
                                    IsStatic = method.IsStatic,
                                    ReturnType = method.ReturnType.FullName ?? method.ReturnType.Name
                                };

                                foreach (var param in method.GetParameters())
                                {
                                    methodInfo.Parameters.Add($"{param.ParameterType.Name} {param.Name}");
                                }

                                typeInfo.Methods.Add(methodInfo);
                            }
                        }

                        // Extract properties
                        foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
                        {
                            if (property.DeclaringType == type) // Only include properties declared in this type
                            {
                                var propertyInfo = new PropertyInfo
                                {
                                    Name = property.Name,
                                    IsPublic = property.GetGetMethod()?.IsPublic ?? false,
                                    IsStatic = property.GetGetMethod()?.IsStatic ?? false,
                                    PropertyType = property.PropertyType.FullName ?? property.PropertyType.Name,
                                    HasGetter = property.CanRead,
                                    HasSetter = property.CanWrite
                                };

                                typeInfo.Properties.Add(propertyInfo);
                            }
                        }

                        // Extract fields
                        foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
                        {
                            if (field.DeclaringType == type) // Only include fields declared in this type
                            {
                                var fieldInfo = new FieldInfo
                                {
                                    Name = field.Name,
                                    IsPublic = field.IsPublic,
                                    IsStatic = field.IsStatic,
                                    FieldType = field.FieldType.FullName ?? field.FieldType.Name
                                };

                                typeInfo.Fields.Add(fieldInfo);
                            }
                        }

                        types.Add(typeInfo);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing type {type.FullName}: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error extracting types: {ex.Message}");
            }

            return types;
        }
    }
} 