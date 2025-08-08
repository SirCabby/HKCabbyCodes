namespace CabbyCodes.Scenes
{
    /// <summary>
    /// Static references to all Hollow Knight scenes for easy access throughout the mod.
    /// 
    /// To mark a scene as non-mappable (won't appear in map room toggle panels), 
    /// set the mappable parameter to false in the SceneMapData constructor.
    /// Examples of non-mappable scenes:
    /// - Tutorial areas (Tutorial_01)
    /// - Special rooms (Room_temple, Room_Mask_maker)
    /// - Boss rooms
    /// - Dream areas
    /// - Transition rooms
    /// </summary>
    public static class SceneInstances
    {
        // Abyss
        public static readonly SceneMapData Abyss_03 = new SceneMapData("Abyss_03", "Abyss");
        public static readonly SceneMapData Abyss_04 = new SceneMapData("Abyss_04", "Abyss");
        public static readonly SceneMapData Abyss_05 = new SceneMapData("Abyss_05", "Abyss");
        public static readonly SceneMapData Abyss_06_Core = new SceneMapData("Abyss_06_Core", "Abyss");
        public static readonly SceneMapData Abyss_06_Core_b = new SceneMapData("Abyss_06_Core_b", "Abyss");
        public static readonly SceneMapData Abyss_08 = new SceneMapData("Abyss_08", "Abyss");
        public static readonly SceneMapData Abyss_09 = new SceneMapData("Abyss_09", "Abyss");
        public static readonly SceneMapData Abyss_10 = new SceneMapData("Abyss_10", "Abyss");
        public static readonly SceneMapData Abyss_12 = new SceneMapData("Abyss_12", "Abyss");
        public static readonly SceneMapData Abyss_16 = new SceneMapData("Abyss_16", "Abyss");
        public static readonly SceneMapData Abyss_17 = new SceneMapData("Abyss_17", "Abyss");
        public static readonly SceneMapData Abyss_18 = new SceneMapData("Abyss_18", "Abyss");
        public static readonly SceneMapData Abyss_18_b = new SceneMapData("Abyss_18_b", "Abyss");
        public static readonly SceneMapData Abyss_19 = new SceneMapData("Abyss_19", "Abyss");
        public static readonly SceneMapData Abyss_20 = new SceneMapData("Abyss_20", "Abyss");
        public static readonly SceneMapData Abyss_21 = new SceneMapData("Abyss_21", "Abyss");
        public static readonly SceneMapData Abyss_22 = new SceneMapData("Abyss_22", "Abyss");

        // City
        public static readonly SceneMapData Crossroads_49b = new SceneMapData("Crossroads_49b", "City");
        public static readonly SceneMapData Ruins1_01 = new SceneMapData("Ruins1_01", "City");
        public static readonly SceneMapData Ruins1_02 = new SceneMapData("Ruins1_02", "City");
        public static readonly SceneMapData Ruins1_03 = new SceneMapData("Ruins1_03", "City");
        public static readonly SceneMapData Ruins1_04 = new SceneMapData("Ruins1_04", "City");
        public static readonly SceneMapData Ruins1_05 = new SceneMapData("Ruins1_05", "City");
        public static readonly SceneMapData Ruins1_05b = new SceneMapData("Ruins1_05b", "City");
        public static readonly SceneMapData Ruins1_05c = new SceneMapData("Ruins1_05c", "City");
        public static readonly SceneMapData Ruins1_06 = new SceneMapData("Ruins1_06", "City");
        public static readonly SceneMapData Ruins1_09 = new SceneMapData("Ruins1_09", "City");
        public static readonly SceneMapData Ruins1_17 = new SceneMapData("Ruins1_17", "City");
        public static readonly SceneMapData Ruins1_18 = new SceneMapData("Ruins1_18", "City");
        public static readonly SceneMapData Ruins1_18_b = new SceneMapData("Ruins1_18_b", "City");
        public static readonly SceneMapData Ruins1_23 = new SceneMapData("Ruins1_23", "City");
        public static readonly SceneMapData Ruins1_24 = new SceneMapData("Ruins1_24", "City");
        public static readonly SceneMapData Ruins1_25 = new SceneMapData("Ruins1_25", "City");
        public static readonly SceneMapData Ruins1_27 = new SceneMapData("Ruins1_27", "City");
        public static readonly SceneMapData Ruins1_28 = new SceneMapData("Ruins1_28", "City");
        public static readonly SceneMapData Ruins1_29 = new SceneMapData("Ruins1_29", "City");
        public static readonly SceneMapData Ruins1_30 = new SceneMapData("Ruins1_30", "City");
        public static readonly SceneMapData Ruins1_31 = new SceneMapData("Ruins1_31", "City");
        public static readonly SceneMapData Ruins1_31b = new SceneMapData("Ruins1_31b", "City");
        public static readonly SceneMapData Ruins1_31_top = new SceneMapData("Ruins1_31_top", "City");
        public static readonly SceneMapData Ruins1_31_top_2 = new SceneMapData("Ruins1_31_top_2", "City");
        public static readonly SceneMapData Ruins1_32 = new SceneMapData("Ruins1_32", "City");
        public static readonly SceneMapData Ruins2_01 = new SceneMapData("Ruins2_01", "City");
        public static readonly SceneMapData Ruins2_01_b = new SceneMapData("Ruins2_01_b", "City");
        public static readonly SceneMapData Ruins2_03 = new SceneMapData("Ruins2_03", "City");
        public static readonly SceneMapData Ruins2_03b = new SceneMapData("Ruins2_03b", "City");
        public static readonly SceneMapData Ruins2_04 = new SceneMapData("Ruins2_04", "City");
        public static readonly SceneMapData Ruins2_05 = new SceneMapData("Ruins2_05", "City");
        public static readonly SceneMapData Ruins2_06 = new SceneMapData("Ruins2_06", "City");
        public static readonly SceneMapData Ruins2_07 = new SceneMapData("Ruins2_07", "City");
        public static readonly SceneMapData Ruins2_07_left = new SceneMapData("Ruins2_07_left", "City");
        public static readonly SceneMapData Ruins2_07_right = new SceneMapData("Ruins2_07_right", "City");
        public static readonly SceneMapData Ruins2_08 = new SceneMapData("Ruins2_08", "City");
        public static readonly SceneMapData Ruins2_09 = new SceneMapData("Ruins2_09", "City");
        public static readonly SceneMapData Ruins2_10_b = new SceneMapData("Ruins2_10_b", "City");
        public static readonly SceneMapData Ruins2_11 = new SceneMapData("Ruins2_11", "City");
        public static readonly SceneMapData Ruins2_11_b = new SceneMapData("Ruins2_11_b", "City");
        public static readonly SceneMapData Ruins2_Watcher_Room = new SceneMapData("Ruins2_Watcher_Room", "City");
        public static readonly SceneMapData Ruins_Bathhouse = new SceneMapData("Ruins_Bathhouse", "City");
        public static readonly SceneMapData Ruins_Elevator = new SceneMapData("Ruins_Elevator", "City");
        public static readonly SceneMapData Ruins_House_01 = new SceneMapData("Ruins_House_01", "City");

        // Cliffs
        public static readonly SceneMapData Cliffs_01 = new SceneMapData("Cliffs_01", "Cliffs");
        public static readonly SceneMapData Cliffs_01_b = new SceneMapData("Cliffs_01_b", "Cliffs");
        public static readonly SceneMapData Cliffs_02 = new SceneMapData("Cliffs_02", "Cliffs");
        public static readonly SceneMapData Cliffs_02_b = new SceneMapData("Cliffs_02_b", "Cliffs");
        public static readonly SceneMapData Cliffs_04 = new SceneMapData("Cliffs_04", "Cliffs");
        public static readonly SceneMapData Cliffs_05 = new SceneMapData("Cliffs_05", "Cliffs");
        public static readonly SceneMapData Cliffs_06 = new SceneMapData("Cliffs_06", "Cliffs");
        public static readonly SceneMapData Cliffs_06_b = new SceneMapData("Cliffs_06_b", "Cliffs");
        public static readonly SceneMapData Fungus1_28 = new SceneMapData("Fungus1_28", "Cliffs");
        public static readonly SceneMapData Fungus1_28_b = new SceneMapData("Fungus1_28_b", "Cliffs");

        // Crossroads
        public static readonly SceneMapData Crossroads_01 = new SceneMapData("Crossroads_01", "Crossroads", "Crossroads Entrance");
        public static readonly SceneMapData Crossroads_02 = new SceneMapData("Crossroads_02", "Crossroads", "Outside Temple of the Black Egg");
        public static readonly SceneMapData Crossroads_03 = new SceneMapData("Crossroads_03", "Crossroads");
        public static readonly SceneMapData Crossroads_04 = new SceneMapData("Crossroads_04", "Crossroads");
        public static readonly SceneMapData Crossroads_05 = new SceneMapData("Crossroads_05", "Crossroads");
        public static readonly SceneMapData Crossroads_06 = new SceneMapData("Crossroads_06", "Crossroads", "Outside Ancestral Mound");
        public static readonly SceneMapData Crossroads_07 = new SceneMapData("Crossroads_07", "Crossroads");
        public static readonly SceneMapData Crossroads_08 = new SceneMapData("Crossroads_08", "Crossroads");
        public static readonly SceneMapData Crossroads_09 = new SceneMapData("Crossroads_09", "Crossroads", "Brooding Mawlek room");
        public static readonly SceneMapData Crossroads_10 = new SceneMapData("Crossroads_10", "Crossroads", "False Knight room");
        public static readonly SceneMapData Crossroads_11_alt = new SceneMapData("Crossroads_11_alt", "Crossroads", "The Pilgrim's Way entrance");
        public static readonly SceneMapData Crossroads_12 = new SceneMapData("Crossroads_12", "Crossroads");
        public static readonly SceneMapData Crossroads_13 = new SceneMapData("Crossroads_13", "Crossroads");
        public static readonly SceneMapData Crossroads_14 = new SceneMapData("Crossroads_14", "Crossroads");
        public static readonly SceneMapData Crossroads_15 = new SceneMapData("Crossroads_15", "Crossroads");
        public static readonly SceneMapData Crossroads_16 = new SceneMapData("Crossroads_16", "Crossroads");
        public static readonly SceneMapData Crossroads_18 = new SceneMapData("Crossroads_18", "Crossroads");
        public static readonly SceneMapData Crossroads_19 = new SceneMapData("Crossroads_19", "Crossroads");
        public static readonly SceneMapData Crossroads_21 = new SceneMapData("Crossroads_21", "Crossroads");
        public static readonly SceneMapData Crossroads_22 = new SceneMapData("Crossroads_22", "Crossroads");
        public static readonly SceneMapData Crossroads_25 = new SceneMapData("Crossroads_25", "Crossroads", "Path to Brooding Mawlek 1");
        public static readonly SceneMapData Crossroads_27 = new SceneMapData("Crossroads_27", "Crossroads");
        public static readonly SceneMapData Crossroads_30 = new SceneMapData("Crossroads_30", "Crossroads");
        public static readonly SceneMapData Crossroads_31 = new SceneMapData("Crossroads_31", "Crossroads");
        public static readonly SceneMapData Crossroads_33 = new SceneMapData("Crossroads_33", "Crossroads");
        public static readonly SceneMapData Crossroads_35 = new SceneMapData("Crossroads_35", "Crossroads");
        public static readonly SceneMapData Crossroads_36 = new SceneMapData("Crossroads_36", "Crossroads", "Path to Brooding Mawlek 2");
        public static readonly SceneMapData Crossroads_37 = new SceneMapData("Crossroads_37", "Crossroads");
        public static readonly SceneMapData Crossroads_38 = new SceneMapData("Crossroads_38", "Crossroads", "The Grubfather's room");
        public static readonly SceneMapData Crossroads_39 = new SceneMapData("Crossroads_39", "Crossroads");
        public static readonly SceneMapData Crossroads_40 = new SceneMapData("Crossroads_40", "Crossroads");
        public static readonly SceneMapData Crossroads_42 = new SceneMapData("Crossroads_42", "Crossroads");
        public static readonly SceneMapData Crossroads_43 = new SceneMapData("Crossroads_43", "Crossroads");
        public static readonly SceneMapData Crossroads_45 = new SceneMapData("Crossroads_45", "Crossroads");
        public static readonly SceneMapData Crossroads_46 = new SceneMapData("Crossroads_46", "Crossroads");
        public static readonly SceneMapData Crossroads_47 = new SceneMapData("Crossroads_47", "Crossroads");
        public static readonly SceneMapData Crossroads_48 = new SceneMapData("Crossroads_48", "Crossroads");
        public static readonly SceneMapData Crossroads_49 = new SceneMapData("Crossroads_49", "Crossroads");
        public static readonly SceneMapData Crossroads_52 = new SceneMapData("Crossroads_52", "Crossroads");
        public static readonly SceneMapData Room_temple = new SceneMapData("Room_temple", "Crossroads", "Temple of the Black Egg", false);
        public static readonly SceneMapData Crossroads_ShamanTemple = new SceneMapData("Crossroads_ShamanTemple", "Crossroads", "Ancestral Mound", false);

        // Deepnest
        public static readonly SceneMapData Abyss_03_b = new SceneMapData("Abyss_03_b", "Deepnest");
        public static readonly SceneMapData Deepnest_01b = new SceneMapData("Deepnest_01b", "Deepnest");
        public static readonly SceneMapData Deepnest_02 = new SceneMapData("Deepnest_02", "Deepnest");
        public static readonly SceneMapData Deepnest_03 = new SceneMapData("Deepnest_03", "Deepnest");
        public static readonly SceneMapData Deepnest_09 = new SceneMapData("Deepnest_09", "Deepnest");
        public static readonly SceneMapData Deepnest_10 = new SceneMapData("Deepnest_10", "Deepnest");
        public static readonly SceneMapData Deepnest_14 = new SceneMapData("Deepnest_14", "Deepnest");
        public static readonly SceneMapData Deepnest_16 = new SceneMapData("Deepnest_16", "Deepnest");
        public static readonly SceneMapData Deepnest_17 = new SceneMapData("Deepnest_17", "Deepnest");
        public static readonly SceneMapData Deepnest_26 = new SceneMapData("Deepnest_26", "Deepnest");
        public static readonly SceneMapData Deepnest_26b = new SceneMapData("Deepnest_26b", "Deepnest");
        public static readonly SceneMapData Deepnest_30 = new SceneMapData("Deepnest_30", "Deepnest");
        public static readonly SceneMapData Deepnest_30_b = new SceneMapData("Deepnest_30_b", "Deepnest");
        public static readonly SceneMapData Deepnest_31 = new SceneMapData("Deepnest_31", "Deepnest");
        public static readonly SceneMapData Deepnest_32 = new SceneMapData("Deepnest_32", "Deepnest");
        public static readonly SceneMapData Deepnest_33 = new SceneMapData("Deepnest_33", "Deepnest");
        public static readonly SceneMapData Deepnest_34 = new SceneMapData("Deepnest_34", "Deepnest");
        public static readonly SceneMapData Deepnest_35 = new SceneMapData("Deepnest_35", "Deepnest");
        public static readonly SceneMapData Deepnest_36 = new SceneMapData("Deepnest_36", "Deepnest");
        public static readonly SceneMapData Deepnest_37 = new SceneMapData("Deepnest_37", "Deepnest");
        public static readonly SceneMapData Deepnest_38 = new SceneMapData("Deepnest_38", "Deepnest");
        public static readonly SceneMapData Deepnest_39 = new SceneMapData("Deepnest_39", "Deepnest");
        public static readonly SceneMapData Deepnest_40 = new SceneMapData("Deepnest_40", "Deepnest");
        public static readonly SceneMapData Deepnest_41 = new SceneMapData("Deepnest_41", "Deepnest");
        public static readonly SceneMapData Deepnest_41_b = new SceneMapData("Deepnest_41_b", "Deepnest");
        public static readonly SceneMapData Deepnest_42 = new SceneMapData("Deepnest_42", "Deepnest");
        public static readonly SceneMapData Deepnest_44 = new SceneMapData("Deepnest_44", "Deepnest");
        public static readonly SceneMapData Deepnest_44_b = new SceneMapData("Deepnest_44_b", "Deepnest");
        public static readonly SceneMapData Fungus2_25 = new SceneMapData("Fungus2_25", "Deepnest");
        public static readonly SceneMapData Room_Mask_maker = new SceneMapData("Room_Mask_maker", "Deepnest");
        public static readonly SceneMapData Deepnest_Spider_Town = new SceneMapData("Deepnest_Spider_Town", "Deepnest");

        // Dirtmouth
        public static readonly SceneMapData Town = new SceneMapData(        "Town",         "Dirtmouth",    "Dirtmouth");
        public static readonly SceneMapData Tutorial_01 = new SceneMapData( "Tutorial_01",  "Dirtmouth",    "King's Pass");

        // FogCanyon
        public static readonly SceneMapData Fungus3_01 = new SceneMapData("Fungus3_01", "FogCanyon");
        public static readonly SceneMapData Fungus3_02 = new SceneMapData("Fungus3_02", "FogCanyon");
        public static readonly SceneMapData Fungus3_03 = new SceneMapData("Fungus3_03", "FogCanyon");
        public static readonly SceneMapData Fungus3_24 = new SceneMapData("Fungus3_24", "FogCanyon");
        public static readonly SceneMapData Fungus3_25 = new SceneMapData("Fungus3_25", "FogCanyon");
        public static readonly SceneMapData Fungus3_25b = new SceneMapData("Fungus3_25b", "FogCanyon");
        public static readonly SceneMapData Fungus3_26 = new SceneMapData("Fungus3_26", "FogCanyon");
        public static readonly SceneMapData Fungus3_27 = new SceneMapData("Fungus3_27", "FogCanyon");
        public static readonly SceneMapData Fungus3_28 = new SceneMapData("Fungus3_28", "FogCanyon");
        public static readonly SceneMapData Fungus3_30 = new SceneMapData("Fungus3_30", "FogCanyon");
        public static readonly SceneMapData Fungus3_35 = new SceneMapData("Fungus3_35", "FogCanyon");
        public static readonly SceneMapData Fungus3_44 = new SceneMapData("Fungus3_44", "FogCanyon");
        public static readonly SceneMapData Fungus3_47 = new SceneMapData("Fungus3_47", "FogCanyon");

        // FungalWastes
        public static readonly SceneMapData Deepnest_01 = new SceneMapData("Deepnest_01", "FungalWastes");
        public static readonly SceneMapData Fungus2_01 = new SceneMapData("Fungus2_01", "FungalWastes");
        public static readonly SceneMapData Fungus2_02 = new SceneMapData("Fungus2_02", "FungalWastes");
        public static readonly SceneMapData Fungus2_03 = new SceneMapData("Fungus2_03", "FungalWastes");
        public static readonly SceneMapData Fungus2_04 = new SceneMapData("Fungus2_04", "FungalWastes");
        public static readonly SceneMapData Fungus2_05 = new SceneMapData("Fungus2_05", "FungalWastes");
        public static readonly SceneMapData Fungus2_06 = new SceneMapData("Fungus2_06", "FungalWastes");
        public static readonly SceneMapData Fungus2_07 = new SceneMapData("Fungus2_07", "FungalWastes");
        public static readonly SceneMapData Fungus2_08 = new SceneMapData("Fungus2_08", "FungalWastes");
        public static readonly SceneMapData Fungus2_09 = new SceneMapData("Fungus2_09", "FungalWastes");
        public static readonly SceneMapData Fungus2_10 = new SceneMapData("Fungus2_10", "FungalWastes");
        public static readonly SceneMapData Fungus2_11 = new SceneMapData("Fungus2_11", "FungalWastes");
        public static readonly SceneMapData Fungus2_12 = new SceneMapData("Fungus2_12", "FungalWastes");
        public static readonly SceneMapData Fungus2_13 = new SceneMapData("Fungus2_13", "FungalWastes");
        public static readonly SceneMapData Fungus2_14 = new SceneMapData("Fungus2_14", "FungalWastes");
        public static readonly SceneMapData Fungus2_14_b = new SceneMapData("Fungus2_14_b", "FungalWastes");
        public static readonly SceneMapData Fungus2_14_c = new SceneMapData("Fungus2_14_c", "FungalWastes");
        public static readonly SceneMapData Fungus2_15 = new SceneMapData("Fungus2_15", "FungalWastes");
        public static readonly SceneMapData Fungus2_17 = new SceneMapData("Fungus2_17", "FungalWastes");
        public static readonly SceneMapData Fungus2_18 = new SceneMapData("Fungus2_18", "FungalWastes");
        public static readonly SceneMapData Fungus2_19 = new SceneMapData("Fungus2_19", "FungalWastes");
        public static readonly SceneMapData Fungus2_20 = new SceneMapData("Fungus2_20", "FungalWastes");
        public static readonly SceneMapData Fungus2_21 = new SceneMapData("Fungus2_21", "FungalWastes");
        public static readonly SceneMapData Fungus2_23 = new SceneMapData("Fungus2_23", "FungalWastes");
        public static readonly SceneMapData Fungus2_26 = new SceneMapData("Fungus2_26", "FungalWastes");
        public static readonly SceneMapData Fungus2_28 = new SceneMapData("Fungus2_28", "FungalWastes");
        public static readonly SceneMapData Fungus2_29 = new SceneMapData("Fungus2_29", "FungalWastes");
        public static readonly SceneMapData Fungus2_30 = new SceneMapData("Fungus2_30", "FungalWastes");
        public static readonly SceneMapData Fungus2_31 = new SceneMapData("Fungus2_31", "FungalWastes");
        public static readonly SceneMapData Fungus2_32 = new SceneMapData("Fungus2_32", "FungalWastes");
        public static readonly SceneMapData Fungus2_29_b = new SceneMapData("Fungus2_29_b", "FungalWastes");
        public static readonly SceneMapData Fungus2_33 = new SceneMapData("Fungus2_33", "FungalWastes");
        public static readonly SceneMapData Fungus2_34 = new SceneMapData("Fungus2_34", "FungalWastes");

        // Greenpath
        public static readonly SceneMapData Fungus1_01 = new SceneMapData("Fungus1_01", "Greenpath");
        public static readonly SceneMapData Fungus1_01b = new SceneMapData("Fungus1_01b", "Greenpath");
        public static readonly SceneMapData Fungus1_02 = new SceneMapData("Fungus1_02", "Greenpath");
        public static readonly SceneMapData Fungus1_03 = new SceneMapData("Fungus1_03", "Greenpath");
        public static readonly SceneMapData Fungus1_04 = new SceneMapData("Fungus1_04", "Greenpath");
        public static readonly SceneMapData Fungus1_05 = new SceneMapData("Fungus1_05", "Greenpath");
        public static readonly SceneMapData Fungus1_06 = new SceneMapData("Fungus1_06", "Greenpath");
        public static readonly SceneMapData Fungus1_07 = new SceneMapData("Fungus1_07", "Greenpath");
        public static readonly SceneMapData Fungus1_08 = new SceneMapData("Fungus1_08", "Greenpath");
        public static readonly SceneMapData Fungus1_09 = new SceneMapData("Fungus1_09", "Greenpath");
        public static readonly SceneMapData Fungus1_09_b = new SceneMapData("Fungus1_09_b", "Greenpath");
        public static readonly SceneMapData Fungus1_10 = new SceneMapData("Fungus1_10", "Greenpath");
        public static readonly SceneMapData Fungus1_11 = new SceneMapData("Fungus1_11", "Greenpath");
        public static readonly SceneMapData Fungus1_12 = new SceneMapData("Fungus1_12", "Greenpath");
        public static readonly SceneMapData Fungus1_13 = new SceneMapData("Fungus1_13", "Greenpath");
        public static readonly SceneMapData Fungus1_14 = new SceneMapData("Fungus1_14", "Greenpath");
        public static readonly SceneMapData Fungus1_14_b = new SceneMapData("Fungus1_14_b", "Greenpath");
        public static readonly SceneMapData Fungus1_15 = new SceneMapData("Fungus1_15", "Greenpath");
        public static readonly SceneMapData Fungus1_16_alt = new SceneMapData("Fungus1_16_alt", "Greenpath");
        public static readonly SceneMapData Fungus1_17 = new SceneMapData("Fungus1_17", "Greenpath");
        public static readonly SceneMapData Fungus1_19 = new SceneMapData("Fungus1_19", "Greenpath");
        public static readonly SceneMapData Fungus1_20_v02 = new SceneMapData("Fungus1_20_v02", "Greenpath");
        public static readonly SceneMapData Fungus1_21 = new SceneMapData("Fungus1_21", "Greenpath");
        public static readonly SceneMapData Fungus1_22 = new SceneMapData("Fungus1_22", "Greenpath");
        public static readonly SceneMapData Fungus1_25 = new SceneMapData("Fungus1_25", "Greenpath");
        public static readonly SceneMapData Fungus1_26 = new SceneMapData("Fungus1_26", "Greenpath");
        public static readonly SceneMapData Fungus1_29 = new SceneMapData("Fungus1_29", "Greenpath");
        public static readonly SceneMapData Fungus1_30 = new SceneMapData("Fungus1_30", "Greenpath");
        public static readonly SceneMapData Fungus1_31 = new SceneMapData("Fungus1_31", "Greenpath");
        public static readonly SceneMapData Fungus1_32 = new SceneMapData("Fungus1_32", "Greenpath");
        public static readonly SceneMapData Fungus1_34 = new SceneMapData("Fungus1_34", "Greenpath");
        public static readonly SceneMapData Fungus1_36 = new SceneMapData("Fungus1_36", "Greenpath");
        public static readonly SceneMapData Fungus1_37 = new SceneMapData("Fungus1_37", "Greenpath");
        public static readonly SceneMapData Fungus1_Slug = new SceneMapData("Fungus1_Slug", "Greenpath");

        // Mines
        public static readonly SceneMapData Mines_01 = new SceneMapData("Mines_01", "Mines");
        public static readonly SceneMapData Mines_02 = new SceneMapData("Mines_02", "Mines");
        public static readonly SceneMapData Mines_03 = new SceneMapData("Mines_03", "Mines");
        public static readonly SceneMapData Mines_04 = new SceneMapData("Mines_04", "Mines");
        public static readonly SceneMapData Mines_05 = new SceneMapData("Mines_05", "Mines");
        public static readonly SceneMapData Mines_06 = new SceneMapData("Mines_06", "Mines");
        public static readonly SceneMapData Mines_07 = new SceneMapData("Mines_07", "Mines");
        public static readonly SceneMapData Mines_10 = new SceneMapData("Mines_10", "Mines");
        public static readonly SceneMapData Mines_11 = new SceneMapData("Mines_11", "Mines");
        public static readonly SceneMapData Mines_13 = new SceneMapData("Mines_13", "Mines");
        public static readonly SceneMapData Mines_16 = new SceneMapData("Mines_16", "Mines");
        public static readonly SceneMapData Mines_17 = new SceneMapData("Mines_17", "Mines");
        public static readonly SceneMapData Mines_18 = new SceneMapData("Mines_18", "Mines");
        public static readonly SceneMapData Mines_19 = new SceneMapData("Mines_19", "Mines");
        public static readonly SceneMapData Mines_20 = new SceneMapData("Mines_20", "Mines");
        public static readonly SceneMapData Mines_20_b = new SceneMapData("Mines_20_b", "Mines");
        public static readonly SceneMapData Mines_23 = new SceneMapData("Mines_23", "Mines");
        public static readonly SceneMapData Mines_24 = new SceneMapData("Mines_24", "Mines");
        public static readonly SceneMapData Mines_25 = new SceneMapData("Mines_25", "Mines");
        public static readonly SceneMapData Mines_28 = new SceneMapData("Mines_28", "Mines");
        public static readonly SceneMapData Mines_28_b = new SceneMapData("Mines_28_b", "Mines");
        public static readonly SceneMapData Mines_29 = new SceneMapData("Mines_29", "Mines");
        public static readonly SceneMapData Mines_30 = new SceneMapData("Mines_30", "Mines");
        public static readonly SceneMapData Mines_31 = new SceneMapData("Mines_31", "Mines");
        public static readonly SceneMapData Mines_32 = new SceneMapData("Mines_32", "Mines");
        public static readonly SceneMapData Mines_33 = new SceneMapData("Mines_33", "Mines");
        public static readonly SceneMapData Mines_34 = new SceneMapData("Mines_34", "Mines");
        public static readonly SceneMapData Mines_35 = new SceneMapData("Mines_35", "Mines");
        public static readonly SceneMapData Mines_36 = new SceneMapData("Mines_36", "Mines");
        public static readonly SceneMapData Mines_37 = new SceneMapData("Mines_37", "Mines");

        // Outskirts
        public static readonly SceneMapData Abyss_03_c = new SceneMapData("Abyss_03_c", "Outskirts");
        public static readonly SceneMapData Deepnest_East_01 = new SceneMapData("Deepnest_East_01", "Outskirts");
        public static readonly SceneMapData Deepnest_East_02 = new SceneMapData("Deepnest_East_02", "Outskirts");
        public static readonly SceneMapData Deepnest_East_02b = new SceneMapData("Deepnest_East_02b", "Outskirts");
        public static readonly SceneMapData Deepnest_East_03 = new SceneMapData("Deepnest_East_03", "Outskirts");
        public static readonly SceneMapData Deepnest_East_04 = new SceneMapData("Deepnest_East_04", "Outskirts");
        public static readonly SceneMapData Deepnest_East_06 = new SceneMapData("Deepnest_East_06", "Outskirts");
        public static readonly SceneMapData Deepnest_East_07 = new SceneMapData("Deepnest_East_07", "Outskirts");
        public static readonly SceneMapData Deepnest_East_08 = new SceneMapData("Deepnest_East_08", "Outskirts");
        public static readonly SceneMapData Deepnest_East_09 = new SceneMapData("Deepnest_East_09", "Outskirts");
        public static readonly SceneMapData Deepnest_East_09_b = new SceneMapData("Deepnest_East_09_b", "Outskirts");
        public static readonly SceneMapData Deepnest_East_10 = new SceneMapData("Deepnest_East_10", "Outskirts");
        public static readonly SceneMapData Deepnest_East_11 = new SceneMapData("Deepnest_East_11", "Outskirts");
        public static readonly SceneMapData Deepnest_East_12 = new SceneMapData("Deepnest_East_12", "Outskirts");
        public static readonly SceneMapData Deepnest_East_13 = new SceneMapData("Deepnest_East_13", "Outskirts");
        public static readonly SceneMapData Deepnest_East_14 = new SceneMapData("Deepnest_East_14", "Outskirts");
        public static readonly SceneMapData Deepnest_East_15 = new SceneMapData("Deepnest_East_15", "Outskirts");
        public static readonly SceneMapData Deepnest_East_16 = new SceneMapData("Deepnest_East_16", "Outskirts");
        public static readonly SceneMapData Deepnest_East_18 = new SceneMapData("Deepnest_East_18", "Outskirts");
        public static readonly SceneMapData Deepnest_East_Hornet = new SceneMapData("Deepnest_East_Hornet", "Outskirts");
        public static readonly SceneMapData Deepnest_East_Hornet_b = new SceneMapData("Deepnest_East_Hornet_b", "Outskirts");
        public static readonly SceneMapData Hive_01 = new SceneMapData("Hive_01", "Outskirts");
        public static readonly SceneMapData Hive_02 = new SceneMapData("Hive_02", "Outskirts");
        public static readonly SceneMapData Hive_03 = new SceneMapData("Hive_03", "Outskirts");
        public static readonly SceneMapData Hive_03_b = new SceneMapData("Hive_03_b", "Outskirts");
        public static readonly SceneMapData Hive_03_c = new SceneMapData("Hive_03_c", "Outskirts");
        public static readonly SceneMapData Hive_04 = new SceneMapData("Hive_04", "Outskirts");
        public static readonly SceneMapData Hive_04_b = new SceneMapData("Hive_04_b", "Outskirts");
        public static readonly SceneMapData Hive_05 = new SceneMapData("Hive_05", "Outskirts");

        // RestingGrounds
        public static readonly SceneMapData Crossroads_46b = new SceneMapData("Crossroads_46b", "RestingGrounds");
        public static readonly SceneMapData Crossroads_50 = new SceneMapData("Crossroads_50", "RestingGrounds");
        public static readonly SceneMapData RestingGrounds_02 = new SceneMapData("RestingGrounds_02", "RestingGrounds");
        public static readonly SceneMapData RestingGrounds_04 = new SceneMapData("RestingGrounds_04", "RestingGrounds");
        public static readonly SceneMapData RestingGrounds_05 = new SceneMapData("RestingGrounds_05", "RestingGrounds");
        public static readonly SceneMapData RestingGrounds_06 = new SceneMapData("RestingGrounds_06", "RestingGrounds");
        public static readonly SceneMapData RestingGrounds_08 = new SceneMapData("RestingGrounds_08", "RestingGrounds");
        public static readonly SceneMapData RestingGrounds_09 = new SceneMapData("RestingGrounds_09", "RestingGrounds");
        public static readonly SceneMapData RestingGrounds_10_b = new SceneMapData("RestingGrounds_10_b", "RestingGrounds");
        public static readonly SceneMapData RestingGrounds_10_c = new SceneMapData("RestingGrounds_10_c", "RestingGrounds");
        public static readonly SceneMapData RestingGrounds_10_d = new SceneMapData("RestingGrounds_10_d", "RestingGrounds");
        public static readonly SceneMapData RestingGrounds_12 = new SceneMapData("RestingGrounds_12", "RestingGrounds");
        public static readonly SceneMapData RestingGrounds_17 = new SceneMapData("RestingGrounds_17", "RestingGrounds");
        public static readonly SceneMapData Ruins2_10 = new SceneMapData("Ruins2_10", "RestingGrounds");
        public static readonly SceneMapData RestingGrounds_10 = new SceneMapData("RestingGrounds_10", "RestingGrounds");

        // RoyalGardens
        public static readonly SceneMapData Deepnest_43 = new SceneMapData("Deepnest_43", "RoyalGardens");
        public static readonly SceneMapData Deepnest_43_b = new SceneMapData("Deepnest_43_b", "RoyalGardens");
        public static readonly SceneMapData Fungus1_23 = new SceneMapData("Fungus1_23", "RoyalGardens");
        public static readonly SceneMapData Fungus1_24 = new SceneMapData("Fungus1_24", "RoyalGardens");
        public static readonly SceneMapData Fungus3_04 = new SceneMapData("Fungus3_04", "RoyalGardens");
        public static readonly SceneMapData Fungus3_05 = new SceneMapData("Fungus3_05", "RoyalGardens");
        public static readonly SceneMapData Fungus3_08 = new SceneMapData("Fungus3_08", "RoyalGardens");
        public static readonly SceneMapData Fungus3_10 = new SceneMapData("Fungus3_10", "RoyalGardens");
        public static readonly SceneMapData Fungus3_11 = new SceneMapData("Fungus3_11", "RoyalGardens");
        public static readonly SceneMapData Fungus3_13 = new SceneMapData("Fungus3_13", "RoyalGardens");
        public static readonly SceneMapData Fungus3_21 = new SceneMapData("Fungus3_21", "RoyalGardens");
        public static readonly SceneMapData Fungus3_22 = new SceneMapData("Fungus3_22", "RoyalGardens");
        public static readonly SceneMapData Fungus3_22_b = new SceneMapData("Fungus3_22_b", "RoyalGardens");
        public static readonly SceneMapData Fungus3_23 = new SceneMapData("Fungus3_23", "RoyalGardens");
        public static readonly SceneMapData Fungus3_23_b = new SceneMapData("Fungus3_23_b", "RoyalGardens");
        public static readonly SceneMapData Fungus3_34 = new SceneMapData("Fungus3_34", "RoyalGardens");
        public static readonly SceneMapData Fungus3_39 = new SceneMapData("Fungus3_39", "RoyalGardens");
        public static readonly SceneMapData Fungus3_40 = new SceneMapData("Fungus3_40", "RoyalGardens");
        public static readonly SceneMapData Fungus3_48 = new SceneMapData("Fungus3_48", "RoyalGardens");
        public static readonly SceneMapData Fungus3_48_bot = new SceneMapData("Fungus3_48_bot", "RoyalGardens");
        public static readonly SceneMapData Fungus3_48_left = new SceneMapData("Fungus3_48_left", "RoyalGardens");
        public static readonly SceneMapData Fungus3_48_top = new SceneMapData("Fungus3_48_top", "RoyalGardens");
        public static readonly SceneMapData Fungus3_49 = new SceneMapData("Fungus3_49", "RoyalGardens");
        public static readonly SceneMapData Fungus3_50 = new SceneMapData("Fungus3_50", "RoyalGardens");

        // Waterways
        public static readonly SceneMapData Abyss_01 = new SceneMapData("Abyss_01", "Waterways");
        public static readonly SceneMapData Abyss_02 = new SceneMapData("Abyss_02", "Waterways");
        public static readonly SceneMapData Waterways_01 = new SceneMapData("Waterways_01", "Waterways");
        public static readonly SceneMapData Waterways_02 = new SceneMapData("Waterways_02", "Waterways");
        public static readonly SceneMapData Waterways_02b = new SceneMapData("Waterways_02b", "Waterways");
        public static readonly SceneMapData Waterways_03 = new SceneMapData("Waterways_03", "Waterways");
        public static readonly SceneMapData Waterways_04 = new SceneMapData("Waterways_04", "Waterways");
        public static readonly SceneMapData Waterways_04_part_b = new SceneMapData("Waterways_04_part_b", "Waterways");
        public static readonly SceneMapData Waterways_04b = new SceneMapData("Waterways_04b", "Waterways");
        public static readonly SceneMapData Waterways_05 = new SceneMapData("Waterways_05", "Waterways");
        public static readonly SceneMapData Waterways_06 = new SceneMapData("Waterways_06", "Waterways");
        public static readonly SceneMapData Waterways_07 = new SceneMapData("Waterways_07", "Waterways");
        public static readonly SceneMapData Waterways_08 = new SceneMapData("Waterways_08", "Waterways");
        public static readonly SceneMapData Waterways_09 = new SceneMapData("Waterways_09", "Waterways");
        public static readonly SceneMapData Waterways_12 = new SceneMapData("Waterways_12", "Waterways");
        public static readonly SceneMapData Waterways_13 = new SceneMapData("Waterways_13", "Waterways");
        public static readonly SceneMapData Waterways_14 = new SceneMapData("Waterways_14", "Waterways");
        public static readonly SceneMapData Waterways_15 = new SceneMapData("Waterways_15", "Waterways");
    }
} 