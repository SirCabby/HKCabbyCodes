using CabbyMenu.UI.CheatPanels;
using CabbyMenu.UI.DynamicPanels;
using System.Collections.Generic;

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

        /// <summary>
        /// Adds all settings-related cheat panels to the mod menu.
        /// </summary>
        public static void AddPanels()
        {
            CabbyCodesPlugin.cabbyMenu.AddCheatPanel(new InfoPanel("Settings").SetColor(CheatPanel.headerColor));
            QuickStartPatch.AddPanel();
            
            // Create the panel container for managing dynamic panels
            panelContainer = new MainMenuPanelContainer(CabbyCodesPlugin.cabbyMenu);
            
            AddCustomSaveLoadPanels();
            SaveGameAnalysisPatch.AddPanels();
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