using CabbyCodes.Flags;
using CabbyCodes.Flags.FlagData;
using CabbyCodes.Patches.BasePatches;
using CabbyMenu.SyncedReferences;
using CabbyMenu.UI.CheatPanels;
using System;
using System.Collections.Generic;

namespace CabbyCodes.Patches.Flags
{
    public class NpcFlagPatch : BasePatch
    {
        public override List<CheatPanel> CreatePanels()
        {
            var panels = new List<CheatPanel>
            {
                new InfoPanel("Bretta").SetColor(CheatPanel.subHeaderColor)
            };
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.brettaRescued,
                FlagInstances.brettaPosition,
                FlagInstances.brettaSeenBench,
            }));

            panels.Add(new InfoPanel("Brumm").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.metBrum
            }));

            panels.Add(new InfoPanel("Cloth").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.metCloth,
                FlagInstances.savedCloth
            }));

            panels.Add(new InfoPanel("Cornifer").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.metCornifer,
                FlagInstances.corniferIntroduced,
                FlagInstances.corn_crossroadsLeft,
                FlagInstances.corn_greenpathEncountered,
                FlagInstances.corn_greenpathLeft,
                FlagInstances.corn_fungalWastesLeft,
                FlagInstances.corn_cliffsEncountered,
                FlagInstances.corn_cliffsLeft,
                FlagInstances.corn_deepnestEncountered,
                FlagInstances.corn_deepnestMet1,
                FlagInstances.corn_deepnestLeft,
                FlagInstances.corn_abyssEncountered,
                FlagInstances.corn_abyssLeft,
                FlagInstances.corn_cityEncountered,
            }));

            panels.Add(new InfoPanel("Divine").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.divineInTown,
                FlagInstances.metDivine,
            }));

            panels.Add(new InfoPanel("Elderbug").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.metElderbug,
                FlagInstances.elderbugHistory,
                FlagInstances.elderbugHistory1,
                FlagInstances.elderbugHistory2,
                FlagInstances.elderbugHistory3,
                FlagInstances.elderbugSpeechEggTemple,
                FlagInstances.elderbugSpeechMapShop,
                FlagInstances.elderbugSpeechSly,
                FlagInstances.elderbugSpeechStation,
                FlagInstances.elderbugSpeechKingsPass,
                FlagInstances.elderbugSpeechBretta,
                FlagInstances.elderbugSpeechMinesLift,



                FlagInstances.elderbugConvoGrimm
            }));

            panels.Add(new InfoPanel("Gorb").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.ALADAR_encountered
            }));

            panels.Add(new InfoPanel("Grey Mourner").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.metXun,
            }));

            panels.Add(new InfoPanel("Grimm").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.metGrimm
            }));
            
            panels.Add(new InfoPanel("Hornet").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.hornetGreenpath,
                FlagInstances.hornetFung,
                FlagInstances.hornetCityBridge_ready,
                FlagInstances.hornetCityBridge_completed,
                FlagInstances.hornetFountainEncounter,
                FlagInstances.hornetDenEncounter
            }));
            
            panels.Add(new InfoPanel("Hunter").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.metHunter 
            }));
            
            panels.Add(new InfoPanel("Iselda").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.metIselda 
            }));
            
            panels.Add(new InfoPanel("Jiji").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.jijiMet 
            }));

            panels.Add(new InfoPanel("Leg Eater").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.metLegEater,
                FlagInstances.paidLegEater
            }));

            panels.Add(new InfoPanel("Mask Maker").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.maskmakerMet,
                FlagInstances.maskmakerConvo1,
                FlagInstances.maskmakerConvo2,
            }));
            
            panels.Add(new InfoPanel("Midwife").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.midwifeMet,
                FlagInstances.midwifeConvo1
            }));
            
            panels.Add(new InfoPanel("Millibelle the Thief").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.bankerTheftCheck,
                FlagInstances.bankerSpaMet
            }));
            
            panels.Add(new InfoPanel("Moss Prophet").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.mossCultist 
            }));

            panels.Add(new InfoPanel("Myla").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.metMiner, 
                FlagInstances.Crossroads_45__Zombie_Myla 
            }));

            panels.Add(new InfoPanel("Nailmaster Mato").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.metNailmasterMato,
            }));

            panels.Add(new InfoPanel("Nailsmith").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.nailsmithCliff,
                FlagInstances.nailsmithKillSpeech,
                FlagInstances.nailsmithKilled
            }));
            
            panels.Add(new InfoPanel("Nightmare Troupe").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.troupeInTown 
            }));
            
            panels.Add(new InfoPanel("Quirrel").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.metQuirrel,
                FlagInstances.quirrelEggTemple,
                FlagInstances.quirrelLeftEggTemple,
                FlagInstances.quirrelLeftStation,
                FlagInstances.quirrelSlugShrine,
                FlagInstances.quirrelSpaReady,
                FlagInstances.quirrelSpaEncountered,
                FlagInstances.quirrelCityEncountered,
                FlagInstances.quirrelRuins,
                FlagInstances.quirrelCityLeft,
                FlagInstances.quirrelMinesEncountered,
                FlagInstances.quirrelMines,
            }));

            panels.Add(new InfoPanel("Relic Seeker Lemm").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.marmOutside,
                FlagInstances.marmOutsideConvo,
                FlagInstances.metRelicDealerShop,
            }));
            
            panels.Add(new InfoPanel("Salubra").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.metCharmSlug 
            }));
            
            panels.Add(new InfoPanel("Shaman").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.shaman,
                FlagInstances.shamanQuakeConvo,
                FlagInstances.shamanScreamConvo
            }));

            panels.Add(new InfoPanel("Seer").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.metMoth,
                FlagInstances.dreamReward1,
                FlagInstances.dreamReward2,
                FlagInstances.dreamReward3,
                FlagInstances.dreamReward4,
                FlagInstances.dreamReward5,
                FlagInstances.dreamReward5b,
                FlagInstances.dreamReward6,
                FlagInstances.dreamReward7,
                FlagInstances.dreamReward8,
                FlagInstances.dreamReward9,
            }));

            panels.Add(new InfoPanel("Sly").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.slyRescued,
                FlagInstances.metSlyShop,
                FlagInstances.gaveSlykey
            }));

            panels.Add(new InfoPanel("Stag").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.metStag,
                FlagInstances.stagRemember1,
                FlagInstances.stagConvoTram,
            }));

            panels.Add(new InfoPanel("Tiso").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.tisoEncounteredTown
            }));

            panels.Add(new InfoPanel("Warriors' Graves").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.GALIEN_encountered,
                FlagInstances.HU_encountered,
                FlagInstances.NOEYES_encountered,
                FlagInstances.XERO_encountered
            }));

            panels.Add(new InfoPanel("Willoh").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.metGiraffe
            }));
            
            panels.Add(new InfoPanel("Zote").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.zoteRescuedBuzzer,
                FlagInstances.zote,
                FlagInstances.zoteTownConvo,
                FlagInstances.zoteRescuedDeepnest,
            }));
            
            return panels;
        }
        
        private List<CheatPanel> CreateNpcPanels(FlagDef[] flags)
        {
            var panels = new List<CheatPanel>();
            foreach (var flag in flags)
            {
                if (flag == FlagInstances.dreamReward1)
                {
                    panels.Add(InventoryPatch.CreateHallownestSealPanel(flag));
                    continue;
                }
                else if (flag == FlagInstances.dreamReward2)
                {
                    panels.Add(CreateDreamRewardPanel(flag));
                    continue;
                }
                else if (flag == FlagInstances.dreamReward3 || flag == FlagInstances.dreamReward9)
                {
                    panels.Add(InventoryPatch.CreatePaleOrePanel(flag));
                    continue;
                }
                else if (flag == FlagInstances.dreamReward4)
                {
                    panels.Add(CharmPatch.CreateCharmTogglePanel(CharmData.GetCharm(3)));
                    continue;
                }
                else if (flag == FlagInstances.dreamReward5)
                {
                    panels.Add(InventoryPatch.CreateVesselFragmentPanel(flag));
                    continue;
                }
                else if (flag == FlagInstances.dreamReward5b)
                {
                    panels.Add(InventoryPatch.CreateDreamGatePanel());
                    continue;
                }
                else if (flag == FlagInstances.dreamReward6)
                {
                    panels.Add(InventoryPatch.CreateArcaneEggPanel(flag));
                    continue;
                }
                else if (flag == FlagInstances.dreamReward7)
                {
                    panels.Add(InventoryPatch.CreateHeartPiecePanel(flag));
                    continue;
                }
                else if (flag == FlagInstances.dreamReward8)
                {
                    panels.Add(InventoryPatch.CreateDreamNailPanel());
                    continue;
                }
                else if (flag == FlagInstances.gaveSlykey)
                {
                    panels.Add(CreateGaveSlykeyPanel());
                    continue;
                }

                var flagPatch = CreatePatch(flag);
                panels.Add(flagPatch.CreatePanel());
            }
            return panels;
        }

        public static TogglePanel CreateDreamRewardPanel(FlagDef dreamFlag)
        {
            // Find the corresponding reward flags entry
            var rewardEntry = rewardFlags.TryGetValue(dreamFlag, out var additionalAction);

            return new TogglePanel(new DelegateReference<bool>(
                () => FlagManager.GetBoolFlag(dreamFlag),
                value =>
                {
                    FlagManager.SetBoolFlag(dreamFlag, value);
                    
                    additionalAction.Invoke();
                }
            ), dreamFlag.ReadableName);
        }

        public static Dictionary<FlagDef, Action> rewardFlags = new Dictionary<FlagDef, Action>()
        {
            { FlagInstances.dreamReward2, () => {
                FlagManager.SetBoolFlag(FlagInstances.gladeDoorOpened, FlagManager.GetBoolFlag(FlagInstances.dreamReward2));
            }},
            { FlagInstances.dreamReward9, () => {
                FlagManager.SetBoolFlag(FlagInstances.mothDeparted, FlagManager.GetBoolFlag(FlagInstances.dreamReward9));
            }},
        };

        private static CheatPanel CreateGaveSlykeyPanel()
        {
            // Check if both hasSlyKey and gaveSlyKey are false
            bool hasSlyKey = FlagManager.GetBoolFlag(FlagInstances.hasSlykey);
            bool gaveSlyKey = FlagManager.GetBoolFlag(FlagInstances.gaveSlykey);
            bool shouldBeInteractable = hasSlyKey || gaveSlyKey;

            var togglePanel = new TogglePanel(new DelegateReference<bool>(
                () => FlagManager.GetBoolFlag(FlagInstances.gaveSlykey),
                value =>
                {
                    FlagManager.SetBoolFlag(FlagInstances.gaveSlykey, value);
                    FlagManager.SetBoolFlag(FlagInstances.hasSlykey, !value);
                }
            ), FlagInstances.gaveSlykey.ReadableName);

            var toggleButton = togglePanel.GetToggleButton();
            toggleButton.SetInteractable(shouldBeInteractable);
            
            if (!shouldBeInteractable)
            {
                toggleButton.SetDisabledMessage("Giving Sly the storeroom key requires\nhaving the storeroom key.\n\nGet or toggle it to set this flag.");
            }

            return togglePanel;
        }
    }
} 