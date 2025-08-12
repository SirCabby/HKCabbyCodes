using CabbyCodes.Flags;
using CabbyCodes.Patches.BasePatches;
using CabbyMenu.UI.CheatPanels;
using System.Collections.Generic;

namespace CabbyCodes.Patches.Flags
{
    public class NpcFlagPatch : BasePatch
    {
        public override List<CheatPanel> CreatePanels()
        {
            var panels = new List<CheatPanel>();

            panels.Add(new InfoPanel("Bretta").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.brettaRescued,
                FlagInstances.brettaPosition,
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


                FlagInstances.elderbugConvoGrimm
            }));

            panels.Add(new InfoPanel("Gorb").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.ALADAR_encountered
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
            
            panels.Add(new InfoPanel("Nightmare Troupe").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.troupeInTown 
            }));
            
            panels.Add(new InfoPanel("Quirrel").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.metQuirrel,
                FlagInstances.quirrelEggTemple,
                FlagInstances.quirrelLeftEggTemple,
                FlagInstances.quirrelSlugShrine
            }));

            panels.Add(new InfoPanel("Relic Seeker Lemm").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.marmOutside 
            }));
            
            panels.Add(new InfoPanel("Salubra").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.metCharmSlug 
            }));
            
            panels.Add(new InfoPanel("Shaman").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.shaman 
            }));

            panels.Add(new InfoPanel("Seer").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.metMoth
            }));

            panels.Add(new InfoPanel("Sly").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.slyRescued,
                FlagInstances.metSlyShop
            }));

            panels.Add(new InfoPanel("Stag").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.metStag,
                FlagInstances.stagRemember1,
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
                var flagPatch = CreatePatch(flag);
                panels.Add(flagPatch.CreatePanel());
            }
            return panels;
        }
    }
} 