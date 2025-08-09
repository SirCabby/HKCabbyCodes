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
            // Cornifer section
            var panels = new List<CheatPanel>
            {
                new InfoPanel("Cornifer").SetColor(CheatPanel.subHeaderColor)
            };
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.metCornifer,
                FlagInstances.corniferIntroduced,
                FlagInstances.corn_crossroadsLeft,
                FlagInstances.corn_greenpathEncountered,
                FlagInstances.corn_greenpathLeft
            }));
            
            // Elderbug section
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
                FlagInstances.elderbugSpeechStation
            }));
            
            // Hunter section
            panels.Add(new InfoPanel("Hunter").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.metHunter 
            }));
            
            // Iselda section
            panels.Add(new InfoPanel("Iselda").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.metIselda 
            }));
            
            // Jiji section
            panels.Add(new InfoPanel("Jiji").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.jijiMet 
            }));

            // Myla section
            panels.Add(new InfoPanel("Myla").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.metMiner, 
                FlagInstances.Crossroads_45__Zombie_Myla 
            }));
            
            // Quirrel section
            panels.Add(new InfoPanel("Quirrel").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.metQuirrel,
                FlagInstances.quirrelEggTemple,
                FlagInstances.quirrelLeftEggTemple,
                FlagInstances.quirrelSlugShrine
            }));
            
            // Shaman section
            panels.Add(new InfoPanel("Shaman").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.shaman 
            }));

            // Seer section
            panels.Add(new InfoPanel("Seer").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.metMoth
            }));

            // Sly section
            panels.Add(new InfoPanel("Sly").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.slyRescued,
                FlagInstances.metSlyShop
            }));

            // Stag section
            panels.Add(new InfoPanel("Stag").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.metStag
            }));

            // Warriors' Graves
            panels.Add(new InfoPanel("Warriors' Graves").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.NOEYES_encountered,
                FlagInstances.XERO_encountered
            }));

            // Zote
            panels.Add(new InfoPanel("Zote").SetColor(CheatPanel.subHeaderColor));
            panels.AddRange(CreateNpcPanels(new[] {
                FlagInstances.zote,
                FlagInstances.zoteRescuedBuzzer
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