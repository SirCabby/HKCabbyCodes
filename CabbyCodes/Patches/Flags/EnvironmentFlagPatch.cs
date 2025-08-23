using System;
using System.Collections.Generic;
using CabbyCodes.Flags;
using CabbyCodes.Patches.BasePatches;
using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;

namespace CabbyCodes.Patches.Flags
{
    public class EnvironmentFlagPatch : BasePatch
    {
        protected override FlagDef[] GetFlags()
        {
            return new FlagDef[]
            {
                FlagInstances.openedMapperShop,
                FlagInstances.openedTownBuilding,
                FlagInstances.jijiDoorUnlocked,
                FlagInstances.bankerAccountPurchased,
                FlagInstances.bankerBalance,
                FlagInstances.menderSignBroken,
                FlagInstances.menderState, // Might need to merge with prior flag
                FlagInstances.crossroadsInfected,
                FlagInstances.crossroadsMawlekWall,
                FlagInstances.shamanPillar,
                FlagInstances.defeatedDoubleBlockers,
                FlagInstances.megaMossChargerDefeated,
                FlagInstances.deepnestWall,
                FlagInstances.deepnestBridgeCollapsed,
                FlagInstances.steppedBeyondBridge,
                FlagInstances.spiderCapture,
                FlagInstances.tollBenchCity,
                FlagInstances.cityLift1,
                FlagInstances.cityBridge1,
                FlagInstances.cityBridge2,
                FlagInstances.cityGateClosed,
                FlagInstances.openedCityGate,
                FlagInstances.bathHouseOpened,
                FlagInstances.restingGroundsCryptWall,
                FlagInstances.dreamReward2,
                FlagInstances.mageLordOrbsCollected,
                FlagInstances.spaBugsEncountered,
                FlagInstances.mineLiftOpened,
                FlagInstances.blizzardEnded,
                FlagInstances.abyssGateOpened,
                FlagInstances.oneWayArchive,
                FlagInstances.tollBenchQueensGardens,
                FlagInstances.openedLoveDoor,
                FlagInstances.usedWhiteKey,
                FlagInstances.openedWaterwaysManhole,
                FlagInstances.godseekerUnlocked,

                


                FlagInstances.nightmareLanternAppeared,
                FlagInstances.nightmareLanternLit,

                


                FlagInstances.galienPinned,
                FlagInstances.huPinned,
                FlagInstances.markothPinned,
                FlagInstances.noEyesPinned,
                FlagInstances.xeroPinned
            };
        }

        /// <summary>
        /// Override CreatePanels to support teleport functionality for boss flags
        /// </summary>
        /// <returns>List of panels to display</returns>
        public override List<CheatPanel> CreatePanels()
        {
            var flags = GetFlags();
            var panels = new List<CheatPanel>();
            
            foreach (var flag in flags)
            {
                if (flag == FlagInstances.openedCityGate)
                {
                    panels.Add(CreateDoorPanel(flag, FlagInstances.hasCityKey, "Cannot open City Gate without City Crest\n\nGet or toggle the City Crest to toggle this gate"));
                    continue;
                }
                else if (flag == FlagInstances.openedLoveDoor)
                {
                    panels.Add(CreateDoorPanel(flag, FlagInstances.hasLoveKey, "Cannot open Love Door without Love Key\n\nGet or toggle the Love Key to toggle this door"));
                    continue;
                }
                else if (flag == FlagInstances.usedWhiteKey)
                {
                    panels.Add(CreateDoorPanel(flag, FlagInstances.hasWhiteKey, "Must have the Elegant Key to open this door", FlagInstances.openedMageDoor_v2));
                    continue;
                }
                else if (flag == FlagInstances.openedWaterwaysManhole)
                {
                    panels.Add(CreateSimpleKeyDoorPanel(flag));
                    continue;
                }
                else if (flag == FlagInstances.jijiDoorUnlocked)
                {
                    panels.Add(CreateSimpleKeyDoorPanel(flag));
                    continue;
                }
                else if (flag == FlagInstances.bathHouseOpened)
                {
                    panels.Add(CreateSimpleKeyDoorPanel(flag));
                    continue;
                }
                else if (flag == FlagInstances.godseekerUnlocked)
                {
                    panels.Add(CreateSimpleKeyDoorPanel(flag));
                    continue;
                }
                else if (flag == FlagInstances.dreamReward2)
                {
                    panels.Add(NpcFlagPatch.CreateDreamRewardPanel(flag));
                    continue;
                }

                var flagPatch = CreatePatch(flag);
                panels.Add(flagPatch.CreatePanel());
            }
            
            return panels;
        }

        /// <summary>
        /// Creates a door panel with key dependency logic
        /// </summary>
        /// <param name="doorFlag">The flag representing the door state</param>
        /// <param name="keyFlag">The flag representing the key possession</param>
        /// <param name="disabledMessage">Message to show when the door cannot be interacted with</param>
        /// <param name="additionalFlag">Optional additional flag to update when door state changes</param>
        /// <returns>A toggle panel for the door</returns>
        private TogglePanel CreateDoorPanel(FlagDef doorFlag, FlagDef keyFlag, string disabledMessage, FlagDef additionalFlag = null)
        {
            var shouldBeInteractable = FlagManager.GetBoolFlag(doorFlag) || FlagManager.GetBoolFlag(keyFlag);

            var togglePanel = new TogglePanel(new DelegateReference<bool>(
                () => FlagManager.GetBoolFlag(doorFlag),
                value =>
                {
                    FlagManager.SetBoolFlag(doorFlag, value);
                    FlagManager.SetBoolFlag(keyFlag, !value);
                    
                    // Update additional flag if provided
                    if (additionalFlag != null)
                    {
                        FlagManager.SetBoolFlag(additionalFlag, value);
                    }
                }
            ), doorFlag.ReadableName);

            var toggleButton = togglePanel.GetToggleButton();
            toggleButton.SetInteractable(shouldBeInteractable);
            
            // Set disabled message for hover popup
            if (!shouldBeInteractable)
            {
                toggleButton.SetDisabledMessage(disabledMessage);
            }
            else
            {
                toggleButton.SetDisabledMessage(""); // Clear message when enabled
            }

            return togglePanel;
        }

        /// <summary>
        /// Creates a door panel that requires simple keys and manages the simple key count
        /// </summary>
        /// <param name="doorFlag">The flag representing the door state</param>
        /// <returns>A toggle panel for the door</returns>
        private TogglePanel CreateSimpleKeyDoorPanel(FlagDef doorFlag)
        {
            var currentSimpleKeys = FlagManager.GetIntFlag(FlagInstances.simpleKeys);
            var isDoorOpen = FlagManager.GetBoolFlag(doorFlag);
            
            // Panel is disabled if the door is closed and there are no simple keys
            var shouldBeInteractable = !(isDoorOpen == false && currentSimpleKeys == 0);

            var togglePanel = new TogglePanel(new DelegateReference<bool>(
                () => FlagManager.GetBoolFlag(doorFlag),
                value =>
                {
                    var simpleKeys = FlagManager.GetIntFlag(FlagInstances.simpleKeys);
                    
                    if (value)
                    {
                        // Opening the door consumes a simple key
                        FlagManager.SetIntFlag(FlagInstances.simpleKeys, Math.Max(0, simpleKeys - 1));
                    }
                    else
                    {
                        // Closing the door returns a simple key
                        FlagManager.SetIntFlag(FlagInstances.simpleKeys, simpleKeys + 1);
                    }
                    
                    FlagManager.SetBoolFlag(doorFlag, value);
                }
            ), doorFlag.ReadableName);

            var toggleButton = togglePanel.GetToggleButton();
            toggleButton.SetInteractable(shouldBeInteractable);
            
            // Set disabled message for hover popup
            if (!shouldBeInteractable)
            {
                toggleButton.SetDisabledMessage("Cannot open door without Simple Keys");
            }
            else
            {
                toggleButton.SetDisabledMessage(""); // Clear message when enabled
            }

            return togglePanel;
        }


    }
} 