# PowerShell script to find missing grubs
$assemblyPath = "CabbyCodes\bin\Release\net472\Assembly-CSharp.dll"
$outputFile = "all_scenes.txt"

# Extract all scene names from the assembly
Write-Host "Extracting scene names from Assembly-CSharp.dll..."
$sceneNames = dotnet-ildasm $assemblyPath | Select-String "ldstr.*_[0-9]" | ForEach-Object {
    if ($_ -match 'ldstr "([^"]+)"') {
        $matches[1]
    }
} | Sort-Object -Unique

# Write all scene names to file
$sceneNames | Out-File -FilePath $outputFile -Encoding UTF8

# Current grub scenes from GrubPatch.cs
$currentGrubScenes = @(
    "Abyss_17",
    "Abyss_19", 
    "Crossroads_03",
    "Crossroads_05",
    "Crossroads_31",
    "Crossroads_35",
    "Crossroads_48",
    "Deepnest_03",
    "Deepnest_31",
    "Deepnest_36",
    "Deepnest_39",
    "Deepnest_East_11",
    "Deepnest_East_14",
    "Deepnest_Spider_Town",
    "Fungus1_06",
    "Fungus1_07",
    "Fungus1_13",
    "Fungus1_21",
    "Fungus1_28",
    "Fungus2_18",
    "Fungus2_20",
    "Fungus3_10",
    "Fungus3_22",
    "Fungus3_47",
    "Fungus3_48",
    "Hive_03",
    "Hive_04",
    "Mines_03",
    "Mines_04",
    "Mines_16",
    "Mines_19",
    "Mines_24",
    "Mines_31",
    "Mines_35",
    "RestingGrounds_10",
    "Ruins_House_01",
    "Ruins1_05",
    "Ruins1_32",
    "Ruins2_03",
    "Ruins2_07",
    "Ruins2_11",
    "Waterways_04",
    "Waterways_13",
    "Waterways_14"
)

Write-Host "Current grub scenes count: $($currentGrubScenes.Count)"
Write-Host "Total scene names found: $($sceneNames.Count)"

# Find potential missing grub scenes
# Look for scenes that follow the pattern but aren't in the current list
$potentialMissing = @()

foreach ($scene in $sceneNames) {
    # Skip scenes that are clearly not grub locations (boss rooms, special areas, etc.)
    if ($scene -match "Tutorial|GG_|Colosseum|White_Palace|Dream_|Room_|Boss|Archive" -or
        $scene -match "_b$|_c$|_d$|_top|_bot|_left|_right|_alt|_v02|_part_") {
        continue
    }
    
    # Check if this scene is not in the current grub list
    if ($scene -notin $currentGrubScenes) {
        $potentialMissing += $scene
    }
}

Write-Host "`nPotential missing grub scenes:"
$potentialMissing | ForEach-Object { Write-Host "  $_" }

Write-Host "`nAnalysis complete. Check $outputFile for all scene names." 