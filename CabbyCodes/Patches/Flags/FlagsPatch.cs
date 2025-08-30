using CabbyCodes.Patches.Flags.Triage;
using CabbyCodes.Patches.Flags.RoomFlags;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.UI.DynamicPanels;
using System.Collections.Generic;
using System.Linq;
using BepInEx.Configuration;

namespace CabbyCodes.Patches.Flags
{
    public class FlagsPatch
    {
        // Configuration entry for last selected flag type
        private static ConfigEntry<int> lastSelectedFlagType;
        
        /// <summary>
        /// Initializes the configuration entry.
        /// </summary>
        private static void InitializeConfig()
        {
            if (lastSelectedFlagType == null)
            {
                CabbyCodesPlugin.BLogger.LogInfo("FlagsPatch: Creating new config entry for LastSelectedFlagType");
                lastSelectedFlagType = CabbyCodesPlugin.configFile.Bind("Flags", "LastSelectedFlagType", 0, 
                    "Last selected flag type category (0-9)");
                CabbyCodesPlugin.BLogger.LogInfo(string.Format("FlagsPatch: Config entry created with value: {0}", lastSelectedFlagType.Value));
            }
            else
            {
                CabbyCodesPlugin.BLogger.LogInfo(string.Format("FlagsPatch: Config entry already exists with value: {0}", lastSelectedFlagType.Value));
            }
        }
        
        public static void AddPanels()
        {
            // Initialize configuration
            InitializeConfig();
            
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Game Flags").SetColor(CheatPanel.headerColor));
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Warning: Cannot toggle flag while in the same room").SetColor(CheatPanel.warningColor));

            // Ensure we have a valid selection value (0-9)
            int initialSelection = lastSelectedFlagType.Value;
            if (initialSelection < 0 || initialSelection > 9)
            {
                initialSelection = 0;
                lastSelectedFlagType.Value = 0;
            }
            
            CabbyCodesPlugin.BLogger.LogInfo(string.Format("FlagsPatch: Creating CategorizedPanelSection with initial selection: {0}", initialSelection));
            
            var flagTypeSection = new CategorizedPanelSection(
                "Flag Type", 
                new List<string> { "Environment", "NPC", "Boss", "Progression", "Stag", "Player", "Room", "Geo Rocks", "Whispering Roots", "Flag Monitor" },
                CreateFlagTypePanels,
                1, // insertion index
                initialSelection // Start with the last selected category
            );
            
            // Use AddToMenu to properly set up dynamic panel management
            var panelManager = flagTypeSection.AddToMenu(CabbyCodesPlugin.cabbyMenu);
            
            // We need to add an additional listener to save our config without breaking the existing one
            if (panelManager != null)
            {
                var dropdownPanel = panelManager.GetDropdownPanel();
                if (dropdownPanel != null)
                {
                    // Ensure the dropdown has the correct initial value BEFORE creating any panels
                    var dropDownSync = dropdownPanel.GetDropDownSync();
                    CabbyCodesPlugin.BLogger.LogInfo(string.Format("FlagsPatch: Setting dropdown value to {0}", lastSelectedFlagType.Value));
                    dropDownSync.SelectedValue.Set(lastSelectedFlagType.Value);
                    
                    // Add our config saving listener without removing the existing one
                    var customDropdown = dropDownSync.GetCustomDropdown();
                    customDropdown.onValueChanged.AddListener((categoryIndex) => {
                        CabbyCodesPlugin.BLogger.LogInfo(string.Format("FlagsPatch: Category changed to {0}", categoryIndex));
                        CabbyCodesPlugin.BLogger.LogInfo(string.Format("FlagsPatch: Saving to config: {0}", categoryIndex));
                        lastSelectedFlagType.Value = categoryIndex;
                        CabbyCodesPlugin.BLogger.LogInfo(string.Format("FlagsPatch: Config value after save: {0}", lastSelectedFlagType.Value));
                    });
                    
                    // Now force the panels to be recreated with the correct selection
                    // This will override the default panels that were created
                    CabbyCodesPlugin.BLogger.LogInfo(string.Format("FlagsPatch: Setting initial dropdown value to {0} and recreating panels", lastSelectedFlagType.Value));
                    // Use CreateInitialPanels to avoid the early return check in RecreateDynamicPanels
                    panelManager.CreateInitialPanels(lastSelectedFlagType.Value);
                }
            }
        }

        private static List<CheatPanel> CreateFlagTypePanels(int flagTypeIndex)
        {
            CabbyCodesPlugin.BLogger.LogInfo(string.Format("FlagsPatch: Creating panels for flag type {0}", flagTypeIndex));
            
            var panels = new List<CheatPanel>();
            
            // Clean up all dynamic panel managers when switching categories
            DynamicPanelCoordinator.CleanupAllManagers();
            
            // Only reset Player flags state when switching away from Player category
            // Don't reset when switching TO Player category (flagTypeIndex == 5)
            if (flagTypeIndex != 5)
            {
                PlayerFlagPatch.ResetState();
            }
            
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
                    
                case 4: // Stag
                    var stagPatch = new StagFlagPatch();
                    panels.AddRange(stagPatch.CreatePanels());
                    break;
                    
                case 5: // Player
                    panels.AddRange(PlayerFlagPatch.CreatePanels());
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
            
            CabbyCodesPlugin.BLogger.LogInfo(string.Format("FlagsPatch: Created {0} panels for flag type {1}", panels.Count, flagTypeIndex));
            return panels;
        }
    }
}
