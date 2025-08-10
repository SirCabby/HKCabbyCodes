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