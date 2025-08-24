using CabbyCodes.Patches.Flags.Triage;
using CabbyCodes.Patches.Flags.RoomFlags;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.UI.DynamicPanels;
using System.Collections.Generic;

namespace CabbyCodes.Patches.Flags
{
    public class FlagsPatch
    {
        public static void AddPanels()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Game Flags").SetColor(CheatPanel.headerColor));
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Warning: Cannot toggle flag while in the same room").SetColor(CheatPanel.warningColor));

            var flagTypeSection = new CategorizedPanelSection(
                "Flag Type", 
                new List<string> { "Environment", "NPC", "Boss", "Progression", "Shop", "Stag", "Room", "Geo Rocks", "Whispering Roots", "Flag Monitor" },
                CreateFlagTypePanels
            );
            flagTypeSection.AddToMenu(CabbyCodesPlugin.cabbyMenu);
        }

        private static List<CheatPanel> CreateFlagTypePanels(int flagTypeIndex)
        {
            var panels = new List<CheatPanel>();
            
            // Clean up all dynamic panel managers when switching categories
            DynamicPanelCoordinator.CleanupAllManagers();
            
            switch (flagTypeIndex)
            {
                case 0: // Environment
                    var envPatch = new EnvironmentFlagPatch();
                    panels.AddRange(envPatch.CreatePanels());
                    break;
                    
                case 1: // NPC
                    var npcPatch = new NpcFlagPatch();
                    panels.AddRange(npcPatch.CreatePanels());
                    break;

                case 2: // Boss
                    var bossPatch = new BossFlagPatch();
                    panels.AddRange(bossPatch.CreatePanels());
                    break;
                
                case 3: // Progression
                    var progressionPatch = new ProgressionPatch();
                    panels.AddRange(progressionPatch.CreatePanels());
                    break;
                
                case 4: // Shop
                    var shopPatch = new ShopFlagPatch();
                    panels.AddRange(shopPatch.CreatePanels());
                    break;
                    
                case 5: // Stag
                    var stagPatch = new StagFlagPatch();
                    panels.AddRange(stagPatch.CreatePanels());
                    break;
                    
                case 6: // Room
                    panels.AddRange(RoomFlagsPatch.CreatePanels());
                    break;
                    
                case 7: // Geo Rocks
                    var geoPatch = new GeoRocksFlagPatch();
                    panels.AddRange(geoPatch.CreatePanels());
                    break;
                    
                case 8: // Whispering Roots
                    var whisperingPatch = new WhisperingRootsPatch();
                    panels.AddRange(whisperingPatch.CreatePanels());
                    break;

                case 9: // Flag Monitor
                    panels.AddRange(FlagMonitorPatch.CreatePanels());
                    panels.AddRange(FlagExtractionPatch.CreatePanels());
                    break;
            }
            
            return panels;
        }
    }
}
