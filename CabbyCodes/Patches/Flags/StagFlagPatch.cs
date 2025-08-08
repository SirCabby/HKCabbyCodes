using CabbyCodes.Flags;
using CabbyCodes.Patches.BasePatches;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.SyncedReferences;
using System;
using System.Collections.Generic;

namespace CabbyCodes.Patches.Flags
{
    public class StagFlagPatch : BasePatch
    {
        public override List<CheatPanel> CreatePanels()
        {
            var panels = new List<CheatPanel>();
            
            panels.AddRange(CreateStagPanels(new[] { 
                FlagInstances.openedCrossroads
            }));
            
            return panels;
        }
        
        private List<CheatPanel> CreateStagPanels(FlagDef[] flags)
        {
            var panels = new List<CheatPanel>();
            foreach (var flag in flags)
            {
                panels.Add(new TogglePanel(
                    new DelegateReference<bool>(
                        () => FlagManager.GetBoolFlag(flag),
                        value =>
                        {
                            // Get current stations count before changing the flag
                            int currentStationsOpened = FlagManager.GetIntFlag(FlagInstances.stationsOpened);
                            bool wasStationOpened = FlagManager.GetBoolFlag(flag);
                            
                            // Set the station flag
                            FlagManager.SetBoolFlag(flag, value);
                            
                            // Update stationsOpened count
                            if (value && !wasStationOpened)
                            {
                                // Station is being opened - increment count
                                FlagManager.SetIntFlag(FlagInstances.stationsOpened, currentStationsOpened + 1);
                            }
                            else if (!value && wasStationOpened)
                            {
                                // Station is being closed - decrement count
                                FlagManager.SetIntFlag(FlagInstances.stationsOpened, Math.Max(0, currentStationsOpened - 1));
                            }
                        }
                    ), flag.ReadableName));
            }
            return panels;
        }
    }
} 