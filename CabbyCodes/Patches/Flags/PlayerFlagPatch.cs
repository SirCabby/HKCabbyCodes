using CabbyCodes.Flags;
using CabbyMenu.UI.CheatPanels;
using CabbyMenu.UI.DynamicPanels;
using CabbyMenu.UI.Controls;
using CabbyMenu.UI.Popups;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CabbyCodes.Patches.Flags
{
    public class PlayerFlagPatch
    {
        private const int FLAGS_PER_PAGE = 100;
        private static List<FlagDef> allPlayerFlags;
        private static int currentPage = 0;
        private static CheatPanel navigationPanel; // Stored for direct manipulation
        private static List<CheatPanel> currentFlagPanels = new List<CheatPanel>();
        private static PopupBase loadingPopup; // Loading popup shown during navigation

        public static List<CheatPanel> CreatePanels()
        {
            var panels = new List<CheatPanel>();

            // Initialize the list of all player flags if not already done
            if (allPlayerFlags == null)
            {
                allPlayerFlags = GetAllPlayerFlags();
            }

            // Create navigation panel
            navigationPanel = CreateNavigationPanel();
            panels.Add(navigationPanel);

            // Create flag panels for current page
            currentFlagPanels = CreateFlagPanelsForCurrentPage();
            panels.AddRange(currentFlagPanels);

            return panels;
        }

                private static List<FlagDef> GetAllPlayerFlags()
        {
            var flags = new List<FlagDef>();

            // Get all static fields from FlagInstances class using reflection
            var flagInstancesType = typeof(FlagInstances);
            var staticFields = flagInstancesType.GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (var field in staticFields)
            {
                try
                {
                    if (field.FieldType == typeof(FlagDef))
                    {
                        var flagDef = (FlagDef)field.GetValue(null);
                        if (flagDef != null && flagDef.Scene == null)
                        {
                            flags.Add(flagDef);
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    UnityEngine.Debug.LogError($"[PlayerFlagPatch] Error accessing field {field.Name}: {ex.Message}");
                }
            }

            // Add flags from UnusedFlags array
            try
            {
                var unusedFlags = FlagInstances.UnusedFlags;
                
                foreach (var flagDef in unusedFlags)
                {
                    if (flagDef.Scene == null)
                    {
                        flags.Add(flagDef);
                    }
                }
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.LogError($"[PlayerFlagPatch] UnusedFlags access failed: {ex.Message}");
            }

            // Sort by readable name for better organization
            return flags.OrderBy(f => f.ReadableName).ToList();
        }

        private static CheatPanel CreateNavigationPanel()
        {
            var totalFlags = allPlayerFlags.Count;
            var totalPages = (totalFlags + FLAGS_PER_PAGE - 1) / FLAGS_PER_PAGE;
            var startIndex = currentPage * FLAGS_PER_PAGE + 1;
            var endIndex = Mathf.Min((currentPage + 1) * FLAGS_PER_PAGE, totalFlags);

            CabbyCodesPlugin.BLogger.LogInfo(string.Format("[PlayerFlagPatch] CreateNavigationPanel: totalFlags={0}, totalPages={1}, currentPage={2}, startIndex={3}, endIndex={4}", 
                totalFlags, totalPages, currentPage, startIndex, endIndex));

            var navText = $"Player Flags {startIndex} - {endIndex} of {totalFlags}";
            var nav = new InfoPanel(navText).SetColor(CheatPanel.subHeaderColor);

            // Always create both buttons for consistent layout
            CabbyCodesPlugin.BLogger.LogInfo("[PlayerFlagPatch] Adding Previous button");
            var prevButton = PanelAdder.AddButton(nav, 0, () => NavigateToPage(currentPage - 1), "Previous",
                new UnityEngine.Vector2(CabbyMenu.Constants.MIN_PANEL_WIDTH * 1.5f, CabbyMenu.Constants.DEFAULT_PANEL_HEIGHT));
            
            CabbyCodesPlugin.BLogger.LogInfo("[PlayerFlagPatch] Adding Next button");
            var nextButton = PanelAdder.AddButton(nav, 1, () => NavigateToPage(currentPage + 1), "Next",
                new UnityEngine.Vector2(CabbyMenu.Constants.MIN_PANEL_WIDTH * 1.5f, CabbyMenu.Constants.DEFAULT_PANEL_HEIGHT));
            
            // Disable buttons when they shouldn't be used
            if (currentPage <= 0)
            {
                var prevButtonComponent = prevButton.GetComponentInChildren<UnityEngine.UI.Button>();
                if (prevButtonComponent != null)
                {
                    prevButtonComponent.interactable = false;
                }
            }
            if (currentPage >= totalPages - 1)
            {
                var nextButtonComponent = nextButton.GetComponentInChildren<UnityEngine.UI.Button>();
                if (nextButtonComponent != null)
                {
                    nextButtonComponent.interactable = false;
                }
            }
            CabbyCodesPlugin.BLogger.LogInfo("[PlayerFlagPatch] Navigation panel created successfully");
            return nav;
        }



                private static void NavigateToPage(int newPage)
        {
            CabbyCodesPlugin.BLogger.LogInfo(string.Format("[PlayerFlagPatch] NavigateToPage called: currentPage={0}, newPage={1}", currentPage, newPage));
            if (newPage == currentPage)
            {
                CabbyCodesPlugin.BLogger.LogInfo("[PlayerFlagPatch] newPage equals currentPage; aborting navigation");
                return;
            }
            if (newPage < 0) return;
            if (newPage * FLAGS_PER_PAGE >= allPlayerFlags.Count) return;

            // Show loading popup immediately
            loadingPopup = new PopupBase(CabbyCodesPlugin.cabbyMenu, "Loading", "Loading . . .", 400f, 200f);
            // Customize the popup appearance to match DynamicPanelManager styling
            loadingPopup.SetPanelBackgroundColor(new UnityEngine.Color(0.2f, 0.4f, 0.8f, 1f)); // Blue background
            loadingPopup.SetMessageBold(); // Make message text bold
            loadingPopup.Show();
            UnityEngine.Canvas.ForceUpdateCanvases();
            CabbyCodesPlugin.BLogger.LogInfo("[PlayerFlagPatch] Loading popup shown");

            CabbyCodesPlugin.BLogger.LogInfo(string.Format("[PlayerFlagPatch] Total flags={0}; FLAGS_PER_PAGE={1}", allPlayerFlags.Count, FLAGS_PER_PAGE));
 
            // Container proxying to the main menu
            var container = new MainMenuPanelContainer(CabbyCodesPlugin.cabbyMenu);
 
            CabbyCodesPlugin.BLogger.LogInfo("[PlayerFlagPatch] Got container, starting panel removal");

            // Delay panel destruction by one frame to give UI time to render popup
            CabbyMenu.Utilities.CoroutineRunner.RunNextFrame(() => {
                NavigateToPageInternal(newPage, container, loadingPopup);
            });
        }
        
        private static void NavigateToPageInternal(int newPage, MainMenuPanelContainer container, PopupBase popup)
        {
            try
            {
                // Remove current panels
                container.RemovePanel(navigationPanel);
                CabbyCodesPlugin.BLogger.LogInfo("[PlayerFlagPatch] Attempting to remove navigation panel");
                foreach (var panel in currentFlagPanels)
                {
                    container.RemovePanel(panel);
                }
                CabbyCodesPlugin.BLogger.LogInfo(string.Format("[PlayerFlagPatch] Removed {0} flag panels", currentFlagPanels.Count));
                currentFlagPanels.Clear();

                // Update page index
                currentPage = newPage;
                CabbyCodesPlugin.BLogger.LogInfo(string.Format("[PlayerFlagPatch] Updated currentPage to {0}", currentPage));

                // Recreate navigation panel and flag panels
                navigationPanel = CreateNavigationPanel();
                CabbyCodesPlugin.BLogger.LogInfo("[PlayerFlagPatch] Created new navigation panel");
                container.AddPanel(navigationPanel);
                CabbyCodesPlugin.BLogger.LogInfo("[PlayerFlagPatch] Added navigation panel to container");

                currentFlagPanels = CreateFlagPanelsForCurrentPage();
                CabbyCodesPlugin.BLogger.LogInfo(string.Format("[PlayerFlagPatch] Created {0} panels for page {1}", currentFlagPanels.Count, currentPage));
                foreach (var panel in currentFlagPanels)
                {
                    container.AddPanel(panel);
                }
                CabbyCodesPlugin.BLogger.LogInfo("[PlayerFlagPatch] Added all flag panels to container");
                CabbyCodesPlugin.BLogger.LogInfo("[PlayerFlagPatch] NavigateToPage completed successfully");
            }
            finally
            {
                // Hide and destroy loading popup in the next frame
                CabbyMenu.Utilities.CoroutineRunner.RunNextFrame(() => {
                    if (popup != null)
                    {
                        popup.Hide();
                        popup.Destroy();
                        CabbyCodesPlugin.BLogger.LogInfo("[PlayerFlagPatch] Loading popup hidden and destroyed");
                    }
                });
            }
        }
        
        private static List<CheatPanel> CreateFlagPanelsForCurrentPage()
        {
            var panels = new List<CheatPanel>();
            var startIndex = currentPage * FLAGS_PER_PAGE;
            var endIndex = Mathf.Min(startIndex + FLAGS_PER_PAGE, allPlayerFlags.Count);

            for (int i = startIndex; i < endIndex; i++)
            {
                var flagDef = allPlayerFlags[i];
                var flagPanel = CreateFlagPanel(flagDef);
                if (flagPanel != null)
                {
                    panels.Add(flagPanel);
                }
            }

            return panels;
        }

        private static CheatPanel CreateFlagPanel(FlagDef flagDef)
        {
            var displayName = flagDef.ReadableName;
            CheatPanel panel = null;

            switch (flagDef.Type)
            {
                case "PlayerData_Bool":
                    var togglePanel = new TogglePanel(
                        new CabbyMenu.SyncedReferences.DelegateReference<bool>(
                            () => FlagManager.GetBoolFlag(flagDef, flagDef.SemiPersistent),
                            v => FlagManager.SetBoolFlag(flagDef, v, flagDef.SemiPersistent)),
                        displayName);
                    panel = togglePanel;
                    break;

                case "PlayerData_Int":
                    var intPanel = new RangeInputFieldPanel<int>(
                        new CabbyMenu.SyncedReferences.DelegateReference<int>(
                            () => FlagManager.GetIntFlag(flagDef),
                            v => FlagManager.SetIntFlag(flagDef, v)),
                        CabbyMenu.Utilities.KeyCodeMap.ValidChars.Numeric, 0, 999, displayName);
                    panel = intPanel;
                    break;

                case "PlayerData_Float":
                    var floatPanel = new RangeInputFieldPanel<float>(
                        new CabbyMenu.SyncedReferences.DelegateReference<float>(
                            () => FlagManager.GetFloatFlag(flagDef),
                            v => FlagManager.SetFloatFlag(flagDef, v)),
                        CabbyMenu.Utilities.KeyCodeMap.ValidChars.Decimal, 0f, 999f, displayName);
                    panel = floatPanel;
                    break;

                case "PlayerData_String":
                    // For String flags, we'll create a simple info panel since FlagManager doesn't support them yet
                    var stringInfoPanel = new InfoPanel($"{displayName} (String - Not Supported Yet)").SetColor(CheatPanel.warningColor);
                    panel = stringInfoPanel;
                    break;

                case "PlayerData_Vector3":
                    // For Vector3 flags, we'll create a simple info panel since they're complex
                    var vectorInfoPanel = new InfoPanel($"{displayName} (Vector3 - Complex Type)").SetColor(CheatPanel.warningColor);
                    panel = vectorInfoPanel;
                    break;

                case "PlayerData_BossSequenceData":
                    // For BossSequenceData flags, we'll create a simple info panel since they're complex
                    var bossInfoPanel = new InfoPanel($"{displayName} (BossSequenceData - Complex Type)").SetColor(CheatPanel.warningColor);
                    panel = bossInfoPanel;
                    break;

                case "PlayerData_MapZone":
                    // For MapZone flags, we'll create a simple info panel since they're complex
                    var mapZoneInfoPanel = new InfoPanel($"{displayName} (MapZone - Complex Type)").SetColor(CheatPanel.warningColor);
                    panel = mapZoneInfoPanel;
                    break;

                default:
                    // For unknown types, create an info panel
                    var unknownInfoPanel = new InfoPanel($"{displayName} ({flagDef.Type} - Unknown Type)").SetColor(CheatPanel.warningColor);
                    panel = unknownInfoPanel;
                    break;
            }

            return panel;
        }

        /// <summary>
        /// Resets all static state when leaving the Player category.
        /// This ensures a clean state when returning to Player flags.
        /// </summary>
        public static void ResetState()
        {
            // Remove panels from UI if they exist
            if (navigationPanel != null || currentFlagPanels.Count > 0)
            {
                var container = new MainMenuPanelContainer(CabbyCodesPlugin.cabbyMenu);
                
                if (navigationPanel != null)
                {
                    container.RemovePanel(navigationPanel);
                    CabbyCodesPlugin.BLogger.LogInfo("[PlayerFlagPatch] Removed navigation panel from UI");
                }
                
                foreach (var panel in currentFlagPanels)
                {
                    container.RemovePanel(panel);
                }
                if (currentFlagPanels.Count > 0)
                {
                    CabbyCodesPlugin.BLogger.LogInfo(string.Format("[PlayerFlagPatch] Removed {0} flag panels from UI", currentFlagPanels.Count));
                }
            }

            currentPage = 0;
            navigationPanel = null;
            currentFlagPanels.Clear();
            // Destroy loading popup if it exists
            if (loadingPopup != null)
            {
                loadingPopup.Destroy();
                loadingPopup = null;
            }
            CabbyCodesPlugin.BLogger.LogInfo("[PlayerFlagPatch] Navigation state reset - returning to page 1");
        }
    }
} 