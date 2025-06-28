# More targeted analysis for missing grubs
Write-Host "Analyzing potential missing grub locations..."

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

# Based on the assembly analysis, here are the most likely missing grub locations
# These are scenes that follow the same pattern as known grub locations
$likelyMissingGrubs = @(
    "Cliffs_01",      # Cliffs area - likely has a grub
    "Cliffs_02",      # Cliffs area - likely has a grub
    "Fungus1_15",     # Fungus area - follows pattern of other Fungus1 grubs
    "Fungus2_25",     # Fungus area - follows pattern of other Fungus2 grubs
    "Fungus3_25",     # Fungus area - follows pattern of other Fungus3 grubs
    "Hive_01",        # Hive area - follows pattern of other Hive grubs
    "Hive_02",        # Hive area - follows pattern of other Hive grubs
    "Mines_01",       # Mines area - follows pattern of other Mines grubs
    "Mines_02",       # Mines area - follows pattern of other Mines grubs
    "Ruins1_01",      # Ruins area - follows pattern of other Ruins1 grubs
    "Ruins1_02",      # Ruins area - follows pattern of other Ruins1 grubs
    "Ruins2_01",      # Ruins area - follows pattern of other Ruins2 grubs
    "Waterways_01",   # Waterways area - follows pattern of other Waterways grubs
    "Waterways_02"    # Waterways area - follows pattern of other Waterways grubs
)

Write-Host "`nMost likely missing grub locations:"
foreach ($scene in $likelyMissingGrubs) {
    if ($scene -notin $currentGrubScenes) {
        Write-Host "  $scene"
    }
}

# Let me also check for any scenes that might be named differently
Write-Host "`nChecking for potential naming variations..."

# Look for scenes that might be the missing ones
$potentialVariations = @(
    "Deepnest_Spider_Town",  # This might be "Deepnest_Spider_Town" instead of a numbered scene
    "Fungus1_35",           # Higher numbered Fungus1 scene
    "Fungus1_36",           # Higher numbered Fungus1 scene
    "Fungus3_49",           # Higher numbered Fungus3 scene
    "Fungus3_50",           # Higher numbered Fungus3 scene
    "Mines_36",             # Higher numbered Mines scene
    "Mines_37",             # Higher numbered Mines scene
    "Ruins1_31",            # Higher numbered Ruins1 scene
    "Ruins2_10"             # Higher numbered Ruins2 scene
)

Write-Host "Potential naming variations:"
foreach ($scene in $potentialVariations) {
    if ($scene -notin $currentGrubScenes) {
        Write-Host "  $scene"
    }
}

Write-Host "`nBased on the analysis, the most likely missing grubs are:"
Write-Host "1. Cliffs_01 or Cliffs_02 (Cliffs area)"
Write-Host "2. One of the higher-numbered scenes in existing areas (Fungus1_35, Fungus1_36, Fungus3_49, Fungus3_50, Mines_36, Mines_37, Ruins1_31, Ruins2_10)"
Write-Host "`nNote: The exact count of 30 grubs vs 44 scenes in your list suggests there might be some confusion about the total count." 