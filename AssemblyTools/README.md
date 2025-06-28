# Assembly Tools

This directory contains tools for analyzing and working with the Hollow Knight Assembly-CSharp.dll file.

## Structure

```
AssemblyTools/
├── Inspector/          # C# assembly analysis tool
│   ├── Program.cs      # Main assembly inspector
│   ├── AssemblyInspector.csproj
│   ├── find_missing_grubs.ps1
│   └── analyze_missing_grubs.ps1
└── Reference/          # Reference documentation
    ├── assembly_reference.md
    ├── quick_reference.txt
    └── all_scenes.txt
```

## Inspector Tool

The Inspector tool is a C# console application that can analyze .NET assemblies and extract detailed information about their types, methods, properties, and fields.

### Building the Inspector

```bash
cd Inspector
dotnet build
```

### Using the Inspector

```bash
# Analyze the default Assembly-CSharp.dll in current directory
dotnet run

# Analyze a specific assembly file
dotnet run "path/to/your/assembly.dll"
```

The tool will generate an `assembly_analysis.json` file with detailed information about the assembly structure.

## PowerShell Scripts

### find_missing_grubs.ps1
Extracts all scene names from the Assembly-CSharp.dll and compares them with the current grub list to identify missing locations.

### analyze_missing_grubs.ps1
Performs a targeted analysis to identify the most likely missing grub locations based on scene patterns.

## Reference Files

### assembly_reference.md
Comprehensive reference document with detailed class descriptions, methods, and modding patterns for the Assembly-CSharp.dll.

### quick_reference.txt
Quick lookup file with class names, purposes, and common patch targets.

### all_scenes.txt
Complete list of all scene names extracted from the Assembly-CSharp.dll.

## Usage Examples

### Finding Missing Grubs
```powershell
cd Inspector
powershell -ExecutionPolicy Bypass -File find_missing_grubs.ps1
```

### Analyzing Assembly Structure
```bash
cd Inspector
dotnet run "../CabbyCodes/bin/Release/net472/Assembly-CSharp.dll"
```

## Notes

- The Inspector tool requires .NET 6.0 or later
- PowerShell scripts require execution policy to be set to allow script execution
- Assembly analysis may take some time for large assemblies
- Some assemblies may have dependencies that prevent full loading 