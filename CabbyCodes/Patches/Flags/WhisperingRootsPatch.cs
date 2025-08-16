using CabbyCodes.Flags;
using CabbyCodes.Patches.BasePatches;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.SyncedReferences;
using System.Collections.Generic;
using System.Linq;

namespace CabbyCodes.Patches.Flags
{
    public class WhisperingRootsPatch : BasePatch
    {
        /// <summary>
        /// Override to return all base flags that need monitoring
        /// </summary>
        protected override FlagDef[] GetFlags()
        {
            return GetFlagMappings().Keys.ToArray();
        }

        /// <summary>
        /// Define the base flags and their associated boolean flags here
        /// Returns a dictionary: baseFlag -> list of associated flags
        /// </summary>
        /// <returns>Dictionary of flag mappings</returns>
        protected virtual Dictionary<FlagDef, List<FlagDef>> GetFlagMappings()
        {
            return new Dictionary<FlagDef, List<FlagDef>>
            {
                { 
                    FlagInstances.Crossroads_07__Dream_Plant, new List<FlagDef>
                    {
                        FlagInstances.Crossroads_07__Dream_Plant_Orb,
                        FlagInstances.Crossroads_07__Dream_Plant_Orb_1,
                        FlagInstances.Crossroads_07__Dream_Plant_Orb_10,
                        FlagInstances.Crossroads_07__Dream_Plant_Orb_11,
                        FlagInstances.Crossroads_07__Dream_Plant_Orb_12,
                        FlagInstances.Crossroads_07__Dream_Plant_Orb_13,
                        FlagInstances.Crossroads_07__Dream_Plant_Orb_14,
                        FlagInstances.Crossroads_07__Dream_Plant_Orb_15,
                        FlagInstances.Crossroads_07__Dream_Plant_Orb_16,
                        FlagInstances.Crossroads_07__Dream_Plant_Orb_17,
                        FlagInstances.Crossroads_07__Dream_Plant_Orb_18,
                        FlagInstances.Crossroads_07__Dream_Plant_Orb_19,
                        FlagInstances.Crossroads_07__Dream_Plant_Orb_2,
                        FlagInstances.Crossroads_07__Dream_Plant_Orb_20,
                        FlagInstances.Crossroads_07__Dream_Plant_Orb_21,
                        FlagInstances.Crossroads_07__Dream_Plant_Orb_22,
                        FlagInstances.Crossroads_07__Dream_Plant_Orb_23,
                        FlagInstances.Crossroads_07__Dream_Plant_Orb_24,
                        FlagInstances.Crossroads_07__Dream_Plant_Orb_25,
                        FlagInstances.Crossroads_07__Dream_Plant_Orb_26,
                        FlagInstances.Crossroads_07__Dream_Plant_Orb_27,
                        FlagInstances.Crossroads_07__Dream_Plant_Orb_28,
                        FlagInstances.Crossroads_07__Dream_Plant_Orb_3,
                        FlagInstances.Crossroads_07__Dream_Plant_Orb_4,
                        FlagInstances.Crossroads_07__Dream_Plant_Orb_5,
                        FlagInstances.Crossroads_07__Dream_Plant_Orb_6,
                        FlagInstances.Crossroads_07__Dream_Plant_Orb_7,
                        FlagInstances.Crossroads_07__Dream_Plant_Orb_8,
                        FlagInstances.Crossroads_07__Dream_Plant_Orb_9
                    }
                },
                { 
                    FlagInstances.RestingGrounds_05__Dream_Plant, new List<FlagDef>
                    {
                        FlagInstances.RestingGrounds_05__Dream_Plant_Orb,
                        FlagInstances.RestingGrounds_05__Dream_Plant_Orb_1,
                        FlagInstances.RestingGrounds_05__Dream_Plant_Orb_10,
                        FlagInstances.RestingGrounds_05__Dream_Plant_Orb_11,
                        FlagInstances.RestingGrounds_05__Dream_Plant_Orb_12,
                        FlagInstances.RestingGrounds_05__Dream_Plant_Orb_13,
                        FlagInstances.RestingGrounds_05__Dream_Plant_Orb_14,
                        FlagInstances.RestingGrounds_05__Dream_Plant_Orb_15,
                        FlagInstances.RestingGrounds_05__Dream_Plant_Orb_16,
                        FlagInstances.RestingGrounds_05__Dream_Plant_Orb_17,
                        FlagInstances.RestingGrounds_05__Dream_Plant_Orb_18,
                        FlagInstances.RestingGrounds_05__Dream_Plant_Orb_19,
                        FlagInstances.RestingGrounds_05__Dream_Plant_Orb_2,
                        FlagInstances.RestingGrounds_05__Dream_Plant_Orb_3,
                        FlagInstances.RestingGrounds_05__Dream_Plant_Orb_4,
                        FlagInstances.RestingGrounds_05__Dream_Plant_Orb_5,
                        FlagInstances.RestingGrounds_05__Dream_Plant_Orb_6,
                        FlagInstances.RestingGrounds_05__Dream_Plant_Orb_7,
                        FlagInstances.RestingGrounds_05__Dream_Plant_Orb_8,
                        FlagInstances.RestingGrounds_05__Dream_Plant_Orb_9
                    }
                },
                { 
                    FlagInstances.Crossroads_ShamanTemple__Dream_Plant, new List<FlagDef>
                    {
                        FlagInstances.Crossroads_ShamanTemple__Dream_Plant_Orb,
                        FlagInstances.Crossroads_ShamanTemple__Dream_Plant_Orb_1,
                        FlagInstances.Crossroads_ShamanTemple__Dream_Plant_Orb_10,
                        FlagInstances.Crossroads_ShamanTemple__Dream_Plant_Orb_11,
                        FlagInstances.Crossroads_ShamanTemple__Dream_Plant_Orb_12,
                        FlagInstances.Crossroads_ShamanTemple__Dream_Plant_Orb_13,
                        FlagInstances.Crossroads_ShamanTemple__Dream_Plant_Orb_14,
                        FlagInstances.Crossroads_ShamanTemple__Dream_Plant_Orb_15,
                        FlagInstances.Crossroads_ShamanTemple__Dream_Plant_Orb_16,
                        FlagInstances.Crossroads_ShamanTemple__Dream_Plant_Orb_17,
                        FlagInstances.Crossroads_ShamanTemple__Dream_Plant_Orb_18,
                        FlagInstances.Crossroads_ShamanTemple__Dream_Plant_Orb_19,
                        FlagInstances.Crossroads_ShamanTemple__Dream_Plant_Orb_2,
                        FlagInstances.Crossroads_ShamanTemple__Dream_Plant_Orb_20,
                        FlagInstances.Crossroads_ShamanTemple__Dream_Plant_Orb_21,
                        FlagInstances.Crossroads_ShamanTemple__Dream_Plant_Orb_22,
                        FlagInstances.Crossroads_ShamanTemple__Dream_Plant_Orb_23,
                        FlagInstances.Crossroads_ShamanTemple__Dream_Plant_Orb_24,
                        FlagInstances.Crossroads_ShamanTemple__Dream_Plant_Orb_25,
                        FlagInstances.Crossroads_ShamanTemple__Dream_Plant_Orb_26,
                        FlagInstances.Crossroads_ShamanTemple__Dream_Plant_Orb_27,
                        FlagInstances.Crossroads_ShamanTemple__Dream_Plant_Orb_28,
                        FlagInstances.Crossroads_ShamanTemple__Dream_Plant_Orb_29,
                        FlagInstances.Crossroads_ShamanTemple__Dream_Plant_Orb_3,
                        FlagInstances.Crossroads_ShamanTemple__Dream_Plant_Orb_30,
                        FlagInstances.Crossroads_ShamanTemple__Dream_Plant_Orb_31,
                        FlagInstances.Crossroads_ShamanTemple__Dream_Plant_Orb_32,
                        FlagInstances.Crossroads_ShamanTemple__Dream_Plant_Orb_33,
                        FlagInstances.Crossroads_ShamanTemple__Dream_Plant_Orb_34,
                        FlagInstances.Crossroads_ShamanTemple__Dream_Plant_Orb_35,
                        FlagInstances.Crossroads_ShamanTemple__Dream_Plant_Orb_36,
                        FlagInstances.Crossroads_ShamanTemple__Dream_Plant_Orb_37,
                        FlagInstances.Crossroads_ShamanTemple__Dream_Plant_Orb_38,
                        FlagInstances.Crossroads_ShamanTemple__Dream_Plant_Orb_39,
                        FlagInstances.Crossroads_ShamanTemple__Dream_Plant_Orb_4,
                        FlagInstances.Crossroads_ShamanTemple__Dream_Plant_Orb_40,
                        FlagInstances.Crossroads_ShamanTemple__Dream_Plant_Orb_41,
                        FlagInstances.Crossroads_ShamanTemple__Dream_Plant_Orb_5,
                        FlagInstances.Crossroads_ShamanTemple__Dream_Plant_Orb_6,
                        FlagInstances.Crossroads_ShamanTemple__Dream_Plant_Orb_7,
                        FlagInstances.Crossroads_ShamanTemple__Dream_Plant_Orb_8,
                        FlagInstances.Crossroads_ShamanTemple__Dream_Plant_Orb_9
                    }
                },
                { 
                    FlagInstances.Fungus1_13__Dream_Plant, new List<FlagDef>
                    {
                        FlagInstances.Fungus1_13__Dream_Plant_Orb_1,
                        FlagInstances.Fungus1_13__Dream_Plant_Orb_1,
                        FlagInstances.Fungus1_13__Dream_Plant_Orb_10,
                        FlagInstances.Fungus1_13__Dream_Plant_Orb_11,
                        FlagInstances.Fungus1_13__Dream_Plant_Orb_12,
                        FlagInstances.Fungus1_13__Dream_Plant_Orb_13,
                        FlagInstances.Fungus1_13__Dream_Plant_Orb_14,
                        FlagInstances.Fungus1_13__Dream_Plant_Orb_15,
                        FlagInstances.Fungus1_13__Dream_Plant_Orb_16,
                        FlagInstances.Fungus1_13__Dream_Plant_Orb_17,
                        FlagInstances.Fungus1_13__Dream_Plant_Orb_18,
                        FlagInstances.Fungus1_13__Dream_Plant_Orb_19,
                        FlagInstances.Fungus1_13__Dream_Plant_Orb_2,
                        FlagInstances.Fungus1_13__Dream_Plant_Orb_20,
                        FlagInstances.Fungus1_13__Dream_Plant_Orb_21,
                        FlagInstances.Fungus1_13__Dream_Plant_Orb_22,
                        FlagInstances.Fungus1_13__Dream_Plant_Orb_23,
                        FlagInstances.Fungus1_13__Dream_Plant_Orb_24,
                        FlagInstances.Fungus1_13__Dream_Plant_Orb_25,
                        FlagInstances.Fungus1_13__Dream_Plant_Orb_26,
                        FlagInstances.Fungus1_13__Dream_Plant_Orb_27,
                        FlagInstances.Fungus1_13__Dream_Plant_Orb_28,
                        FlagInstances.Fungus1_13__Dream_Plant_Orb_29,
                        FlagInstances.Fungus1_13__Dream_Plant_Orb_3,
                        FlagInstances.Fungus1_13__Dream_Plant_Orb_30,
                        FlagInstances.Fungus1_13__Dream_Plant_Orb_31,
                        FlagInstances.Fungus1_13__Dream_Plant_Orb_32,
                        FlagInstances.Fungus1_13__Dream_Plant_Orb_33,
                        FlagInstances.Fungus1_13__Dream_Plant_Orb_34,
                        FlagInstances.Fungus1_13__Dream_Plant_Orb_35,
                        FlagInstances.Fungus1_13__Dream_Plant_Orb_36,
                        FlagInstances.Fungus1_13__Dream_Plant_Orb_37,
                        FlagInstances.Fungus1_13__Dream_Plant_Orb_38,
                        FlagInstances.Fungus1_13__Dream_Plant_Orb_39,
                        FlagInstances.Fungus1_13__Dream_Plant_Orb_4,
                        FlagInstances.Fungus1_13__Dream_Plant_Orb_40,
                        FlagInstances.Fungus1_13__Dream_Plant_Orb_41,
                        FlagInstances.Fungus1_13__Dream_Plant_Orb_42,
                        FlagInstances.Fungus1_13__Dream_Plant_Orb_43,
                        FlagInstances.Fungus1_13__Dream_Plant_Orb_5,
                        FlagInstances.Fungus1_13__Dream_Plant_Orb_6,
                        FlagInstances.Fungus1_13__Dream_Plant_Orb_7,
                        FlagInstances.Fungus1_13__Dream_Plant_Orb_8,
                        FlagInstances.Fungus1_13__Dream_Plant_Orb_9
                    }
                },
                { 
                    FlagInstances.Fungus2_17__Dream_Plant, new List<FlagDef>
                    {
                        FlagInstances.Fungus2_17__Dream_Plant_Orb,
                        FlagInstances.Fungus2_17__Dream_Plant_Orb_1,
                        FlagInstances.Fungus2_17__Dream_Plant_Orb_10,
                        FlagInstances.Fungus2_17__Dream_Plant_Orb_11,
                        FlagInstances.Fungus2_17__Dream_Plant_Orb_12,
                        FlagInstances.Fungus2_17__Dream_Plant_Orb_13,
                        FlagInstances.Fungus2_17__Dream_Plant_Orb_14,
                        FlagInstances.Fungus2_17__Dream_Plant_Orb_15,
                        FlagInstances.Fungus2_17__Dream_Plant_Orb_16,
                        FlagInstances.Fungus2_17__Dream_Plant_Orb_17,
                        FlagInstances.Fungus2_17__Dream_Plant_Orb_2,
                        FlagInstances.Fungus2_17__Dream_Plant_Orb_3,
                        FlagInstances.Fungus2_17__Dream_Plant_Orb_4,
                        FlagInstances.Fungus2_17__Dream_Plant_Orb_5,
                        FlagInstances.Fungus2_17__Dream_Plant_Orb_6,
                        FlagInstances.Fungus2_17__Dream_Plant_Orb_7,
                        FlagInstances.Fungus2_17__Dream_Plant_Orb_8,
                        FlagInstances.Fungus2_17__Dream_Plant_Orb_9,
                    }
                },
                { 
                    FlagInstances.Cliffs_01__Dream_Plant, new List<FlagDef>
                    {
                        FlagInstances.Cliffs_01__Dream_Plant_Orb_11,
                        FlagInstances.Cliffs_01__Dream_Plant_Orb_13,
                        FlagInstances.Cliffs_01__Dream_Plant_Orb_14,
                        FlagInstances.Cliffs_01__Dream_Plant_Orb_15,
                        FlagInstances.Cliffs_01__Dream_Plant_Orb_16,
                        FlagInstances.Cliffs_01__Dream_Plant_Orb_17,
                        FlagInstances.Cliffs_01__Dream_Plant_Orb_18,
                        FlagInstances.Cliffs_01__Dream_Plant_Orb_19,
                        FlagInstances.Cliffs_01__Dream_Plant_Orb_2,
                        FlagInstances.Cliffs_01__Dream_Plant_Orb_22,
                        FlagInstances.Cliffs_01__Dream_Plant_Orb_24,
                        FlagInstances.Cliffs_01__Dream_Plant_Orb_25,
                        FlagInstances.Cliffs_01__Dream_Plant_Orb_26,
                        FlagInstances.Cliffs_01__Dream_Plant_Orb_27,
                        FlagInstances.Cliffs_01__Dream_Plant_Orb_28,
                        FlagInstances.Cliffs_01__Dream_Plant_Orb_29,
                        FlagInstances.Cliffs_01__Dream_Plant_Orb_3,
                        FlagInstances.Cliffs_01__Dream_Plant_Orb_30,
                        FlagInstances.Cliffs_01__Dream_Plant_Orb_31,
                        FlagInstances.Cliffs_01__Dream_Plant_Orb_33,
                        FlagInstances.Cliffs_01__Dream_Plant_Orb_35,
                        FlagInstances.Cliffs_01__Dream_Plant_Orb_36,
                        FlagInstances.Cliffs_01__Dream_Plant_Orb_38,
                        FlagInstances.Cliffs_01__Dream_Plant_Orb_39,
                        FlagInstances.Cliffs_01__Dream_Plant_Orb_4,
                        FlagInstances.Cliffs_01__Dream_Plant_Orb_41,
                        FlagInstances.Cliffs_01__Dream_Plant_Orb_42,
                        FlagInstances.Cliffs_01__Dream_Plant_Orb_45,
                        FlagInstances.Cliffs_01__Dream_Plant_Orb_47,
                        FlagInstances.Cliffs_01__Dream_Plant_Orb_48,
                        FlagInstances.Cliffs_01__Dream_Plant_Orb_50,
                        FlagInstances.Cliffs_01__Dream_Plant_Orb_52,
                        FlagInstances.Cliffs_01__Dream_Plant_Orb_53,
                        FlagInstances.Cliffs_01__Dream_Plant_Orb_55,
                        FlagInstances.Cliffs_01__Dream_Plant_Orb_56,
                        FlagInstances.Cliffs_01__Dream_Plant_Orb_57,
                        FlagInstances.Cliffs_01__Dream_Plant_Orb_58,
                        FlagInstances.Cliffs_01__Dream_Plant_Orb_59,
                        FlagInstances.Cliffs_01__Dream_Plant_Orb_6,
                        FlagInstances.Cliffs_01__Dream_Plant_Orb_61,
                        FlagInstances.Cliffs_01__Dream_Plant_Orb_63,
                        FlagInstances.Cliffs_01__Dream_Plant_Orb_64,
                        FlagInstances.Cliffs_01__Dream_Plant_Orb_66,
                        FlagInstances.Cliffs_01__Dream_Plant_Orb_67,
                        FlagInstances.Cliffs_01__Dream_Plant_Orb_8,
                        FlagInstances.Cliffs_01__Dream_Plant_Orb_9,
                    }
                },
                { 
                    FlagInstances.Deepnest_39__Dream_Plant, new List<FlagDef>
                    {
                        FlagInstances.Deepnest_39__Dream_Plant_Orb,
                        FlagInstances.Deepnest_39__Dream_Plant_Orb_1,
                        FlagInstances.Deepnest_39__Dream_Plant_Orb_10,
                        FlagInstances.Deepnest_39__Dream_Plant_Orb_11,
                        FlagInstances.Deepnest_39__Dream_Plant_Orb_12,
                        FlagInstances.Deepnest_39__Dream_Plant_Orb_13,
                        FlagInstances.Deepnest_39__Dream_Plant_Orb_14,
                        FlagInstances.Deepnest_39__Dream_Plant_Orb_15,
                        FlagInstances.Deepnest_39__Dream_Plant_Orb_16,
                        FlagInstances.Deepnest_39__Dream_Plant_Orb_17,
                        FlagInstances.Deepnest_39__Dream_Plant_Orb_18,
                        FlagInstances.Deepnest_39__Dream_Plant_Orb_19,
                        FlagInstances.Deepnest_39__Dream_Plant_Orb_2,
                        FlagInstances.Deepnest_39__Dream_Plant_Orb_20,
                        FlagInstances.Deepnest_39__Dream_Plant_Orb_21,
                        FlagInstances.Deepnest_39__Dream_Plant_Orb_22,
                        FlagInstances.Deepnest_39__Dream_Plant_Orb_23,
                        FlagInstances.Deepnest_39__Dream_Plant_Orb_24,
                        FlagInstances.Deepnest_39__Dream_Plant_Orb_25,
                        FlagInstances.Deepnest_39__Dream_Plant_Orb_26,
                        FlagInstances.Deepnest_39__Dream_Plant_Orb_27,
                        FlagInstances.Deepnest_39__Dream_Plant_Orb_28,
                        FlagInstances.Deepnest_39__Dream_Plant_Orb_29,
                        FlagInstances.Deepnest_39__Dream_Plant_Orb_3,
                        FlagInstances.Deepnest_39__Dream_Plant_Orb_30,
                        FlagInstances.Deepnest_39__Dream_Plant_Orb_31,
                        FlagInstances.Deepnest_39__Dream_Plant_Orb_32,
                        FlagInstances.Deepnest_39__Dream_Plant_Orb_33,
                        FlagInstances.Deepnest_39__Dream_Plant_Orb_34,
                        FlagInstances.Deepnest_39__Dream_Plant_Orb_35,
                        FlagInstances.Deepnest_39__Dream_Plant_Orb_36,
                        FlagInstances.Deepnest_39__Dream_Plant_Orb_37,
                        FlagInstances.Deepnest_39__Dream_Plant_Orb_38,
                        FlagInstances.Deepnest_39__Dream_Plant_Orb_39,
                        FlagInstances.Deepnest_39__Dream_Plant_Orb_4,
                        FlagInstances.Deepnest_39__Dream_Plant_Orb_40,
                        FlagInstances.Deepnest_39__Dream_Plant_Orb_41,
                        FlagInstances.Deepnest_39__Dream_Plant_Orb_42,
                        FlagInstances.Deepnest_39__Dream_Plant_Orb_43,
                        FlagInstances.Deepnest_39__Dream_Plant_Orb_44,
                        FlagInstances.Deepnest_39__Dream_Plant_Orb_5,
                        FlagInstances.Deepnest_39__Dream_Plant_Orb_6,
                        FlagInstances.Deepnest_39__Dream_Plant_Orb_7,
                        FlagInstances.Deepnest_39__Dream_Plant_Orb_8,
                        FlagInstances.Deepnest_39__Dream_Plant_Orb_9,
                    }
                },
                { 
                    FlagInstances.Abyss_01__Dream_Plant, new List<FlagDef>
                    {
                        FlagInstances.Abyss_01__Dream_Plant_Orb,
                        FlagInstances.Abyss_01__Dream_Plant_Orb_1,
                        FlagInstances.Abyss_01__Dream_Plant_Orb_10,
                        FlagInstances.Abyss_01__Dream_Plant_Orb_11,
                        FlagInstances.Abyss_01__Dream_Plant_Orb_12,
                        FlagInstances.Abyss_01__Dream_Plant_Orb_13,
                        FlagInstances.Abyss_01__Dream_Plant_Orb_14,
                        FlagInstances.Abyss_01__Dream_Plant_Orb_15,
                        FlagInstances.Abyss_01__Dream_Plant_Orb_16,
                        FlagInstances.Abyss_01__Dream_Plant_Orb_17,
                        FlagInstances.Abyss_01__Dream_Plant_Orb_18,
                        FlagInstances.Abyss_01__Dream_Plant_Orb_19,
                        FlagInstances.Abyss_01__Dream_Plant_Orb_2,
                        FlagInstances.Abyss_01__Dream_Plant_Orb_20,
                        FlagInstances.Abyss_01__Dream_Plant_Orb_21,
                        FlagInstances.Abyss_01__Dream_Plant_Orb_22,
                        FlagInstances.Abyss_01__Dream_Plant_Orb_23,
                        FlagInstances.Abyss_01__Dream_Plant_Orb_24,
                        FlagInstances.Abyss_01__Dream_Plant_Orb_25,
                        FlagInstances.Abyss_01__Dream_Plant_Orb_26,
                        FlagInstances.Abyss_01__Dream_Plant_Orb_27,
                        FlagInstances.Abyss_01__Dream_Plant_Orb_28,
                        FlagInstances.Abyss_01__Dream_Plant_Orb_29,
                        FlagInstances.Abyss_01__Dream_Plant_Orb_3,
                        FlagInstances.Abyss_01__Dream_Plant_Orb_30,
                        FlagInstances.Abyss_01__Dream_Plant_Orb_31,
                        FlagInstances.Abyss_01__Dream_Plant_Orb_32,
                        FlagInstances.Abyss_01__Dream_Plant_Orb_33,
                        FlagInstances.Abyss_01__Dream_Plant_Orb_34,
                        FlagInstances.Abyss_01__Dream_Plant_Orb_4,
                        FlagInstances.Abyss_01__Dream_Plant_Orb_5,
                        FlagInstances.Abyss_01__Dream_Plant_Orb_6,
                        FlagInstances.Abyss_01__Dream_Plant_Orb_7,
                        FlagInstances.Abyss_01__Dream_Plant_Orb_8,
                        FlagInstances.Abyss_01__Dream_Plant_Orb_9
                    }
                },
                { 
                    FlagInstances.Ruins1_17__Dream_Plant, new List<FlagDef>
                    {
                        FlagInstances.Ruins1_17__Dream_Plant_Orb,
                        FlagInstances.Ruins1_17__Dream_Plant_Orb_1,
                        FlagInstances.Ruins1_17__Dream_Plant_Orb_10,
                        FlagInstances.Ruins1_17__Dream_Plant_Orb_11,
                        FlagInstances.Ruins1_17__Dream_Plant_Orb_12,
                        FlagInstances.Ruins1_17__Dream_Plant_Orb_13,
                        FlagInstances.Ruins1_17__Dream_Plant_Orb_14,
                        FlagInstances.Ruins1_17__Dream_Plant_Orb_15,
                        FlagInstances.Ruins1_17__Dream_Plant_Orb_16,
                        FlagInstances.Ruins1_17__Dream_Plant_Orb_17,
                        FlagInstances.Ruins1_17__Dream_Plant_Orb_18,
                        FlagInstances.Ruins1_17__Dream_Plant_Orb_19,
                        FlagInstances.Ruins1_17__Dream_Plant_Orb_2,
                        FlagInstances.Ruins1_17__Dream_Plant_Orb_20,
                        FlagInstances.Ruins1_17__Dream_Plant_Orb_21,
                        FlagInstances.Ruins1_17__Dream_Plant_Orb_22,
                        FlagInstances.Ruins1_17__Dream_Plant_Orb_23,
                        FlagInstances.Ruins1_17__Dream_Plant_Orb_24,
                        FlagInstances.Ruins1_17__Dream_Plant_Orb_25,
                        FlagInstances.Ruins1_17__Dream_Plant_Orb_26,
                        FlagInstances.Ruins1_17__Dream_Plant_Orb_27,
                        FlagInstances.Ruins1_17__Dream_Plant_Orb_3,
                        FlagInstances.Ruins1_17__Dream_Plant_Orb_4,
                        FlagInstances.Ruins1_17__Dream_Plant_Orb_5,
                        FlagInstances.Ruins1_17__Dream_Plant_Orb_6,
                        FlagInstances.Ruins1_17__Dream_Plant_Orb_7,
                        FlagInstances.Ruins1_17__Dream_Plant_Orb_8,
                        FlagInstances.Ruins1_17__Dream_Plant_Orb_9,
                    }
                },
                { 
                    FlagInstances.RestingGrounds_08__Dream_Plant, new List<FlagDef>
                    {
                        FlagInstances.RestingGrounds_08__Dream_Plant_Orb,
                        FlagInstances.RestingGrounds_08__Dream_Plant_Orb_1,
                        FlagInstances.RestingGrounds_08__Dream_Plant_Orb_10,
                        FlagInstances.RestingGrounds_08__Dream_Plant_Orb_11,
                        FlagInstances.RestingGrounds_08__Dream_Plant_Orb_12,
                        FlagInstances.RestingGrounds_08__Dream_Plant_Orb_13,
                        FlagInstances.RestingGrounds_08__Dream_Plant_Orb_14,
                        FlagInstances.RestingGrounds_08__Dream_Plant_Orb_15,
                        FlagInstances.RestingGrounds_08__Dream_Plant_Orb_16,
                        FlagInstances.RestingGrounds_08__Dream_Plant_Orb_17,
                        FlagInstances.RestingGrounds_08__Dream_Plant_Orb_18,
                        FlagInstances.RestingGrounds_08__Dream_Plant_Orb_19,
                        FlagInstances.RestingGrounds_08__Dream_Plant_Orb_2,
                        FlagInstances.RestingGrounds_08__Dream_Plant_Orb_20,
                        FlagInstances.RestingGrounds_08__Dream_Plant_Orb_21,
                        FlagInstances.RestingGrounds_08__Dream_Plant_Orb_22,
                        FlagInstances.RestingGrounds_08__Dream_Plant_Orb_23,
                        FlagInstances.RestingGrounds_08__Dream_Plant_Orb_24,
                        FlagInstances.RestingGrounds_08__Dream_Plant_Orb_25,
                        FlagInstances.RestingGrounds_08__Dream_Plant_Orb_26,
                        FlagInstances.RestingGrounds_08__Dream_Plant_Orb_27,
                        FlagInstances.RestingGrounds_08__Dream_Plant_Orb_28,
                        FlagInstances.RestingGrounds_08__Dream_Plant_Orb_29,
                        FlagInstances.RestingGrounds_08__Dream_Plant_Orb_3,
                        FlagInstances.RestingGrounds_08__Dream_Plant_Orb_30,
                        FlagInstances.RestingGrounds_08__Dream_Plant_Orb_31,
                        FlagInstances.RestingGrounds_08__Dream_Plant_Orb_32,
                        FlagInstances.RestingGrounds_08__Dream_Plant_Orb_33,
                        FlagInstances.RestingGrounds_08__Dream_Plant_Orb_4,
                        FlagInstances.RestingGrounds_08__Dream_Plant_Orb_5,
                        FlagInstances.RestingGrounds_08__Dream_Plant_Orb_6,
                        FlagInstances.RestingGrounds_08__Dream_Plant_Orb_7,
                        FlagInstances.RestingGrounds_08__Dream_Plant_Orb_8,
                        FlagInstances.RestingGrounds_08__Dream_Plant_Orb_9
                    }
                },
            };
        }

        /// <summary>
        /// Override CreatePanels to provide custom panel creation with syncing
        /// </summary>
        public override List<CheatPanel> CreatePanels()
        {
            var panels = new List<CheatPanel>();
            var flagMappings = GetFlagMappings();
            
            foreach (var kvp in flagMappings)
            {
                var baseFlag = kvp.Key;
                var associatedFlags = kvp.Value;
                
                // Create a custom synced reference using a delegate
                var syncedReference = new DelegateReference<bool>(
                    () => FlagManager.GetBoolFlag(baseFlag),
                    (value) => 
                    {
                        FlagManager.SetBoolFlag(baseFlag, value);
                        foreach (var associatedFlag in associatedFlags)
                        {
                            FlagManager.SetBoolFlag(associatedFlag, value);
                        }
                    }
                );
                
                panels.Add(new TogglePanel(syncedReference, baseFlag.Scene?.ReadableName));
            }
            
            return panels;
        }
    }
} 