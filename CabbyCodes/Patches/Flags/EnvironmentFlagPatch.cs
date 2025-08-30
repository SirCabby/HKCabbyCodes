using System;
using System.Collections.Generic;
using CabbyCodes.Flags;
using CabbyCodes.Patches.BasePatches;
using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Scenes;

namespace CabbyCodes.Patches.Flags
{
    public class EnvironmentFlagPatch : BasePatch
    {
        /// <summary>
        /// Mapping of door flags to the scenes where they should be disabled.
        /// These doors cannot be interacted with when the player is in these specific scenes.
        /// </summary>
        private static readonly Dictionary<FlagDef, SceneMapData> DoorSceneRestrictions = new Dictionary<FlagDef, SceneMapData>
        {
            { FlagInstances.openedCityGate, SceneInstances.Fungus2_21 },
            { FlagInstances.openedLoveDoor, SceneInstances.Ruins2_11_b },
            { FlagInstances.usedWhiteKey, SceneInstances.Ruins1_31 },
            { FlagInstances.openedWaterwaysManhole, SceneInstances.Ruins1_05b },
            { FlagInstances.jijiDoorUnlocked, SceneInstances.Town },
            { FlagInstances.bathHouseOpened, SceneInstances.Ruins2_04 },
            { FlagInstances.godseekerUnlocked, SceneInstances.GG_Waterways }
        };

        /// <summary>
        /// Checks if a door flag should be disabled based on the current scene.
        /// </summary>
        /// <param name="doorFlag">The door flag to check</param>
        /// <returns>True if the door should be disabled in the current scene</returns>
        private static bool IsDoorDisabledInCurrentScene(FlagDef doorFlag)
        {
            if (!DoorSceneRestrictions.TryGetValue(doorFlag, out var restrictedScene))
            {
                return false; // No restrictions for this door
            }

            string currentScene = GameStateProvider.GetCurrentSceneName();
            return restrictedScene.SceneName == currentScene;
        }

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
                FlagInstances.summonedMonomon,
                FlagInstances.blueVineDoor,
                FlagInstances.openedBlackEggPath,
                FlagInstances.openedBlackEggDoor,
                FlagInstances.unchainedHollowKnight,
                FlagInstances.galienPinned,
                FlagInstances.huPinned,
                FlagInstances.markothPinned,
                FlagInstances.noEyesPinned,
                FlagInstances.xeroPinned,
                FlagInstances.nightmareLanternAppeared,
                FlagInstances.nightmareLanternLit,
                FlagInstances.troupeInTown,
                FlagInstances.destroyedNightmareLantern,
                FlagInstances.greyPrinceDefeats,
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
            var hasKeyOrDoorOpen = FlagManager.GetBoolFlag(doorFlag) || FlagManager.GetBoolFlag(keyFlag);
            var isInRestrictedScene = IsDoorDisabledInCurrentScene(doorFlag);
            var shouldBeInteractable = hasKeyOrDoorOpen && !isInRestrictedScene;

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
                string finalMessage = disabledMessage;
                if (isInRestrictedScene)
                {
                    finalMessage = $"Cannot change this flag while in the flag's scene.\n\n{disabledMessage}";
                }
                toggleButton.SetDisabledMessage(finalMessage);
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
            var isInRestrictedScene = IsDoorDisabledInCurrentScene(doorFlag);
            
            // Panel is disabled if the door is closed and there are no simple keys, OR if in restricted scene
            var shouldBeInteractable = !(isDoorOpen == false && currentSimpleKeys == 0) && !isInRestrictedScene;

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
                string message = "Cannot open door without Simple Keys";
                if (isInRestrictedScene)
                {
                    message = $"Cannot interact with flag while in flag's scene\n\n{message}";
                }
                toggleButton.SetDisabledMessage(message);
            }
            else
            {
                toggleButton.SetDisabledMessage(""); // Clear message when enabled
            }

            return togglePanel;
        }
    }
} 