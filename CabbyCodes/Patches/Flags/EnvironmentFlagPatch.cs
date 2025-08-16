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
                FlagInstances.restingGroundsCryptWall,
                FlagInstances.dreamReward2,
                FlagInstances.mageLordOrbsCollected,

                




                
                FlagInstances.galienPinned,
                FlagInstances.huPinned,
                FlagInstances.nightmareLanternAppeared,
                FlagInstances.nightmareLanternLit,
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
                    panels.Add(CreateCityGatePanel());
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

        private TogglePanel CreateCityGatePanel()
        {
            var flag = FlagInstances.openedCityGate;
            var childFlag = FlagInstances.hasCityKey;
            var shouldBeInteractable = FlagManager.GetBoolFlag(flag) || FlagManager.GetBoolFlag(childFlag);

            var togglePanel = new TogglePanel(new DelegateReference<bool>(
                () => FlagManager.GetBoolFlag(flag),
                value =>
                {
                    FlagManager.SetBoolFlag(flag, value);
                    FlagManager.SetBoolFlag(childFlag, !value);
                }
            ), flag.ReadableName);

            var toggleButton = togglePanel.GetToggleButton();
            toggleButton.SetInteractable(shouldBeInteractable);
            
            // Set disabled message for hover popup
            if (!shouldBeInteractable)
            {
                toggleButton.SetDisabledMessage("Cannot open City Gate without City Crest\n\nGet or toggle the City Crest to toggle this gate");
            }
            else
            {
                toggleButton.SetDisabledMessage(""); // Clear message when enabled
            }

            return togglePanel;
        }
    }
} 