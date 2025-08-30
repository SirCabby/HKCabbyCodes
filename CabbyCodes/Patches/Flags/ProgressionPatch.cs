using CabbyCodes.Flags;
using CabbyCodes.Patches.BasePatches;
using CabbyMenu.UI.CheatPanels;
using CabbyCodes.Patches.Teleport;
using CabbyMenu.SyncedReferences;
using System;
using System.Collections.Generic;
using CabbyCodes.Scenes;
using UnityEngine;
using UnityEngine.EventSystems;
using CabbyMenu.UI.Controls;
using CabbyMenu.UI.Controls.CustomDropdown;

namespace CabbyCodes.Patches.Flags
{
    public class ProgressionPatch : BasePatch
    {
        /// <summary>
        /// Dictionary of dreamer locations with their teleport coordinates
        /// </summary>
        private static readonly Dictionary<string, TeleportLocation> teleportLocations = TeleportLocations();

        /// <summary>
        /// Creates the dreamer locations dictionary with teleport coordinates
        /// </summary>
        private static Dictionary<string, TeleportLocation> TeleportLocations()
        {
            var locations = new Dictionary<string, TeleportLocation>
            {
                [FlagInstances.hegemolDefeated.ReadableName] = new TeleportLocation(SceneInstances.Deepnest_Spider_Town, new Vector2(58, 153)),
                [FlagInstances.lurienDefeated.ReadableName] = new TeleportLocation(SceneInstances.Ruins2_Watcher_Room, new Vector2(52, 136)),
                [FlagInstances.monomonDefeated.ReadableName] = new TeleportLocation(SceneInstances.Fungus3_archive_02, new Vector2(46, 85)),
            };
            
            return locations;
        }

        public override List<CheatPanel> CreatePanels()
        {
            var panels = new List<CheatPanel>
            {
                new InfoPanel("Dreamers").SetColor(CheatPanel.subHeaderColor)
            };
            panels.AddRange(CreateDreamerPanels(new[] {
                new[] { FlagInstances.hegemolDefeated, FlagInstances.maskBrokenHegemol },
                new[] { FlagInstances.lurienDefeated, FlagInstances.maskBrokenLurien },
                new[] { FlagInstances.monomonDefeated, FlagInstances.maskBrokenMonomon }
            }));
            panels.Add(new DropdownPanel(CreateColosseumValueList(), "Colosseum Status", Constants.DEFAULT_PANEL_HEIGHT));

            panels.Add(new InfoPanel("Grimm Troupe").SetColor(CheatPanel.headerColor));
            panels.Add(new ToggleWithTeleportPanel(new BoolPatch(FlagInstances.gotBrummsFlame),
                    () => {
                        TeleportService.DoTeleport(new TeleportLocation(SceneInstances.Room_spider_small, new Vector2(19, 14)));
                    }, $"{FlagInstances.gotBrummsFlame.ReadableName}"));

            panels.Add(new InfoPanel("Godhome").SetColor(CheatPanel.headerColor));
            panels.Add(new TogglePanel(new BoolPatch(FlagInstances.bossDoorCageUnlocked), FlagInstances.bossDoorCageUnlocked.ReadableName));
            panels.Add(new TogglePanel(new BoolPatch(FlagInstances.finalBossDoorUnlocked), FlagInstances.finalBossDoorUnlocked.ReadableName));
            panels.Add(new TogglePanel(new BoolPatch(FlagInstances.blueRoomDoorUnlocked), FlagInstances.blueRoomDoorUnlocked.ReadableName));
            panels.Add(new TogglePanel(new BoolPatch(FlagInstances.blueRoomActivated), FlagInstances.blueRoomActivated.ReadableName));
            panels.Add(new TogglePanel(new BoolPatch(FlagInstances.zoteStatueWallBroken), FlagInstances.zoteStatueWallBroken.ReadableName));
            panels.Add(new TogglePanel(new BoolPatch(FlagInstances.ordealAchieved), FlagInstances.ordealAchieved.ReadableName));
            panels.AddRange(CreateBossStatuePanels());
            panels.AddRange(CreateBossDoorPanels());

            return panels;
        }

        /// <summary>
        /// Creates a DelegateValueList for managing colosseum flags through a dropdown
        /// </summary>
        private DelegateValueList CreateColosseumValueList()
        {
            return new DelegateValueList(
                // Getter: returns the current colosseum state as an integer
                () =>
                {
                    // Check each colosseum tier and return appropriate value
                    if (FlagManager.GetBoolFlag(FlagInstances.colosseumGoldCompleted))
                        return 6; // Gold completed
                    if (FlagManager.GetBoolFlag(FlagInstances.colosseumGoldOpened))
                        return 5; // Gold opened
                    if (FlagManager.GetBoolFlag(FlagInstances.colosseumSilverCompleted))
                        return 4; // Silver completed
                    if (FlagManager.GetBoolFlag(FlagInstances.colosseumSilverOpened))
                        return 3; // Silver opened
                    if (FlagManager.GetBoolFlag(FlagInstances.colosseumBronzeCompleted))
                        return 2; // Bronze completed
                    if (FlagManager.GetBoolFlag(FlagInstances.colosseumBronzeOpened))
                        return 1; // Bronze opened
                    return 0; // None opened
                },
                // Setter: sets the colosseum flags based on the selected value
                (value) =>
                {
                    // Set flags based on selected value
                    switch (value)
                    {
                        case 0: // None opened
                            FlagManager.SetBoolFlag(FlagInstances.colosseumBronzeOpened, false);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumBronzeCompleted, false);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumSilverOpened, false);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumSilverCompleted, false);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumGoldOpened, false);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumGoldCompleted, false);
                            break;
                        case 1: // Bronze opened
                            FlagManager.SetBoolFlag(FlagInstances.colosseumBronzeOpened, true);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumBronzeCompleted, false);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumSilverOpened, false);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumSilverCompleted, false);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumGoldOpened, false);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumGoldCompleted, false);
                            break;
                        case 2: // Bronze completed
                            FlagManager.SetBoolFlag(FlagInstances.colosseumBronzeOpened, true);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumBronzeCompleted, true);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumSilverOpened, false);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumSilverCompleted, false);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumGoldOpened, false);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumGoldCompleted, false);
                            break;
                        case 3: // Silver opened
                            FlagManager.SetBoolFlag(FlagInstances.colosseumBronzeOpened, true);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumBronzeCompleted, true);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumSilverOpened, true);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumSilverCompleted, false);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumGoldOpened, false);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumGoldCompleted, false);
                            break;
                        case 4: // Silver completed
                            FlagManager.SetBoolFlag(FlagInstances.colosseumBronzeOpened, true);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumBronzeCompleted, true);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumSilverOpened, true);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumSilverCompleted, true);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumGoldOpened, false);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumGoldCompleted, false);
                            break;
                        case 5: // Gold opened
                            FlagManager.SetBoolFlag(FlagInstances.colosseumBronzeOpened, true);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumBronzeCompleted, true);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumSilverOpened, true);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumSilverCompleted, true);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumGoldOpened, true);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumGoldCompleted, false);
                            break;
                        case 6: // Gold completed
                            FlagManager.SetBoolFlag(FlagInstances.colosseumBronzeOpened, true);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumBronzeCompleted, true);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumSilverOpened, true);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumSilverCompleted, true);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumGoldOpened, true);
                            FlagManager.SetBoolFlag(FlagInstances.colosseumGoldCompleted, true);
                            break;
                    }
                },
                // List provider: returns the list of available colosseum states
                () => new List<string>
                {
                    "None Opened",
                    "Bronze Opened",
                    "Bronze Completed",
                    "Silver Opened",
                    "Silver Completed",
                    "Gold Opened",
                    "Gold Completed"
                }
            );
        }

        /// <summary>
        /// Creates panels for managing boss statue states
        /// </summary>
        private List<CheatPanel> CreateBossStatuePanels()
        {
            var panels = new List<CheatPanel>();
            
            // Group boss statues by category for better organization
            var bossGroups = new[]
            {
                // Floor 1
                new[] {
                    FlagInstances.statueStateGruzMother,
                    FlagInstances.statueStateVengefly,
                    FlagInstances.statueStateBroodingMawlek,
                    FlagInstances.statueStateFalseKnight,
                    FlagInstances.statueStateFailedChampion,
                    FlagInstances.statueStateHornet1,
                    FlagInstances.statueStateHornet2,
                    FlagInstances.statueStateMegaMossCharger,
                    FlagInstances.statueStateFlukemarm,
                    FlagInstances.statueStateMantisLords,
                    FlagInstances.statueStateMantisLordsExtra,
                    FlagInstances.statueStateOblobbles,
                    FlagInstances.statueStateHiveKnight,
                    FlagInstances.statueStateBrokenVessel,
                    FlagInstances.statueStateLostKin,
                    FlagInstances.statueStateNosk,
                    FlagInstances.statueStateNoskHornet,
                    FlagInstances.statueStateCollector,
                    FlagInstances.statueStateGodTamer,
                    FlagInstances.statueStateCrystalGuardian1,
                    FlagInstances.statueStateCrystalGuardian2,
                    FlagInstances.statueStateUumuu,
                    FlagInstances.statueStateTraitorLord,
                    FlagInstances.statueStateGreyPrince
                },
                // Floor 2
                new [] {
                    FlagInstances.statueStateMageKnight,
                    FlagInstances.statueStateSoulMaster,
                    FlagInstances.statueStateSoulTyrant,
                    FlagInstances.statueStateDungDefender,
                    FlagInstances.statueStateWhiteDefender,
                    FlagInstances.statueStateWatcherKnights,
                    FlagInstances.statueStateNoEyes,
                    FlagInstances.statueStateMarmu,
                    FlagInstances.statueStateXero,
                    FlagInstances.statueStateMarkoth,
                    FlagInstances.statueStateGalien,
                    FlagInstances.statueStateGorb,
                    FlagInstances.statueStateElderHu,
                    FlagInstances.statueStateNailmasters,
                    FlagInstances.statueStatePaintmaster,
                    FlagInstances.statueStateSly,
                    FlagInstances.statueStateHollowKnight,
                    FlagInstances.statueStateGrimm,
                }

                    // FlagInstances.statueStateNoskHornet,
                //     FlagInstances.statueStateMantisLordsExtra,
                //     FlagInstances.statueStateHollowKnight,
                //     FlagInstances.statueStateRadiance,
                //     FlagInstances.statueStateGrimm,
                //     FlagInstances.statueStateNightmareGrimm,
                //     FlagInstances.statueStateGreyPrince,
                //     FlagInstances.statueStateSly,
                //     FlagInstances.statueStatePaintmaster,
                //     FlagInstances.statueStateZote
            };

            var groupNames = new[] { "Floor 1", "Floor 2" };

            for (int i = 0; i < bossGroups.Length; i++)
            {
                panels.Add(new InfoPanel(groupNames[i]).SetColor(CheatPanel.subHeaderColor));
                panels.AddRange(CreateBossStatueGroupPanels(bossGroups[i]));
                // After adding, mark disable logic on last added panels maybe done in CreateBossStatueGroupPanels
            }

            return panels;
        }

        /// <summary>
        /// Creates panels for a group of boss statues
        /// </summary>
        private List<CheatPanel> CreateBossStatueGroupPanels(FlagDef[] bossFlags)
        {
            var panels = new List<CheatPanel>();
            
            foreach (var bossFlag in bossFlags)
            {
                // Add a subheader for each statue
                var bossName = bossFlag.ReadableName.Replace("statueState", "");
                panels.Add(new InfoPanel(bossName).SetColor(CheatPanel.subHeaderColor));

                // Add individual toggles for each boolean field we care about
                panels.AddRange(CreateBossStatueTogglePanels(bossFlag));
            }
            
            return panels;
        }

        /// <summary>
        /// Creates individual toggle panels for the different boolean fields of a boss statue
        /// </summary>
        private List<CheatPanel> CreateBossStatueTogglePanels(FlagDef bossFlag)
        {
            var panels = new List<CheatPanel>();

            // Ordered so they appear logically: Seen -> Unlocked -> Tier1 -> Tier2 -> Tier3
            var fieldInfo = new (string fieldName, string label)[]
            {
                ("hasBeenSeen", "Seen"),
                ("isUnlocked", "Unlocked"),
                ("completedTier1", "Tier 1 Completed"),
                ("completedTier2", "Tier 2 Completed"),
                ("completedTier3", "Tier 3 Completed")
            };

            foreach (var (fieldName, label) in fieldInfo)
            {
                
                var toggleReference = new DelegateReference<bool>(
                    () =>
                    {
                        var data = FlagManager.GetCompletionFlag(bossFlag);
                        if (data == null)
                        {
                            Debug.LogWarning($"[Boss Statue] Data is null for {bossFlag?.ReadableName}");
                            return false;
                        }
                        var result = GetBoolField(data, fieldName);
                        return result;
                    },
                    value =>
                    {
                        var data = FlagManager.GetCompletionFlag(bossFlag);
                        if (data == null) 
                        {
                            Debug.LogWarning($"[Boss Statue] Cannot set field {fieldName} - data is null");
                            return;
                        }
                        SetBoolField(data, fieldName, value);
                        FlagManager.SetCompletionFlag(bossFlag, data);
                    });

                var togglePanel = new TogglePanel(toggleReference, label);

                // Disable interaction inside GG_Workshop
                togglePanel.updateActions.Add(() =>
                {
                    bool interactable = GameStateProvider.GetCurrentSceneName() != "GG_Workshop";
                    var toggleButton = togglePanel.GetToggleButton();
                    if (toggleButton != null)
                    {
                        toggleButton.SetInteractable(interactable);
                        if (!interactable)
                        {
                            toggleButton.SetDisabledMessage("Cannot edit boss statue flags while inside Godhome Workshop");
                        }
                        else
                        {
                            toggleButton.SetDisabledMessage("");
                        }
                    }
                    else
                    {
                        Debug.LogError($"[Boss Statue] Toggle button is null for {bossFlag?.ReadableName}");
                    }
                });

                // Execute once to set initial state
                if (togglePanel.updateActions.Count > 0)
                {
                    togglePanel.updateActions[togglePanel.updateActions.Count - 1]();
                }
                else
                {
                    Debug.LogWarning($"[Boss Statue] No update actions found for {bossFlag?.ReadableName}");
                }

                panels.Add(togglePanel);
            }

            return panels;
        }
        
        /// <summary>
        /// Helper method to safely get a boolean field value using reflection
        /// </summary>
        private bool GetBoolField(object obj, string fieldName)
        {
            try
            {
                if (obj == null)
                {
                    Debug.LogError($"[Boss Statue] GetBoolField: obj is null");
                    return false;
                }
                
                var field = obj.GetType().GetField(fieldName);
                if (field != null && field.FieldType == typeof(bool))
                {
                    var value = (bool)field.GetValue(obj);
                    return value;
                }
                else
                {
                    Debug.LogWarning($"[Boss Statue] GetBoolField: field {fieldName} not found or not bool type");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[Boss Statue] Failed to get field {fieldName}: {ex.Message}");
            }
            return false;
        }
        
        /// <summary>
        /// Helper method to safely set a boolean field value using reflection
        /// </summary>
        private void SetBoolField(object obj, string fieldName, bool value)
        {
            try
            {
                if (obj == null)
                {
                    Debug.LogError($"[Boss Statue] SetBoolField: obj is null");
                    return;
                }
                
                var field = obj.GetType().GetField(fieldName);
                if (field != null && field.FieldType == typeof(bool))
                {
                    field.SetValue(obj, value);
                }
                else
                {
                    Debug.LogWarning($"[Boss Statue] SetBoolField: field {fieldName} not found or not bool type");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[Boss Statue] Failed to set field {fieldName}: {ex.Message}");
            }
        }

        /// <summary>
        /// Ensures a HoverPopup on the dropdown button, wiring enter/exit triggers, and sets message.
        /// </summary>
        private void ConfigureDropdownHover(CustomDropdown dropdown, string message)
        {
            if (dropdown == null) return;
            var go = dropdown.gameObject;

            // Add or get HoverPopup component
            var hover = go.GetComponent<HoverPopup>();
            if (hover == null)
            {
                hover = go.AddComponent<HoverPopup>();
            }

            // Add EventTrigger for pointer enter/exit if not present
            var trigger = go.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = go.AddComponent<EventTrigger>();
            }

            // Local functions to add entries only once
            void AddEntry(EventTriggerType type, UnityEngine.Events.UnityAction<BaseEventData> action)
            {
                bool exists = false;
                foreach (var e in trigger.triggers)
                {
                    if (e.eventID == type)
                    {
                        exists = true;
                        break;
                    }
                }
                if (!exists)
                {
                    var entry = new EventTrigger.Entry { eventID = type };
                    entry.callback.AddListener(action);
                    trigger.triggers.Add(entry);
                }
            }

            AddEntry(EventTriggerType.PointerEnter, (data) => { if (!go.GetComponent<UnityEngine.UI.Button>().interactable) hover.ShowPopup(message); });
            AddEntry(EventTriggerType.PointerExit, (data) => { hover.HidePopup(); });
        }

        private List<CheatPanel> CreateBossDoorPanels()
        {
            var panels = new List<CheatPanel>();

            var bossDoorFlags = new[]
            {
                FlagInstances.bossDoorStateTier1,
                FlagInstances.bossDoorStateTier2,
                FlagInstances.bossDoorStateTier3,
                FlagInstances.bossDoorStateTier4,
                FlagInstances.bossDoorStateTier5
            };

            for (int i = 0; i < bossDoorFlags.Length; i++)
            {
                var doorFlag = bossDoorFlags[i];

                // Subheader for each door
                panels.Add(new InfoPanel(doorFlag.ReadableName).SetColor(CheatPanel.subHeaderColor));

                // Dropdown for main status (canUnlock, unlocked, completed)
                var doorDropdownPanel = new DropdownPanel(CreateBossDoorValueList(doorFlag), "Door Status", Constants.DEFAULT_PANEL_HEIGHT);
                // Disable when in GG_Atrium
                doorDropdownPanel.updateActions.Add(() =>
                {
                    bool interactable = GameStateProvider.GetCurrentSceneName() != "GG_Atrium";
                    var dropdown = doorDropdownPanel.GetDropDownSync()?.GetCustomDropdown();
                    dropdown?.SetInteractable(interactable);
                });
                // Execute to set initial state
                if (doorDropdownPanel.updateActions.Count > 0)
                {
                    doorDropdownPanel.updateActions[doorDropdownPanel.updateActions.Count - 1]();
                }
                ConfigureDropdownHover(doorDropdownPanel.GetDropDownSync()?.GetCustomDropdown(), "Cannot edit boss door flags while inside Godhome Atrium");
                panels.Add(doorDropdownPanel);

                // Toggles for additional fields
                var extraFields = new[] { "allBindings", "noHits", "boundNail", "boundShell", "boundCharms", "boundSoul" };
                foreach (var fieldName in extraFields)
                {
                    var toggleReference = new DelegateReference<bool>(
                        () =>
                        {
                            var data = FlagManager.GetCompletionFlag(doorFlag);
                            if (data == null) return false;
                            var result = GetBoolField(data, fieldName);
                            return result;
                        },
                        value =>
                        {
                            var data = FlagManager.GetCompletionFlag(doorFlag);
                            if (data == null) 
                            {
                                return;
                            }
                            SetBoolField(data, fieldName, value);
                            FlagManager.SetCompletionFlag(doorFlag, data);
                        }
                    );

                    // Convert field name to a nicer label, e.g. "boundNail" -> "Bound Nail"
                    var label = System.Text.RegularExpressions.Regex.Replace(fieldName, "([a-z])([A-Z])", "$1 $2");
                    label = char.ToUpper(label[0]) + label.Substring(1);

                    var togglePanel = new TogglePanel(toggleReference, label);
                    togglePanel.updateActions.Add(() =>
                    {
                        bool interactable = GameStateProvider.GetCurrentSceneName() != "GG_Atrium";
                        var toggleButton = togglePanel.GetToggleButton();
                        if (toggleButton != null)
                        {
                            toggleButton.SetInteractable(interactable);
                            if (!interactable)
                            {
                                toggleButton.SetDisabledMessage("Cannot edit boss door flags while inside Godhome Atrium");
                            }
                            else
                            {
                                toggleButton.SetDisabledMessage("");
                            }
                        }
                    });
                    // Execute once immediately to set initial state
                    if (togglePanel.updateActions.Count > 0)
                    {
                        togglePanel.updateActions[togglePanel.updateActions.Count - 1]();
                    }
                    panels.Add(togglePanel);
                }
            }

            return panels;
        }

        /// <summary>
        /// Creates a DelegateValueList for managing boss door state (canUnlock/unlocked/completed)
        /// </summary>
        private DelegateValueList CreateBossDoorValueList(FlagDef doorFlag)
        {
            return new DelegateValueList(
                // Getter
                () =>
                {
                    var data = FlagManager.GetCompletionFlag(doorFlag);
                    if (data == null) return 0;

                    if (GetBoolField(data, "completed")) return 3;
                    if (GetBoolField(data, "unlocked")) return 2;
                    if (GetBoolField(data, "canUnlock")) return 1;
                    return 0;
                },
                // Setter
                value =>
                {
                    var data = FlagManager.GetCompletionFlag(doorFlag);
                    if (data == null) return;

                    switch (value)
                    {
                        case 0: // Locked
                            SetBoolField(data, "canUnlock", false);
                            SetBoolField(data, "unlocked", false);
                            SetBoolField(data, "completed", false);
                            break;
                        case 1: // Can Unlock
                            SetBoolField(data, "canUnlock", true);
                            SetBoolField(data, "unlocked", false);
                            SetBoolField(data, "completed", false);
                            break;
                        case 2: // Unlocked
                            SetBoolField(data, "canUnlock", true);
                            SetBoolField(data, "unlocked", true);
                            SetBoolField(data, "completed", false);
                            break;
                        case 3: // Completed
                            SetBoolField(data, "canUnlock", true);
                            SetBoolField(data, "unlocked", true);
                            SetBoolField(data, "completed", true);
                            break;
                    }

                    FlagManager.SetCompletionFlag(doorFlag, data);
                },
                // Options
                () => new List<string> { "Locked", "Can Unlock", "Unlocked", "Completed" }
            );
        }

        private List<CheatPanel> CreateDreamerPanels(FlagDef[][] flagGroups)
        {
            var panels = new List<CheatPanel>();
            foreach (var flagGroup in flagGroups)
            {
                if (flagGroup.Length > 0)
                {
                    // Create a synced reference that handles all flags in the group
                    var syncedReference = new DelegateReference<bool>(
                        () => FlagManager.GetBoolFlag(flagGroup[0]),
                        (newValue) =>
                        {
                            foreach (var flag in flagGroup)
                            {
                                FlagManager.SetBoolFlag(flag, newValue);
                            }
                            
                            // Handle mask and guardian flag increments/decrements
                            if (newValue)
                            {
                                // Toggle ON: increment both flags by 1
                                var currentGuardianValue = FlagManager.GetIntFlag(FlagInstances.guardiansDefeated);
                                FlagManager.SetIntFlag(FlagInstances.guardiansDefeated, currentGuardianValue + 1);
                            }
                            else
                            {
                                // Toggle OFF: decrement both flags by 1
                                var currentGuardianValue = FlagManager.GetIntFlag(FlagInstances.guardiansDefeated);
                                FlagManager.SetIntFlag(FlagInstances.guardiansDefeated, Math.Max(0, currentGuardianValue - 1));
                            }
                        }
                    );
                    
                    // Check if this dreamer has teleport coordinates defined
                    var dreamerName = flagGroup[0].ReadableName;
                    if (teleportLocations.TryGetValue(dreamerName, out var teleportLocation) && teleportLocation != null)
                    {
                        // Create panel with toggle and teleport button
                        panels.Add(new ToggleWithTeleportPanel(
                            syncedReference, 
                            () => TeleportService.DoTeleportWithConfirmation(teleportLocation, dreamerName), 
                            dreamerName));
                    }
                    else
                    {
                        // Create panel with just toggle (no teleport button)
                        var panel = new TogglePanel(syncedReference, dreamerName);
                        panels.Add(panel);
                    }
                }
            }
            return panels;
        }
    }
} 