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

            var flagTypeSection = new CategorizedPanelSection(
                "Flag Type", 
                new List<string> { "General", "NPC", "Room" },
                CreateFlagTypePanels
            );
            flagTypeSection.AddToMenu(CabbyCodesPlugin.cabbyMenu);

            FlagExtractionPatch.AddPanel();
            FlagMonitorPatch.AddPanel();
        }

        private static List<CheatPanel> CreateFlagTypePanels(int flagTypeIndex)
        {
            var panels = new List<CheatPanel>();
            
            // Clean up all dynamic panel managers when switching categories
            DynamicPanelCoordinator.CleanupAllManagers();
            
            switch (flagTypeIndex)
            {
                case 0: // General
                    panels.AddRange(GeneralFlagPatch.CreatePanels());
                    break;
                    
                case 1: // NPC
                    panels.AddRange(NpcFlagPatch.CreatePanels());
                    break;
                    
                case 2: // Room
                    panels.AddRange(RoomFlagsPatch.CreatePanels());
                    break;
            }
            
            return panels;
        }
    }
}
