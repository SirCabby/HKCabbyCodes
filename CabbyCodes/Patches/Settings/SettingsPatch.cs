using CabbyMenu.UI.CheatPanels;
using CabbyMenu.UI.DynamicPanels;
using CabbyMenu.SyncedReferences;
using System.Collections.Generic;
using System.Linq;
using CabbyCodes.Patches.SpriteViewer;

namespace CabbyCodes.Patches.Settings
{
    /// <summary>
    /// Main settings patch class that coordinates all settings-related cheat panels.
    /// </summary>
    public class SettingsPatch
    {
        private static MainMenuPanelContainer panelContainer;
        // dynamic list managed each refresh; no static index needed
        private static readonly List<CheatPanel> customSavePanels = new List<CheatPanel>();
        private static CheatPanel savedGamesHeader;
        
        // Track Save Game Analyzer panels for dynamic management
        private static readonly List<CheatPanel> saveGameAnalyzerPanels = new List<CheatPanel>();
        private static bool saveGameAnalyzerPanelsLoaded = false;
        
        // Public static field for dev options state
        public static readonly BoxedReference<bool> DevOptionsEnabled = new BoxedReference<bool>(Constants.DEFAULT_DEV_OPTIONS_ENABLED);

        /// <summary>
        /// Adds all settings-related cheat panels to the mod menu.
        /// </summary>
        public static void AddPanels()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Settings").SetColor(CheatPanel.headerColor));
            QuickStartPatch.AddPanel();
            
            // Add dev options toggle
            AddDevOptionsPanel();
            
            // Create the panel container for managing dynamic panels
            panelContainer = new MainMenuPanelContainer(CabbyCodesPlugin.cabbyMenu);
            
            AddCustomSaveLoadPanels();
            
            // Only show Save Game Analyzer panels when dev options are enabled
            if (DevOptionsEnabled.Get())
            {
                AddSaveGameAnalyzerPanels();
            }
        }

        /// <summary>
        /// Adds the dev options toggle panel.
        /// </summary>
        private static void AddDevOptionsPanel()
        {
            var devOptionsPanel = new TogglePanel(
                new CabbyMenu.SyncedReferences.DelegateReference<bool>(
                    () => DevOptionsEnabled.Get(),
                    (enabled) => {
                        // Store the dev options state in the BoxedReference
                        DevOptionsEnabled.Set(enabled);
                        
                        // Refresh the dev options dependent panels when the dev options setting changes.
                        RefreshDevOptionsDependentPanels();
                    }
                ),
                "Enable Dev Options"
            );
            
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(devOptionsPanel);
        }

        /// <summary>
        /// Refreshes the dev options dependent panels when the dev options setting changes.
        /// </summary>
        private static void RefreshDevOptionsDependentPanels()
        {
            if (CabbyCodesPlugin.cabbyMenu != null)
            {
                bool devOptionsEnabled = DevOptionsEnabled.Get();
                
                // Refresh the sprite viewer category to show/hide based on new setting
                try
                {
                    CabbyCodesPlugin.cabbyMenu.UpdateCategoryRegistration("Sprite Viewer", devOptionsEnabled, SpriteViewerPatch.AddPanels);
                }
                catch
                {
                    // Handle any errors silently
                }
                
                // Refresh the Save Game Analyzer panels
                RefreshSaveGameAnalyzerPanels(devOptionsEnabled);
            }
        }
        
        /// <summary>
        /// Refreshes the Save Game Analyzer panels based on dev options setting.
        /// </summary>
        private static void RefreshSaveGameAnalyzerPanels(bool showPanels)
        {
            if (showPanels)
            {
                // Add the Save Game Analyzer panels if they're not already loaded
                if (!saveGameAnalyzerPanelsLoaded)
                {
                    AddSaveGameAnalyzerPanels();
                }
            }
            else
            {
                // Remove the Save Game Analyzer panels if they're currently loaded
                if (saveGameAnalyzerPanelsLoaded)
                {
                    RemoveSaveGameAnalyzerPanels();
                }
            }
        }

        /// <summary>
        /// Adds the Save Game Analyzer panels and tracks them for dynamic management.
        /// </summary>
        private static void AddSaveGameAnalyzerPanels()
        {
            if (saveGameAnalyzerPanelsLoaded)
            {
                return; // Already loaded
            }
            
            // Clear any existing panels
            saveGameAnalyzerPanels.Clear();
            
            // Store the current panels to know which ones are new
            var currentPanels = new HashSet<CheatPanel>(CabbyCodesPlugin.cabbyMenu.GetAllPanels());
            
            // Add the Save Game Analyzer panels
            SaveGameAnalysisPatch.AddPanels();
            
            // Find the newly added panels by comparing with the current panels
            var newPanels = CabbyCodesPlugin.cabbyMenu.GetAllPanels();
            foreach (var panel in newPanels)
            {
                if (!currentPanels.Contains(panel))
                {
                    saveGameAnalyzerPanels.Add(panel);
                }
            }
            
            saveGameAnalyzerPanelsLoaded = true;
        }
        
        /// <summary>
        /// Removes the Save Game Analyzer panels from the UI.
        /// </summary>
        private static void RemoveSaveGameAnalyzerPanels()
        {
            if (!saveGameAnalyzerPanelsLoaded)
            {
                return; // Not loaded
            }
            
            // Remove all tracked panels
            foreach (var panel in saveGameAnalyzerPanels)
            {
                if (panel != null)
                {
                    CabbyCodesPlugin.cabbyMenu.RemoveCheatPanel(panel);
                }
            }
            
            // Clear tracking
            saveGameAnalyzerPanels.Clear();
            saveGameAnalyzerPanelsLoaded = false;
        }

        /// <summary>
        /// Adds custom save/load panels with dynamic panel management.
        /// </summary>
        private static void AddCustomSaveLoadPanels()
        {
            // Add the header panel
            var headerPanel = new InfoPanel("Save / Load").SetColor(CheatPanel.headerColor);
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(headerPanel);
            
            // Add save panel provided by CustomSaveLoadPatch (encapsulates logic)
            var savePanel = CustomSaveLoadPatch.CreateSaveButtonPanel();
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(savePanel);
            
            // Add custom save name input field
            var customSaveNamePanel = CustomSaveLoadPatch.CreateCustomSaveNamePanel();
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(customSaveNamePanel);
            
            // Add sub-header for saved games
            savedGamesHeader = new InfoPanel("Saved Games").SetColor(CheatPanel.subHeaderColor);
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(savedGamesHeader);

            // Initial population
            RefreshCustomSavePanels();
        }

        /// <summary>
        /// Creates custom save panels at the correct position.
        /// </summary>
        private static void CreateCustomSavePanels()
        {
            // Locate header index
            int headerIndex = panelContainer.GetPanelIndex(savedGamesHeader);
            if (headerIndex < 0)
            {
                return;
            }

            int insertionPoint = headerIndex + 1;

            // Detach all panels after the header so we can rebuild cleanly
            var detachedAll = panelContainer.DetachPanelsAtRange(insertionPoint, panelContainer.GetAllPanels().Count - insertionPoint);

            // Any panels that were part of previous save list?  They are in our tracked list.
            var oldSet = new HashSet<CheatPanel>(customSavePanels);
            List<CheatPanel> detachedRemainder = new List<CheatPanel>();
            foreach (var p in detachedAll)
            {
                if (!oldSet.Contains(p)) detachedRemainder.Add(p);
            }

            // Clear our tracked list; stale panels will be dropped
            customSavePanels.Clear();

            var newPanels = CustomSaveLoadPatch.BuildSavePanels(RefreshCustomSavePanels);
            foreach (var p in newPanels)
            {
                panelContainer.InsertPanel(p, insertionPoint + customSavePanels.Count);
                customSavePanels.Add(p);
            }

            // Reattach non-save panels after the new dynamic block
            panelContainer.ReattachPanelsAtRange(detachedRemainder, insertionPoint + customSavePanels.Count);
        }

        /// <summary>
        /// Refreshes the custom save panels when saves are added or removed.
        /// </summary>
        public static void RefreshCustomSavePanels()
        {
            if (savedGamesHeader != null)
            {
                CreateCustomSavePanels();
            }
        }
    }
} 